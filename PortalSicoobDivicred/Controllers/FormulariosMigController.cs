using System;
using System.Web.Mvc;
using PortalSicoobDivicred.Aplicacao;
using PortalSicoobDivicred.Models;

namespace PortalSicoobDivicred.Controllers
{
    public class FormulariosMigController : Controller
    {
        public ActionResult ParecerProcesso(string cpf, string idVaga)
        {
            var recuperaDados = new QueryMysqlCurriculo();


            var formulario = recuperaDados.RecuperaFormularioParecerProcesso(cpf, idVaga);
            if (formulario.Count > 0)
            {
                var dadosPessoais = recuperaDados.FormularioParecerProcessoDadosPessoais(cpf, idVaga);
                var dadosProfissionais = recuperaDados.FormularioParecerProcessoDadosProfissionais(cpf);

                var resultado = new ParecerProcesso();

                resultado.Conclusao = formulario[0]["conclusao"];
                resultado.DemandaParecer = formulario[0]["demandaparecer"];
                resultado.MetodologiaProcesso = formulario[0]["metodologiaprocesso"];
                resultado.PerfilTecnico = formulario[0]["perfiltecnico"];
                resultado.Solicitante = formulario[0]["solicitante"];
                TempData["ValorTipo"] = formulario[0]["tiporecrutamento"];


                TempData["Cpf"] = cpf;
                TempData["IdVaga"] = idVaga;
                TempData["Atualiza"] = "Atualizar";
                TempData["Remuneracao"] = dadosPessoais[0]["salario"];
                TempData["Vaga"] = dadosPessoais[0]["titulo"];
                TempData["Nome"] = dadosPessoais[0]["nome"];
                TempData["Nascimento"] = dadosPessoais[0]["nascimento"];
                TempData["Escolaridade"] = dadosPessoais[0]["escolaridade"];
                var nascimento = Convert.ToDateTime(dadosPessoais[0]["nascimento"]);
                var hoje = DateTime.Now;
                var idade = DateTime.Now.Year - Convert.ToDateTime(dadosPessoais[0]["nascimento"]).Year;
                if (nascimento > hoje.AddYears(-idade)) idade--;
                TempData["Idade"] = idade + " Anos";


                var dadosEscolares = recuperaDados.FormularioParecerProcessoDadosEscolares(cpf);
                TempData["TotalEscolares"] = dadosEscolares.Count;
                for (var i = 0; i < dadosEscolares.Count; i++)
                    TempData["DadosEscolares" + i] =
                        dadosEscolares[i]["nomecurso"] + " - Conclusão:  " + dadosEscolares[i]["anofim"];


                TempData["TotalDadosProfissionais"] = dadosProfissionais.Count;
                for (var j = 0; j < dadosProfissionais.Count; j++)
                    TempData["DadosProfissionais" + j] =
                        dadosProfissionais[j]["nomeempresa"] + " - " + dadosProfissionais[j]["nomecargo"];


                return View(resultado);
            }
            else
            {
                var dadosPessoais = recuperaDados.FormularioParecerProcessoDadosPessoais(cpf, idVaga);
                var dadosProfissionais = recuperaDados.FormularioParecerProcessoDadosProfissionais(cpf);
                TempData["Atualiza"] = "Novo";
                TempData["ValorTipo"] = "Interno";
                TempData["Cpf"] = cpf;
                TempData["Remuneracao"] = dadosPessoais[0]["salario"];
                TempData["IdVaga"] = idVaga;
                TempData["Vaga"] = dadosPessoais[0]["titulo"];
                TempData["Nome"] = dadosPessoais[0]["nome"];
                TempData["Nascimento"] = dadosPessoais[0]["nascimento"];
                TempData["Escolaridade"] = dadosPessoais[0]["escolaridade"];
                var nascimento = Convert.ToDateTime(dadosPessoais[0]["nascimento"]);
                var hoje = DateTime.Now;
                var idade = DateTime.Now.Year - Convert.ToDateTime(dadosPessoais[0]["nascimento"]).Year;
                if (nascimento > hoje.AddYears(-idade)) idade--;
                TempData["Idade"] = idade + " Anos";


                var dadosEscolares = recuperaDados.FormularioParecerProcessoDadosEscolares(cpf);
                TempData["TotalEscolares"] = dadosEscolares.Count;
                for (var i = 0; i < dadosEscolares.Count; i++)
                    TempData["DadosEscolares" + i] =
                        dadosEscolares[i]["nomecurso"] + " - Conclusão:  " + dadosEscolares[i]["anofim"];


                TempData["TotalDadosProfissionais"] = dadosProfissionais.Count;
                for (var j = 0; j < dadosProfissionais.Count; j++)
                    TempData["DadosProfissionais" + j] =
                        dadosProfissionais[j]["nomeempresa"] + " - " + dadosProfissionais[j]["nomecargo"];


                return View();
            }
        }

        [HttpPost]
        public ActionResult ParecerProcesso(ParecerProcesso dadosFormulario, FormCollection formulario)
        {
            var verificaDados = new QueryMysqlCurriculo();
            var logado = verificaDados.UsuarioLogado();
            if (logado)
                if (ModelState.IsValid)
                    if (!formulario["Atualiza"].Equals("Atualizar"))
                    {
                        verificaDados.InserirFormularioParecerProcesso(dadosFormulario.Solicitante,
                            dadosFormulario.MetodologiaProcesso, dadosFormulario.DemandaParecer,
                            formulario["TipoRecrutamento"], dadosFormulario.PerfilTecnico, dadosFormulario.Conclusao,
                            formulario["Cpf"], formulario["IdVaga"]);
                        return RedirectToAction("ParecerProcesso",
                            new {Cpf = formulario["Cpf"], IdVaga = formulario["IdVaga"]});
                    }
                    else
                    {
                        verificaDados.AtualizarFormularioParecerProcesso(dadosFormulario.Solicitante,
                            dadosFormulario.MetodologiaProcesso, dadosFormulario.DemandaParecer,
                            formulario["TipoRecrutamento"], dadosFormulario.PerfilTecnico,
                            dadosFormulario.Conclusao, formulario["Cpf"], formulario["IdVaga"]);
                        return RedirectToAction("ParecerProcesso",
                            new {Cpf = formulario["Cpf"], IdVaga = formulario["IdVaga"]});
                    }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult RecrutamentoSelecao(string idVaga)
        {
            var formulario = new QueryMysqlCurriculo();

            var logado = formulario.UsuarioLogado();
            if (logado)
            {
                var existe = formulario.ExisteFormularioRecrutamentoSelecaoProcesso(idVaga);
                if (existe[0]["count"].Equals("1"))
                {
                    var dados = new RecrutamentoSelecao();
                    var dadosVaga = formulario.FormularioRecrutamentoSelecaoVaga(idVaga);
                    var dadosHistorico = formulario.FormularioRecrutamentoSelecaoHistorico(idVaga);
                    var dadosProcesso = formulario.FormularioRecrutamentoSelecaoProcesso(idVaga);
                    var formularioCompleto = formulario.FormularioRecrutamentoSelecao(idVaga);

                    dados.ClasseCargo = formularioCompleto[0]["classecargo"];
                    dados.NivelCargo = formularioCompleto[0]["nivelcargo"];
                    dados.NumeroVaga = formularioCompleto[0]["numerovaga"];
                    dados.ChefiaImediata = formularioCompleto[0]["chefiaimediata"];
                    dados.MesAdmissao = formularioCompleto[0]["mesadmissao"];
                    dados.DinamicaNumero = formularioCompleto[0]["dinamicanumero"];
                    dados.DinamicaNumeroPreSelecionado = formularioCompleto[0]["dinamicaprenumero"];
                    dados.ConhecimentoTeste = formularioCompleto[0]["conhecimentoteste"];
                    dados.ConhecimentoNumero = formularioCompleto[0]["conhecimentonumero"];
                    dados.ConhecimentoNumeroPreSelecionado = formularioCompleto[0]["conhecimentoprenumero"];
                    dados.PsicologicaTeste = formularioCompleto[0]["psicologicoteste"];
                    dados.PsicologicaNumero = formularioCompleto[0]["psicologiconumero"];
                    dados.PsicologicaNumeroPreSelecionado = formularioCompleto[0]["psicologicoprenumero"];
                    dados.NomeEntrevistador = formularioCompleto[0]["psicologaentrevistador"];
                    dados.EntrevistaNumero = formularioCompleto[0]["psicologanumero"];
                    dados.EntrevistaNumeroPreSelecionado = formularioCompleto[0]["psicologaprenumero"];
                    dados.Setor = formularioCompleto[0]["setor"];

                    TempData["NomeEmpregado"] = formularioCompleto[0]["nomeempregadosubstituido"];
                    TempData["Dinamica"] = formularioCompleto[0]["dinamicagrupo"];
                    TempData["TipoSelecao"] = formularioCompleto[0]["motivoselecao"];
                    TempData["FormaRecrutamento"] = formularioCompleto[0]["formarecrutamento"];
                    TempData["PrevisaoOrcamento"] = formularioCompleto[0]["previsaoorcamento"];

                    TempData["IdVaga"] = idVaga;
                    TempData["Vaga"] = dadosVaga[0]["titulo"];
                    TempData["Solicitacao"] = dadosVaga[0]["datainicio"];
                    TempData["TenhoInteresse"] = dadosHistorico[0]["count"];
                    TempData["ProcessoSeletivo"] = dadosProcesso[0]["count"];
                    TempData["Atualiza"] = "Atualizar";
                    return View(dados);
                }
                else
                {
                    var dadosVaga = formulario.FormularioRecrutamentoSelecaoVaga(idVaga);
                    var dadosHistorico = formulario.FormularioRecrutamentoSelecaoHistorico(idVaga);
                    var dadosProcesso = formulario.FormularioRecrutamentoSelecaoProcesso(idVaga);
                    TempData["IdVaga"] = idVaga;
                    TempData["Vaga"] = dadosVaga[0]["titulo"];
                    TempData["Solicitacao"] = dadosVaga[0]["datainicio"];
                    TempData["TenhoInteresse"] = dadosHistorico[0]["count"];
                    TempData["ProcessoSeletivo"] = dadosProcesso[0]["count"];
                    TempData["Atualiza"] = "";
                    return View();
                }
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult RecrutamentoSelecao(RecrutamentoSelecao dadosFormulario, FormCollection formulario)
        {
            var verificaDados = new QueryMysqlCurriculo();
            var logado = verificaDados.UsuarioLogado();
            if (logado)
                if (ModelState.IsValid)
                    if (!formulario["Atualiza"].Equals("Atualizar"))
                    {
                        verificaDados.InserirFormularioRecrutamentoSelecao(formulario["IdVaga"],
                            dadosFormulario.ClasseCargo, dadosFormulario.NivelCargo, dadosFormulario.NumeroVaga,
                            dadosFormulario.ChefiaImediata, dadosFormulario.MesAdmissao, formulario["MotivoSelecao"],
                            formulario["EmpregadoSubstituido"], formulario["PrevisaoOrcamento"],
                            formulario["TipoRecrutamento"], formulario["Dinamica"],
                            dadosFormulario.DinamicaNumero, dadosFormulario.DinamicaNumeroPreSelecionado,
                            dadosFormulario.ConhecimentoTeste, dadosFormulario.ConhecimentoNumero,
                            dadosFormulario.ConhecimentoNumeroPreSelecionado, dadosFormulario.PsicologicaTeste,
                            dadosFormulario.PsicologicaNumero, dadosFormulario.PsicologicaNumeroPreSelecionado,
                            dadosFormulario.NomeEntrevistador, dadosFormulario.EntrevistaNumero,
                            dadosFormulario.EntrevistaNumeroPreSelecionado, dadosFormulario.Setor);
                        return RedirectToAction("RecrutamentoSelecao",
                            new {IdVaga = formulario["IdVaga"]});
                    }
                    else
                    {
                        verificaDados.AtualizarFormularioRecrutamentoSelecao(formulario["IdVaga"],
                            dadosFormulario.ClasseCargo, dadosFormulario.NivelCargo, dadosFormulario.NumeroVaga,
                            dadosFormulario.ChefiaImediata, dadosFormulario.MesAdmissao, formulario["MotivoSelecao"],
                            formulario["EmpregadoSubstituido"], formulario["PrevisaoOrcamento"],
                            formulario["TipoRecrutamento"], formulario["Dinamica"],
                            dadosFormulario.DinamicaNumero, dadosFormulario.DinamicaNumeroPreSelecionado,
                            dadosFormulario.ConhecimentoTeste, dadosFormulario.ConhecimentoNumero,
                            dadosFormulario.ConhecimentoNumeroPreSelecionado, dadosFormulario.PsicologicaTeste,
                            dadosFormulario.PsicologicaNumero, dadosFormulario.PsicologicaNumeroPreSelecionado,
                            dadosFormulario.NomeEntrevistador, dadosFormulario.EntrevistaNumero,
                            dadosFormulario.EntrevistaNumeroPreSelecionado, dadosFormulario.Setor);
                        return RedirectToAction("RecrutamentoSelecao",
                            new {IdVaga = formulario["IdVaga"]});
                    }

            return RedirectToAction("Login", "Login");
        }
    }
}