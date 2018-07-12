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
            string dataNascimentoFuncionario, string sexo, string descricaoSexo, string etnia, string estadoCivil,
            string formacao, string formacaoAcademica, string usuarioSistema, string email, string pa, string rua,
            string numero, string bairro, string cidade, string setor, string funcao, string quatidadeFilhos,
            string dataNascimentoFilhos, string emergencia, string principaisHobbies, string comidaFavorita,
            string viagem, string confirmacaoCertificacao, string confirmadados, string nacionalidade, string nomeMae,
            string nomePai, string localNascimento, string ufNascimento, string complemento, string cep, string pais,
            string residenciaPropria, string recursoFgts, string numeroCtps, string serieCtps, string ufCtps,
            string telefoneFixo,
            string telefoneCelular, string emailSecundario, string cnh, string orgaoCnh, DateTime dataExpedicaoCnh,
            DateTime dataValidadeCnh,
            string oc, string orgaoOc, DateTime dataExpedicaoOc, DateTime dataValidadeOc, string deficienteMotor,
            string deficienteVisual,
            string deficienteAuditivo, string reabilitado, string observacaoDeficiente, int idTipoConta,
            string codigoBanco, string agencia,
            string contaCorrente, string dependenteIrrf, string dependenteFamilia, string dadosDependentes,
            string tipoDependentes, string matricula,
            string primeiroEmprego, string emissaoCtps, string divorcio, string orgaoEmissorRg, DateTime dataEmissaoRg,
            string cpfIrrf,
            string notificacaoEmail, string contribuicaoSindical)
        {
            var queryAtualizaFuncionario = "UPDATE funcionarios SET nome='" + nome + "', cpf='" + cpf + "',rg='" +
                                           rg + "', pis='" + pis + "',datanascimento='" +
                                           Convert.ToDateTime(dataNascimentoFuncionario).ToString("yyyy/MM/dd") +
                                           "', sexo=" + sexo + ",descricaosexo='" + descricaoSexo + "',etnia=" +
                                           etnia + ",idestadocivil=" + estadoCivil + ",  idescolaridade=" +
                                           formacao + ", formacaoacademica='" + formacaoAcademica + "', login='" +
                                           usuarioSistema + "', email='" + email + "', idpa=" + pa + ", rua='" +
                                           rua + "',numero=" + numero + ",bairro='" + bairro + "',cidade='" +
                                           cidade + "', idsetor=" + setor + ", funcao='" + funcao +
                                           "', quantidadefilho='" + quatidadeFilhos + "',datanascimentofilho='" +
                                           dataNascimentoFilhos + "', contatoemergencia='" + emergencia +
                                           "', principalhobbie='" + principaisHobbies + "', comidafavorita='" +
                                           comidaFavorita + "',viagem='" + viagem +
                                           "', perfilcompleto='S',confirmacaodado='" + confirmadados +
                                           "',confirmacaocertificacao='" +
                                           confirmacaoCertificacao + "',nacionalidade='" + nacionalidade + "'," +
                                           "nomemae='" + nomeMae + "',nomepai='" + nomePai + "',localnascimento='" +
                                           localNascimento + "'" +
                                           ",ufnascimento='" + ufNascimento + "',complemento='" + complemento +
                                           "',cep='" + cep + "'" +
                                           ",pais='" + pais + "',residenciapropria='" + residenciaPropria +
                                           "',recursofgts='" + recursoFgts + "'" +
                                           ",numeroctps='" + numeroCtps + "',seriectps='" + serieCtps + "',ufctps='" +
                                           ufCtps + "', telefonefixo='" + telefoneFixo + "'" +
                                           ",telefonecelular='" + telefoneCelular + "',emailsecundario='" +
                                           emailSecundario + "',cnh='" + cnh + "',orgaoemissorcnh='" + orgaoCnh + "'" +
                                           ",dataexpedicaocnh='" + dataExpedicaoCnh.ToString("yyyy/MM/dd") +
                                           "',datavalidadecnh='" + dataValidadeCnh.ToString("yyyy/MM/dd") + "'," +
                                           "oc='" + oc + "',orgaoemissoroc='" + orgaoOc + "',dataexpedicaooc='" +
                                           dataExpedicaoOc.ToString("yyyy/MM/dd") + "'" +
                                           ",datavalidadeoc='" + dataValidadeOc.ToString("yyyy/MM/dd") +
                                           "',deficientemotor='" + deficienteMotor + "',deficientevisual='" +
                                           deficienteVisual + "'" +
                                           ",deficienteauditivo='" + deficienteAuditivo + "',reabilitado='" +
                                           reabilitado + "',observacaodeficiente='" + observacaoDeficiente +
                                           "', idtipoconta=" + idTipoConta + "," +
                                           "codigobanco=" + codigoBanco + ",agencia='" + agencia + "',contacorrente='" +
                                           contaCorrente + "', informacaodependente='" + dadosDependentes +
                                           "',dependenteirrpf='" + dependenteIrrf + "'," +
                                           "dependentesalariofamilia='" + dependenteFamilia + "',tipodependente='" +
                                           tipoDependentes + "',matricula='" + matricula + "',anoprimeiroemprego='" +
                                           primeiroEmprego + "'," +
                                           "dataemissaoctps='" +
                                           Convert.ToDateTime(emissaoCtps).Date.ToString("yyyy/MM/dd") +
                                           "',paisdivorciado='" + divorcio + "'," +
                                           " dataemissaorg='" + dataEmissaoRg.Date.ToString("yyyy/MM/dd") +
                                           "',orgaoemissorrg='" + orgaoEmissorRg + "', cpfirrf='" + cpfIrrf + "'," +
                                           "contribuicaosindical='" + contribuicaoSindical + "',notificacaoemail='" +
                                           notificacaoEmail + "'" +
                                           " WHERE login='" + usuarioSistema + "'";
            _conexaoMysql.ExecutaComandoComRetorno(queryAtualizaFuncionario);
        }

        public void InserirVinculoEmpregaticio(string idFuncionario, string nomeEmpresa, string cnpj,
            string remuneracao, string comentario)
        {
            var queryInserirVinculo =
                "INSERT INTO vinculosempregaticiosfuncionarios (idfuncionario,nomeempresa,cnpj,remuneracao,comentario) VALUES(" +
                idFuncionario + ",'" + nomeEmpresa + "','" + cnpj + "','" + remuneracao + "','" + comentario + "')";

            _conexaoMysql.ExecutaComando(queryInserirVinculo);
        }

        public void AtualizaVinculoEmpregaticio(string idFuncionario, string nomeEmpresa, string cnpj,
            string remuneracao, string comentario)
        {
            var queryInserirVinculo =
                "UPDATE vinculosempregaticiosfuncionarios set nomeempresa='" + nomeEmpresa + "',cnpj='" + cnpj +
                "',remuneracao='" + remuneracao + "',comentario='" + comentario + "' WHERE idfuncionario=" +
                idFuncionario + "";

            _conexaoMysql.ExecutaComando(queryInserirVinculo);
        }

        public void AtualizadadosFuncionariodadosPessoais(string nome, string cpf, string rg, string pis,
            string dataNascimentoFuncionario, string sexo, string descricaoSexo, string etnia, string estadoCivil,
            string formacao, string formacaoAcademica, string usuarioSistema, string email, string pa, string rua,
            string numero, string bairro, string cidade, string confirmadados, string nacionalidade, string nomeMae,
            string nomePai, string localNascimento, string ufNascimento, string complemento, string cep, string pais,
            string residenciaPropria, string recursoFgts, string telefoneFixo, string telefoneCelular,
            string emailSecundario,
            string cnh, string orgaoEmissorCnh, DateTime dataExpedicaoDocumentoCnh, DateTime dataValidadeCnh, string oc,
            string orgaoEmissorOc,
            DateTime dataExpedicaoOc, DateTime dataValidadeOc, string deficienteMotor, string deficienteVisual,
            string deficienteAuditivo,
            string reabilitado, string observacaoDeficiente, string paisDivorciados, string orgaoEmissorRg,
            DateTime dataExpedicaoDocumentoRg, int idHorarioTrabalho)
        {
            var queryAtualizaFuncionario = "UPDATE funcionarios SET nome='" + nome + "', cpf='" + cpf + "',rg='" +
                                           rg + "', pis='" + pis + "',datanascimento='" +
                                           Convert.ToDateTime(dataNascimentoFuncionario).ToString("yyyy/MM/dd") +
                                           "', sexo=" + sexo + ",descricaosexo='" + descricaoSexo + "',etnia=" +
                                           etnia + ",idestadocivil=" + estadoCivil + ",  idescolaridade=" +
                                           formacao + ", formacaoacademica='" + formacaoAcademica + "', login='" +
                                           usuarioSistema + "', email='" + email + "', idpa=" + pa + ", rua='" +
                                           rua + "',numero=" + numero + ",bairro='" + bairro + "',cidade='" +
                                           cidade + "', confirmacaodado='" + confirmadados + "',nacionalidade='" +
                                           nacionalidade + "'," +
                                           "nomemae='" + nomeMae + "',nomepai='" + nomePai + "',localnascimento='" +
                                           localNascimento + "'" +
                                           ",ufnascimento='" + ufNascimento + "',complemento='" + complemento +
                                           "',cep='" + cep + "'" +
                                           ",pais='" + pais + "',residenciapropria='" + residenciaPropria +
                                           "',recursofgts='" + recursoFgts + "'" +
                                           ",telefonefixo='" + telefoneFixo + "'" + ",telefonecelular='" +
                                           telefoneCelular + "',emailsecundario='" + emailSecundario + "'" +
                                           ",cnh='" + cnh + "',orgaoemissorcnh='" + orgaoEmissorCnh + "'" +
                                           ",dataexpedicaocnh='" + dataExpedicaoDocumentoCnh.ToString("yyyy/MM/dd") +
                                           "',datavalidadecnh='" + dataValidadeCnh.ToString("yyyy/MM/dd") + "'," +
                                           "oc='" + oc + "',orgaoemissoroc='" + orgaoEmissorOc + "',dataexpedicaooc='" +
                                           dataExpedicaoOc.ToString("yyyy/MM/dd") + "'" +
                                           ",datavalidadeoc='" + dataValidadeOc.ToString("yyyy/MM/dd") +
                                           "',deficientemotor='" + deficienteMotor + "',deficientevisual='" +
                                           deficienteVisual + "'" +
                                           ",deficienteauditivo='" + deficienteAuditivo + "',reabilitado='" +
                                           reabilitado + "',observacaodeficiente='" + observacaoDeficiente + "'" +
                                           ",paisdivorciado='" + paisDivorciados + "', dataemissaorg='" +
                                           dataExpedicaoDocumentoRg.ToString("yyyy/MM/dd") + "',orgaoemissorrg='" +
                                           orgaoEmissorRg + "'" +
                                           ", idhorariotrabalho=" + idHorarioTrabalho + "  WHERE login='" +
                                           usuarioSistema + "'";
            _conexaoMysql.ExecutaComandoComRetorno(queryAtualizaFuncionario);
        }

        public void AtualizadadosFuncionarioProfissional(string setor, string funcao, string usuarioSistema,
            string numeroCtps,
            string serieCtps, string ufCtps, int idTipoConta, string codigoBanco,
            string agencia, string contaCorrente, string dependenteIrrf, string dependenteFamilia,
            string dadosDependentes, string tiposDependentes, string matricula, string anoPrimeiroEmprego,
            string emissaoCtps,
            string cpfIrrf, int idHorariOtrabalho)
        {
            var queryAtualizaFuncionario = "UPDATE funcionarios SET idsetor=" + setor + ", funcao='" + funcao +
                                           "',numeroctps='" + numeroCtps + "',seriectps='" + serieCtps + "',ufctps='" +
                                           ufCtps + "', idtipoconta=" + idTipoConta + "," +
                                           "codigobanco=" + codigoBanco + ",agencia='" + agencia + "',contacorrente='" +
                                           contaCorrente + "', informacaodependente='" + dadosDependentes +
                                           "',dependenteirrpf='" + dependenteIrrf + "'," +
                                           "dependentesalariofamilia='" + dependenteFamilia + "',tipodependente='" +
                                           tiposDependentes + "',matricula='" + matricula + "'" +
                                           ",anoprimeiroemprego='" + anoPrimeiroEmprego + "', dataemissaoctps='" +
                                           Convert.ToDateTime(emissaoCtps).Date.ToString("yyyy/MM/dd") +
                                           "', cpfirrf='" + cpfIrrf + "',idhorariotrabalho=" + idHorariOtrabalho +
                                           " WHERE login='" + usuarioSistema + "'";
            _conexaoMysql.ExecutaComandoComRetorno(queryAtualizaFuncionario);
        }

        public void AtualizaFoto(string foto, string usuarioSistema)
        {
            var queryAtualizaFuncionario =
                "UPDATE funcionarios SET foto='" + foto + "' WHERE login='" + usuarioSistema + "'";
            _conexaoMysql.ExecutaComandoComRetorno(queryAtualizaFuncionario);
        }

        public void AtualizadadosFuncionarioPerguntas(string usuarioSistema, string quatidadeFilhos,
            string dataNascimentoFilhos, string emergencia, string principaisHobbies, string comidaFavorita,
            string viagem, string sindicato, string email)
        {
            var queryAtualizaFuncionario = "UPDATE funcionarios SET quantidadefilho='" + quatidadeFilhos +
                                           "',datanascimentofilho='" +
                                           dataNascimentoFilhos + "', contatoemergencia='" + emergencia +
                                           "', principalhobbie='" + principaisHobbies + "', comidafavorita='" +
                                           comidaFavorita + "',viagem='" + viagem +
                                           "', perfilcompleto='S',contribuicaosindical = '" + sindicato +
                                           "',notificacaoemail = '" + email + "' WHERE login='" + usuarioSistema + "'";
            _conexaoMysql.ExecutaComandoComRetorno(queryAtualizaFuncionario);
        }

        public void AtualizarArquivoPessoal(string nomeArquivo, byte[] foto, string login)
        {
            var queryIdFuncionario = "SELECT id FROM funcionarios WHERE login='" + login + "'";
            var row = _conexaoMysql.ExecutaComandoComRetorno(queryIdFuncionario);

            var queryArquivousuario =
                "SELECT count(id) as count FROM documentospessoaisfuncionarios WHERE idfuncionario='" + row[0]["id"] +
                "' and nomearquivo='" + nomeArquivo + "'";
            var row2 = _conexaoMysql.ExecutaComandoComRetorno(queryArquivousuario);

            if (Convert.ToInt32(row2[0]["count"]) == 0)
            {
                var queryAtualizaFuncionario =
                    "INSERT INTO documentospessoaisfuncionarios (idfuncionario,nomearquivo,arquivo,dataupload) VALUES(" +
                    row[0]["id"] + ",'" + nomeArquivo + "',@image ,NOW()) ";
                _conexaoMysql.ExecutaComandoArquivo(queryAtualizaFuncionario, foto);
            }
            else
            {
                var queryAtualizaFuncionario =
                    "UPDATE documentospessoaisfuncionarios SET arquivo=@image and dataupload=NOW() WHERE idfuncionario=" +
                    row[0]["id"] + " AND nomearquivo='" + nomeArquivo + "'";
                _conexaoMysql.ExecutaComandoArquivo(queryAtualizaFuncionario, foto);
            }
        }

        public List<Dictionary<string, string>> RecuperaTodosArquivos(string login)
        {
            var query =
                "SELECT nomearquivo, dataupload FROM documentospessoaisfuncionarios WHERE login='" + login + "'";
            var row = _conexaoMysql.ExecutaComandoComRetorno(query);
            return row;
        }

        public DataTable RetornaDocumentosFuncionario(string login)
        {
            var dados = _conexaoMysql.ComandoArquivo(login);
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

        public List<Dictionary<string, string>> RetornaHoras(string cracha)
        {
            var query = "select hora,datareferencia from horasextrasfuncionarios WHERE crachafirebird=" + cracha + ";";
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

        public List<Dictionary<string, string>> RetornaFuncionariosSetor(string idSetor)
        {
            var query =
                "SELECT nome,idnotificacao,notificacaoemail,id,email FROM funcionarios WHERE idsetor='" + idSetor + "'";
            var dados = _conexaoMysql.ExecutaComandoComRetorno(query);


            return dados;
        }

        public void AtualizaEmailSenha(string idusuario)
        {
            var query = "UPDATE funcionarios SET emailtrocasenha='S' WHERE id='" + idusuario + "'";
            _conexaoMysql.ExecutaComandoComRetorno(query);
        }

        public void AtualizaSenha(string usuario, string senha)
        {
            var query = "UPDATE funcionarios SET senha='" + senha + "', emailtrocasenha='N' WHERE login='" + usuario +
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

        public List<Dictionary<string, string>> RetornaExtrato(string cpf)
        {
            var query = "SELECT* FROM showdepremioscupons WHERE  numdoccliente = " + cpf + "";
            var dados = _conexaoMysql.ExecutaComandoComRetorno(query);
            return dados;
        }

        public void CadastraIdNotificacao(string idNotificacao, string login)
        {
            var query = "UPDATE funcionarios set idnotificacao='" + idNotificacao + "' WHERE login='" + login + "'";
            _conexaoMysql.ExecutaComando(query);
        }

        public List<Dictionary<string, string>> RetornaInformacoesNotificacao(string idFuncionario)
        {
            var query = "select id,email,idnotificacao,notificacaoemail,idsetor,nome from funcionarios where id=" +
                        idFuncionario + " ";
            var chamados = _conexaoMysql.ExecutaComandoComRetorno(query);
            return chamados;
        }

        public List<Dictionary<string, string>> RetornaInformacoesGestor(string idSetor)
        {
            var query = "select id,email,idnotificacao,notificacaoemail from funcionarios where idsetor=" +
                        idSetor + " and gestor='S' ";
            var chamados = _conexaoMysql.ExecutaComandoComRetorno(query);
            return chamados;
        }

        public List<Dictionary<string, string>> RetornaInformacoesNumerario(string idAgencia)
        {
            var query = "select * from numerariosagencias WHERE idagencia=" + idAgencia + " ";
            var chamados = _conexaoMysql.ExecutaComandoComRetorno(query);
            return chamados;
        }

        public string RetornaNomeFuncionario(string idFuncionario)
        {
            var query = "select nome from funcionarios WHERE id=" + idFuncionario + " ";
            var chamados = _conexaoMysql.ExecutaComandoComRetorno(query);
            return chamados[0]["nome"];
        }

        public void AtualizaNumerario(string valor, string observacao, string agencia, string usuarioAlteracao)
        {
            var query = "UPDATE numerariosagencias SET valor='" + valor + "',idfuncionarioalteracao=" +
                        usuarioAlteracao + ",observacao='" + observacao + "',dataalteracao=NOW() WHERE idagencia=" +
                        agencia + "";
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