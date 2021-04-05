using DataAccess.Database.Interfaces;
using DataContracts.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TazzerClean.Util;

namespace ServiceContracts.Common
{
    public class CommonManager : ICommonManager
    {
        private readonly IConfiguration Configuration;
        private readonly ICommonRepository _commonRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly string suburbByCode = "suburbByCode";

        public CommonManager(ICommonRepository commonRepository,IMemoryCache memoryCache, IConfiguration configuration)
        {
            _commonRepository = commonRepository;
            _memoryCache = memoryCache;
            Configuration = configuration;
        }

        public async Task<string> FileUpload(BucketFolder bucketFolder, IFormFile file)
        {
            if(file!= null)
            {
                AwsS3Helper s3 = new AwsS3Helper(Configuration);

                List<string> path = new List<string>
                {
                    Enum.GetName(typeof(BucketFolder),bucketFolder),
                    DateTime.Now.Year.ToString(),
                    DateTime.Now.Day.ToString()
                };

                var s3path = await s3.UploadFileToS3(file, path);
                return "https://" + Configuration.GetValue<string>("S3Details:BucketName") + "." + Configuration.GetValue<string>("S3Details:BucketUrl") + "/" + s3path;

            }
            return string.Empty;
        }

        public async Task<List<PostalLookup>> FindSuburb(string code)
        {
            var query = new List<PostalLookup>();

            if (!_memoryCache.TryGetValue(suburbByCode + $"{code}", out List<PostalLookup> cacheResult))
            {
                query = await _commonRepository.FindSuburbAsync(code);

                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(60));

                // Save data in cache.
                _memoryCache.Set(suburbByCode + $"{code}", query, cacheEntryOptions);
                query = cacheResult;
            }
            else
            {
                query = cacheResult;
            }


            return query;
        }

        public async Task<DashboardStats> GetDashboardStats()
        {
            return await _commonRepository.GetDashboardStats();
        }
    }
}
