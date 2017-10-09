using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PortalSicoobDivicred.Aplicacao;
using PortalSicoobDivicred.Models;

namespace PortalSicoobDivicred.Controllers
{
    public class FormulariosMigController : Controller
    {
        public ActionResult ParecerProcesso(string Cpf, string IdVaga)
        {
            QuerryMysql RecuperaDados = new QuerryMysql();


            var Formulario = RecuperaDados.RecuperaFormularioParecerProcesso(Cpf, IdVaga);
            if (Formulario.Count > 0)
            {
                var DadosPessoais = RecuperaDados.FormularioParecerProcessoDadosPessoais(Cpf, IdVaga);
                var DadosProfissionais = RecuperaDados.FormularioParecerProcessoDadosProfissionais(Cpf);

                var Resultado = new Models.ParecerProcesso();

                Resultado.Conclusao = Formulario[0]["conclusao"];
                Resultado.DemandaParecer= Formulario[0]["demandaparecer"];
                Resultado.MetodologiaProcesso = Formulario[0]["metodologiaprocesso"];
                Resultado.PerfilTecnico = Formulario[0]["perfiltecnico"];
                Resultado.Solicitante = Formulario[0]["solicitante"];
                TempData["ValorTipo"] = Formulario[0]["tiporecrutamento"];



                TempData["Cpf"] = Cpf;
                TempData["IdVaga"] = IdVaga;
                TempData["Atualiza"] = "Atualizar";
                TempData["Remuneracao"]= DadosPessoais[0]["salario"];
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
                for (int i = 0; i < DadosEscolares.Count; i++)
                {
                    TempData["DadosEscolares" + i] =
                        DadosEscolares[i]["nomecurso"] + " - Conclusão:  " + DadosEscolares[i]["anofim"];

                }


                TempData["TotalDadosProfissionais"] = DadosProfissionais.Count;
                for (int j = 0; j < DadosProfissionais.Count; j++)
                {
                    TempData["DadosProfissionais" + j] =
                        DadosProfissionais[j]["nomeempresa"] + " - " + DadosProfissionais[j]["nomecargo"];

                }




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
                for (int i = 0; i < DadosEscolares.Count; i++)
                {
                    TempData["DadosEscolares" + i] =
                        DadosEscolares[i]["nomecurso"] + " - Conclusão:  " + DadosEscolares[i]["anofim"];

                }


                TempData["TotalDadosProfissionais"] = DadosProfissionais.Count;
                for (int j = 0; j < DadosProfissionais.Count; j++)
                {
                    TempData["DadosProfissionais" + j] =
                        DadosProfissionais[j]["nomeempresa"] + " - " + DadosProfissionais[j]["nomecargo"];

                }




                return View();
            }
        }

        [HttpPost]
        public ActionResult ParecerProcesso(ParecerProcesso DadosFormulario, FormCollection Formulario)
        {
            var VerificaDados = new QuerryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                if (ModelState.IsValid)
                {
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
                }
            }
            return RedirectToAction("Login", "Login");
        }











        public ActionResult RecrutamentoSelecao()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RecrutamentoSelecao(FormCollection Formulario)
        {
            return View();
        }


    }
}