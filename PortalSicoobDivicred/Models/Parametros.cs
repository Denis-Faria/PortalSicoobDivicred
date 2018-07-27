using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PortalSicoobDivicred.Models
{
    public class Parametros
    {



        
        public int idDescricaoGrupo { get; set; }
        [Required(ErrorMessage = "Favor informar o Grupo!")]
        public List<SelectListItem> DescricaoGrupo { get; set; }


        public int idPermissaoDescricaoGrupo { get; set; }
        [Required(ErrorMessage = "Favor informar o Grupo!")]
        public List<SelectListItem> PermissaoDescricaoGrupo { get; set; }


        public int idPermissao { get; set; }
        [Required(ErrorMessage = "Favor informar a Permiss�o!")]
        public List<SelectListItem> IdPermissaoDescricao { get; set; }



        public int id { get; set;}

        [Required(ErrorMessage = "Favor informar o seu nome!")]
        public string NomeFuncionario { get; set; }

        [Required(ErrorMessage = "Favor informar o seu PA!")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Informe apenas n�meros")]
        public int Pa { get; set; }

        [Required(ErrorMessage = "Favor informar a data de Admiss�o")]
        public string dataAdmissao { get; set; }

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


        //   [Required(ErrorMessage = "Informe se o funcion�rio � gestor")]
        [Required(ErrorMessage = "Favor informar o seu nome!")]
        public string Gestor { get; set; }

        [Required(ErrorMessage = "Informe se o funcion�rio � estagi�rio")]
        public string Estagiario { get; set; }

        [Required(ErrorMessage = "Informe se o n�mero da matr�cula")]
        public string Matricula { get; set; }

        

        [Required(ErrorMessage = "Favor informar a descri��o do Grupo!")]
        public string DescricaoGrupos{ get; set; }


    }
}