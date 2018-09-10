using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YoutubeExplode;
using YoutubeExplode.Models;
using YoutubeExplode.Models.ClosedCaptions;

namespace CoreUI.Web.Controllers
{
    [Produces("application/json")]
    public class YoutubeApiController : Controller
    {
        [Route("youtubeApi/videoOfChannel/{channel}")]
        public async Task<IEnumerable<Video>> VideoOfChannel(string channel)
        {
            var client = new YoutubeClient();
            var list = await client.GetChannelUploadsAsync(channel);
            return list;
        }

        [Route("youtubeApi/subtitles/{video}")]
        public async Task<ClosedCaptionTrack> Subtitles(string video)
        {
            var client = new YoutubeClient();
            var info = await client.GetVideoClosedCaptionTrackInfosAsync(video);
            return await client.GetClosedCaptionTrackAsync(info.First());
        }
    }
}