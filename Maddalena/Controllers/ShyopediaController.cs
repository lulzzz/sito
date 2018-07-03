using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maddalena.Models.Shyopedia;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using YoutubeExplode;
using YoutubeExplode.Models;

namespace Maddalena.Controllers
{
    public class ShyopediaController : Controller
    {
        static readonly IMongoCollection<SubVideo> videoCollection;
        static readonly IMongoCollection<VideoText> textCollection;

        static ShyopediaController()
        {
            var client = new MongoClient();
            videoCollection = client.GetDatabase("default").GetCollection<SubVideo>("shyopediaVideo");
            textCollection = client.GetDatabase("default").GetCollection<VideoText>("shyopediaText");

            if (textCollection.EstimatedDocumentCount() == 0)
            {
                Task.Run(async () => await Load());
            }

            textCollection.Indexes.CreateOne(new CreateIndexModel<VideoText>(Builders<VideoText>.IndexKeys.Text(x => x.Text)));
        }

        static async Task Load()
        {
            var client = new YoutubeClient();
            var list = await client.GetChannelUploadsAsync("UC4V3oCikXeSqYQr0hBMARwg");

            foreach (var video in list)
            {
                var info = await client.GetVideoClosedCaptionTrackInfosAsync(video.Id);

                if (info.Count <= 0) continue;

                var subVideo = new SubVideo
                {
                    Title = video.Title,
                    Thumbnail = video.Thumbnails.MediumResUrl,
                    Url = video.GetEmbedUrl()
                };

                await videoCollection.InsertOneAsync(subVideo);

                var captions = await client.GetClosedCaptionTrackAsync(info.First());
                var mergedCaptions = new List<VideoText>();

                foreach (var cap in captions.Captions)
                {
                    if (mergedCaptions.Count == 0)
                    {
                        mergedCaptions.Add(new VideoText
                        {
                            VideoId = video.Id,
                            OffSet = cap.Offset.TotalSeconds,
                            Text = cap.Text,
                            Duration = cap.Duration.TotalSeconds
                        });
                        continue;
                    }

                    if (mergedCaptions[mergedCaptions.Count - 1].Duration < 60)
                    {
                        mergedCaptions[mergedCaptions.Count - 1].OffSet += cap.Offset.TotalSeconds;
                        mergedCaptions[mergedCaptions.Count - 1].Text += $" {cap.Text}";
                        mergedCaptions[mergedCaptions.Count - 1].Duration += cap.Duration.TotalSeconds;
                    }
                    else
                    {
                        mergedCaptions.Add(new VideoText
                        {
                            VideoId = video.Id,
                            OffSet = cap.Offset.TotalSeconds,
                            Text = cap.Text,
                            Duration = cap.Duration.TotalSeconds
                        });
                    }
                }

                await textCollection.InsertManyAsync(mergedCaptions);
            }
        }

        public async Task<IActionResult> Index(string q)
        {
            if (string.IsNullOrWhiteSpace(q)) q = "freschezza";

            var res = await textCollection.FindAsync(Builders<VideoText>.Filter.Text(q), new FindOptions<VideoText, VideoText>()
            {
                Limit = 25
            });

            return View(await res.ToListAsync());
        }
    }
}   