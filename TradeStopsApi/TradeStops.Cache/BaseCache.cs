using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TradeStops.Common.Utils;

namespace TradeStops.Cache
{
    // todo now: extract static implementations
    public class BaseCache
    {
        private readonly ICache _cache;
        private readonly int _tempItemsExpirationInSeconds;

        public BaseCache(ICache cache, int tempItemsExpirationInSeconds)
        {
            _cache = cache;
            _tempItemsExpirationInSeconds = tempItemsExpirationInSeconds;
        }

        public void Set<T>(T value, string methodName, string cacheKeySuffix, int expirationTime, [CallerFilePath]string filePath = "")
        {
            var className = Path.GetFileNameWithoutExtension(filePath);
            var cacheKey = GetCacheKeyInternal(className, methodName, cacheKeySuffix);

            _cache.Set(cacheKey, value, expirationTime);
        }

        /// <summary>
        /// Load multiple items from cache/database in optimal way
        /// </summary>
        /// <typeparam name="TIdentifier">
        /// Type of identifiers that will be used to get data from dataAccessMethod.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// Type of results that will be returned from method
        /// (for example, it can be SymbolItem, so List of SymbolItems will be returned)
        /// </typeparam>
        /// <param name="dataAccessMethod">
        /// Method to get data from repository (database).
        /// This method must accept the list of identifiers of generic type TIdentifier.
        /// If repository method has some static parameters, you can wrap it like (ids) => _repository.Method(ids, staticParameter).
        /// If data access method by single ID returns List instead of single item, then check usages of ListWithId class as example.
        /// </param>
        /// <param name="identifiers">
        /// List of identifiers to get data from repository or from cache.
        /// Each identifier will be used to create unique cache key, so 'Distinct()' method will be used for identifiers
        /// </param>
        /// <param name="identifierSelector">
        /// Selector that will be used on database items to understand which records were missing in cache
        /// </param>
        /// <param name="methodName">
        /// The name of the method that is used to get TResult item by single TIdentifier.
        /// Usually such method uses BaseCache.Get method inside.
        /// If there are no such method to get single item then it's okay to use the name of caller method.
        /// </param>
        /// <param name="cacheKeySuffixGenerator">
        /// Method to generate cacheKeySuffix by TIdentifier, like (id) => $"{tradeDate.ToShortString()}_{id}"
        /// Note: cacheKeySuffix generator must generate unique cacheKeys for unique TIdentifiers
        /// </param>
        /// <param name="expirationTime">Cache expiration time</param>
        /// <param name="databaseIdentifierMatcher">
        /// (optional) Method to find corresponding database identifier by input identifier.
        /// Usually database item has the same id as was requested: if we request symbol by SymbolId = 666 then database item will also have this SymbolId = 666.
        /// But for Latest Prices we can request price with TradeDate = Today and database result may be with TradeDate = 2DaysAgo,
        /// so we have to match requested identifiers with returned database items to cache it.
        /// This method parameters contains the list with database identifiers and identifier requested by user,
        /// and must return corresponding database identifier if it's found, or requested identifier, if nothing found.
        /// </param>
        /// <param name="filePath">[caller file path] set automatically at compile-time</param>
        /// <returns>
        /// Items that were found in cache or in repository.
        /// </returns>
        public virtual List<TResult> GetMultiple<TIdentifier, TResult>(
            Func<List<TIdentifier>, IEnumerable<TResult>> dataAccessMethod,
            IEnumerable<TIdentifier> identifiers,
            Func<TResult, TIdentifier> identifierSelector,
            string methodName,
            Func<TIdentifier, string> cacheKeySuffixGenerator,
            int expirationTime,
            Func<IEnumerable<TIdentifier>, TIdentifier, TIdentifier> databaseIdentifierMatcher = null,
            [CallerFilePath]string filePath = "")
        {
            Debug.WriteLine($"*** BaseCache.GetMultiple for {methodName} has been called");

            var className = Path.GetFileNameWithoutExtension(filePath);

            // Note: this code won't work if non-unique cacheKeys will be generated for different identifiers,
            // like LatestPrice_1991-10-10 for TradeDates with same Date but different Time.
            // Check file version history for 2019-04-16 for possible fix
            var keysDictionary = new Dictionary<string, TIdentifier>();
            foreach (var identifier in identifiers.Distinct())
            {
                var cacheKey = GetCacheKeyInternal(className, methodName, cacheKeySuffixGenerator(identifier));
                keysDictionary[cacheKey] = identifier;
            }

            var cachedGenericItems = _cache.GetMultipleGeneric<TResult>(keysDictionary.Keys);

            var cachedItems = cachedGenericItems
                .Where(x => !ObjectUtils.IsEqualsDefaultValue(x.CacheObject))
                .Select(x => x.CacheObject)
                ////.Distinct()
                .ToList();

            var existingCacheKeys = cachedGenericItems.Select(x => x.CacheKey);
            var missingCacheKeys = keysDictionary.Keys.Except(existingCacheKeys).ToList();

            if (!missingCacheKeys.Any())
            {
                return cachedItems;
            }

            var itemsNotFoundInCacheIds = missingCacheKeys.Select(x => keysDictionary[x]).ToList();

            var databaseItems = dataAccessMethod.Invoke(itemsNotFoundInCacheIds).ToList();

            // var databaseItemsDictionary = databaseItems.ToDictionary(identifierSelector);
            // the commented code above will fail if there are not unique TIdentifiers for multiple databaseItems.
            // Possible fix: use .Distinct() on databaseItems list
            var databaseItemsDictionary = new Dictionary<TIdentifier, TResult>();
            foreach (var databaseItem in databaseItems)
            {
                var identifier = identifierSelector(databaseItem);
                databaseItemsDictionary[identifier] = databaseItem;
            }

            var existingDatabaseItemsKeyValuePairs = new List<KeyValuePair<string, TResult>>();
            var missingDatabaseItemsKeyValuePairs = new List<KeyValuePair<string, TResult>>();
            foreach (var cacheKey in missingCacheKeys)
            {
                var missingIdentifier = keysDictionary[cacheKey];

                var databaseItemIdentifier = databaseIdentifierMatcher == null
                    ? missingIdentifier
                    : databaseIdentifierMatcher(databaseItemsDictionary.Keys, missingIdentifier);

                var isItemFoundInDatabase = databaseItemsDictionary.ContainsKey(databaseItemIdentifier);

                if (isItemFoundInDatabase)
                {
                    var databaseItem = databaseItemsDictionary[databaseItemIdentifier];
                    var keyValuePair = new KeyValuePair<string, TResult>(cacheKey, databaseItem);
                    existingDatabaseItemsKeyValuePairs.Add(keyValuePair);
                }
                else
                {
                    var keyValuePair = new KeyValuePair<string, TResult>(cacheKey, default(TResult));
                    missingDatabaseItemsKeyValuePairs.Add(keyValuePair);
                }
            }

            _cache.SetMultiple(existingDatabaseItemsKeyValuePairs, expirationTime);
            _cache.SetMultiple(missingDatabaseItemsKeyValuePairs, _tempItemsExpirationInSeconds);

            return cachedItems.Concat(databaseItems).ToList(); /*.Distinct()*/
        }

        /// <summary>
        /// Get item from cache. Default value (null/0) also will be cached.
        /// If you don't need to cache default values, pass _tempItemsExpirationInSeconds as 0.
        /// The cache won't cache item if expirationTime less or equals to 0.
        /// </summary>
        /// <param name="dataAccessMethod">Method that will return necessary data if the cache is empty</param>
        /// <param name="cacheKeySuffix">
        /// Changing parameter for cache key, examples: userId.ToString(), $"{userId}_{viewId}, string.Empty.
        /// Cache key is generated this way: ClassName + MethodName + CacheKeySuffix.
        /// </param>
        /// <param name="expirationTimeMethod">Method that returns cache expiration time in seconds. Example: UserContextItem has ExpirationDate property. No sense to cache item for more that the value of expiration.</param>
        /// <param name="methodName">[caller member name] set automatically at compile-time. Used to generate cache key.</param>
        /// <param name="filePath">[caller file path] set automatically at compile-time. Used to generate cache key.</param>
        /// <typeparam name="T">Generic type parameter</typeparam>
        /// <returns>item from cache or default value for specified type (0, null)</returns>
        public virtual T Get<T>(Func<T> dataAccessMethod, string cacheKeySuffix, Func<T, int> expirationTimeMethod, [CallerMemberName] string methodName = "", [CallerFilePath] string filePath = "")
        {
            return GetInternal(dataAccessMethod, cacheKeySuffix, expirationTimeMethod, methodName, filePath);
        }

        /// <summary>
        /// Get item from cache. Default value (null/0) also will be cached.
        /// The cache won't cache item if expirationTime less or equals to 0.
        /// </summary>
        /// <param name="dataAccessMethod">Method that will return necessary data if the cache is empty</param>
        /// <param name="cacheKeySuffix">Changing parameter for cache key, examples: userId.ToString(), $"{userId}_{viewId}, string.Empty</param>
        /// <param name="expirationTime">Cache expiration time in seconds. Default _tempItemsExpirationInSeconds value will be used for not found items</param>
        /// <param name="methodName">[caller member name] set automatically at compile-time</param>
        /// <param name="filePath">[caller file path] set automatically at compile-time</param>
        /// <typeparam name="T">Generic type parameter</typeparam>
        /// <returns>item from cache or default value for specified type (0, null)</returns>
        public virtual T Get<T>(Func<T> dataAccessMethod, string cacheKeySuffix, int expirationTime, [CallerMemberName] string methodName = "", [CallerFilePath] string filePath = "")
        {
            return GetInternal(dataAccessMethod, cacheKeySuffix, item => expirationTime, methodName, filePath);
        }

        public virtual Task<T> GetAsync<T>(Func<Task<T>> dataAccessMethod, string cacheKeySuffix, Func<T, int> expirationTimeMethod, [CallerMemberName] string methodName = "", [CallerFilePath] string filePath = "")
        {
            return GetInternalAsync(dataAccessMethod, cacheKeySuffix, expirationTimeMethod, methodName, filePath);
        }

        public virtual Task<T> GetAsync<T>(Func<Task<T>> dataAccessMethod, string cacheKeySuffix, int expirationTime, [CallerMemberName] string methodName = "", [CallerFilePath] string filePath = "")
        {
            return GetInternalAsync(dataAccessMethod, cacheKeySuffix, item => expirationTime, methodName, filePath);
        }

        internal T GetInternal<T>(Func<T> dataAccessMethod, string cacheKeySuffix, Func<T, int> expirationTimeMethod, string methodName, string filePath)
        {
            var cacheObjectWrapper = GetItemFromCache<T>(cacheKeySuffix, methodName, filePath);

            if (cacheObjectWrapper != null)
            {
                return cacheObjectWrapper.CacheObject;
            }

            var result = dataAccessMethod.Invoke();

            SetItemToCache(result, cacheKeySuffix, expirationTimeMethod, methodName, filePath);

            return result;
        }

        internal async Task<T> GetInternalAsync<T>(Func<Task<T>> dataAccessMethod, string cacheKeySuffix, Func<T, int> expirationTimeMethod, string methodName, string filePath)
        {
            var cacheObjectWrapper = GetItemFromCache<T>(cacheKeySuffix, methodName, filePath);

            if (cacheObjectWrapper != null)
            {
                return cacheObjectWrapper.CacheObject;
            }

            var result = await dataAccessMethod();

            SetItemToCache(result, cacheKeySuffix, expirationTimeMethod, methodName, filePath);

            return result;
        }

        // this method must be used only for methods from the same class
        protected string GetCacheKey(string methodName, string cacheKeySuffix, [CallerFilePath]string filePath = "")
        {
            var className = Path.GetFileNameWithoutExtension(filePath);

            return GetCacheKeyInternal(className, methodName, cacheKeySuffix);
        }

        // this method must be used only for methods from the same class
        protected void ClearCache(string methodName, string cacheKeySuffix, [CallerFilePath]string filePath = "")
        {
            var className = Path.GetFileNameWithoutExtension(filePath);

            var cacheKey = GetCacheKeyInternal(className, methodName, cacheKeySuffix);
            _cache.Remove(cacheKey);
        }

        private CacheObjectWrapper<T> GetItemFromCache<T>(string cacheKeySuffix, string methodName, string filePath)
        {
            Debug.WriteLine($"*** BaseCache.Get for {methodName} has been called");

            var className = Path.GetFileNameWithoutExtension(filePath);
            var cacheKey = GetCacheKeyInternal(className, methodName, cacheKeySuffix);

            return _cache.GetGeneric<T>(cacheKey);
        }

        private void SetItemToCache<T>(T item, string cacheKeySuffix, Func<T, int> expirationTimeMethod, string methodName, string filePath)
        {
            var isItemFoundInDatabase = !ObjectUtils.IsEqualsDefaultValue(item);

            var itemExpirationTime = isItemFoundInDatabase ? expirationTimeMethod.Invoke(item) : _tempItemsExpirationInSeconds;

            if (itemExpirationTime > 0)
            {
                var className = Path.GetFileNameWithoutExtension(filePath);
                var cacheKey = GetCacheKeyInternal(className, methodName, cacheKeySuffix);

                _cache.Set(cacheKey, item, itemExpirationTime);
            }
        }

        private string GetCacheKeyInternal(string className, string methodName, string cacheKeySuffix)
        {
            return $"{_cache.CacheKeyPrefix}_{className}_{methodName}_{cacheKeySuffix}";
        }
    }
}
