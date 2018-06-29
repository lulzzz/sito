using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Maddalena.Models.FuGet
{
    public abstract class DataCacheBase
    {
        protected readonly MemoryCache cache = new MemoryCache(new MemoryCacheOptions());
    }

    public abstract class DataCache<TResult> : DataCacheBase
    {
        private readonly TimeSpan expireAfter;

        public DataCache(TimeSpan expireAfter)
        {
            this.expireAfter = expireAfter;
        }

        public Task<TResult> GetAsync()
        {
            return GetAsync(CancellationToken.None);
        }

        public Task<TResult> GetAsync(CancellationToken token)
        {
            var key = "";
            if (cache.TryGetValue(key, out var result) && result is TResult rm) return Task.FromResult(rm);
            return GetValueAsync(token).ContinueWith(t =>
            {
                var rt = t.Result;
                cache.Set(key, rt, expireAfter);
                return rt;
            });
        }

        protected abstract Task<TResult> GetValueAsync(CancellationToken token);
    }

    public abstract class DataCache<TArg, TResult> : DataCacheBase
    {
        private readonly TimeSpan expireAfter;

        public DataCache(TimeSpan expireAfter)
        {
            this.expireAfter = expireAfter;
        }

        public Task<TResult> GetAsync(TArg arg)
        {
            return GetAsync(arg, CancellationToken.None);
        }

        public Task<TResult> GetAsync(TArg arg, CancellationToken token)
        {
            var key = arg;
            if (cache.TryGetValue(key, out var result) && result is TResult rm) return Task.FromResult(rm);
            return GetValueAsync(arg, token).ContinueWith(t =>
            {
                var rt = t.Result;
                cache.Set(key, rt, expireAfter);
                return rt;
            });
        }

        protected abstract Task<TResult> GetValueAsync(TArg arg, CancellationToken token);
    }

    public abstract class DataCache<TArg0, TArg1, TResult> : DataCacheBase
    {
        private readonly TimeSpan expireAfter;

        public DataCache(TimeSpan expireAfter)
        {
            this.expireAfter = expireAfter;
        }

        public Task<TResult> GetAsync(TArg0 arg0, TArg1 arg1)
        {
            return GetAsync(arg0, arg1, CancellationToken.None);
        }

        public Task<TResult> GetAsync(TArg0 arg0, TArg1 arg1, CancellationToken token)
        {
            var key = Tuple.Create(arg0, arg1);
            if (cache.TryGetValue(key, out var result) && result is TResult rm) return Task.FromResult(rm);
            return GetValueAsync(arg0, arg1, token).ContinueWith(t =>
            {
                var rt = t.Result;
                cache.Set(key, rt, expireAfter);
                return rt;
            });
        }

        protected abstract Task<TResult> GetValueAsync(TArg0 arg0, TArg1 arg1, CancellationToken token);
    }
}