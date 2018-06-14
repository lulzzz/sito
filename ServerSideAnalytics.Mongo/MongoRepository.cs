using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace ServerSideAnalytics.Mongo
{
    public class MongoRepository : IWebRequestRepository<MongoWebRequest>
    {
        private readonly IMongoCollection<MongoWebRequest> _mongoCollection;

        public MongoRepository()
        {
            _mongoCollection = (new MongoClient()).GetDatabase("default").GetCollection<MongoWebRequest>("serverSideAnalytics");
        }

        public MongoRepository(string collectionName)
        {
            _mongoCollection = (new MongoClient()).GetDatabase("default").GetCollection<MongoWebRequest>(collectionName);
        }

        public MongoRepository(string connectionString, string collectionName)
        {
            var url = new MongoUrl(connectionString);
            var client = new MongoClient(connectionString);
            _mongoCollection = client.GetDatabase(url.DatabaseName ?? "default").GetCollection<MongoWebRequest>(collectionName);
        }

        public MongoWebRequest GetNew() => new MongoWebRequest();

        public Task AddAsync(MongoWebRequest request) => _mongoCollection.InsertOneAsync(request);

        public Task<long> CountAsync(DateTime from, DateTime to) => _mongoCollection.CountAsync(x => x.Timestamp >= from && x.Timestamp <= to);

        public async Task<IEnumerable<IWebRequest>> QueryAsync(Expression<Func<MongoWebRequest, bool>> where)
        {
            return (await _mongoCollection.FindAsync(where)).ToEnumerable();
        }
    }
}