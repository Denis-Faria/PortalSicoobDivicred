using System;
using System.Web.Mvc;
using PortalSicoobDivicred.Aplicacao;
using PortalSicoobDivicred.Models;

namespace PortalSicoobDivicred.Controllers
{
    public class FormulariosMigController : Controller
    {
        public ActionResult ParecerProcesso(string Cpf, string IdVaga)
        {
            var RecuperaDados = new QuerryMysqlCurriculo();


            var Formulario = RecuperaDados.RecuperaFormularioParecerProcesso(Cpf, IdVaga);
            if (Formulario.Count > 0)
            {
                var DadosPessoais = RecuperaDados.FormularioParecerProcessoDadosPessoais(Cpf, IdVaga);
                var DadosProfissionais = RecuperaDados.FormularioParecerProcessoDadosProfissionais(Cpf);

                var Resultado = new ParecerProcesso();

                Resultado.Conclusao = Formulario[0]["conclusao"];
                Resultado.DemandaParecer = Formulario[0]["demandaparecer"];
                Resultado.MetodologiaProcesso = Formulario[0]["metodologiaprocesso"];
                Resultado.PerfilTecnico = Formulario[0]["perfiltecnico"];
                Resultado.Solicitante = Formulario[0]["solicitante"];
                TempData["ValorTipo"] = Formulario[0]["tiporecrutamento"];


                TempData["Cpf"] = Cpf;
                TempData["IdVaga"] = IdVaga;
                TempData["Atualiza"] = "Atualizar";
                TempData["Remuneracao"] = DadosPessoais[0]["salario"];
                TempData["Vaga"] = DadosPessoais[0]["titulo"];
                TempData["Nome"] = DadosPessoais[0]["nome"];
                TempData["Nascimento"] = DadosPessoais[0]["nascimento"];
                TempData["Escolaridade"] = DadosPessoais[0]["escolaridade"];
                var Nascimento = Convert.ToDateTime(DadosPessoais[0]["nascimento"]);
                var Hoje = DateTime.Now;
                var Idade = DateTime.Now.Year - Convert.ToDateTime(DadosPessoais[0]["nascimento"]).Year;
                if (Nascimento > Hoje.AddYears(-Idade)) Idade--;
                TempData["Idade"] = Idade + " Anos";


                var DadosEscolares = RecuperaDados.FormularioParecerProcessoDadosEscolares(Cpf);
                TempData["TotalEscolares"] = DadosEscolares.Count;
                for (var i = 0; i < DadosEscolares.Count; i++)
                    TempData["DadosEscolares" + i] =
                        DadosEscolares[i]["nomecurso"] + " - Conclusão:  " + DadosEscolares[i]["anofim"];


                TempData["TotalDadosProfissionais"] = DadosProfissionais.Count;
                for (var j = 0; j < DadosProfissionais.Count; j++)
                    TempData["DadosProfissionais" + j] =
                        DadosProfissionais[j]["nomeempresa"] + " - " + DadosProfissionais[j]["nomecargo"];


                return View(Resultado);
            }
            else
            {
                var DadosPessoais = RecuperaDados.FormularioParecerProcessoDadosPessoais(Cpf, IdVaga);
                var DadosProfissionais = RecuperaDados.FormularioParecerProcessoDadosProfissionais(Cpf);
                TempData["Atualiza"] = "Novo";
                TempData["ValorTipo"] = "Interno";
                TempData["Cpf"] = Cpf;
                TempData["Remuneracao"] = DadosPessoais[0]["salario"];
                TempData["IdVaga"] = IdVaga;
                TempData["Vaga"] = DadosPessoais[0]["titulo"];
                TempData["Nome"] = DadosPessoais[0]["nome"];
                TempData["Nascimento"] = DadosPessoais[0]["nascimento"];
                TempData["Escolaridade"] = DadosPessoais[0]["escolaridade"];
                var Nascimento = Convert.ToDateTime(DadosPessoais[0]["nascimento"]);
                var Hoje = DateTime.Now;
                var Idade = DateTime.Now.Year - Convert.ToDateTime(DadosPessoais[0]["nascimento"]).Year;
                if (Nascimento > Hoje.AddYears(-Idade)) Idade--;
                TempData["Idade"] = Idade + " Anos";


                var DadosEscolares = RecuperaDados.FormularioParecerProcessoDadosEscolares(Cpf);
                TempData["TotalEscolares"] = DadosEscolares.Count;
                for (var i = 0; i < DadosEscolares.Count; i++)
                    TempData["DadosEscolares" + i] =
                        DadosEscolares[i]["nomecurso"] + " - Conclusão:  " + DadosEscolares[i]["anofim"];


                TempData["TotalDadosProfissionais"] = DadosProfissionais.Count;
                for (var j = 0; j < DadosProfissionais.Count; j++)
                    TempData["DadosProfissionais" + j] =
                        DadosProfissionais[j]["nomeempresa"] + " - " + DadosProfissionais[j]["nomecargo"];


                return View();
            }
        }

        [HttpPost]
        public ActionResult ParecerProcesso(ParecerProcesso DadosFormulario, FormCollection Formulario)
        {
            var VerificaDados = new QuerryMysqlCurriculo();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
                if (ModelState.IsValid)
                    if (!Formulario["Atualiza"].Equals("Atualizar"))
                    {
                        VerificaDados.InserirFormularioParecerProcesso(DadosFormulario.Solicitante,
                            DadosFormulario.MetodologiaProcesso, DadosFormulario.DemandaParecer,
                            Formulario["TipoRecrutamento"], DadosFormulario.PerfilTecnico, DadosFormulario.Conclusao,
                            Formulario["Cpf"], Formulario["IdVaga"]);
                        return RedirectToAction("ParecerProcesso",
                            new {Cpf = Formulario["Cpf"], IdVaga = Formulario["IdVaga"]});
                    }
                    else
                    {
                        VerificaDados.AtualizarFormularioParecerProcesso(DadosFormulario.Solicitante,
                            DadosFormulario.MetodologiaProcesso, DadosFormulario.DemandaParecer,
                            Formulario["TipoRecrutamento"], DadosFormulario.PerfilTecnico,
                            DadosFormulario.Conclusao, Formulario["Cpf"], Formulario["IdVaga"]);
                        return RedirectToAction("ParecerProcesso",
                            new {Cpf = Formulario["Cpf"], IdVaga = Formulario["IdVaga"]});
                    }
            return RedirectToAction("Login", "Login");
        }

        public ActionResult RecrutamentoSelecao(string IdVaga)
        {
            var Formulario = new QuerryMysqlCurriculo();

            var Logado = Formulario.UsuarioLogado();
            if (Logado)
            {
                var Existe = Formulario.ExisteFormularioRecrutamentoSelecaoProcesso(IdVaga);
                if (Existe[0]["count"].Equals(0))
                {
                    var Dados = new RecrutamentoSelecao();
                    var DadosVaga = Formulario.FormularioRecrutamentoSelecaoVaga(IdVaga);
                    var DadosHistorico = Formulario.FormularioRecrutamentoSelecaoHistorico(IdVaga);
                    var DadosProcesso = Formulario.FormularioRecrutamentoSelecaoProcesso(IdVaga);
                    var FormularioCompleto = Formulario.FormularioRecrutamentoSelecao(IdVaga);

                    Dados.ClasseCargo = FormularioCompleto[0]["classecargo"];
                    Dados.NivelCargo = FormularioCompleto[0]["nivelcargo"];
                    Dados.NumeroVaga = FormularioCompleto[0]["numerovaga"];
                    Dados.ChefiaImediata = FormularioCompleto[0]["chefiaimediata"];
                    Dados.MesAdmissao = FormularioCompleto[0]["mesadmissao"];
                    Dados.DinamicaNumero = FormularioCompleto[0]["dinamicanumero"];
                    Dados.DinamicaNumeroPreSelecionado = FormularioCompleto[0]["dinamicaprenumero"];
                    Dados.ConhecimentoTeste = FormularioCompleto[0]["conhecimentoteste"];
                    Dados.ConhecimentoNumero = FormularioCompleto[0]["conhecimentonumero"];
                    Dados.ConhecimentoNumeroPreSelecionado = FormularioCompleto[0]["conhecimentoprenumero"];
                    Dados.PsicologicaTeste = FormularioCompleto[0]["psicologicoteste"];
                    Dados.PsicologicaNumero = FormularioCompleto[0]["psicologiconumero"];
                    Dados.PsicologicaNumeroPreSelecionado = FormularioCompleto[0]["psicologicoprenumero"];
                    Dados.NomeEntrevistador = FormularioCompleto[0]["psicologaentrevistador"];
                    Dados.EntrevistaNumero = FormularioCompleto[0]["psicologanumero"];
                    Dados.EntrevistaNumeroPreSelecionado = FormularioCompleto[0]["psicologaprenumero"];
                    Dados.Setor = FormularioCompleto[0]["setor"];

                    TempData["NomeEmpregado"] = FormularioCompleto[0]["nomeempregadosubstituido"];
                    TempData["Dinamica"] = FormularioCompleto[0]["dinamicagrupo"];
                    TempData["TipoSelecao"] = FormularioCompleto[0]["motivoselecao"];
                    TempData["FormaRecrutamento"] = FormularioCompleto[0]["formarecrutamento"];
                    TempData["PrevisaoOrcamento"] = FormularioCompleto[0]["previsaoorcamento"];

                    TempData["IdVaga"] = IdVaga;
                    TempData["Vaga"] = DadosVaga[0]["titulo"];
                    TempData["Solicitacao"] = DadosVaga[0]["datainicio"];
                    TempData["TenhoInteresse"] = DadosHistorico[0]["count"];
                    TempData["ProcessoSeletivo"] = DadosProcesso[0]["count"];
                    TempData["Atualiza"] = "Atualizar";
                    return View(Dados);
                }
                else
                {
                    var DadosVaga = Formulario.FormularioRecrutamentoSelecaoVaga(IdVaga);
                    var DadosHistorico = Formulario.FormularioRecrutamentoSelecaoHistorico(IdVaga);
                    var DadosProcesso = Formulario.FormularioRecrutamentoSelecaoProcesso(IdVaga);
                    TempData["IdVaga"] = IdVaga;
                    TempData["Vaga"] = DadosVaga[0]["titulo"];
                    TempData["Solicitacao"] = DadosVaga[0]["datainicio"];
                    TempData["TenhoInteresse"] = DadosHistorico[0]["count"];
                    TempData["ProcessoSeletivo"] = DadosProcesso[0]["count"];
                    TempData["Atualiza"] = "";
                    return View();
                }
            }
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult RecrutamentoSelecao(RecrutamentoSelecao DadosFormulario, FormCollection Formulario)
        {
            var VerificaDados = new QuerryMysqlCurriculo();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
                if (ModelState.IsValid)
                    if (!Formulario["Atualiza"].Equals("Atualizar"))
                    {
                        VerificaDados.InserirFormularioRecrutamentoSelecao(Formulario["IdVaga"],
                            DadosFormulario.ClasseCargo, DadosFormulario.NivelCargo, DadosFormulario.NumeroVaga,
                            DadosFormulario.ChefiaImediata, DadosFormulario.MesAdmissao, Formulario["MotivoSelecao"],
                            Formulario["EmpregadoSubstituido"], Formulario["PrevisaoOrcamento"],
                            Formulario["TipoRecrutamento"], Formulario["Dinamica"],
                            DadosFormulario.DinamicaNumero, DadosFormulario.DinamicaNumeroPreSelecionado,
                            DadosFormulario.ConhecimentoTeste, DadosFormulario.ConhecimentoNumero,
                            DadosFormulario.ConhecimentoNumeroPreSelecionado, DadosFormulario.PsicologicaTeste,
                            DadosFormulario.PsicologicaNumero, DadosFormulario.PsicologicaNumeroPreSelecionado,
                            DadosFormulario.NomeEntrevistador, DadosFormulario.EntrevistaNumero,
                            DadosFormulario.EntrevistaNumeroPreSelecionado, DadosFormulario.Setor);
                        return RedirectToAction("RecrutamentoSelecao",
                            new {IdVaga = Formulario["IdVaga"]});
                    }
                    else
                    {
                        VerificaDados.AtualizarFormularioRecrutamentoSelecao(Formulario["IdVaga"],
                            DadosFormulario.ClasseCargo, DadosFormulario.NivelCargo, DadosFormulario.NumeroVaga,
                            DadosFormulario.ChefiaImediata, DadosFormulario.MesAdmissao, Formulario["MotivoSelecao"],
                            Formulario["EmpregadoSubstituido"], Formulario["PrevisaoOrcamento"],
                            Formulario["TipoRecrutamento"], Formulario["Dinamica"],
                            DadosFormulario.DinamicaNumero, DadosFormulario.DinamicaNumeroPreSelecionado,
                            DadosFormulario.ConhecimentoTeste, DadosFormulario.ConhecimentoNumero,
                            DadosFormulario.ConhecimentoNumeroPreSelecionado, DadosFormulario.PsicologicaTeste,
                            DadosFormulario.PsicologicaNumero, DadosFormulario.PsicologicaNumeroPreSelecionado,
                            DadosFormulario.NomeEntrevistador, DadosFormulario.EntrevistaNumero,
                            DadosFormulario.EntrevistaNumeroPreSelecionado, DadosFormulario.Setor);
                        return RedirectToAction("RecrutamentoSelecao",
                            new {IdVaga = Formulario["IdVaga"]});
                    }
            return RedirectToAction("Login", "Login");
        }
    }
}