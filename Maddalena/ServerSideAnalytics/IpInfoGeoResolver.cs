using System.Net;
using ServerSideAnalytics;

namespace Maddalena.ServerSideAnalytics
{
    public class IpInfoGeoResolver : IGeoIpResolver
    {
        public CountryCode GetCountry(IPAddress address) => Extensions.HttpContexExtensions.GetCountry(address);
    }
}
