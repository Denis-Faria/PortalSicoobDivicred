using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace PortalSicoobDivicred.Models
{
    public class Tesouraria
    {
        public double valorJudicial { get; set; }
        public double valor6192 { get; set; }
        public double valorCampoNr1 { get; set; }
        public double valorCampoNr2 { get; set; }
        public double valorCampoNr3 { get; set; }
        public double valorCampoNr4 { get; set; }
        public string justificativa { get; set; }
        public DateTime data { get; set; }
        public string historico { get; set; }
        public string extrato { get; set; }
        public string arquivos { get; set; }
        public string diferenca { get; set; }
        //public string justifica { get; set; }
    }
}