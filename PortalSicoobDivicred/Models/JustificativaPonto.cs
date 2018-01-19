using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortalSicoobDivicred.Models
{
    public class JustificativaPonto
    {

        [DisplayName("Justificativa")]
        public List<SelectListItem> Justificativa { get; set; }

        [Required(ErrorMessage = "Favor informar uma justificativa!")]
        [DisplayName("Justificativa")]
        public int IdJustificativa { get; set; }

        [DataType(DataType.Time,ErrorMessage = "Favor Informar uma hora válida")]
        [Required(ErrorMessage = "Favor informar uma hora válida")]
        public TimeSpan HoraJustificada { get; set; }


    }
}