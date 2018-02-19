
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
        public ActionResult Pgd()
        {
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



            
            return PartialView("ViewCadastro",dadosPGD);
        }

        [HttpPost]
        public  ActionResult Salvarproducao(Pgd Dados)
        {

            var Cookie = Request.Cookies.Get("CookieFarm");
            var Login = Criptografa.Descriptografar(Cookie.Value);

            var insereDados = new QueryMysql();
            

            var DadosProdutos = insereDados.retornaDadosProdutos(Dados.idProduto);
            string peso = DadosProdutos[0]["peso"];
            string valorminimo = DadosProdutos[0]["valorminimo"];
            double valorponto = 0;
            
            if (valorminimo != "1")
            {
                valorponto = (Convert.ToDouble(Dados.valor) / Convert.ToDouble(valorminimo)) *
                                 Convert.ToDouble(peso);
            }
            else
            {
                valorponto = Convert.ToDouble(peso);
            }

            string  idUsuario=insereDados.RecuperaUsuario(Login);
            insereDados.InsereProducao(Dados.cpf,Dados.idProduto,Dados.observacao,Dados.datacontratacao,idUsuario,Dados.valor,valorponto);

            insereDados.IncluirPontucao(Convert.ToInt32(idUsuario), valorponto);
            return View("Pgd");
        }

        public ActionResult ExcluirRegistro(int id)
        {

            
            var ExcluiRegistro = new QueryMysql();
            ExcluiRegistro.ExcluirRegistro(id);

            //return PartialView("ViewExtrato");
            return View("Pgd");
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
                
                    TempData["id"+i] = DadosTabelaFuncionario[i]["id"];
                    TempData["cpf"+i] = DadosTabelaFuncionario[i]["cpf"];
                    TempData["DataContratacao" + i] = Convert.ToDateTime(DadosTabelaFuncionario[i]["datacontratacao"]).ToString("dd/MM/yyyy");
                    TempData["Observacao" + i] = DadosTabelaFuncionario[i]["observacao"];
                    TempData["Produtos" + i] = (VerificaDados.RecuperaProduto(Convert.ToInt32(DadosTabelaFuncionario[i]["produto"]))).ToString();
            }
            return PartialView("ViewExtrato");
        }
    }
}