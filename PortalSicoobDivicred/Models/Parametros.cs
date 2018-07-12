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
        [RegularExpression("^[0-9]*$", ErrorMessage = "Informe apenas n�meros")]
        public string Pa { get; set; }

        [Required(ErrorMessage = "Favor informar a data de Admiss�o")]
        public DateTime dataAdmissao { get; set; }

        [Required(ErrorMessage = "Favor informar o seu CPF!")]
        public string CpfFuncionario { get; set; }

        [Required(ErrorMessage = "Favor informar a sua identidade!")]
        public string RgFuncionario { get; set; }

        [Required(ErrorMessage = "Favor informar o seu PIS!")]
        public string PisFuncionario { get; set; }

        [Required(ErrorMessage = "Favor informar Login do funcion�rio")]
        public string LoginFuncionario { get; set; }


        [Required(ErrorMessage = "Favor informar o seu e-mail!")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Informe se o funcion�rio � gestor")]
        public string Gestor { get; set; }

        [Required(ErrorMessage = "Informe se o funcion�rio � estagi�rio")]
        public string Estagiario { get; set; }


    }
}