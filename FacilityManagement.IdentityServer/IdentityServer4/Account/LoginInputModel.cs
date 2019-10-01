// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.ComponentModel.DataAnnotations;

namespace IdentityServer4.Account
{
    public class LoginInputModel
    {
        [Required(ErrorMessage = "Ова поле е задолжително.")]
        [Display(Name = "Е-пошта")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Ова поле е задолжително.")]
        [Display(Name = "Лозинка")]
        public string Password { get; set; }
        [Display(Name = "Запомни ме")]
        public bool RememberLogin { get; set; }
        public string ReturnUrl { get; set; }
    }
}