using System.ComponentModel.DataAnnotations;

namespace PortalSicoobDivicred.Models
{
    public class Setor
    {
        [Required(ErrorMessage = "Favor inserir um nome para este setor!")]
        public string NomeSetor { get; set; }
    }
}