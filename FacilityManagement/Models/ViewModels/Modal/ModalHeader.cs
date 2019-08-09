using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.Web.Models.ViewModels.Modal
{
    public class ModalHeader
    {
        public string Heading { get; set; }
        public bool IsDeleteHeader { get; set; } = false;
    }
}
