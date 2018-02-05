using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
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
            var QueryConfirmaLogin = "SELECT login FROM usuarios WHERE login='" + Usuario + "' AND senha=MD5('" +
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
    }
}