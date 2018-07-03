using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using PortalSicoobDivicred.Repositorios;

namespace PortalSicoobDivicred.Aplicacao
{
    public class QueryMysqlCim
    {
        private readonly Conexao _conexaoMysql;

        public QueryMysqlCim()
        {
            _conexaoMysql = new Conexao();
        }


        public string RecuperaFuncao(string login)
        {

            string queryRecuperaFuncaoUsuario = "SELECT funcao from funcionarios where Login='" + login + "'";
            var funcao = _conexaoMysql.ExecutaComandoComRetorno( queryRecuperaFuncaoUsuario );
            return funcao [0] ["funcao"];
        }

        public string RecuperaMetaCim(string funcao)
        {
            string queryRecuperaMetaUsuario = "SELECT cimmeta from funcoes where id='" + funcao + "'";
            var meta = _conexaoMysql.ExecutaComandoComRetorno( queryRecuperaMetaUsuario );
            return meta [0] ["cimmeta"];
        }



        public string BuscaSaldoAtual(string login)
        {
            string queryConsultaValorAtual =
                "select pontuacaoatual from cimpontuacao where Login='" + login + "'";

            var pontuacaoAtual = _conexaoMysql.ExecutaComandoComRetorno( queryConsultaValorAtual );
            if ( pontuacaoAtual.Count == 0 )
            {
                var queryIncluiPontuacao = "INSERT INTO cimpontuacao (pontuacaoatual,login) values ('0','" + login + "')";
                _conexaoMysql.ExecutaComando( queryIncluiPontuacao );

                string queryConsultaValorPrimeiraVez =
                    "select pontuacaoatual from cimpontuacao where Login='" + login + "'";

                pontuacaoAtual = _conexaoMysql.ExecutaComandoComRetorno( queryConsultaValorPrimeiraVez );

            }

            return pontuacaoAtual [0] ["pontuacaoatual"];
        }

        public string Gestor(string login)
        {
            string queryGestor = "SELECT gestor from funcionarios where login='" + login + "'";

            var gestor = _conexaoMysql.ExecutaComandoComRetorno( queryGestor );

            return gestor [0] ["gestor"];
        }

        public List<SelectListItem> RetornaProdutos()
        {
            var produtos = new List<SelectListItem>();

            const string queryRetornaProdutos = "SELECT id,descricao FROM pgdprodutos order by descricao";

            var dados = _conexaoMysql.ExecutaComandoComRetorno( queryRetornaProdutos );
            foreach ( var row in dados )
                produtos.Add( new SelectListItem
                {
                    Value = row ["id"],
                    Text = row ["descricao"]
                } );

            return produtos;
        }



        public List<Dictionary<string, string>> RetornaDadosProdutos(int id)
        {

            string queryRetornaDadosProdutos = "SELECT peso,valorminimo from pgdprodutos where id='" + id + "'";
            var dados = _conexaoMysql.ExecutaComandoComRetorno( queryRetornaDadosProdutos );
            return dados;
        }


        public void InsereProducao(string cpf, int produtos, string observacao, DateTime data, string login, string valor, string valorponto)
        {

            string queryInsereProducao = "INSERT INTO cimproducao (cpf,produto,observacao,datacontratacao,excluido,Login,valor,valorponto) values ('" + cpf + "','" + produtos + "','" + observacao + "','" + Convert.ToDateTime( data ).ToString( "yyyy-MM-dd" ) + "','N','" + login + "','" + valor.Replace( ",", "." ) + "','" + valorponto.Replace( ".", "" ).Replace( ",", "." ) + "') ";
            _conexaoMysql.ExecutaComando( queryInsereProducao );

        }

        public string ExisteRegistro(string login)
        {
            string queryExiste = "SELECT count(*) from cimpontuacao where Login='" + login + "'";

            var id = _conexaoMysql.ExecutaComandoComRetorno( queryExiste );

            return id [0] ["count(*)"];
        }




        public void IncluirPontucao(string login, double ponto)
        {
            string auxSaldoAtual = BuscaSaldoAtual( login );
            double saldoatual = Convert.ToDouble( auxSaldoAtual );


            double valorAtual = saldoatual + ponto;
            var queryAtualizaPontuacao = "update cimpontuacao set pontuacaoatual =" + valorAtual.ToString( "N2" ).Replace( ".", "" ).Replace( ",", "." ) + " where Login='" + login + "'";
            _conexaoMysql.ExecutaComando( queryAtualizaPontuacao );

        }

        public List<Dictionary<string, string>> BuscaDadosProducao(int idProducao)
        {
            string queryConsultaValorAtual =
                "select Login,valorponto from cimproducao where id=" + idProducao;

            var idDados = _conexaoMysql.ExecutaComandoComRetorno( queryConsultaValorAtual );

            return idDados;
        }

        public void ExcluirRegistro(int id)
        {
            var queryExcluiProducao = "UPDATE cimproducao SET excluido='S' where id='" + id + "'";
            _conexaoMysql.ExecutaComandoComRetorno( queryExcluiProducao );
        }

        public void AtualizarRegistroExclusao(string login, double valor)
        {

            string auxSaldoAtual = BuscaSaldoAtual( login );
            double saldoatual = Convert.ToDouble( auxSaldoAtual );

            double saldo = saldoatual - valor;

            var queryExcluiProducao = "UPDATE cimpontuacao set pontuacaoatual =" + saldo.ToString().Replace( ",", "." ) +
                                      " where Login='" + login + "'";
            _conexaoMysql.ExecutaComandoComRetorno( queryExcluiProducao );
        }

        public List<Dictionary<string, string>> RecuperaDadosProducao(string usuarioSistema)
        {
            var query =
                "SELECT * FROM cimproducao WHERE Login='" + usuarioSistema + "' and excluido='N'";
            var dados = _conexaoMysql.ExecutaComandoComRetorno( query );


            return dados;
        }

        public string RecuperaProduto(int idProduto)
        {
            var query =
                "select descricao from pgdprodutos where id='" + idProduto + "'";
            var dados = _conexaoMysql.ExecutaComandoComRetorno( query );


            return dados [0] ["descricao"];
        }

        public List<Dictionary<string, string>> RecuperaSubordinadosGestor(string login)
        {

            string queryRecuperaSetorUsuario = "SELECT idsetor from funcionarios where login='" + login + "'";
            var idsetor = _conexaoMysql.ExecutaComandoComRetorno( queryRecuperaSetorUsuario );

            var query = "select a.nome,b.pontuacaoatual,a.funcao from funcionarios as a inner join cimpontuacao as b on a.login=b.login " +
                       "where a.idsetor='" + idsetor [0] ["idsetor"] + "'";
            var row = _conexaoMysql.ExecutaComandoComRetorno( query );
            return row;
        }

    }
}