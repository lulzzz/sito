using System;
using System.Threading.Tasks;
using AutoMapper;
using Maddalena.Client;
using Maddalena.Datastore.Data;
using Maddalena.Datastore.Mongolino;

namespace Maddalena.Datastore
{
    public static class Datastore
    {
        private static Collection<T> GetCollection<T>() where T : CollectionItem
        {
            return new Collection<T>("mongodb://localhost/ml", typeof(T).Name.ToLowerInvariant());
        }

        private static readonly IMapper _mapper;
        private static readonly Collection<MongoFeed> _feedCollection = GetCollection<MongoFeed>();
        private static readonly Collection<MongoLabel> _labelCollection = GetCollection<MongoLabel>();
        private static readonly Collection<MongoNews> _newsCollection = GetCollection<MongoNews>();

        static Datastore()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Feed, MongoFeed>();
                cfg.CreateMap<MongoFeed, Feed>();

                cfg.CreateMap<Label, MongoLabel>();
                cfg.CreateMap<MongoLabel, Label>();

                cfg.CreateMap<Client.News, MongoNews>();
                cfg.CreateMap<MongoNews, Client.News>();
            });

            _mapper = config.CreateMapper();
        }

        public static Task Insert(Feed feed) => _feedCollection.InsertOneAsync(_mapper.Map<MongoFeed>(feed));

        public static Task Insert(Client.News news) => _newsCollection.InsertOneAsync(_mapper.Map<MongoNews>(news));

        public static Task Insert(Label label) => _labelCollection.InsertOneAsync(_mapper.Map<MongoLabel>(label));
    }
}
