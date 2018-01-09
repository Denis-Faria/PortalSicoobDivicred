using System.Collections.Generic;
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

        public void CadastraVagaInterna(string Titulo, string Descricao, string Requisitos)
        {
            var Querry = "INSERT INTO vagasinternas (titulo,descricao,requisito) VALUES('" + Titulo + "','" +
                         Descricao + "','" + Requisitos + "')";
            contexto.ExecutaComandoComRetorno(Querry);
        }

        public void AtualizaVagaInterna(string Titulo, string Descricao, string Requisitos, string IdVaga)
        {
            var Query = "UPDATE vagasinternas SET titulo='" + Titulo + "', descricao='" + Descricao + "',requisito='" +
                        Requisitos + "' WHERE id=" + IdVaga + "";
            contexto.ExecutaComandoComRetorno(Query);
        }

        public List<Dictionary<string, string>> RetornaVagaInterna()
        {
            var Querry = "SELECT * FROM vagasinternas WHERE encerrada='N'";
            var row = contexto.ExecutaComandoComRetorno(Querry);
            return row;
        }

        public List<Dictionary<string, string>> RetornaVagaInternaTotal()
        {
            var Querry = "SELECT * FROM vagasinternas";
            var row = contexto.ExecutaComandoComRetorno(Querry);
            return row;
        }

        public List<Dictionary<string, string>> RetornaVaga(string IdVaga)
        {
            var Querry = "SELECT * FROM vagasinternas WHERE id=" + IdVaga + "";
            var row = contexto.ExecutaComandoComRetorno(Querry);
            return row;
        }

        public List<Dictionary<string, string>> RetornaInteresseVagaInterna(string IdFuncionario)
        {
            var Querry =
                "SELECT a.*,b.titulo FROM processosseletivos a, vagasinternas b WHERE a.idvaga=b.id and idfuncionario=" +
                IdFuncionario + "";
            var row = contexto.ExecutaComandoComRetorno(Querry);
            return row;
        }

        public void CadastraInteresse(string IdVaga, string IdFuncionario)
        {
            var Querry = "INSERT INTO processosseletivos (idvaga,idfuncionario) VALUES(" + IdVaga + "," +
                         IdFuncionario + ")";
            contexto.ExecutaComandoComRetorno(Querry);
        }

        public List<Dictionary<string, string>> RecuperaFuncionariosVaga(string IdVaga)
        {
            var Query =
                "select a.id,b.id as idvaga, a.login, a.nome,a.foto,a.idpa, b.titulo,b.descricao,b.encerrada,d.descricao as setorfuncionario, c.aprovado from funcionarios a,vagasinternas b, processosseletivos c, setores d where a.idsetor=d.id AND a.id=c.idfuncionario and b.id=c.idvaga and c.idvaga=" +
                IdVaga + "";
            var row = contexto.ExecutaComandoComRetorno(Query);
            return row;
        }

        public void AtualizaStatus(string IdVaga, string IdFuncionario, bool Aprovado)
        {
            var Query = "UPDATE processosseletivos set aprovado=" + Aprovado + " WHERE idvaga=" + IdVaga +
                        " AND idfuncionario=" + IdFuncionario + "";
            contexto.ExecutaComandoComRetorno(Query);
        }

        public void EncerraVaga(string IdVaga)
        {
            var Query = "UPDATE vagasinternas SET encerrada='S' WHERE id=" + IdVaga + ";";
            contexto.ExecutaComandoComRetorno(Query);
        }
    }
}