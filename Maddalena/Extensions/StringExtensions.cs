using HtmlAgilityPack;

namespace Maddalena.Extensions
{
    public static class StringExtensions
    {
        public static string CleanHtml(this string str)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(str);

            return doc.DocumentNode.InnerText;
        }
    }
}
