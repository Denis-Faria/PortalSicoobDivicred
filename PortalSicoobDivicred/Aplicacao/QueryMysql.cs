using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Spreadsheet;
using Port.Repositorios;
using PortalSicoobDivicred.Models;

namespace PortalSicoobDivicred.Aplicacao
{
    public class QueryMysql
    {
        private readonly Conexao ConexaoMysql;


        public QueryMysql()
        {
            ConexaoMysql = new Conexao();
        }

        public bool ConfirmaLogin(string Usuario, string Senha)
        {
            var QueryConfirmaLogin = "SELECT login FROM funcionarios WHERE login='" + Usuario + "' AND senha=MD5('" +
                                     Senha +
                                     "')";

            var Dados = ConexaoMysql.ExecutaComandoComRetorno(QueryConfirmaLogin);

            if (Dados.Count == 0)
                return false;
            var CookieUsuario = new HttpCookie("CookieFarm");
            CookieUsuario.Value = Criptografa.Criptografar(Dados[0]["login"]);
            CookieUsuario.Expires = DateTime.Now.AddHours(1);
            HttpContext.Current.Response.Cookies.Add(CookieUsuario);
            return true;
        }

        internal bool TrocaSenha(string Usuario)
        {
            var QueryConfirmaLogin = "SELECT trocasenha FROM funcionarios WHERE login='" + Usuario + "' ;";

            var Dados = ConexaoMysql.ExecutaComandoComRetorno(QueryConfirmaLogin);
            try
            {
                if (Dados[0]["trocasenha"].Equals("S"))
                    return false;
                return true;
            }
            catch
            {
                return true;
            }
        }

        public bool PrimeiroLogin(string Usuario)
        {
            var Query = "SELECT perfilcompleto FROM funcionarios WHERE login='" + Usuario + "';";

            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);

            if (Dados[0]["perfilcompleto"].Equals("S"))
                return false;
            return true;
        }

        public bool PermissaoCurriculos(string Usuario)
        {
            var Query =
                "select a.valor from permissoesgrupo a, usuarios b, grupos c where a.idgrupo = c.id and b.idgrupo = c.id and b.login='" +
                Usuario + "' and a.idaplicativo=9";

            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            try
            {
                if (Dados[0]["valor"].Equals("S"))
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool PermissaoTesouraria(string Usuario)
        {
            var Query =
                "select a.valor from permissoesgrupo a, usuarios b, grupos c where a.idgrupo = c.id and b.idgrupo = c.id and b.login='" +
                Usuario + "' and a.idaplicativo=10";

            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            try
            {
                if (Dados[0]["valor"].Equals("S"))
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        public List<Dictionary<string, string>> RecuperaDocumentosFuncionario(string Login)
        {
            var Query =
                "SELECT * FROM documentospessoaisfuncionarios WHERE idfuncionario=(SELECT id FROM funcionarios where login='" +
                Login + "')";
            var DadosCurriculos = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return DadosCurriculos;
        }
        public List<Dictionary<string, string>> RecuperaVinculoExtra(string IdFuncionario)
        {
            var Query =
                "SELECT * FROM vinculosempregaticiosfuncionarios WHERE idfuncionario="+IdFuncionario+"";
            var DadosCurriculos = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return DadosCurriculos;
        }

        public void InserirFormacao(string Formacao, string IdFuncionario)
        {
            var QueryFormacao = "INSERT INTO formacoesfuncionarios (idfuncionario,descricao) VALUES('" +
                                IdFuncionario + "','" + Formacao + "')";
            ConexaoMysql.ExecutaComandoComRetorno(QueryFormacao);
        }

        public void AtualizaFormacao(string Formacao, string Id)
        {
            var QueryFormacao = "UPDATE formacoesfuncionarios  SET descricao='" + Formacao + "' WHERE id='" + Id + "'";
            ConexaoMysql.ExecutaComandoComRetorno(QueryFormacao);
        }

        public bool UsuarioLogado()
        {
            var Usuario = HttpContext.Current.Request.Cookies["CookieFarm"];
            if (Usuario == null)
                return false;
            return true;
        }

        public List<Dictionary<string, string>> RecuperaDadosUsuarios(string Login)
        {
            var QueryRecuperaUsuario =
                "SELECT * FROM funcionarios  WHERE login='" + Login + "'";


            var Dados = ConexaoMysql.ExecutaComandoComRetorno(QueryRecuperaUsuario);


            return Dados;
        }

        public List<Dictionary<string, string>> RecuperaDadosFuncionariosSetor()
        {
            var QueryRecuperaUsuario =
                "SELECT a.*,b.descricao as setor  FROM funcionarios a, setores b where a.idsetor=b.id and a.ativo='S'";


            var Dados = ConexaoMysql.ExecutaComandoComRetorno(QueryRecuperaUsuario);


            return Dados;
        }

        public List<Dictionary<string, string>> RecuperaDadosFuncionariosTabelaUsuario(string Usuario)
        {
            var Query =
                "SELECT * FROM usuarios WHERE login='" + Usuario + "'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);


            return Dados;
        }

        public List<Dictionary<string, string>> RecuperaDadosFuncionariosTabelaFuncionarios(string NomeFuncionario)
        {
            var Query =
                "SELECT * FROM funcionarios where (soundex(nome)like concat('%',soundex('" + NomeFuncionario +
                "'),'%')) ";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);


            return Dados;
        }
        public List<Dictionary<string, string>> RecuperaDadosFuncionariosTabelaFuncionariosLogin(string Login)
        {
            var Query =
                "SELECT * FROM funcionarios where login='" + Login + "'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);


            return Dados;
        }

        public List<Dictionary<string, string>> RecuperaDadosFuncionariosTabelaFuncionariosPerfil(string UsuarioSistema)
        {
            var Query =
                "SELECT * FROM funcionarios WHERE login='" + UsuarioSistema + "'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);


            return Dados;
        }

        public List<Dictionary<string, string>> RetornaCertificacao(string IdCertificao)
        {
            var Query =
                "SELECT descricao FROM certificacoesfuncionarios WHERE id='" + IdCertificao + "'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);


            return Dados;
        }

        public List<Dictionary<string, string>> RetornaCertificacaoFuncao(string IdFuncao)
        {
            var Query =
                "SELECT idcertificacao FROM funcoes WHERE id='" + IdFuncao + "'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);


            return Dados;
        }

        public List<Dictionary<string, string>> RetornaFormacaoFuncionario(string IdFuncionario)
        {
            var Query =
                "SELECT * FROM formacoesfuncionarios WHERE idfuncionario='" + IdFuncionario + "'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);


            return Dados;
        }

        public string RetornaFuncaoFuncionario(string IdFuncao)
        {
            var Query =
                "SELECT descricao FROM funcoes WHERE id='" + IdFuncao + "'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);


            return Dados[0]["descricao"];
        }

        public string RetornaGeneroFuncionario(string IdGenero)
        {
            var Query =
                "SELECT descricao FROM tipossexos WHERE id='" + IdGenero + "'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);


            return Dados[0]["descricao"];
        }

        public string RetornaSetorFuncionario(string IdSetor)
        {
            var Query =
                "SELECT descricao FROM setores WHERE id='" + IdSetor + "'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);


            return Dados[0]["descricao"];
        }

        public string RetornaEscolaridadeFuncionario(string IdEscolaridade)
        {
            var Query =
                "SELECT descricao FROM tiposformacoes WHERE id='" + IdEscolaridade + "'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);


            return Dados[0]["descricao"];
        }

        public string RetornaEstadoCivilFuncionario(string IdEstadoCivil)
        {
            var Query =
                "SELECT descricao FROM tiposestadoscivis WHERE id='" + IdEstadoCivil + "'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);


            return Dados[0]["descricao"];
        }

        public string RetornaEtiniaFuncionario(string Etinia)
        {
            var Query =
                "SELECT descricao FROM tiposetnias WHERE id='" + Etinia + "'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);


            return Dados[0]["descricao"];
        }

        public List<SelectListItem> RetornaEstadoCivil()
        {
            var EstadoCivil = new List<SelectListItem>();

            const string QueryRetornaEstadoCivil = "SELECT id,descricao FROM tiposestadoscivis";

            var Dados = ConexaoMysql.ExecutaComandoComRetorno(QueryRetornaEstadoCivil);
            foreach (var row in Dados)
                EstadoCivil.Add(new SelectListItem
                {
                    Value = row["id"],
                    Text = row["descricao"]
                });

            return EstadoCivil;
        }

        public List<SelectListItem> RetornaTipoConta()
        {
            var TipoConta = new List<SelectListItem>();

            const string QueryRetornaTipoConta = "SELECT id,descricao FROM tiposcontas";

            var Dados = ConexaoMysql.ExecutaComandoComRetorno(QueryRetornaTipoConta);
            foreach (var row in Dados)
                TipoConta.Add(new SelectListItem
                {
                    Value = row["id"],
                    Text = row["descricao"]
                });

            return TipoConta;
        }

        public List<SelectListItem> RetornaSexo()
        {
            var Sexo = new List<SelectListItem>();

            const string QueryRetornaSexo = "SELECT id,descricao FROM tipossexos";

            var Dados = ConexaoMysql.ExecutaComandoComRetorno(QueryRetornaSexo);
            foreach (var row in Dados)
                Sexo.Add(new SelectListItem
                {
                    Value = row["id"],
                    Text = row["descricao"]
                });

            return Sexo;
        }



        

        
       

        public List<Dictionary<string, string>> RecuperaPontuacaoFuncionariosSetor(string UsuarioSistema)
        {
            var Query1 =
                "SELECT idsetor from funcionarios WHERE usuario='" + UsuarioSistema;
            var idSetor = ConexaoMysql.ExecutaComandoComRetorno(Query1);

            var Query =
                "SELECT * from funcionarios where idsetor='" + idSetor + "' and login !='" + UsuarioSistema + "'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);

            return Dados;
        }

        

        public string RecuperaDadosPontos(int idProduto)
        {
            var Query =
                "select valorminimo,peso from pgdprodutos where id='" + idProduto + "'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);


            return Dados[0]["descricao"];
        }


        

        public List<SelectListItem> RetornaFuncionario()
        {
            var Funcionario = new List<SelectListItem>();

            const string QueryRetornaFuncionario = "SELECT id,nome FROM funcionarios";

            var Dados = ConexaoMysql.ExecutaComandoComRetorno(QueryRetornaFuncionario);
            foreach (var row in Dados)
                Funcionario.Add(new SelectListItem
                {
                    Value = row["id"],
                    Text = row["nome"]
                });

            return Funcionario;
        }

        


        

        public string RecuperaUsuario(string login)
        {
            var QueryRecuperaUsuario = "SELECT id from usuarios where login='" + login + "'";

            var id = ConexaoMysql.ExecutaComandoComRetorno(QueryRecuperaUsuario);

            return id[0]["id"];
        }

        public string RecuperaUsuarioLogin(string usuario)
        {
            var QueryRecuperaUsuarioLogin = "SELECT login from usuarios where id='" + usuario + "'";

            var id = ConexaoMysql.ExecutaComandoComRetorno(QueryRecuperaUsuarioLogin);

            return id[0]["login"];
        }


        public List<SelectListItem> RetornaEtnia()
        {
            var Etnia = new List<SelectListItem>();

            const string QueryRetornaEtnia = "SELECT id,descricao FROM tiposetnias";

            var Dados = ConexaoMysql.ExecutaComandoComRetorno(QueryRetornaEtnia);
            foreach (var row in Dados)
                Etnia.Add(new SelectListItem
                {
                    Value = row["id"],
                    Text = row["descricao"]
                });

            return Etnia;
        }

        public List<SelectListItem> RetornaFormacao()
        {
            var Formacao = new List<SelectListItem>();

            const string QueryRetornaFormacao = "SELECT id,descricao FROM tiposformacoes";

            var Dados = ConexaoMysql.ExecutaComandoComRetorno(QueryRetornaFormacao);
            foreach (var row in Dados)
                Formacao.Add(new SelectListItem
                {
                    Value = row["id"],
                    Text = row["descricao"]
                });

            return Formacao;
        }

        public List<SelectListItem> RetornaSetor()
        {
            var Setor = new List<SelectListItem>();

            const string QueryRetornaSetor = "SELECT id,descricao FROM setores";

            var Dados = ConexaoMysql.ExecutaComandoComRetorno(QueryRetornaSetor);
            foreach (var row in Dados)
                Setor.Add(new SelectListItem
                {
                    Value = row["id"],
                    Text = row["descricao"]
                });

            return Setor;
        }

        public List<SelectListItem> RetornaFuncao()
        {
            var Funcao = new List<SelectListItem>();

            const string Query = "SELECT id,descricao FROM funcoes";

            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            foreach (var row in Dados)
                Funcao.Add(new SelectListItem
                {
                    Value = row["id"],
                    Text = row["descricao"]
                });

            return Funcao;
        }

        public void AtualizaDadosFuncionarioFormulario(string Nome, string Cpf, string Rg, string Pis,
            string DataNascimentoFuncionario, string Sexo, string DescricaoSexo, string Etnia, string EstadoCivil,
            string Formacao, string FormacaoAcademica, string UsuarioSistema, string Email, string PA, string Rua,
            string Numero, string Bairro, string Cidade, string Setor, string Funcao, string QuatidadeFilhos,
            string DataNascimentoFilhos, string Emergencia, string PrincipaisHobbies, string ComidaFavorita,
            string Viagem, string ConfirmacaoCertificacao, string ConfirmaDados,string Nacionalidade,string NomeMae,
            string NomePai,string LocalNascimento,string UfNascimento,string Complemento,string Cep,string Pais,
            string ResidenciaPropria,string RecursoFgts,string NumeroCtps,string SerieCtps,string UfCtps,string TelefoneFixo,
            string TelefoneCelular,string EmailSecundario,string Cnh,string OrgaoCnh,DateTime DataExpedicaoCnh,DateTime DataValidadeCnh,
            string Oc,string OrgaoOc,DateTime DataExpedicaoOc,DateTime DataValidadeOc,string DeficienteMotor,string DeficienteVisual,
            string DeficienteAuditivo,string Reabilitado,string ObservacaoDeficiente,int IdTipoConta,string CodigoBanco,string Agencia,
            string ContaCorrente,string DependenteIrrf,string DependenteFamilia,string DadosDependentes,string TipoDependentes,string Matricula,
            string PrimeiroEmprego, string EmissaoCtps,string Divorcio,string OrgaoEmissorRg,DateTime DataEmissaoRg,string CpfIrrf,
            string NotificacaoEmail,string ContribuicaoSindical)
        {
            var QueryAtualizaFuncionario = "UPDATE funcionarios SET nome='" + Nome + "', cpf='" + Cpf + "',rg='" +
                                           Rg + "', pis='" + Pis + "',datanascimento='" +
                                           Convert.ToDateTime(DataNascimentoFuncionario).ToString("yyyy/MM/dd") +
                                           "', sexo=" + Sexo + ",descricaosexo='" + DescricaoSexo + "',etnia=" +
                                           Etnia + ",idestadocivil=" + EstadoCivil + ",  idescolaridade=" +
                                           Formacao + ", formacaoacademica='" + FormacaoAcademica + "', login='" +
                                           UsuarioSistema + "', email='" + Email + "', idpa=" + PA + ", rua='" +
                                           Rua + "',numero=" + Numero + ",bairro='" + Bairro + "',cidade='" +
                                           Cidade + "', idsetor=" + Setor + ", funcao='" + Funcao +
                                           "', quantidadefilho='" + QuatidadeFilhos + "',datanascimentofilho='" +
                                           DataNascimentoFilhos + "', contatoemergencia='" + Emergencia +
                                           "', principalhobbie='" + PrincipaisHobbies + "', comidafavorita='" +
                                           ComidaFavorita + "',viagem='" + Viagem +
                                           "', perfilcompleto='S',confirmacaodado='" + ConfirmaDados +
                                           "',confirmacaocertificacao='" +
                                           ConfirmacaoCertificacao + "',nacionalidade='"+Nacionalidade+"'," +
                                           "nomemae='"+NomeMae+"',nomepai='"+NomePai+"',localnascimento='"+LocalNascimento+"'" +
                                           ",ufnascimento='"+UfNascimento+"',complemento='"+Complemento+"',cep='"+Cep+"'" +
                                           ",pais='"+Pais+"',residenciapropria='"+ResidenciaPropria+"',recursofgts='"+RecursoFgts+"'" +
                                           ",numeroctps='"+NumeroCtps+"',seriectps='"+SerieCtps+"',ufctps='"+UfCtps+"', telefonefixo='"+TelefoneFixo+"'" +
                                           ",telefonecelular='"+TelefoneCelular+"',emailsecundario='"+EmailSecundario+"',cnh='"+Cnh+"',orgaoemissorcnh='"+OrgaoCnh+"'" +
                                           ",dataexpedicaocnh='"+DataExpedicaoCnh.ToString("yyyy/MM/dd")+"',datavalidadecnh='"+DataValidadeCnh.ToString("yyyy/MM/dd")+"'," +
                                           "oc='"+Oc+"',orgaoemissoroc='"+OrgaoOc+ "',dataexpedicaooc='" + DataExpedicaoOc.ToString("yyyy/MM/dd") + "'" +
                                           ",datavalidadeoc='" + DataValidadeOc.ToString("yyyy/MM/dd") + "',deficientemotor='"+DeficienteMotor+"',deficientevisual='"+DeficienteVisual+"'" +
                                           ",deficienteauditivo='"+DeficienteAuditivo+"',reabilitado='"+Reabilitado+"',observacaodeficiente='"+ObservacaoDeficiente+"', idtipoconta="+IdTipoConta+"," +
                                           "codigobanco="+CodigoBanco+",agencia='"+Agencia+"',contacorrente='"+ContaCorrente+"', informacaodependente='"+DadosDependentes+ "',dependenteirrpf='" + DependenteIrrf+"'," +
                                           "dependentesalariofamilia='"+DependenteFamilia+"',tipodependente='"+TipoDependentes+"',matricula='"+Matricula+"',anoprimeiroemprego='"+PrimeiroEmprego+"'," +
                                           "dataemissaoctps='"+Convert.ToDateTime(EmissaoCtps).Date.ToString("yyyy/MM/dd")+"',paisdivorciado='"+Divorcio+ "'," +
                                           " dataemissaorg='" + DataEmissaoRg.Date.ToString("yyyy/MM/dd") + "',orgaoemissorrg='" + OrgaoEmissorRg + "', cpfirrf='" + CpfIrrf + "'," +
                                           "contribuicaosindical='"+ContribuicaoSindical+"',notificacaoemail='"+NotificacaoEmail+"'" +
                                           " WHERE login='" + UsuarioSistema + "'";
            ConexaoMysql.ExecutaComandoComRetorno(QueryAtualizaFuncionario);
        }

        public void InserirVinculoEmpregaticio(string IdFuncionario, string NomeEmpresa, string Cnpj,
            string Remuneracao, string Comentario)
        {
            var QueryInserirVinculo =
                "INSERT INTO vinculosempregaticiosfuncionarios (idfuncionario,nomeempresa,cnpj,remuneracao,comentario) VALUES(" +
                IdFuncionario + ",'" + NomeEmpresa + "','" + Cnpj + "','" + Remuneracao + "','" + Comentario + "')";

            ConexaoMysql.ExecutaComando(QueryInserirVinculo);
        }
        public void AtualizaVinculoEmpregaticio(string IdFuncionario, string NomeEmpresa, string Cnpj,
            string Remuneracao, string Comentario)
        {
            var QueryInserirVinculo =
                "UPDATE vinculosempregaticiosfuncionarios set nomeempresa='" + NomeEmpresa + "',cnpj='" + Cnpj + "',remuneracao='" + Remuneracao + "',comentario='" + Comentario + "' WHERE idfuncionario="+IdFuncionario+"";

            ConexaoMysql.ExecutaComando(QueryInserirVinculo);
        }
        public void AtualizaDadosFuncionarioDadosPessoais(string Nome, string Cpf, string Rg, string Pis,
            string DataNascimentoFuncionario, string Sexo, string DescricaoSexo, string Etnia, string EstadoCivil,
            string Formacao, string FormacaoAcademica, string UsuarioSistema, string Email, string PA, string Rua,
            string Numero, string Bairro, string Cidade, string ConfirmaDados,      string Nacionalidade,string NomeMae,
            string NomePai, string LocalNascimento, string UfNascimento, string Complemento, string Cep, string Pais, 
            string ResidenciaPropria, string RecursoFgts, string TelefoneFixo, string TelefoneCelular,string EmailSecundario, 
            string Cnh, string OrgaoEmissorCnh, DateTime DataExpedicaoDocumentoCnh, DateTime DataValidadeCnh, string Oc, string OrgaoEmissorOc, 
            DateTime DataExpedicaoOc, DateTime DataValidadeOc, string DeficienteMotor, string DeficienteVisual, string DeficienteAuditivo,
            string Reabilitado, string ObservacaoDeficiente, string PaisDivorciados, string OrgaoEmissorRg, DateTime DataExpedicaoDocumentoRg)
        {
            var QueryAtualizaFuncionario = "UPDATE funcionarios SET nome='" + Nome + "', cpf='" + Cpf + "',rg='" +
                                           Rg + "', pis='" + Pis + "',datanascimento='" +
                                           Convert.ToDateTime(DataNascimentoFuncionario).ToString("yyyy/MM/dd") +
                                           "', sexo=" + Sexo + ",descricaosexo='" + DescricaoSexo + "',etnia=" +
                                           Etnia + ",idestadocivil=" + EstadoCivil + ",  idescolaridade=" +
                                           Formacao + ", formacaoacademica='" + FormacaoAcademica + "', login='" +
                                           UsuarioSistema + "', email='" + Email + "', idpa=" + PA + ", rua='" +
                                           Rua + "',numero=" + Numero + ",bairro='" + Bairro + "',cidade='" +
                                           Cidade + "', confirmacaodado='" + ConfirmaDados + "',nacionalidade='"+Nacionalidade+"'," +
                                           "nomemae='"+NomeMae+"',nomepai='"+NomePai+"',localnascimento='"+LocalNascimento+"'" +
                                           ",ufnascimento='"+UfNascimento+"',complemento='"+Complemento+"',cep='"+Cep+"'" +
                                           ",pais='"+Pais+"',residenciapropria='"+ResidenciaPropria+"',recursofgts='"+RecursoFgts+"'" +
                                           ",telefonefixo='"+TelefoneFixo+"'" +",telefonecelular='"+TelefoneCelular+"',emailsecundario='"+EmailSecundario+"'" +
                                           ",cnh='"+Cnh+"',orgaoemissorcnh='"+OrgaoEmissorCnh+"'" +
                                           ",dataexpedicaocnh='"+DataExpedicaoDocumentoCnh.ToString("yyyy/MM/dd")+"',datavalidadecnh='"+DataValidadeCnh.ToString("yyyy/MM/dd")+"'," +
                                           "oc='"+Oc+"',orgaoemissoroc='"+OrgaoEmissorOc+ "',dataexpedicaooc='" + DataExpedicaoOc.ToString("yyyy/MM/dd") + "'" +
                                           ",datavalidadeoc='" + DataValidadeOc.ToString("yyyy/MM/dd") + "',deficientemotor='"+DeficienteMotor+"',deficientevisual='"+DeficienteVisual+"'" +
                                           ",deficienteauditivo='"+DeficienteAuditivo+"',reabilitado='"+Reabilitado+"',observacaodeficiente='"+ObservacaoDeficiente+"'"+
                                           ",paisdivorciado='"+PaisDivorciados+ "', dataemissaorg='" + DataExpedicaoDocumentoRg.ToString("yyyy/MM/dd") + "',orgaoemissorrg='" + OrgaoEmissorRg + "'" +
                                           "  WHERE login='"+UsuarioSistema + "'";
            ConexaoMysql.ExecutaComandoComRetorno(QueryAtualizaFuncionario);
        }

        public void AtualizaDadosFuncionarioProfissional(string Setor, string Funcao, string UsuarioSistema, string NumeroCTPS,
            string SerieCTPS, string UfCTPS, int IdTipoConta, string CodigoBanco,
            string Agencia, string ContaCorrente, string DependenteIrrf, string DependenteFamilia,
            string DadosDependentes, string TiposDependentes, string Matricula, string AnoPrimeiroEmprego, string EmissaoCtps,
            string CpfIrrf)
        {
            var QueryAtualizaFuncionario = "UPDATE funcionarios SET idsetor=" + Setor + ", funcao='" + Funcao +
                                           "',numeroctps='" + NumeroCTPS + "',seriectps='" + SerieCTPS + "',ufctps='" + UfCTPS + "', idtipoconta=" + IdTipoConta + "," +
                                           "codigobanco=" + CodigoBanco + ",agencia='" + Agencia + "',contacorrente='" + ContaCorrente + "', informacaodependente='" + DadosDependentes + "',dependenteirrpf='" + DependenteIrrf + "'," +
                                           "dependentesalariofamilia='" + DependenteFamilia + "',tipodependente='" + TiposDependentes + "',matricula='" + Matricula + "'" +
                                           ",anoprimeiroemprego='" + AnoPrimeiroEmprego + "', dataemissaoctps='" + Convert.ToDateTime(EmissaoCtps).Date.ToString("yyyy/MM/dd") + "', cpfirrf='" + CpfIrrf + "' WHERE login='" + UsuarioSistema + "'";
            ConexaoMysql.ExecutaComandoComRetorno(QueryAtualizaFuncionario);
        }

        public void AtualizaFoto(string Foto, string UsuarioSistema)
        {
            var QueryAtualizaFuncionario =
                "UPDATE funcionarios SET foto='" + Foto + "' WHERE login='" + UsuarioSistema + "'";
            ConexaoMysql.ExecutaComandoComRetorno(QueryAtualizaFuncionario);
        }

        public void AtualizaDadosFuncionarioPerguntas(string UsuarioSistema, string QuatidadeFilhos,
            string DataNascimentoFilhos, string Emergencia, string PrincipaisHobbies, string ComidaFavorita,
            string Viagem,string Sindicato,string Email)
        {
            var QueryAtualizaFuncionario = "UPDATE funcionarios SET quantidadefilho='" + QuatidadeFilhos +
                                           "',datanascimentofilho='" +
                                           DataNascimentoFilhos + "', contatoemergencia='" + Emergencia +
                                           "', principalhobbie='" + PrincipaisHobbies + "', comidafavorita='" +
                                           ComidaFavorita + "',viagem='" + Viagem +
                                           "', perfilcompleto='S',contribuicaosindical = '"+Sindicato+"',notificacaoemail = '"+Email+"' WHERE login='" + UsuarioSistema + "'";
            ConexaoMysql.ExecutaComandoComRetorno(QueryAtualizaFuncionario);
        }

        public void AtualizarArquivoPessoal(string NomeArquivo, byte[] Foto, string Login)
        {
            var QueryIdFuncionario = "SELECT id FROM funcionarios WHERE login='" + Login + "'";
            var row = ConexaoMysql.ExecutaComandoComRetorno(QueryIdFuncionario);

            var QueryArquivoUsuario =
                "SELECT count(id) as count FROM documentospessoaisfuncionarios WHERE idfuncionario='" + row[0]["id"] +
                "' and nomearquivo='" + NomeArquivo + "'";
            var row2 = ConexaoMysql.ExecutaComandoComRetorno(QueryArquivoUsuario);

            if (Convert.ToInt32(row2[0]["count"]) == 0)
            {
                var QueryAtualizaFuncionario =
                    "INSERT INTO documentospessoaisfuncionarios (idfuncionario,nomearquivo,arquivo,dataupload) VALUES(" +
                    row[0]["id"] + ",'" + NomeArquivo + "',@image ,NOW()) ";
                ConexaoMysql.ExecutaComandoArquivo(QueryAtualizaFuncionario, Foto);
            }
            else
            {
                var QueryAtualizaFuncionario =
                    "UPDATE documentospessoaisfuncionarios SET arquivo=@image and dataupload=NOW() WHERE idfuncionario=" +
                    row[0]["id"] + " AND nomearquivo='" + NomeArquivo + "'";
                ConexaoMysql.ExecutaComandoArquivo(QueryAtualizaFuncionario, Foto);
            }
        }

        public List<Dictionary<string, string>> RecuperaTodosArquivos(string Login)
        {
            var Query =
                "SELECT nomearquivo, dataupload FROM documentospessoaisfuncionarios WHERE login='" + Login + "'";
            var row = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return row;
        }

        public DataTable RetornaDocumentosFuncionario(string Login)
        {
            var Dados = ConexaoMysql.ComandoArquivo(Login);
            return Dados;
        }

        public void cadastrarAlert(string IdUsuario, string IdAplicativo, string Descricao)
        {
            var Query = "INSERT INTO alertas (idusuario,idaplicativo,descricao,data) VALUES(" + IdUsuario + "," +
                        IdAplicativo + ",'" + Descricao + "',NOW())";
            ConexaoMysql.ExecutaComando(Query);
        }

        public List<Dictionary<string, string>> RetornaHoras(string Cracha)
        {
            var Query = "select hora,datareferencia from horasextrasfuncionarios WHERE crachafirebird=" + Cracha + ";";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
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
            var Query =
                "SELECT nome,idnotificacao,notificacaoemail,id,email FROM funcionarios WHERE idsetor='" + IdSetor + "'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);


            return Dados;
        }

        public void AtualizaEmailSenha(string IdUsuario)
        {
            var Query = "UPDATE funcionarios SET emailtrocasenha='S' WHERE id='" + IdUsuario + "'";
            ConexaoMysql.ExecutaComandoComRetorno(Query);
        }

        public void AtualizaSenha(string Usuario, string Senha)
        {
            var Query = "UPDATE funcionarios SET senha='" + Senha + "', emailtrocasenha='N' WHERE login='" + Usuario +
                        "'";
            ConexaoMysql.ExecutaComandoComRetorno(Query);
        }

        public List<Dictionary<string, string>> RetornaEmailSenha(string Usuario)
        {
            var Query =
                "SELECT emailtrocasenha FROM funcionarios WHERE login='" + Usuario + "'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);


            return Dados;
        }

        public List<Dictionary<string, string>> BuscaNumerodaSorte()
        {
            var Query = "SELECT * FROM showdepremiosnumerosorte order by data LIMIT 1;";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }

        public List<Dictionary<string, string>> RetornaExtrato(string Cpf)
        {
            var Query = "SELECT* FROM showdepremioscupons WHERE  numdoccliente = " + Cpf + "";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }

        public void CadastraIdNotificacao(string IdNotificacao, string Login)
        {
            var Query = "UPDATE funcionarios set idnotificacao='"+IdNotificacao+"' WHERE login='"+Login+"'";
            ConexaoMysql.ExecutaComando(Query);
        }
        public List<Dictionary<string, string>> RetornaInformacoesNotificacao(string IdFuncionario)
        {
            var Query = "select email,idnotificacao,notificacaoemail,idsetor,nome from funcionarios where id=" + IdFuncionario + " ";
            var Chamados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Chamados;
        }

        public List<Dictionary<string, string>> RetornaInformacoesGestor(string IdSetor)
        {
            var Query = "select id,email,idnotificacao,notificacaoemail from funcionarios where idsetor=" + IdSetor + " and gestor='S' ";
            var Chamados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Chamados;
        }
        public List<Dictionary<string, string>> RetornaInformacoesNumerario(string IdAgencia)
        {
            var Query = "select * from numerariosagencias WHERE idagencia="+IdAgencia+" ";
            var Chamados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Chamados;
        }
        public string RetornaNomeFuncionario(string IdFuncionario)
        {
            var Query = "select nome from funcionarios WHERE id=" + IdFuncionario + " ";
            var Chamados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Chamados[0]["nome"];
        }

        public void AtualizaNumerario(string Valor, string Observacao, string Agencia, string UsuarioAlteracao)
        {
            var Query = "UPDATE numerariosagencias SET valor='"+Valor+"',idfuncionarioalteracao="+UsuarioAlteracao+",observacao='"+Observacao+"',dataalteracao=NOW() WHERE idagencia="+Agencia+"";
            ConexaoMysql.ExecutaComando(Query);
        }
        public bool PermissaoControleTesouraria(string Usuario)
        {
            var Query =
                "select a.valor from permissoesgrupo a, funcionarios b, grupos c where a.idgrupo = c.id and b.idgrupo = c.id and b.login='" +
                Usuario + "' and a.idaplicativo=12";

            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            try
            {
                if (Dados[0]["valor"].Equals("S"))
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }
        public bool PermissaoControleFuncionario(string Usuario)
        {
            var Query =
                "select a.valor from permissoesfuncionarios a, funcionarios b where a.idfuncionario = b.id and b.id = a.idfuncionario and b.login='" +
                Usuario + "' and a.idaplicativo=12";

            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            try
            {
                if (Dados[0]["valor"].Equals("S"))
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