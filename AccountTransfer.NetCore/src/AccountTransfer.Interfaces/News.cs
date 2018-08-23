using System;
using Microsoft.ML.Runtime.Api;

namespace AccountTransfer.Interfaces
{
    [Serializable]
    public class News
    {
        [Column("0")]
        public string Title { get; set; }

        [Column("1")]
        public string Description { get; set; }

        [Column("3")]
        public DateTime Timestamp { get; set; }

        [Column("4")]
        public string[] Categories { get; set; }

        [Column("5")]
        public string Link { get; set; }
    }
}
