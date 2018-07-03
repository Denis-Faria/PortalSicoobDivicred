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


        private FbConnection _connexao;

        public void Dispose()
        {
            GC.SuppressFinalize( this );
            GC.Collect();
        }

        public FbConnection AbrirConexao()
        {
            var stringConexao = ConfigurationManager.ConnectionStrings ["pontorh"].ConnectionString;
            _connexao = new FbConnection( stringConexao );
            if ( _connexao.State != ConnectionState.Open )
                _connexao.Open();
            return _connexao;
        }

        public FbConnection FecharConexao(FbConnection conexao)
        {
            if ( conexao.State != ConnectionState.Closed )
                conexao.Close();
            return conexao;
        }

        public FbCommand CriarComandoSql(string query)
        {
            var conexao = AbrirConexao();
            var comandoSql = new FbCommand( query, conexao );
            return comandoSql;
        }


        #region Consultas Uteis

        public List<Dictionary<string, string>> RetornaListaFuncionario()
        {
            List<Dictionary<string, string>> linhas;
            try
            {
                var cmdComando = CriarComandoSql( "SELECT * FROM FUNCIONARIO WHERE DATA_DEMISSAO IS NULL;" );

                using ( var reader = cmdComando.ExecuteReader() )
                {
                    linhas = new List<Dictionary<string, string>>();
                    while ( reader.Read() )
                    {
                        var linha = new Dictionary<string, string>();

                        for ( var i = 0; i < reader.FieldCount; i++ )
                        {
                            var nomeDaColuna = reader.GetName( i );
                            var valorDaColuna = reader.IsDBNull( i ) ? null : reader.GetString( i );
                            linha.Add( nomeDaColuna, valorDaColuna );
                        }

                        linhas.Add( linha );
                    }
                }
            }
            finally
            {
                FecharConexao( _connexao );
            }

            return linhas;
        }

        public List<Dictionary<string, string>> RetornaFuncionarioMatricula(string matricula)
        {
            List<Dictionary<string, string>> linhas;
            try
            {
                var cmdComando = CriarComandoSql( "SELECT * FROM FUNCIONARIO WHERE MATRICULA=" + matricula +
                                                 " AND DATA_DEMISSAO IS NULL;" );

                using ( var reader = cmdComando.ExecuteReader() )
                {
                    linhas = new List<Dictionary<string, string>>();
                    while ( reader.Read() )
                    {
                        var linha = new Dictionary<string, string>();

                        for ( var i = 0; i < reader.FieldCount; i++ )
                        {
                            var nomeDaColuna = reader.GetName( i );
                            var valorDaColuna = reader.IsDBNull( i ) ? null : reader.GetString( i );
                            linha.Add( nomeDaColuna, valorDaColuna );
                        }

                        linhas.Add( linha );
                    }
                }
            }
            finally
            {
                FecharConexao( _connexao );
            }

            return linhas;
        }

        public List<Dictionary<string, string>> RetornaListaAfastamentoFuncionario(string idFuncionario,
            DateTime diaValidar)
        {
            List<Dictionary<string, string>> linhas;
            try
            {
                var cmdComando = CriarComandoSql( "SELECT * FROM AFASTAMENTO_FUNCIONARIO WHERE ID_FUNCIONARIO=" +
                                                 idFuncionario + "  AND '" + diaValidar.ToString( "yyyy/MM/dd" ) +
                                                 "' BETWEEN DATA_INICIO AND DATA_FIM ;" );

                using ( var reader = cmdComando.ExecuteReader() )
                {
                    linhas = new List<Dictionary<string, string>>();
                    while ( reader.Read() )
                    {
                        var linha = new Dictionary<string, string>();

                        for ( var i = 0; i < reader.FieldCount; i++ )
                        {
                            var nomeDaColuna = reader.GetName( i );
                            var valorDaColuna = reader.IsDBNull( i ) ? null : reader.GetString( i );
                            linha.Add( nomeDaColuna, valorDaColuna );
                        }

                        linhas.Add( linha );
                    }
                }
            }
            finally
            {
                FecharConexao( _connexao );
            }

            return linhas;
        }
        public List<Dictionary<string, string>> RetornaDataAdmissao(string idFuncionario, DateTime diaValidar)
        {
            List<Dictionary<string, string>> linhas;
            try
            {
                var cmdComando = CriarComandoSql(
                    "SELECT * FROM FUNCIONARIO WHERE ID_FUNCIONARIO=" +
                    idFuncionario + " AND DATA_ADMISSAO <='" + diaValidar.ToString( "yyyy/MM/dd" ) + "';" );
                using ( var reader = cmdComando.ExecuteReader() )
                {
                    linhas = new List<Dictionary<string, string>>();
                    while ( reader.Read() )
                    {
                        var linha = new Dictionary<string, string>();

                        for ( var i = 0; i < reader.FieldCount; i++ )
                        {
                            var nomeDaColuna = reader.GetName( i );
                            var valorDaColuna = reader.IsDBNull( i ) ? null : reader.GetString( i );
                            linha.Add( nomeDaColuna, valorDaColuna );
                        }

                        linhas.Add( linha );
                    }
                }
            }
            finally
            {
                FecharConexao( _connexao );
            }

            return linhas;
        }
        public List<Dictionary<string, string>> VerificaFeriado(string idFuncionario, string idCalendario, DateTime diaValidar)
        {
            List<Dictionary<string, string>> linhas;
            try
            {
                var cmdComando = CriarComandoSql(
                    "SELECT count(ID_FERIADO_CALENDARIO) as total FROM FERIADO_CALENDARIO  WHERE DATA = '" + diaValidar.ToString( "yyyy/MM/dd" ) +
                    "' AND ID_CALENDARIO=" + idCalendario + "" );
                using ( var reader = cmdComando.ExecuteReader() )
                {
                    linhas = new List<Dictionary<string, string>>();
                    while ( reader.Read() )
                    {
                        var linha = new Dictionary<string, string>();

                        for ( var i = 0; i < reader.FieldCount; i++ )
                        {
                            var nomeDaColuna = reader.GetName( i );
                            var valorDaColuna = reader.IsDBNull( i ) ? null : reader.GetString( i );
                            linha.Add( nomeDaColuna, valorDaColuna );
                        }

                        linhas.Add( linha );
                    }
                }
            }
            finally
            {
                FecharConexao( _connexao );
            }

            return linhas;
        }

        public List<Dictionary<string, string>> VerificaCalendario(string idFuncionario, DateTime diaValidar)
        {
            List<Dictionary<string, string>> linhas;
            try
            {


                var cmdComando = CriarComandoSql( "SELECT ID_CALENDARIO FROM CALENDARIO_FUNCIONARIO WHERE ID_FUNCIONARIO = " + idFuncionario + " and DATA_INICIO <= '" + diaValidar.ToString( "yyyy/MM/dd" ) + "' ORDER BY DATA_INICIO DESC" );
                using ( var reader = cmdComando.ExecuteReader() )
                {
                    linhas = new List<Dictionary<string, string>>();
                    while ( reader.Read() )
                    {
                        var linha = new Dictionary<string, string>();

                        for ( var i = 0; i < reader.FieldCount; i++ )
                        {
                            var nomeDaColuna = reader.GetName( i );
                            var valorDaColuna = reader.IsDBNull( i ) ? null : reader.GetString( i );
                            linha.Add( nomeDaColuna, valorDaColuna );
                        }

                        linhas.Add( linha );
                    }
                }
            }
            finally
            {
                FecharConexao( _connexao );
            }

            return linhas;
        }

        public List<Dictionary<string, string>> BuscaJornada(string idFuncionario, string dataAtualizacao)
        {
            List<Dictionary<string, string>> linhas;
            try
            {

                var cmdComando = CriarComandoSql( "SELECT ID_JORNADA FROM JORNADA_FUNCIONARIO WHERE ID_FUNCIONARIO=" + idFuncionario +
                            " AND DATA_INICIO <='" + dataAtualizacao + "' ORDER BY DATA_INICIO desc;");

                using ( var reader = cmdComando.ExecuteReader() )
                {
                    linhas = new List<Dictionary<string, string>>();
                    while ( reader.Read() )
                    {
                        var linha = new Dictionary<string, string>();

                        for ( var i = 0; i < reader.FieldCount; i++ )
                        {
                            var nomeDaColuna = reader.GetName( i );
                            var valorDaColuna = reader.IsDBNull( i ) ? null : reader.GetString( i );
                            linha.Add( nomeDaColuna, valorDaColuna );
                        }

                        linhas.Add( linha );
                    }
                }
            }
            finally
            {
                FecharConexao( _connexao );
            }

            return linhas;
        }



        public List<Dictionary<string, string>> VerificaFeriadoDivinopolis(DateTime data)
        {
            List<Dictionary<string, string>> linhas;
            try
            {
                // var DiaValidar = new DateTime();

                //                if (DateTime.Now.AddDays(-1).DayOfWeek == DayOfWeek.Sunday)
                //                  DiaValidar = DateTime.Now.AddDays(-3);
                //            else
                //              DiaValidar = DateTime.Now.AddDays(-1);
                //
                var cmdComando = CriarComandoSql(
                    "SELECT count(ID_FERIADO_CALENDARIO) as total FROM FERIADO_CALENDARIO  WHERE DATA='" +
                    data.ToString( "yyyy/MM/dd" ) +
                    "' AND ID_CALENDARIO=2" );
                using ( var reader = cmdComando.ExecuteReader() )
                {
                    linhas = new List<Dictionary<string, string>>();
                    while ( reader.Read() )
                    {
                        var linha = new Dictionary<string, string>();

                        for ( var i = 0; i < reader.FieldCount; i++ )
                        {
                            var nomeDaColuna = reader.GetName( i );
                            var valorDaColuna = reader.IsDBNull( i ) ? null : reader.GetString( i );
                            linha.Add( nomeDaColuna, valorDaColuna );
                        }

                        linhas.Add( linha );
                    }
                }
            }
            finally
            {
                FecharConexao( _connexao );
            }

            return linhas;
        }


        public List<Dictionary<string, string>> RetornaMarcacao(string idFuncionario, DateTime diaValidar)
        {
            List<Dictionary<string, string>> linhas;
            try
            {
                var cmdComando = CriarComandoSql(
                    "SELECT   b.ID_CARGO, a.HORA,a.DATA from MARCACAO a, FUNCIONARIO b WHERE a.DATA='" +
                    diaValidar.ToString( "yyyy/MM/dd" ) + "' AND a.ID_FUNCIONARIO=" + idFuncionario +
                    " AND a.ID_FUNCIONARIO=b.ID_FUNCIONARIO ORDER BY HORA ASC" );

                using ( var reader = cmdComando.ExecuteReader() )
                {
                    linhas = new List<Dictionary<string, string>>();
                    while ( reader.Read() )
                    {
                        var linha = new Dictionary<string, string>();

                        for ( var i = 0; i < reader.FieldCount; i++ )
                        {
                            var nomeDaColuna = reader.GetName( i );
                            var valorDaColuna = reader.IsDBNull( i ) ? null : reader.GetString( i );
                            linha.Add( nomeDaColuna, valorDaColuna );
                        }

                        linhas.Add( linha );
                    }
                }
            }
            finally
            {
                FecharConexao( _connexao );
            }

            return linhas;
        }

        public List<Dictionary<string, string>> RetornaIdFuncionario(string nomeFuncionario)
        {
            List<Dictionary<string, string>> linhas;
            try
            {
                var cmdComando = CriarComandoSql(
                    "SELECT   ID_FUNCIONARIO from FUNCIONARIO  WHERE NOME LIKE'%" + nomeFuncionario + "%'" );

                using ( var reader = cmdComando.ExecuteReader() )
                {
                    linhas = new List<Dictionary<string, string>>();
                    while ( reader.Read() )
                    {
                        var linha = new Dictionary<string, string>();

                        for ( var i = 0; i < reader.FieldCount; i++ )
                        {
                            var nomeDaColuna = reader.GetName( i );
                            var valorDaColuna = reader.IsDBNull( i ) ? null : reader.GetString( i );
                            linha.Add( nomeDaColuna, valorDaColuna );
                        }

                        linhas.Add( linha );
                    }
                }
            }
            finally
            {
                FecharConexao( _connexao );
            }

            return linhas;
        }

        public List<Dictionary<string, string>> RetornaCrachaFuncionario(string nomeFuncionario)
        {
            List<Dictionary<string, string>> linhas;
            try
            {
                var cmdComando = CriarComandoSql(
                    "SELECT   MATRICULA from FUNCIONARIO  WHERE NOME LIKE'%" + nomeFuncionario + "%'" );

                using ( var reader = cmdComando.ExecuteReader() )
                {
                    linhas = new List<Dictionary<string, string>>();
                    while ( reader.Read() )
                    {
                        var linha = new Dictionary<string, string>();

                        for ( var i = 0; i < reader.FieldCount; i++ )
                        {
                            var nomeDaColuna = reader.GetName( i );
                            var valorDaColuna = reader.IsDBNull( i ) ? null : reader.GetString( i );
                            linha.Add( nomeDaColuna, valorDaColuna );
                        }

                        linhas.Add( linha );
                    }
                }
            }
            finally
            {
                FecharConexao( _connexao );
            }

            return linhas;
        }

        public List<Dictionary<string, string>> RetornaDadosMarcacao(string dataMarcacao, string idFuncionario)
        {
            List<Dictionary<string, string>> linhas;
            try
            {
                var cmdComando = CriarComandoSql(
                    "SELECT * from MARCACAO  WHERE DATA='" + Convert.ToDateTime( dataMarcacao ).ToString( "yyyy/MM/dd" ) +
                    "' AND ID_FUNCIONARIO=" + idFuncionario + "" );

                using ( var reader = cmdComando.ExecuteReader() )
                {
                    linhas = new List<Dictionary<string, string>>();
                    while ( reader.Read() )
                    {
                        var linha = new Dictionary<string, string>();

                        for ( var i = 0; i < reader.FieldCount; i++ )
                        {
                            var nomeDaColuna = reader.GetName( i );
                            var valorDaColuna = reader.IsDBNull( i ) ? null : reader.GetString( i );
                            linha.Add( nomeDaColuna, valorDaColuna );
                        }

                        linhas.Add( linha );
                    }
                }
            }
            finally
            {
                FecharConexao( _connexao );
            }

            return linhas;
        }

        public void InserirMarcacao(string idFuncionarioFireBird, string idJustificativaFireBird, string dataPendencia,
            string horaPendencia)
        {
            var ultimosDados = RetornaUltimaId();
            var pisFuncionario = RetornaPis( idFuncionarioFireBird );

            try
            {
                var cmdComando = CriarComandoSql(
                    "INSERT INTO MARCACAO(ID_FUNCIONARIO, ID_JUSTIFICATIVA, NUMERO_REP, PIS, SEQUENCIAL, DATA, HORA, TIPO_REGISTRO, TIPO_MARCACAO, IDENTIFICACAO) VALUES(" +
                    idFuncionarioFireBird + ", " +
                    idJustificativaFireBird + ",0,'" + pisFuncionario [0] ["PIS"] + "'," +
                    (Convert.ToInt32( ultimosDados [0] ["SEQUENCIAL"] ) + 1) + ", '" +
                    Convert.ToDateTime( dataPendencia ).ToString( "yyyy/MM/dd" ) + "','" + horaPendencia + "', 'I','', '" +
                    pisFuncionario [0] ["PIS"] + "'); " );
                cmdComando.ExecuteNonQuery();

            }
            finally
            {
                FecharConexao( _connexao );
            }
        }

        public List<Dictionary<string, string>> RetornaUltimaId()
        {
            List<Dictionary<string, string>> linhas;
            try
            {
                var cmdComando = CriarComandoSql(
                    "SELECT FIRST 1 ID_MARCACAO,SEQUENCIAL FROM MARCACAO ORDER BY SEQUENCIAL DESC;" );

                using ( var reader = cmdComando.ExecuteReader() )
                {
                    linhas = new List<Dictionary<string, string>>();
                    while ( reader.Read() )
                    {
                        var linha = new Dictionary<string, string>();

                        for ( var i = 0; i < reader.FieldCount; i++ )
                        {
                            var nomeDaColuna = reader.GetName( i );
                            var valorDaColuna = reader.IsDBNull( i ) ? null : reader.GetString( i );
                            linha.Add( nomeDaColuna, valorDaColuna );
                        }

                        linhas.Add( linha );
                    }
                }
            }
            finally
            {
                FecharConexao( _connexao );
            }

            return linhas;
        }

        public List<Dictionary<string, string>> RetornaPis(string idFuncionario)
        {
            List<Dictionary<string, string>> linhas;
            try
            {
                var cmdComando = CriarComandoSql(
                    "SELECT PIS FROM FUNCIONARIO WHERE ID_FUNCIONARIO=" + idFuncionario + ";" );

                using ( var reader = cmdComando.ExecuteReader() )
                {
                    linhas = new List<Dictionary<string, string>>();
                    while ( reader.Read() )
                    {
                        var linha = new Dictionary<string, string>();

                        for ( var i = 0; i < reader.FieldCount; i++ )
                        {
                            var nomeDaColuna = reader.GetName( i );
                            var valorDaColuna = reader.IsDBNull( i ) ? null : reader.GetString( i );
                            linha.Add( nomeDaColuna, valorDaColuna );
                        }

                        linhas.Add( linha );
                    }
                }
            }
            finally
            {
                FecharConexao( _connexao );
            }

            return linhas;
        }

        public List<Dictionary<string, string>> RecuperaJustificativas()
        {
            List<Dictionary<string, string>> linhas;

            var cmdComando =
                CriarComandoSql( "SELECT ID_JUSTIFICATIVA, DESCRICAO FROM JUSTIFICATIVA ORDER BY DESCRICAO ASC" );
            try
            {
                using ( var reader = cmdComando.ExecuteReader() )
                {
                    linhas = new List<Dictionary<string, string>>();
                    while ( reader.Read() )
                    {
                        var linha = new Dictionary<string, string>();

                        for ( var i = 0; i < reader.FieldCount; i++ )
                        {
                            var nomeDaColuna = reader.GetName( i );
                            var valorDaColuna = reader.IsDBNull( i ) ? null : reader.GetString( i );
                            linha.Add( nomeDaColuna, valorDaColuna );
                        }

                        linhas.Add( linha );
                    }
                }
            }
            finally
            {
                FecharConexao( _connexao );
            }


            return linhas;
        }

        public List<Dictionary<string, string>> RecuperaJustificativasFuncioanrio(string idJustificativa)
        {
            List<Dictionary<string, string>> linhas;

            var cmdComando = CriarComandoSql( "SELECT DESCRICAO FROM JUSTIFICATIVA WHERE  ID_JUSTIFICATIVA=" +
                                             idJustificativa + "" );
            try
            {
                using ( var reader = cmdComando.ExecuteReader() )
                {
                    linhas = new List<Dictionary<string, string>>();
                    while ( reader.Read() )
                    {
                        var linha = new Dictionary<string, string>();

                        for ( var i = 0; i < reader.FieldCount; i++ )
                        {
                            var nomeDaColuna = reader.GetName( i );
                            var valorDaColuna = reader.IsDBNull( i ) ? null : reader.GetString( i );
                            linha.Add( nomeDaColuna, valorDaColuna );
                        }

                        linhas.Add( linha );
                    }
                }
            }
            finally
            {
                FecharConexao( _connexao );
            }


            return linhas;
        }

        #endregion
    }
}