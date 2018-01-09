using System.ComponentModel.DataAnnotations;

namespace PortalSicoobDivicred.Models
{
    public class ParecerProcesso
    {
        [Required(ErrorMessage = "Favor inserir quem solicitou!")]
        public string Solicitante { get; set; }

        [Required(ErrorMessage = "Favor inserir a metodologia utilizada!")]
        public string MetodologiaProcesso { get; set; }

        public string DemandaParecer { get; set; }

        [Required(ErrorMessage = "Favor inserir qual tipo de recrutamento foi utilizado!")]
        public string TipoRecrutamento { get; set; }

        [Required(ErrorMessage = "Favor inserir o perfil técnico ou psicológico!")]
        public string PerfilTecnico { get; set; }

        [Required(ErrorMessage = "Favor inserir a conclusao!")]
        public string Conclusao { get; set; }
    }
}