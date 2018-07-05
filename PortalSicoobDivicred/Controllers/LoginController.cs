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

                    TempData["Erro"] = "Usuário ou senha informados incorretos.";
                    return View(Dados);
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

                var password = DadosLogin["Senha"];
                var DadosUsuario = VerificaDados.RecuperaDadosUsuarios(DadosLogin["Usuario"]);

                var md5Hash = MD5.Create();
                var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                var sBuilder = new StringBuilder();
                for (var i = 0; i < data.Length; i++) sBuilder.Append(data[i].ToString("x2"));

                var builder = new MailBuilder();
                builder.From.Add(new MailBox("correio@divicred.com.br", "Portal Sicoob Divicred - Troca de Senha"));
                builder.To.Add(new MailBox(DadosUsuario[0]["email"]));
                builder.Subject = "Troca de senha";
                builder.Html =
                    "<b>Você solicitou uma troca de sneha para confirmar esta alteração clique no link abaixo.</b> <br/>  http://10.11.17.30:9090/Login/AlterarSenha?Senha=" +
                    sBuilder + "&Usuario=" + DadosLogin["Usuario"] + "";

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

                VerificaDados.AtualizaEmailSenha(DadosUsuario[0]["id"]);

                return RedirectToAction("Login", "Login",
                    new {Mensagem = "Um e-mail foi enviado para você com maiores informações! "});
            }

            TempData["Erro"] = "Usuário ou senha informados incorretos.";
            return View("Login");
        }

        public ActionResult AlterarSenha(string Senha, string Usuario)
        {
            var VerificaDados = new QueryMysql();
            var Dados = VerificaDados.RetornaEmailSenha(Usuario);

            if (Dados[0]["emailtrocasenha"].Equals("S"))
            {
                VerificaDados.AtualizaSenha(Usuario, Senha);
                return RedirectToAction("Login", "Login", new {Mensagem = "Senha alterada com sucesso !"});
            }

            return RedirectToAction("Login", "Login",
                new {Mensagem = "Este link não está mais ativo, favor pedir nova alteração de senha!"});
        }
    }
}