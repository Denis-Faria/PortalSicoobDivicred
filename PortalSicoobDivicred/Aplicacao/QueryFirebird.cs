using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Mvc;
using FirebirdSql.Data.FirebirdClient;

namespace PortalSicoobDivicred.Aplicacao
{
    public class QueryFirebird : IDisposable
    {
        //recebemos a connection string do Web.Config


        private FbConnection con;

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            GC.Collect();
        }

        public FbConnection AbrirConexao()
        {
            var StringConexao = ConfigurationManager.ConnectionStrings["pontorh"].ConnectionString;
            con = new FbConnection(StringConexao);
            if (con.State != ConnectionState.Open)
                con.Open();
            return con;
        }

        public FbConnection FecharConexao(FbConnection con)
        {
            if (con.State != ConnectionState.Closed)
                con.Close();
            return con;
        }

        public FbCommand CriarComandoSQL(string Query)
        {
            var Conexao = AbrirConexao();
            var ComandoSQL = new FbCommand(Query, Conexao);
            return ComandoSQL;
        }

      

        #region Consultas Uteis

        public List<Dictionary<string, string>> RetornaListaFuncionario()
        {
            List<Dictionary<string, string>> linhas = null;
            try
            {
                var cmdComando = CriarComandoSQL("SELECT * FROM FUNCIONARIO;");

                using (var reader = cmdComando.ExecuteReader())
                {
                    linhas = new List<Dictionary<string, string>>();
                    while (reader.Read())
                    {
                        var linha = new Dictionary<string, string>();

                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            var nomeDaColuna = reader.GetName(i);
                            var valorDaColuna = reader.IsDBNull(i) ? null : reader.GetString(i);
                            linha.Add(nomeDaColuna, valorDaColuna);
                        }

                        linhas.Add(linha);
                    }
                }
            }
            finally
            {
                FecharConexao(con);
            }

            return linhas;
        }

        public List<Dictionary<string, string>> RetornaListaAfastamentoFuncionario(string IdFuncionario)
        {
            var DiaValidar = new DateTime();
            if (DateTime.Now.AddDays(-1).DayOfWeek == DayOfWeek.Sunday)
                DiaValidar = DateTime.Now.AddDays(-3);
            else
                DiaValidar = DateTime.Now.AddDays(-1);
            List<Dictionary<string, string>> linhas = null;
            try
            {
                var cmdComando = CriarComandoSQL("SELECT * FROM AFASTAMENTO_FUNCIONARIO WHERE ID_FUNCIONARIO=" +
                                                 IdFuncionario + "  AND '" + DiaValidar.ToString("yyyy/MM/dd") + "' BETWEEN DATA_INICIO AND DATA_FIM ;");

                using (var reader = cmdComando.ExecuteReader())
                {
                    linhas = new List<Dictionary<string, string>>();
                    while (reader.Read())
                    {
                        var linha = new Dictionary<string, string>();

                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            var nomeDaColuna = reader.GetName(i);
                            var valorDaColuna = reader.IsDBNull(i) ? null : reader.GetString(i);
                            linha.Add(nomeDaColuna, valorDaColuna);
                        }

                        linhas.Add(linha);
                    }
                }
            }
            finally
            {
                FecharConexao(con);
            }

            return linhas;
        }

        public List<Dictionary<string, string>> VerificaFeriado(string IdFuncionario)
        {
            List<Dictionary<string, string>> linhas = null;
            try
            {
                var DiaValidar = new DateTime();

                if (DateTime.Now.AddDays(-1).DayOfWeek == DayOfWeek.Sunday)
                    DiaValidar = DateTime.Now.AddDays(-3);
                else
                    DiaValidar = DateTime.Now.AddDays(-1);

                var cmdComando = CriarComandoSQL(
                    "SELECT count(ID_FERIADO_CALENDARIO) as total FROM FERIADO_CALENDARIO  WHERE 'DATA'='" +
                    DiaValidar.ToString("yyyy/MM/dd") +
                    "' AND ID_CALENDARIO=(SELECT ID_CALENDARIO FROM CALENDARIO_FUNCIONARIO WHERE ID_FUNCIONARIO=" +
                    IdFuncionario + ")");
                using (var reader = cmdComando.ExecuteReader())
                {
                    linhas = new List<Dictionary<string, string>>();
                    while (reader.Read())
                    {
                        var linha = new Dictionary<string, string>();

                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            var nomeDaColuna = reader.GetName(i);
                            var valorDaColuna = reader.IsDBNull(i) ? null : reader.GetString(i);
                            linha.Add(nomeDaColuna, valorDaColuna);
                        }

                        linhas.Add(linha);
                    }
                }
            }
            finally
            {
                FecharConexao(con);
            }

            return linhas;
        }

        public List<Dictionary<string, string>> RetornaMarcacao(string IdFuncionario)
        {
            List<Dictionary<string, string>> linhas = null;
            try
            {

                var DiaValidar = new DateTime();
                if (DateTime.Now.AddDays(-1).DayOfWeek == DayOfWeek.Sunday)
                    DiaValidar = DateTime.Now.AddDays(-3);
                else
                    DiaValidar = DateTime.Now.AddDays(-1);

                var cmdComando = CriarComandoSQL(
                    "SELECT   b.ID_CARGO, a.HORA,a.DATA from MARCACAO a, FUNCIONARIO b WHERE a.DATA='"+DiaValidar.ToString("yyyy/MM/dd")+"' AND a.ID_FUNCIONARIO="+IdFuncionario+" AND a.ID_FUNCIONARIO=b.ID_FUNCIONARIO");

                using (var reader = cmdComando.ExecuteReader())
                {
                    linhas = new List<Dictionary<string, string>>();
                    while (reader.Read())
                    {
                        var linha = new Dictionary<string, string>();

                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            var nomeDaColuna = reader.GetName(i);
                            var valorDaColuna = reader.IsDBNull(i) ? null : reader.GetString(i);
                            linha.Add(nomeDaColuna, valorDaColuna);
                        }

                        linhas.Add(linha);
                    }
                }
            }
            finally
            {
                FecharConexao(con);
            }

            return linhas;
        }

        #endregion


        public List<Dictionary<string, string>> VerificaFalta(string IdFuncionario)
        {
            List<Dictionary<string, string>> linhas = null;
            try
            {
                var DiaValidar = new DateTime();

                if (DateTime.Now.AddDays(-1).DayOfWeek == DayOfWeek.Sunday)
                    DiaValidar = DateTime.Now.AddDays(-3);
                else
                    DiaValidar = DateTime.Now.AddDays(-1);


                var cmdComando = CriarComandoSQL("select * from MARCACAO  WHERE DATA='" +
                                                 DiaValidar.ToString("yyyy/MM/dd") + "' AND ID_FUNCIONARIO=" +
                                                 IdFuncionario + " order by HORA asc;");

                using (var reader = cmdComando.ExecuteReader())
                {
                    linhas = new List<Dictionary<string, string>>();
                    while (reader.Read())
                    {
                        var linha = new Dictionary<string, string>();

                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            var nomeDaColuna = reader.GetName(i);
                            var valorDaColuna = reader.IsDBNull(i) ? null : reader.GetString(i);
                            linha.Add(nomeDaColuna, valorDaColuna);
                        }

                        linhas.Add(linha);
                    }
                }
            }
            finally
            {
                FecharConexao(con);
            }

            return linhas;
        }


  

        public List<Dictionary<string,string>> RecuperaJustificativas()
        {
            
            List<Dictionary<string, string>> linhas = null;

            var cmdComando = CriarComandoSQL("SELECT ID_JUSTIFICATIVA, DESCRICAO FROM JUSTIFICATIVA");
            try
            {
                using (var reader = cmdComando.ExecuteReader())
                {
                    linhas = new List<Dictionary<string, string>>();
                    while (reader.Read())
                    {
                        var linha = new Dictionary<string, string>();

                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            var nomeDaColuna = reader.GetName(i);
                            var valorDaColuna = reader.IsDBNull(i) ? null : reader.GetString(i);
                            linha.Add(nomeDaColuna, valorDaColuna);
                        }

                        linhas.Add(linha);
                    }
                }
            }
            finally
            {
                FecharConexao(con);
            }
            

            return linhas;
        }
    }
}