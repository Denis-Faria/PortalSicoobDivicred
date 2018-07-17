using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PortalSicoobDivicred.Models
{
    public class Formulario
    {
        [Required( ErrorMessage = "Favor inserir um nome para este campo!" )]
        public string NomeCampo { get; set; }

        [Required( ErrorMessage = "Favor selecionar a categoria deste campo !" )]
        public int IdCategoria { get; set; }

        [DisplayName( "Descrição Categoria" )] public List<SelectListItem> DescricaoCategoria { get; set; }

        [Required( ErrorMessage = "Favor informar se o campo é obrigatório !" )]
        public string CampoObrigatorio { get; set; }

        [Required( ErrorMessage = "Favor informar se o campo faz parte de um combo !" )]
        public string Combo { get; set; }

        [Required( ErrorMessage = "Favor inserir o nome do combo!" )]
        public string NomeCombo { get; set; }

    }
}