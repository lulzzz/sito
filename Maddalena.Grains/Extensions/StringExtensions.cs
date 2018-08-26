using HtmlAgilityPack;

namespace Maddalena.Grains.Extensions
{
    static class StringExtensions
    {
        public static string PurgeHtml(this string str)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(str);

            return doc.DocumentNode.InnerText;
        }
    }
}
