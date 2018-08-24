using System;
using System.Linq;
using System.Threading.Tasks;
using AccountTransfer.Grains;
using AutoMapper;
using Maddalena.Client;
using Maddalena.Client.Interfaces;
using Maddalena.News.Client;
using Maddalena.News.Client.Interfaces;
using Maddalena.News.Grains.Libraries.Mongolino;
using Maddalena.News.Grains.NewsProcessing;
using Orleans;

namespace Maddalena.News.Grains.Grains
{
    class NewsGrain : Grain, INewsGrain
    {
        private IMapper _mapper;
        private Collection<MongoNews> _collection = Settings.GetCollection<MongoNews>();

        public override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();

            _collection = Settings.GetCollection<MongoNews>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Maddalena.Client.News, MongoNews>();
                cfg.CreateMap<MongoNews, Maddalena.Client.News>();
            });

            _mapper = config.CreateMapper();
        }

        public Task Create(Maddalena.Client.News news)
        {
            return _collection.InsertOneAsync(_mapper.Map<MongoNews>(news));
        }

        public Task Delete(Maddalena.Client.News news)
        {
            return _collection.DeleteAsync(_mapper.Map<MongoNews>(news));
        }

        public Task<Maddalena.Client.News[]> GetNews()
        {
            return  Task.FromResult(_collection.All.Select(x=>_mapper.Map<Maddalena.Client.News>(x)).ToArray());
        }

        public Task<Maddalena.Client.News[]> NewsInLabel(string label, LabelValue value)
        {
            throw new NotImplementedException();
        }

        public Task Update(Maddalena.Client.News news)
        {
            return _collection.UpdateAsync(_mapper.Map<MongoNews>(news));
        }
    }
}
