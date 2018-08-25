﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maddalena.Client;
using Maddalena.Datastorage;
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

            _model = await Datastore.Model.LoadModelAsync(IdentityString);
        }

        public async Task LabelAsync(News news)
        {
            await UpdateModelAsync();

            var res = _model.Predict(new LabeledNews
            {
                Categories = string.Join(" ", news.Categories),
                Description = news.Description,
                Link = news.Link,
                Title = news.Title
            });

            Datastore.News.Label(news,IdentityString, res.LabelValue);
        }

        public async Task UpdateModelAsync()
        {
            var generator = new DecisionTreeGenerator();

            var labeled = new Func<News, LabelValue, LabeledNews>((news, label) =>
             new LabeledNews
             {
                 Categories = string.Concat(" ", news.Categories),
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

            var data = bad.Concat(good.Concat(neutral));
             
            _model = generator.Generate(descriptor, data);

            await Datastore.Model.SaveModelAsync(IdentityString, _model);
        }
    }   
}