using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Maddalena.Core.Identity.Stores;
using MongoDB.Driver;

namespace Maddalena.Core.Mongo
{
	public class MongoObjectCollection<TItem> : IIdentityObjectCollection<TItem> where TItem : MongoObject
	{
		public MongoObjectCollection(string connectionString, string collectionName)
		{
			var type = typeof(TItem);

			if (connectionString != null)
			{
				var url = new MongoUrl(connectionString);
				var client = new MongoClient(connectionString);
				MongoCollection = client.GetDatabase(url.DatabaseName ?? "default")
					.GetCollection<TItem>(collectionName ?? type.Name.ToLowerInvariant());
			}
			else
			{
				MongoCollection = new MongoClient().GetDatabase("default")
					.GetCollection<TItem>(collectionName ?? type.Name.ToLowerInvariant());
			}
		}

		public IMongoCollection<TItem> MongoCollection { get; }

		public async Task<TItem> FindByIdAsync(string itemId)
		{
			return await FirstOrDefaultAsync(item => item.Id == itemId);
		}

	    public Task<IEnumerable<TField>> Distinct<TField>(Expression<Func<TItem, TField>> field)
	    {
	        return Distinct(field, x => true);
	    }

        public async Task<IEnumerable<TField>> Distinct<TField>(Expression<Func<TItem,TField>> field, Expression<Func<TItem, bool>> where)
	    {
	        var list = await (await MongoCollection.DistinctAsync(field, where)).ToListAsync();
	        return list;
	    }

	    public async Task<IEnumerable<TItem>> TakeAsync(int count, int skip = 0)
	    {
	        return await (await MongoCollection.FindAsync(x => true, new FindOptions<TItem, TItem>()
	        {
	            Skip = skip,
	            Limit = count
	        })).ToListAsync();
	    }

        public Task<IEnumerable<TItem>> GetAll()
		{
			return Task.FromResult(MongoCollection.AsQueryable() as IEnumerable<TItem>);
		}

		public async Task<TItem> CreateAsync(TItem obj)
		{
			await MongoCollection.InsertOneAsync(obj);
			return obj;
		}


		public async Task UpdateAsync(TItem obj)
		{
			var filter = Builders<TItem>.Filter.Eq(x => x.Id, obj.Id);
			await MongoCollection.ReplaceOneAsync(filter, obj);
		}

		public async Task DeleteAsync(TItem obj)
		{
			await MongoCollection.DeleteOneAsync(Builders<TItem>.Filter.Eq(x => x.Id, obj.Id));
		}


		public IQueryable<TItem> Queryable()
		{
			return MongoCollection.AsQueryable();
		}


		public bool Any(Expression<Func<TItem, bool>> p)
		{
			return MongoCollection.Find(p).Any();
		}


		public async Task<IEnumerable<TItem>> AnyEqualAsync<K>(Expression<Func<TItem, IEnumerable<K>>> sel, K value)
		{
			var filter = Builders<TItem>.Filter.AnyEq(sel, value);
			var res = await MongoCollection.FindAsync(filter);
			return res.ToEnumerable();
		}

		public async Task DeleteAsync(Expression<Func<TItem, bool>> p)
		{
			await MongoCollection.DeleteManyAsync(p);
		}


		public TItem FirstOrDefault()
		{
			return MongoCollection.Find(Builders<TItem>.Filter.Empty).FirstOrDefault();
		}

		public TItem FirstOrDefault(Expression<Func<TItem, bool>> p)
		{
			return MongoCollection.Find(p).FirstOrDefault();
		}

		public async Task<TItem> FirstOrDefaultAsync(Expression<Func<TItem, bool>> p)
		{
			return await (await MongoCollection.FindAsync(p)).FirstOrDefaultAsync();
		}


		public async Task<IEnumerable<TItem>> WhereAsync(Expression<Func<TItem, bool>> p)
		{
			return (await MongoCollection.FindAsync(p)).ToEnumerable();
		}
	}
}