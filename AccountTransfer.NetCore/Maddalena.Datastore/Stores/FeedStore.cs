using System.Linq;
using System.Threading.Tasks;
using Maddalena.Client;
using Maddalena.Datastorage.Data;
using MongoDB.Driver;

namespace Maddalena.Datastorage
{
    public class FeedStore
    {
        private static readonly IMongoCollection<MongoFeed> _feedCollection;


        public Task Create(Feed feed) => _feedCollection
                              .InsertOneAsync(Datastore.Map<MongoFeed>(feed));

        public Feed[] Feeds => _feedCollection.AsQueryable()
                                        .Select(x => Datastore.Map<Feed>(x))
                                        .ToArray();
        static FeedStore()
        {
            _feedCollection = Datastore.GetCollection<MongoFeed>();

            if(_feedCollection.EstimatedDocumentCount()==0)
            {
                _feedCollection.InsertOne(new MongoFeed
                {
                    Name = "The Guardian",
                    Url = "https://www.theguardian.com/politics/blog/rss"
                });

                _feedCollection.InsertOne(new MongoFeed
                {
                    Name = "Politics – FiveThirtyEight",
                    Url = "https://fivethirtyeight.com/politics/feed/"
                });

                _feedCollection.InsertOne(new MongoFeed
                {
                    Name = "Politics – Mother Jones",
                    Url = "https://www.motherjones.com/politics/feed/"
                });

                _feedCollection.InsertOne(new MongoFeed
                {
                    Name = "Salon > politics",
                    Url = "https://www.salon.com/category/politics/feed"
                });

                _feedCollection.InsertOne(new MongoFeed
                {
                    Name = "Westminster blog",
                    Url = "http://feeds2.feedburner.com/ft/westminster"
                });

                _feedCollection.InsertOne(new MongoFeed
                {
                    Name = "Boing Boing",
                    Url = "https://boingboing.net/tag/politics/feed"
                });

                _feedCollection.InsertOne(new MongoFeed
                {
                    Name = "The Corner | National Review",
                    Url = "https://www.nationalreview.com/corner/feed/"
                });

                _feedCollection.InsertOne(new MongoFeed
                {
                    Name = "Reddit UK Politics",
                    Url = "https://www.reddit.com/r/ukpolitics/.rss"
                });

                _feedCollection.InsertOne(new MongoFeed
                {
                    Name = "Politics – Chicago Sun-Times",
                    Url = "https://chicago.suntimes.com/section/politics/feed"
                });

                _feedCollection.InsertOne(new MongoFeed
                {
                    Name = "Hit & Run",
                    Url = "http://feeds.feedburner.com/reason/HitandRun"
                });


                _feedCollection.InsertOne(new MongoFeed
                {
                    Name = "US News – twitchy.com",
                    Url = "https://twitchy.com/category/us-politics/feed/"
                });

                _feedCollection.InsertOne(new MongoFeed
                {
                    Name = "Power LinePower Line",
                    Url = "https://www.powerlineblog.com/index.xml"
                });

                _feedCollection.InsertOne(new MongoFeed
                {
                    Name = "Politics – The Daily Signal",
                    Url = "https://www.dailysignal.com/category/politics-topics/feed"
                });

                _feedCollection.InsertOne(new MongoFeed
                {
                    Name = "Vox - Politics & Policy",
                    Url = "https://www.vox.com/rss/policy-and-politics/index.xml"
                });

                _feedCollection.InsertOne(new MongoFeed
                {
                    Name = "Huffington Post Politics - U.S. Political News, Opinion and Analysis",
                    Url = "https://www.huffingtonpost.com/section/politics/feed"
                });

                _feedCollection.InsertOne(new MongoFeed
                {
                    Name = "Reddit PoliticalDiscussion",
                    Url = "https://www.reddit.com/r/PoliticalDiscussion/.rss"
                });

                _feedCollection.InsertOne(new MongoFeed
                {
                    Name = "Latest from Crooks and Liars",
                    Url = "http://feeds.crooksandliars.com/crooksandliars/YaCP"
                });

                _feedCollection.InsertOne(new MongoFeed
                {
                    Name = "Jihad Watch",
                    Url = "https://www.jihadwatch.org/feed"
                });

                _feedCollection.InsertOne(new MongoFeed
                {
                    Name = "POLITICO.EU",
                    Url = "http://www.politico.eu/feed/"
                });

                _feedCollection.InsertOne(new MongoFeed
                {
                    Name = "RealClearPolitics - Homepage",
                    Url = "https://www.realclearpolitics.com/index.xml"
                });

                _feedCollection.InsertOne(new MongoFeed
                {
                    Name = "politics.co.uk",
                    Url = "http://www.politics.co.uk/rss.xml"
                });

                _feedCollection.InsertOne(new MongoFeed
                {
                    Name = "iPolitics",
                    Url = "https://ipolitics.ca/feed/"
                });
            }
        }
    }
}
