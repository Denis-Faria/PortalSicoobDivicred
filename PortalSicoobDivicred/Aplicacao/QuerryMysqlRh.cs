using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Port.Repositorios;

namespace PortalSicoobDivicred.Aplicacao
{
    public class QuerryMysqlRh
    {

        private readonly Conexao contexto;


        public QuerryMysqlRh()
        {
            contexto = new Conexao();
        }
        public bool UsuarioLogado()
        {
            var Usuario = HttpContext.Current.Request.Cookies["CookieFarm"];
            if (Usuario == null)
                return false;
            return true;
        }

        public void CadastraVagaInterna(string Titulo,string Descricao,string Requisitos)
        {
            var Querry = "INSERT INTO vagasinternas (titulo,descricao,requisito) VALUES('" +Titulo + "','"+Descricao + "','"+Requisitos+"')";
            contexto.ExecutaComandoComRetorno(Querry);
        }

        public List<Dictionary<string, string>> RetornaVagaInterna()
        {
            var Querry = "SELECT * FROM vagasinternas WHERE encerrada='N'";
            var row =contexto.ExecutaComandoComRetorno(Querry);
            return row;
        }
        public List<Dictionary<string, string>> RetornaVagaInternaTotal()
        {
            var Querry = "SELECT * FROM vagasinternas WHERE encerrada='N'";
            var row = contexto.ExecutaComandoComRetorno(Querry);
            return row;
        }
        public List<Dictionary<string, string>> RetornaInteresseVagaInterna(string IdFuncionario)
        {
            var Querry = "SELECT a.*,b.titulo FROM processosseletivos a, vagasinternas b WHERE a.idvaga=b.id and idfuncionario="+IdFuncionario+"";
            var row = contexto.ExecutaComandoComRetorno(Querry);
            return row;
        }
        public void CadastraInteresse(string IdVaga,string IdFuncionario)
        {
            var Querry = "INSERT INTO processosseletivos (idvaga,idfuncionario) VALUES(" + IdVaga + "," +IdFuncionario+ ")";
            contexto.ExecutaComandoComRetorno(Querry);
        }
    }
}