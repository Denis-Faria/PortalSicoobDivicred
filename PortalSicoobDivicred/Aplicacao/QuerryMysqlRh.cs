using System;
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
                "select a.id,b.id as idvaga, a.login, a.nome,a.foto,a.idpa, b.titulo,b.descricao,b.encerrada,d.descricao as setorfuncionario, c.aprovado,c.observacao from funcionarios a,vagasinternas b, processosseletivos c, setores d where a.idsetor=d.id AND a.id=c.idfuncionario and b.id=c.idvaga and c.idvaga=" +
                IdVaga + "";
            var row = contexto.ExecutaComandoComRetorno(Query);
            return row;
        }

        public void AtualizaStatus(string IdVaga, string IdFuncionario, bool Aprovado, string Observacao)
        {
            var Query = "UPDATE processosseletivos set aprovado=" + Aprovado + ", observacao='" + Observacao +
                        "' WHERE idvaga=" + IdVaga +
                        " AND idfuncionario=" + IdFuncionario + "";
            contexto.ExecutaComandoComRetorno(Query);
        }

        public void EncerraVaga(string IdVaga)
        {
            var Query = "UPDATE vagasinternas SET encerrada='S' WHERE id=" + IdVaga + ";";
            contexto.ExecutaComandoComRetorno(Query);
        }

        public string InserirHistoricoJustificativa(string IdFuncionario)
        {
            var Query =
                "INSERT INTO historicosjustificativaspontos (validacaogestor,validacaorh,idfuncionario) VALUES('N','N'," +
                IdFuncionario + ")";
            var IdHistorico = contexto.ExecutaComandoComRetornoId(Query);
            return IdHistorico;
        }

        public void InserirHistoricoHorario(string IdHistorico, TimeSpan Horario, string IdFuncionario,
            string IdJustificativa, DateTime Data)
        {
            var Query =
                "INSERT INTO historicoshorariosponto (idhistorico,horario,idfuncionariofirebird,idjustificativafirebird,data) VALUES(" +
                IdHistorico + ",'" + Horario + "'," + IdFuncionario + "," + IdJustificativa + ",'" +
                Data.Date.ToString("yyyy/MM/dd") + "')";
            contexto.ExecutaComandoComRetorno(Query);
        }

        public List<Dictionary<string, string>> RetornaIdPendenciasNaoJustificada(string IdFuncionario)
        {
            var Query = "select id,validacaogestor from historicosjustificativaspontos where idfuncionario=" +
                        IdFuncionario + " and validacaorh='N';";
            var DadosJustificativas = contexto.ExecutaComandoComRetorno(Query);
            return DadosJustificativas;
        }

        public List<Dictionary<string, string>> RetornaPendenciasNaoJustificada(string IdHistorico)
        {
            var Query =
                "select idhistorico,horario,data,idjustificativafirebird from historicoshorariosponto  where idhistorico=" +
                IdHistorico + ";";
            var DadosJustificativas = contexto.ExecutaComandoComRetorno(Query);
            return DadosJustificativas;
        }

        public bool VerificaPendencia(string IdFuncionarioFireBird)
        {
            var Query =
                "select count(a.id) from historicosjustificativaspontos a, historicoshorariosponto b where b.idhistorico=a.id and b.idfuncionariofirebird=" +
                IdFuncionarioFireBird + " and a.validacaorh='N'";
            var DadosJustificativas = contexto.ExecutaComandoComRetorno(Query);

            if (DadosJustificativas.Count > 0)
                return false;
            return true;
        }
    }
}