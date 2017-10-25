using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortalSicoobDivicred.Models
{
    public class RecrutamentoSelecao
    {

        public string ClasseCargo { get; set; }

        public string NivelCargo { get; set; }

        [Required(ErrorMessage = "Favor inserir a quantidade de vagas !")]
        public string NumeroVaga { get; set; }

        [Required(ErrorMessage = "Favor inserir quem solicitou!")]
        public string ChefiaImediata { get; set; }

        [Required(ErrorMessage = "Favor inserir o setor da vaga!")]
        public string Setor { get; set; }

        [Required(ErrorMessage = "Favor inserir o mês de admissão!")]
        public string MesAdmissao { get; set; }

        [Required(ErrorMessage = "Favor inserir a quantidade de pessoas selecionadas para esta etapa!")]
        public string DinamicaNumero { get; set; }

        [Required(ErrorMessage = "Favor inserir a quantidade de pessoas pre selecionadas para esta etapa!!")]
        public string DinamicaNumeroPreSelecionado { get; set; }

        [Required(ErrorMessage = "Favor inserir a descrição do teste utilizado nesta etapa!")]
        public string ConhecimentoTeste { get; set; }

        [Required(ErrorMessage = "Favor inserir a quantidade de pessoas selecionadas para esta etapa!")]
        public string ConhecimentoNumero { get; set; }

        [Required(ErrorMessage = "Favor inserir a quantidade de pessoas pre selecionadas para esta etapa!")]
        public string ConhecimentoNumeroPreSelecionado { get; set; }


        [Required(ErrorMessage = "Favor inserir a descrição do teste utilizado nesta etapa!")]
        public string PsicologicaTeste { get; set; }


        [Required(ErrorMessage = "Favor inserir a quantidade de pessoas selecionadas para esta etapa!")]
        public string PsicologicaNumero { get; set; }

        [Required(ErrorMessage = "Favor inserir a quantidade de pessoas pre selecionadas para esta etapa!")]
        public string PsicologicaNumeroPreSelecionado { get; set; }


        public string NomeEntrevistador{ get; set; }


        public string EntrevistaNumero { get; set; }

     
        public string EntrevistaNumeroPreSelecionado { get; set; }








    }
}