using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maddalena.Core.Mongo;
using MongoDB.Driver;
using YoutubeExplode;
using YoutubeExplode.Models;

namespace Maddalena.Core.Youtube
{
    public class YoutubeService : IYoutubeService
    {
        private readonly IMongoCollection<SubtitledVideo> _video;

        public YoutubeService(string connectionString)
        {
            _video = MongoUtil.FromConnectionString<SubtitledVideo>(connectionString, "shyopedia");
        }

        public long Count() => _video.EstimatedDocumentCount();

        public async Task LoadChannel(string channelId)
        {
            var client = new YoutubeClient();
            var list = await client.GetChannelUploadsAsync(channelId);

            foreach (var video in list)
            {
                await _video.InsertOneAsync(await GetSubtitledVideoAsync(video.Id));
            }
        }

        public Task<SubtitledVideo> GetSubtitledVideoAsyncFromUrl(string url)
        {
            if (!YoutubeClient.TryParseVideoId(url, out var videoId))
            {
                throw new ArgumentException();
            }

            return GetSubtitledVideoAsync(videoId);
        }

        public async Task<SubtitledVideo> GetSubtitledVideoAsync(string videoId)
        {
            var client = new YoutubeClient();

            var video = await client.GetVideoAsync(videoId);
            var captionTrackInfos = await client.GetVideoClosedCaptionTrackInfosAsync(videoId);
            var mediaStreamInfoSet = await client.GetVideoMediaStreamInfosAsync(videoId);
            var captions = await client.GetClosedCaptionTrackAsync(captionTrackInfos.First());

            var subVideo = new SubtitledVideo
            {
                Title = video.Title,
                YoutubeUrl = video.GetUrl(),
                YoutubeId = videoId,
                Thumbnail = video.Thumbnails.MediumResUrl,
                VideoSourceUrl = mediaStreamInfoSet.Muxed[0].Url,
                Published = video.UploadDate.DateTime,
                TextBody = string.Join(" ",captions.Captions.Select(x=>x.Text))
            };

            var strHtml = $"<span onclick=\"goto(0)\">{captions.Captions[0].Text}</span>";

            for (int i = 1; i < captions.Captions.Count; i++)
            {
                var diff = captions.Captions[i].Offset - (captions.Captions[i - 1].Offset + captions.Captions[i - 1].Duration);

                if (diff >= TimeSpan.FromMilliseconds(150)) strHtml += ".</br>";

                strHtml += $"<span onclick=\"goto({(int)captions.Captions[i].Offset.TotalSeconds})\">{captions.Captions[i].Text}</span>";
            }

            subVideo.HtmlBody = strHtml;

            return subVideo;
        }
    }
}
