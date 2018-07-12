using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using PortalSicoobDivicred.Aplicacao;

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
            var resultado = 0;
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
            long id = 0;
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


        public DataTable ComandoArquivo(string login)
        {

                AbrirConexao();
                var comando =
                    new MySqlCommand(
                        "SELECT * FROM documentospessoaisfuncionarios WHERE idfuncionario=(SELECT id FROM funcionarios where login='" +
                        login + "'); ", _conexao);
                var dados = new MySqlDataAdapter(comando);
                var tabela = new DataTable();
                dados.Fill(tabela);
                FecharConexao();
                return tabela;
            
        }

        public DataTable ComandoArquivoWebDesk(string idInteracao)
        {

                AbrirConexao();
                var comando =
                    new MySqlCommand(
                        "SELECT * FROM webdeskanexos WHERE idinteracao='" + idInteracao + "'; ", _conexao);
                var dados = new MySqlDataAdapter(comando);
                var tabela = new DataTable();
                dados.Fill(tabela);
                FecharConexao();
                return tabela;

        }

        public List<Dictionary<string, string>> ExecutaComandoComRetorno(string comandoSql)
        {
            List<Dictionary<string, string>> linhas = null;

            if (string.IsNullOrEmpty(comandoSql))
                throw new ArgumentException("O comandoSQL não pode ser nulo ou vazio");
            try
            {
                AbrirConexao();
                var cmdComando = CriarComando(comandoSql);

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

        public string ExecutaComandoComRetornoId(string comandoSql)
        {
            long id = 0;

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

        public void ExecutaComandoArquivo(string comandoSql, byte[] imagem)
        {
            if (string.IsNullOrEmpty(comandoSql))
                throw new ArgumentException("O comandoSQL não pode ser nulo ou vazio");
            try
            {
                AbrirConexao();
                var cmdComando = new MySqlCommand(comandoSql, _conexao);
                cmdComando.Parameters.Add("@image", MySqlDbType.Blob).Value = imagem;
                cmdComando.ExecuteNonQuery();
            }
            finally
            {
                FecharConexao();
            }
        }

        public List<Dictionary<string, string>> ExecutaComandoComRetornoPortal(string comandoSql)
        {
            List<Dictionary<string, string>> linhas = null;

            if (string.IsNullOrEmpty(comandoSql))
                throw new ArgumentException("O comandoSQL não pode ser nulo ou vazio");
            try
            {
                AbrirConexaoPortal();
                var cmdComando = CriarComando(comandoSql);

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

        private MySqlCommand CriarComando(string comandoSql)
        {
            var cmdComando = _conexao.CreateCommand();
            cmdComando.CommandText = comandoSql;
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