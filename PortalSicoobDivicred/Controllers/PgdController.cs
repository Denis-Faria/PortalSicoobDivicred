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
            var verificaDados = new QueryMysqlCIM();
            var verificaDadosLogin = new QueryMysql();
            var Logado = verificaDadosLogin.UsuarioLogado();
            if (Logado)
            {
                var Cookie = Request.Cookies.Get("CookieFarm");
                var Login = Criptografa.Descriptografar(Cookie.Value);

<<<<<<< refs/remotes/upstream/master
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
=======
            
                var funcao = verificaDados.RecuperaFuncao(Login);
                @TempData["meta"]= verificaDados.RecuperaMetaCim(funcao);
                
                string idUsuario = verificaDadosLogin.RecuperaUsuario(Login);
                var saldoAtual = verificaDados.BuscaSaldoAtual(Login);
                
                @TempData["saldo"] = saldoAtual;
                var gestor = verificaDados.Gestor(Login);
                TempData["Gestor"] = gestor.ToString();
>>>>>>> Alteração
                TempData["Mensagem"] = Mensagem;
                return View("Pgd");
            }

            return RedirectToAction("Login", "Login");
        }


        public ActionResult Cadastro()
        {
<<<<<<< refs/remotes/upstream/master
            var consultaDados = new QueryMysql();
=======

            var consultaDados = new QueryMysqlCIM();
            var verificaDadosLogin = new QueryMysql();
>>>>>>> Alteração
            var dadosPGD = new Pgd();
            var dadosTabelaPGD = consultaDados.RetornaProdutos();
            dadosPGD.descricaoProduto = dadosTabelaPGD;
            dadosTabelaPGD = verificaDadosLogin.RetornaFuncionario();
            dadosPGD.nomeFuncionario = dadosTabelaPGD;


            return PartialView("Cadastro", dadosPGD);
        }

        [HttpPost]
        public ActionResult Salvarproducao(Pgd Dados, FormCollection receberForm)
        {
            var insereDados = new QueryMysqlCIM();
            var verificaDadosLogin = new QueryMysql();

            var Cookie = Request.Cookies.Get("CookieFarm");
            var Login = Criptografa.Descriptografar(Cookie.Value);

<<<<<<< refs/remotes/upstream/master

=======
>>>>>>> Alteração
            var valor = receberForm["valor"];
            var DadosProdutos = insereDados.retornaDadosProdutos(Dados.idProduto);
            var peso = DadosProdutos[0]["peso"];
            var valorminimo = DadosProdutos[0]["valorminimo"];
            double valorponto = 0;

            if (valorminimo != "1")
<<<<<<< refs/remotes/upstream/master
                valorponto = Convert.ToDouble(valor) / Convert.ToDouble(valorminimo) *
                             Convert.ToDouble(peso);
=======
            {
                double teste = Convert.ToDouble(valor.ToString().Replace(".",","));
                valorponto = (teste / Convert.ToDouble(valorminimo)) *
                                 Convert.ToDouble(peso);
            }
>>>>>>> Alteração
            else
                valorponto = Convert.ToDouble(peso);

<<<<<<< refs/remotes/upstream/master
            //string idUsuario = insereDados.RecuperaUsuario(Login);
            //  insereDados.InsereProducao(Dados.cpf, Dados.idProduto, Dados.observacao, Dados.datacontratacao, idUsuario,
            insereDados.InsereProducao(Dados.cpf, Dados.idProduto, Dados.observacao, Dados.datacontratacao, Login,
                valor,
=======
                insereDados.InsereProducao(Dados.cpf, Dados.idProduto, Dados.observacao, Dados.datacontratacao, Login,
                valor.ToString(), 
>>>>>>> Alteração
                valorponto.ToString("N2"));

            insereDados.IncluirPontucao(Login, valorponto);

<<<<<<< refs/remotes/upstream/master
            var idUsu = insereDados.RecuperaUsuario(Login);
=======
            string idUsu = verificaDadosLogin.RecuperaUsuario(Login);
>>>>>>> Alteração
            var saldoAtual = insereDados.BuscaSaldoAtual(Login);

            TempData["saldo"] = saldoAtual;


            return RedirectToAction("Pgd", "Pgd", new {Mensagem = "Produção cadastrada com sucesso !"});
        }

        public ActionResult ExcluirRegistro(int id)
        {
<<<<<<< refs/remotes/upstream/master
            var ExcluiRegistro = new QueryMysql();
=======

            var ExcluiRegistro = new QueryMysqlCIM();
>>>>>>> Alteração
            var usuario = ExcluiRegistro.BuscaDadosProducao(id);
            ExcluiRegistro.ExcluirRegistro(id);
            ExcluiRegistro.AtualizarRegistroExclusao(usuario[0]["Login"], Convert.ToDouble(usuario[0]["valorponto"]));

            var saldoAtual = ExcluiRegistro.BuscaSaldoAtual(usuario[0]["Login"]);

<<<<<<< refs/remotes/upstream/master

            //return PartialView("ViewExtrato");
            return RedirectToAction("Pgd", "Pgd", new {Mensagem = "Pontuação excluída com sucesso !"});
=======
            return RedirectToAction("Pgd", "Pgd", new { Mensagem = "Pontuação excluída com sucesso !" });
>>>>>>> Alteração
        }

        public ActionResult Extrato()
        {
<<<<<<< refs/remotes/upstream/master
=======
            var VerificaDados = new QueryMysqlCIM();

>>>>>>> Alteração
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
<<<<<<< refs/remotes/upstream/master
=======
            var VerificaDadosGestor = new QueryMysqlCIM();

>>>>>>> Alteração
            var Validacoes = new ValidacoesPonto();
            var Cookie = Request.Cookies.Get("CookieFarm");
            var Login = Criptografa.Descriptografar(Cookie.Value);
            var VerificaDados = new QueryMysql();
<<<<<<< refs/remotes/upstream/master
            var dadosSubordinados = VerificaDados.RecuperaSubordinadosGestor(Login);

=======
            var dadosSubordinados = VerificaDadosGestor.RecuperaSubordinadosGestor(Login);
            
>>>>>>> Alteração

            TempData["TotalFuncionarios"] = dadosSubordinados.Count;
            double pontuacaoTotal = 0;
            var metatotal = 0;
            var metaindIvidual = 0;
            for (var i = 0; i < dadosSubordinados.Count; i++)
            {
<<<<<<< refs/remotes/upstream/master
                TempData["meta" + i] = VerificaDados.RecuperaMetaCim(dadosSubordinados[i]["funcao"]);
=======
                TempData["meta" +i] = VerificaDadosGestor.RecuperaMetaCim(dadosSubordinados[i]["funcao"]);
>>>>>>> Alteração
                TempData["nome" + i] = dadosSubordinados[i]["nome"];
                TempData["pontuacaoatual" + i] = dadosSubordinados[i]["pontuacaoatual"];
                pontuacaoTotal = pontuacaoTotal + Convert.ToDouble(dadosSubordinados[i]["pontuacaoatual"]);
                metaindIvidual = Convert.ToInt32(TempData["meta" + i]);
                metatotal = metatotal + metaindIvidual;
            }

            TempData["pontuacaototal"] = pontuacaoTotal.ToString();
            TempData["metatotal"] = metatotal.ToString();

<<<<<<< refs/remotes/upstream/master
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
=======
            
            return PartialView("ViewExtratoGestor");
>>>>>>> Alteração
        }
    }
}