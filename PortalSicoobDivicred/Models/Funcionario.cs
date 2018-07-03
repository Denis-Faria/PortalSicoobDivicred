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
        [RegularExpression("^[0-9]*$", ErrorMessage = "Informe apenas números")]
        public string Pa { get; set; }

        [Required(ErrorMessage = "Favor informar sua Nacionalidade!")]
        public string Nacionalidade { get; set; }

        [Required(ErrorMessage = "Favor informar o nome da mãe!")]
        public string NomeMae { get; set; }

        [Required(ErrorMessage = "Favor informar o nome do pai")]
        public string NomePai { get; set; }

        [Required(ErrorMessage = "Favor informar o local de nascimento")]
        public string LocalNascimento { get; set; }

        [Required(ErrorMessage = "Favor informar o estado de nascimento")]
        [MaxLength(2, ErrorMessage = "Coloque a abreviação de seu estado")]
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

        [Required(ErrorMessage = "Favor informar se você é uma pessoa com deficiência motora")]
        public string DeficienteMotor { get; set; }

        [Required(ErrorMessage = "Favor informar se você é uma pessoa com deficiência auditiva")]
        public string DeficienteAuditivo { get; set; }

        [Required(ErrorMessage = "Favor informar se você é uma pessoa com deficiência visual")]
        public string DeficienteVisual { get; set; }

        [Required(ErrorMessage = "Favor informar se você é uma pessoa que passou por reabilitação")]
        public string Reabilitado { get; set; }

        public string ObservacaoDeficiente { get; set; }

        public string DadosDependentes { get; set; }

        [Required(ErrorMessage = "Favor informar o codigo de seu banco")]
        public string CodigoBanco { get; set; }


        [Required(ErrorMessage = "Favor informar o numero de sua agência")]
        public string Agencia { get; set; }


        [Required(ErrorMessage = "Favor informar o número de sua conta")]
        public string ContaCorrente { get; set; }


        [DisplayName("Tipo Conta")] public List<SelectListItem> Conta { get; set; }

        [Required(ErrorMessage = "Favor informar o estado civil!")]
        [DisplayName("IdTipoConta")]
        public int IdTipoConta { get; set; }



        [Required(ErrorMessage = "Favor informar o número da sua CTPS")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Informe apenas números")]
        public string NumeroCtps { get; set; }

        [Required(ErrorMessage = "Favor informar a serie da sua CTPS")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Informe apenas números")]
        public string SerieCtps { get; set; }

        [Required(ErrorMessage = "Favor informar o estado em que sua CTPS foi emitida")]
        [MaxLength(2, ErrorMessage = "Coloque a abreviação de seu estado")]
        public string UfCtps { get; set; }

        public string Cnh { get; set; }

        public string Oc { get; set; }

        public DateTime DataExpedicaoDocumentoCnh { get; set; }

        public string OrgaoEmissorCnh { get; set; }

        public DateTime DataValidadeCnh { get; set; }

        public DateTime DataExpedicaoOc { get; set; }

        public string OrgaoEmissorOc { get; set; }

        public DateTime DataValidadeOc { get; set; }

        public string TelefoneFixo { get; set; }

        public string Dependente { get; set; }

        [Required(ErrorMessage = "Favor informar o número de telefone movel")]
        public string TelefoneCelular { get; set; }

        [Required(ErrorMessage = "Favor informar um e-mail secundário")]
        public string EmailSecundario { get; set; }

        [Required(ErrorMessage = "Informe a sua rua!")]
        public string Rua { get; set; }

        [Required(ErrorMessage = "Informe o numero da sua casa!")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Informe apenas números")]
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

        [Required(ErrorMessage = "Favor informar se os dependentes são para fins de IRRF!")]
        public string DependenteIrrf { get; set; }

        [Required(ErrorMessage = "Favor informar se os dependentes são para salário-família!")]
        public string DependenteFamilia { get; set; }



        [Required(ErrorMessage = "Favor informar o seu horario de trabalho!")]
        [DisplayName("IdHorario")]
        public int IdHorario { get; set; }

        public List<SelectListItem> HorarioTrabalho { get; set; }



        [Required(ErrorMessage = "Favor informar o estado civil de seus pais!")]
        [DisplayName("IdEstadoCivilPais")]
        public int IdEstadoCivilPais { get; set; }

        public List<SelectListItem> PaisDivorciados { get; set; }

        [Required(ErrorMessage = "Favor informar o ano de seu primeiro emprego!")]
        [MaxLength(4, ErrorMessage = "Favor adicionar somente o ano de fichamento.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Informe apenas números")]
        public string AnoPrimeiroEmprego { get; set; }

        [Required(ErrorMessage = "Favor informar a data de emissçao de sua CTPS!")]
        public string EmissaoCtps { get; set; }

        [Required(ErrorMessage = "Favor informar a sua matricula!")]
        public string Matricula { get; set; }

        [Required(ErrorMessage = "Favor informar a data de expedissão de seu RG!")]
        public DateTime DataExpedicaoDocumentoRg { get; set; }

        [Required(ErrorMessage = "Favor informar o orgão emissor de seu RG!")]
        [MaxLength(10, ErrorMessage = "Favor adicionar somente o ano de fichamento.")]
        public string OrgaoEmissorRg { get; set; }


        public string CpfIrrf { get; set; }


        public string MultiploNomeEmpresa { get; set; }


        public string MultiploCnpj { get; set; }


        public string MultiploRemuneracao { get; set; }


        public string MultiploComentario { get; set; }


        [Required(ErrorMessage = "Favor informar se você deseja contribuir com o sindicato!")]
        public string ContribuicaoSindical { get; set; }


        [Required(ErrorMessage = "Favor informar o orgão emissor de seu RG!")]
        public string NotificacaoEmail { get; set; }
    }
}