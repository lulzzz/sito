using numl.Model;
using System;

namespace AccountTransfer.Grains.NewsProcessing
{
    class LabeledNews
    {
        [StringFeature]
        public string Title { get; set; }

        [StringFeature]
        public string Description { get; set; }

        [Feature]
        public DateTime Timestamp { get; set; }

        [StringFeature]
        public string Categories { get; set; }

        [StringFeature]
        public string Link { get; set; }

        [StringLabel]
        public string Label { get; set; } = "BULLSHIT";
    }
}
