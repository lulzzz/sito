using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Maddalena.Client;
using Maddalena.Datastorage.Data;
using Maddalena.Datastorage.Stores;
using Maddalena.Numl.Serialization;
using Maddalena.Numl.Supervised;
using Maddalena.Numl.Utils;
using MongoDB.Driver;

namespace Maddalena.Datastorage
{
    public static class Datastore
    {
        internal static IMongoCollection<T> GetCollection<T>()
        {
            var url = new MongoUrl("mongodb://localhost/ml");

            var client = new MongoClient(url);
            return client.GetDatabase(url.DatabaseName).GetCollection<T>(typeof(T).Name.ToLowerInvariant());
        }

        internal static T2 Map<T2>(object obj) => _mapper.Map<T2>(obj);

        private static readonly IMapper _mapper;

        public static FeedStore Feed { get; } = new FeedStore();

        public static NewsStore News { get; } = new NewsStore();

        public static ModelStore Model { get; } = new ModelStore();

        public static SettingStore Settings { get; } = new SettingStore();

        static Datastore()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Feed, MongoFeed>();
                cfg.CreateMap<MongoFeed, Feed>();

                cfg.CreateMap<Client.News, MongoNews>();
                cfg.CreateMap<MongoNews, Client.News>();
            });

            _mapper = config.CreateMapper();
        }
    }
}
