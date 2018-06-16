using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using Mongolino;
using Mongolino.Attributes;
using Newtonsoft.Json;

namespace Maddalena.Models.Blog
{
    public class BlogArticle : DBObject<BlogArticle>
    {
        [JsonIgnore]
        [BsonIgnore]
        [AscendingIndex]
        public static IEnumerable<string> Categories
        {
            get { return All.Select(x => x.Category).Distinct(); }
        }

        [AscendingIndex]
        public string Author { get; set; }

        [Required]
        [AscendingIndex]
        public string Title { get; set; }

        [Required]
        [AscendingIndex]
        public string Link { get; set; }

        public DateTime DateTime { get; set; }

        [Required]
        [AscendingIndex]
        public string Category { get; set; }

        [Required]
        public string Body { get; set; }

        [Required]
        public bool Visible { get; set; }

        public double Views { get; set; }
        
        [FullTextIndex]
        public string FullText
        {
            get { return $"{Author} {Title} {Body} {Category}"; }
            set { }
        }

        public string TextPreview { get; internal set; }
    }
}