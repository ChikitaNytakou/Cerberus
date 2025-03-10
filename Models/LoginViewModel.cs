﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace ByeBye.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Логин (никнейм)")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }
}