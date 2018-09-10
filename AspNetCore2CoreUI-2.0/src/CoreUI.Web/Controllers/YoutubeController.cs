using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YoutubeExplode;
using YoutubeExplode.Models;
using YoutubeExplode.Models.ClosedCaptions;

namespace CoreUI.Web.Controllers
{
    public class YoutubeController : Controller
    {
        public async Task<ActionResult> Index(string url)
        {
            if (url == null) return View();

            var client = new YoutubeClient();
            if (!YoutubeClient.TryParseVideoId(url, out var videoId))
            {
                return View();
            }

            var minfo = await client.GetVideoMediaStreamInfosAsync(videoId);
            return View(minfo);
        }

        [Produces("application/json")]
        [Route("api/videoOfChannel/{channel}")]
        public async Task<IEnumerable<Video>> VideoOfChannel(string channel)
        {
            var client = new YoutubeClient();
            var list = await client.GetChannelUploadsAsync(channel);
            return list;
        }

        [Produces("application/json")]
        [Route("api/subtitles/{video}")]
        public async Task<IEnumerable<ClosedCaptionTrackInfo>> Subtitles(string video)
        {
            var client = new YoutubeClient();
            return await client.GetVideoClosedCaptionTrackInfosAsync(video);
        }

        [Produces("application/json")]
        [Route("api/subtitles/{video}/{n}")]
        public async Task<ClosedCaptionTrack> Subtitles(string video, int n)
        {
            var client = new YoutubeClient();
            var info = await client.GetVideoClosedCaptionTrackInfosAsync(video);
            return await client.GetClosedCaptionTrackAsync(info[n]);
        }
    }
}