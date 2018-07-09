using System;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace Maddalena.Extensions
{
    public static class HttpContexExtensions
    {
        static CountryCode[] Europe;

        static HttpContexExtensions()
        {
            Europe = new[]
            {
                Country.Austria,
                Country.Belgium,
                Country.Bulgaria,
                Country.Croatia,
                Country.Cyprus,
                Country.CzechRepublic,
                Country.Denmark,
                Country.Estonia,
                Country.Finland,
                Country.France,
                Country.Germany,
                Country.Greenland,
                Country.Greece,
                Country.Hungary,
                Country.Ireland,
                Country.Iceland,
                Country.Italy,
                Country.Latvia,
                Country.Lithuania,
                Country.Luxembourg,
                Country.Malta,
                Country.Netherlands,
                Country.Poland,
                Country.Portugal,
                Country.Romania,
                Country.Slovakia,
                Country.Slovenia,
                Country.Spain,
                Country.Sweden,
                //remove this line soon
                Country.UnitedKingdom
            }
            .Select(x => x.CountryCode)
            .ToArray();
        }

        public static async Task<CountryCode> ResolveCountryCodeAsync(IPAddress address)
        {
            try
            {
                var accessKey = "8627be1fa0273d3f9422440a803803c6";
                var ipstr = address.ToString();
                var response = await (new HttpClient()).GetStringAsync($"http://api.ipstack.com/{ipstr}?access_key={accessKey}&format=1");

                var obj = JsonConvert.DeserializeObject(response) as JObject;
                return (CountryCode)Enum.Parse(typeof(CountryCode), obj["country_code"].ToString());
            }
            catch (Exception)
            {
            }
            return CountryCode.World;
        }

        public static string GetCountry(this HttpContext context)
        {
            var task = ResolveCountryCodeAsync(context.Connection.RemoteIpAddress);
            task.Wait();
            return Country.FromCode(task.Result).CommonName;
        }

        public static bool IsEEuropean(this HttpContext context)
        {
            var task = ResolveCountryCodeAsync(context.Connection.RemoteIpAddress);
            task.Wait();
            return Europe.Contains(task.Result);
        }
    }
}