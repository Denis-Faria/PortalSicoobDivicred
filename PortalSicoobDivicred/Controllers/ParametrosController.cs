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
        public ActionResult Parametros(string mensagemValidacao, string erro)
        {

            TempData["MensagemValidacao"] = mensagemValidacao;
            TempData["Erro"] = erro;
            var insereDados = new QueryMysql();
            var logado = insereDados.UsuarioLogado();
            if (logado)
            {

                //return RedirectToAction("Login", "Login");
                //return View("Parametros");
                //return PartialView("Funcionario");
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
            var dadosPgd = new Pgd();
            var dadosTabelaPgd = consultaDados.RetornaProdutos();
            dadosPgd.DescricaoProduto = dadosTabelaPgd;
            dadosTabelaPgd = verificaDadosLogin.RetornaFuncionario();
            dadosPgd.NomeFuncionario = dadosTabelaPgd;


            return PartialView("Cadastro", dadosPgd);
        }
    }
}