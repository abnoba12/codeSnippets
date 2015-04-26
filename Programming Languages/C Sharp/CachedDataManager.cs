using System;
using System.Web;
using System.Web.Caching;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Concurrent;

/*Example of use:
XDocument doc = CachedDataManager.GetCachedItem("homepage_carousel_rss", () =>
{
	return XDocument.Load("http://cms.jhilburn.com/?page_id=107&tag=feature");
});
*/
public static class CachedDataManager
{
	private static readonly ConcurrentDictionary<string, object> cacheItemLocks = new ConcurrentDictionary<string, object>(StringComparer.Ordinal);
	private static readonly Lazy<int> cacheTimeout = new Lazy<int>(() => {
		int timeout = int.TryParse(ConfigurationManager.AppSettings["cacheTimeout"], out timeout) ? timeout : 0;
		return Math.Max(timeout, 60);
	});

	public static T GetContextItem<T>(string contextKey, Func<T> getValue = null)
	{
		var tup = HttpContext.Current.Items[contextKey] as Tuple<T>;
		if (tup != null)
			return tup.Item1;

		if (getValue == null)
			return default(T);

		tup = Tuple.Create(getValue());
		HttpContext.Current.Items[contextKey] = tup;

		return tup.Item1;
	}

	public static T GetCachedItem<T>(string cacheKey, Func<T> getValue = null, params string[] dependsOnKeys)
	{
		var tup = HttpRuntime.Cache[cacheKey] as Tuple<T>;
		if (tup != null)
			return tup.Item1;

		if (getValue == null)
			return default(T);

		lock (cacheItemLocks.GetOrAdd(cacheKey, _ => new object()))
		{
			tup = HttpRuntime.Cache[cacheKey] as Tuple<T>;
			if (tup == null)
			{
				tup = Tuple.Create(getValue());
				var dependency = dependsOnKeys.Length > 0 ? new CacheDependency(null, dependsOnKeys) : null;
				var timeout = dependency == null ? DateTime.Now.AddSeconds(cacheTimeout.Value + (new Random()).Next(-30, 31)) : Cache.NoAbsoluteExpiration;
				HttpRuntime.Cache.Insert(cacheKey, tup, dependency, timeout, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
			}
		}
		object o = cacheItemLocks.TryRemove(cacheKey, out o) ? o : null;

		return tup.Item1;
	}

	public static string GetDatabaseXmlString(string connectionStringName, string sprocName)
	{
		return GetCachedItem(string.Join("_", connectionStringName, sprocName), () => {
			using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString))
			{
				cn.Open();
				var cmd = new SqlCommand(sprocName, cn) { CommandType = CommandType.StoredProcedure };
				var rdr = cmd.ExecuteXmlReader();
				return rdr.Read() ? rdr.ReadOuterXml() : null;
			}
		});
	}
}
