﻿using System;
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
            
            string QueryInsereConferencia = "INSERT INTO dadosextrato (data,historico,extrato,arquivos,diferenca,excluido) values ('" + data + "','" + historico + "','" + extrato + "','" + arquivos + "','"+ (Math.Round(Convert.ToDouble(diferenca),2)).ToString() + "','N') ";
            ConexaoMysql.ExecutaComando(QueryInsereConferencia);


        }

        public void InsereJustificativa(String data, string justificativa)
        {
                string QueryInsereJustificativa = "INSERT INTO justificativaextrato (data,justificativa,excluido) values ('" + data + "','" + justificativa + "','N') ";
                ConexaoMysql.ExecutaComando(QueryInsereJustificativa);

        }

        public List<Dictionary<string, string>> RecuperaDadosTabela(string Data)
        {
            var Query =
                "SELECT * FROM dadosextrato WHERE data='" + Data + "' and excluido='N'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);


            return Dados;
        }

        public void DeletaProducao(string Data)
        {

            var Query1 = "DELETE from dadosextrato where data='" +
                            (Convert.ToDateTime(Data)).ToString("yyyy-MM-dd 00:00:00")+"'" ;
            ConexaoMysql.ExecutaComandoComRetorno(Query1);

            var Query2 = "DELETE from justificativaextrato where data='" +
                (Convert.ToDateTime(Data)).ToString("yyyy-MM-dd 00:00:00")+"'" ;
            ConexaoMysql.ExecutaComandoComRetorno(Query2);
        }



        public List<Dictionary<string, string>> RecuperaDadosJustificativa(string Data)
        {
            var Query =
                "SELECT * FROM justificativaextrato WHERE data='" + Data + "' and excluido='N'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);


            return Dados;
        }

        public int ExisteJustificativa(string Data)
        {
            var Query =
                "SELECT count(*) FROM justificativaextrato WHERE data='" + Data + "' and excluido='N'";
            var Dados = ConexaoMysql.ExecutaComando(Query);


            return Dados;
        }

        public int VerificaData(string Data)
        {
            var Query =
                "SELECT count(*) as count FROM dadosextrato WHERE data='" + Data + "'and excluido='N'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);

            return Convert.ToInt32(Dados[0]["count"]);
        }

        public void atualizaJustificativa(string data, string justificativa)
        {
            var QueryJustificativa= "UPDATE justificativaextrato  SET justificativa='" + justificativa + "' WHERE data='" + data + "'";
            ConexaoMysql.ExecutaComandoComRetorno(QueryJustificativa);
        }
    }
}