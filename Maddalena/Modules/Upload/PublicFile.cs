using System;
using Maddalena.Identity;
using Maddalena.Mongo;

namespace Maddalena.Modules.Upload
{
    public class PublicFile : DBObject<PublicFile>
    {
        public string Name { get; internal set; }
    }
}
