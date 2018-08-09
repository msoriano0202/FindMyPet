﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FindMyPet.MVC.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [MaxLength(30)]
        [Required(ErrorMessage = "Correo es requerido.")]
        [Display(Name = "Correo")]
        [EmailAddress(ErrorMessage = "Por favor, ingrese un correo valido.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [MaxLength(20)]
        [Required(ErrorMessage = "Contraseña es requerida.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "Recordarme?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [MaxLength(20)]
        [Required(ErrorMessage="Nombre es requerido.")]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [MaxLength(20)]
        [Required(ErrorMessage = "Apellido es requerido.")]
        [Display(Name = "Apellido")]
        public string LastName { get; set; }

        [MaxLength(30)]
        [Required(ErrorMessage = "Correo es requerido.")]
        [EmailAddress(ErrorMessage = "Correo no valido.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo")]
        public string Email { get; set; }

        [MaxLength(20)]
        [Required(ErrorMessage = "Contraseña es requerida.")]
        [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [MaxLength(20)]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña no coincide con confirmar contraseña.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
