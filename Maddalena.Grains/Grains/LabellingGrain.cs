using System;
using System.Linq;
using System.Threading.Tasks;
using Maddalena.Client;
using Maddalena.Client.Interfaces;
using Maddalena.Datastorage;
using Maddalena.Grains.Learning;
using Maddalena.Numl.Model;
using Maddalena.Numl.Supervised;
using Maddalena.Numl.Supervised.DecisionTree;
using Orleans;
using Orleans.Runtime;

namespace Maddalena.Grains.Grains
{
    class LabellingGrain : Grain, ILabellingGrain, IRemindable
    {
        private IModel _model;

        public override async Task OnActivateAsync()
        {
            await base.OnActivateAsync();

            _model = await Datastore.Model.LoadModelAsync(this.GetPrimaryKeyString());
        }

        public async Task LabelAsync(News news)
        {
            if(_model == null)
            {
                var value = (LabelValue) (DateTime.Now.Millisecond % 3);
                await Datastore.News.LabelAsync(news, this.GetPrimaryKeyString(), value);
                return;
            }

            var res = _model.Predict(new LabeledNews
            {
                Categories = string.Join(" ", news.Categories),
                Description = news.Description,
                Link = news.Link,
                Title = news.Title
            });

            await Datastore.News.LabelAsync(news,IdentityString, res.LabelValue);
        }

        public async Task ReceiveReminder(string reminderName, TickStatus status)
        {
            var generator = new DecisionTreeGenerator(25, 4);

            var labeled = new Func<News, LabelValue, LabeledNews>((news, label) =>
             new LabeledNews
             {
                 Categories = string.Join(" ", news.Categories),
                 Description = news.Description,
                 LabelValue = label,
                 Link = news.Link,
                 Title = news.Title
             });

            var bad = (await Datastore.News.GetNews(IdentityString, LabelValue.Bad, 200))
                      .Select(x => labeled(x, LabelValue.Bad));

            var good = (await Datastore.News.GetNews(IdentityString, LabelValue.Good, 208))
                       .Select(x => labeled(x, LabelValue.Good));

            var neutral = (await Datastore.News.GetNews(IdentityString, LabelValue.Irrelevant, 700))
                            .Select(x => labeled(x, LabelValue.Irrelevant));

            var data = bad.Concat(good.Concat(neutral)).ToArray();

            if (data.Length > 0)
            {
                Descriptor descriptor = Descriptor.Create<LabeledNews>();

                _model = generator.Generate(descriptor, data);

                await Datastore.Model.SaveModelAsync(IdentityString, _model);

                foreach (var item in await Datastore.News.AllAsync())
                {
                    await LabelAsync(item);
                }
            }
        }

        public async Task SetupReminderAsync()
        {
            const string reminderName = "modelReminder";

            if (await GetReminder(reminderName) == null)
            {
                await RegisterOrUpdateReminder(reminderName, TimeSpan.Zero, TimeSpan.FromMinutes(5));
            }
        }
    }   
}
