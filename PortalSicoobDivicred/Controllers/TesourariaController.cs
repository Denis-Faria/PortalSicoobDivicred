using System.IO;
using System.Web;
using System.Web.Mvc;

namespace PortalSicoobDivicred.Controllers
{
    public class TesourariaController : Controller
    {
        // GET: Tesouraria
        public ActionResult Tesouraria()
        {
            // string teste;
            // teste = TempData["RelRejeitados"].ToString();
            return View("Tesouraria");
        }

        [HttpPost]
        public ActionResult ProcessaArquivos(HttpPostedFileBase file)
        {
            var NomeArquivo = Path.GetFileName(file.FileName);
            var Caminho = Path.Combine(Server.MapPath("~/Uploads/"), NomeArquivo);

            file.SaveAs(Caminho);

            return View("Tesouraria");
        }
    }
}