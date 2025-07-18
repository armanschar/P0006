using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace P0006.Models.ViewModels
{
    public class QueryViewModels
    {
        public int _Id { get; set; }
        public string _Email { get; set; }
        public int? _Edad { get; set; }

    }
    public class AddUserViewModels
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre del usuario")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El usuario es obligatorio")]
        [Display(Name = "Usuario")]
        public string Usuario { get; set; }
        [Required(ErrorMessage = "El email es obligatorio")]
        [Display(Name = "Correo")]
        public string Email { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "La confirmación es obligatoria")]
        [Compare("Password", ErrorMessage = "No coinciden los passwords")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirma Password")]
        public string ConfirmaPassword { get; set; }
        [Required(ErrorMessage = "La edad es obligatoria")]
        [Display(Name = "Edad")]
        public int Edad { get; set; }

    }
    public class EditUserViewModels
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El usuario es obligatorio")]
        [Display(Name = "Usuario")]
        public string Usuario { get; set; }
        [Required]
        [Display(Name = "Nombre de usuario")]
        public string Nombre { get; set; }
        [Required]
        [Display(Name = "Correo")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "No coinciden los passwords")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirma Password")]
        public string ConfirmaPassword { get; set; }
        [Required]
        [Display(Name = "Edad")]
        public int? Edad { get; set; }
    }
}