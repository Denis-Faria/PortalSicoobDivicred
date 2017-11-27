﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mail;
using System.Web.Mvc;
using PortalSicoobDivicred.Aplicacao;
using PortalSicoobDivicred.Models;
using ElasticEmailClient;
using Microsoft.SqlServer.Server;
using OneApi.Client.Impl;
using OneApi.Config;
using OneApi.Model;
using RestSharp;

namespace PortalSicoobDivicred.Controllers
{
    public class PrincipalController : Controller
    {
        // GET: Principal
        public ActionResult Principal(string Acao, string Mensagem,string Controlle)
        {
            var VerificaDados = new QuerryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            try
            {
                if (!Acao.Equals("Dashboard"))

                {
                    TempData["Opcao"] = Acao;
                    TempData["Mensagem"] = Mensagem;
                    TempData["Controlle"] = Controlle;
                    ModelState.Clear();
                }
                else
                {
                    TempData["Opcao"] = "Dashboard";
                    TempData["Controle"] = "Principal";
                }
            }
            catch
            {
                TempData["Opcao"] = "Dashboard";
                TempData["Controle"] = "Principal";
            }
            if (Logado)
            {



                var Cookie = Request.Cookies.Get("CookieFarm");

                var Login = Criptografa.Descriptografar(Cookie.Value);
               if (VerificaDados.PrimeiroLogin(Login))
                {
                    return RedirectToAction("FormularioCadastro", "Principal");

                }
                else
                {
                    var DadosUsuarioBanco = VerificaDados.RecuperaDadosUsuarios(Login);

                    if (VerificaDados.PermissaoCurriculos(DadosUsuarioBanco[0]["login"]))
                    {
                        TempData["PermissaoCurriculo"] =
                            " <a  href='javascript: Curriculo(); void(0); ' class='item' style='color: #38d5c5;'><span class='icon'><i class='fa fa-book'></i></span><span class='name'> Currículo</span></a>";
                    }
                    else
                    {
                        TempData["PermissaoCurriculo"] = "";

                    }
                    TempData["NomeLateral"] = DadosUsuarioBanco[0]["login"];
                    TempData["EmailLateral"] = DadosUsuarioBanco[0]["email"];
                    if (DadosUsuarioBanco[0]["foto"] == null)
                    {
                        TempData["ImagemPerfil"] = "https://docs.google.com/uc?id=0B2CLuTO3N2_obWdkajEzTmpGeU0";
                    }
                    else
                    {
                        TempData["ImagemPerfil"] = DadosUsuarioBanco[0]["foto"];
                    }
                    return View();
                }
            }

            return RedirectToAction("Login", "Login");
            }
        

        public ActionResult Dashboard()
        {
            var VerificaDados = new QuerryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                return PartialView("Dashboard");
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public void SMS()
        {
            string Celular = "(37) 99988-3728";
            var EnvioSms = new string[1];
            var certo = "55" + Celular.Replace(')', ' ').Replace('(', ' ').Replace('-', ' ');
       EnvioSms[0] =certo.Replace(" ","");
        
        Configuration configuration = new Configuration("Divicred", "Euder17!");
        SMSClient smsClient = new SMSClient(configuration);
        SMSRequest smsRequest = new SMSRequest("Portal Sicoob Divicred", "Portal Sicoob Divicred, Novas vagas cadastradas. ", EnvioSms);
        SendMessageResult requestId = smsClient.SmsMessagingClient.SendSMS(smsRequest);


    }

        public ActionResult FormularioCadastro()
        {
            var VerificaDados = new QuerryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Cookie = Request.Cookies.Get("CookieFarm");
                var Login = Criptografa.Descriptografar(Cookie.Value);

                var DadosTabelaUsuario = VerificaDados.RecuperaDadosFuncionariosTabelaUsuario(Login);
                var DadosTabelaFuncionario =VerificaDados.RecuperaDadosFuncionariosTabelaFuncionarios(DadosTabelaUsuario[0]["nome"]);

                Funcionario DadosFuncionario = new Funcionario();
                DadosFuncionario.NomeFuncionario = DadosTabelaFuncionario[0]["nome"];
                DadosFuncionario.CpfFuncionario = DadosTabelaFuncionario[0]["cpf"];
                DadosFuncionario.RgFuncionario = DadosTabelaFuncionario[0]["rg"];
                DadosFuncionario.PisFuncionario = DadosTabelaFuncionario[0]["pis"];
                DadosFuncionario.DataNascimentoFuncionario = Convert.ToDateTime(DadosTabelaFuncionario[0]["datanascimento"]).ToString("dd/MM/yyyy");
                DadosFuncionario.FormacaoAcademica = DadosTabelaFuncionario[0]["formacaoacademica"];
                DadosFuncionario.UsuarioSistema = DadosTabelaUsuario[0]["login"];
                DadosFuncionario.Email = DadosTabelaFuncionario[0]["email"];
                DadosFuncionario.PA = DadosTabelaFuncionario[0]["idpa"];
                DadosFuncionario.Rua = DadosTabelaFuncionario[0]["rua"];
                DadosFuncionario.Numero = DadosTabelaFuncionario[0]["numero"];
                DadosFuncionario.Bairro = DadosTabelaFuncionario[0]["bairro"];
                DadosFuncionario.Cidade = DadosTabelaFuncionario[0]["cidade"];
                DadosFuncionario.QuatidadeFilho = DadosTabelaFuncionario[0]["quantidadefilho"];
                DadosFuncionario.DataNascimentoFilho = DadosTabelaFuncionario[0]["datanascimentofilho"];
                DadosFuncionario.ContatoEmergencia = DadosTabelaFuncionario[0]["contatoemergencia"];
                DadosFuncionario.PrincipaisHobbies = DadosTabelaFuncionario[0]["principalhobbie"];
                DadosFuncionario.ComidaFavorita = DadosTabelaFuncionario[0]["comidafavorita"];
                DadosFuncionario.Viagem = DadosTabelaFuncionario[0]["viagem"];

                var EstadoCivil = VerificaDados.RetornaEstadoCivil();
                var Sexo = VerificaDados.RetornaSexo();
                var Etnia = VerificaDados.RetornaEtnia();
                var Formacao = VerificaDados.RetornaFormacao();
                var Setor = VerificaDados.RetornaSetor();
                var Funcao = VerificaDados.RetornaFuncao();

                DadosFuncionario.EstadoCivil = EstadoCivil;
                DadosFuncionario.Sexo = Sexo;
                DadosFuncionario.Etnia = Etnia;
                DadosFuncionario.Formacao = Formacao;
                DadosFuncionario.Setor = Setor;
                DadosFuncionario.Funcao = Funcao;

                TempData["Salario"]= DadosTabelaFuncionario[0]["salariobase"];
                TempData["QuebraCaixa"]= DadosTabelaFuncionario[0]["quebradecaixa"];
                TempData["Anuenio"]= DadosTabelaFuncionario[0]["anuenio"];
                TempData["Ticket"]= DadosTabelaFuncionario[0]["ticket"];

                if (DadosTabelaFuncionario[0]["estagiario"].Equals("S"))
                {
                    TempData["Estagiario"] = "SIM";
                    TempData["DataEstagio"] = DadosTabelaFuncionario[0]["contratoestagio"];
                }
                else
                {
                    TempData["Estagiario"] = "NÃO";
                    TempData["DataEstagio"] = "-";
                }

                return View(DadosFuncionario);
            }
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult FormularioCadastro(Funcionario DadosFuncionario)
        {
            var VerificaDados = new QuerryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {

                if (ModelState.IsValid)
                {
                    var DescricaoSexo = "";
                    var DataNascimentoFilho = "";
                    if (DadosFuncionario.DescricaoSexo==null)
                    {
                        DescricaoSexo = "NÃO INFORMOU";
                    }
                    else
                    {
                        DescricaoSexo = DadosFuncionario.DescricaoSexo;
                    }
                    if (DadosFuncionario.DataNascimentoFilho==null)
                    {
                        DataNascimentoFilho = "NÂO TEM";
                    }
                    else
                    {
                        DataNascimentoFilho = DadosFuncionario.DataNascimentoFilho;
                    }
                    VerificaDados.AtualizaDadosFuncionarioFormulario(DadosFuncionario.NomeFuncionario,
                        DadosFuncionario.CpfFuncionario, DadosFuncionario.RgFuncionario,
                        DadosFuncionario.PisFuncionario, DadosFuncionario.DataNascimentoFuncionario,
                        DadosFuncionario.IdSexo.ToString(), DescricaoSexo, DadosFuncionario.IdEtnia.ToString(),
                        DadosFuncionario.IdEstadoCivil.ToString(), DadosFuncionario.IdFormacao.ToString(), DadosFuncionario.FormacaoAcademica,
                        DadosFuncionario.UsuarioSistema, DadosFuncionario.Email, DadosFuncionario.PA,
                        DadosFuncionario.Rua, DadosFuncionario.Numero, DadosFuncionario.Bairro, DadosFuncionario.Cidade,
                        DadosFuncionario.IdSetor.ToString(), DadosFuncionario.IdFuncao.ToString(), DadosFuncionario.QuatidadeFilho,
                        DataNascimentoFilho, DadosFuncionario.ContatoEmergencia,
                        DadosFuncionario.PrincipaisHobbies, DadosFuncionario.ComidaFavorita, DadosFuncionario.Viagem);
                    return RedirectToAction("Principal", "Principal");
                }
            }
            return RedirectToAction("Login", "Login");
        }

        public ActionResult BuscaCertificacao(string IdFuncao)
        {
            var VerificaDados = new QuerryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var CertificacoesFuncao = VerificaDados.RetornaCertificacaoFuncao(IdFuncao);
                var IdCertificacoes = CertificacoesFuncao[0]["idcertificacao"].Split(';');

                TempData["TotalCertificacao"] = IdCertificacoes.Length;
                
                for (int j = 0; j < IdCertificacoes.Length; j++)
                {
                    var Certificacoes = VerificaDados.RetornaCertificacao(IdCertificacoes[j]);
                    @TempData["Certificacao" + j] = Certificacoes[0]["descricao"];
                }
                return PartialView("CertificacaoFuncao");
            }
            return RedirectToAction("Login", "Login");
        }





    }
}