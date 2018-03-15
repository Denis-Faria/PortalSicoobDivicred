﻿using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Limilabs.Client.SMTP;
using Limilabs.Mail;
using Limilabs.Mail.Headers;
using PortalSicoobDivicred.Aplicacao;
using PortalSicoobDivicred.Models;




namespace PortalSicoobDivicred.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Login(string Mensagem)
        {
            TempData["Mensagem"] = Mensagem;
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login Dados)
        {
            var Confere = new QueryMysql();

            if (ModelState.IsValid)
            {
                var Confirma = Confere.ConfirmaLogin(Dados.Usuario, Dados.Senha);
                var TrocaSenha = Confere.TrocaSenha(Dados.Usuario);
                if (TrocaSenha)
                {
                    if (Confirma)
                    {
                        Session["Usuario"] = Dados.Usuario;
                        return RedirectToAction("Principal", "Principal");

                    }
                    else
                    {
                        TempData["Erro"] = "Usuário ou senha informados incorretos.";
                        return View(Dados);
                    }
                }
                else
                {
                    return RedirectToAction("EsqueciMinhaSenha");
                }

                
            }

            if (ModelState["Usuario"].Errors.Count == 0)
                TempData["Erro"] = ModelState["Senha"].Errors[0].ErrorMessage;
            else if (ModelState["Senha"].Errors.Count == 0)
                TempData["Erro"] = ModelState["Usuario"].Errors[0].ErrorMessage;
            else
                TempData["Erro"] = ModelState["Usuario"].Errors[0].ErrorMessage + "\\n" +
                                   ModelState["Senha"].Errors[0].ErrorMessage;
            return View();
        }

        public ActionResult Logoff()
        {
            if (Request.Cookies["CookieFarm"] != null)
            {
                var Cookie = new HttpCookie("CookieFarm");
                Cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(Cookie);
            }
            return RedirectToAction("Login");
        }

        public ActionResult EsqueciMinhaSenha()
        {
            return View("EsqueciMinhaSenha");
        }

        [HttpPost]
        public ActionResult EsqueciMinhaSenha(FormCollection DadosLogin)
        {
            if (ModelState.IsValid)
            {
                var VerificaDados = new QueryMysql();

                string password = DadosLogin["Senha"];
                var DadosUsuario = VerificaDados.RecuperaDadosUsuarios(DadosLogin["Usuario"]);

                MD5 md5Hash = MD5.Create();
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                MailBuilder builder = new MailBuilder();
                builder.From.Add(new MailBox("correio@divicred.com.br", "Portal Sicoob Divicred - Troca de Senha"));
                builder.To.Add(new MailBox(DadosUsuario[0]["email"]));
                builder.Subject = "Troca de senha";
                builder.Html = "<b>Você solicitou uma troca de sneha para confirmar esta alteração clique no link abaixo.</b> <br/>  http://localhost:56657/Login/AlterarSenha?Senha="+ sBuilder.ToString() + "&Usuario="+DadosLogin["Usuario"]+"";

                IMail email = builder.Create();

                using (Smtp smtp = new Smtp())
                {
                    smtp.Connect("mail.divicred.com.br",587);
                    smtp.UseBestLogin("correio@divicred.com.br", "DIVICRED@4030");

                    ISendMessageResult result = smtp.SendMessage(email);
                    if (result.Status == SendMessageStatus.Success)
                    {
                       
                    }

                    smtp.Close();
                }
                VerificaDados.AtualizaEmailSenha(DadosUsuario[0]["id"]);

                return RedirectToAction("Login","Login", new { Mensagem = "Um e-mail foi enviado para você com maiores informações! " });
            }
            else
            {
                TempData["Erro"] = "Usuário ou senha informados incorretos.";
                return View(DadosLogin);
            }
        }

        public ActionResult AlterarSenha(string Senha, string Usuario)
        {

            var VerificaDados = new QueryMysql();
            var Dados = VerificaDados.RetornaEmailSenha(Usuario);

            if (Dados[0]["emailtrocasenha"].ToString().Equals("S"))
            {
                VerificaDados.AtualizaSenha(Usuario,Senha);
                return RedirectToAction("Login","Login",new {Mensagem="Senha alterada com sucesso !"} );
            }
            else
            {
                return RedirectToAction("Login", "Login", new { Mensagem = "Este link não está mais ativo, favor pedir nova alteração de senha!" });
            }
        }
    }
}