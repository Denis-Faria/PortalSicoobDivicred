using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortalSicoobDivicred.Models
{
    public class Setor
    {

        [Required(ErrorMessage = "Favor inserir um nome para este setor!")]
        public string NomeSetor { get; set; }
    }
}