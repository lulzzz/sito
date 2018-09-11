using System.Collections.Generic;
using Maddalena.Core.Mongo;
using Maddalena.Core.Settings;

namespace Maddalena.Core.Blog
{
    public class BlogSettings
    {
        public List<string> BlogCategories { get; set; } = new List<string>();
    }
}
