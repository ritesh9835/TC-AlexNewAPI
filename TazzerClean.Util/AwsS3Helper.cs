using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TazzerClean.Util
{
    public class AwsS3Helper
    {
        protected readonly IConfiguration Configuration;
        public AwsS3Helper(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<string> UploadFileToS3(IFormFile file, List<string> folders)
        {
            StringBuilder folderPath = new StringBuilder();
            foreach (var val in folders)
                folderPath.Append(val + "/");

            using (var client = new AmazonS3Client(Configuration.GetValue<string>("S3Details:AccessId"),Configuration.GetValue<string>("S3Details:Key"), RegionEndpoint.EUWest2))
            {
                using (var newMemoryStream = new MemoryStream())
                {
                    file.CopyTo(newMemoryStream);

                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = newMemoryStream,
                        Key = folderPath.ToString() + file.FileName,
                        BucketName = Configuration.GetValue<string>("S3Details:BucketName"),
                        CannedACL = S3CannedACL.PublicRead
                    };

                    var fileTransferUtility = new TransferUtility(client);
                    await fileTransferUtility.UploadAsync(uploadRequest);
                }
            }

            return folderPath + file.FileName;
        }
    }
}
