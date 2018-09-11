using System;
using System.Collections.Generic;
using System.Text;

namespace Maddalena.Core.Identity
{
    public class SiteSettings
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }

        public string GoogleClientId { get; set; }
        public string GoogleClientSecret { get; set; }

        public string TwitterConsumerKey { get; set; }
        public string TwitterConsumerSecret { get; set; }
    }
}
