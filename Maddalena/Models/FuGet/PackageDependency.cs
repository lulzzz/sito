using System.Xml.Linq;

namespace Maddalena.Models.FuGet
{
    public class PackageDependency
    {
        public PackageDependency()
        {
        }

        public PackageDependency(XElement element)
        {
            PackageId = element.Attribute("id").Value;
            VersionSpec = element.Attribute("version")?.Value ?? "0";
        }

        public string PackageId { get; set; } = "";
        public string VersionSpec { get; set; } = "";
    }
}