using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Port.Repositorios;

namespace PortalSicoobDivicred.Aplicacao
{
    public class QueryMysqlWebdesk
    {
        private readonly Conexao ConexaoMysql;

        public QueryMysqlWebdesk()
        {
            ConexaoMysql = new Conexao();
        }

        public bool UsuarioLogado()
        {
            var Usuario = HttpContext.Current.Request.Cookies["CookieFarm"];
            if (Usuario == null)
                return false;
            return true;
        }

        public List<Dictionary<string,string>> BuscaChamadosMeuSetor(string Pesquisa,string IdUsuario)
        {
            var Query =
                "SELECT distinct a.id,a.titulochamado,u1.nome CADASTRO,u2.nome AS OPERADOR FROM webdeskchamados a LEFT JOIN usuarios u1 on a.idusuariocadastro=u1.id LEFT JOIN usuarios u2 on a.idoperador=u2.id, webdeskinteracoes b where  b.idchamado=a.id and(MATCH (b.textointeracao) AGAINST ('" +
                Pesquisa + "')) and b.idusuariointeracao=" + IdUsuario + "";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }
        public List<Dictionary<string, string>> BuscaInteracaoChamados(string IdChamado)
        {
            var Query =
                "SELECT b.nome,a.textointeracao, a.datahorainteracao as data FROM webdeskinteracoessolicitacoes a, usuarios b WHERE a.idfuncionariointeracao=b.id AND idchamado="+IdChamado+" ";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }
        public List<SelectListItem> RetornaSetor()
        {
            var EstadoCivil = new List<SelectListItem>();

            const string QueryRetornaEstadoCivil = "SELECT id,descricao FROM setores WHERE excluido='N'";

            var Dados = ConexaoMysql.ExecutaComandoComRetorno(QueryRetornaEstadoCivil);
            foreach (var row in Dados)
                EstadoCivil.Add(new SelectListItem
                {
                    Value = row["id"],
                    Text = row["descricao"]
                });

            return EstadoCivil;
        }
        public List<SelectListItem> RetornaCategoria(string IdSetor)
        {
            var EstadoCivil = new List<SelectListItem>();

            string QueryRetornaEstadoCivil = "SELECT id,descricao FROM webdeskcategorias WHERE idsetor="+IdSetor+" AND excluido='N'";

            var Dados = ConexaoMysql.ExecutaComandoComRetorno(QueryRetornaEstadoCivil);
            foreach (var row in Dados)
                EstadoCivil.Add(new SelectListItem
                {
                    Value = row["id"],
                    Text = row["descricao"]
                });

            return EstadoCivil;
        }

        public List<SelectListItem> RetornaFuncionario(string IdSetor)
        {
            var EstadoCivil = new List<SelectListItem>();

            string QueryRetornaFuncionario = "SELECT id,nome FROM funcionarios WHERE idsetor="+IdSetor+" AND ativo='S'";

            var Dados = ConexaoMysql.ExecutaComandoComRetorno(QueryRetornaFuncionario);
            foreach (var row in Dados)
                EstadoCivil.Add(new SelectListItem
                {
                    Value = row["id"],
                    Text = row["nome"]
                });

            return EstadoCivil;
        }

        public string CadastraSolicitacao(string IdSetor,string IdCategoria,string IdOperador,string Descricao,string IdUsuario)
        {
            var Query = "INSERT INTO webdesksolicitacoes (idfuncionariocadastro,idfuncionarioresponsavel,idcategoria,idsetor,idsituacao,datahoracadastro) VALUES("+IdUsuario+","+IdOperador+","+IdCategoria+","+IdSetor+",1,NOW())";
            var IdChamado = ConexaoMysql.ExecutaComandoComRetornoId(Query);
            var QueryInteracao =
                "INSERT INTO webdeskinteracoessolicitacoes(idsolicitacao,textointeracao,idfuncionariointeracao,datahorainteracao) VALUES(" +
                IdChamado + ",'" + Descricao + "'," + IdUsuario + ",NOW())";
            var IdInteracao = ConexaoMysql.ExecutaComandoComRetornoId(QueryInteracao);
            return IdInteracao;

        }

        public List<Dictionary<string, string>> RecuperaDadosUsuarios(string Login)
        {
            var QueryRecuperaUsuario =
                "SELECT * FROM funcionarios  WHERE login='" + Login + "'";


            var Dados = ConexaoMysql.ExecutaComandoComRetorno(QueryRecuperaUsuario);


            return Dados;
        }

        public void InserirAnexo(string IdInteracao, byte[] Foto)
        {

            
                var QueryAtualizaFuncionario =
                    "INSERT INTO webdeskanexos (idinteracao,arquivo) VALUES(" +IdInteracao + ",@image) ";
                ConexaoMysql.ExecutaComandoArquivo(QueryAtualizaFuncionario, Foto);
           
        }

        public List<Dictionary<string, string>> RetornaChamadosAbertos(string IdUsuario)
        {
            var Query =
                "SELECT a.id,b.descricao as titulo,c.nome as operador,d.descricao as situacao,b.tempo,a.datahoracadastro from webdesksolicitacoes a, webdeskcategorias b,funcionarios c,webdesksituacoes d WHERE a.idfuncionarioresponsavel = c.id AND a.idcategoria=b.id AND a.idsituacao=d.id and a.idfuncionariocadastro=" + IdUsuario+" ";
            var Chamados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Chamados;
        }

        public List<Dictionary<string, string>> RetornaChamadosResponsavel(string IdUsuario)
        {
            var Query =
                "SELECT a.id,b.descricao as titulo,u1.nome cadastro,u2.nome AS operador,d.descricao as situacao,b.tempo,a.datahoracadastro FROM webdesksolicitacoes a LEFT JOIN funcionarios u1 on a.idfuncionariocadastro=u1.id INNER JOIN funcionarios u2 on (a.idfuncionarioresponsavel=u2.id AND u2.id="+IdUsuario+") LEFT JOIN  webdeskcategorias b on a.idcategoria=b.id INNER JOIN webdesksituacoes d on a.idsituacao =d.id AND (a.idsituacao=2 or a.idsituacao=1)  ORDER BY a.id";
            var Chamados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Chamados;
        }
        public List<Dictionary<string, string>> RetornaChamadosSetor(string IdSetor)
        {
            var Query =
                "SELECT a.id,b.descricao as titulo,u1.nome cadastro,u2.nome AS operador,d.descricao as situacao,b.tempo,a.datahoracadastro FROM webdesksolicitacoes a LEFT JOIN funcionarios u1 on a.idfuncionariocadastro=u1.id INNER JOIN funcionarios u2 on (a.idfuncionarioresponsavel=u2.id) LEFT JOIN  webdeskcategorias b on a.idcategoria=b.id INNER JOIN webdesksituacoes d on a.idsituacao =d.id AND (a.idsituacao=2 or a.idsituacao=1) and a.idsetor="+IdSetor+"  ORDER BY a.id";
            var Chamados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Chamados;
        }
        public List<Dictionary<string, string>> RetornaDadosChamado(string IdSolicitacao)
        {

            var Query = "SELECT a.id,b.descricao as titulo,u1.id as idcadastro,u1.nome cadastro,u2.nome AS operador,d.descricao as situacao,b.tempo,a.datahoracadastro,e.descricao as setor FROM webdesksolicitacoes a LEFT JOIN funcionarios u1 on a.idfuncionariocadastro=u1.id INNER JOIN funcionarios u2 on (a.idfuncionarioresponsavel=u2.id) LEFT JOIN  webdeskcategorias b on a.idcategoria=b.id INNER JOIN webdesksituacoes d on a.idsituacao =d.id INNER JOIN setores e on a.idsetor=e.id and a.id=" + IdSolicitacao + " ";
            var Chamados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Chamados;
        }

        public List<Dictionary<string, string>> RetornaInteracoesChamado(string IdSolicitacao)
        {
            var Query = "select b.id,a.nome as nome, b.textointeracao,b.datahorainteracao from funcionarios a, webdeskinteracoessolicitacoes b where a.id=b.idfuncionariointeracao and b.idsolicitacao=" + IdSolicitacao + " ";
            var Chamados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Chamados;
        }
        public DataTable RetornaAnexoInteracao(string IdInteracao)
        {
            var Dados = ConexaoMysql.ComandoArquivoWebDesk(IdInteracao);
            return Dados;

        }
    }
}