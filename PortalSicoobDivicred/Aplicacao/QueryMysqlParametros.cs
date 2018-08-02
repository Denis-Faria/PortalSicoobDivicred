using System;
using System.Collections.Generic;
using PortalSicoobDivicred.Repositorios;
using System.Web.Mvc;
using System.Web;
using DocumentFormat.OpenXml.Drawing.Diagrams;


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
            var Usuario = HttpContext.Current.Request.Cookies["CookieFarm"];
            if (Usuario == null)
                return false;
            return true;
        }

        public void InsereUsuario(string nome,int pa, string dataAdmissao,string cpf,string rg,string pis,
            string estagiario,string login,string senha,string email,int idgrupo,string gestor,string matricula)
        {
            var queryInsereFuncionario =
                "INSERT INTO funcionarios (nome,idpa,admissao,cpf,rg,pis,estagiario,login,senha,email,idgrupo,gestor,matricula) values ('" +
                nome + "','" + pa + "','" + Convert.ToDateTime(dataAdmissao).ToString("yyyy-MM-dd") +
                "','"+cpf+"','" + rg + "','" + pis+ "','" +
                estagiario+ "','"+login+"',md5('"+senha+"'),'"+email+"','"+idgrupo+"','"+gestor+"','"+matricula+"') ";
            ConexaoMysql.ExecutaComando(queryInsereFuncionario);
        }

        public void InsereGrupos(string descricao)
        {
            var queryInsereGrupos =
                "INSERT INTO grupos (descricao,excluido) values ('" +
                descricao + "','N')"; 
            ConexaoMysql.ExecutaComando(queryInsereGrupos);
        }

        
        public List<Dictionary<string, string>> BuscaFuncionario(string Nome)
        {
            var Query = "Select id,nome from funcionarios where nome like'%" + Nome + "%' and ativo='S' order by nome";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }
        
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
        }


        



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

        public List<Dictionary<string, string>> RecuperaDadosFuncionario(string IdFuncionario)
        {
            var Query = "Select * from funcionarios where id='" + IdFuncionario + "' and ativo='S'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }

        public List<Dictionary<string, string>> RecuperaDadosGrupo(string IdGrupos)
        {
            var Query = "Select * from grupos where id='" + IdGrupos + "' and excluido='N'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }

        public void AtualizaUsuario(int id,string nome, int pa, string dataAdmissao, string cpf, string rg, string pis,
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


        public void ExcluirFuncionario(string IdFuncionario)
        {
            var Query2 = "UPDATE funcionarios SET ativo='N' WHERE id=" + IdFuncionario + "";
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


    }
}