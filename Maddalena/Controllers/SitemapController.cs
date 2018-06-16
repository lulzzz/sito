using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maddalena.Models.Blog;
using Maddalena.Models.Sitemap;

namespace Maddalena.Controllers
{
    public class SitemapController : Controller
    {
        public SitemapController()
        {
        }

        [Route("sitemap")]
        public async Task<ActionResult> SitemapAsync()
        {
            string baseUrl = "https://matteofabbri.org/read/";

            // get a list of published articles
            var posts = BlogArticle.Queryable();

            // get last modified date of the home page
            var siteMapBuilder = new SitemapBuilder();

            // add the home page to the sitemap
            siteMapBuilder.AddUrl(baseUrl, modified: DateTime.UtcNow, changeFrequency: ChangeFrequency.Weekly, priority: 1.0);

            // add the blog posts to the sitemap
            foreach (var post in posts)
            {
                siteMapBuilder.AddUrl(baseUrl + post.Link, modified: post.DateTime, changeFrequency: null, priority: 0.9);
            }

            // generate the sitemap xml
            string xml = siteMapBuilder.ToString();
            return Content(xml, "text/xml");
        }
    }
}
