using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PortalSicoobDivicred.Aplicacao;
using PortalSicoobDivicred.Models;

namespace PortalSicoobDivicred.Controllers
{
    public class WebdeskController : Controller
    {
        public ActionResult Chamados()
        {
            var VerificaDados = new QueryMysqlWebdesk();

            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Solicitacao = new SolicitacaoWebDesk(); 
                var Setores = VerificaDados.RetornaSetor();
                Solicitacao.SetorResponsavel = Setores;

                return View(Solicitacao);
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult BuscaChamados(string Busca)
        {
            var VerificaDados = new QueryMysqlWebdesk();

            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Cookie = Request.Cookies.Get("CookieFarm");
                var Login = Criptografa.Descriptografar(Cookie.Value);
                var VerificaDadosUsuario = new QueryMysql();

                var DadosUsuarios = VerificaDadosUsuario.RecuperaDadosFuncionariosTabelaUsuario(Login);

                var ResultadoPEsquisa = VerificaDados.BuscaChamadosMeuSetor(Busca, DadosUsuarios[0]["id"]);
                TempData["TotalResultado"] = ResultadoPEsquisa.Count;

                for (int i = 0; i < ResultadoPEsquisa.Count; i++)
                {
                    TempData["NumeroChamado" + i] = ResultadoPEsquisa[i]["id"];
                    TempData["TituloChamado" + i] = ResultadoPEsquisa[i]["titulochamado"];
                    TempData["UsuarioCadastro" + i] = ResultadoPEsquisa[i]["CADASTRO"];
                    TempData["Operador" + i] = ResultadoPEsquisa[i]["OPERADOR"];
                    var Interacoes = VerificaDados.BuscaInteracaoChamados(ResultadoPEsquisa[i]["id"]);

                    TempData["TotalInteracao" + ResultadoPEsquisa[i]["id"]] = Interacoes.Count;

                    for (int j = 0; j < Interacoes.Count; j++)
                    {
                        TempData["UsuarioInteracao" + ResultadoPEsquisa[i]["id"] +j] = Interacoes[j]["nome"];
                        TempData["TextoInteracao" + ResultadoPEsquisa[i]["id"] + j] = Interacoes[j]["textointeracao"];
                        TempData["DataInteracao" + ResultadoPEsquisa[i]["id"] + j] = Interacoes[j]["data"];
                    }
                }

                return View("ResultadoPesquisa");
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CadastrarSolicitacao(SolicitacaoWebDesk DadosSolicitacao,FormCollection Dados, List<HttpPostedFileBase> postedFiles)
        {
            return View("Chamados");
        }

        public ActionResult RetornaCategoriaFuncionario(string IdSetor)
        {
            var Solicitacao =new SolicitacaoWebDesk();
            var VerificaDados = new QueryMysqlWebdesk();
            var Categoria =VerificaDados.RetornaCategoria(IdSetor);
            var Funcionario =VerificaDados.RetornaFuncionario(IdSetor);

            Solicitacao.Categoria = Categoria;
            Solicitacao.FuncionarioResponsavel = Funcionario;

            return View("CategoriaSetorSolicitacao",Solicitacao);
        }

        public ActionResult Documentos(string IdSetor)
        {

            return View("Chamados");
        }
    }
}