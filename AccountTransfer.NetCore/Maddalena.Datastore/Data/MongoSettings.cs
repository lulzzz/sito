using System;
using System.Collections.Generic;
using System.Text;

namespace Maddalena.Datastorage.Data
{
    class MongoSettings : MongoBaseObject
    {
        public List<string> Labels { get; set; } = new List<string>();
    }
}
