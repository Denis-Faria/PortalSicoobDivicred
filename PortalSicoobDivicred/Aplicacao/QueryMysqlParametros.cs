using System.Collections.Generic;
using PortalSicoobDivicred.Repositorios;
using System.Web.Mvc;
using System.Web;
using System;



namespace PortalSicoobDivicred.Aplicacao
{
    public class QueryMysqlParametros
    {
        private readonly Conexao ConexaoMysql;
        

        public QueryMysqlParametros()
        {
            ConexaoMysql = new Conexao();
        }


        public List<SelectListItem> RetornaGrupos()
        {
            var grupos = new List<SelectListItem>();

            const string queryRetornaGrupos = "SELECT id,descricao FROM grupos descricao where excluido='N' ";

            var dados = ConexaoMysql.ExecutaComandoComRetorno(queryRetornaGrupos);
            foreach (var row in dados)
                grupos.Add(new SelectListItem
                {
                    Value = row["id"],
                    Text = row["descricao"]
                });

            return grupos;
        }

        public List<SelectListItem> RetornaTarefas()
        {
            var tarefas = new List<SelectListItem>();

            const string queryRetornaTarefas = "SELECT id,descricao FROM webdesktarefas descricao where excluido='N' ";

            var dados = ConexaoMysql.ExecutaComandoComRetorno(queryRetornaTarefas);
            foreach (var row in dados)
                tarefas.Add(new SelectListItem
                {
                    Value = row["id"],
                    Text = row["descricao"]
                });

            return tarefas;
        }

        public List<SelectListItem> RetornaFuncionarios()
        {
            var funcionarios = new List<SelectListItem>();

            const string queryRetornaFuncionarios = "SELECT id,nome FROM funcionarios descricao where ativo='S' order by nome";

            var dados = ConexaoMysql.ExecutaComandoComRetorno(queryRetornaFuncionarios);
            foreach (var row in dados)
                funcionarios.Add(new SelectListItem
                {
                    Value = row["id"],
                    Text = row["nome"]
                });

            return funcionarios;
        }


        public List<SelectListItem> RetornaPermissoesGrupo(string grupo)
        {
            var grupos = new List<SelectListItem>();

            string queryRetornaPermissoesGrupo = "SELECT idpermissao FROM permissoesgrupo where idgrupo='" + grupo+"' ";

            var dados = ConexaoMysql.ExecutaComandoComRetorno(queryRetornaPermissoesGrupo);
            foreach (var row in dados)
                grupos.Add(new SelectListItem
                {
                    Value = row["idpermissao"],
                    Text = row["idpermissao"]
                });

            return grupos;
        }





        public List<SelectListItem> RetornaPermissoes()
        {
            var permissoes = new List<SelectListItem>();

            const string queryRetornaPermissoes = "SELECT idpermissao,idgrupo FROM permissoesgrupo";

            var dados = ConexaoMysql.ExecutaComandoComRetorno(queryRetornaPermissoes);
            foreach (var row in dados)
                permissoes.Add(new SelectListItem
                {
                    Value = row["idpermissao"],
                    Text = row["idpermissao"]
                });

            return permissoes;
        }




        public bool UsuarioLogado()
        {
            var usuario = HttpContext.Current.Request.Cookies["CookieFarm"];
            if (usuario == null)
                return false;
            return true;
        }

        public void InsereUsuario(string nome,int pa,string dataAdmissao,string cpf,string rg,string pis,
            string estagiario,string login,string senha,string email,int idgrupo,string gestor,string matricula)
        {
            var queryInsereFuncionario =
                "INSERT INTO funcionarios (nome,idpa,admissao,cpf,rg,pis,estagiario,login,senha,email,idgrupo,gestor,matricula) values ('" +
                nome + "','" + pa + "','" + Convert.ToDateTime(dataAdmissao).ToString("yyyy-MM-dd") +
                "','"+cpf+"','" + rg + "','" + pis+ "','" +
                estagiario+ "','"+login+"',md5('"+senha+"'),'"+email+"','"+idgrupo+"','"+gestor+"','"+matricula+"') ";
            ConexaoMysql.ExecutaComando(queryInsereFuncionario);
        }
       
        public void InsereSubtarefa(string subtarefasDescricao, int idTarefaSubtarefas, int id, string TempoSubTarefa,string MultipliAtendente)
        {
            var queryInsereSubtarefa =
                "INSERT INTO webdesksubtarefas (descricao,idtarefa,idfuncionarioresponsavel,excluido,tempo,multiploatendente) values ('" +
                subtarefasDescricao + "','" + idTarefaSubtarefas + "','" + id + "','N','" +TempoSubTarefa + "','"+MultipliAtendente+"') ";
            ConexaoMysql.ExecutaComando(queryInsereSubtarefa);
        }

        public void InserePermissaoFuncionario(string idpermissao, string idFuncionario)
        {
            var queryInserePermissaoFuncionario =
                "INSERT INTO permissoesfuncionarios (idpermissao,idfuncionario,idaplicativo,valor) values ('" +
                idpermissao + "','" + idFuncionario + "','0','S') ";
            ConexaoMysql.ExecutaComando(queryInserePermissaoFuncionario);
        }

        public void InsereGrupos(string descricao)
        {
            var queryInsereGrupos =
                "INSERT INTO grupos (descricao,excluido) values ('" +
                descricao + "','N')"; 
            ConexaoMysql.ExecutaComando(queryInsereGrupos);
        }

        public void InsereTarefas(string descricao)
        {
            var queryInsereTarefas =
                "INSERT INTO webdesktarefas (descricao,excluido) values ('" +
                descricao + "','N')";
            ConexaoMysql.ExecutaComando(queryInsereTarefas);
        }


        public List<Dictionary<string, string>> BuscaFuncionario(string Nome)
        {
            var Query = "Select id,nome from funcionarios where nome like'%" + Nome + "%' and ativo='S' order by nome";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }

        public List<Dictionary<string, string>> BuscaPermissoesFuncionario(string id)
        {
            var Query =
                "select id,descricao from permissoes where descricao not in (select idpermissao from permissoesfuncionarios where idfuncionario ='" +
                id + "' and valor='S')";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;

        }


        public List<Dictionary<string, string>> BuscaPermissoesAtivasFuncionario(string id)
        {
            var Query =
                "select id,descricao from permissoes where descricao in (select idpermissao from permissoesfuncionarios where idfuncionario ='" +
                id + "' and valor='S')";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;

        }


        public List<Dictionary<string, string>> BuscaPermissoesCadastradasFuncionario(string id)
        {
            var Query =
                "select id,descricao from permissoes where descricao in (select idpermissao from permissoesfuncionarios where idfuncionario ='" +
                id + "')";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;

        }

        /*
        public List<SelectListItem> BuscaPermissoesFuncionario(string id)
        {
            var permissoes = new List<SelectListItem>();
            //  var Query = " select idpermissao from permissoesfuncionarios where idfuncionario = (select id from funcionarios where nome like '%"+NomeFruncionario+"%' )";
            var Query =
                "select descricao from permissoes where descricao not in (select idpermissao from permissoesfuncionarios where idfuncionario ='" +
                id + "')";
            
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);

            foreach(var row in Dados)
                permissoes.Add(new SelectListItem
                {
                    Value = row["descricao"],
                    Text = row["descricao"]

                });
            return permissoes;
        }*/






        public List<Dictionary<string, string>> BuscaPermissao(string Permissao)
        {
            var Query = "Select idpermissao,idgrupo,valor from permissoesgrupo where nome like'%" + Permissao + " order by idpermissao";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }

        public List<Dictionary<string, string>> BuscaGrupo(string DescricaoGrupo)
        {
            var Query = "Select id,descricao from grupos where descricao like'%" + DescricaoGrupo + "%' and excluido='N' order by descricao";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }

        public List<Dictionary<string, string>> BuscaTarefa(string DescricaoTarefa)
        {
            var Query = "Select id,descricao from webdesktarefas where descricao like'%" + DescricaoTarefa + "%' and excluido='N' order by descricao";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }

        public List<Dictionary<string, string>> BuscaSubtarefa(string DescricaoTarefa)
        {
            var Query = "Select id,descricao from webdesksubtarefas where descricao like'%" + DescricaoTarefa + "%' and excluido='N' order by descricao";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }

        public List<Dictionary<string, string>> RecuperaDadosFuncionario(string IdFuncionario)
        {
            var Query = "Select * from funcionarios where id='" + IdFuncionario + "' and ativo='S' order by nome";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }

        public List<Dictionary<string, string>> RecuperaDadosGrupo(string IdGrupos)
        {
            var Query = "Select * from grupos where id='" + IdGrupos + "' and excluido='N'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }

        public void AtualizaFuncionario(int id,string nome, int pa, string dataAdmissao, string cpf, string rg, string pis,
            string estagiario, string login,string email, int idgrupo, string gestor, string matricula)
        {
            var queryAlteraFuncionario = "UPDATE FUNCIONARIOS SET nome='" + nome + "',idpa='" + pa + "',admissao='" +
                                         Convert.ToDateTime(dataAdmissao).ToString("yyyy-MM-dd") + "',cpf='"+cpf+"',rg='" + rg + "',pis='" + pis + "',estagiario='" + estagiario + "'," +
                                         " login='" + login + "',email='" + email + "',idgrupo='" + idgrupo +
                                         "',gestor='" + gestor + "',matricula='" + matricula + "' where id='"+id+"'";            
            var Dados = ConexaoMysql.ExecutaComando(queryAlteraFuncionario);
            
        }

        public void AtualizaGrupos(int id, string descricao)
        {
            var queryAlteraGrupos = "UPDATE grupos SET descricao='" + descricao + "' where id='" + id + "'";
            var Dados = ConexaoMysql.ExecutaComando(queryAlteraGrupos);

        }

        public void ReiniciarSenha(string id)
        {
            var queryReiniciarSenha = "UPDATE funcionarios SET senha=md5('123') where id='"+id+"'";
            var Dados = ConexaoMysql.ExecutaComando(queryReiniciarSenha);

        }

        public void IncluiPermissaoInativa(string permissao, string idFuncionario)
        {
            var queryAtualizaStatusPermissao = "UPDATE permissoesfuncionarios SET valor='S' where idpermissao='" + permissao + "' and idfuncionario='" + idFuncionario + "'";
            ConexaoMysql.ExecutaComando(queryAtualizaStatusPermissao);

        }

        public void ExcluiPermissaoFuncionario(string idpermissao,string idfuncionario)
        {
            var queryExcluiPermissao = "UPDATE permissoesfuncionarios SET valor='N' where idpermissao='" + idpermissao + "' and idfuncionario='"+idfuncionario+"'";
            ConexaoMysql.ExecutaComando(queryExcluiPermissao);

        }


        public void ExcluirFuncionario(string IdFuncionario)
        {
            var Query2 = "UPDATE funcionarios SET ativo='N' WHERE id='" + IdFuncionario + "'";
            ConexaoMysql.ExecutaComando(Query2);
        }

        public void ExcluirTarefa(string IdTarefa)
        {
            var Query2 = "UPDATE webdesktarefas SET excluido='S' WHERE id='" + IdTarefa + "'";
            ConexaoMysql.ExecutaComando(Query2);
        }

        public void ExcluirSubtarefa(string IdSubtarefa)
        {
            var Query2 = "UPDATE webdesksubtarefas SET excluido='S' WHERE id='" + IdSubtarefa + "'";
            ConexaoMysql.ExecutaComando(Query2);
        }

        public void AtualizarTarefa(string IdTarefa,string descricao)
        {
            var Query2 = "UPDATE webdesktarefas SET descricao='"+descricao+"' WHERE id='" + IdTarefa + "'";
            ConexaoMysql.ExecutaComando(Query2);
        }

        public void AtualizarSubtarefa(int IdSubtarefa, string descricaoSubTarefa,int idTarefa,int idFuncionarioResponsavel,string tempoSubTarefa,string multiploAtendente)
        {
            var Query2 = "UPDATE webdesksubtarefas SET descricao='" + descricaoSubTarefa + "'," +
                         " idtarefa='"+idTarefa+"', idfuncionarioresponsavel='"+idFuncionarioResponsavel+"' ,tempo='"+tempoSubTarefa+"' WHERE id='" + IdSubtarefa + "'";
            ConexaoMysql.ExecutaComando(Query2);
        }


        public void ExcluirGrupo(string IdGrupo)
        {
            var Query2 = "UPDATE grupos SET excluido='S' WHERE id=" + IdGrupo + "";
            ConexaoMysql.ExecutaComando(Query2);
        }

        public void AtualizaPermissao(int idGrupo,string permissao,string definicaoPermissao)
        {
            var Query2 = "UPDATE permissoesgrupo SET valor='"+definicaoPermissao+"' WHERE idpermissao='" + permissao + "' and idgrupo='"+idGrupo+"'";
            ConexaoMysql.ExecutaComando(Query2);
        }

        public string RetornaPermissoesDefinicaoGrupo(string idGrupo, string idpermissao)
        {
            var queryPermissaoDefinicao =
                "select valor from permissoesgrupo where idpermissao='"+idpermissao+"' and idgrupo='"+idGrupo+"'";

            var consultaValor = ConexaoMysql.ExecutaComandoComRetorno(queryPermissaoDefinicao);

            return consultaValor[0]["valor"];
        }

        public List<Dictionary<string, string>> RecuperaDadosTarefa(string IdTarefa)
        {
            var Query = "Select * from webdesktarefas where id='" + IdTarefa + "' and excluido='N'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }

        public List<Dictionary<string, string>> RecuperaDadosSubtarefa(string IdSubtarefa)
        {
            var Query = "Select * from webdesksubtarefas where id='" + IdSubtarefa + "' and excluido='N'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }


    }
}