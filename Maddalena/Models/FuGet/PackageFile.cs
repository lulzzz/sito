using System.IO.Compression;

namespace Maddalena.Models.FuGet
{
    public class PackageFile
    {
        public PackageFile(ZipArchiveEntry entry)
        {
            ArchiveEntry = entry;
        }

        public ZipArchiveEntry ArchiveEntry { get; }
        public string FileName => ArchiveEntry?.Name;
        public long SizeInBytes => ArchiveEntry?.Length ?? 0;
    }
}