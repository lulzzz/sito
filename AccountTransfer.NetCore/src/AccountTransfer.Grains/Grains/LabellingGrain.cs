using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountTransfer.Grains;
using Maddalena.News.Client;
using Maddalena.News.Grains.Data;
using Maddalena.News.Grains.Learning;
using Maddalena.News.Grains.Libraries.Mongolino;
using Maddalena.News.Grains.NewsProcessing;
using numl.Model;
using numl.Supervised;
using numl.Supervised.DecisionTree;
using Orleans;

namespace Maddalena.News.Grains.Grains
{
    class LabellingGrain : Grain, ILabellingGrain
    {
        private IModel _model;
        private Descriptor descriptor = Descriptor.Create<LabeledNews>();
        private Collection<NewsLabel> _collection = Settings.GetCollection<NewsLabel>();

        public override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();
            _model = await ModelRepository.LoadModelAsync(IdentityString);
        }

        public async Task LabelAsync(MongoNews news)
        {
            await UpdateModelAsync();

            var res = _model.Predict(news);
        }

        private async Task<LabeledNews[]> LoadLabeledAsync()
        {
            var  (await _collection.WhereAsync(x => x.Label == IdentityString)).ToArray();
        }

        public async Task UpdateModelAsync()
        {
            var generator = new DecisionTreeGenerator();

            var data = new List<MongoNews>();
            foreach (var item in _collection.All)
            {
                data.Add(item);
            }

            _model = generator.Generate(descriptor, data);

            await ModelRepository.SaveModelAsync(IdentityString, _model);
        }
    }   
}
