using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortalSicoobDivicred.Models
{
    public class Tarefa
    {
        [Required( ErrorMessage = "Favor inserir um nome para esta função!" )]
        public int IdTarefa { get; set; }

        public List<SelectListItem> DescricaoTarefa { get; set; }

        [Required( ErrorMessage = "Favor selecionar uma subtarefa!" )]
        public int IdSubTarefa { get; set; }

        public List<SelectListItem> DescricaoSubTarefa { get; set; }

        [Required( ErrorMessage = "Favor inserir alguma informação complementar!" )]
        public string InformacoesComplementares { get; set; }


    }
}