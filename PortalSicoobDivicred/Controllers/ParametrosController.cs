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
        public ActionResult Parametros(string Mensagem, string Erro)
        {
            
            TempData["Mensagem"] = Mensagem;
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
                TempData["Mensagem"] = Mensagem;
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


                return PartialView("ParametrosFuncionario", dadosGrupos);
            }
            return RedirectToAction("Login", "Login");
        }

        public ActionResult Grupos()
        {
            var verificaDados = new QueryMysqlParametros();
            var logado = verificaDados.UsuarioLogado();
            if (logado)
            {

                var dadosGrupos = new Parametros();
                var dadosTablelaGrupo = verificaDados.RetornaGrupos();
                dadosGrupos.DescricaoGrupo = dadosTablelaGrupo;


                return PartialView("ParametrosFuncionario", dadosGrupos);
            }
            return RedirectToAction("Login", "Login");
        }

        public ActionResult PermissaoGrupos()
        {
            var verificaDados = new QueryMysqlParametros();
            var logado = verificaDados.UsuarioLogado();
            if (logado)
            {

                var dadosPermissaoGrupos = new Parametros();
                var dadosTablelaPermissaoGrupo = verificaDados.RetornaGrupos();
                
                dadosPermissaoGrupos.PermissaoDescricaoGrupo = dadosTablelaPermissaoGrupo;


                return PartialView("ParametrosPermissao", dadosPermissaoGrupos);
            }
            return RedirectToAction("Login", "Login");
        }

        public ActionResult Permissao()
        {
            var verificaDados = new QueryMysqlParametros();
            var logado = verificaDados.UsuarioLogado();
            if (logado)
            {

                var dadosPermissoes = new Parametros();
                var dadosTablelaPermissoes = verificaDados.RetornaPermissoes();
                var dadosTablelaGrupo = verificaDados.RetornaGrupos();

                dadosPermissoes.IdPermissaoDescricao  = dadosTablelaPermissoes;
                dadosPermissoes.PermissaoDescricaoGrupo = dadosTablelaGrupo;
                return PartialView("ParametrosPermissao", dadosPermissoes);
            }
            return RedirectToAction("Login", "Login");
        }

        public ActionResult PermissaoFuncionario()
        {
            var verificaDados = new QueryMysqlParametros();
            var logado = verificaDados.UsuarioLogado();
            if (logado)
            {

                var dadosPermissoes = new Parametros();

                
                return PartialView("ParametrosPermissoesFuncionario", dadosPermissoes);
            }
            return RedirectToAction("Login", "Login");
        }

        public ActionResult ListaPermissoesFuncionario(string IdFuncionario)
        {
            var verificaDados = new QueryMysqlParametros();
            var logado = verificaDados.UsuarioLogado();
            if (logado)
            {
                var dadosPermissoes = new Parametros();
                var permissoes =  verificaDados.BuscaPermissoesFuncionario(IdFuncionario);
                dadosPermissoes.PermissaoFuncionario = permissoes;

                TempData["TotalResultadoPermissoes"] = permissoes.Count;
                return PartialView("ParametrosPermissoesFuncionario", dadosPermissoes);
            }
            return RedirectToAction("Login", "Login");
        }


        /*
        [HttpPost]
        public ActionResult BuscarFuncionarioPermissao(string NomeFuncionario)
        {
            var VerificaDados = new QueryMysqlParametros();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Permissoes = VerificaDados.BuscaPermissoesFuncionario(NomeFuncionario);
                if (Permissoes.Count != 0)
                {
                    for (var i = 0; i < Permissoes.Count; i++)
                    {
                        TempData["Id" + i] = Permissoes[i]["id"];
                        TempData["Permissoes" + i] = Permissoes[i]["nome"];
                    }


                   // TempData["TotalResultado"] = Funcionarios.Count;
                   // TempData["Editar"] = "EditarFuncao";
                   // TempData["id"] = Funcionarios[0]["id"];
                    return PartialView("PesquisaParametros");
                }
                else
                {
                    return RedirectToAction("BuscarFuncionario", new { Erro = "Nenhum registro encontrato." });
                }
            }

            return RedirectToAction("Login", "Login");
        }
        */

        // RetonarPermissoesFuncionario

        public ActionResult Grupo()
        {
            var verificaDados = new QueryMysqlParametros();
            var logado = verificaDados.UsuarioLogado();
            if (logado)
            {

                var dadosGrupos = new Parametros();
               


                return PartialView("ParametrosGrupos", dadosGrupos);
            }
            return RedirectToAction("Login", "Login");
        }

        public ActionResult RetornarPermissoesGrupo(string idGrupo)
        {
            var verificaDados = new QueryMysqlParametros();
            var logado = verificaDados.UsuarioLogado();
            if (logado)
            {
                var dadosGrupos = new Parametros();
                
                var dadosTablelaGrupo = verificaDados.RetornaPermissoesGrupo(idGrupo);
                dadosGrupos.DescricaoGrupo = dadosTablelaGrupo;
                dadosGrupos.Permissao = dadosTablelaGrupo;
                TempData["Grupo"] = idGrupo;
                return PartialView("PermissaoInteracao", dadosGrupos);
            }
            return RedirectToAction("Login", "Login");
        }

        public ActionResult RetornarPermissoesDefinicaoGrupo(string idGrupo,string idPermissao)
        {
            var verificaDados = new QueryMysqlParametros();
            var logado = verificaDados.UsuarioLogado();
            if (logado)
            {
                var dadosGrupos = new Parametros();
                dadosGrupos.Permitido = verificaDados.RetornaPermissoesDefinicaoGrupo(idGrupo, idPermissao);
                dadosGrupos.idPermissaoDescricaoGrupo = Convert.ToInt32(idGrupo);
                dadosGrupos.descricaoPermissao = idPermissao;
                
                
                return PartialView("PermissaoDefinicaoInteracao", dadosGrupos);
            }
            return RedirectToAction("Login", "Login");
        }


        /*
        [HttpPost]
        public ActionResult Funcionario(Funcao dadosCadastro, FormCollection dados)
        {
            var verificaDados = new QueryMysqlParametros();
            var logado = verificaDados.UsuarioLogado();

            /*var cookie = Request.Cookies.Get("CookieFarm");

            if (cookie != null)
            {
                var login = Criptografa.Descriptografar(cookie.Value);
            }

            if (logado)
            {
                var dadosGrupos = verificaDados.RetornaGrupos();
                return PartialView("ParametrosGrupos", dadosGrupos);
            }

            return RedirectToAction("Login", "Login");
        }
        */




        [HttpPost]
        public ActionResult SalvarUsuario(Parametros dados, FormCollection receberForm)
        {
            var insereDados = new QueryMysqlParametros();

            var cookie = Request.Cookies.Get("CookieFarm");
            if (cookie != null)
            {

                
                insereDados.InsereUsuario(dados.NomeFuncionario, dados.Pa, dados.dataAdmissao, dados.CpfFuncionario, dados.RgFuncionario,
                    dados.PisFuncionario, dados.Estagiario, dados.LoginFuncionario, "123", dados.Email, dados.idDescricaoGrupo, dados.Gestor, dados.Matricula);

            }


            return RedirectToAction("Parametros", "Parametros", new { Mensagem = "Usuário cadastrada com sucesso !" });
        }

        [HttpPost]
        public ActionResult SalvarPermissao(Parametros dados, FormCollection receberForm)
        {
            var insereDados = new QueryMysqlParametros();

            var cookie = Request.Cookies.Get("CookieFarm");
            if (cookie != null)
            {
                insereDados.AtualizaPermissao(dados.idPermissaoDescricaoGrupo,
                    dados.descricaoPermissao, dados.Permitido);

                // insereDados.InsereUsuario(dados.NomeFuncionario, dados.Pa, dados.dataAdmissao, dados.CpfFuncionario, dados.RgFuncionario,
                //     dados.PisFuncionario, dados.Estagiario, dados.LoginFuncionario, "123", dados.Email, dados.idDescricaoGrupo, dados.Gestor, dados.Matricula);

            }


            return RedirectToAction("Parametros", "Parametros", new { Mensagem = "Usuário cadastrada com sucesso !" });
        }

        [HttpPost]
        public ActionResult SalvarGrupos(Parametros dados, FormCollection receberForm)
        {
            var insereDados = new QueryMysqlParametros();

            var cookie = Request.Cookies.Get("CookieFarm");
            if (cookie != null)
            {
                insereDados.InsereGrupos(dados.DescricaoGrupos);
            }
            return RedirectToAction("Parametros", "Parametros", new { Mensagem = "Grupo cadastrado com sucesso !" });
        }



        [HttpPost]
        public ActionResult BuscarFuncionario(string NomeFuncionario)
        {
            var VerificaDados = new QueryMysqlParametros();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Funcionarios = VerificaDados.BuscaFuncionario(NomeFuncionario);
                if (Funcionarios.Count != 0)
                {
                    for (var i = 0; i < Funcionarios.Count; i++)
                    {
                        TempData["Id" + i] = Funcionarios[i]["id"];
                        TempData["Nome" + i] = Funcionarios[i]["nome"];
                    }


                    TempData["TotalResultado"] = Funcionarios.Count;
                    TempData["Editar"] = "EditarFuncao";
                    TempData["id"] = Funcionarios[0]["id"];
                    return PartialView("PesquisaParametros");
                }
                else
                {
                    return RedirectToAction("BuscarFuncionario", new { Erro = "Nenhum registro encontrato." });
                }
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult BuscarFuncionarioPermissoes(string NomeFuncionario)
        {
            var VerificaDados = new QueryMysqlParametros();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Funcionarios = VerificaDados.BuscaFuncionario(NomeFuncionario);
                if (Funcionarios.Count != 0)
                {
                    for (var i = 0; i < Funcionarios.Count; i++)
                    {
                        TempData["Id" + i] = Funcionarios[i]["id"];
                        TempData["Nome" + i] = Funcionarios[i]["nome"];
                    }


                    TempData["TotalResultado"] = Funcionarios.Count;
                   // TempData["Editar"] = "EditarFuncao";
                    TempData["id"] = Funcionarios[0]["id"];
                    return PartialView("PesquisaPermissoes");
                }
                else
                {
                    return RedirectToAction("BuscarFuncionarioPermissoes", new { Erro = "Nenhum registro encontrato." });
                }
            }

            return RedirectToAction("Login", "Login");
        }





        [HttpPost]
        public ActionResult BuscarPermissoes(string IdPermissao)
        {
            var VerificaDados = new QueryMysqlParametros();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Funcionarios = VerificaDados.BuscaPermissao(IdPermissao);
                if (Funcionarios.Count != 0)
                {
                    for (var i = 0; i < Funcionarios.Count; i++)
                    {
                        TempData["IdPermissao" + i] = Funcionarios[i]["idpermissao"];
                        TempData["IdGrupo" + i] = Funcionarios[i]["idgrupo"];
                        TempData["Valor" + i] = Funcionarios[i]["valor"];
                    }


                    TempData["TotalResultado"] = Funcionarios.Count;
                    //TempData["Editar"] = "EditarFuncao";
                    TempData["idpermissao"] = Funcionarios[0]["idpermissao"];
                    return PartialView("PesquisaParametros");
                }
                else
                {
                    return RedirectToAction("BuscarFuncionario", new { Erro = "Nenhum registro encontrato." });
                }
            }

            return RedirectToAction("Login", "Login");
        }




        [HttpPost]
        public ActionResult BuscaGrupo(string DescricaoGrupo)
        {
            var VerificaDados = new QueryMysqlParametros();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Grupos = VerificaDados.BuscaGrupo(DescricaoGrupo);
                if (Grupos.Count != 0)
                {
                    for (var i = 0; i < Grupos.Count; i++)
                    {
                        TempData["Id" + i] = Grupos[i]["id"];
                        TempData["Descricao" + i] = Grupos[i]["descricao"];
                    }


                    TempData["TotalResultado"] = Grupos.Count;
                    
                    TempData["id"] = Grupos[0]["id"];
                    return PartialView("PesquisaGrupos");
                }
                else
                {
                    return RedirectToAction("BuscarFuncionario", new { Erro = "Nenhum registro encontrato." });
                }
            }

            return RedirectToAction("Login", "Login");
        }




        [HttpPost]
        public ActionResult RecuperaFuncionarios(string IdFuncionario)
        {
            var VerificaDados = new QueryMysqlParametros();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                
                //var Certificacoes = VerificaDados.RetornaCertificacoes();
                var DadosFuncionario = VerificaDados.RecuperaDadosFuncionario(IdFuncionario);
                var FuncionarioRecupera = new Parametros();


                var dadosTablelaGrupo = VerificaDados.RetornaGrupos();
                TempData["id"] = Convert.ToInt32(DadosFuncionario[0]["id"]);
                FuncionarioRecupera.DescricaoGrupo = dadosTablelaGrupo;
                FuncionarioRecupera.id = Convert.ToInt32(DadosFuncionario[0]["id"]);
                FuncionarioRecupera.idDescricaoGrupo = Convert.ToInt32(DadosFuncionario[0]["idgrupo"]);
                FuncionarioRecupera.dataAdmissao = Convert.ToDateTime(DadosFuncionario[0]["admissao"]).ToString("dd/MM/yyyy");
                FuncionarioRecupera.NomeFuncionario = DadosFuncionario[0]["nome"];
                FuncionarioRecupera.Pa = Convert.ToInt32(DadosFuncionario[0]["idpa"]);
                FuncionarioRecupera.CpfFuncionario = DadosFuncionario[0]["cpf"];
                FuncionarioRecupera.RgFuncionario = DadosFuncionario[0]["rg"];
                FuncionarioRecupera.PisFuncionario = DadosFuncionario[0]["pis"];
                FuncionarioRecupera.LoginFuncionario = DadosFuncionario[0]["login"];
                FuncionarioRecupera.Email = DadosFuncionario[0]["email"];
                FuncionarioRecupera.Gestor = DadosFuncionario[0]["gestor"];
                FuncionarioRecupera.Estagiario = DadosFuncionario[0]["estagiario"];
                FuncionarioRecupera.Matricula = DadosFuncionario[0]["matricula"];


                return PartialView("EditarFuncionario", FuncionarioRecupera);
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult RecuperaGrupos(string IdGrupo)
        {
            var VerificaDados = new QueryMysqlParametros();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {

                //var Certificacoes = VerificaDados.RetornaCertificacoes();
                var DadosGrupo = VerificaDados.RecuperaDadosGrupo(IdGrupo);
                var GrupoRecupera = new Parametros();


                var dadosTablelaGrupo = VerificaDados.RetornaGrupos();
                TempData["id"] = Convert.ToInt32(DadosGrupo[0]["id"]);
               
                GrupoRecupera.id = Convert.ToInt32(DadosGrupo[0]["id"]);
                GrupoRecupera.DescricaoGrupos = (DadosGrupo[0]["descricao"]).ToString();


                return PartialView("EditarGrupos", GrupoRecupera);
            }

            return RedirectToAction("Login", "Login");
        }



        public ActionResult AtualizaDadosFuncionario()
        {
            return PartialView("EditarFuncionario");
        }

        [HttpPost]
        public ActionResult AtualizaDadosFuncionario(Parametros dados, FormCollection receberForm)
        {
           // TempData["id"] = dados.id;
            var atualizaFuncionario = new QueryMysqlParametros();
            atualizaFuncionario.AtualizaUsuario(dados.id, dados.NomeFuncionario, dados.Pa, dados.dataAdmissao, dados.CpfFuncionario, dados.RgFuncionario,
                dados.PisFuncionario, dados.Estagiario, dados.LoginFuncionario, dados.Email, dados.idDescricaoGrupo, dados.Gestor, dados.Matricula);
            return RedirectToAction("Parametros", new { Mensagem = "Registro alterado com sucesso!!" });
        }

        public ActionResult AtualizaDadosGrupo()
        {
            return PartialView("EditarGrupos");
        }

        [HttpPost]
        public ActionResult AtualizaDadosGrupo(Parametros dados, FormCollection receberForm)
        {
            //TempData["id"] = dados.id;
            var atualizaFuncionario = new QueryMysqlParametros();
            atualizaFuncionario.AtualizaGrupos(dados.id, dados.DescricaoGrupos);
            return RedirectToAction("Parametros", new { Mensagem = "Registro alterado com sucesso!!" });
        }



        public ActionResult ExcluirFuncionario(string IdFuncionario)
        {
            var VerificaDados = new QueryMysqlParametros();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                VerificaDados.ExcluirFuncionario(IdFuncionario);
                return RedirectToAction("Parametros", "Parametros",
                    new { Mensagem = "Funcionário excluido com sucesso !" });
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult ExcluirGrupo(string IdFuncionario)
        {
            var VerificaDados = new QueryMysqlParametros();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                VerificaDados.ExcluirFuncionario(IdFuncionario);
                return RedirectToAction("Parametros", "Parametros",
                    new { Mensagem = "Funcionário excluido com sucesso !" });
            }

            return RedirectToAction("Login", "Login");
        }


    }
}