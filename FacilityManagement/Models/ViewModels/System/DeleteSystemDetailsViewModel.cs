using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.Web.Models.ViewModels
{
    public class DeleteSystemDetailsViewModel
    {
        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "System")]
        public int InventoryObjectSystemId { get; set; }
        public int InventoryObjectTypeId { get; set; }

        public ICollection<SystemDetailsViewModel> AllSystems { get; set; }
    }
}
