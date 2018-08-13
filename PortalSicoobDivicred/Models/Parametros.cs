using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

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

        public string descricaoPermissao { get; set; }


        public int idPermissao { get; set; }
        [Required(ErrorMessage = "Favor informar a Permiss�o!")]
        public List<SelectListItem> IdPermissaoDescricao { get; set; }



        public int id { get; set;}

        [Required(ErrorMessage = "Favor informar o seu nome!")]
        public string NomeFuncionario { get; set; }
        public List<SelectListItem> FuncionariosNome { get; set; }


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

        [Required(ErrorMessage = "Informe se o funcionário é estagiário")]
        public string Estagiario { get; set; }

        [Required(ErrorMessage = "Informe se o número da matrícula")]
        public string Matricula { get; set; }

        

        [Required(ErrorMessage = "Favor informar a descrição do Grupo!")]
        public string DescricaoGrupos{ get; set; }


        public int IdPermissaoPermissao { get; set; }
        public List<SelectListItem> Permissao { get; set; }
        
        public string Permitido { get; set; }

        public int idPermissaoFuncionario { get; set; }
        public List<SelectListItem> PermissaoFuncionario { get; set; }

        public int idTarefa { get; set; }
        [Required(ErrorMessage = "Favor informar a Tarefa!")]
        public List<SelectListItem> DescricaoTarefas { get; set; }
        public string DescricaoTarefa { get; set; }


        public int idSubtarefas { get; set; }

        [Required(ErrorMessage = "Favor inserir a descrição")]
        public string SubtarefasDescricao { get; set; }

        public int idTarefaSubtarefas { get; set; }
      
        //  public int idTarefaDescricaoSubtarefas { get; set; }
       // public List<SelectListItem> DescricaoSubtarefas { get; set; }

        public int idFunRespSubtarefas { get; set; }

        

        public string multiploatendenteSubtarefas { get; set; }

        [Required(ErrorMessage = "Favor inserir um tempo da subtarefa")]
        public string TempoSubTarefa { get; set; }

        public string MultiploAtendente { get; set; }



    }
}