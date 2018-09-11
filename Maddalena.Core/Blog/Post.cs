using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Maddalena.Core.Mongo;

namespace Maddalena.Core.Blog.Models
{
    public class Post : MongoObject
    {
        [Required]
        public string Title { get; set; }

        public string Slug { get; set; }

        [Required]
        public string Excerpt { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime PubDate { get; set; } = DateTime.UtcNow;

        public DateTime LastModified { get; set; } = DateTime.UtcNow;

        public bool IsPublished { get; set; } = true;

        public IList<string> Categories { get; set; } = new List<string>();
    }
}
