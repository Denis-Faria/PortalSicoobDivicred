using System.ComponentModel.DataAnnotations;

namespace PortalSicoobDivicred.Models
{
    public class Certificacao
    {
        [Required(ErrorMessage = "Favor inserir um nome para esta função!")]
        public string NomeCertificacao { get; set; }
    }
}