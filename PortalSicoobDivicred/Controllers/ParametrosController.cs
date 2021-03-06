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
using Id = DocumentFormat.OpenXml.Office2010.Excel.Id;

namespace PortalSicoobDivicred.Controllers
{
    public class ParametrosController : Controller
    {
        public ActionResult Parametros(string mensagemValidacao, string erro)
        {

            //TempData["MensagemValidacao"] = mensagemValidacao;
            TempData["Erro"] = erro;
            
            var insereDados = new QueryMysql();
            var logado = insereDados.UsuarioLogado();
            if (logado)
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
                TempData["Mensagem"] = mensagemValidacao;
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
                var permissoesAtivas = verificaDados.BuscaPermissoesAtivasFuncionario(IdFuncionario);
                if (permissoes.Count != 0)
                {
                    for (var i = 0; i < permissoes.Count; i++)
                    {
                        TempData["descricao" + i] = permissoes[i]["descricao"];
                    }
                }

                if (permissoesAtivas.Count != 0)
                {
                    for (var i = 0; i < permissoesAtivas.Count; i++)
                    {
                        TempData["descricaoAtivas" + i] = permissoesAtivas[i]["descricao"];
                    }
                }

                // dadosPermissoes.PermissaoFuncionario = permissoes;
                if (permissoes.Count > permissoesAtivas.Count)
                {
                    TempData["TotalResultados"] = permissoes.Count;
                    TempData["Maior"] ="Permissoes";
                    TempData["TotalMenor"]= permissoesAtivas.Count;
                }
                else
                {
                    TempData["TotalResultados"] = permissoesAtivas.Count;
                    TempData["Maior"] = "PermissoesAtivas";
                    TempData["TotalMenor"] = permissoes.Count;
                }

                TempData["idFuncionario"] = IdFuncionario;

             
                return PartialView("ExibirPermissoes", dadosPermissoes);
            }
            return RedirectToAction("Login", "Login");
        }


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


            return RedirectToAction("Parametros", "Parametros", new { mensagemValidacao = "Usuário cadastrada com sucesso !" });
        }

        [HttpPost]
        public ActionResult SalvarSubtarefa(Parametros dados, FormCollection formularioSubtarefa)
        {
            var insereDados = new QueryMysqlParametros();

            var cookie = Request.Cookies.Get("CookieFarm");
            if (cookie != null)
            {
                TimeSpan sla = TimeSpan.FromSeconds(Convert.ToDouble(formularioSubtarefa["seconds"]));
                insereDados.InsereSubtarefa(dados.SubtarefasDescricao, dados.idTarefaSubtarefas, dados.id,sla.ToString(),dados.MultiploAtendente );

            }


            return RedirectToAction("Parametros", "Parametros", new { mensagemValidacao = "SubTarefa cadastrada com sucesso !" });
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
               
            }


            return RedirectToAction("Parametros", "Parametros", new { Mensagem = "Permissão cadastrada com sucesso !" });
        }

       

        [HttpPost]
        public void  ReinicarSenha(string IdFuncionarios)
        {
           

            var cookie = Request.Cookies.Get("CookieFarm");
            if (cookie != null)
            {
                var verificaDados = new QueryMysqlParametros();
                verificaDados.ReiniciarSenha(IdFuncionarios);

            }


        
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
            return RedirectToAction("Parametros", "Parametros", new { mensagemValidacao = "Grupo cadastrado com sucesso !" });
        }

        [HttpPost]
        public ActionResult SalvarTarefa(Parametros dados)
        {
            var insereDados = new QueryMysqlParametros();

            var cookie = Request.Cookies.Get("CookieFarm");
            if (cookie != null)
            {
                insereDados.InsereTarefas(dados.DescricaoTarefa);
            }
            return RedirectToAction("Parametros", "Parametros", new { mensagemValidacao = "Tarefa cadastrado com sucesso !" });
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
            atualizaFuncionario.AtualizaFuncionario(dados.id, dados.NomeFuncionario, dados.Pa, dados.dataAdmissao, dados.CpfFuncionario, dados.RgFuncionario,
                dados.PisFuncionario, dados.Estagiario, dados.LoginFuncionario, dados.Email, dados.idDescricaoGrupo, dados.Gestor, dados.Matricula);
            return RedirectToAction("Parametros", new { mensagemValidacao = "Registro alterado com sucesso!!" });
        }

        public ActionResult AtualizaDadosTarefa()
        {
            return PartialView("EditarFuncionario");
        }

        [HttpPost]
        public ActionResult AtualizaDadosTarefa(Parametros dados, FormCollection receberForm)
        {
           
            var atualizaTarefa = new QueryMysqlParametros();
            atualizaTarefa.AtualizarTarefa(dados.idTarefa.ToString(),dados.DescricaoTarefa);
            return RedirectToAction("Parametros", new { mensagemValidacao = "Registro alterado com sucesso!!" });
        }

        [HttpPost]
        public ActionResult AtualizaDadosSubtarefa(Parametros dados, FormCollection receberForm)
        {
           
            var atualizaTarefa = new QueryMysqlParametros();

            TimeSpan sla = TimeSpan.FromSeconds(Convert.ToDouble(receberForm["Editarseconds"]));

            atualizaTarefa.AtualizarSubtarefa(dados.idSubtarefas,dados.SubtarefasDescricao, dados.idTarefaSubtarefas,dados.id,sla.ToString(),dados.MultiploAtendente);
            return RedirectToAction("Parametros", new { mensagemValidacao = "Registro alterado com sucesso!!" });
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
            return RedirectToAction("Parametros", new { mensagemValidacao = "Registro alterado com sucesso!!" });
        }



        public ActionResult ExcluirFuncionario(string IdFuncionario)
        {
            var VerificaDados = new QueryMysqlParametros();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                VerificaDados.ExcluirFuncionario(IdFuncionario);
                return RedirectToAction("Parametros", "Parametros",
                    new { mensagemValidacao = "Funcionário excluido com sucesso !" });
            }

            return RedirectToAction("Login", "Login");
        }
      

        public ActionResult Tarefas()
        {
            var verificaDados = new QueryMysqlParametros();
            var logado = verificaDados.UsuarioLogado();
            if (logado)
            {

                var dadosTarefas = new Parametros();


                return PartialView("ParametrosTarefas", dadosTarefas);
            }
            return RedirectToAction("Login", "Login");
        }

        public ActionResult Subtarefas()
        {
            var verificaDados = new QueryMysqlParametros();
            var logado = verificaDados.UsuarioLogado();
            if (logado)
            {

                var dadosSubtarefas = new Parametros();

                var dadosTabelaFuncionarios = verificaDados.RetornaFuncionarios();
                var dadosTablelaTarefas = verificaDados.RetornaTarefas();
                dadosSubtarefas.DescricaoTarefas = dadosTablelaTarefas;
                dadosSubtarefas.FuncionariosNome = dadosTabelaFuncionarios;
              


                return PartialView("ParametrosSubtarefas", dadosSubtarefas);
            }
            return RedirectToAction("Login", "Login");
        }


        
        [HttpPost]
        public async Task<JsonResult> EnviarDadosArrayPermissoes(PermissaoFuncionario[] tabelaPermissoesFuncionario, PermissaoFuncionario[] tabelaPermissoesInativasFuncionario, string idFuncionario)
        {

            var dadosPermissoes = new PermissaoFuncionario();
            var VerificaDados = new QueryMysqlParametros();

            //var permissoes = VerificaDados.BuscaPermissoesFuncionario(idFuncionario);
            var permissoesAtivas = VerificaDados.BuscaPermissoesCadastradasFuncionario(idFuncionario);

            if (permissoesAtivas.Count == 0)
            {
                for (var m = 0; m < tabelaPermissoesFuncionario.Length; m++)
                {

                        VerificaDados.InserePermissaoFuncionario(tabelaPermissoesFuncionario[m].Permissao,
                            idFuncionario);
                 
                }
            }

            if (tabelaPermissoesInativasFuncionario!= null)
            {
                for (var i = 0; i < permissoesAtivas.Count; i++)
                {

                    for (var m = 0; m < tabelaPermissoesInativasFuncionario.Length; m++)
                    {
                        if (permissoesAtivas[i]["descricao"] ==
                            tabelaPermissoesInativasFuncionario[m].PermissaoExcluida)
                        {
                            VerificaDados.ExcluiPermissaoFuncionario(
                                tabelaPermissoesInativasFuncionario[m].PermissaoExcluida, idFuncionario);
                        }
                    }
                }
            }

            if (tabelaPermissoesFuncionario!=null)
            {
                for (var i = 0; i < permissoesAtivas.Count; i++)
                {
                    for (var m = 0; m < tabelaPermissoesFuncionario.Length; m++)
                    {
                        if (permissoesAtivas[i]["descricao"] == tabelaPermissoesFuncionario[m].Permissao)
                        {
                            VerificaDados.IncluiPermissaoInativa(tabelaPermissoesFuncionario[m].Permissao, idFuncionario);
                        }
                        else
                        {
                            VerificaDados.InserePermissaoFuncionario(tabelaPermissoesFuncionario[m].Permissao,
                                idFuncionario);
                        }
                    }
                }
            }

            return Json("Ok");
        }

        [HttpPost]
        public ActionResult BuscaTarefa(string DescricaoTarefa)
        {
            var VerificaDados = new QueryMysqlParametros();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Grupos = VerificaDados.BuscaTarefa(DescricaoTarefa);
                if (Grupos.Count != 0)
                {
                    for (var i = 0; i < Grupos.Count; i++)
                    {
                        TempData["Id" + i] = Grupos[i]["id"];
                        TempData["Descricao" + i] = Grupos[i]["descricao"];
                    }


                    TempData["TotalResultado"] = Grupos.Count;

                    TempData["id"] = Grupos[0]["id"];
                    return PartialView("PesquisaTarefas");
                }
                else
                {
                    return RedirectToAction("BuscarFuncionario", new { Erro = "Nenhum registro encontrato." });
                }
            }

            return RedirectToAction("Login", "Login");
        }


        [HttpPost]
        public ActionResult BuscaSubtarefa(string DescricaoSubtarefa)
        {
            var VerificaDados = new QueryMysqlParametros();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Subtarefa = VerificaDados.BuscaSubtarefa(DescricaoSubtarefa);
                if (Subtarefa.Count != 0)
                {
                    for (var i = 0; i < Subtarefa.Count; i++)
                    {
                        TempData["Id" + i] = Subtarefa[i]["id"];
                        TempData["Descricao" + i] = Subtarefa[i]["descricao"];
                    }


                    TempData["TotalResultado"] = Subtarefa.Count;

                    TempData["id"] = Subtarefa[0]["id"];
                    return PartialView("PesquisaSubtarefas");
                }
                else
                {
                    return RedirectToAction("BuscarFuncionario", new { Erro = "Nenhum registro encontrato." });
                }
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult RecuperaTarefa(string IdTarefa)
        {
            var VerificaDados = new QueryMysqlParametros();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {

                //var Certificacoes = VerificaDados.RetornaCertificacoes();
                var DadosTarefa = VerificaDados.RecuperaDadosTarefa(IdTarefa);
                var TarefaRecupera = new Parametros();


              
                TempData["id"] = Convert.ToInt32(DadosTarefa[0]["id"]);

                TarefaRecupera.idTarefa = Convert.ToInt32(DadosTarefa[0]["id"]);
                TarefaRecupera.DescricaoTarefa = (DadosTarefa[0]["descricao"]).ToString();


                return PartialView("EditarTarefa", TarefaRecupera);
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult RecuperaSubtarefa(string IdSubtarefa)
        {
            var VerificaDados = new QueryMysqlParametros();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {

               
                var DadosSubtarefa = VerificaDados.RecuperaDadosSubtarefa(IdSubtarefa);
                var SubtarefaRecupera = new Parametros();
                var dadosTabelaTarefa = VerificaDados.RetornaTarefas();
                var dadosTabelaFuncionario = VerificaDados.RetornaFuncionarios();

                SubtarefaRecupera.idSubtarefas = Convert.ToInt32(DadosSubtarefa[0]["id"]);
                SubtarefaRecupera.SubtarefasDescricao = DadosSubtarefa[0]["descricao"];
                SubtarefaRecupera.DescricaoTarefas = dadosTabelaTarefa;
                SubtarefaRecupera.idTarefaSubtarefas = Convert.ToInt32(DadosSubtarefa[0]["idtarefa"]);
                SubtarefaRecupera.FuncionariosNome = dadosTabelaFuncionario;
                SubtarefaRecupera.id = Convert.ToInt32(DadosSubtarefa[0]["idfuncionarioresponsavel"]);
                SubtarefaRecupera.TempoSubTarefa = DadosSubtarefa[0]["tempo"];

     //           TempData["Tempo"] =  TimeSpan.FromSeconds(Convert.ToDouble(DadosSubtarefa[0]["tempo"]));

                
                TimeSpan sla1 = TimeSpan.Parse(DadosSubtarefa[0]["tempo"]);
                TempData["Tempo"] = sla1.TotalSeconds;
                



                SubtarefaRecupera.MultiploAtendente = DadosSubtarefa[0]["multiploatendente"];


                return PartialView("EditarSubtarefa", SubtarefaRecupera);
            }

            return RedirectToAction("Login", "Login");
        }


        public ActionResult ExcluirTarefa(string IdTarefa)
        {
            var VerificaDados = new QueryMysqlParametros();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                VerificaDados.ExcluirTarefa(IdTarefa);
                return RedirectToAction("Parametros", "Parametros",
                    new { mensagemValidacao = "Tarefa excluida com sucesso !" });
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult ExcluirSubtarefa(string IdSubtarefa)
        {
            var VerificaDados = new QueryMysqlParametros();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                VerificaDados.ExcluirSubtarefa(IdSubtarefa);
                return RedirectToAction("Parametros", "Parametros",
                    new { mensagemValidacao = "Tarefa excluída com sucesso !" });
            }

            return RedirectToAction("Login", "Login");
        }


    }
}