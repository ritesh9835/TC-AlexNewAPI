using DataContracts.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TazzerClean.Util;

namespace ServiceContracts.Common
{
    public interface ICommonManager
    {
        Task<List<PostalLookup>> FindSuburb(string code);
        Task<DashboardStats> GetDashboardStats();
        Task<string> FileUpload(BucketFolder bucketFolder, IFormFile file);
    }
}
