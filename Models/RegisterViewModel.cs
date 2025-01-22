using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
namespace ByeBye.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Фамилия обязательна")]
        [Display(Name = "Фамилия")]
        public string SurName { get; set; }

        [Display(Name = "Имя")]
        public string? FirstName { get; set; }

        [Display(Name = "Отчество")]
        public string? FatherName { get; set; }

        [Required]
        [Display(Name = "Логин (никнейм)")]
        [Remote(action: "IsUserNameAvailable", controller: "Account")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}