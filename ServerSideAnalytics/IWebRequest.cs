﻿using System;

namespace ServerSideAnalytics
{
    public interface IWebRequest
    {
        DateTime Timestamp { get; set; }
        string Identity { get; set; }
        string RemoteIpAddress { get; set; }
        string User { get; set; }
        string Method { get; set; }
        string Path { get; set; }
        UserAgent UserAgent { get; set; }
        string Referer { get; set; }
    }
}