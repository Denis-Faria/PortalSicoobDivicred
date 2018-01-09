using System;
using System.Web;
using System.Web.Mvc;
using PortalSicoobDivicred.Aplicacao;
using PortalSicoobDivicred.Models;

namespace PortalSicoobDivicred.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login Dados)
        {
            var Confere = new QuerryMysql();

            if (ModelState.IsValid)
            {
                var Confirma = Confere.ConfirmaLogin(Dados.Usuario, Dados.Senha);
                if (Confirma)
                {
                    Session["Usuario"] = Dados.Usuario;
                    return RedirectToAction("Principal", "Principal");
                }

                TempData["Erro"] = "Usuário ou senha informados incorretos.";
                return View(Dados);
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
    }
}