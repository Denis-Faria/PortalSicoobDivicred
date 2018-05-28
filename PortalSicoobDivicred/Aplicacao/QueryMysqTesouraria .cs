using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Port.Repositorios;

namespace PortalSicoobDivicred.Aplicacao
{
    public class QueryMysqlTesouraria
    {
        private readonly Conexao ConexaoMysql;

        public QueryMysqlTesouraria()
        {
            ConexaoMysql = new Conexao();
        }


        public void InsereConferencia(String data, string historico, string extrato,string arquivos,string diferenca)
        {
            
            string QueryInsereConferencia = "INSERT INTO dadosextrato (data,historico,extrato,arquivos,diferenca) values ('" + data + "','" + historico + "','" + extrato + "','" + arquivos + "','"+ (Math.Round(Convert.ToDouble(diferenca),2)).ToString() + "') ";
            ConexaoMysql.ExecutaComando(QueryInsereConferencia);


        }

        public void InsereJustificativa(String data, string justificativa)
        {
                string QueryInsereJustificativa = "INSERT INTO justificativaextrato (data,justificativa) values ('" + data + "','" + justificativa + "') ";
                ConexaoMysql.ExecutaComando(QueryInsereJustificativa);

        }

        public List<Dictionary<string, string>> RecuperaDadosTabela(string Data)
        {
            var Query =
                "SELECT * FROM dadosextrato WHERE data='" + Data + "'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);


            return Dados;
        }

        public List<Dictionary<string, string>> RecuperaDadosJustificativa(string Data)
        {
            var Query =
                "SELECT * FROM justificativaextrato WHERE data='" + Data + "'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);


            return Dados;
        }

        public int ExisteJustificativa(string Data)
        {
            var Query =
                "SELECT count(*) FROM justificativaextrato WHERE data='" + Data + "'";
            var Dados = ConexaoMysql.ExecutaComando(Query);


            return Dados;
        }

        public void atualizaJustificativa(string data, string justificativa)
        {
            var QueryJustificativa= "UPDATE justificativaextrato  SET justificativa='" + justificativa + "' WHERE data='" + data + "'";
            ConexaoMysql.ExecutaComandoComRetorno(QueryJustificativa);
        }
    }
}