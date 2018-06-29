using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using PortalSicoobDivicred.Repositorios;

namespace PortalSicoobDivicred.Aplicacao
{
    public class QueryMysqlCIM
    {
        private readonly Conexao ConexaoMysql;

        public QueryMysqlCIM()
        {
            ConexaoMysql = new Conexao();
        }


        public string RecuperaFuncao(string Login)
        {
            
            string QueryRecuperaFuncaoUsuario = "SELECT funcao from funcionarios where Login='" + Login + "'";
            var funcao = ConexaoMysql.ExecutaComandoComRetorno(QueryRecuperaFuncaoUsuario);
            return funcao[0]["funcao"];
        }

        public string RecuperaMetaCim(string funcao)
        {
            string QueryRecuperaMetaUsuario = "SELECT cimmeta from funcoes where id='" + funcao + "'";
            var meta = ConexaoMysql.ExecutaComandoComRetorno(QueryRecuperaMetaUsuario);
            return meta[0]["cimmeta"];
        }

        

        public string BuscaSaldoAtual(string Login)
        {
            string QueryConsultaValorAtual =
                "select pontuacaoatual from cimpontuacao where Login='" + Login + "'";

            var pontuacaoAtual = ConexaoMysql.ExecutaComandoComRetorno(QueryConsultaValorAtual);
            if (pontuacaoAtual.Count == 0)
            {
                var QueryIncluiPontuacao = "INSERT INTO cimpontuacao (pontuacaoatual,login) values ('0','" + Login + "')";
                ConexaoMysql.ExecutaComando(QueryIncluiPontuacao);
               
                string QueryConsultaValorPrimeiraVez =
                    "select pontuacaoatual from cimpontuacao where Login='" + Login + "'";

                pontuacaoAtual = ConexaoMysql.ExecutaComandoComRetorno(QueryConsultaValorPrimeiraVez);

            }

            return pontuacaoAtual[0]["pontuacaoatual"];
        }

        public string Gestor(string login)
        {
            string QueryGestor = "SELECT gestor from funcionarios where login='" + login + "'";

            var gestor = ConexaoMysql.ExecutaComandoComRetorno(QueryGestor);

            return gestor[0]["gestor"];
        }

        public List<SelectListItem> RetornaProdutos()
        {
            var Produtos = new List<SelectListItem>();

            const string QueryRetornaProdutos = "SELECT id,descricao FROM pgdprodutos order by descricao";

            var Dados = ConexaoMysql.ExecutaComandoComRetorno(QueryRetornaProdutos);
            foreach (var row in Dados)
                Produtos.Add(new SelectListItem
                {
                    Value = row["id"],
                    Text = row["descricao"]
                });

            return Produtos;
        }

       

        public List<Dictionary<string, string>> retornaDadosProdutos(int id)
        {

            string QueryRetornaDadosProdutos = "SELECT peso,valorminimo from pgdprodutos where id='" + id + "'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(QueryRetornaDadosProdutos);
            return Dados;
        }


        public void InsereProducao(string cpf, int produtos, string observacao, DateTime data, string Login, string valor, string valorponto)
        {

            string QueryInsereProducao = "INSERT INTO cimproducao (cpf,produto,observacao,datacontratacao,excluido,Login,valor,valorponto) values ('" + cpf + "','" + produtos + "','" + observacao + "','" + Convert.ToDateTime(data).ToString("yyyy-MM-dd") + "','N','" + Login + "','" + valor.ToString().Replace(",", ".") + "','" + valorponto.ToString().Replace(".", "").Replace(",", ".") + "') ";
            ConexaoMysql.ExecutaComando(QueryInsereProducao);

        }

        public string ExisteRegistro(string Login)
        {
            string QueryExiste = "SELECT count(*) from cimpontuacao where Login='" + Login + "'";

            var id = ConexaoMysql.ExecutaComandoComRetorno(QueryExiste);

            return id[0]["count(*)"];
        }



        
        public void IncluirPontucao(string Login, double ponto)
        {
            string existe = ExisteRegistro(Login);
                string auxSaldoAtual = BuscaSaldoAtual(Login);
            double saldoatual = Convert.ToDouble(auxSaldoAtual);


            double valorAtual = saldoatual + ponto;
            var QueryAtualizaPontuacao = "update cimpontuacao set pontuacaoatual =" + valorAtual.ToString("N2").Replace(".", "").Replace(",", ".") + " where Login='" + Login + "'";
            ConexaoMysql.ExecutaComando(QueryAtualizaPontuacao);
            
        }

        public List<Dictionary<string, string>> BuscaDadosProducao(int idProducao)
        {
            string QueryConsultaValorAtual =
                "select Login,valorponto from cimproducao where id=" + idProducao;

            var idDados = ConexaoMysql.ExecutaComandoComRetorno(QueryConsultaValorAtual);

            return idDados;
        }

        public void ExcluirRegistro(int id)
        {
            var QueryExcluiProducao = "UPDATE cimproducao SET excluido='S' where id='" + id + "'";
            ConexaoMysql.ExecutaComandoComRetorno(QueryExcluiProducao);
        }

        public void AtualizarRegistroExclusao(string Login, double valor)
        {

            string auxSaldoAtual = BuscaSaldoAtual(Login);
            double saldoatual = Convert.ToDouble(auxSaldoAtual);

            double saldo = saldoatual - valor;

            var QueryExcluiProducao = "UPDATE cimpontuacao set pontuacaoatual =" + saldo.ToString().Replace(",", ".") +
                                      " where Login='" + Login + "'";
            ConexaoMysql.ExecutaComandoComRetorno(QueryExcluiProducao);
        }

        public List<Dictionary<string, string>> RecuperaDadosProducao(string UsuarioSistema)
        {
            var Query =
                "SELECT * FROM cimproducao WHERE Login='" + UsuarioSistema + "' and excluido='N'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);


            return Dados;
        }

        public string RecuperaProduto(int idProduto)
        {
            var Query =
                "select descricao from pgdprodutos where id='" + idProduto + "'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);


            return Dados[0]["descricao"];
        }

        public List<Dictionary<string, string>> RecuperaSubordinadosGestor(string login)
        {

            string QueryRecuperaSetorUsuario = "SELECT idsetor from funcionarios where login='" + login + "'";
            var idsetor = ConexaoMysql.ExecutaComandoComRetorno(QueryRecuperaSetorUsuario);

            var Query = "select a.nome,b.pontuacaoatual,a.funcao from funcionarios as a inner join cimpontuacao as b on a.login=b.login " +
                       "where a.idsetor='" + idsetor[0]["idsetor"] + "'";
            var row = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return row;
        }

    }
}