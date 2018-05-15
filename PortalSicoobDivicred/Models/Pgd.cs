using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PortalSicoobDivicred.Models
{
    public class Pgd
    {
        public int idProduto { get; set; }

        [Required(ErrorMessage = "Favor informar o Produto!")]
        public List<SelectListItem> descricaoProduto { get; set; }

        public List<SelectListItem> pontuacao { get; set; }
        public int idFuncionario { get; set; }
        public List<SelectListItem> nomeFuncionario { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "Favor informar a data da Contratação!")]
        public DateTime datacontratacao { get; set; }

        public int produto { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "Favor informar o cpf do cliente!")]
        public string cpf { get; set; }

        public string observacao { get; set; }
        public string excluido { get; set; }
        public string Login { get; set; }
        public double valor { get; set; }
        public double valorponto { get; set; }
    }
}