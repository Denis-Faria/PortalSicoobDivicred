using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortalSicoobDivicred.Models
{
    public class Funcionario
    {

        public string NomeFuncionario { get; set; }

        public string CpfFuncionario { get; set; }

        public string RgFuncionario { get; set; }

        public string PisFuncionario { get; set; }

        public string DataNascimentoFuncionario { get; set; }

        public string DescricaoSexo { get; set; }

        public string FormacaoAcademica { get; set; }

        public string UsuarioSistema { get; set; }

        public string Email { get; set; }

        public string PA { get; set; }

        public string Rua { get; set; }

        public string Numero { get; set; }

        public string Bairro { get; set; }

        public string Cidade { get; set; }

        public string Funcao { get; set; }

        public string QuatidadeFilho { get; set; }

        public string DataNascimentoFilho { get; set; }

        public string ContatoEmergencia { get; set; }

        public string PrincipaisHobbies { get; set; }

        public string ComidaFavorita { get; set; }

        public string Viagem { get; set; }

        [DisplayName("Estado Civil")]
        public List<SelectListItem> EstadoCivil { get; set; }

        [Required(ErrorMessage = "Favor informar o estado civil!")]
        [DisplayName("IdEstadoCivil")]
        public int IdEstadoCivil { get; set; }


        [DisplayName("Sexo")]
        public List<SelectListItem> Sexo { get; set; }

        [Required(ErrorMessage = "Favor informar o seu sexo!")]
        [DisplayName("IdSexo")]
        public int IdSexo { get; set; }

        [DisplayName("Formação")]
        public List<SelectListItem> Formacao { get; set; }

        [Required(ErrorMessage = "Favor informar a sua Formação!")]
        [DisplayName("IdFormacao")]
        public int IdFormacao { get; set; }

        [DisplayName("Etnia/Raça")]
        public List<SelectListItem> Etnia { get; set; }

        [Required(ErrorMessage = "Favor informar a sua etnia!")]
        [DisplayName("IdEtnia")]
        public int IdEtnia { get; set; }

        [DisplayName("Setor")]
        public List<SelectListItem> Setor { get; set; }

        [Required(ErrorMessage = "Favor informar o seu Setor!")]
        [DisplayName("IdSetor")]
        public int IdSetor { get; set; }
    }
}