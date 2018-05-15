using System.ComponentModel.DataAnnotations;

namespace PortalSicoobDivicred.Models
{
    public class Funcao
    {
        [Required(ErrorMessage = "Favor inserir um nome para esta função!")]
        public string NomeFuncao { get; set; }
    }
}