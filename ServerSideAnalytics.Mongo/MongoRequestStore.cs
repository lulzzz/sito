using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace ServerSideAnalytics.Mongo
{
    public class MongoRequestStore : IWebRequestStore<MongoWebRequest>
    {
        private readonly IMongoCollection<MongoWebRequest> _mongoCollection;

        public MongoRequestStore()
        {
            _mongoCollection = (new MongoClient()).GetDatabase("default").GetCollection<MongoWebRequest>("serverSideAnalytics");
        }

        public MongoRequestStore(string collectionName)
        {
            _mongoCollection = (new MongoClient()).GetDatabase("default").GetCollection<MongoWebRequest>(collectionName);
        }

        public Task<IEnumerable<IWebRequest>> QueryAsync(Func<object, object> p)
        {
            throw new NotImplementedException();
        }

        public MongoRequestStore(string connectionString, string collectionName)
        {
            var url = new MongoUrl(connectionString);
            var client = new MongoClient(connectionString);
            _mongoCollection = client.GetDatabase(url.DatabaseName ?? "default").GetCollection<MongoWebRequest>(collectionName);
        }

        public MongoWebRequest GetNew() => new MongoWebRequest();

        public Task AddAsync(MongoWebRequest request) => _mongoCollection.InsertOneAsync(request);

        public async Task<long> CountUniqueAsync(DateTime from, DateTime to)
        {
            var identities = await _mongoCollection.DistinctAsync(x => x.Identity, x => x.Timestamp >= from && x.Timestamp <= to);
            return identities.ToEnumerable().Count();
        }

        public async Task<IEnumerable<string>> IpAddresses(DateTime from, DateTime to)
        {
            var identities = await _mongoCollection.DistinctAsync(x => x.RemoteIpAddress, x => x.Timestamp >= from && x.Timestamp <= to);
            return identities.ToEnumerable();
        }

        public async Task<IEnumerable<IWebRequest>> RequestByIdentity(string identity)
        {
            var identities = await _mongoCollection.FindAsync(x => x.Identity == identity);
            return identities.ToEnumerable();
        }
    }
}