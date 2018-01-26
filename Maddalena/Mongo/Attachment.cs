using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.StaticFiles;
using MongoDB.Bson;

namespace Maddalena.Mongo
{
    public class Attachment
    {
        static readonly FileExtensionContentTypeProvider MimeProvider =  new FileExtensionContentTypeProvider();

        public Attachment()
        {
            
        }

        public Attachment(string parentId, string name, string filename, Stream stream)
        {
            Name = name;
            FileName = filename;
            Mime = MimeProvider.TryGetContentType(filename, out string type) ? type : "octect/stream";
            GridName = $"{parentId}{DateTime.Now:MM.dd.yyyy.HH:mm:ss.fff}{filename}";

            Id = GridFS.Bucket.UploadFromStream(GridName, stream);
        }

        public ObjectId Id { get; set; }

        public string Name { get; set; }

        public string FileName { get; set; }

        public string Mime { get; set; }

        public string GridName { get; set; }

        public Task Download(Stream stream) => GridFS.Bucket.DownloadToStreamAsync(Id, stream);

        public Task Delete() => GridFS.Bucket.DeleteAsync(Id);


    }
}
