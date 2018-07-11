using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Maddalena.Models.Blog;
using Maddalena.Models.Sitemap;
using System.IO;
using System.Xml;
using Microsoft.SyndicationFeed.Rss;
using Microsoft.SyndicationFeed;
using System.Linq;

namespace Maddalena.Controllers
{
    public class SitemapController : Controller
    {
        public SitemapController()
        {
        }

        [Route("rss.xml")]
        public async Task<ActionResult> Rss()
        {
            var sw = new StringWriter();
            using (XmlWriter xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { Async = true, Indent = true }))
            {
                var writer = new RssFeedWriter(xmlWriter);

                foreach(var art in BlogArticle.Queryable())
                {
                    // Create item
                    var item = new SyndicationItem()
                    {
                        Title = art.Title,
                        Description = art.TextPreview,
                        Id = $"https://matteofabbri.org/read/{art.Link}",
                        Published = new DateTimeOffset(art.DateTime)
                    };

                    item.AddCategory(new SyndicationCategory(art.Category));
                    item.AddContributor(new SyndicationPerson("Matteo Fabbri", "matteo@phascode.org"));

                    await writer.Write(item);
                }

                xmlWriter.Flush();
            }
            return Content(sw.ToString(), "application/rss+xml");
        }


        [Route("robots.txt")]
        public ActionResult Robots()
        {
            return this.Content("#First useless line to get rid of BOM\r\nUser-Agent: *\r\nDisallow:");
        }

        [Route("sitemap")]
        public ActionResult Sitemap()
        {
            string baseUrl = "https://matteofabbri.org/";

            // get a list of published articles
            var posts = BlogArticle.Queryable();

            // get last modified date of the home page
            var siteMapBuilder = new SitemapBuilder();

            // add the home page to the sitemap
            siteMapBuilder.AddUrl(baseUrl, modified: DateTime.UtcNow, changeFrequency: ChangeFrequency.Weekly, priority: 1.0);

            // add the blog posts to the sitemap
            foreach (var post in posts)
            {
                siteMapBuilder.AddUrl($"{baseUrl}read/{post.Link}", modified: post.DateTime, changeFrequency: null, priority: 0.9);
            }

            siteMapBuilder.AddUrl($"{baseUrl}/privacy", modified: DateTime.UtcNow, changeFrequency: ChangeFrequency.Weekly, priority: 1.0);
            siteMapBuilder.AddUrl($"{baseUrl}/stat", modified: DateTime.UtcNow, changeFrequency: ChangeFrequency.Weekly, priority: 1.0);

            // generate the sitemap xml
            string xml = siteMapBuilder.ToString();
            return Content(xml, "text/xml");
        }
    }
}
