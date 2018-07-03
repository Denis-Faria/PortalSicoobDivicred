using System;
using System.Collections.Generic;
using PortalSicoobDivicred.Repositorios;


namespace PortalSicoobDivicred.Aplicacao
{
    public class QueryMysqlTesouraria
    {
        private readonly Conexao _conexaoMysql;

        public QueryMysqlTesouraria()
        {
            _conexaoMysql = new Conexao();
        }
        public string ConsultaValorNr()
        {
            string queryValor = "SELECT valor from tesourariavalornr where data = (SELECT Max(data) FROM tesourariavalornr)";

            var consultaValor = _conexaoMysql.ExecutaComandoComRetorno( queryValor );

            return consultaValor [0] ["valor"];
        }

        public void InsereValorNr(string valor, String data)
        {
            var queryInsereValorNr = "INSERT INTO tesourariavalornr (valor,data) values('" + valor.Replace( ",", "." ) + "','" + data + "')";
            _conexaoMysql.ExecutaComando( queryInsereValorNr );
        }

        public void InsereValoresCampos(string valorJudicial, string valorDevolucaoCheques, string valorCampo1, string valorCampo2, string valorCampo3, DateTime dataEscolhida, DateTime dataGeracao)
        {
            var queryInsereValorCamposDigitaveis = "INSERT INTO tesourariavalorescamposdigitaveis (valorjudicial,valordevolucaocheques,valorcampo1,valorcampo2,valorcampo3,dataescolhida,datageracao,excluido)" +
                " values('" + valorJudicial.Replace( ",", "." ) + "','" + valorDevolucaoCheques.Replace( ",", "." ) + "','" + valorCampo1.Replace( ",", "." ) + "','" + valorCampo2.Replace( ",", "." ) + "','" + valorCampo3.Replace( ",", "." ) + "','" + dataEscolhida.ToString( "yyyy-MM-dd 00:00:00" ) + "','" + dataGeracao.ToString( "yyyy-MM-dd hh:MM:ss" ) + "','N')";
            _conexaoMysql.ExecutaComando( queryInsereValorCamposDigitaveis );
        }






        public void InsereConferencia(String data, string historico, string extrato, string arquivos, string diferenca)
        {

            string queryInsereConferencia = "INSERT INTO dadosextrato (data,historico,extrato,arquivos,diferenca,excluido) values ('" + data + "','" + historico + "','" + extrato + "','" + arquivos + "','" + (Math.Round( Convert.ToDouble( diferenca ), 2 )) + "','N') ";
            _conexaoMysql.ExecutaComando( queryInsereConferencia );


        }

        public void InsereJustificativa(String data, string justificativa)
        {
            string queryInsereJustificativa = "INSERT INTO justificativaextrato (data,justificativa,excluido) values ('" + data + "','" + justificativa + "','N') ";
            _conexaoMysql.ExecutaComando( queryInsereJustificativa );

        }

        public List<Dictionary<string, string>> RecuperaDadosTabela(string data)
        {
            var query =
                "SELECT * FROM dadosextrato WHERE data='" + data + "' and excluido='N'";
            var dados = _conexaoMysql.ExecutaComandoComRetorno( query );


            return dados;
        }

        public void DeletaProducao(string data)
        {

            var query1 = "UPDATE  dadosextrato SET excluido='S',dataexclusao='" + (DateTime.Now).ToString( "yyyy-MM-dd 00:00:00" ) + "'" + " where data='" +
                            (Convert.ToDateTime( data )).ToString( "yyyy-MM-dd 00:00:00" ) + "'";

            _conexaoMysql.ExecutaComandoComRetorno( query1 );

            var query2 = "UPDATE  justificativaextrato set excluido='S',dataexclusao='" + (DateTime.Now).ToString( "yyyy-MM-dd 00:00:00" ) + "'" + "where data='" +
                (Convert.ToDateTime( data )).ToString( "yyyy-MM-dd 00:00:00" ) + "'";
            _conexaoMysql.ExecutaComandoComRetorno( query2 );

            var query3 = "UPDATE  tesourariavalorescamposdigitaveis set excluido='S',dataexclusao='" + (DateTime.Now).ToString( "yyyy-MM-dd 00:00:00" ) + "'" + "  where dataescolhida='" +
    (Convert.ToDateTime( data )).ToString( "yyyy-MM-dd 00:00:00" ) + "'";
            _conexaoMysql.ExecutaComandoComRetorno( query3 );


        }



        public List<Dictionary<string, string>> RecuperaDadosJustificativa(string data)
        {
            var query =
                "SELECT * FROM justificativaextrato WHERE data='" + data + "' and excluido='N'";
            var dados = _conexaoMysql.ExecutaComandoComRetorno( query );


            return dados;
        }

        public int ExisteJustificativa(string data)
        {
            var query =
                "SELECT count(*) FROM justificativaextrato WHERE data='" + data + "' and excluido='N'";
            var dados = _conexaoMysql.ExecutaComando( query );


            return dados;
        }

        public int VerificaFeriadoDivinopolis(DateTime data)
        {

            var query =
                "SELECT count(id_feriado) as total FROM FERIADO_nacional  WHERE DATA='" +
                data.ToString( "yyyy/MM/dd" ) + "'";
            var dados = _conexaoMysql.ExecutaComandoComRetorno( query );

            return Convert.ToInt32( dados [0] ["total"] );
        }

        public int VerificaData(string data)
        {
            var query =
                "SELECT count(*) as count FROM dadosextrato WHERE data='" + data + "'and excluido='N'";
            var dados = _conexaoMysql.ExecutaComandoComRetorno( query );

            return Convert.ToInt32( dados [0] ["count"] );
        }

        public void AtualizaJustificativa(string data, string justificativa)
        {
            var queryJustificativa = "UPDATE justificativaextrato  SET justificativa='" + justificativa + "' WHERE data='" + data + "'";
            _conexaoMysql.ExecutaComandoComRetorno( queryJustificativa );
        }
    }
}