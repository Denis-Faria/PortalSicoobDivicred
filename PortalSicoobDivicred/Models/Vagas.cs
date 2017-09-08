using System;
using System.ComponentModel.DataAnnotations;

namespace PortalSicoobDivicred.Models
{
    public class Vagas
    {

        [Required(ErrorMessage = "Favor inserir o titulo da vaga!")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "Favor inserir a descrição da vaga!")]
        public string Descricao{ get; set; }

        [Required(ErrorMessage = "Favor inserir os requisitos da vaga!")]
        public string Requisitos { get; set; }

        [Required(ErrorMessage = "Favor inserir o salário da vaga!")]
        public string Salario { get; set; }

        [Required(ErrorMessage = "Favor inserir a data do processo seletivo!")]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "Favor inserir o local do processo seletivo!")]
        public string Local { get; set; }



    }
}