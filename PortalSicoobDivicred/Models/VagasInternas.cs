using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortalSicoobDivicred.Models
{
    public class VagasInternas
    {
        [Required(ErrorMessage = "Favor inserir o titulo da vaga!")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "Favor inserir a descrição da vaga!")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Favor inserir os requisitos da vaga!")]
        public string Requisitos { get; set; }

    }
}