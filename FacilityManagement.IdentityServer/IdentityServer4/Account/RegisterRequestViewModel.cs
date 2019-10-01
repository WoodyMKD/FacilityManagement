using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Account
{
    public class RegisterRequestViewModel
    {
        [Required(ErrorMessage = "Ова поле е задолжително.")]
        [StringLength(50, ErrorMessage = "Ова поле мора да биде најмалку {2} и најмногу {1} карактери долго.", MinimumLength = 2)]
        [Display(Name = "Име и презиме")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Ова поле е задолжително.")]
        [EmailAddress(ErrorMessage = "Е-поштата не е валидна.")]
        [Display(Name = "Е-пошта")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Ова поле е задолжително.")]
        [StringLength(50, ErrorMessage = "Ова поле мора да биде најмалку {2} и најмногу {1} карактери долго.", MinimumLength = 3)]
        [Display(Name = "Работна позиција")]
        public string Position { get; set; }

        [Required(ErrorMessage = "Ова поле е задолжително.")]
        [StringLength(100, ErrorMessage = "Ова поле мора да биде најмалку {2} и најмногу {1} карактери долго.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Лозинка")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Потврди лозинка")]
        [Compare("Password", ErrorMessage = "Внесените лозинки не се совпаѓаат.")]
        public string ConfirmPassword { get; set; }
    }
}
