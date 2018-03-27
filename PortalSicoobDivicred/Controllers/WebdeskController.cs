using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PortalSicoobDivicred.Aplicacao;
using PortalSicoobDivicred.Models;

namespace PortalSicoobDivicred.Controllers
{
    public class WebdeskController : Controller
    {
        public ActionResult Chamados(string Mensagem)
        {
            var VerificaDados = new QueryMysqlWebdesk();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                TempData["Mensagem"] = Mensagem;
                var Cookie = Request.Cookies.Get("CookieFarm");
                var Login = Criptografa.Descriptografar(Cookie.Value);

                var DadosUsuario = VerificaDados.RecuperaDadosUsuarios(Login);

                var ChamadosEmAberto = VerificaDados.RetornaChamadosAbertos(DadosUsuario[0]["id"]);
                TempData["TotalChamados"] = ChamadosEmAberto.Count;
                for (int i = 0; i < ChamadosEmAberto.Count; i++)
                {

                    TempData["Titulo" + i] = ChamadosEmAberto[i]["titulo"];
                    TempData["Numero" + i] = ChamadosEmAberto[i]["id"];
                    TempData["Operador" + i] = ChamadosEmAberto[i]["operador"];
                    TempData["Situacao" + i] = ChamadosEmAberto[i]["situacao"];

                    var Sla = DateTime.Now.TimeOfDay.Subtract(Convert.ToDateTime(ChamadosEmAberto[i]["datahoracadastro"]).TimeOfDay);


                    var Horas = Sla.TotalMinutes*100/TimeSpan.Parse(ChamadosEmAberto[i]["tempo"]).TotalMinutes;

                    TempData["InformacaoSLA" + i] ="TEMPO DECORRIDO: " +Sla.ToString("g") + " || TEMPO ESTIMADO: " + ChamadosEmAberto[i]["tempo"];
                    TempData["Sla" + i] = Convert.ToInt32(Horas);

                }

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
        public ActionResult CadastrarSolicitacao(FormCollection Dados, IEnumerable<HttpPostedFileBase> postedFiles)
        {
            var VerificaDados = new QueryMysqlWebdesk();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Cookie = Request.Cookies.Get("CookieFarm");
                var Login = Criptografa.Descriptografar(Cookie.Value);

                var DadosUsuario = VerificaDados.RecuperaDadosUsuarios(Login);
                var IdInteracao =VerificaDados.CadastraSolicitacao(Dados["IdSetorResponsavel"], Dados["IdCategoria"], Dados["IdFuncionarioResponsavel"],
                    Dados["Descricao"], DadosUsuario[0]["id"]);

                var lista = postedFiles.ToList();

                for (int i = 0;i< lista.Count; i++)
                {
                    if (lista[i] != null)
                    {
                        var NomeArquivo = Path.GetFileName(lista[i].FileName);
                        byte[] fileData = null;
                        using (var binaryReader = new BinaryReader(lista[i].InputStream))
                        {
                            fileData = binaryReader.ReadBytes(lista[i].ContentLength);
                        }
                        VerificaDados.InserirAnexo(IdInteracao, fileData);
                    }
                }
                return RedirectToAction("Chamados","Webdesk",new{Mensagem="Solicitação cadastrada com sucesso!"});
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
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

        public ActionResult Documentos(string Anexos)
        {

            return View("Chamados");
        }
    }
}