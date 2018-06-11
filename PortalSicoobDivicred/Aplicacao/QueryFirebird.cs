using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
                var cmdComando = CriarComandoSQL("SELECT * FROM FUNCIONARIO WHERE DATA_DEMISSAO IS NULL;");

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

        public List<Dictionary<string, string>> RetornaFuncionarioMatricula(string Matricula)
        {
            List<Dictionary<string, string>> linhas = null;
            try
            {
                var cmdComando = CriarComandoSQL("SELECT * FROM FUNCIONARIO WHERE MATRICULA=" + Matricula +
                                                 " AND DATA_DEMISSAO IS NULL;");

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

        public List<Dictionary<string, string>> RetornaListaAfastamentoFuncionario(string IdFuncionario,
            DateTime DiaValidar)
        {
            List<Dictionary<string, string>> linhas = null;
            try
            {
                var cmdComando = CriarComandoSQL("SELECT * FROM AFASTAMENTO_FUNCIONARIO WHERE ID_FUNCIONARIO=" +
                                                 IdFuncionario + "  AND '" + DiaValidar.ToString("yyyy/MM/dd") +
                                                 "' BETWEEN DATA_INICIO AND DATA_FIM ;");

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
                    "' AND ID_CALENDARIO=(SELECT max(ID_CALENDARIO) FROM CALENDARIO_FUNCIONARIO WHERE ID_FUNCIONARIO=" +
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

        public List<Dictionary<string, string>> VerificaFeriadoDivinopolis(DateTime data)
        {
            List<Dictionary<string, string>> linhas = null;
            try
            {
               // var DiaValidar = new DateTime();

//                if (DateTime.Now.AddDays(-1).DayOfWeek == DayOfWeek.Sunday)
  //                  DiaValidar = DateTime.Now.AddDays(-3);
    //            else
      //              DiaValidar = DateTime.Now.AddDays(-1);
      //
                var cmdComando = CriarComandoSQL(
                    "SELECT count(ID_FERIADO_CALENDARIO) as total FROM FERIADO_CALENDARIO  WHERE DATA='" +
                    data.ToString("yyyy/MM/dd") +
                    "' AND ID_CALENDARIO=2");
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


        public List<Dictionary<string, string>> RetornaMarcacao(string IdFuncionario, DateTime DiaValidar)
        {
            List<Dictionary<string, string>> linhas = null;
            try
            {
                var cmdComando = CriarComandoSQL(
                    "SELECT   b.ID_CARGO, a.HORA,a.DATA from MARCACAO a, FUNCIONARIO b WHERE a.DATA='" +
                    DiaValidar.ToString("yyyy/MM/dd") + "' AND a.ID_FUNCIONARIO=" + IdFuncionario +
                    " AND a.ID_FUNCIONARIO=b.ID_FUNCIONARIO ORDER BY HORA ASC");

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

        public List<Dictionary<string, string>> RetornaIdFuncionario(string NomeFuncionario)
        {
            List<Dictionary<string, string>> linhas = null;
            try
            {
                var cmdComando = CriarComandoSQL(
                    "SELECT   ID_FUNCIONARIO from FUNCIONARIO  WHERE NOME LIKE'%" + NomeFuncionario + "%'");

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

        public List<Dictionary<string, string>> RetornaCrachaFuncionario(string NomeFuncionario)
        {
            List<Dictionary<string, string>> linhas = null;
            try
            {
                var cmdComando = CriarComandoSQL(
                    "SELECT   MATRICULA from FUNCIONARIO  WHERE NOME LIKE'%" + NomeFuncionario + "%'");

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

        public List<Dictionary<string, string>> RetornaDadosMarcacao(string DataMarcacao, string IdFuncionario)
        {
            List<Dictionary<string, string>> linhas = null;
            try
            {
                var cmdComando = CriarComandoSQL(
                    "SELECT * from MARCACAO  WHERE DATA='" + Convert.ToDateTime(DataMarcacao).ToString("yyyy/MM/dd") +
                    "' AND ID_FUNCIONARIO=" + IdFuncionario + "");

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

        public void InserirMarcacao(string IdFuncionarioFireBird, string IdJustificativaFireBird, string DataPendencia,
            string HoraPendencia)
        {
            var UltimosDados = RetornaUltimaId();
            var PisFuncionario = RetornaPis(IdFuncionarioFireBird);

            try
            {
                var cmdComando = CriarComandoSQL(
                    "INSERT INTO MARCACAO(ID_FUNCIONARIO, ID_JUSTIFICATIVA, NUMERO_REP, PIS, SEQUENCIAL, DATA, HORA, TIPO_REGISTRO, TIPO_MARCACAO, IDENTIFICACAO) VALUES(" +
                    IdFuncionarioFireBird + ", " +
                    IdJustificativaFireBird + ",0,'" + PisFuncionario[0]["PIS"] + "'," +
                    (Convert.ToInt32(UltimosDados[0]["SEQUENCIAL"]) + 1) + ", '" +
                    Convert.ToDateTime(DataPendencia).ToString("yyyy/MM/dd") + "','" + HoraPendencia + "', 'I','', '" +
                    PisFuncionario[0]["PIS"] + "'); ");
                cmdComando.ExecuteNonQuery();
                
            }
            finally
            {
                FecharConexao(con);
            }
        }

        public List<Dictionary<string, string>> RetornaUltimaId()
        {
            List<Dictionary<string, string>> linhas = null;
            try
            {
                var cmdComando = CriarComandoSQL(
                    "SELECT FIRST 1 ID_MARCACAO,SEQUENCIAL FROM MARCACAO ORDER BY SEQUENCIAL DESC;");

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

        public List<Dictionary<string, string>> RetornaPis(string IdFuncionario)
        {
            List<Dictionary<string, string>> linhas = null;
            try
            {
                var cmdComando = CriarComandoSQL(
                    "SELECT PIS FROM FUNCIONARIO WHERE ID_FUNCIONARIO=" + IdFuncionario + ";");

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

        public List<Dictionary<string, string>> RecuperaJustificativas()
        {
            List<Dictionary<string, string>> linhas = null;

            var cmdComando =
                CriarComandoSQL("SELECT ID_JUSTIFICATIVA, DESCRICAO FROM JUSTIFICATIVA ORDER BY DESCRICAO ASC");
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

        public List<Dictionary<string, string>> RecuperaJustificativasFuncioanrio(string IdJustificativa)
        {
            List<Dictionary<string, string>> linhas = null;

            var cmdComando = CriarComandoSQL("SELECT DESCRICAO FROM JUSTIFICATIVA WHERE  ID_JUSTIFICATIVA=" +
                                             IdJustificativa + "");
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

        #endregion
    }
}