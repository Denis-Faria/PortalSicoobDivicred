using System;

namespace PortalSicoobDivicred.Models
{
    public class Tesouraria
    {
        public double ValorJudicial { get; set; }
        public double Valor6192 { get; set; }
        public double ValorCampoNr1 { get; set; }
        public double ValorCampoNr2 { get; set; }
        public double ValorCampoNr3 { get; set; }
        public double ValorCampoNr4 { get; set; }
        public string Justificativa { get; set; }
        public DateTime Data { get; set; }
        public string Historico { get; set; }
        public string Extrato { get; set; }
        public string Arquivos { get; set; }

        public string Diferenca { get; set; }
        //public string justifica { get; set; }
    }
}