using HtmlAgilityPack;

namespace Maddalena.ML.Extensions
{
    internal static class StringExtensions
    {
        public static string PurgeHtml(this string str)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(str);
            return doc.DocumentNode.InnerText;
        }
    }
}