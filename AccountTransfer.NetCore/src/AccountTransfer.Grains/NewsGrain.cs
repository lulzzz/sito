using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AccountTransfer.Interfaces;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using Orleans;

namespace AccountTransfer.Grains
{
    class NewsGrain : Grain, INewsGrain
    {
        List<News> _news = new List<News>();

        public Task AnalizeAsync(News news)
        {
            _news.Add(news);

            var pipeline = new LearningPipeline
            {
                CollectionDataSource.Create(_news),
                new TextFeaturizer("Features", "SentimentText"),
                new FastTreeBinaryClassifier()
                {
                    NumLeaves = 5,
                    NumTrees = 5,
                    MinDocumentsInLeafs = 2
                }
            };

            pipeline.Train<News, SentimentPrediction>();
            return Task.CompletedTask;
        }
    }
}
