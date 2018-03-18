using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Maddalena.Security;
using Mongolino;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Maddalena.Modules.Blog
{
    public class BlogArticle : DBObject<BlogArticle>
    {
        static BlogArticle()
        {
            DescendingIndex(x => x.Category);
            DescendingIndex(x => x.Link);
            DescendingIndex(x => x.DateTime);
            DescendingIndex(x => x.Tags);
        }

        [JsonIgnore]
        [BsonIgnore]
        public static IEnumerable<string> Categories
        {
            get { return All.Select(x => x.Category).Distinct(); }
        }

        public string Author { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Link { get; set; }

        public DateTime DateTime { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Body { get; set; }

        public string[] Tags { get; set; }

        public bool Visible { get; set; }

        public double Views { get; set; }
    }
}
