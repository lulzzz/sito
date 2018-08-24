using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Maddalena.Datastore.Mongolino.Attributes;
using MongoDB.Driver;

namespace Maddalena.Datastore.Mongolino
{
    internal class Collection<T> where T: CollectionItem
    {
        internal Collection(string connectionString, string collectionName)
        {
            var type = typeof(T);

            if (connectionString != null)
            {
                var url = new MongoUrl(connectionString);
                var client = new MongoClient(connectionString);
                MongoCollection = client.GetDatabase(url.DatabaseName ?? "default").GetCollection<T>(collectionName ?? type.Name.ToLowerInvariant());
            }
            else
            {
                MongoCollection = (new MongoClient()).GetDatabase("default").GetCollection<T>(collectionName ?? type.Name.ToLowerInvariant());
            }

            foreach (var prop in type.GetProperties())
            {
                if (prop.GetCustomAttribute(typeof(AscendingIndexAttribute)) != null)
                {
                    MongoCollection.Indexes.CreateOne(
                        Builders<T>.IndexKeys.Ascending(new StringFieldDefinition<T>(prop.Name)));
                }

                if (prop.GetCustomAttribute(typeof(DescendingIndexAttribute)) != null)
                {
                    MongoCollection.Indexes.CreateOne(
                        Builders<T>.IndexKeys.Descending(new StringFieldDefinition<T>(prop.Name)));
                }

                if (prop.GetCustomAttribute(typeof(FullTextIndexAttribute)) != null)
                {
                    MongoCollection.Indexes.CreateOne(Builders<T>.IndexKeys.Text(new StringFieldDefinition<T>(prop.Name)));
                }
            }
        }

        internal Task InsertOneAsync(T save) => MongoCollection.InsertOneAsync(save);

        private IMongoCollection<T> MongoCollection { get; }

        internal void FullTextindex(Expression<Func<T, object>> func)
        {
            MongoCollection.Indexes.CreateOne(Builders<T>.IndexKeys.Text(func));
        }

        internal void AscendingIndex(Expression<Func<T, object>> func)
        {
            MongoCollection.Indexes.CreateOne(Builders<T>.IndexKeys.Ascending(func));
        }

        internal void DescendingIndex(Expression<Func<T, object>> func)
        {
            MongoCollection.Indexes.CreateOne(Builders<T>.IndexKeys.Descending(func));
        }

        internal IEnumerable<T> FullTextSearch(string text)
        {
            var filter = Builders<T>.Filter.Text(text);
            var res = MongoCollection.Find(filter);
            return res.ToEnumerable();
        }

        internal async Task<IEnumerable<T>> FullTextSearchAsync(string text)
        {
            var filter = Builders<T>.Filter.Text(text);
            var res = await MongoCollection.FindAsync(filter);

            return res.ToEnumerable();
        }

        internal T Random
        {
            get
            {
                if (Empty) return default(T);

                var res = MongoCollection.Find(Builders<T>.Filter.Empty);

                var rnd = new Random(Guid.NewGuid().GetHashCode());

                return res.Skip(rnd.Next((int)res.Count() - 1)).FirstOrDefault();
            }
        }

        internal IEnumerable<T> TakeRandom(int v)
        {
            for (int i = 0; i < v; i++) yield return Random;
        }

        internal IEnumerable<T> All => MongoCollection.Find(Builders<T>.Filter.Empty).ToEnumerable();

        internal bool Empty => Count() == 0;

        internal IQueryable<T> Queryable() => MongoCollection.AsQueryable();

        internal IEnumerable<K> Select<K>(Func<T, K> sel)
        {
            var filter = Builders<T>.Filter.Empty;
            return MongoCollection.Find(filter).ToEnumerable().Select(sel);
        }

        internal IEnumerable<T> SortBy(Expression<Func<T, object>> sel)
        {
            var filter = Builders<T>.Filter.Empty;
            var res = MongoCollection.Find(filter).SortBy(sel);
            return res.ToEnumerable();
        }

        internal IEnumerable<T> SortByDescending(Expression<Func<T, object>> sel)
        {
            var filter = Builders<T>.Filter.Empty;
            var res = MongoCollection.Find(filter).SortByDescending(sel);
            return res.ToEnumerable();
        }

        internal bool Any(Expression<Func<T, bool>> p) => MongoCollection.Find(p).Any();

        internal async Task<bool> AnyAsync(Expression<Func<T, bool>> p)
        {
            var res = await MongoCollection.FindAsync(p);
            return await res.AnyAsync();
        }

        internal long Count() => MongoCollection.Count(Builders<T>.Filter.Empty);

        internal long Count(Expression<Func<T, bool>> sel) => MongoCollection.Count(sel);

        internal Task<long> CountAsync(Expression<Func<T, bool>> sel) => MongoCollection.CountAsync(sel);

        internal Task<long> CountAsync() => MongoCollection.CountAsync(Builders<T>.Filter.Empty);

        internal void Add(T obj)
        {
            MongoCollection.InsertOneAsync(obj);
        }

        internal async Task<IEnumerable<T>> AnyEqualAsync<K>(Expression<Func<T, IEnumerable<K>>> sel, K value)
        {
            var filter = Builders<T>.Filter.AnyEq(sel, value);
            var res = await MongoCollection.FindAsync(filter);
            return res.ToEnumerable();
        }

        internal async Task<IEnumerable<T>> AnyGreaterAsync<K>(Expression<Func<T, IEnumerable<K>>> sel, K value)
        {
            var filter = Builders<T>.Filter.AnyGt(sel, value);
            var res = await MongoCollection.FindAsync(filter);
            return res.ToEnumerable();
        }

        internal async Task<IEnumerable<T>> AnyGreaterOrEqualAsync<K>(Expression<Func<T, IEnumerable<K>>> sel, K value)
        {
            var filter = Builders<T>.Filter.AnyGte(sel, value);
            var res = await MongoCollection.FindAsync(filter);
            return res.ToEnumerable();
        }


        internal async Task<IEnumerable<T>> AnyLowerAsync<K>(Expression<Func<T, IEnumerable<K>>> sel, K value)
        {
            var filter = Builders<T>.Filter.AnyLt(sel, value);
            var res = await MongoCollection.FindAsync(filter);
            return res.ToEnumerable();
        }

        internal async Task<IEnumerable<T>> AnyLowerOrEqualAsync<K>(Expression<Func<T, IEnumerable<K>>> sel, K value)
        {
            var filter = Builders<T>.Filter.AnyLte(sel, value);
            var res = await MongoCollection.FindAsync(filter);
            return res.ToEnumerable();
        }

        internal IEnumerable<T> AnyEqual<K>(Expression<Func<T, IEnumerable<K>>> sel, K value)
        {
            var filter = Builders<T>.Filter.AnyEq(sel, value);
            var res = MongoCollection.Find(filter);
            return res.ToEnumerable();
        }

        internal IEnumerable<T> AnyGreater<K>(Expression<Func<T, IEnumerable<K>>> sel, K value)
        {
            var filter = Builders<T>.Filter.AnyGt(sel, value);
            var res = MongoCollection.Find(filter);
            return res.ToEnumerable();
        }

        internal IEnumerable<T> AnyGreaterOrEqual<K>(Expression<Func<T, IEnumerable<K>>> sel, K value)
        {
            var filter = Builders<T>.Filter.AnyGte(sel, value);
            var res = MongoCollection.Find(filter);
            return res.ToEnumerable();
        }


        internal IEnumerable<T> AnyLower<K>(Expression<Func<T, IEnumerable<K>>> sel, K value)
        {
            var filter = Builders<T>.Filter.AnyLt(sel, value);
            var res = MongoCollection.Find(filter);
            return res.ToEnumerable();
        }

        internal IEnumerable<T> AnyLowerOrEqual<K>(Expression<Func<T, IEnumerable<K>>> sel, K value)
        {
            var filter = Builders<T>.Filter.AnyLte(sel, value);
            var res = MongoCollection.Find(filter);
            return res.ToEnumerable();
        }


        internal T Create(T obj)
        {
            MongoCollection.InsertOne(obj);
            return obj;
        }

        internal async Task<T> CreateAsync(T obj)
        {
            await MongoCollection.InsertOneAsync(obj);
            return obj;
        }

        internal void Replace(T obj)
        {
            var filter = Builders<T>.Filter.Eq(x => x.Id, obj.Id);
            MongoCollection.ReplaceOne(filter, obj);
        }


        internal async Task ReplaceAsync(T obj)
        {
            var filter = Builders<T>.Filter.Eq(x => x.Id, obj.Id);
            await MongoCollection.ReplaceOneAsync(filter, obj);
        }


        internal async Task<T> AddToAsync<K>(T obj, Expression<Func<T, IEnumerable<K>>> sel, K value)
        {
            var filter = Builders<T>.Filter.Eq(x => x.Id, obj.Id);
            var update = Builders<T>.Update.AddToSet(sel, value);
            await MongoCollection.UpdateOneAsync(filter, update);
            return obj;
        }

        internal async Task<T> AddToAsync<K>(T obj, Expression<Func<T, IEnumerable<K>>> sel, IEnumerable<K> value)
        {
            var filter = Builders<T>.Filter.Eq(x => x.Id, obj.Id);
            var update = Builders<T>.Update.AddToSetEach(sel, value);
            await MongoCollection.UpdateOneAsync(filter, update);
            return obj;
        }

        internal T AddTo<K>(T obj, Expression<Func<T, IEnumerable<K>>> sel, K value)
        {
            var filter = Builders<T>.Filter.Eq(x => x.Id, obj.Id);
            var update = Builders<T>.Update.AddToSet(sel, value);
            MongoCollection.UpdateOne(filter, update);
            return obj;
        }

        internal async Task UpdateAsync(T obj)
        {
            var filter = Builders<T>.Filter.Eq(x => x.Id, obj.Id);
            await MongoCollection.ReplaceOneAsync(filter, obj);
        }

        internal void Update<K>(T obj, Expression<Func<T, K>> sel, K value)
        {
            var filter = Builders<T>.Filter.Eq(x => x.Id, obj.Id);
            var update = Builders<T>.Update.Set(sel, value);
            MongoCollection.UpdateOne(filter, update);
        }

        internal async Task UpdateAsync<K>(T obj, Expression<Func<T, K>> sel, K value)
        {
            var filter = Builders<T>.Filter.Eq(x => x.Id, obj.Id);
            var update = Builders<T>.Update.Set(sel, value);
            await MongoCollection.UpdateOneAsync(filter, update);
        }

        internal async Task AppendAsync<K>(T obj, Expression<Func<T, IEnumerable<K>>> sel, K value)
        {
            var filter = Builders<T>.Filter.Eq(x => x.Id, obj.Id);
            var update = Builders<T>.Update.AddToSet(sel, value);
            await MongoCollection.UpdateOneAsync(filter, update);
        }

        internal void Increase<K>(T obj, Expression<Func<T, K>> sel, K value)
        {
            var filter = Builders<T>.Filter.Eq(x => x.Id, obj.Id);
            var update = Builders<T>.Update.Inc(sel, value);
            MongoCollection.UpdateOne(filter, update);
        }

        internal async Task IncreaseAsync<K>(T obj, Expression<Func<T, K>> sel, K value)
        {
            var filter = Builders<T>.Filter.Eq(x => x.Id, obj.Id);
            var update = Builders<T>.Update.Inc(sel, value);
            await MongoCollection.UpdateOneAsync(filter, update);
        }

        internal void Delete(T obj)
        {
            MongoCollection.DeleteOne(Builders<T>.Filter.Eq(x => x.Id, obj.Id));
        }

        internal void Delete(IEnumerable<T> obj)
        {
            MongoCollection.DeleteMany(Builders<T>.Filter.In(x => x.Id, obj.Select(x => x.Id)));
        }

        internal async Task DeleteAsync(Expression<Func<T, bool>> p)
        {
            await MongoCollection.DeleteManyAsync(p);
        }

        internal async Task DeleteAsync(T obj)
        {
            await MongoCollection.DeleteOneAsync(Builders<T>.Filter.Eq(x => x.Id, obj.Id));
        }

        internal async Task DeleteAsync(IEnumerable<T> obj)
        {
            await MongoCollection.DeleteManyAsync(Builders<T>.Filter.In(x => x.Id, obj.Select(x => x.Id)));
        }

        internal T GetOrCreate(Expression<Func<T, bool>> p, T obj) => FirstOrDefault(p) ?? Create(obj);

        internal async Task<T> GetOrCreateAsync(Expression<Func<T, bool>> p, T obj)
        {
            return (await FirstOrDefaultAsync(p)) ?? await CreateAsync(obj);
        }

        internal T FirstOrDefault() => MongoCollection.Find(Builders<T>.Filter.Empty).FirstOrDefault();

        internal T FirstOrDefault(Expression<Func<T, bool>> p) => MongoCollection.Find(p).FirstOrDefault();

        internal async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> p) => await (await MongoCollection.FindAsync(p)).FirstOrDefaultAsync();

        internal T First(Expression<Func<T, bool>> p) => MongoCollection.Find(p).FirstOrDefault();

        internal IEnumerable<T> Where(Expression<Func<T, bool>> p)
        {
            return (MongoCollection.Find(p)).ToEnumerable();
        }

        internal async Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> p)
        {
            return (await MongoCollection.FindAsync(p)).ToEnumerable();
        }

        internal IEnumerable<T> Search(string search) => MongoCollection.Find(Builders<T>.Filter.Text(search)).ToEnumerable();

        internal int Sum(Func<T, int> sum) => MongoCollection.Find(Builders<T>.Filter.Empty).ToEnumerable().Sum(sum);

        internal double Sum(Func<T, double> sum) => MongoCollection.Find(Builders<T>.Filter.Empty).ToEnumerable().Sum(sum);

        internal decimal Sum(Func<T, decimal> sum) => MongoCollection.Find(Builders<T>.Filter.Empty).ToEnumerable().Sum(sum);

        internal float Sum(Func<T, float> sum) => MongoCollection.Find(Builders<T>.Filter.Empty).ToEnumerable().Sum(sum);
    }
}
