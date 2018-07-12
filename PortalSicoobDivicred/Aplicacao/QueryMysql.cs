using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PortalSicoobDivicred.Models;
using PortalSicoobDivicred.Repositorios;

namespace PortalSicoobDivicred.Aplicacao
{
    public class QueryMysql
    {
        private readonly Conexao _conexaoMysql;


        public QueryMysql()
        {
            _conexaoMysql = new Conexao();
        }

        public bool ConfirmaLogin(string usuario, string senha)
        {
            var queryConfirmaLogin = "SELECT login FROM funcionarios WHERE login='" + usuario + "' AND senha=MD5('" +
                                     senha +
                                     "')";

            var dados = _conexaoMysql.ExecutaComandoComRetorno(queryConfirmaLogin);

            if (dados.Count == 0)
                return false;
            var cookieusuario = new HttpCookie("CookieFarm");
            cookieusuario.Value = Criptografa.Criptografar(dados[0]["login"]);
            cookieusuario.Expires = DateTime.Now.AddHours(1);
            HttpContext.Current.Response.Cookies.Add(cookieusuario);
            return true;
        }

        internal bool TrocaSenha(string usuario)
        {
            var queryConfirmaLogin = "SELECT trocasenha FROM funcionarios WHERE login='" + usuario + "' ;";

            var dados = _conexaoMysql.ExecutaComandoComRetorno(queryConfirmaLogin);
            try
            {
                if (dados[0]["trocasenha"].Equals("S"))
                    return false;
                return true;
            }
            catch
            {
                return true;
            }
        }

        public bool PrimeiroLogin(string usuario)
        {
            var query = "SELECT perfilcompleto FROM funcionarios WHERE login='" + usuario + "';";

            var dados = _conexaoMysql.ExecutaComandoComRetorno(query);

            if (dados[0]["perfilcompleto"].Equals("S"))
                return false;
            return true;
        }

        public bool PermissaoCurriculos(string usuario)
        {
            var query =
                "select a.valor from permissoesgrupo a, funcionarios b, grupos c where a.idgrupo = c.id and b.idgrupo = c.id and b.login='" +
                usuario + "' and a.idaplicativo=9";

            var dados = _conexaoMysql.ExecutaComandoComRetorno(query);
            try
            {
                if (dados[0]["valor"].Equals("S"))
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool PermissaoParametros(string usuario)
        {
            var query =
                "select a.valor from permissoesgrupo a, funcionarios b, grupos c where a.idgrupo = c.id and b.idgrupo = c.id and b.login='" +
                usuario + "' and a.idaplicativo=13";

            var dados = _conexaoMysql.ExecutaComandoComRetorno(query);
            try
            {
                if (dados[0]["valor"].Equals("S"))
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }


        public bool PermissaoPesquisaWebdDesk(string usuario)
        {
            var query =
                "select a.valor from permissoesgrupo a, funcionarios b, grupos c where a.idgrupo = c.id and b.idgrupo = c.id and b.login='" +
                usuario + "' and a.idpermissao='WEBDESK_PESQUISA'";

            var dados = _conexaoMysql.ExecutaComandoComRetorno(query);
            try
            {
                if (dados[0]["valor"].Equals("S"))
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool PermissaoTesouraria(string usuario)
        {
            var query =
                "select a.valor from permissoesgrupo a, funcionarios b, grupos c where a.idgrupo = c.id and b.idgrupo = c.id and b.login='" +
                usuario + "' and a.idaplicativo=10";

            var dados = _conexaoMysql.ExecutaComandoComRetorno(query);
            try
            {
                if (dados[0]["valor"].Equals("S"))
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        public List<Dictionary<string, string>> RecuperaDocumentosFuncionario(string login)
        {
            var query =
                "SELECT * FROM documentospessoaisfuncionarios WHERE idfuncionario=(SELECT id FROM funcionarios where login='" +
                login + "')";
            var dadosCurriculos = _conexaoMysql.ExecutaComandoComRetorno(query);
            return dadosCurriculos;
        }

        public List<Dictionary<string, string>> RecuperaVinculoExtra(string idFuncionario)
        {
            var query =
                "SELECT * FROM vinculosempregaticiosfuncionarios WHERE idfuncionario=" + idFuncionario + "";
            var dadosCurriculos = _conexaoMysql.ExecutaComandoComRetorno(query);
            return dadosCurriculos;
        }

        public void InserirFormacao(string formacao, string idFuncionario, string tipoFormacao)
        {
            var queryFormacao = "INSERT INTO formacoesfuncionarios (idfuncionario,descricao,tipoformacao) VALUES('" +
                                idFuncionario + "','" + formacao + "','" + tipoFormacao + "')";
            _conexaoMysql.ExecutaComandoComRetorno(queryFormacao);
        }

        public void AtualizaFormacao(string formacao, string id, string tipoFormacao)
        {
            var queryFormacao = "UPDATE formacoesfuncionarios  SET descricao='" + formacao + "',tipoformacao='" +
                                tipoFormacao + "' WHERE id='" + id + "'";
            _conexaoMysql.ExecutaComandoComRetorno(queryFormacao);
        }

        public bool UsuarioLogado()
        {
            var usuario = HttpContext.Current.Request.Cookies["CookieFarm"];
            if (usuario == null)
                return false;
            return true;
        }

        public List<Dictionary<string, string>> RecuperaDadosUsuarios(string login)
        {
            var queryRecuperausuario =
                "SELECT * FROM funcionarios  WHERE login='" + login + "'";


            var dados = _conexaoMysql.ExecutaComandoComRetorno(queryRecuperausuario);


            return dados;
        }

        public List<Dictionary<string, string>> RecuperadadosFuncionariosSetor()
        {
            var queryRecuperausuario =
                "SELECT a.*,b.descricao as setor  FROM funcionarios a, setores b where a.idsetor=b.id and a.ativo='S'";


            var dados = _conexaoMysql.ExecutaComandoComRetorno(queryRecuperausuario);


            return dados;
        }

        public List<Dictionary<string, string>> RecuperadadosFuncionariosTabelausuario(string usuario)
        {
            var query =
                "SELECT * FROM funcionarios WHERE login='" + usuario + "'";
            var dados = _conexaoMysql.ExecutaComandoComRetorno(query);


            return dados;
        }

        public List<Dictionary<string, string>> RecuperadadosFuncionariosTabelaFuncionarios(string nomeFuncionario)
        {
            var query =
                "SELECT * FROM funcionarios where (soundex(nome)like concat('%',soundex('" + nomeFuncionario +
                "'),'%')) ";
            var dados = _conexaoMysql.ExecutaComandoComRetorno(query);

            return dados;
        }

        public List<Dictionary<string, string>> RecuperadadosFuncionariosTabelaFuncionariosLogin(string login)
        {
            var query =
                "SELECT * FROM funcionarios where login='" + login + "'";
            var dados = _conexaoMysql.ExecutaComandoComRetorno(query);


            return dados;
        }

        public List<Dictionary<string, string>> RecuperadadosFuncionariosTabelaFuncionariosPerfil(string usuarioSistema)
        {
            var query =
                "SELECT * FROM funcionarios WHERE login='" + usuarioSistema + "'";
            var dados = _conexaoMysql.ExecutaComandoComRetorno(query);


            return dados;
        }

        public List<Dictionary<string, string>> RetornaCertificacao(string idCertificao)
        {
            var query =
                "SELECT descricao FROM certificacoesfuncionarios WHERE id='" + idCertificao + "'";
            var dados = _conexaoMysql.ExecutaComandoComRetorno(query);


            return dados;
        }

        public List<Dictionary<string, string>> RetornaCertificacaoFuncao(string idFuncao)
        {
            var query =
                "SELECT idcertificacao FROM funcoes WHERE id='" + idFuncao + "'";
            var dados = _conexaoMysql.ExecutaComandoComRetorno(query);


            return dados;
        }

        public List<Dictionary<string, string>> RetornaFormacaoFuncionario(string idFuncionario)
        {
            var query =
                "SELECT * FROM formacoesfuncionarios WHERE idfuncionario='" + idFuncionario + "'";
            var dados = _conexaoMysql.ExecutaComandoComRetorno(query);


            return dados;
        }

        public string RetornaFuncaoFuncionario(string idFuncao)
        {
            var query =
                "SELECT descricao FROM funcoes WHERE id='" + idFuncao + "'";
            var dados = _conexaoMysql.ExecutaComandoComRetorno(query);


            return dados[0]["descricao"];
        }

        public string RetornaGeneroFuncionario(string idGenero)
        {
            var query =
                "SELECT descricao FROM tipossexos WHERE id='" + idGenero + "'";
            var dados = _conexaoMysql.ExecutaComandoComRetorno(query);


            return dados[0]["descricao"];
        }

        public string RetornaSetorFuncionario(string idSetor)
        {
            var query =
                "SELECT descricao FROM setores WHERE id='" + idSetor + "'";
            var dados = _conexaoMysql.ExecutaComandoComRetorno(query);


            return dados[0]["descricao"];
        }

        public string RetornaEscolaridadeFuncionario(string idEscolaridade)
        {
            var query =
                "SELECT descricao FROM tiposformacoes WHERE id='" + idEscolaridade + "'";
            var dados = _conexaoMysql.ExecutaComandoComRetorno(query);


            return dados[0]["descricao"];
        }

        public string RetornaEstadoCivilFuncionario(string idEstadoCivil)
        {
            var query =
                "SELECT descricao FROM tiposestadoscivis WHERE id='" + idEstadoCivil + "'";
            var dados = _conexaoMysql.ExecutaComandoComRetorno(query);


            return dados[0]["descricao"];
        }

        public string RetornaEtiniaFuncionario(string etinia)
        {
            var query =
                "SELECT descricao FROM tiposetnias WHERE id='" + etinia + "'";
            var dados = _conexaoMysql.ExecutaComandoComRetorno(query);


            return dados[0]["descricao"];
        }

        public List<SelectListItem> RetornaEstadoCivil()
        {
            var estadoCivil = new List<SelectListItem>();

            const string queryRetornaEstadoCivil = "SELECT id,descricao FROM tiposestadoscivis";

            var dados = _conexaoMysql.ExecutaComandoComRetorno(queryRetornaEstadoCivil);
            foreach (var row in dados)
                estadoCivil.Add(new SelectListItem
                {
                    Value = row["id"],
                    Text = row["descricao"]
                });

            return estadoCivil;
        }

        public List<SelectListItem> RetornaHorarioTrabalho()
        {
            var horarioTrabalho = new List<SelectListItem>();

            const string queryRetornaHorarioTrabalho = "SELECT id,descricao FROM tiposhorariosfuncionarios";

            var dados = _conexaoMysql.ExecutaComandoComRetorno(queryRetornaHorarioTrabalho);
            foreach (var row in dados)
                horarioTrabalho.Add(new SelectListItem
                {
                    Value = row["id"],
                    Text = row["descricao"]
                });

            return horarioTrabalho;
        }

        public List<SelectListItem> RetornaEstadoCivilPais()
        {
            var estadoCivilPais = new List<SelectListItem>();

            const string queryRetornaEstadoCivilPais = "SELECT id,descricao FROM tiposestadoscivispais";

            var dados = _conexaoMysql.ExecutaComandoComRetorno(queryRetornaEstadoCivilPais);
            foreach (var row in dados)
                estadoCivilPais.Add(new SelectListItem
                {
                    Value = row["id"],
                    Text = row["descricao"]
                });

            return estadoCivilPais;
        }

        public List<SelectListItem> RetornaTipoConta()
        {
            var tipoConta = new List<SelectListItem>();

            const string queryRetornaTipoConta = "SELECT id,descricao FROM tiposcontas";

            var dados = _conexaoMysql.ExecutaComandoComRetorno(queryRetornaTipoConta);
            foreach (var row in dados)
                tipoConta.Add(new SelectListItem
                {
                    Value = row["id"],
                    Text = row["descricao"]
                });

            return tipoConta;
        }

        public List<SelectListItem> RetornaSexo()
        {
            var sexo = new List<SelectListItem>();

            const string queryRetornaSexo = "SELECT id,descricao FROM tipossexos";

            var dados = _conexaoMysql.ExecutaComandoComRetorno(queryRetornaSexo);
            foreach (var row in dados)
                sexo.Add(new SelectListItem
                {
                    Value = row["id"],
                    Text = row["descricao"]
                });

            return sexo;
        }


        public List<Dictionary<string, string>> RecuperaPontuacaoFuncionariosSetor(string usuarioSistema)
        {
            var query1 =
                "SELECT idsetor from funcionarios WHERE usuario='" + usuarioSistema;
            var idSetor = _conexaoMysql.ExecutaComandoComRetorno(query1);

            var query =
                "SELECT * from funcionarios where idsetor='" + idSetor + "' and login !='" + usuarioSistema + "'";
            var dados = _conexaoMysql.ExecutaComandoComRetorno(query);

            return dados;
        }


        public string RecuperadadosPontos(int idProduto)
        {
            var query =
                "select valorminimo,peso from pgdprodutos where id='" + idProduto + "'";
            var dados = _conexaoMysql.ExecutaComandoComRetorno(query);


            return dados[0]["descricao"];
        }


        public List<SelectListItem> RetornaFuncionario()
        {
            var funcionario = new List<SelectListItem>();

            const string queryRetornaFuncionario = "SELECT id,nome FROM funcionarios";

            var dados = _conexaoMysql.ExecutaComandoComRetorno(queryRetornaFuncionario);
            foreach (var row in dados)
                funcionario.Add(new SelectListItem
                {
                    Value = row["id"],
                    Text = row["nome"]
                });

            return funcionario;
        }


        public string Recuperausuario(string login)
        {
            var queryRecuperausuario = "SELECT id from funcionarios where login='" + login + "'";

            var id = _conexaoMysql.ExecutaComandoComRetorno(queryRecuperausuario);

            return id[0]["id"];
        }

        public string RecuperausuarioLogin(string usuario)
        {
            var queryRecuperausuarioLogin = "SELECT login from funcionarios where id='" + usuario + "'";

            var id = _conexaoMysql.ExecutaComandoComRetorno(queryRecuperausuarioLogin);

            return id[0]["login"];
        }


        public List<SelectListItem> RetornaEtnia()
        {
            var etnia = new List<SelectListItem>();

            const string queryRetornaEtnia = "SELECT id,descricao FROM tiposetnias";

            var dados = _conexaoMysql.ExecutaComandoComRetorno(queryRetornaEtnia);
            foreach (var row in dados)
                etnia.Add(new SelectListItem
                {
                    Value = row["id"],
                    Text = row["descricao"]
                });

            return etnia;
        }

        public List<SelectListItem> RetornaFormacao()
        {
            var formacao = new List<SelectListItem>();

            const string queryRetornaFormacao = "SELECT id,descricao FROM tiposformacoes";

            var dados = _conexaoMysql.ExecutaComandoComRetorno(queryRetornaFormacao);
            foreach (var row in dados)
                formacao.Add(new SelectListItem
                {
                    Value = row["id"],
                    Text = row["descricao"]
                });

            return formacao;
        }

        public List<SelectListItem> RetornaSetor()
        {
            var setor = new List<SelectListItem>();

            const string queryRetornaSetor = "SELECT id,descricao FROM setores";

            var dados = _conexaoMysql.ExecutaComandoComRetorno(queryRetornaSetor);
            foreach (var row in dados)
                setor.Add(new SelectListItem
                {
                    Value = row["id"],
                    Text = row["descricao"]
                });

            return setor;
        }

        public List<SelectListItem> RetornaFuncao()
        {
            var funcao = new List<SelectListItem>();

            const string query = "SELECT id,descricao FROM funcoes";

            var dados = _conexaoMysql.ExecutaComandoComRetorno(query);
            foreach (var row in dados)
                funcao.Add(new SelectListItem
                {
                    Value = row["id"],
                    Text = row["descricao"]
                });

            return funcao;
        }

        public void AtualizadadosFuncionarioFormulario(string nome, string cpf, string rg, string pis,
            string dataNascimentoFuncionario, string Sexo, string descricaoSexo, string Etnia, string estadoCivil,
            string formacao, string FormacaoAcademica, string usuarioSistema, string Email, string PA, string Rua,
            string numero, string Bairro, string Cidade, string Setor, string Funcao, string QuatidadeFilhos,
            string DataNascimentoFilhos, string Emergencia, string PrincipaisHobbies, string ComidaFavorita,
            string Viagem, string ConfirmacaoCertificacao, string Confirmadados, string Nacionalidade, string NomeMae,
            string NomePai, string LocalNascimento, string UfNascimento, string Complemento, string Cep, string Pais,
            string ResidenciaPropria, string RecursoFgts, string NumeroCtps, string SerieCtps, string UfCtps,
            string TelefoneFixo,
            string TelefoneCelular, string EmailSecundario, string Cnh, string OrgaoCnh, DateTime DataExpedicaoCnh,
            DateTime DataValidadeCnh,
            string Oc, string OrgaoOc, DateTime DataExpedicaoOc, DateTime DataValidadeOc, string DeficienteMotor,
            string DeficienteVisual,
            string DeficienteAuditivo, string Reabilitado, string ObservacaoDeficiente, int IdTipoConta,
            string CodigoBanco, string Agencia,
            string ContaCorrente, string DependenteIrrf, string DependenteFamilia, string dadosDependentes,
            string TipoDependentes, string Matricula,
            string PrimeiroEmprego, string EmissaoCtps, string Divorcio, string OrgaoEmissorRg, DateTime DataEmissaoRg,
            string CpfIrrf,
            string notificacaoEmail, string contribuicaoSindical)
        {
            var queryAtualizaFuncionario = "UPDATE funcionarios SET nome='" + nome + "', cpf='" + cpf + "',rg='" +
                                           rg + "', pis='" + pis + "',datanascimento='" +
                                           Convert.ToDateTime(dataNascimentoFuncionario).ToString("yyyy/MM/dd") +
                                           "', sexo=" + Sexo + ",descricaosexo='" + descricaoSexo + "',etnia=" +
                                           Etnia + ",idestadocivil=" + estadoCivil + ",  idescolaridade=" +
                                           formacao + ", formacaoacademica='" + FormacaoAcademica + "', login='" +
                                           usuarioSistema + "', email='" + Email + "', idpa=" + PA + ", rua='" +
                                           Rua + "',numero=" + numero + ",bairro='" + Bairro + "',cidade='" +
                                           Cidade + "', idsetor=" + Setor + ", funcao='" + Funcao +
                                           "', quantidadefilho='" + QuatidadeFilhos + "',datanascimentofilho='" +
                                           DataNascimentoFilhos + "', contatoemergencia='" + Emergencia +
                                           "', principalhobbie='" + PrincipaisHobbies + "', comidafavorita='" +
                                           ComidaFavorita + "',viagem='" + Viagem +
                                           "', perfilcompleto='S',confirmacaodado='" + Confirmadados +
                                           "',confirmacaocertificacao='" +
                                           ConfirmacaoCertificacao + "',nacionalidade='" + Nacionalidade + "'," +
                                           "nomemae='" + NomeMae + "',nomepai='" + NomePai + "',localnascimento='" +
                                           LocalNascimento + "'" +
                                           ",ufnascimento='" + UfNascimento + "',complemento='" + Complemento +
                                           "',cep='" + Cep + "'" +
                                           ",pais='" + Pais + "',residenciapropria='" + ResidenciaPropria +
                                           "',recursofgts='" + RecursoFgts + "'" +
                                           ",numeroctps='" + NumeroCtps + "',seriectps='" + SerieCtps + "',ufctps='" +
                                           UfCtps + "', telefonefixo='" + TelefoneFixo + "'" +
                                           ",telefonecelular='" + TelefoneCelular + "',emailsecundario='" +
                                           EmailSecundario + "',cnh='" + Cnh + "',orgaoemissorcnh='" + OrgaoCnh + "'" +
                                           ",dataexpedicaocnh='" + DataExpedicaoCnh.ToString("yyyy/MM/dd") +
                                           "',datavalidadecnh='" + DataValidadeCnh.ToString("yyyy/MM/dd") + "'," +
                                           "oc='" + Oc + "',orgaoemissoroc='" + OrgaoOc + "',dataexpedicaooc='" +
                                           DataExpedicaoOc.ToString("yyyy/MM/dd") + "'" +
                                           ",datavalidadeoc='" + DataValidadeOc.ToString("yyyy/MM/dd") +
                                           "',deficientemotor='" + DeficienteMotor + "',deficientevisual='" +
                                           DeficienteVisual + "'" +
                                           ",deficienteauditivo='" + DeficienteAuditivo + "',reabilitado='" +
                                           Reabilitado + "',observacaodeficiente='" + ObservacaoDeficiente +
                                           "', idtipoconta=" + IdTipoConta + "," +
                                           "codigobanco=" + CodigoBanco + ",agencia='" + Agencia + "',contacorrente='" +
                                           ContaCorrente + "', informacaodependente='" + dadosDependentes +
                                           "',dependenteirrpf='" + DependenteIrrf + "'," +
                                           "dependentesalariofamilia='" + DependenteFamilia + "',tipodependente='" +
                                           TipoDependentes + "',matricula='" + Matricula + "',anoprimeiroemprego='" +
                                           PrimeiroEmprego + "'," +
                                           "dataemissaoctps='" +
                                           Convert.ToDateTime(EmissaoCtps).Date.ToString("yyyy/MM/dd") +
                                           "',paisdivorciado='" + Divorcio + "'," +
                                           " dataemissaorg='" + DataEmissaoRg.Date.ToString("yyyy/MM/dd") +
                                           "',orgaoemissorrg='" + OrgaoEmissorRg + "', cpfirrf='" + CpfIrrf + "'," +
                                           "contribuicaosindical='" + contribuicaoSindical + "',notificacaoemail='" +
                                           notificacaoEmail + "'" +
                                           " WHERE login='" + usuarioSistema + "'";
            _conexaoMysql.ExecutaComandoComRetorno(queryAtualizaFuncionario);
        }

        public void InserirVinculoEmpregaticio(string IdFuncionario, string NomeEmpresa, string Cnpj,
            string Remuneracao, string Comentario)
        {
            var queryInserirVinculo =
                "INSERT INTO vinculosempregaticiosfuncionarios (idfuncionario,nomeempresa,cnpj,remuneracao,comentario) VALUES(" +
                IdFuncionario + ",'" + NomeEmpresa + "','" + Cnpj + "','" + Remuneracao + "','" + Comentario + "')";

            _conexaoMysql.ExecutaComando(queryInserirVinculo);
        }

        public void AtualizaVinculoEmpregaticio(string IdFuncionario, string NomeEmpresa, string Cnpj,
            string Remuneracao, string Comentario)
        {
            var queryInserirVinculo =
                "UPDATE vinculosempregaticiosfuncionarios set nomeempresa='" + NomeEmpresa + "',cnpj='" + Cnpj +
                "',remuneracao='" + Remuneracao + "',comentario='" + Comentario + "' WHERE idfuncionario=" +
                IdFuncionario + "";

            _conexaoMysql.ExecutaComando(queryInserirVinculo);
        }

        public void AtualizadadosFuncionariodadosPessoais(string Nome, string Cpf, string Rg, string Pis,
            string DataNascimentoFuncionario, string Sexo, string DescricaoSexo, string Etnia, string EstadoCivil,
            string Formacao, string FormacaoAcademica, string usuarioSistema, string Email, string PA, string Rua,
            string Numero, string Bairro, string Cidade, string Confirmadados, string Nacionalidade, string NomeMae,
            string NomePai, string LocalNascimento, string UfNascimento, string Complemento, string Cep, string Pais,
            string ResidenciaPropria, string RecursoFgts, string TelefoneFixo, string TelefoneCelular,
            string EmailSecundario,
            string Cnh, string OrgaoEmissorCnh, DateTime DataExpedicaoDocumentoCnh, DateTime DataValidadeCnh, string Oc,
            string OrgaoEmissorOc,
            DateTime DataExpedicaoOc, DateTime DataValidadeOc, string DeficienteMotor, string DeficienteVisual,
            string DeficienteAuditivo,
            string Reabilitado, string ObservacaoDeficiente, string PaisDivorciados, string OrgaoEmissorRg,
            DateTime DataExpedicaoDocumentoRg, int IdHorarioTrabalho)
        {
            var queryAtualizaFuncionario = "UPDATE funcionarios SET nome='" + Nome + "', cpf='" + Cpf + "',rg='" +
                                           Rg + "', pis='" + Pis + "',datanascimento='" +
                                           Convert.ToDateTime(DataNascimentoFuncionario).ToString("yyyy/MM/dd") +
                                           "', sexo=" + Sexo + ",descricaosexo='" + DescricaoSexo + "',etnia=" +
                                           Etnia + ",idestadocivil=" + EstadoCivil + ",  idescolaridade=" +
                                           Formacao + ", formacaoacademica='" + FormacaoAcademica + "', login='" +
                                           usuarioSistema + "', email='" + Email + "', idpa=" + PA + ", rua='" +
                                           Rua + "',numero=" + Numero + ",bairro='" + Bairro + "',cidade='" +
                                           Cidade + "', confirmacaodado='" + Confirmadados + "',nacionalidade='" +
                                           Nacionalidade + "'," +
                                           "nomemae='" + NomeMae + "',nomepai='" + NomePai + "',localnascimento='" +
                                           LocalNascimento + "'" +
                                           ",ufnascimento='" + UfNascimento + "',complemento='" + Complemento +
                                           "',cep='" + Cep + "'" +
                                           ",pais='" + Pais + "',residenciapropria='" + ResidenciaPropria +
                                           "',recursofgts='" + RecursoFgts + "'" +
                                           ",telefonefixo='" + TelefoneFixo + "'" + ",telefonecelular='" +
                                           TelefoneCelular + "',emailsecundario='" + EmailSecundario + "'" +
                                           ",cnh='" + Cnh + "',orgaoemissorcnh='" + OrgaoEmissorCnh + "'" +
                                           ",dataexpedicaocnh='" + DataExpedicaoDocumentoCnh.ToString("yyyy/MM/dd") +
                                           "',datavalidadecnh='" + DataValidadeCnh.ToString("yyyy/MM/dd") + "'," +
                                           "oc='" + Oc + "',orgaoemissoroc='" + OrgaoEmissorOc + "',dataexpedicaooc='" +
                                           DataExpedicaoOc.ToString("yyyy/MM/dd") + "'" +
                                           ",datavalidadeoc='" + DataValidadeOc.ToString("yyyy/MM/dd") +
                                           "',deficientemotor='" + DeficienteMotor + "',deficientevisual='" +
                                           DeficienteVisual + "'" +
                                           ",deficienteauditivo='" + DeficienteAuditivo + "',reabilitado='" +
                                           Reabilitado + "',observacaodeficiente='" + ObservacaoDeficiente + "'" +
                                           ",paisdivorciado='" + PaisDivorciados + "', dataemissaorg='" +
                                           DataExpedicaoDocumentoRg.ToString("yyyy/MM/dd") + "',orgaoemissorrg='" +
                                           OrgaoEmissorRg + "'" +
                                           ", idhorariotrabalho=" + IdHorarioTrabalho + "  WHERE login='" +
                                           usuarioSistema + "'";
            _conexaoMysql.ExecutaComandoComRetorno(queryAtualizaFuncionario);
        }

        public void AtualizadadosFuncionarioProfissional(string Setor, string Funcao, string usuarioSistema,
            string NumeroCTPS,
            string SerieCTPS, string UfCTPS, int IdTipoConta, string CodigoBanco,
            string Agencia, string ContaCorrente, string DependenteIrrf, string DependenteFamilia,
            string dadosDependentes, string TiposDependentes, string Matricula, string AnoPrimeiroEmprego,
            string EmissaoCtps,
            string CpfIrrf, int IdHorariOtrabalho)
        {
            var queryAtualizaFuncionario = "UPDATE funcionarios SET idsetor=" + Setor + ", funcao='" + Funcao +
                                           "',numeroctps='" + NumeroCTPS + "',seriectps='" + SerieCTPS + "',ufctps='" +
                                           UfCTPS + "', idtipoconta=" + IdTipoConta + "," +
                                           "codigobanco=" + CodigoBanco + ",agencia='" + Agencia + "',contacorrente='" +
                                           ContaCorrente + "', informacaodependente='" + dadosDependentes +
                                           "',dependenteirrpf='" + DependenteIrrf + "'," +
                                           "dependentesalariofamilia='" + DependenteFamilia + "',tipodependente='" +
                                           TiposDependentes + "',matricula='" + Matricula + "'" +
                                           ",anoprimeiroemprego='" + AnoPrimeiroEmprego + "', dataemissaoctps='" +
                                           Convert.ToDateTime(EmissaoCtps).Date.ToString("yyyy/MM/dd") +
                                           "', cpfirrf='" + CpfIrrf + "',idhorariotrabalho=" + IdHorariOtrabalho +
                                           " WHERE login='" + usuarioSistema + "'";
            _conexaoMysql.ExecutaComandoComRetorno(queryAtualizaFuncionario);
        }

        public void AtualizaFoto(string Foto, string usuarioSistema)
        {
            var queryAtualizaFuncionario =
                "UPDATE funcionarios SET foto='" + Foto + "' WHERE login='" + usuarioSistema + "'";
            _conexaoMysql.ExecutaComandoComRetorno(queryAtualizaFuncionario);
        }

        public void AtualizadadosFuncionarioPerguntas(string usuarioSistema, string QuatidadeFilhos,
            string DataNascimentoFilhos, string Emergencia, string PrincipaisHobbies, string ComidaFavorita,
            string Viagem, string Sindicato, string Email)
        {
            var queryAtualizaFuncionario = "UPDATE funcionarios SET quantidadefilho='" + QuatidadeFilhos +
                                           "',datanascimentofilho='" +
                                           DataNascimentoFilhos + "', contatoemergencia='" + Emergencia +
                                           "', principalhobbie='" + PrincipaisHobbies + "', comidafavorita='" +
                                           ComidaFavorita + "',viagem='" + Viagem +
                                           "', perfilcompleto='S',contribuicaosindical = '" + Sindicato +
                                           "',notificacaoemail = '" + Email + "' WHERE login='" + usuarioSistema + "'";
            _conexaoMysql.ExecutaComandoComRetorno(queryAtualizaFuncionario);
        }

        public void AtualizarArquivoPessoal(string NomeArquivo, byte[] Foto, string Login)
        {
            var queryIdFuncionario = "SELECT id FROM funcionarios WHERE login='" + Login + "'";
            var row = _conexaoMysql.ExecutaComandoComRetorno(queryIdFuncionario);

            var queryArquivousuario =
                "SELECT count(id) as count FROM documentospessoaisfuncionarios WHERE idfuncionario='" + row[0]["id"] +
                "' and nomearquivo='" + NomeArquivo + "'";
            var row2 = _conexaoMysql.ExecutaComandoComRetorno(queryArquivousuario);

            if (Convert.ToInt32(row2[0]["count"]) == 0)
            {
                var queryAtualizaFuncionario =
                    "INSERT INTO documentospessoaisfuncionarios (idfuncionario,nomearquivo,arquivo,dataupload) VALUES(" +
                    row[0]["id"] + ",'" + NomeArquivo + "',@image ,NOW()) ";
                _conexaoMysql.ExecutaComandoArquivo(queryAtualizaFuncionario, Foto);
            }
            else
            {
                var queryAtualizaFuncionario =
                    "UPDATE documentospessoaisfuncionarios SET arquivo=@image and dataupload=NOW() WHERE idfuncionario=" +
                    row[0]["id"] + " AND nomearquivo='" + NomeArquivo + "'";
                _conexaoMysql.ExecutaComandoArquivo(queryAtualizaFuncionario, Foto);
            }
        }

        public List<Dictionary<string, string>> RecuperaTodosArquivos(string Login)
        {
            var query =
                "SELECT nomearquivo, dataupload FROM documentospessoaisfuncionarios WHERE login='" + Login + "'";
            var row = _conexaoMysql.ExecutaComandoComRetorno(query);
            return row;
        }

        public DataTable RetornaDocumentosFuncionario(string Login)
        {
            var dados = _conexaoMysql.ComandoArquivo(Login);
            return dados;
        }

        public void CadastrarAlert(string idusuario, string idAplicativo, string descricao)
        {
            var query = "INSERT INTO alertas (idusuario,idaplicativo,descricao,data) VALUES(" + idusuario + "," +
                        idAplicativo + ",'" + descricao + "',NOW())";
            _conexaoMysql.ExecutaComando(query);
        }

        public void RemoverAlerta(string idAlerta)
        {
            var query = "UPDATE alertas SET lido='S' WHERE id='"+idAlerta+"'";
            _conexaoMysql.ExecutaComando( query );
        }

        public List<Dictionary<string, string>> BuscaAlerta(string idFuncionario)
        {
            var query =
                "SELECT * FROM alertas WHERE idusuario='" + idFuncionario + "' and lido='N'";
            var row = _conexaoMysql.ExecutaComandoComRetorno( query );
            return row;
        }

        public List<Dictionary<string, string>> RetornaHoras(string Cracha)
        {
            var query = "select hora,datareferencia from horasextrasfuncionarios WHERE crachafirebird=" + Cracha + ";";
            var dados = _conexaoMysql.ExecutaComandoComRetorno(query);
            return dados;
        }

        public string RemoveAccents(string text)
        {
            var sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (var letter in arrayText)
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            return sbReturn.ToString();
        }

        public List<Dictionary<string, string>> RetornaFuncionariosSetor(string IdSetor)
        {
            var query =
                "SELECT nome,idnotificacao,notificacaoemail,id,email FROM funcionarios WHERE idsetor='" + IdSetor + "'";
            var dados = _conexaoMysql.ExecutaComandoComRetorno(query);


            return dados;
        }

        public void AtualizaEmailSenha(string Idusuario)
        {
            var query = "UPDATE funcionarios SET emailtrocasenha='S' WHERE id='" + Idusuario + "'";
            _conexaoMysql.ExecutaComandoComRetorno(query);
        }

        public void AtualizaSenha(string usuario, string Senha)
        {
            var query = "UPDATE funcionarios SET senha='" + Senha + "', emailtrocasenha='N' WHERE login='" + usuario +
                        "'";
            _conexaoMysql.ExecutaComandoComRetorno(query);
        }

        public List<Dictionary<string, string>> RetornaEmailSenha(string usuario)
        {
            var query =
                "SELECT emailtrocasenha FROM funcionarios WHERE login='" + usuario + "'";
            var dados = _conexaoMysql.ExecutaComandoComRetorno(query);


            return dados;
        }

        public List<Dictionary<string, string>> BuscaNumerodaSorte()
        {
            var query = "SELECT * FROM showdepremiosnumerosorte order by data LIMIT 1;";
            var dados = _conexaoMysql.ExecutaComandoComRetorno(query);
            return dados;
        }

        public List<Dictionary<string, string>> RetornaExtrato(string Cpf)
        {
            var query = "SELECT* FROM showdepremioscupons WHERE  numdoccliente = " + Cpf + "";
            var dados = _conexaoMysql.ExecutaComandoComRetorno(query);
            return dados;
        }

        public void CadastraIdNotificacao(string IdNotificacao, string Login)
        {
            var query = "UPDATE funcionarios set idnotificacao='" + IdNotificacao + "' WHERE login='" + Login + "'";
            _conexaoMysql.ExecutaComando(query);
        }

        public List<Dictionary<string, string>> RetornaInformacoesNotificacao(string IdFuncionario)
        {
            var query = "select id,email,idnotificacao,notificacaoemail,idsetor,nome from funcionarios where id=" +
                        IdFuncionario + " ";
            var Chamados = _conexaoMysql.ExecutaComandoComRetorno(query);
            return Chamados;
        }

        public List<Dictionary<string, string>> RetornaInformacoesGestor(string idSetor)
        {
            var query = "select id,email,idnotificacao,notificacaoemail from funcionarios where idsetor=" +
                        idSetor + " and gestor='S' ";
            var chamados = _conexaoMysql.ExecutaComandoComRetorno(query);
            return chamados;
        }

        public List<Dictionary<string, string>> RetornaInformacoesNumerario(string IdAgencia)
        {
            var query = "select * from numerariosagencias WHERE idagencia=" + IdAgencia + " ";
            var Chamados = _conexaoMysql.ExecutaComandoComRetorno(query);
            return Chamados;
        }

        public string RetornaNomeFuncionario(string IdFuncionario)
        {
            var query = "select nome from funcionarios WHERE id=" + IdFuncionario + " ";
            var chamados = _conexaoMysql.ExecutaComandoComRetorno(query);
            return chamados[0]["nome"];
        }

        public void AtualizaNumerario(string Valor, string Observacao, string Agencia, string usuarioAlteracao)
        {
            var query = "UPDATE numerariosagencias SET valor='" + Valor + "',idfuncionarioalteracao=" +
                        usuarioAlteracao + ",observacao='" + Observacao + "',dataalteracao=NOW() WHERE idagencia=" +
                        Agencia + "";
            _conexaoMysql.ExecutaComando(query);
        }

        public bool PermissaoControleTesouraria(string usuario)
        {
            var query =
                "select a.valor from permissoesgrupo a, funcionarios b, grupos c where a.idgrupo = c.id and b.idgrupo = c.id and b.login='" +
                usuario + "' and a.idaplicativo=12";

            var dados = _conexaoMysql.ExecutaComandoComRetorno(query);
            try
            {
                if (dados[0]["valor"].Equals("S"))
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool PermissaoControleFuncionario(string usuario)
        {
            var query =
                "select a.valor from permissoesfuncionarios a, funcionarios b where a.idfuncionario = b.id and b.id = a.idfuncionario and b.login='" +
                usuario + "' and a.idaplicativo=12";

            var dados = _conexaoMysql.ExecutaComandoComRetorno(query);
            try
            {
                if (dados[0]["valor"].Equals("S"))
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}