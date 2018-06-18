// based on accepted SO answer here https://stackoverflow.com/questions/17104265/caching-a-jquery-ajax-response-in-javascript-browser

function AjaxCacheConstructor() {

    var self = this;

    self.defaultTtl = AppSettings_AjaxCacheDefaultTtlSeconds * 1000;
    self.storage = AppSettings_AjaxCachePersistent ? localStorage : sessionStorage;

    self.Clear = function () {
        var keys = Object.keys(self.storage)
            .filter(function (key) { return key.startsWith('AjaxCache_'); })
            .forEach(function (key) { self.storage.removeItem(key); });
    };
    self.Remove = function (id) {
        self.storage.removeItem('AjaxCache_data_' + id);
        self.storage.removeItem('AjaxCache_meta_' + id);
    };
    self.IsValid = function (entry) {
        return entry && (entry.expirationTime > new Date().getTime());
    };
    self.Get = function (id) {
        var dataPart = self.storage.getItem('AjaxCache_data_' + id);
        var metaPart = self.storage.getItem('AjaxCache_meta_' + id);
        return dataPart ? Object.assign(JSON.parse(dataPart), JSON.parse(metaPart)) : undefined;
    };
    self.Set = function (id, data, ttlSeconds, isRetry) {

        self.Remove(id);

        try {
            var dataPart = {
                id: id,
                data: data
            };
            self.storage.setItem('AjaxCache_data_' + id, JSON.stringify(dataPart));

            var now = new Date();
            var ttl = ttlSeconds ? ttlSeconds * 1000 : self.defaultTtl;
            var metaPart = {
                id: id,
                creationDate: now,
                ttl: ttl,
                expirationTime: now.getTime() + ttl
            };
            self.storage.setItem('AjaxCache_meta_' + id, JSON.stringify(metaPart));
        } catch (e) {
            if (self.IsQuataExceededError(e) && !isRetry) {
                // if we encounter a "quota exceeded" error,
                // and this is the 1st attempt (!isRetry),
                // clear the cache and try the Set operation again
                self.Clear();
                self.Set(id, data, ttlSeconds, true);
            } else {
                // otherwise, rethrow the exception
                throw e;
            }
        }
    };
    self.GetId = function (url, data) {
        return url + (data && Object.keys(data).length ? JSON.stringify(data) : '');
    };

    self.Prune = function () {
        // prune
        var prunedCount = 0;
        Object.keys(self.storage)
            .filter(function (key) { return key.startsWith('AjaxCache_meta_'); })
            .forEach(function (key) {
                var metaPart = JSON.parse(self.storage.getItem(key));
                if (!self.IsValid(metaPart)) {
                    self.Remove(metaPart.id);
                    prunedCount++;
                }
            });
        GlobalHandler('Pruned ' + prunedCount + ' cache entries', 'debug');

        // schedule next pruning
        setTimeout(function () { self.Prune(); }, AppSettings_AjaxCachePruningIntervalSeconds * 1000);
    };

    // https://developer.mozilla.org/en-US/docs/Web/API/Web_Storage_API/Using_the_Web_Storage_API
    self.StorageSupported = function () {
        try {
            var x = '__storage_test__';
            self.storage.setItem(x, x);
            self.storage.removeItem(x);
            return true;
        }
        catch (e) {
            return self.IsQuataExceededError(e);
        }
    }

    self.IsQuataExceededError = function (e) {
        return e instanceof DOMException && (
            // everything except Firefox
            e.code === 22 ||
            // Firefox
            e.code === 1014 ||
            // test name field too, because code might not be present
            // everything except Firefox
            e.name === 'QuotaExceededError' ||
            // Firefox
            e.name === 'NS_ERROR_DOM_QUOTA_REACHED') &&
            // acknowledge QuotaExceededError only if there's something already stored
            self.storage.length !== 0;
    };
}



// build AjaxCache object
var AjaxCache = new AjaxCacheConstructor();

// check for browser support
if (AjaxCache.StorageSupported() != true) {
    GlobalHandler('Browser storage not supported - AJAX caching will be disabled!', 'warn');
    AppSettings_AjaxCache = false;
}

// kick off AjaxCache scheduled pruning
AjaxCache.Prune();



AjaxCacheForKendoDataSource  = function (e, ttlSeconds) {
    e.sender.options.transport.read.cache = true;
    e.sender.options.transport.read.cacheTtlSeconds = ttlSeconds;
}

$.ajaxPrefilter(function (options, originalOptions, jqXHR) {
    if (AppSettings_AjaxCache === true && originalOptions.cache === true) {
        var id = AjaxCache.GetId(originalOptions.url, originalOptions.data);
        var entry = AjaxCache.Get(id);
        if (!AjaxCache.IsValid(entry)) {
            jqXHR.promise().done(function (data, textStatus) {
                GlobalHandler('Caching result for ' + id, 'debug');
                AjaxCache.Set(id, data, originalOptions.cacheTtlSeconds);
            });
        }
    }
});

$.ajaxTransport("+*", function (options, originalOptions, jqXHR, headers, completeCallback) {
    if (AppSettings_AjaxCache === true && originalOptions.cache === true) {
        var id = AjaxCache.GetId(originalOptions.url, originalOptions.data);
        var entry = AjaxCache.Get(id);
        if (AjaxCache.IsValid(entry)) {
            GlobalHandler('Returning cached result for ' + id + ' created on ' + entry.creationDate, 'debug');
            return {
                send: function (headers, completeCallback) {
                    completeCallback(200, "OK", { cache: entry.data });
                },
                abort: function () {
                    /* abort code, nothing needed here I guess... */
                }
            };
        }
    }
});