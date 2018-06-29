﻿using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Maddalena.Models.FuGet.Extensions
{
    static class UrlExtensions
    {
        public static Task<string> GetTextFileAsync (string url)
        {
            var rawUrl = GetRawTextUrl (url);
            var client = new HttpClient ();
            //Debug.WriteLine ("GET " + rawUrl);
            return client.GetStringAsync (rawUrl);
        }

        static readonly (Regex, string)[] rawTextUrlTransforms = {
            (new Regex ("https://github.com/([^/]+)/([^/]+)/blob/master/([^/]+)", RegexOptions.Compiled),
             "https://raw.githubusercontent.com/$1/$2/master/$3")
        };

        public static string GetRawTextUrl (string url)
        {
            foreach (var (regex, repl) in rawTextUrlTransforms) {
                var m = regex.Match (url);
                if (m.Success) {
                    return regex.Replace (url, repl);
                }
            }
            return url;
        }
    }
}
