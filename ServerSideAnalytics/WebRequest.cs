using System;

namespace ServerSideAnalytics
{
    public class WebRequest
    {
        public DateTime Timestamp { get; set; }
        public string SessionId { get; set; }
        public string RemoteIpAddress { get; set; }
        public string User { get; set; }
        public string Method { get; set; }
        public string Path { get; set; }
        public UserAgent UserAgent { get; set; }
        public string Referer { get; set; }
    }
}