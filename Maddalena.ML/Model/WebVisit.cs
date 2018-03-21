using Mongolino;

namespace Maddalena.ML.Model
{
    public class WebVisit : DBObject<WebVisit>
    {
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
    }
}
