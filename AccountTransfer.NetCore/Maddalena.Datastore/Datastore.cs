using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Maddalena.Client;
using Maddalena.Datastore.Data;
using Maddalena.Datastore.Mongolino;
using Maddalena.Grains.Learning;
using Maddalena.Numl.Serialization;
using Maddalena.Numl.Supervised;

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

        private static readonly Collection<MongoModel> _modelCollection = GetCollection<MongoModel>();

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

        public static IEnumerable<News> NewsForLabel(string identityString, Label bad, int v)
        {
            throw new NotImplementedException();
        }

        public static Task Create(Feed feed) => _feedCollection.InsertOneAsync(_mapper.Map<MongoFeed>(feed));

        public static Feed[] Feeds => _feedCollection.All
                                        .Select(x => _mapper.Map<Feed>(x))
                                        .ToArray();

        public static Task<string[]> Labels()
        {
           return Task.FromResult(_labelCollection.All.Select(x => x.Label).Distinct().ToArray());
        }

        public static async Task Create(News news)
        {
            if (await _newsCollection.AnyAsync(x => x.Link == news.Link)) return;

            await _newsCollection.InsertOneAsync(_mapper.Map<MongoNews>(news));
        }

        public static async Task SaveModelAsync(string name, IModel model)
        {
            var existing = await _modelCollection.FirstOrDefaultAsync(x => x.Name == name);

            if (existing != null)
            {
                existing.Json = JsonWriter.SaveJson(model);
                await _modelCollection.ReplaceAsync(existing);
            }
            else
            {
                await _modelCollection.InsertOneAsync(new MongoModel
                {
                    Name = name,
                    Json = JsonWriter.SaveJson(model)
                });
            }
        }

        public static async Task<IModel> LoadModelAsync(string name)
        {
            var f = await _modelCollection.FirstOrDefaultAsync(x => x.Name == name);
            return f != null ? JsonReader.ReadJson<IModel>(f.Json) : null;
        }

        public static async Task<Label> GetLabel(News news, string label)
        {
            return (await _labelCollection
                            .FirstOrDefaultAsync(x => x.NewsId == news.Id
                                                 && x.Label == label)

                             )?.LabelValue ?? Label.Irrelevant;
        }

        public static async Task<News[]> GetNews(string label, Label value)
        {
            var lbl = (await _labelCollection
                            .WhereAsync(x => x.Label == label && x.LabelValue == value))
                     .Take(100);


        }
    }
}
