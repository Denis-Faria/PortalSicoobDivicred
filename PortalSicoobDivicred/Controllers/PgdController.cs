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
            var insereDados = new QueryMysql();
            var Logado = insereDados.UsuarioLogado();
            if (Logado)
            {
                var Cookie = Request.Cookies.Get("CookieFarm");
                var Login = Criptografa.Descriptografar(Cookie.Value);

                if (insereDados.PermissaoCurriculos(Login))
                    TempData["PermissaoCurriculo"] =
                        " ";
                else
                    TempData["PermissaoCurriculo"] = "display: none";

                var funcao = insereDados.RecuperaFuncao(Login);
                TempData["meta"] = insereDados.RecuperaMetaCim(funcao);

                var idUsuario = insereDados.RecuperaUsuario(Login);
                var saldoAtual = insereDados.BuscaSaldoAtual(Login);

                TempData["saldo"] = saldoAtual;
                var gestor = insereDados.Gestor(Login);
                TempData["Gestor"] = gestor;
                TempData["Mensagem"] = Mensagem;
                return View("Pgd");
            }

            return RedirectToAction("Login", "Login");
        }


        public ActionResult Cadastro()
        {
            var consultaDados = new QueryMysql();
            var dadosPGD = new Pgd();
            var dadosTabelaPGD = consultaDados.RetornaProdutos();
            dadosPGD.descricaoProduto = dadosTabelaPGD;
            dadosTabelaPGD = consultaDados.RetornaFuncionario();
            dadosPGD.nomeFuncionario = dadosTabelaPGD;


            return PartialView("Cadastro", dadosPGD);
        }

        [HttpPost]
        public ActionResult Salvarproducao(Pgd Dados, FormCollection receberForm)
        {
            var insereDados = new QueryMysql();

            var Cookie = Request.Cookies.Get("CookieFarm");
            var Login = Criptografa.Descriptografar(Cookie.Value);


            var valor = receberForm["valor"];
            var DadosProdutos = insereDados.retornaDadosProdutos(Dados.idProduto);
            var peso = DadosProdutos[0]["peso"];
            var valorminimo = DadosProdutos[0]["valorminimo"];
            double valorponto = 0;

            if (valorminimo != "1")
                valorponto = Convert.ToDouble(valor) / Convert.ToDouble(valorminimo) *
                             Convert.ToDouble(peso);
            else
                valorponto = Convert.ToDouble(peso);

            //string idUsuario = insereDados.RecuperaUsuario(Login);
            //  insereDados.InsereProducao(Dados.cpf, Dados.idProduto, Dados.observacao, Dados.datacontratacao, idUsuario,
            insereDados.InsereProducao(Dados.cpf, Dados.idProduto, Dados.observacao, Dados.datacontratacao, Login,
                valor,
                valorponto.ToString("N2"));

            insereDados.IncluirPontucao(Login, valorponto);

            var idUsu = insereDados.RecuperaUsuario(Login);
            var saldoAtual = insereDados.BuscaSaldoAtual(Login);

            TempData["saldo"] = saldoAtual;


            return RedirectToAction("Pgd", "Pgd", new {Mensagem = "Produção cadastrada com sucesso !"});
        }

        public ActionResult ExcluirRegistro(int id)
        {
            var ExcluiRegistro = new QueryMysql();
            var usuario = ExcluiRegistro.BuscaDadosProducao(id);
            ExcluiRegistro.ExcluirRegistro(id);
            ExcluiRegistro.AtualizarRegistroExclusao(usuario[0]["Login"], Convert.ToDouble(usuario[0]["valorponto"]));

            var saldoAtual = ExcluiRegistro.BuscaSaldoAtual(usuario[0]["Login"]);


            //return PartialView("ViewExtrato");
            return RedirectToAction("Pgd", "Pgd", new {Mensagem = "Pontuação excluída com sucesso !"});
        }

        public ActionResult Extrato()
        {
            var Validacoes = new ValidacoesPonto();
            var Cookie = Request.Cookies.Get("CookieFarm");
            var Login = Criptografa.Descriptografar(Cookie.Value);
            var VerificaDados = new QueryMysql();
            //string loginUsuario = VerificaDados.RecuperaUsuario(Login);
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
            var Validacoes = new ValidacoesPonto();
            var Cookie = Request.Cookies.Get("CookieFarm");
            var Login = Criptografa.Descriptografar(Cookie.Value);
            var VerificaDados = new QueryMysql();
            var dadosSubordinados = VerificaDados.RecuperaSubordinadosGestor(Login);


            TempData["TotalFuncionarios"] = dadosSubordinados.Count;
            double pontuacaoTotal = 0;
            var metatotal = 0;
            var metaindIvidual = 0;
            for (var i = 0; i < dadosSubordinados.Count; i++)
            {
                TempData["meta" + i] = VerificaDados.RecuperaMetaCim(dadosSubordinados[i]["funcao"]);
                TempData["nome" + i] = dadosSubordinados[i]["nome"];
                TempData["pontuacaoatual" + i] = dadosSubordinados[i]["pontuacaoatual"];
                pontuacaoTotal = pontuacaoTotal + Convert.ToDouble(dadosSubordinados[i]["pontuacaoatual"]);
                metaindIvidual = Convert.ToInt32(TempData["meta" + i]);
                metatotal = metatotal + metaindIvidual;
            }

            TempData["pontuacaototal"] = pontuacaoTotal.ToString();
            TempData["metatotal"] = metatotal.ToString();

            //var DadosTabelaFuncionario = VerificaDados.RecuperaDadosProducao(loginUsuario);
            /*            var pontuacaoFuncionarios = VerificaDados.

                        TempData["TotalPonto"] = DadosTabelaFuncionario.Count;
                        for (int i = 0; i < DadosTabelaFuncionario.Count; i++)
                        {

                            TempData["id" + i] = DadosTabelaFuncionario[i]["id"];
                            TempData["cpf" + i] = DadosTabelaFuncionario[i]["cpf"];
                            TempData["DataContratacao" + i] = Convert.ToDateTime(DadosTabelaFuncionario[i]["datacontratacao"]).ToString("dd/MM/yyyy");
                            TempData["Observacao" + i] = DadosTabelaFuncionario[i]["observacao"];
                            TempData["Produtos" + i] = (VerificaDados.RecuperaProduto(Convert.ToInt32(DadosTabelaFuncionario[i]["produto"]))).ToString();
                            TempData["valorponto" + i] = DadosTabelaFuncionario[i]["valorponto"].ToString();
                        }
          */
            return PartialView("ExtratoGestor");
        }
    }
}