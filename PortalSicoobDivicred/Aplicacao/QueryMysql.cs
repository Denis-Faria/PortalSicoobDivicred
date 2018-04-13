using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
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
                else
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

        public List<Dictionary<string, string>> RecuperaDocumentosFuncionario(string Login)
        {
            var Query =
                "SELECT * FROM documentospessoaisfuncionarios WHERE idfuncionario=(SELECT id FROM funcionarios where login='" +
                Login + "')";
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
                "SELECT * FROM funcionarios where (soundex(nome)like concat('%',soundex('" + NomeFuncionario + "'),'%')) ";
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



        public List<SelectListItem> RetornaProdutos()
        {
            var Produtos = new List<SelectListItem>();

            const string QueryRetornaProdutos = "SELECT id,descricao FROM pgdprodutos";

            var Dados = ConexaoMysql.ExecutaComandoComRetorno(QueryRetornaProdutos);
            foreach (var row in Dados)
                Produtos.Add(new SelectListItem
                {
                    Value = row["id"],
                    Text = row["descricao"]
                });

            return Produtos;
        }

        public List<Dictionary<string,string>>BuscaDadosProducao(int idProducao)
        {
            string QueryConsultaValorAtual =
                "select Login,valorponto from cimproducao where id=" + idProducao;

            var idDados = ConexaoMysql.ExecutaComandoComRetorno(QueryConsultaValorAtual);

            return idDados;
        }

        public List<Dictionary<string, string>> RecuperaDadosProducao(string UsuarioSistema)
        {
            var Query =
                "SELECT * FROM cimproducao WHERE Login='" + UsuarioSistema + "' and excluido='N'" ;
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);


            return Dados;
        }

        public List<Dictionary<string, string>> RecuperaPontuacaoFuncionariosSetor(string UsuarioSistema)
        {
            var Query1 =
                "SELECT idsetor from funcionarios WHERE usuario='" + UsuarioSistema  ;
            var idSetor = ConexaoMysql.ExecutaComandoComRetorno(Query1);

            var Query =
                "SELECT * from funcionarios where idsetor='"+idSetor.ToString()+"' and login !='"+UsuarioSistema+"'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);

            return Dados;
        }

        public string ExisteRegistro(string Login)
        {
            string QueryExiste = "SELECT count(*) from cimpontuacao where Login='" + Login+"'";

            var id = ConexaoMysql.ExecutaComandoComRetorno(QueryExiste);

            return id[0]["count(*)"];
        }

        public string BuscaSaldoAtual (string Login)
        {
            string QueryConsultaValorAtual =
                "select pontuacaoatual from cimpontuacao where Login='" + Login + "'";

            var pontuacaoAtual = ConexaoMysql.ExecutaComandoComRetorno(QueryConsultaValorAtual);
            if (pontuacaoAtual.Count == 0)
            {
                //string login = RecuperaUsuarioLogin(usuario.ToString());

                var QueryIncluiPontuacao = "INSERT INTO cimpontuacao (pontuacaoatual,login) values ('0','" + Login + "')";
                ConexaoMysql.ExecutaComando(QueryIncluiPontuacao);
                //var QueryIncluiPontuacao = "INSERT INTO cimpontuacao (usuario,pontuacaoatual,) values ('" + usuario + "','0')";
                //ConexaoMysql.ExecutaComando(QueryIncluiPontuacao);

                string QueryConsultaValorPrimeiraVez =
                    "select pontuacaoatual from cimpontuacao where Login='" + Login + "'";

                 pontuacaoAtual = ConexaoMysql.ExecutaComandoComRetorno(QueryConsultaValorPrimeiraVez);

            }

            return pontuacaoAtual[0]["pontuacaoatual"];
        }

        

        public void ExcluirRegistro(int id)
        {
            var QueryExcluiProducao = "UPDATE cimproducao SET excluido='S' where id='"+id+"'";
            ConexaoMysql.ExecutaComandoComRetorno(QueryExcluiProducao);
        }

        public void AtualizarRegistroExclusao(string Login, double valor)
        {

            string auxSaldoAtual = BuscaSaldoAtual(Login);
            double saldoatual = Convert.ToDouble(auxSaldoAtual);

            double saldo = saldoatual - valor;

            var QueryExcluiProducao = "UPDATE cimpontuacao set pontuacaoatual =" + saldo.ToString().Replace(",", ".") +
                                      " where Login='" + Login + "'";
            ConexaoMysql.ExecutaComandoComRetorno(QueryExcluiProducao);
        }




        public string RecuperaProduto(int idProduto)
        {
            var Query =
                "select descricao from pgdprodutos where id='" + idProduto + "'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);


            return Dados[0]["descricao"];
        }

        public string RecuperaDadosPontos(int idProduto)
        {
            var Query =
                "select valorminimo,peso from pgdprodutos where id='" + idProduto + "'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);


            return Dados[0]["descricao"];
        }


        public List<Dictionary<string, string>> retornaDadosProdutos(int id)
        {
            
            string QueryRetornaDadosProdutos = "SELECT peso,valorminimo from pgdprodutos where id='" + id + "'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(QueryRetornaDadosProdutos);
            return Dados;
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

        public void InsereProducao(string cpf, int produtos, string observacao, DateTime data,string Login,string valor,string valorponto)
        {


            string QueryInsereProducao = "INSERT INTO cimproducao (cpf,produto,observacao,datacontratacao,excluido,Login,valor,valorponto) values ('"+cpf+"','"+produtos+ "','"+observacao+ "','"+Convert.ToDateTime(data).ToString("yyyy-MM-dd")+ "','N','"+Login+"','"+valor.ToString().Replace(",", ".") + "','"+valorponto.ToString().Replace(".", "").Replace(",", ".") + "') ";
            ConexaoMysql.ExecutaComando(QueryInsereProducao);

        }

       

        public void IncluirPontucao(string Login,double ponto)
        {
            string existe=ExisteRegistro(Login);
        /*    if (existe == "0")
            {
                string aux1=ponto.ToString("N2");
                double aux2 = Convert.ToDouble(aux1);

                string login = RecuperaUsuarioLogin(usuario.ToString());

                var QueryIncluiPontuacao = "INSERT INTO cimpontuacao (usuario,pontuacaoatual,login) values ('"+usuario+"','" + ponto.ToString("N2").Replace(".","").Replace(",",".") + "','"+login+")";
                ConexaoMysql.ExecutaComando(QueryIncluiPontuacao);
            }
            else
            {*/
                string auxSaldoAtual = BuscaSaldoAtual(Login);
                double saldoatual = Convert.ToDouble(auxSaldoAtual);

                
                double valorAtual = saldoatual + ponto;
                var QueryAtualizaPontuacao = "update cimpontuacao set pontuacaoatual =" + valorAtual.ToString("N2").Replace(".", "").Replace(",", ".") + " where Login='"+Login+"'";
                ConexaoMysql.ExecutaComando(QueryAtualizaPontuacao);
           // }
         }

        public string RecuperaUsuario(string login)
        {
            string QueryRecuperaUsuario = "SELECT id from usuarios where login='"+login+"'";

            var id =  ConexaoMysql.ExecutaComandoComRetorno(QueryRecuperaUsuario);

            return id[0]["id"];
        }

        public string Gestor(string login)
        {
            string QueryGestor = "SELECT gestor from funcionarios where login='" + login + "'";

            var gestor = ConexaoMysql.ExecutaComandoComRetorno(QueryGestor);

            return gestor[0]["gestor"];
        }

        public List<Dictionary<string, string>> RecuperaSubordinadosGestor(string login)
        {

            string QueryRecuperaSetorUsuario = "SELECT idsetor from funcionarios where login='" + login + "'";
            var idsetor = ConexaoMysql.ExecutaComandoComRetorno(QueryRecuperaSetorUsuario);

            var Query ="select a.nome,b.pontuacaoatual,a.funcao from funcionarios as a inner join cimpontuacao as b on a.login=b.login " +
                       "where a.idsetor='"+idsetor[0]["idsetor"]+"'" ;
            var row = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return row;
        }

        public string RecuperaMetaCim(string funcao)
        {
            string QueryRecuperaMetaUsuario = "SELECT cimmeta from funcoes where id='" + funcao + "'";
            var meta = ConexaoMysql.ExecutaComandoComRetorno(QueryRecuperaMetaUsuario);
            return meta[0]["cimmeta"];
        }

        public string RecuperaFuncao(string Login)
        {
            string QueryRecuperaFuncaoUsuario = "SELECT funcao from funcionarios where Login='" + Login + "'";
            var funcao = ConexaoMysql.ExecutaComandoComRetorno(QueryRecuperaFuncaoUsuario);
            return funcao[0]["funcao"];
        }


        public string RecuperaUsuarioLogin(string usuario)
        {
            string QueryRecuperaUsuarioLogin = "SELECT login from usuarios where id='" + usuario + "'";

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
            string Viagem, string ConfirmacaoCertificacao, string ConfirmaDados)
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
                                            ConfirmacaoCertificacao + "' WHERE nome='" + Nome + "'";
            ConexaoMysql.ExecutaComandoComRetorno(QueryAtualizaFuncionario);
        }

        public void AtualizaDadosFuncionarioDadosPessoais(string Nome, string Cpf, string Rg, string Pis,
            string DataNascimentoFuncionario, string Sexo, string DescricaoSexo, string Etnia, string EstadoCivil,
            string Formacao, string FormacaoAcademica, string UsuarioSistema, string Email, string PA, string Rua,
            string Numero, string Bairro, string Cidade, string ConfirmaDados)
        {
            var QueryAtualizaFuncionario = "UPDATE funcionarios SET nome='" + Nome + "', cpf='" + Cpf + "',rg='" +
                                            Rg + "', pis='" + Pis + "',datanascimento='" +
                                            Convert.ToDateTime(DataNascimentoFuncionario).ToString("yyyy/MM/dd") +
                                            "', sexo=" + Sexo + ",descricaosexo='" + DescricaoSexo + "',etnia=" +
                                            Etnia + ",idestadocivil=" + EstadoCivil + ",  idescolaridade=" +
                                            Formacao + ", formacaoacademica='" + FormacaoAcademica + "', login='" +
                                            UsuarioSistema + "', email='" + Email + "', idpa=" + PA + ", rua='" +
                                            Rua + "',numero=" + Numero + ",bairro='" + Bairro + "',cidade='" +
                                            Cidade + "', confirmacaodado='" + ConfirmaDados + "' WHERE login='" +
                                            UsuarioSistema + "'";
            ConexaoMysql.ExecutaComandoComRetorno(QueryAtualizaFuncionario);
        }

        public void AtualizaDadosFuncionarioProfissional(string Setor, string Funcao, string UsuarioSistema)
        {
            var QueryAtualizaFuncionario = "UPDATE funcionarios SET idsetor=" + Setor + ", funcao='" + Funcao +
                                            "' WHERE login='" + UsuarioSistema + "'";
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
            string Viagem)
        {
            var QueryAtualizaFuncionario = "UPDATE funcionarios SET quantidadefilho='" + QuatidadeFilhos +
                                            "',datanascimentofilho='" +
                                            DataNascimentoFilhos + "', contatoemergencia='" + Emergencia +
                                            "', principalhobbie='" + PrincipaisHobbies + "', comidafavorita='" +
                                            ComidaFavorita + "',viagem='" + Viagem +
                                            "', perfilcompleto='S' WHERE login='" + UsuarioSistema + "'";
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

        public void cadastrarAlert(string IdUsuario,string IdAplicativo, string Descricao)
        {
            var Query = "INSERT INTO alertas (idusuario,idaplicativo,descricao,data) VALUES("+IdUsuario+","+IdAplicativo+",'"+Descricao+"',NOW())";
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
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }
        public List<Dictionary<string,string>> RetornaFuncionariosSetor(string IdSetor)
        {
            var Query =
                "SELECT nome FROM funcionarios WHERE idsetor='" + IdSetor + "'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);


            return Dados;
        }
        public void  AtualizaEmailSenha(string IdUsuario)
        {
            var Query ="UPDATE funcionarios SET emailtrocasenha='S' WHERE id='" +IdUsuario + "'";
            ConexaoMysql.ExecutaComandoComRetorno(Query);

        }
        public void AtualizaSenha(string Usuario,string Senha)
        {
            var Query = "UPDATE funcionarios SET senha='"+Senha+"', emailtrocasenha='N' WHERE login='" + Usuario + "'";
            ConexaoMysql.ExecutaComandoComRetorno(Query);

        }
        public List<Dictionary<string, string>> RetornaEmailSenha(string Usuario)
        {
            var Query =
                "SELECT emailtrocasenha FROM funcionarios WHERE login='" + Usuario + "'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);


            return Dados;
        }
    }
}