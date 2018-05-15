using System.ComponentModel.DataAnnotations;

namespace PortalSicoobDivicred.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Favor inserir um usuário!")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "Favor inserir uma senha!")]
        public string Senha { get; set; }

        public string ConfirmarSenha { get; set; }
    }
}