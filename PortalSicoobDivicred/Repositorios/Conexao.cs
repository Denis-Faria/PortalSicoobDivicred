using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

namespace PortalSicoobDivicred.Repositorios
{
    public class Conexao : IDisposable
    {
        private MySqlConnection _conexao;

        public void Dispose()
        {
            if (_conexao == null) return;

            _conexao.Dispose();
            _conexao = null;
        }

        public int ExecutaComando(string comandoSql)
        {
            int resultado;
            if (string.IsNullOrEmpty(comandoSql))
                throw new ArgumentException("O comandoSQL não pode ser nulo ou vazio");
            try
            {
                AbrirConexao();
                var cmdComando = CriarComando(comandoSql);
                resultado = cmdComando.ExecuteNonQuery();
            }
            finally
            {
                FecharConexao();
            }

            return resultado;
        }

        public string ExecutaComandoCandidato(string comandoSql)
        {
            long id;
            if (string.IsNullOrEmpty(comandoSql))
                throw new ArgumentException("O comandoSQL não pode ser nulo ou vazio");
            try
            {
                AbrirConexao();
                var cmdComando = CriarComando(comandoSql);
                cmdComando.ExecuteNonQuery();
                id = cmdComando.LastInsertedId;
            }
            finally
            {
                FecharConexao();
            }

            return id.ToString();
        }


        public DataTable ComandoArquivo(string Login)
        {
            AbrirConexao();
            var comando =
                new MySqlCommand(
                    "SELECT * FROM documentospessoaisfuncionarios WHERE idfuncionario=(SELECT id FROM funcionarios where login='" +
                    Login + "'); ", _conexao);
            var Dados = new MySqlDataAdapter(comando);
            var Tabela = new DataTable();
            Dados.Fill(Tabela);
            FecharConexao();
            return Tabela;
        }

        public DataTable ComandoArquivoWebDesk(string IdInteracao)
        {
            AbrirConexao();
            var comando =
                new MySqlCommand(
                    "SELECT * FROM webdeskanexos WHERE idinteracao='" + IdInteracao + "'; ", _conexao);
            var Dados = new MySqlDataAdapter(comando);
            var Tabela = new DataTable();
            Dados.Fill(Tabela);
            FecharConexao();
            return Tabela;
        }

        public List<Dictionary<string, string>> ExecutaComandoComRetorno(string comandoSQL)
        {
            List<Dictionary<string, string>> linhas = null;

            if (string.IsNullOrEmpty(comandoSQL))
                throw new ArgumentException("O comandoSQL não pode ser nulo ou vazio");
            try
            {
                AbrirConexao();
                var cmdComando = CriarComando(comandoSQL);

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
                FecharConexao();
            }

            return linhas;
        }

        public string ExecutaComandoComRetornoId(string comandoSQL)
        {
            long Id;

            if (string.IsNullOrEmpty(comandoSQL))
                throw new ArgumentException("O comandoSQL não pode ser nulo ou vazio");
            try
            {
                AbrirConexao();
                var cmdComando = CriarComando(comandoSQL);
                cmdComando.ExecuteNonQuery();
                Id = cmdComando.LastInsertedId;
            }
            finally
            {
                FecharConexao();
            }

            return Id.ToString();
        }

        public void ExecutaComandoArquivo(string comandoSQL, byte[] Imagem)
        {
            if (string.IsNullOrEmpty(comandoSQL))
                throw new ArgumentException("O comandoSQL não pode ser nulo ou vazio");
            try
            {
                AbrirConexao();
                var cmdComando = new MySqlCommand(comandoSQL, _conexao);
                cmdComando.Parameters.Add("@image", MySqlDbType.Blob).Value = Imagem;
                cmdComando.ExecuteNonQuery();
            }
            finally
            {
                FecharConexao();
            }
        }

        public List<Dictionary<string, string>> ExecutaComandoComRetornoPortal(string comandoSQL)
        {
            List<Dictionary<string, string>> linhas = null;

            if (string.IsNullOrEmpty(comandoSQL))
                throw new ArgumentException("O comandoSQL não pode ser nulo ou vazio");
            try
            {
                AbrirConexaoPortal();
                var cmdComando = CriarComando(comandoSQL);

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
                FecharConexao();
            }

            return linhas;
        }

        private MySqlCommand CriarComando(string comandoSQL)
        {
            var cmdComando = _conexao.CreateCommand();
            cmdComando.CommandText = comandoSQL;
            return cmdComando;
        }

        private void AbrirConexao()
        {
            var conexaoString = ConfigurationManager.ConnectionStrings["portalinterno"].ConnectionString;
            _conexao = new MySqlConnection(conexaoString);

            if (_conexao.State == ConnectionState.Open) return;

            _conexao.Open();
        }

        private void AbrirConexaoPortal()
        {
            var conexaoString = ConfigurationManager.ConnectionStrings["portaldetalentos"].ConnectionString;
            _conexao = new MySqlConnection(conexaoString);

            if (_conexao.State == ConnectionState.Open) return;

            _conexao.Open();
        }

        private void FecharConexao()
        {
            if (_conexao.State == ConnectionState.Open)
                _conexao.Close();
        }
    }
}