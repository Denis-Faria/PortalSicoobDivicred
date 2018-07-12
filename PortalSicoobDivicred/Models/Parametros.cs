using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PortalSicoobDivicred.Models
{
    public class Parametros
    {

        public int idDescricaoGrupo { get; set; }

        [Required(ErrorMessage = "Favor selecionar o grupo!")]
        public List<SelectListItem> DescricaoGrupo { get; set; }

       

        [Required(ErrorMessage = "Favor informar o seu nome!")]
        public string NomeFuncionario { get; set; }

        [Required(ErrorMessage = "Favor informar o seu PA!")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Informe apenas números")]
        public string Pa { get; set; }

        [Required(ErrorMessage = "Favor informar a data de Admissão")]
        public DateTime dataAdmissao { get; set; }

        [Required(ErrorMessage = "Favor informar o seu CPF!")]
        public string CpfFuncionario { get; set; }

        [Required(ErrorMessage = "Favor informar a sua identidade!")]
        public string RgFuncionario { get; set; }

        [Required(ErrorMessage = "Favor informar o seu PIS!")]
        public string PisFuncionario { get; set; }

        [Required(ErrorMessage = "Favor informar Login do funcionário")]
        public string LoginFuncionario { get; set; }


        [Required(ErrorMessage = "Favor informar o seu e-mail!")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Informe se o funcionário é gestor")]
        public string Gestor { get; set; }

        [Required(ErrorMessage = "Informe se o funcionário é estagiário")]
        public string Estagiario { get; set; }


    }
}