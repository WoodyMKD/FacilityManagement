using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FacilityManagement.Web.Services
{
    public interface IFacilityManagementHttpClient
    {
        Task<HttpClient> GetClient();
    }
}
