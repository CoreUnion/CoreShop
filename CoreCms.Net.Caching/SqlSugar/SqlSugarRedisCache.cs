using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCms.Net.Caching.Redis;
using StackExchange.Redis;
using CoreCms.Net.Configuration;

namespace CoreCms.Net.Caching.SqlSugar
{
    public class SqlSugarRedisCache : ICacheService
    {
        readonly RedisCacheManager _service = null;

        public SqlSugarRedisCache()
        {
            _service = new RedisCacheManager(); ;
        }

        public void Add<TV>(string key, TV value)
        {
            _service.Set(key, value);
        }

        public void Add<TV>(string key, TV value, int cacheDurationInSeconds)
        {
            _service.Set(key, value, cacheDurationInSeconds);
        }

        public bool ContainsKey<TV>(string key)
        {
            return _service.Exists(key);
        }

        public TV Get<TV>(string key)
        {
            return _service.Get<TV>(key);
        }

        public IEnumerable<string> GetAllKey<TV>()
        {

            return _service.SearchCacheRegex("SqlSugarDataCache.*");
        }

        public TV GetOrCreate<TV>(string cacheKey, Func<TV> create, int cacheDurationInSeconds = int.MaxValue)
        {
            if (this.ContainsKey<TV>(cacheKey))
            {
                return this.Get<TV>(cacheKey);
            }
            else
            {
                var result = create();
                this.Add(cacheKey, result, cacheDurationInSeconds);
                return result;
            }
        }

        public void Remove<TV>(string key)
        {
            _service.Remove(key);
        }
    }

}
