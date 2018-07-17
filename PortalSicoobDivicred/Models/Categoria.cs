using System.ComponentModel.DataAnnotations;

namespace PortalSicoobDivicred.Models
{
    public class Categoria
    {
        [Required(ErrorMessage = "Favor inserir uma descrição para está categoria!")]
        public string DescricaoCategoria { get; set; }

        [Required(ErrorMessage = "Favor inserir um tempo de solução para está categoria!")]
        public string TempoCategoria { get; set; }
    }
}