
using PortalSicoobDivicred.Aplicacao;
using PortalSicoobDivicred.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortalSicoobDivicred.Controllers
{
    public class PgdController : Controller
    {
        // GET: Pgd
        public ActionResult Pgd(string Mensagem)
        {
            var Cookie = Request.Cookies.Get("CookieFarm");
            var Login = Criptografa.Descriptografar(Cookie.Value);
            var insereDados = new QueryMysql();
            string idUsuario = insereDados.RecuperaUsuario(Login);
            var saldoAtual = insereDados.BuscaSaldoAtual(Convert.ToInt32(idUsuario));


            @TempData["saldo"] = saldoAtual;

            var gestor = insereDados.Gestor(Login);
            TempData["Gestor"] = gestor.ToString();
            TempData["Mensagem"] = Mensagem;
            return View("Pgd");
        }

        public ActionResult Cadastro()
        {

            var consultaDados = new QueryMysql();
            var dadosPGD = new Pgd();
            var dadosTabelaPGD = consultaDados.RetornaProdutos();
            dadosPGD.descricaoProduto = dadosTabelaPGD;
            dadosTabelaPGD = consultaDados.RetornaFuncionario();
            dadosPGD.nomeFuncionario = dadosTabelaPGD;




            return PartialView("ViewCadastro", dadosPGD);
        }

        [HttpPost]
        public ActionResult Salvarproducao(Pgd Dados, FormCollection receberForm)
        {
            var insereDados = new QueryMysql();

            var Cookie = Request.Cookies.Get("CookieFarm");
            var Login = Criptografa.Descriptografar(Cookie.Value);


            
            

            var valor = receberForm["valor"];
            var DadosProdutos = insereDados.retornaDadosProdutos(Dados.idProduto);
            string peso = DadosProdutos[0]["peso"];
            string valorminimo = DadosProdutos[0]["valorminimo"];
            double valorponto = 0;

            if (valorminimo != "1")
            {
                valorponto = (Convert.ToDouble(valor) / Convert.ToDouble(valorminimo)) *
                                 Convert.ToDouble(peso);
            }
            else
            {
                valorponto = Convert.ToDouble(peso);
            }

            string idUsuario = insereDados.RecuperaUsuario(Login);
            insereDados.InsereProducao(Dados.cpf, Dados.idProduto, Dados.observacao, Dados.datacontratacao, idUsuario,
                valor.ToString(), 
                valorponto.ToString("N2"));

            insereDados.IncluirPontucao(Convert.ToInt32(idUsuario), valorponto);

            string idUsu = insereDados.RecuperaUsuario(Login);
            var saldoAtual = insereDados.BuscaSaldoAtual(Convert.ToInt32(idUsu));

            @TempData["saldo"] = saldoAtual;

            return View("Pgd");
        }

        public ActionResult ExcluirRegistro(int id)
        {

            var ExcluiRegistro = new QueryMysql();
            var usuario = ExcluiRegistro.BuscaDadosProducao(id);
            ExcluiRegistro.ExcluirRegistro(id);
            ExcluiRegistro.AtualizarRegistroExclusao(Convert.ToInt32(usuario[0]["usuario"]), Convert.ToDouble(usuario[0]["valorponto"]));

            var saldoAtual = ExcluiRegistro.BuscaSaldoAtual(Convert.ToInt32(usuario[0]["usuario"]));



            //return PartialView("ViewExtrato");
            return RedirectToAction("Pgd", "Pgd", new { Mensagem = "Pontuação excluída com sucesso !" });
        }

        public ActionResult Extrato()
        {


            var Validacoes = new ValidacoesPonto();
            var Cookie = Request.Cookies.Get("CookieFarm");
            var Login = Criptografa.Descriptografar(Cookie.Value);
            var VerificaDados = new QueryMysql();
            string loginUsuario = VerificaDados.RecuperaUsuario(Login);
            var DadosTabelaFuncionario = VerificaDados.RecuperaDadosProducao(loginUsuario);

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
            return PartialView("ViewExtrato");
        }

        public ActionResult ExtratoGestor()
        {

            var Validacoes = new ValidacoesPonto();
            var Cookie = Request.Cookies.Get("CookieFarm");
            var Login = Criptografa.Descriptografar(Cookie.Value);
            var VerificaDados = new QueryMysql();
            var dadosSubordinados = VerificaDados.RecuperaSubordinadosGestor(Login);

            TempData["TotalFuncionarios"] = dadosSubordinados.Count;

            for (int i = 0; i < dadosSubordinados.Count; i++)
            {
                TempData["nome" + i] = dadosSubordinados[i]["nome"];
                TempData["pontuacaoatual" + i] = dadosSubordinados[i]["pontuacaoatual"];

            }

            
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
            return PartialView("ViewExtratoGestor");
        }
    }
}