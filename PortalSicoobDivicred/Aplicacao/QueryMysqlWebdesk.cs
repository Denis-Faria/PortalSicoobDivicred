using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Mvc;
using PortalSicoobDivicred.Repositorios;

namespace PortalSicoobDivicred.Aplicacao
{
    public class QueryMysqlWebdesk
    {
        private readonly Conexao _conexaoMysql;

        public QueryMysqlWebdesk()
        {
            _conexaoMysql = new Conexao();
        }

        public bool UsuarioLogado()
        {
            var usuario = HttpContext.Current.Request.Cookies["CookieFarm"];
            if (usuario == null)
                return false;
            return true;
        }

        public List<Dictionary<string, string>> BuscaChamadosMeuSetor(string pesquisa, string idUsuario, string idSetor)
        {
            var query =
                "SELECT distinct a.id,a.titulochamado,u1.nome CADASTRO,u2.nome AS OPERADOR FROM webdeskchamados a LEFT JOIN usuarios u1 on a.idusuariocadastro=u1.id LEFT JOIN usuarios u2 on a.idoperador=u2.id, webdeskinteracoes b where  b.idchamado=a.id and(MATCH (b.textointeracao) AGAINST ('" +
                pesquisa + "')  or a.id='" + pesquisa + "') and b.idusuariointeracao=" + idUsuario +
                " and a.idsetor='" + idSetor + "'";
            var dados = _conexaoMysql.ExecutaComandoComRetorno(query);
            return dados;
        }

        public List<Dictionary<string, string>> BuscaChamadosTotalAntigo(string pesquisa, string idUsuario)
        {
            var query =
                "SELECT distinct a.id,a.titulochamado,u1.nome CADASTRO,u2.nome AS OPERADOR FROM webdeskchamados a LEFT JOIN usuarios u1 on a.idusuariocadastro=u1.id LEFT JOIN usuarios u2 on a.idoperador=u2.id, webdeskinteracoes b where  b.idchamado=a.id and(MATCH (b.textointeracao) AGAINST ('" +
                pesquisa + "')  or a.id='" + pesquisa + "')";
            var dados = _conexaoMysql.ExecutaComandoComRetorno(query);
            return dados;
        }

        public List<Dictionary<string, string>> BuscaInteracaoChamados(string idChamado)
        {
            var query =
                "SELECT b.nome,a.textointeracao, a.datahorainteracao as data FROM webdeskinteracoes a, usuarios b WHERE a.idusuariointeracao=b.id AND idchamado=" +
                idChamado + " ";
            var dados = _conexaoMysql.ExecutaComandoComRetorno(query);
            return dados;
        }


        public List<Dictionary<string, string>> BuscaInteracaoChamadosNovo(string idChamado)
        {
            var query =
                "SELECT b.nome,a.textointeracao, a.datahorainteracao as data FROM webdeskinteracoessolicitacoes a, funcionarios b WHERE a.idfuncionariointeracao=b.id AND idsolicitacao=" +
                idChamado + " ";
            var dados = _conexaoMysql.ExecutaComandoComRetorno(query);
            return dados;
        }

        public List<SelectListItem> RetornaSetor()
        {
            var setor = new List<SelectListItem>();

            const string queryRetornaSetor =
                "SELECT id,descricao FROM setores WHERE excluido='N' ORDER BY descricao  ASC";

            var dados = _conexaoMysql.ExecutaComandoComRetorno(queryRetornaSetor);
            foreach (var row in dados)
                setor.Add(new SelectListItem
                {
                    Value = row["id"],
                    Text = row["descricao"]
                });

            return setor;
        }

        public List<SelectListItem> RetornaCategoria(string idSetor)
        {
            var categoria = new List<SelectListItem>();

            var queryRetornaCategoria = "SELECT id,descricao FROM webdeskcategorias WHERE idsetor=" + idSetor +
                                        " AND excluido='N'";

            var dados = _conexaoMysql.ExecutaComandoComRetorno(queryRetornaCategoria);
            foreach (var row in dados)
                categoria.Add(new SelectListItem
                {
                    Value = row["id"],
                    Text = row["descricao"]
                });

            return categoria;
        }

        public List<SelectListItem> RetornaFuncionario(string idSetor)
        {
            var funcionarioSetor = new List<SelectListItem>();

            var queryRetornaFuncionario =
                "SELECT id,nome FROM funcionarios WHERE idsetor=" + idSetor + " AND ativo='S'";

            var dados = _conexaoMysql.ExecutaComandoComRetorno(queryRetornaFuncionario);
            foreach (var row in dados)
                funcionarioSetor.Add(new SelectListItem
                {
                    Value = row["id"],
                    Text = row["nome"]
                });

            return funcionarioSetor;
        }

        public string CadastraSolicitacao(string idSetor, string idCategoria, string idOperador, string descricao,
            string idUsuario, string cpf)
        {
            var query =
                "INSERT INTO webdesksolicitacoes (idfuncionariocadastro,idfuncionarioresponsavel,idcategoria,idsetor,idsituacao,datahoracadastro,cpf) VALUES(" +
                idUsuario + "," + idOperador + "," + idCategoria + "," + idSetor + ",1,NOW(),'" + cpf + "')";

            var idChamado = _conexaoMysql.ExecutaComandoComRetornoId(query);

            var queryInteracao =
                "INSERT INTO webdeskinteracoessolicitacoes(idsolicitacao,textointeracao,idfuncionariointeracao,datahorainteracao) VALUES(" +
                idChamado + ",'" + descricao + "'," + idUsuario + ",NOW())";

            var idInteracao = _conexaoMysql.ExecutaComandoComRetornoId(queryInteracao);

            return idInteracao + ";" + idChamado;
        }

        public List<Dictionary<string, string>> RecuperaDadosUsuarios(string login)
        {
            var queryRecuperaUsuario =
                "SELECT * FROM funcionarios  WHERE login='" + login + "'";


            var dados = _conexaoMysql.ExecutaComandoComRetorno(queryRecuperaUsuario);


            return dados;
        }

        public void InserirAnexo(string idInteracao, byte[] foto, string tipoArquivo, string nomeArquivo)
        {
            var queryAtualizaFuncionario =
                "INSERT INTO webdeskanexos (idinteracao,arquivo,tipoarquivo,nomearquivo) VALUES(" + idInteracao +
                ",@image,'" + tipoArquivo + "','" + nomeArquivo + "') ";
            _conexaoMysql.ExecutaComandoArquivo(queryAtualizaFuncionario, foto);
        }

        public List<Dictionary<string, string>> RetornaChamadosAbertos(string idUsuario)
        {
            var query =
                "SELECT a.id,b.descricao as titulo,c.nome as operador,d.descricao as situacao,b.tempo,a.datahoracadastro,TIMEDIFF(Now() ,a.datahoracadastro) as sla,a.fimatendimento,a.cpf from webdesksolicitacoes a, webdeskcategorias b,funcionarios c,webdesksituacoes d WHERE a.idfuncionarioresponsavel = c.id AND a.idcategoria=b.id AND a.idsituacao=d.id and a.idfuncionariocadastro=" +
                idUsuario + " ";
            var chamados = _conexaoMysql.ExecutaComandoComRetorno(query);
            return chamados;
        }

        public List<Dictionary<string, string>> RetornaChamadosResponsavel(string idUsuario)
        {
            var query =
                "SELECT a.id,b.descricao as titulo,u1.nome cadastro,u2.nome AS operador,d.descricao as situacao,b.tempo,a.datahoracadastro ,TIMEDIFF(Now() ,a.datahoracadastro) as sla,a.cpf FROM webdesksolicitacoes a LEFT JOIN funcionarios u1 on a.idfuncionariocadastro=u1.id INNER JOIN funcionarios u2 on (a.idfuncionarioresponsavel=u2.id AND u2.id=" +
                idUsuario +
                ") LEFT JOIN  webdeskcategorias b on a.idcategoria=b.id INNER JOIN webdesksituacoes d on a.idsituacao =d.id AND (a.idsituacao=2 or a.idsituacao=1 or a.idsituacao=4 or a.idsituacao=5)  ORDER BY a.id";
            var chamados = _conexaoMysql.ExecutaComandoComRetorno(query);
            return chamados;
        }

        public List<Dictionary<string, string>> RetornaChamadosSetor(string idSetor)
        {
            var query =
                "SELECT a.id,b.descricao as titulo,u1.nome cadastro,u2.nome AS operador,d.descricao as situacao,b.tempo,a.datahoracadastro,TIMEDIFF(Now() ,a.datahoracadastro) as sla,a.cpf FROM webdesksolicitacoes a LEFT JOIN funcionarios u1 on a.idfuncionariocadastro=u1.id INNER JOIN funcionarios u2 on (a.idfuncionarioresponsavel=u2.id) LEFT JOIN  webdeskcategorias b on a.idcategoria=b.id INNER JOIN webdesksituacoes d on a.idsituacao =d.id AND (a.idsituacao=2 or a.idsituacao=1 or a.idsituacao=4 or a.idsituacao=5) and a.idsetor=" +
                idSetor + "  ORDER BY a.id";
            var chamados = _conexaoMysql.ExecutaComandoComRetorno(query);
            return chamados;
        }

        public List<Dictionary<string, string>> RetornaDadosChamado(string idSolicitacao)
        {
            var query =
                "SELECT a.id,b.descricao as titulo,u1.id as idcadastro,u1.nome cadastro,u2.nome AS operador,d.descricao as situacao,b.tempo,a.datahoracadastro,e.descricao as setor,a.inicioatendimento,a.fimatendimento FROM webdesksolicitacoes a LEFT JOIN funcionarios u1 on a.idfuncionariocadastro=u1.id INNER JOIN funcionarios u2 on (a.idfuncionarioresponsavel=u2.id) LEFT JOIN  webdeskcategorias b on a.idcategoria=b.id INNER JOIN webdesksituacoes d on a.idsituacao =d.id INNER JOIN setores e on a.idsetor=e.id and a.id=" +
                idSolicitacao + " ";
            var chamados = _conexaoMysql.ExecutaComandoComRetorno(query);
            return chamados;
        }

        public List<Dictionary<string, string>> RetornaInteracoesChamado(string idSolicitacao)
        {
            var query =
                "select b.id,a.nome as nome, b.textointeracao,b.datahorainteracao,b.acao from funcionarios a, webdeskinteracoessolicitacoes b where a.id=b.idfuncionariointeracao and b.idsolicitacao=" +
                idSolicitacao + " ";
            var chamados = _conexaoMysql.ExecutaComandoComRetorno(query);
            return chamados;
        }

        public DataTable RetornaAnexoInteracao(string idInteracao)
        {
            var dados = _conexaoMysql.ComandoArquivoWebDesk(idInteracao);
            return dados;
        }

        public string CadastrarInteracao(string idChamado, string descricao, string idUsuario, string acao)
        {
            var queryInteracao =
                "INSERT INTO webdeskinteracoessolicitacoes(idsolicitacao,textointeracao,idfuncionariointeracao,datahorainteracao,acao) VALUES(" +
                idChamado + ",'" + descricao + "'," + idUsuario + ",NOW(),'" + acao + "')";
            var idInteracao = _conexaoMysql.ExecutaComandoComRetornoId(queryInteracao);
            return idInteracao;
        }

        public void EncerrarSolicitacao(string idChamado)
        {
            var queryInteracao =
                "UPDATE webdesksolicitacoes SET idsituacao=3, fimatendimento=NOW() WHERE  id=" + idChamado + ";";
            _conexaoMysql.ExecutaComando(queryInteracao);
        }

        public void AlterarCategoriaSolicitacao(string idChamado, string idCategoria)
        {
            var queryInteracao =
                "UPDATE webdesksolicitacoes SET idcategoria=" + idCategoria + " WHERE id=" + idChamado + ";";
            _conexaoMysql.ExecutaComando(queryInteracao);
        }

        public void AlterarResponsavelSolicitacao(string idChamado, string idSetor, string idFuncionario)
        {
            var queryInteracao =
                "UPDATE webdesksolicitacoes SET idsetor=" + idSetor + ", idfuncionarioresponsavel=" + idFuncionario +
                " WHERE id=" + idChamado + ";";
            _conexaoMysql.ExecutaComando(queryInteracao);
        }

        public void ReabrirSolicitacao(string idChamado)
        {
            var queryInteracao =
                "UPDATE webdesksolicitacoes SET idsituacao=2 WHERE id=" + idChamado + ";";
            _conexaoMysql.ExecutaComando(queryInteracao);
        }

        public void PendenciaSolicitacao(string idChamado)
        {
            var queryInteracao =
                "UPDATE webdesksolicitacoes SET idsituacao=4 WHERE id=" + idChamado + ";";
            _conexaoMysql.ExecutaComando(queryInteracao);
        }

        public void SolucionaSolicitacao(string idChamado)
        {
            var queryInteracao =
                "UPDATE webdesksolicitacoes SET idsituacao=2 WHERE id=" + idChamado + ";";
            _conexaoMysql.ExecutaComando(queryInteracao);
        }

        public List<Dictionary<string, string>> RetornaRepasseFuncionarioChamado(string idFuncionario)
        {
            var query = "select nome from funcionarios where id=" + idFuncionario + " ";
            var chamados = _conexaoMysql.ExecutaComandoComRetorno(query);
            return chamados;
        }

        public List<Dictionary<string, string>> RetornaRepasseCategoriaChamado(string idFuncionario)
        {
            var query = "select descricao from webdeskcategorias where id=" + idFuncionario + " ";
            var chamados = _conexaoMysql.ExecutaComandoComRetorno(query);
            return chamados;
        }

        public void IniciarAtendimentoSolicitacao(string idChamado)
        {
            var queryInteracao =
                "UPDATE webdesksolicitacoes SET inicioatendimento=NOW(), idsituacao=2 WHERE id=" + idChamado + ";";
            _conexaoMysql.ExecutaComando(queryInteracao);
        }

        public List<Dictionary<string, string>> RetornaDadosChamadoAntigo(string idSolicitacao)
        {
            var query =
                "SELECT a.id,b.descricao as titulo,u1.id as idcadastro,u1.nome cadastro,u2.nome AS operador,d.descricao as situacao,b.tempo,a.datahoracadastro,e.descricao as setor,a.confirmacaoleitura,a.dataconclusaochamado FROM webdeskchamados a LEFT JOIN usuarios u1 on a.idusuariocadastro=u1.id INNER JOIN usuarios u2 on (a.idoperador=u2.id) LEFT JOIN  webdeskcategorias b on a.idcategoria=b.id INNER JOIN webdesksituacoes d on a.idsituacao =d.id INNER JOIN setores e on a.idsetor=e.id and a.id=" +
                idSolicitacao + " ";
            var chamados = _conexaoMysql.ExecutaComandoComRetorno(query);
            return chamados;
        }

        public List<Dictionary<string, string>> RetornaInteracoesChamadoAntigo(string idSolicitacao)
        {
            var query =
                "select b.id,a.nome as nome, b.textointeracao,b.datahorainteracao,b.idarquivogoogle,b.idpastaarquivogoogle from usuarios a, webdeskinteracoes b where a.id=b.idusuariointeracao and b.idchamado=" +
                idSolicitacao + " ";
            var chamados = _conexaoMysql.ExecutaComandoComRetorno(query);
            return chamados;
        }

        public List<Dictionary<string, string>> RetornaInformacoesNotificacao(string idFuncionario)
        {
            var query = "select id,email,idnotificacao,notificacaoemail from funcionarios where id=" + idFuncionario +
                        " ";
            var chamados = _conexaoMysql.ExecutaComandoComRetorno(query);
            return chamados;
        }

        public List<Dictionary<string, string>> RetornaIdSolicitantes(string idChamado)
        {
            var query = "select idfuncionariocadastro,idfuncionarioresponsavel from webdesksolicitacoes where id=" +
                        idChamado + " ";
            var chamados = _conexaoMysql.ExecutaComandoComRetorno(query);
            return chamados;
        }

        public List<Dictionary<string, string>> RetornaFormulariosSetor(string idSetor)
        {
            var query =
                "SELECT a.*,b.descricao FROM webdeskformularioscategorias a,webdeskcategorias b WHERE a.idcategoria=b.id and a.idsetor=" +
                idSetor + "";
            var formularios = _conexaoMysql.ExecutaComandoComRetorno(query);
            return formularios;
        }

        public List<Dictionary<string, string>> RetornaFormularioCategoria(string idCategoria)
        {
            var query = "SELECT * FROM webdeskformularioscategorias WHERE idcategoria=" + idCategoria + "";
            var formularios = _conexaoMysql.ExecutaComandoComRetorno(query);
            return formularios;
        }

        public List<Dictionary<string, string>> RetornaFormularioChamado(string idSolicitacao)
        {
            var query = "SELECT * FROM webdeskformularios WHERE idsolicitacao=" + idSolicitacao + "";
            var formularios = _conexaoMysql.ExecutaComandoComRetorno(query);
            return formularios;
        }

        public void InserirFormulario(string nomeCampos, string dadoFormulario, string idSolicitacao)
        {
            var query = "INSERT INTO webdeskformularios (idsolicitacao,nomecampo,dadoformulario) VALUES(" +
                        idSolicitacao + ",'" + nomeCampos + "','" + dadoFormulario + "')";
            _conexaoMysql.ExecutaComando(query);
        }

        public void CadastrarCategoria(string descricaoCategoria, string tempoCategoria, string idSetor)
        {
            var query = "INSERT INTO webdeskcategorias (descricao,tempo,idsetor) VALUES('" +
                        descricaoCategoria + "','" + tempoCategoria + "'," + idSetor + ")";
            _conexaoMysql.ExecutaComando(query);
        }

       
    }
}