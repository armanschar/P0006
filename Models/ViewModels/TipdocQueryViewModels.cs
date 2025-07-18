using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace P0006.Models.ViewModels
{
    public class TipdocQueryViewModels
    {
        public int Id { get; set; }
        public string Tipodoc { get; set; }
        public string Descripcion { get; set; }
        public string Origen { get; set; }
    }

    public class TipdocAddViewModels
    {
        [Required(ErrorMessage = "El tipo de documento es obligatorio")]
        [Display(Name = "Tipo de Documento")]
        public string Tipodoc { get; set; }

        [Required(ErrorMessage = "La descripcion es obligatorio")]
        [Display(Name = "Descripcion")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El origen es obligatorio")]
        [Display(Name = "Origen Debito / Credito")]
        public string Origen { get; set; }

        [Required(ErrorMessage = "La cuenta de débito es obligatoria")]
        [Display(Name = "Cuenta Contable Débito")]
        public string CtaDebito { get; set; }

        [Required(ErrorMessage = "La cuenta de crédito es obligatoria")]
        [Display(Name = "Cuenta Contable Crédito")]
        public string CtaCredito { get; set; }

    }
    public class TipdocEditViewModels
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El tipo de documento es obligatorio")]
        [Display(Name = "Tipo de Documento")]
        public string Tipodoc { get; set; }

        [Required(ErrorMessage = "La descripcion es obligatorio")]
        [Display(Name = "Descripcion")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El origen es obligatorio")]
        [Display(Name = "Origen Debito / Credito")]
        public string Origen { get; set; }

        [Required(ErrorMessage = "La cuenta de débito es obligatoria")]
        [Display(Name = "Cuenta Contable Débito")]
        public string CtaDebito { get; set; }

        [Required(ErrorMessage = "La cuenta de crédito es obligatoria")]
        [Display(Name = "Cuenta Contable Crédito")]
        public string CtaCredito { get; set; }

    }
}