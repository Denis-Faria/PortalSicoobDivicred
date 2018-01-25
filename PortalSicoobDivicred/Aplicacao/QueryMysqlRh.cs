using System;
using System.Collections.Generic;
using System.Web;
using Port.Repositorios;

namespace PortalSicoobDivicred.Aplicacao
{
    public class QueryMysqlRh
    {
        private readonly Conexao ConexaoMysql;


        public QueryMysqlRh()
        {
            ConexaoMysql = new Conexao();
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
            var Query = "INSERT INTO vagasinternas (titulo,descricao,requisito) VALUES('" + Titulo + "','" +
                         Descricao + "','" + Requisitos + "')";
            ConexaoMysql.ExecutaComandoComRetorno(Query);
        }

        public void AtualizaVagaInterna(string Titulo, string Descricao, string Requisitos, string IdVaga)
        {
            var Query = "UPDATE vagasinternas SET titulo='" + Titulo + "', descricao='" + Descricao + "',requisito='" +
                        Requisitos + "' WHERE id=" + IdVaga + "";
            ConexaoMysql.ExecutaComandoComRetorno(Query);
        }

        public List<Dictionary<string, string>> RetornaVagaInterna()
        {
            var Query = "SELECT * FROM vagasinternas WHERE encerrada='N'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }

        public List<Dictionary<string, string>> RetornaVagaInternaTotal()
        {
            var Query = "SELECT * FROM vagasinternas";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }

        public List<Dictionary<string, string>> RetornaVaga(string IdVaga)
        {
            var Query = "SELECT * FROM vagasinternas WHERE id=" + IdVaga + "";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }

        public List<Dictionary<string, string>> RetornaInteresseVagaInterna(string IdFuncionario)
        {
            var Query =
                "SELECT a.*,b.titulo FROM processosseletivos a, vagasinternas b WHERE a.idvaga=b.id and idfuncionario=" +
                IdFuncionario + "";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }

        public void CadastraInteresse(string IdVaga, string IdFuncionario)
        {
            var Query = "INSERT INTO processosseletivos (idvaga,idfuncionario) VALUES(" + IdVaga + "," +
                         IdFuncionario + ")";
            ConexaoMysql.ExecutaComandoComRetorno(Query);
        }

        public List<Dictionary<string, string>> RecuperaFuncionariosVaga(string IdVaga)
        {
            var Query =
                "select a.id,b.id as idvaga, a.login, a.nome,a.foto,a.idpa, b.titulo,b.descricao,b.encerrada,d.descricao as setorfuncionario, c.aprovado,c.observacao from funcionarios a,vagasinternas b, processosseletivos c, setores d where a.idsetor=d.id AND a.id=c.idfuncionario and b.id=c.idvaga and c.idvaga=" +
                IdVaga + "";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }

        public void AtualizaStatus(string IdVaga, string IdFuncionario, bool Aprovado, string Observacao)
        {
            var Query = "UPDATE processosseletivos set aprovado=" + Aprovado + ", observacao='" + Observacao +
                        "' WHERE idvaga=" + IdVaga +
                        " AND idfuncionario=" + IdFuncionario + "";
            ConexaoMysql.ExecutaComandoComRetorno(Query);
        }

        public void EncerraVaga(string IdVaga)
        {
            var Query = "UPDATE vagasinternas SET encerrada='S' WHERE id=" + IdVaga + ";";
            ConexaoMysql.ExecutaComandoComRetorno(Query);
        }

        public string InserirHistoricoJustificativa(string IdFuncionario)
        {
            var Query =
                "INSERT INTO historicosjustificativaspontos (validacaogestor,validacaorh,idfuncionario) VALUES('N','N'," +
                IdFuncionario + ")";
            var IdHistorico = ConexaoMysql.ExecutaComandoComRetornoId(Query);
            return IdHistorico;
        }

        public void InserirHistoricoHorario(string IdHistorico, TimeSpan Horario, string IdFuncionario,
            string IdJustificativa, DateTime Data)
        {
            var Query =
                "INSERT INTO historicoshorariosponto (idhistorico,horario,idfuncionariofirebird,idjustificativafirebird,data) VALUES(" +
                IdHistorico + ",'" + Horario + "'," + IdFuncionario + "," + IdJustificativa + ",'" +
                Data.Date.ToString("yyyy/MM/dd") + "')";
            ConexaoMysql.ExecutaComandoComRetorno(Query);
        }

        public List<Dictionary<string, string>> RetornaIdPendenciasNaoJustificada(string IdFuncionario)
        {
            var Query = "select id,validacaogestor from historicosjustificativaspontos where idfuncionario=" +
                        IdFuncionario + " and validacaorh='N';";
            var DadosJustificativas = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return DadosJustificativas;
        }


        public List<Dictionary<string, string>> RetornaPendenciasNaoJustificada(string IdHistorico)
        {
            var Query =
                "select idhistorico,horario,data,idjustificativafirebird,idfuncionariofirebird from historicoshorariosponto  where idhistorico=" +
                IdHistorico + " order by horario asc;";
            var DadosJustificativas = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return DadosJustificativas;
        }

        public bool VerificaPendencia(string IdFuncionarioFireBird)
        {
            var Query =
                "select count(a.id) from historicosjustificativaspontos a, historicoshorariosponto b where b.idhistorico=a.id and b.idfuncionariofirebird=" +
                IdFuncionarioFireBird + " and a.validacaorh='N'";
            var DadosJustificativas = ConexaoMysql.ExecutaComandoComRetorno(Query);

            if (DadosJustificativas.Count > 0)
                return false;
            return true;
        }

        public void InseriJustificativa(string IdHistorico,TimeSpan Horario,string Idfuncionariofirebird,string Idjustificativafirebird,DateTime Data)
        {
            var Query = "INSERT INTO historicoshorariosponto (idhistorico,horario,idfuncionariofirebird,idjustificativafirebird,data) VALUES("+IdHistorico+",'"+Horario+"',"+Idfuncionariofirebird+", "+Idjustificativafirebird+", '"+Data.ToString("yyyy/MM/dd")+"')";
            ConexaoMysql.ExecutaComandoComRetorno(Query);
        }

        public List<Dictionary<string, string>> RetornaPendencias()
        {
            var Query = "select a.id,a.validacaogestor,b.nome from historicosjustificativaspontos a,funcionarios b where a.validacaorh='N' AND a.idfuncionario=b.id;";
            var DadosJustificativas = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return DadosJustificativas;
        }
        public List<Dictionary<string, string>> RetornaDadosPendencias(string IdHistorico)
        {
            var Query = "select * from historicoshorariosponto where idhistorico="+IdHistorico+";";
            var DadosJustificativas = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return DadosJustificativas;
        }
    }
}