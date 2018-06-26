using System;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

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

        class record
        {
            public string ip;
            public string city;
            public string region;
            public string country;
            public string loc;
            public string postal;
            public string phone;
            public string org;
        }

        public static CountryCode GetCountry(string ipAddress)
        {
            try
            {
                var obj = JsonConvert.DeserializeObject<record>((new WebClient()).DownloadString($"https://ipinfo.io/{ipAddress}/json"));
                return (CountryCode)Enum.Parse(typeof(CountryCode), obj.country);
            }
            catch (Exception e)
            {
                return CountryCode.World;
            }
        }

        public static CountryCode GetCountry(IPAddress address)
        {
            try
            {
                var ipAddress = address.ToString();
                var obj = JsonConvert.DeserializeObject<record>((new WebClient()).DownloadString($"https://ipinfo.io/{ipAddress}/json"));
                return (CountryCode)Enum.Parse(typeof(CountryCode), obj.country);
            }
            catch (Exception e)
            {
                return CountryCode.World;
            }
        }

        public static CountryCode GetCountry(this HttpContext context)
        {
            try
            {
                var ipAddress = context.Connection.RemoteIpAddress.ToString();
                var obj = JsonConvert.DeserializeObject<record>((new WebClient()).DownloadString($"https://ipinfo.io/{ipAddress}/json"));
                return (CountryCode)Enum.Parse(typeof(CountryCode), obj.country);
            }
            catch (Exception)
            {
                return CountryCode.World;
            }
        }

        public static bool IsEEuropean(this HttpContext context)
        {
            return Europe.Contains(GetCountry(context));
        }
    }
}