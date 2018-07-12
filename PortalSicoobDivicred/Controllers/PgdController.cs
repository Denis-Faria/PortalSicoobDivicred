using System;
using System.Web.Mvc;
using PortalSicoobDivicred.Aplicacao;
using PortalSicoobDivicred.Models;

namespace PortalSicoobDivicred.Controllers
{
    public class PgdController : Controller
    {
        // GET: Pgd
        public ActionResult Pgd(string mensagem)
        {
            var verificaDados = new QueryMysqlCim();
            var verificaDadosLogin = new QueryMysql();
            var logado = verificaDadosLogin.UsuarioLogado();
            if (logado)
            {
                var cookie = Request.Cookies.Get("CookieFarm");
                if (cookie != null)
                {
                    var login = Criptografa.Descriptografar(cookie.Value);

                    var funcao = verificaDados.RecuperaFuncao(login);
                    TempData["meta"] = verificaDados.RecuperaMetaCim(funcao);

                    var saldoAtual = verificaDados.BuscaSaldoAtual(login);

                    var dadosUsuarioBanco = verificaDadosLogin.RecuperaDadosUsuarios(login);

                    var validacoes = new ValidacoesIniciais();

                    validacoes.AlertasUsuario( this, dadosUsuarioBanco[0]["id"] );
                    validacoes.Permissoes( this, dadosUsuarioBanco );
                    validacoes.DadosNavBar( this, dadosUsuarioBanco );

                    TempData["EmailLateral"] = dadosUsuarioBanco[0]["email"];

                    TempData["saldo"] = saldoAtual;
                    var gestor = verificaDados.Gestor(login);
                    TempData["Gestor"] = gestor;
                }

                TempData["Mensagem"] = mensagem;
                return View("Pgd");
            }

            return RedirectToAction("Login", "Login");
        }


        public ActionResult Cadastro()
        {
            var consultaDados = new QueryMysqlCim();
            var verificaDadosLogin = new QueryMysql();
            var dadosPgd = new Pgd();
            var dadosTabelaPgd = consultaDados.RetornaProdutos();
            dadosPgd.DescricaoProduto = dadosTabelaPgd;
            dadosTabelaPgd = verificaDadosLogin.RetornaFuncionario();
            dadosPgd.NomeFuncionario = dadosTabelaPgd;


            return PartialView("Cadastro", dadosPgd);
        }

        [HttpPost]
        public ActionResult Salvarproducao(Pgd dados, FormCollection receberForm)
        {
            var insereDados = new QueryMysqlCim();

            var cookie = Request.Cookies.Get("CookieFarm");
            if (cookie != null)
            {
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

                TempData["saldo"] = saldoAtual;
            }


            return RedirectToAction("Pgd", "Pgd", new {Mensagem = "Produção cadastrada com sucesso !"});
        }

        public ActionResult ExcluirRegistro(int id)
        {
            var excluiRegistro = new QueryMysqlCim();
            var usuario = excluiRegistro.BuscaDadosProducao(id);
            excluiRegistro.ExcluirRegistro(id);
            excluiRegistro.AtualizarRegistroExclusao(usuario[0]["Login"], Convert.ToDouble(usuario[0]["valorponto"]));

            return RedirectToAction("Pgd", "Pgd", new {Mensagem = "Pontuação excluída com sucesso !"});
        }

        public ActionResult Extrato()
        {
            var verificaDados = new QueryMysqlCim();

            var cookie = Request.Cookies.Get("CookieFarm");
            if (cookie != null)
            {
                var login = Criptografa.Descriptografar(cookie.Value);


                var dadosTabelaFuncionario = verificaDados.RecuperaDadosProducao(login);

                TempData["TotalPonto"] = dadosTabelaFuncionario.Count;
                for (var i = 0; i < dadosTabelaFuncionario.Count; i++)
                {
                    TempData["id" + i] = dadosTabelaFuncionario[i]["id"];
                    TempData["cpf" + i] = dadosTabelaFuncionario[i]["cpf"];
                    TempData["DataContratacao" + i] = Convert.ToDateTime(dadosTabelaFuncionario[i]["datacontratacao"])
                        .ToString("dd/MM/yyyy");
                    TempData["Observacao" + i] = dadosTabelaFuncionario[i]["observacao"];
                    TempData["Produtos" + i] =
                        verificaDados.RecuperaProduto(Convert.ToInt32(dadosTabelaFuncionario[i]["produto"]));
                    TempData["valorponto" + i] = dadosTabelaFuncionario[i]["valorponto"];
                }
            }

            return PartialView("Extrato");
        }

        public ActionResult ExtratoGestor()
        {
            var verificaDadosGestor = new QueryMysqlCim();
            
            var cookie = Request.Cookies.Get("CookieFarm");
            if (cookie != null)
            {
                var login = Criptografa.Descriptografar(cookie.Value);
                var dadosSubordinados = verificaDadosGestor.RecuperaSubordinadosGestor(login);


                TempData["TotalFuncionarios"] = dadosSubordinados.Count;
                double pontuacaoTotal = 0;
                var metatotal = 0;
               int metaindIvidual;
                for (var i = 0; i < dadosSubordinados.Count; i++)
                {
                    TempData["meta" + i] = verificaDadosGestor.RecuperaMetaCim(dadosSubordinados[i]["funcao"]);
                    TempData["nome" + i] = dadosSubordinados[i]["nome"];
                    TempData["pontuacaoatual" + i] = dadosSubordinados[i]["pontuacaoatual"];
                    pontuacaoTotal = pontuacaoTotal + Convert.ToDouble(dadosSubordinados[i]["pontuacaoatual"]);
                    metaindIvidual = Convert.ToInt32(TempData["meta" + i]);
                    metatotal = metatotal + metaindIvidual;
                }

                TempData["pontuacaototal"] = pontuacaoTotal;
                TempData["metatotal"] = metatotal.ToString();
            }


            return PartialView("ExtratoGestor");
        }
    }
}