using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.WebPages.Html;

namespace PortalSicoobDivicred.Models
{
    public class Usuario
    {
        [Required(ErrorMessage = "Favor informar seu nome.")]
        [DisplayName("Nome Completo")]
        public string NomeCompleto { get; set; }

        [Required(ErrorMessage = "Favor informar sua data de nascimento.")]
        [DisplayName("Data Nascimento")]
        public DateTime Idade { get; set; }

        [DisplayName("Estado Civil")] public List<SelectListItem> EstadoCivil { get; set; }


        [DisplayName("IdEstadoCivil")] public int IdEstadoCivil { get; set; }

        [Required(ErrorMessage = "Favor informar um numero de telefone principal para contato.")]
        [DisplayName("Telefone Principal")]
        public string TelefonePrincipal { get; set; }

        [Required(ErrorMessage = "Favor informar um segundo numero de telefone para contato.")]
        [DisplayName("Telefone Secundario")]
        public string TelefoneSecundario { get; set; }

        [Required(ErrorMessage = "Favor informar seu CEP.")]
        [DisplayName("CEP")]
        public string Cep { get; set; }


        [DisplayName("Endereço")] public string Rua { get; set; }


        [DisplayName("Complemento")] public string Complemento { get; set; }


        [DisplayName("Numero")] public string Numero { get; set; }


        [DisplayName("Bairro")] public string Bairro { get; set; }


        [DisplayName("Cidade")] public string Cidade { get; set; }


        [DisplayName("Estado")] public string Uf { get; set; }

        [Required(ErrorMessage = "Favor informar uma senha de acesso.")]
        [DataType(DataType.Password)]
        [DisplayName("Senha")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "Favor informar uma senha de acesso.")]
        [DataType(DataType.Password)]
        [DisplayName("Senha")]
        public string ConfirmacaoSenha { get; set; }

        [Required(ErrorMessage = "Favor informar um e-mail para contato.")]
        [DisplayName("E-mail")]
        public string Email { get; set; }


        [DisplayName("Resumo")] public string Resumo { get; set; }

        [Required(ErrorMessage = "Favor informar seu sexo.")]
        [DisplayName("Sexo")]
        public string Sexo { get; set; }


        [DisplayName("Disponibilidade Viagem")]
        public string Disponibilidade { get; set; }

        [Required(ErrorMessage = "Favor informar sua identidade.")]
        [DisplayName("Identidade")]
        public string Identidade { get; set; }

        [Required(ErrorMessage = "Favor informar seu CPF.")]
        [DisplayName("CPF")]
        public string Cpf { get; set; }

        [DisplayName("CNH")] public string Cnh { get; set; }


        [DisplayName("Categoria CNH")] public string CatCnh { get; set; }


        [DisplayName("Tipo de Deficiência")] public string TipoDeficiencia { get; set; }


        [DisplayName("Primeira Cnh")] public string PrimeiraCnh { get; set; }


        [DisplayName("Quantidade Filhos")] public string QuantidadeFilho { get; set; }


        [DisplayName("Tipo Escolaridade")] public List<SelectListItem> IdTipoEscolaridade { get; set; }

        public int? TipoEscolaridade { get; set; }


        [DisplayName("Nome da Instituição")] public List<string> NomeInstituicao { get; set; }

        [DisplayName("Nome do Curso")] public List<string> TipoFormacao { get; set; }


        [DisplayName("Nome do Curso")] public List<string> NomeCurso { get; set; }


        [DisplayName("Ano de Inicio")] public List<string> AnoInicio { get; set; }

        [Required]
        [DisplayName("Ano de Termino")]
        public List<string> AnoTermino { get; set; }


        [DisplayName("Nome da Instituição")] public List<string> NomeEmpresa { get; set; }

        [DisplayName("Nome do Cargo")] public List<string> NomeCargo { get; set; }


        [DisplayName("Data Entrada")] public List<string> DataEntrada { get; set; }


        [DisplayName("Data Saida")] public List<string> DataSaida { get; set; }


        [DisplayName("Atividades Desempenhadas")]
        public List<string> Atividades { get; set; }

        [DisplayName("Emprego Atual")] public List<string> EmpregoAtual { get; set; }

        [DisplayName("Tipo de Deficiência")] public string Conhecido { get; set; }

        [DisplayName("Tipo de Deficiência")] public string Certificacao { get; set; }


        [Required(ErrorMessage = "Favor informar seu nivel de conhecimento em informática.")]
        [DisplayName("Informatica")]
        public string Informatica { get; set; }
    }
}