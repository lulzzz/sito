using System;
using Maddalena.Client;
using Maddalena.News.Client;
using Maddalena.News.Grains.Libraries.Mongolino;
using numl.Model;

namespace Maddalena.News.Grains.Learning
{
    [Serializable]
    class LabeledNews : CollectionItem
    {
        public string LabelId { get; set; }

        public string NewsId { get; set; }

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

        [Label]
        public LabelValue LabelValue { get; set; }
    }
}
