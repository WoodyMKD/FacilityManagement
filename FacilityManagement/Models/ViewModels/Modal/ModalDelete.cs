using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.Web.Models.ViewModels.Modal
{
    public class ModalDelete
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public int ModelId { get; set; }
        public string Message { get; set; }
    }
}
