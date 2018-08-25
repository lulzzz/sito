using System;

namespace Maddalena.Client
{
    [Serializable]
    public sealed class Feed
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public DateTime LastCheck { get; set; }
    }
}
