using System;
using System.Security.Cryptography;
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
        public ActionResult Login(string mensagem)
        {
            TempData["Mensagem"] = mensagem;
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login dados)
        {
            var confere = new QueryMysql();

            if (ModelState.IsValid)
            {
                var confirma = confere.ConfirmaLogin(dados.Usuario, dados.Senha);
                var trocaSenha = confere.TrocaSenha(dados.Usuario);
                if (trocaSenha)
                {
                    if (confirma)
                    {
                        Session["Usuario"] = dados.Usuario;
                        return RedirectToAction("Principal", "Principal");
                    }

                    TempData["Erro"] = "Usuário ou senha informados incorretos.";
                    return View(dados);
                }

                return RedirectToAction("EsqueciMinhaSenha");
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
                var cookie = new HttpCookie("CookieFarm");
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }

            return RedirectToAction("Login");
        }

        public ActionResult EsqueciMinhaSenha()
        {
            return View("EsqueciMinhaSenha");
        }

        [HttpPost]
        public ActionResult EsqueciMinhaSenha(FormCollection dadosLogin)
        {
            if (ModelState.IsValid)
            {
                var verificaDados = new QueryMysql();

                var password = dadosLogin["Senha"];
                var dadosUsuario = verificaDados.RecuperaDadosUsuarios(dadosLogin["Usuario"]);

                var md5Hash = MD5.Create();
                var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                var sBuilder = new StringBuilder();
                for (var i = 0; i < data.Length; i++) sBuilder.Append(data[i].ToString("x2"));

                var builder = new MailBuilder();
                builder.From.Add(new MailBox("correio@divicred.com.br", "Portal Sicoob Divicred - Troca de Senha"));
                builder.To.Add(new MailBox(dadosUsuario[0]["email"]));
                builder.Subject = "Troca de senha";
                builder.Html =
                    "<b>Você solicitou uma troca de sneha para confirmar esta alteração clique no link abaixo.</b> <br/>  http://10.11.17.30:9090/Login/AlterarSenha?Senha=" +
                    sBuilder + "&Usuario=" + dadosLogin["Usuario"] + "";

                var email = builder.Create();

                using (var smtp = new Smtp())
                {
                    smtp.Connect("mail.divicred.com.br", 587);
                    smtp.UseBestLogin("correio@divicred.com.br", "DIVICRED@4030");

                    var result = smtp.SendMessage(email);
                    if (result.Status == SendMessageStatus.Success)
                    {
                    }

                    smtp.Close();
                }

                verificaDados.AtualizaEmailSenha(dadosUsuario[0]["id"]);

                return RedirectToAction("Login", "Login",
                    new {Mensagem = "Um e-mail foi enviado para você com maiores informações! "});
            }

            TempData["Erro"] = "Usuário ou senha informados incorretos.";
            return View("Login");
        }

        public ActionResult AlterarSenha(string senha, string usuario)
        {
            var verificaDados = new QueryMysql();
            var dados = verificaDados.RetornaEmailSenha(usuario);

            if (dados[0]["emailtrocasenha"].Equals("S"))
            {
                verificaDados.AtualizaSenha(usuario, senha);
                return RedirectToAction("Login", "Login", new {Mensagem = "Senha alterada com sucesso !"});
            }

            return RedirectToAction("Login", "Login",
                new {Mensagem = "Este link não está mais ativo, favor pedir nova alteração de senha!"});
        }
    }
}