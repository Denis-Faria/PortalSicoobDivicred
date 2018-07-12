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

        public void InsereUsuario(string nome,string pa, string dataAdmissao,string cpf,string rg,string pis, string login,
            string email,char gestor,char estagiario)
        {
            var queryInsereProducao =
                "INSERT INTO cimproducao (cpf,sexo,pa,admissao,cpf,rg,pis,vencimentoperiodico,funcao,idescolaridade,) values ('" +
                cpf + "','" + produtos + "','" + observacao + "','" + Convert.ToDateTime(data).ToString("yyyy-MM-dd") +
                "','N','" + login + "','" + valor.Replace(",", ".") + "','" +
                valorponto.Replace(".", "").Replace(",", ".") + "') ";
            _conexaoMysql.ExecutaComando(queryInsereProducao);
        }

    }
}