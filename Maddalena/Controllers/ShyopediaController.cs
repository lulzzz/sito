using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maddalena.Models.Shyopedia;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using YoutubeExplode;
using YoutubeExplode.Models;
using YoutubeExplode.Models.ClosedCaptions;

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
                    Url = video.GetEmbedUrl(),
                    Published = video.UploadDate.DateTime
                };

                await videoCollection.InsertOneAsync(subVideo);

                var captions = await client.GetClosedCaptionTrackAsync(info.First());
                var mergedCaptions = new List<VideoText>();



                for (int i = 0; i < captions.Captions.Count; i++)
                {
                    for (int size = 1; size < 4; size++)
                    {
                        var cap = captions.Captions.Skip(i).Take(size).Aggregate((f, s) =>
                            new ClosedCaption($"{f.Text} {s.Text}", f.Offset, f.Duration + s.Duration));

                        mergedCaptions.Add(new VideoText
                        {
                            VideoId = subVideo.Id,
                            OffSet = cap.Offset,
                            Text = cap.Text,
                        });
                    }
                }

                await textCollection.InsertManyAsync(mergedCaptions);
            }
        }

        public async Task<IActionResult> Index(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                q = "freschezza";
            }

            var res = await textCollection.FindAsync(Builders<VideoText>.Filter.Text(q), new FindOptions<VideoText, VideoText>()
            {
                Limit = 25
            });

            ViewBag["SubTitle"] = $"Risultati della ricerca per {q}";

            var list = await res.ToListAsync();
            return View(list);
        }
    }
} 