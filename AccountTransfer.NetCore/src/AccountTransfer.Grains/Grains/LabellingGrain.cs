using System.Collections.Generic;
using System.Threading.Tasks;
using Maddalena.Client;
using Maddalena.Grains.Learning;
using Maddalena.Numl.Model;
using Maddalena.Numl.Supervised;
using Maddalena.Numl.Supervised.DecisionTree;
using Orleans;

namespace Maddalena.Grains.Grains
{
    class LabellingGrain : Grain, ILabellingGrain
    {
        private IModel _model;
        private Descriptor descriptor = Descriptor.Create<LabeledNews>();

        public override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();

            _model = await Datastore.Datastore.LoadModelAsync(IdentityString);
        }

        public async Task LabelAsync(News news)
        {
            await UpdateModelAsync();

            var res = _model.Predict(new LabeledNews
            {
                Categories = string.Join(" ", news.Categories),
                Description = news.Description,
                Link = news.Link,
                Timestamp = news.Timestamp,
                Title = news.Title
            });
        }

        public Task UpdateModelAsync()
        {
            var generator = new DecisionTreeGenerator();
            var data = new List<LabeledNews>();

            data.AddRange(Datastore.Datastore.NewsForLabel(IdentityString, Label.Bad, 108);

            _model = generator.Generate(descriptor, data);

            await ModelRepository.SaveModelAsync(IdentityString, _model);*/

            return Task.CompletedTask;
        }
    }   
}
