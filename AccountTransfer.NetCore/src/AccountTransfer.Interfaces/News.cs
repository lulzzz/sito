using System;

namespace Maddalena.Client
{
    [Serializable]
    public sealed class News
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Timestamp { get; set; }

        public string[] Categories { get; set; }

        public string Link { get; set; }
    }
}
