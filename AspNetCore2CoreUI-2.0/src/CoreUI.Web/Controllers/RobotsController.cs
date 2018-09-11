using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Maddalena.Core;
using Maddalena.Core.Blog;
using Maddalena.Core.Identity;
using Maddalena.Core.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Atom;
using Microsoft.SyndicationFeed.Rss;

namespace CoreUI.Web.Controllers
{
    public class RobotsController : Controller
    {
        private readonly IBlogService _blog;
        private readonly ISettingsService _settings;

        public RobotsController(IBlogService blog, ISettingsService settings)
        {
            _blog = blog;
            _settings = settings;
        }

        private SiteSettings SiteSettings => _settings.Get<SiteSettings>();

        [Route("/robots.txt")]
        public string RobotsTxt()
        {
            var sb = new StringBuilder();
            sb.AppendLine("User-agent: *");
            sb.AppendLine("Disallow:");
            sb.AppendLine($"sitemap: {Request.Scheme}://{Request.Host}/sitemap.xml");

            return sb.ToString();
        }

        [Route("/sitemap.xml")]
        public async Task SitemapXml()
        {
            Response.ContentType = "application/xml";

            using (var xml = XmlWriter.Create(Response.Body, new XmlWriterSettings { Indent = true }))
            {
                xml.WriteStartDocument();
                xml.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

                var posts = await _blog.GetPosts(int.MaxValue);

                foreach (var post in posts)
                {
                    var lastMod = new[] { post.PubDate, post.LastModified };

                    xml.WriteStartElement("url");
                    xml.WriteElementString("loc", $"{Request.Scheme}://{Request.Host}/read/{post.Slug}");
                    xml.WriteElementString("lastmod", lastMod.Max().ToString("yyyy-MM-ddThh:mmzzz"));
                    xml.WriteEndElement();
                }

                xml.WriteEndElement();
            }
        }

        [Route("/rsd.xml")]
        public void RsdXml()
        {
            Response.ContentType = "application/xml";
            Response.Headers["cache-control"] = "no-cache, no-store, must-revalidate";

            using (var xml = XmlWriter.Create(Response.Body, new XmlWriterSettings { Indent = true }))
            {
                xml.WriteStartDocument();
                xml.WriteStartElement("rsd");
                xml.WriteAttributeString("version", "1.0");

                xml.WriteStartElement("service");

                xml.WriteElementString("enginename", "Miniblog.Core");
                xml.WriteElementString("enginelink", "http://github.com/madskristensen/Miniblog.Core/");
                xml.WriteElementString("homepagelink", $"{Request.Scheme}://{Request.Host}");

                xml.WriteStartElement("apis");
                xml.WriteStartElement("api");
                xml.WriteAttributeString("name", "MetaWeblog");
                xml.WriteAttributeString("preferred", "true");
                xml.WriteAttributeString("apilink",$"{Request.Scheme}://{Request.Host}/metaweblog");
                xml.WriteAttributeString("blogid", "1");

                xml.WriteEndElement(); // api
                xml.WriteEndElement(); // apis
                xml.WriteEndElement(); // service
                xml.WriteEndElement(); // rsd
            }
        }

        [Route("/feed/{type}")]
        public async Task Rss(string type)
        {
            Response.ContentType = "application/xml";

            using (var xmlWriter = XmlWriter.Create(Response.Body, new XmlWriterSettings() { Async = true, Indent = true, Encoding = new UTF8Encoding(false) }))
            {
                var posts = await _blog.GetPosts(10);
                var writer = await GetWriter(type, xmlWriter, posts.Max(p => p.PubDate));

                foreach (var post in posts)
                {
                    var item = new AtomEntry
                    {
                        Title = post.Title,
                        Description = post.Content,
                        Id = $"{Request.Scheme}://{Request.Host}/read/{post.Slug}",
                        Published = post.PubDate,
                        LastUpdated = post.LastModified,
                        ContentType = "html",
                    };

                    item.AddCategory(new SyndicationCategory(post.Category));

                    item.AddContributor(new SyndicationPerson("test@example.com", SiteSettings.Owner));
                    item.AddLink(new SyndicationLink(new Uri(item.Id)));

                    await writer.Write(item);
                }
            }
        }

        private async Task<ISyndicationFeedWriter> GetWriter(string type, XmlWriter xmlWriter, DateTime updated)
        {
            if (type.Equals("rss", StringComparison.OrdinalIgnoreCase))
            {
                var rss = new RssFeedWriter(xmlWriter);
                await rss.WriteTitle(SiteSettings.Name);
                await rss.WriteDescription(SiteSettings.Description);
                await rss.WriteGenerator("Miniblog.Core");
                await rss.WriteValue("link", $"{Request.Scheme}://{Request.Host}");
                return rss;
            }

            var atom = new AtomFeedWriter(xmlWriter);
            await atom.WriteTitle(SiteSettings.Name);
            await atom.WriteId($"{Request.Scheme}://{Request.Host}");
            await atom.WriteSubtitle(SiteSettings.Description);
            await atom.WriteGenerator("Miniblog.Core", "https://github.com/madskristensen/Miniblog.Core", "1.0");
            await atom.WriteValue("updated", updated.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            return atom;
        }
    }
}
