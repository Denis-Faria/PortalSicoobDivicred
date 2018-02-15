using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortalSicoobDivicred.Models
{
    public class Certificacao
    {
        [Required(ErrorMessage = "Favor inserir um nome para esta função!")]
        public string NomeCertificacao { get; set; }

    }
}