using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PortalSicoobDivicred.Models
{
    public class Pgd
    {
        public int IdProduto { get; set; }

        [Required(ErrorMessage = "Favor informar o Produto!")]
        public List<SelectListItem> DescricaoProduto { get; set; }

        public List<SelectListItem> Pontuacao { get; set; }
        public int IdFuncionario { get; set; }
        public List<SelectListItem> NomeFuncionario { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "Favor informar a data da Contratação!")]
        public DateTime Datacontratacao { get; set; }

        public int Produto { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "Favor informar o cpf do cliente!")]
        public string Cpf { get; set; }

        public string Observacao { get; set; }
        public string Excluido { get; set; }
        public string Login { get; set; }
        public double Valor { get; set; }
        public double Valorponto { get; set; }
    }
}