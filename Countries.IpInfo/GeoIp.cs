using System;
using System.IO;
using System.Net;
using Maddalena;
using Newtonsoft.Json;

namespace Countries.IpInfo
{
    public class GeoIp
    {
        public static Country GetCountry(string ip)
        {
            dynamic obj = JsonConvert.DeserializeObject((new WebClient()).DownloadString($"https://ipinfo.io/{ip}/json"));
            Enum.Parse(typeof(CountryCode), (string) obj.country);
            return Country.FromCode()
        }
    }
}
