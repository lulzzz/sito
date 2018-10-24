using Maddalena.Core.Mongo;
using System;

namespace Maddalena.Core.Nuget
{
    public class PackageHistory : MongoObject
    {
        public DateTime DateTime { get; set; } = DateTime.Now;

        public Package Package { get; set; }
    }
}
