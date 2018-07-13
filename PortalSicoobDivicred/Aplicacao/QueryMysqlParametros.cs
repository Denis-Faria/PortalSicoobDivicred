using System;
using System.Collections.Generic;
using PortalSicoobDivicred.Repositorios;
using System.Web.Mvc;
using System.Web;




namespace PortalSicoobDivicred.Aplicacao
{
    public class QueryMysqlParametros
    {
        private readonly Conexao _conexaoMysql;

        public QueryMysqlParametros()
        {
            _conexaoMysql = new Conexao();
        }
        public List<SelectListItem> RetornaGrupos()
        {
            var grupos = new List<SelectListItem>();

            const string queryRetornaGrupos = "SELECT id,descricao FROM grupos descricao where excluido='N' ";

            var dados = _conexaoMysql.ExecutaComandoComRetorno(queryRetornaGrupos);
            foreach (var row in dados)
                grupos.Add(new SelectListItem
                {
                    Value = row["id"],
                    Text = row["descricao"]
                });

            return grupos;
        }

        public bool UsuarioLogado()
        {
            var Usuario = HttpContext.Current.Request.Cookies["CookieFarm"];
            if (Usuario == null)
                return false;
            return true;
        }

        public void InsereUsuario(string nome,int pa, DateTime dataAdmissao,string cpf,string rg,string pis,
            string estagiario,string login,string senha,string email,int idgrupo,string gestor,string matricula)
        {
            var queryInsereFuncionario =
                "INSERT INTO funcionarios (nome,idpa,admissao,cpf,rg,pis,estagiario,login,senha,email,idgrupo,gestor,matricula) values ('" +
                nome + "','" + pa + "','" + dataAdmissao.ToString("yyyy-MM-dd") +
                "','"+cpf+"','" + rg + "','" + pis+ "','" +
                estagiario+ "','"+login+"','"+senha+"','"+email+"','"+idgrupo+"','"+gestor+"','"+matricula+"') ";
            _conexaoMysql.ExecutaComando(queryInsereFuncionario);
        }

    }
}