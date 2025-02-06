using System;
using System.Web;
using System.Web.Caching;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Concurrent;
using System.Web.Hosting;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Wrapper for the HttpRuntime Cache.
/// </summary>
public static class CachedDataManager
{
    // Locks used to synchronize cache insertions by key.
    private static readonly ConcurrentDictionary<string, object> cacheItemLocks = new ConcurrentDictionary<string, object>(StringComparer.Ordinal);

    /// <summary>
    /// Cache timeout in seconds.
    /// The default (and minimum) TTL is 60 seconds if one is not set in the configuration.
    /// Example in web.config:
    ///   <add key="cacheTimeout" value="43200"/><!-- Server cache time 12 hours -->
    /// </summary>
    private static readonly Lazy<int> cacheTimeout = new Lazy<int>(() =>
    {
        return int.TryParse(ConfigurationManager.AppSettings["cacheTimeout"], out int timeout)
            ? Math.Max(timeout, 60)
            : 60;
    });

    /// <summary>
    /// Gets a value stored in the current HttpContext.Items collection.
    /// If the value does not exist and a function is provided, it will be executed and stored in the Items collection.
    /// </summary>
    /// <typeparam name="T">The type of the item.</typeparam>
    /// <param name="contextKey">The key used to reference the item in HttpContext.Items.</param>
    /// <param name="getValue">Optional. A function to retrieve the value if it is not already present.</param>
    /// <returns>The item of type T, or the default value if none exists and no getValue function is provided.</returns>
    public static T GetContextItem<T>(string contextKey, Func<T> getValue = null)
    {
        if (HttpContext.Current.Items[contextKey] is Tuple<T> tup)
        {
            return tup.Item1;
        }

        if (getValue == null)
        {
            return default;
        }

        tup = Tuple.Create(getValue());
        HttpContext.Current.Items[contextKey] = tup;
        return tup.Item1;
    }

    /// <summary>
    /// Retrieves an item from the HttpRuntime.Cache with the specified key. If the item does not exist, it calls the provided function
    /// to obtain the value, caches it, and then returns it.
    /// </summary>
    /// <typeparam name="T">The type of the item to cache.</typeparam>
    /// <param name="cacheKey">The key used to reference the cached value.</param>
    /// <param name="getValue">A function to retrieve the value if it is not in the cache.</param>
    /// <param name="timeoutLength">
    /// Optional. The time-to-live for the cached item. If not provided, the configured cacheTimeout (with a slight random jitter) is used.
    /// </param>
    /// <param name="dontCacheEmpty">
    /// Optional. If set to true, and the result of getValue is empty or the default value, the value is not cached. Default is false.
    /// </param>
    /// <param name="dependsOnKeys">
    /// Optional. An array of keys that the cached item depends on. If any of these change, the cached item becomes invalid.
    /// </param>
    /// <returns>The cached item of type T, or the default value if not found and no getValue function is provided.</returns>
    public static T GetCachedItem<T>(string cacheKey, Func<T> getValue = null, TimeSpan? timeoutLength = null, bool dontCacheEmpty = false, params string[] dependsOnKeys)
    {
        // Try to get from cache.
        if (HttpRuntime.Cache[cacheKey] is Tuple<T> tup)
        {
            return tup.Item1;
        }

        if (getValue == null)
        {
            return default;
        }

        // Variable to hold the value to be returned.
        T result = default;

        // Synchronize cache population for this key.
        lock (cacheItemLocks.GetOrAdd(cacheKey, _ => new object()))
        {
            // Double-check the cache in case it was added while waiting for the lock.
            if (HttpRuntime.Cache[cacheKey] is Tuple<T> cachedTup)
            {
                return cachedTup.Item1;
            }

            // Retrieve fresh value.
            result = getValue();

            // If dontCacheEmpty is true and the value is considered empty, return it without caching.
            if (dontCacheEmpty && !HasValues(result))
            {
                return result;
            }

            // If a valid value is returned, cache it.
            if (result != null)
            {
                tup = Tuple.Create(result);
                var dependency = dependsOnKeys.Length > 0 ? new CacheDependency(null, dependsOnKeys) : null;
                DateTime expiration = timeoutLength.HasValue
                    ? DateTime.Now.Add(timeoutLength.Value)
                    : DateTime.Now.AddSeconds(cacheTimeout.Value + new Random().Next(-30, 31));

                // If dependencies are provided, let the dependency drive expiration.
                expiration = dependency == null ? expiration : Cache.NoAbsoluteExpiration;

                HttpRuntime.Cache.Insert(
                    cacheKey,
                    tup,
                    dependency,
                    expiration,
                    Cache.NoSlidingExpiration,
                    CacheItemPriority.NotRemovable,
                    null);
            }
        }

        // Clean up the lock.
        cacheItemLocks.TryRemove(cacheKey, out _);

        return result;
    }

    /// <summary>
    /// Retrieves an XML string from the database by executing a stored procedure, caching the result.
    /// </summary>
    /// <param name="connectionStringName">The name of the connection string in the configuration.</param>
    /// <param name="sprocName">The name of the stored procedure to execute.</param>
    /// <returns>An XML string returned from the stored procedure, or null if no XML is returned.</returns>
    public static string GetDatabaseXmlString(string connectionStringName, string sprocName)
    {
        string cacheKey = $"{connectionStringName}_{sprocName}";
        return GetCachedItem(cacheKey, () =>
        {
            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString))
            {
                cn.Open();
                using (var cmd = new SqlCommand(sprocName, cn) { CommandType = CommandType.StoredProcedure })
                {
                    using (var rdr = cmd.ExecuteXmlReader())
                    {
                        return rdr.Read() ? rdr.ReadOuterXml() : null;
                    }
                }
            }
        });
    }

    /// <summary>
    /// Clears all entries from the HttpRuntime.Cache.
    /// </summary>
    public static void ClearCache()
    {
        foreach (DictionaryEntry entry in HttpRuntime.Cache)
        {
            HttpRuntime.Cache.Remove((string)entry.Key);
        }
    }

    /// <summary>
    /// Removes the cached item with the specified key from the HttpRuntime.Cache.
    /// </summary>
    /// <param name="cacheKey">The key of the cached item to remove.</param>
    public static void RemoveCachedItem(string cacheKey)
    {
        if (HttpRuntime.Cache[cacheKey] != null)
        {
            HttpRuntime.Cache.Remove(cacheKey);
        }
    }

    /// <summary>
    /// Gets a versioned URL for a virtual path by appending a query string parameter based on the file hash.
    /// </summary>
    /// <param name="path">The virtual path to version.</param>
    /// <returns>The versioned URL as a string.</returns>
    public static string HrefVersioned(string path)
    {
        string virtualPath = HttpRuntime.AppDomainAppVirtualPath;
        if (!VirtualPathUtility.IsAppRelative(path))
        {
            path = VirtualPathUtility.Combine(virtualPath, path);
        }

        if (HttpRuntime.Cache[path] == null)
        {
            HttpRuntime.Cache.Insert(
                path,
                HostingEnvironment.VirtualPathProvider.GetFileHash(path, new[] { path }),
                HostingEnvironment.VirtualPathProvider.GetCacheDependency(path, new[] { path }, DateTime.Now.ToUniversalTime()));
        }

        return $"{VirtualPathUtility.ToAbsolute(path)}?v{HttpRuntime.Cache[path]}";
    }

    /// <summary>
    /// Executes the provided function on every call to get fresh data.
    /// Under normal operation, the function is executed, its result is cached (or the cache is updated),
    /// and then the fresh data is returned.
    /// If the function throws an exception, the method attempts to return a previously cached value as a fallback.
    /// The cache duration is specified in days.
    /// </summary>
    /// <typeparam name="T">The type of the item to cache.</typeparam>
    /// <param name="cacheKey">The key used to reference the cached value.</param>
    /// <param name="getValue">A function to retrieve the value for fresh data.</param>
    /// <param name="cacheDays">The number of days to cache the value.</param>
    /// <param name="dontCacheEmpty">
    /// Optional. If set to true, and the result of getValue is empty or the default value, the value is not cached.
    /// Default is false.
    /// </param>
    /// <param name="dependsOnKeys">
    /// Optional. An array of keys that the cached item depends on. Changes to any of these keys will invalidate the cached item.
    /// </param>
    /// <returns>The fresh data of type T if retrieval is successful, or the cached data as a fallback if retrieval fails.</returns>
    public static T GetCachedItemAsFallback<T>(string cacheKey, Func<T> getValue, int cacheDays, bool dontCacheEmpty = false, params string[] dependsOnKeys)
    {
        // Try to get fresh data.
        T result;
        try
        {
            result = getValue();

            // If dontCacheEmpty is true and the result is empty, simply return the result without caching.
            if (dontCacheEmpty && !HasValues(result))
            {
                return result;
            }

            // If a valid result was returned, cache (or update) it.
            if (result != null)
            {
                // Lock to synchronize cache updates.
                lock (cacheItemLocks.GetOrAdd(cacheKey, _ => new object()))
                {
                    var tup = Tuple.Create(result);
                    var dependency = dependsOnKeys.Length > 0 ? new CacheDependency(null, dependsOnKeys) : null;
                    // Use the specified cacheDays to calculate the expiration.
                    DateTime expiration = DateTime.Now.AddDays(cacheDays);
                    // If dependencies are provided, let the dependency drive expiration.
                    if (dependency != null)
                    {
                        expiration = Cache.NoAbsoluteExpiration;
                    }
                    HttpRuntime.Cache.Insert(
                        cacheKey,
                        tup,
                        dependency,
                        expiration,
                        Cache.NoSlidingExpiration,
                        CacheItemPriority.NotRemovable,
                        null);
                }
                // Remove the lock object for this key.
                cacheItemLocks.TryRemove(cacheKey, out _);
            }

            return result;
        }
        catch (Exception)
        {
            // If fresh retrieval fails, try to return a cached fallback value.
            if (HttpRuntime.Cache[cacheKey] is Tuple<T> cached)
            {
                return cached.Item1;
            }
            // Otherwise, rethrow the exception.
            throw;
        }
    }

    /// <summary>
    /// Determines whether the specified value is considered "non-empty."
    /// It supports checking collections, strings, nullable types, and arrays.
    /// </summary>
    /// <typeparam name="T">The type of the value to check.</typeparam>
    /// <param name="val">The value to check.</param>
    /// <returns>True if the value is non-empty; otherwise, false.</returns>
    private static bool HasValues<T>(T val)
    {
        if (val == null)
        {
            return false;
        }

        Type type = typeof(T);

        // Check if the type is a collection (but not a string)
        if (typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string))
        {
            if (((IEnumerable)val).Cast<object>().Any() == false)
            {
                return false;
            }
        }
        // Check if the type is a string
        else if (type == typeof(string))
        {
            if (string.IsNullOrWhiteSpace(val as string))
            {
                return false;
            }
        }
        // Check if the type is a nullable value type
        else if (Nullable.GetUnderlyingType(type) != null)
        {
            if (val.Equals(default(T)))
            {
                return false;
            }
        }
        // Check if the type is an array
        else if (type.IsArray)
        {
            if (val is Array array && array.Length == 0)
            {
                return false;
            }
        }
        // Check if the value is the default value for the type
        else if (val.Equals(default(T)))
        {
            return false;
        }

        return true;
    }
}
