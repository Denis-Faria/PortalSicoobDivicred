using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Nest;
using PortalSicoobDivicred.Aplicacao;
using PortalSicoobDivicred.Models;

namespace PortalSicoobDivicred.Controllers
{
    public class ParametrosController : Controller
    {
        public ActionResult Parametros(string MensagemValidacao, string Erro)
        {

            TempData["MensagemValidacao"] = MensagemValidacao;
            TempData["Erro"] = Erro;
            var insereDados = new QueryMysql();
            var Logado = insereDados.UsuarioLogado();
            if (Logado)
            {

                var cookie = Request.Cookies.Get("CookieFarm");
                if (cookie != null)
                {
                    var login = Criptografa.Descriptografar(cookie.Value);
                    var dadosUsuarioBanco = insereDados.RecuperaDadosUsuarios(login);
                    var validacoes = new ValidacoesIniciais();

                    validacoes.AlertasUsuario(this, dadosUsuarioBanco[0]["id"]);
                    validacoes.Permissoes(this, dadosUsuarioBanco);
                    validacoes.DadosNavBar(this, dadosUsuarioBanco);
                }

                return View("Parametros");
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

        }
        public ActionResult Funcionario()
        {
            var verificaDados = new QueryMysqlParametros();
            var logado = verificaDados.UsuarioLogado();
            if (logado)
            {
               
                var dadosGrupos = new Parametros();
                var dadosTablelaGrupo = verificaDados.RetornaGrupos();
                dadosGrupos.DescricaoGrupo = dadosTablelaGrupo;
                

                return PartialView("ParametrosFuncionario",dadosGrupos);
            }
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult Funcionario(Funcao dadosCadastro, FormCollection dados)
        {
            var verificaDados = new QueryMysqlParametros();
            var logado = verificaDados.UsuarioLogado();

            /*var cookie = Request.Cookies.Get("CookieFarm");

            if (cookie != null)
            {
                var login = Criptografa.Descriptografar(cookie.Value);
            }*/

            if (logado)
            {
                var dadosGrupos = verificaDados.RetornaGrupos();
                return PartialView("ParametrosFuncionario", dadosGrupos);
            }

            return RedirectToAction("Login", "Login");
        }


        public ActionResult Cadastro()
        {

            var consultaDados = new QueryMysqlCim();
            var verificaDadosLogin = new QueryMysql();
            var dadosPGD = new Pgd();
            var dadosTabelaPGD = consultaDados.RetornaProdutos();
            dadosPGD.DescricaoProduto = dadosTabelaPGD;
            dadosTabelaPGD = verificaDadosLogin.RetornaFuncionario();
            dadosPGD.NomeFuncionario = dadosTabelaPGD;


            return PartialView("Cadastro", dadosPGD);
        }


        [HttpPost]
        public ActionResult SalvarUsuario(Parametros dados, FormCollection receberForm)
        {
            var insereDados = new QueryMysqlParametros();

            var cookie = Request.Cookies.Get("CookieFarm");
            if (cookie != null)
            {

                //var login = Criptografa.Descriptografar(cookie.Value);
                insereDados.InsereUsuario(dados.NomeFuncionario,dados.Pa,dados.dataAdmissao,dados.CpfFuncionario,dados.RgFuncionario,
                    dados.PisFuncionario,dados.Estagiario,dados.LoginFuncionario,"123",dados.Email,dados.idDescricaoGrupo,dados.Gestor, dados.Matricula);
                
                /*
                var login = Criptografa.Descriptografar(cookie.Value);

                var valor = receberForm["valor"];
                var dadosProdutos = insereDados.RetornaDadosProdutos(dados.IdProduto);
                var peso = dadosProdutos[0]["peso"];
                var valorminimo = dadosProdutos[0]["valorminimo"];
                double valorponto;

                if (valorminimo != "1")
                {
                    var teste = Convert.ToDouble(valor.Replace(".", ","));
                    valorponto = teste / Convert.ToDouble(valorminimo) *
                                 Convert.ToDouble(peso);
                }
                else
                {
                    valorponto = Convert.ToDouble(peso);
                }

                insereDados.InsereProducao(dados.Cpf, dados.IdProduto, dados.Observacao, dados.Datacontratacao, login,
                    valor,
                    valorponto.ToString("N2"));

                insereDados.IncluirPontucao(login, valorponto);


                var saldoAtual = insereDados.BuscaSaldoAtual(login);

                TempData["saldo"] = saldoAtual;*/
            }


            return RedirectToAction("Parametros", "Parametros", new { Mensagem = "Usuário cadastrada com sucesso !" });
        }

        [HttpPost]
        public ActionResult BuscarFuncionario(string DescricaoFuncao)
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Funcoes = VerificaDados.BuscaFuncao(DescricaoFuncao);
                for (var i = 0; i < Funcoes.Count; i++)
                {
                    TempData["Id" + i] = Funcoes[i]["id"];
                    TempData["Descricao" + i] = Funcoes[i]["descricao"];
                }

                TempData["TotalResultado"] = Funcoes.Count;
                TempData["Editar"] = "EditarFuncao";
                return PartialView("ResultadoPesquisa");
            }

            return RedirectToAction("Login", "Login");
        }

    }
}