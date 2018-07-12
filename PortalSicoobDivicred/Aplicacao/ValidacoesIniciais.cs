using System.Collections.Generic;
using System.Web.Mvc;


namespace PortalSicoobDivicred.Aplicacao
{
    public class ValidacoesIniciais
    {
        QueryMysql _verificaDados = new QueryMysql();

        public void AlertasUsuario(Controller controller, string idUsuario)
        {
       
            var alertas = _verificaDados.BuscaAlerta( idUsuario );
            controller.TempData["TotalAlertas"] = alertas.Count;

            for(int i = 0; i < alertas.Count; i++)
            {
                controller.TempData["Alerta" + i] = alertas[i]["descricao"];

                controller.TempData["IdAlerta" + i] = alertas[i]["id"];
            }

        }

        public void Permissoes(Controller controller, List<Dictionary<string,string>> dadosUsuarioBanco)
        {

            if(_verificaDados.PermissaoCurriculos( dadosUsuarioBanco[0]["login"] ))
                controller.TempData["PermissaoCurriculo"] =
                    " ";
            else
                controller.TempData["PermissaoCurriculo"] = "display: none";

            if(_verificaDados.PermissaoTesouraria( dadosUsuarioBanco[0]["login"] ))
                controller.TempData["PermissaoTesouraria"] =
                    " ";
            else
                controller.TempData["PermissaoTesouraria"] = "display: none";

            if(dadosUsuarioBanco[0]["gestor"].Equals( "S" ))
            {
                controller.TempData["PermissaoGestor"] = "N";
                controller.TempData["AreaGestor"] = "S";
            }
            else
            {
                controller.TempData["PermissaoGestor"] = "N";
                controller.TempData["AreaGestor"] = "N";
            }

            if(_verificaDados.PermissaoControleFuncionario( dadosUsuarioBanco[0]["login"] ))
                controller.TempData["PermissaoNumerario"] =
                    " ";
            else if(_verificaDados.PermissaoControleTesouraria( dadosUsuarioBanco[0]["login"] ))
                controller.TempData["PermissaoNumerario"] =
                    " ";
            else
                controller.TempData["PermissaoNumerario"] = "display: none";
        }

        public void DadosNavBar(Controller controller, List<Dictionary<string, string>> dadosUsuarioBanco)
        {
            controller.TempData["NomeLateral"] = dadosUsuarioBanco[0]["login"];
            controller.TempData["EmailLateral"] = dadosUsuarioBanco[0]["email"];

            if(dadosUsuarioBanco[0]["foto"] == null)
                controller.TempData["ImagemPerfil"] = "https://docs.google.com/uc?id=0B2CLuTO3N2_obWdkajEzTmpGeU0";
            else
                controller.TempData["ImagemPerfil"] = dadosUsuarioBanco[0]["foto"];

        }


    }
}