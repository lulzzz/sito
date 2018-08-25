using System;
using Maddalena.Client;
using Maddalena.Numl.Model;

namespace Maddalena.Grains.Learning
{
    [Serializable]
    class LabeledNews
    {
        [StringFeature]
        public string Title { get; set; }

        [StringFeature]
        public string Description { get; set; }

        [StringFeature]
        public string Categories { get; set; }

        [StringFeature]
        public string Link { get; set; }

        [Label]
        public LabelValue LabelValue { get; set; }
    }
}
