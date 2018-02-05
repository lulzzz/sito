using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Maddalena.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;

namespace Maddalena.Mongo
{
    public class UploadFile : DBObject<UploadFile>
    {
        static readonly FileExtensionContentTypeProvider MimeProvider =  new FileExtensionContentTypeProvider();

        public UploadFile()
        {
            
        }

        public static async Task<UploadFile> Create(IFormFile file, ClaimsPrincipal claimPrincipal, ACL acl = null)
        {
            var upload = new UploadFile
            {
                ContentDisposition = file.ContentDisposition,
                ContentType = file.ContentType,
                FileName = file.FileName,
                Length = file.Length,
                Name = file.Name,
                Uploader = await claimPrincipal.ToUser(),
                DateTime = DateTimeOffset.Now,
                ACL = acl ?? new ACL { Public = true }
            };

            var gridName = $"{Guid.NewGuid():N}{upload.DateTime.Ticks}{upload.FileName}";

            await GridFS.Bucket.UploadFromStreamAsync(gridName, file.OpenReadStream());

            upload.GridName = gridName;
            await CreateAsync(upload);
            return upload;
        }

        public string GridName { get; set; }

        public ObjectRef<ApplicationUser> Uploader { get; set; }
        
        public DateTimeOffset DateTime { get; set; }

        public string ContentType { get; set; }

        public string ContentDisposition { get; set; }

        public long Length { get; set; }

        public string Name { get; set; }

        public string FileName { get; set; }

        public ACL ACL { get; set; }

        public Task<GridFSDownloadStream<ObjectId>> Download() => GridFS.Bucket.OpenDownloadStreamByNameAsync(GridName);

        public Task Delete() => GridFS.Bucket.DeleteAsync(Id);
    }
}
