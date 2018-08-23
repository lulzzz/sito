using System;
using Microsoft.ML.Runtime.Api;

namespace AccountTransfer.Interfaces
{
    [Serializable]
    public class News
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Timestamp { get; set; }

        public string[] Categories { get; set; }

        public string Link { get; set; }
    }
}
