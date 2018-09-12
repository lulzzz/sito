using System.Threading.Tasks;

namespace Maddalena.Core.Youtube
{
    public interface IYoutubeService
    {
        Task LoadChannel(string channelId);
        Task<SubtitledVideo> GetSubtitledVideoAsyncFromUrl(string url);
        Task<SubtitledVideo> GetSubtitledVideoAsync(string videoId);
        long Count();
    }
}