using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace PostCodes.API.Cache
{
    public class RedisCacheStorage : ICacheStorage
    {
        private readonly ConnectionMultiplexer _redisConnection;
        private IDatabase _redis;

        public RedisCacheStorage(ConnectionMultiplexer redisConnection)
        {
            _redisConnection = redisConnection;
            _redis = redisConnection.GetDatabase(0);
        }       
        

        public async Task<T> AddValueAsync<T>(string key, T value)
        {
            try
            {
                var updatedCode = await _redis.
                                    StringSetAsync(key, JsonConvert.SerializeObject(value));
                if (!updatedCode)
                {
                    return default(T);
                }
                return await GetValueAsync<T>(key);
            }
            catch (RedisException rEx)
            {
                throw new Exception(" Error occured while adding data to redis(" + rEx.Message + ")", rEx);
            }

        }

        public async Task<T> GetValueAsync<T>(string key)
        {
            try
            {
                var postCodeData = await _redis.StringGetAsync(key);
                if (postCodeData.IsNullOrEmpty)
                {
                    return default(T);
                }
                return JsonConvert.DeserializeObject<T>(postCodeData);
            }
            catch(RedisException rEx)
            {
                throw new Exception(" Error occured while accesing data from redis(" + rEx.Message + ")", rEx);
            }           
        }

        public async Task<bool> RemoveValueAsync(string key)
        {
            try
            {
                return await _redis.KeyDeleteAsync(key);
            }
            catch (RedisException rEx)
            {
                throw new Exception(" Error occured while deleting data from redis(" + rEx.Message + ")", rEx);
            }

        }
    }
}
