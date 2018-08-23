using System;

namespace AccountTransfer.Interfaces
{
    [Serializable]
    public class Feed
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public DateTime LastCheck { get; set; }
    }
}
