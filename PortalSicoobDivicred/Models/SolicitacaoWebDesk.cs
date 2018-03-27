using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortalSicoobDivicred.Models
{
    public class SolicitacaoWebDesk
    {
        [DisplayName("Setor")]
        public List<SelectListItem> SetorResponsavel { get; set; }

        [Required(ErrorMessage = "Favor informar o setor que irá atender está solicitação!")]
        [DisplayName("IdSetor")]
        public int IdSetorResponsavel { get; set; }

        [DisplayName("Funcionário Responsavel")]
        public List<SelectListItem> FuncionarioResponsavel { get; set; }

        [Required(ErrorMessage = "Favor informar o funcionário que irá atender está solicitação!")]
        [DisplayName("IdSetor")]
        public int IdFuncionarioResponsavel { get; set; }

        [DisplayName("Categoria Solicitação")]
        public List<SelectListItem> Categoria { get; set; }

        [Required(ErrorMessage = "Favor informar a categoria desta solicitação!")]
        [DisplayName("IdSetor")]
        public int IdCategoria { get; set;}

        [Required(ErrorMessage = "Favor informar o conteúdo desta solicitação !")]
        public string Descricao { get; set; }


    }
}