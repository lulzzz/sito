using System;
using System.Threading.Tasks;
using Maddalena.Client;
using Maddalena.Client.Interfaces;
using Maddalena.Datastorage;
using Maddalena.Grains.DataProvider;
using Maddalena.Grains.Learning;
using Maddalena.Numl.Utils;
using Orleans;
using Orleans.MultiCluster;
using Orleans.Runtime;

namespace Maddalena.Grains.Grains
{
    [GlobalSingleInstance]
    public class FeedGrain : Grain, IFeedGrain, IRemindable
    {
        public override async Task OnActivateAsync()
        {

            Ject.AddAssembly(typeof(Datastore).Assembly);
            Ject.AddAssembly(typeof(LabelValue).Assembly);
            Ject.AddAssembly(typeof(LabeledNews).Assembly);

            await base.OnActivateAsync();
        }

        public async Task ReceiveReminder(string reminderName, TickStatus status)
        {
            foreach (var feed in Datastore.Feed.Feeds)
            {
                await FeedProvider.ReadFeedAsync(feed, async news => 
                {
                    await Datastore.News.Create(news);

                    foreach (var label in await Datastore.Settings.Labels())
                    {
                        await GrainFactory.GetGrain<ILabellingGrain>(label).LabelAsync(news);
                    }
                });
            }
        }

        public async Task SetupReminderAsync()
        {
            const string reminderName = "reminder";

            if (await GetReminder(reminderName) == null)
            {
                await RegisterOrUpdateReminder(reminderName, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            }
        }
    }
}