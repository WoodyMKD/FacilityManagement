using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.Web.Areas.Identity
{
    public class EmployeeUser: IdentityUser
    {
        [PersonalData]
        public string Name { get; set; }
        public string Position { get; set; }
        public string ThumbnailUrl { get; set; }
    }
}
