using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using PortalSicoobDivicred.Repositorios;
using PortalSicoobDivicred.Models;

namespace PortalSicoobDivicred.Aplicacao
{
    public class QueryMysqlTesouraria
    {
        private readonly Conexao ConexaoMysql;

        public QueryMysqlTesouraria()
        {
            ConexaoMysql = new Conexao();
        }

        



        public string consultaValorNR()
        {
            string QueryValor = "SELECT valor from tesourariavalornr where data = (SELECT Max(data) FROM tesourariavalornr)";

            var consultaValor = ConexaoMysql.ExecutaComandoComRetorno(QueryValor);

            return consultaValor[0]["valor"];
        }

        public void insereValorNR(string valor, String data)
        {
            var QueryInsereValorNR = "INSERT INTO tesourariavalornr (valor,data) values('"+valor.Replace(",",".")+"','"+ data.ToString() +"')";
            ConexaoMysql.ExecutaComando(QueryInsereValorNR);
        }

        public void insereValoresCampos(string valorJudicial,string valorDevolucaoCheques,string valorCampo1, string valorCampo2, string valorCampo3,DateTime dataEscolhida,DateTime dataGeracao)
        {
            var QueryInsereValorCamposDigitaveis = "INSERT INTO tesourariavalorescamposdigitaveis (valorjudicial,valordevolucaocheques,valorcampo1,valorcampo2,valorcampo3,dataescolhida,datageracao,excluido)" +
                " values('" + valorJudicial.Replace(",", ".") + "','" + valorDevolucaoCheques.Replace(",", ".") + "','" + valorCampo1.Replace(",", ".") + "','" + valorCampo2.Replace(",", ".") + "','" + valorCampo3.Replace(",", ".") + "','" + dataEscolhida.ToString("yyyy-MM-dd 00:00:00") + "','" + dataGeracao.ToString("yyyy-MM-dd hh:MM:ss") + "','N')";
            ConexaoMysql.ExecutaComando(QueryInsereValorCamposDigitaveis);
        }






        public void InsereConferencia(String data, string historico, string extrato,string arquivos,string diferenca)
        {
            
            string QueryInsereConferencia = "INSERT INTO dadosextrato (data,historico,extrato,arquivos,diferenca,excluido) values ('" + data + "','" + historico + "','" + extrato + "','" + arquivos + "','"+ (Math.Round(Convert.ToDouble(diferenca),2)).ToString() + "','N') ";
            ConexaoMysql.ExecutaComando(QueryInsereConferencia);


        }

        public void InsereJustificativa(string data, string justificativa)
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

            var Query1 = "UPDATE  dadosextrato SET excluido='S',dataexclusao='" + (DateTime.Now).ToString("yyyy-MM-dd 00:00:00") + "'" + " where data='" +
                            (Convert.ToDateTime(Data)).ToString("yyyy-MM-dd 00:00:00")+"'" ;
            
            ConexaoMysql.ExecutaComandoComRetorno(Query1);

            var Query2 = "UPDATE  justificativaextrato set excluido='S',dataexclusao='" + (DateTime.Now).ToString("yyyy-MM-dd 00:00:00") + "'" + "where data='" +
                (Convert.ToDateTime(Data)).ToString("yyyy-MM-dd 00:00:00")+"'" ;
            ConexaoMysql.ExecutaComandoComRetorno(Query2);

            var Query3 = "UPDATE  tesourariavalorescamposdigitaveis set excluido='S',dataexclusao='"+ (DateTime.Now).ToString("yyyy-MM-dd 00:00:00") + "'" + "  where dataescolhida='" +
    (Convert.ToDateTime(Data)).ToString("yyyy-MM-dd 00:00:00") + "'";
            ConexaoMysql.ExecutaComandoComRetorno(Query3);


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

        public int VerificaFeriadoDivinopolis(DateTime data)
        {

            var Query =
                "SELECT count(id_feriado) as total FROM FERIADO_nacional  WHERE DATA='" +
                data.ToString("yyyy/MM/dd")+"'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);

            return Convert.ToInt32(Dados[0]["total"]);
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