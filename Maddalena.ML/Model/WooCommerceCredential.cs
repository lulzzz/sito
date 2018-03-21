using System;
using System.Collections.Generic;
using System.Text;
using Mongolino;

namespace Maddalena.ML.Model
{
    public class WooCommerceCredential : DBObject<WooCommerceCredential>
    {
        public string Organization { get; set; }

        public string Url { get; set; }

        public string Key { get; set; }

        public string Secret { get; set; }
    }
}
