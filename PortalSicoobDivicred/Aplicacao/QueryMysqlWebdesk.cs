using System.Collections.Generic;
using System.Data;
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

        public List<Dictionary<string, string>> BuscaChamadosMeuSetor(string Pesquisa, string IdUsuario,string IdSetor)
        {
            var Query =
                "SELECT distinct a.id,a.titulochamado,u1.nome CADASTRO,u2.nome AS OPERADOR FROM webdeskchamados a LEFT JOIN usuarios u1 on a.idusuariocadastro=u1.id LEFT JOIN usuarios u2 on a.idoperador=u2.id, webdeskinteracoes b where  b.idchamado=a.id and(MATCH (b.textointeracao) AGAINST ('" +
                Pesquisa + "')  or a.id='" + Pesquisa + "') and b.idusuariointeracao=" + IdUsuario + " and a.idsetor='"+IdSetor+"'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }


        public List<Dictionary<string, string>> BuscaChamadosMeuSetorNovo(string Pesquisa, string IdUsuario,string IdSetor)
        {
            var Query =
                "SELECT distinct a.id,c.descricao as titulo,u1.nome CADASTRO,u2.nome AS OPERADOR FROM webdesksolicitacoes a LEFT JOIN funcionarios u1 on a.idfuncionariocadastro=u1.id LEFT JOIN funcionarios u2 on a.idfuncionarioresponsavel=u2.id, webdeskinteracoessolicitacoes b,webdeskcategorias c where  b.idsolicitacao=a.id and(MATCH (b.textointeracao) AGAINST ('" +
                Pesquisa + "')) and b.idfuncionariointeracao=" + IdUsuario + " AND a.idcategoria=c.id AND  a.id='" +
                Pesquisa + "' and a.idsetor='"+IdSetor+"'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }


        public List<Dictionary<string, string>> BuscaInteracaoChamados(string IdChamado)
        {
            var Query =
                "SELECT b.nome,a.textointeracao, a.datahorainteracao as data FROM webdeskinteracoes a, usuarios b WHERE a.idusuariointeracao=b.id AND idchamado=" +
                IdChamado + " ";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }


        public List<Dictionary<string, string>> BuscaInteracaoChamadosNovo(string IdChamado)
        {
            var Query =
                "SELECT b.nome,a.textointeracao, a.datahorainteracao as data FROM webdeskinteracoessolicitacoes a, funcionarios b WHERE a.idfuncionariointeracao=b.id AND idsolicitacao=" +
                IdChamado + " ";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }


        public List<SelectListItem> RetornaSetor()
        {
            var EstadoCivil = new List<SelectListItem>();

            const string QueryRetornaEstadoCivil =
                "SELECT id,descricao FROM setores WHERE excluido='N' ORDER BY descricao  ASC";

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

            var QueryRetornaEstadoCivil = "SELECT id,descricao FROM webdeskcategorias WHERE idsetor=" + IdSetor +
                                          " AND excluido='N'";

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

            var QueryRetornaFuncionario =
                "SELECT id,nome FROM funcionarios WHERE idsetor=" + IdSetor + " AND ativo='S'";

            var Dados = ConexaoMysql.ExecutaComandoComRetorno(QueryRetornaFuncionario);
            foreach (var row in Dados)
                EstadoCivil.Add(new SelectListItem
                {
                    Value = row["id"],
                    Text = row["nome"]
                });

            return EstadoCivil;
        }

        public string CadastraSolicitacao(string IdSetor, string IdCategoria, string IdOperador, string Descricao,
            string IdUsuario, string cpf)
        {
            var Query =
                "INSERT INTO webdesksolicitacoes (idfuncionariocadastro,idfuncionarioresponsavel,idcategoria,idsetor,idsituacao,datahoracadastro,cpf) VALUES(" +
                IdUsuario + "," + IdOperador + "," + IdCategoria + "," + IdSetor + ",1,NOW(),'" + cpf + "')";
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

        public void InserirAnexo(string IdInteracao, byte[] Foto, string TipoArquivo, string NomeArquivo)
        {
            var QueryAtualizaFuncionario =
                "INSERT INTO webdeskanexos (idinteracao,arquivo,tipoarquivo,nomearquivo) VALUES(" + IdInteracao +
                ",@image,'" + TipoArquivo + "','" + NomeArquivo + "') ";
            ConexaoMysql.ExecutaComandoArquivo(QueryAtualizaFuncionario, Foto);
        }

        public List<Dictionary<string, string>> RetornaChamadosAbertos(string IdUsuario)
        {
            var Query =
                "SELECT a.id,b.descricao as titulo,c.nome as operador,d.descricao as situacao,b.tempo,a.datahoracadastro,TIMEDIFF(Now() ,a.datahoracadastro) as sla,a.fimatendimento,a.cpf from webdesksolicitacoes a, webdeskcategorias b,funcionarios c,webdesksituacoes d WHERE a.idfuncionarioresponsavel = c.id AND a.idcategoria=b.id AND a.idsituacao=d.id and a.idfuncionariocadastro=" +
                IdUsuario + " ";
            var Chamados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Chamados;
        }

        public List<Dictionary<string, string>> RetornaChamadosResponsavel(string IdUsuario)
        {
            var Query =
                "SELECT a.id,b.descricao as titulo,u1.nome cadastro,u2.nome AS operador,d.descricao as situacao,b.tempo,a.datahoracadastro ,TIMEDIFF(Now() ,a.datahoracadastro) as sla,a.cpf FROM webdesksolicitacoes a LEFT JOIN funcionarios u1 on a.idfuncionariocadastro=u1.id INNER JOIN funcionarios u2 on (a.idfuncionarioresponsavel=u2.id AND u2.id=" +
                IdUsuario +
                ") LEFT JOIN  webdeskcategorias b on a.idcategoria=b.id INNER JOIN webdesksituacoes d on a.idsituacao =d.id AND (a.idsituacao=2 or a.idsituacao=1)  ORDER BY a.id";
            var Chamados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Chamados;
        }

        public List<Dictionary<string, string>> RetornaChamadosSetor(string IdSetor)
        {
            var Query =
                "SELECT a.id,b.descricao as titulo,u1.nome cadastro,u2.nome AS operador,d.descricao as situacao,b.tempo,a.datahoracadastro,TIMEDIFF(Now() ,a.datahoracadastro) as sla,a.cpf FROM webdesksolicitacoes a LEFT JOIN funcionarios u1 on a.idfuncionariocadastro=u1.id INNER JOIN funcionarios u2 on (a.idfuncionarioresponsavel=u2.id) LEFT JOIN  webdeskcategorias b on a.idcategoria=b.id INNER JOIN webdesksituacoes d on a.idsituacao =d.id AND (a.idsituacao=2 or a.idsituacao=1) and a.idsetor=" +
                IdSetor + "  ORDER BY a.id";
            var Chamados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Chamados;
        }

        public List<Dictionary<string, string>> RetornaDadosChamado(string IdSolicitacao)
        {
            var Query =
                "SELECT a.id,b.descricao as titulo,u1.id as idcadastro,u1.nome cadastro,u2.nome AS operador,d.descricao as situacao,b.tempo,a.datahoracadastro,e.descricao as setor,a.inicioatendimento,a.fimatendimento FROM webdesksolicitacoes a LEFT JOIN funcionarios u1 on a.idfuncionariocadastro=u1.id INNER JOIN funcionarios u2 on (a.idfuncionarioresponsavel=u2.id) LEFT JOIN  webdeskcategorias b on a.idcategoria=b.id INNER JOIN webdesksituacoes d on a.idsituacao =d.id INNER JOIN setores e on a.idsetor=e.id and a.id=" +
                IdSolicitacao + " ";
            var Chamados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Chamados;
        }

        public List<Dictionary<string, string>> RetornaInteracoesChamado(string IdSolicitacao)
        {
            var Query =
                "select b.id,a.nome as nome, b.textointeracao,b.datahorainteracao,b.acao from funcionarios a, webdeskinteracoessolicitacoes b where a.id=b.idfuncionariointeracao and b.idsolicitacao=" +
                IdSolicitacao + " ";
            var Chamados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Chamados;
        }

        public DataTable RetornaAnexoInteracao(string IdInteracao)
        {
            var Dados = ConexaoMysql.ComandoArquivoWebDesk(IdInteracao);
            return Dados;
        }

        public string CadastrarInteracao(string IdChamado, string Descricao, string IdUsuario, string Acao)
        {
            var QueryInteracao =
                "INSERT INTO webdeskinteracoessolicitacoes(idsolicitacao,textointeracao,idfuncionariointeracao,datahorainteracao,acao) VALUES(" +
                IdChamado + ",'" + Descricao + "'," + IdUsuario + ",NOW(),'" + Acao + "')";
            var IdInteracao = ConexaoMysql.ExecutaComandoComRetornoId(QueryInteracao);
            return IdInteracao;
        }

        public void EncerrarSolicitacao(string IdChamado)
        {
            var QueryInteracao =
                "UPDATE webdesksolicitacoes SET idsituacao=3, fimatendimento=NOW() WHERE  id=" + IdChamado + ";";
            ConexaoMysql.ExecutaComando(QueryInteracao);
        }

        public void AlterarCategoriaSolicitacao(string IdChamado, string IdCategoria)
        {
            var QueryInteracao =
                "UPDATE webdesksolicitacoes SET idcategoria=" + IdCategoria + " WHERE id=" + IdChamado + ";";
            ConexaoMysql.ExecutaComando(QueryInteracao);
        }

        public void AlterarResponsavelSolicitacao(string IdChamado, string IdSetor, string IdFuncionario)
        {
            var QueryInteracao =
                "UPDATE webdesksolicitacoes SET idsetor=" + IdSetor + ", idfuncionarioresponsavel=" + IdFuncionario +
                " WHERE id=" + IdChamado + ";";
            ConexaoMysql.ExecutaComando(QueryInteracao);
        }

        public void ReabrirSolicitacao(string IdChamado)
        {
            var QueryInteracao =
                "UPDATE webdesksolicitacoes SET idsituacao=2 WHERE id=" + IdChamado + ";";
            ConexaoMysql.ExecutaComando(QueryInteracao);
        }

        public List<Dictionary<string, string>> RetornaRepasseFuncionarioChamado(string IdFuncionario)
        {
            var Query = "select nome from funcionarios where id=" + IdFuncionario + " ";
            var Chamados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Chamados;
        }

        public List<Dictionary<string, string>> RetornaRepasseCategoriaChamado(string IdFuncionario)
        {
            var Query = "select descricao from webdeskcategorias where id=" + IdFuncionario + " ";
            var Chamados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Chamados;
        }

        public void IniciarAtendimentoSolicitacao(string IdChamado)
        {
            var QueryInteracao =
                "UPDATE webdesksolicitacoes SET inicioatendimento=NOW(), idsituacao=2 WHERE id=" + IdChamado + ";";
            ConexaoMysql.ExecutaComando(QueryInteracao);
        }

        public List<Dictionary<string, string>> RetornaDadosChamadoAntigo(string IdSolicitacao)
        {
            var Query =
                "SELECT a.id,b.descricao as titulo,u1.id as idcadastro,u1.nome cadastro,u2.nome AS operador,d.descricao as situacao,b.tempo,a.datahoracadastro,e.descricao as setor,a.confirmacaoleitura,a.dataconclusaochamado FROM webdeskchamados a LEFT JOIN usuarios u1 on a.idusuariocadastro=u1.id INNER JOIN usuarios u2 on (a.idoperador=u2.id) LEFT JOIN  webdeskcategorias b on a.idcategoria=b.id INNER JOIN webdesksituacoes d on a.idsituacao =d.id INNER JOIN setores e on a.idsetor=e.id and a.id=" +
                IdSolicitacao + " ";
            var Chamados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Chamados;
        }

        public List<Dictionary<string, string>> RetornaInteracoesChamadoAntigo(string IdSolicitacao)
        {
            var Query =
                "select b.id,a.nome as nome, b.textointeracao,b.datahorainteracao,b.idarquivogoogle,b.idpastaarquivogoogle from usuarios a, webdeskinteracoes b where a.id=b.idusuariointeracao and b.idchamado=" +
                IdSolicitacao + " ";
            var Chamados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Chamados;
        }
        public List<Dictionary<string, string>> RetornaInformacoesNotificacao(string IdFuncionario)
        {
            var Query = "select email,idnotificacao,notificacaoemail from funcionarios where id=" + IdFuncionario + " ";
            var Chamados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Chamados;
        }
        public List<Dictionary<string, string>> RetornaIdSolicitantes(string IdChamado)
        {
            var Query = "select idfuncionariocadastro from webdesksolicitacoes where id=" + IdChamado + " ";
            var Chamados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Chamados;
        }

    }
}