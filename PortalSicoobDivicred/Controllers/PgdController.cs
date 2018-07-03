using System;
using System.Web.Mvc;
using PortalSicoobDivicred.Aplicacao;
using PortalSicoobDivicred.Models;

namespace PortalSicoobDivicred.Controllers
{
    public class PgdController : Controller
    {
        // GET: Pgd
        public ActionResult Pgd(string Mensagem)
        {
            var verificaDados = new QueryMysqlCim();
            var verificaDadosLogin = new QueryMysql();
            var Logado = verificaDadosLogin.UsuarioLogado();
            if (Logado)
            {
                var Cookie = Request.Cookies.Get("CookieFarm");
                var Login = Criptografa.Descriptografar(Cookie.Value);
                var DadosUsuario = verificaDadosLogin.RecuperaDadosUsuarios(Login);



                if (verificaDadosLogin.PermissaoCurriculos(Login))
                    TempData["PermissaoCurriculo"] =
                        " ";
                else
                    TempData["PermissaoCurriculo"] = "display: none";

                if (DadosUsuario[0]["foto"] == null)
                    TempData["ImagemPerfil"] = "http://bulma.io/images/placeholders/128x128.png";
                else
                    TempData["ImagemPerfil"] = DadosUsuario[0]["foto"];

                if (verificaDadosLogin.PermissaoTesouraria(DadosUsuario[0]["login"]))
                    TempData["PermissaoTesouraria"] =
                        " ";
                else
                    TempData["PermissaoTesouraria"] = "display: none";


                var funcao = verificaDados.RecuperaFuncao(Login);
                @TempData["meta"]= verificaDados.RecuperaMetaCim(funcao);
                
                string idUsuario = verificaDadosLogin.RecuperaUsuario(Login);
                var saldoAtual = verificaDados.BuscaSaldoAtual(Login);

                var DadosUsuarioBanco = verificaDadosLogin.RecuperaDadosUsuarios(Login);
                TempData["NomeLateral"] = DadosUsuarioBanco[0]["login"];
                TempData["EmailLateral"] = DadosUsuarioBanco[0]["email"];

                @TempData["saldo"] = saldoAtual;
                var gestor = verificaDados.Gestor(Login);
                TempData["Gestor"] = gestor.ToString();
                TempData["Mensagem"] = Mensagem;
                return View("Pgd");
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
        public ActionResult Salvarproducao(Pgd Dados, FormCollection receberForm)
        {
            var insereDados = new QueryMysqlCim();
            var verificaDadosLogin = new QueryMysql();

            var Cookie = Request.Cookies.Get("CookieFarm");
            var Login = Criptografa.Descriptografar(Cookie.Value);

            var valor = receberForm["valor"];
            var DadosProdutos = insereDados.RetornaDadosProdutos(Dados.IdProduto);
            var peso = DadosProdutos[0]["peso"];
            var valorminimo = DadosProdutos[0]["valorminimo"];
            double valorponto = 0;

            if (valorminimo != "1")
            {
                double teste = Convert.ToDouble(valor.ToString().Replace(".",","));
                valorponto = (teste / Convert.ToDouble(valorminimo)) *
                                 Convert.ToDouble(peso);
            }
            else
                valorponto = Convert.ToDouble(peso);

                insereDados.InsereProducao(Dados.Cpf, Dados.IdProduto, Dados.Observacao, Dados.Datacontratacao, Login,
                valor.ToString(), 
                valorponto.ToString("N2"));

            insereDados.IncluirPontucao(Login, valorponto);

            string idUsu = verificaDadosLogin.RecuperaUsuario(Login);
            var saldoAtual = insereDados.BuscaSaldoAtual(Login);

            TempData["saldo"] = saldoAtual;


            return RedirectToAction("Pgd", "Pgd", new {Mensagem = "Produção cadastrada com sucesso !"});
        }

        public ActionResult ExcluirRegistro(int id)
        {

            var ExcluiRegistro = new QueryMysqlCim();
            var usuario = ExcluiRegistro.BuscaDadosProducao(id);
            ExcluiRegistro.ExcluirRegistro(id);
            ExcluiRegistro.AtualizarRegistroExclusao(usuario[0]["Login"], Convert.ToDouble(usuario[0]["valorponto"]));

            var saldoAtual = ExcluiRegistro.BuscaSaldoAtual(usuario[0]["Login"]);

            return RedirectToAction("Pgd", "Pgd", new { Mensagem = "Pontuação excluída com sucesso !" });
        }

        public ActionResult Extrato()
        {
            var VerificaDados = new QueryMysqlCim();

            var Validacoes = new ValidacoesPonto();
            var Cookie = Request.Cookies.Get("CookieFarm");
            var Login = Criptografa.Descriptografar(Cookie.Value);
            
            
            var DadosTabelaFuncionario = VerificaDados.RecuperaDadosProducao(Login);

            TempData["TotalPonto"] = DadosTabelaFuncionario.Count;
            for (var i = 0; i < DadosTabelaFuncionario.Count; i++)
            {
                TempData["id" + i] = DadosTabelaFuncionario[i]["id"];
                TempData["cpf" + i] = DadosTabelaFuncionario[i]["cpf"];
                TempData["DataContratacao" + i] = Convert.ToDateTime(DadosTabelaFuncionario[i]["datacontratacao"])
                    .ToString("dd/MM/yyyy");
                TempData["Observacao" + i] = DadosTabelaFuncionario[i]["observacao"];
                TempData["Produtos" + i] =
                    VerificaDados.RecuperaProduto(Convert.ToInt32(DadosTabelaFuncionario[i]["produto"]));
                TempData["valorponto" + i] = DadosTabelaFuncionario[i]["valorponto"];
            }

            return PartialView("Extrato");
        }

        public ActionResult ExtratoGestor()
        {
            var VerificaDadosGestor = new QueryMysqlCim();

            var Validacoes = new ValidacoesPonto();
            var Cookie = Request.Cookies.Get("CookieFarm");
            var Login = Criptografa.Descriptografar(Cookie.Value);
            var VerificaDados = new QueryMysql();
            var dadosSubordinados = VerificaDadosGestor.RecuperaSubordinadosGestor(Login);
            

            TempData["TotalFuncionarios"] = dadosSubordinados.Count;
            double pontuacaoTotal = 0;
            var metatotal = 0;
            var metaindIvidual = 0;
            for (var i = 0; i < dadosSubordinados.Count; i++)
            {
                TempData["meta" +i] = VerificaDadosGestor.RecuperaMetaCim(dadosSubordinados[i]["funcao"]);
                TempData["nome" + i] = dadosSubordinados[i]["nome"];
                TempData["pontuacaoatual" + i] = dadosSubordinados[i]["pontuacaoatual"];
                pontuacaoTotal = pontuacaoTotal + Convert.ToDouble(dadosSubordinados[i]["pontuacaoatual"]);
                metaindIvidual = Convert.ToInt32(TempData["meta" + i]);
                metatotal = metatotal + metaindIvidual;
            }

            TempData["pontuacaototal"] = pontuacaoTotal.ToString();
            TempData["metatotal"] = metatotal.ToString();

            
            return PartialView("ExtratoGestor");
        }
    }
}