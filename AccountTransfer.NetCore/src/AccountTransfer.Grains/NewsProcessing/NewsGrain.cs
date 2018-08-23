using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountTransfer.Interfaces;
using AutoMapper;
using MongoDB.Driver;
using numl.Model;
using numl.Supervised;
using numl.Supervised.DecisionTree;
using Orleans;

namespace AccountTransfer.Grains.NewsProcessing
{
    class NewsGrain : Grain, INewsGrain
    {
        private IModel model;
        private IMapper _mapper;
        private IMongoCollection<MongoNews> _collection;

        public override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();

            _collection = Mongo.Database.GetCollection<MongoNews>("news");

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<News, MongoNews>();
                cfg.CreateMap<MongoNews, News>();

                cfg.CreateMap<News, LabeledNews>()
                    .ForMember(x => x.Categories, x => x.MapFrom(req => string.Join(" ", req.Categories)));
            });

            _mapper = config.CreateMapper();

            model = await Mongo.LoadModelAsync("modello");
        }

        public async Task AnalizeAsync(News news)
        {
            var existing = await _collection.FindAsync(x => x.Link == news.Link);

            if (existing.Any()) return;

            await _collection.InsertOneAsync(_mapper.Map<MongoNews>(news));

            await UpdateModelAsync();

            var res = model.Predict(_mapper.Map<LabeledNews>(news));
        }

        public async Task UpdateModelAsync()
        {
            var description = Descriptor.Create<LabeledNews>();
            var generator = new DecisionTreeGenerator();

            var data = new List<LabeledNews>();
            foreach (var item in _collection.AsQueryable())
            {
                data.Add(_mapper.Map<LabeledNews>(item));
            }

            model = generator.Generate(description, data);

            await Mongo.SaveModelAsync("modello", model);
        }
    }   
}
