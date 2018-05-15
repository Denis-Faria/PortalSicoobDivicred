using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PortalSicoobDivicred.Models
{
    public class Funcionario
    {
        [Required(ErrorMessage = "Favor informar o seu nome!")]
        public string NomeFuncionario { get; set; }

        [Required(ErrorMessage = "Favor informar o seu CPF!")]
        public string CpfFuncionario { get; set; }

        [Required(ErrorMessage = "Favor informar a sua identidade!")]
        public string RgFuncionario { get; set; }

        [Required(ErrorMessage = "Favor informar o seu PIS!")]
        public string PisFuncionario { get; set; }

        [Required(ErrorMessage = "Favor informar a sua data de nascimento!")]
        public string DataNascimentoFuncionario { get; set; }

        public string DescricaoSexo { get; set; }

        [Required(ErrorMessage = "Favor informar sua formacao acadêmica!")]
        public string FormacaoAcademica { get; set; }

        [Required(ErrorMessage = "Favor informar o seu usuário!")]
        public string UsuarioSistema { get; set; }

        [Required(ErrorMessage = "Favor informar o seu e-mail!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Favor informar o seu PA!")]
        public string PA { get; set; }

        [Required(ErrorMessage = "Favor informar sua Nacionalidade!")]
        public string Nacionalidade { get; set; }

        [Required(ErrorMessage = "Favor informar o nome da mãe!")]
        public string NomeMae { get; set; }

        [Required(ErrorMessage = "Favor informar o nome do pai")]
        public string NomePai { get; set; }

        [Required(ErrorMessage = "Favor informar o local de nascimento")]
        public string LocalNascimento { get; set; }

        [Required(ErrorMessage = "Favor informar o estado de nascimento")]
        [MaxLength(2,ErrorMessage = "Coloque a abreviação de seu estado")]
        public string UfNascimento { get; set; }
       
        public string Complemento { get; set; }

        [Required(ErrorMessage = "Favor informar o CEP")]
        public string Cep { get; set; }

        [Required(ErrorMessage = "Favor informar o nome do país")]
        public string Pais { get; set; }

        [Required(ErrorMessage = "Favor informar se sua residência é própria")]
        public string ResidenciaPropria { get; set; }

        [Required(ErrorMessage = "Favor informar se sua residência foi adquirida com recursos de FGTS")]
        public string RecursoFgts { get; set; }

        [Required(ErrorMessage = "Favor informar o número da sua CTPS")]
        public string NumeroCTPS{ get; set; }

        [Required(ErrorMessage = "Favor informar a serie da sua CTPS")]
        public string SerieCTPS { get; set; }

        [Required(ErrorMessage = "Favor informar o estado em que sua CTPS foi emitida")]
        public string UfCTPS { get; set; }

        [Required(ErrorMessage = "Favor informar a data de expedição do seu documento")]
        public DateTime DataExpedicaoDocumento { get; set; }

        [Required(ErrorMessage = "Favor informar o orgão emissor do seu documento")]
        public string OrgaoEmissor { get; set; }

        [Required(ErrorMessage = "Favor informar a data de validade do seu documento")]
        public DateTime DataValidadeDocumento { get; set; }

        [Required(ErrorMessage = "Favor informar o número de telefone fixo")]
        public string TelefoneFixo { get; set; }

        [Required(ErrorMessage = "Favor informar o número de telefone movel")]
        public string TelefoneCelular { get; set; }

        [Required(ErrorMessage = "Informe a sua rua!")]
        public string Rua { get; set; }

        [Required(ErrorMessage = "Informe o numero da sua casa!")]
        public string Numero { get; set; }

        [Required(ErrorMessage = "Informe o seu bairro!")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "Informe a sua cidade!")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "Favor informar se você possui filhos!")]
        public string QuatidadeFilho { get; set; }

        public string DataNascimentoFilho { get; set; }

        [Required(ErrorMessage = "Favor informar ao menos um contato de emergência!")]
        public string ContatoEmergencia { get; set; }

        [Required(ErrorMessage = "Favor informar seus hobbies!")]
        public string PrincipaisHobbies { get; set; }

        [Required(ErrorMessage = "Favor informar sua comida favorita!")]
        public string ComidaFavorita { get; set; }

        [Required(ErrorMessage = "Favor informar se você gosta de viajar!")]
        public string Viagem { get; set; }

        [DisplayName("Estado Civil")] public List<SelectListItem> EstadoCivil { get; set; }

        [Required(ErrorMessage = "Favor informar o estado civil!")]
        [DisplayName("IdEstadoCivil")]
        public int IdEstadoCivil { get; set; }


        [DisplayName("Sexo")] public List<SelectListItem> Sexo { get; set; }

        [Required(ErrorMessage = "Favor informar o seu sexo!")]
        [DisplayName("IdSexo")]
        public int IdSexo { get; set; }

        [DisplayName("Formação")] public List<SelectListItem> Formacao { get; set; }

        [Required(ErrorMessage = "Favor informar a sua Formação!")]
        [DisplayName("IdFormacao")]
        public int IdFormacao { get; set; }

        [DisplayName("Etnia/Raça")] public List<SelectListItem> Etnia { get; set; }

        [Required(ErrorMessage = "Favor informar a sua etnia!")]
        [DisplayName("IdEtnia")]
        public int IdEtnia { get; set; }

        [DisplayName("Setor")] public List<SelectListItem> Setor { get; set; }

        [Required(ErrorMessage = "Favor informar o seu Setor!")]
        [DisplayName("IdSetor")]
        public int IdSetor { get; set; }

        [DisplayName("Estado Civil")] public List<SelectListItem> Funcao { get; set; }

        [Required(ErrorMessage = "Favor informar o estado civil!")]
        [DisplayName("IdEstadoCivil")]
        public int IdFuncao { get; set; }
    }
}