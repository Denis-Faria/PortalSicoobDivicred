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

        public string InserirHistoricoJustificativa(string IdFuncionario,DateTime DataPendencia)
        {
            var Query =
                "INSERT INTO historicosjustificativaspontos (validacaogestor,validacaorh,idfuncionario,data) VALUES('N','N'," +
                IdFuncionario + ",'"+DataPendencia.ToString("yyyy/MM/dd")+"')";
            var IdHistorico = ConexaoMysql.ExecutaComandoComRetornoId(Query);
            return IdHistorico;
        }

        public void InserirHistoricoHorario(string IdHistorico, TimeSpan Horario, string IdFuncionario,
            string IdJustificativa)
        {
            var Query =
                "INSERT INTO historicoshorariosponto (idhistorico,horario,idfuncionariofirebird,idjustificativafirebird) VALUES(" +
                IdHistorico + ",'" + Horario + "'," + IdFuncionario + "," + IdJustificativa + ")";
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

        public void InseriJustificativa(string IdHistorico,TimeSpan Horario,string Idfuncionariofirebird,string Idjustificativafirebird)
        {
            var Query = "INSERT INTO historicoshorariosponto (idhistorico,horario,idfuncionariofirebird,idjustificativafirebird) VALUES("+IdHistorico+",'"+Horario+"',"+Idfuncionariofirebird+", "+Idjustificativafirebird+")";
            ConexaoMysql.ExecutaComandoComRetorno(Query);
        }

        public List<Dictionary<string, string>> RetornaPendencias()
        {
            var Query = "select a.id,a.validacaogestor,b.nome,a.data from historicosjustificativaspontos a,funcionarios b where a.validacaorh='N' AND a.idfuncionario=b.id;";
            var DadosJustificativas = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return DadosJustificativas;
        }
        public List<Dictionary<string, string>> RetornaDadosPendencias(string IdHistorico)
        {
            var Query = "select * from historicoshorariosponto where idhistorico="+IdHistorico+" order by horario asc;";
            var DadosJustificativas = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return DadosJustificativas;
        }
        public List<Dictionary<string, string>> RetornaHistoricoPendencias()
        {
            var Query = "select a.id,a.validacaogestor,b.nome,a.data from historicosjustificativaspontos a,funcionarios b where a.idfuncionario=b.id ;";
            var DadosJustificativas = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return DadosJustificativas;
        }
        public List<Dictionary<string, string>> RetornaPendenciasUsuario(string IdUsuario)
        {
            var Query = "select a.id,a.validacaogestor,b.nome,a.data from historicosjustificativaspontos a,funcionarios b where a.validacaogestor='N' and a.validacaorh='N' AND a.idfuncionario=b.id AND b.id="+IdUsuario+";";
            var DadosJustificativas = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return DadosJustificativas;
        }
        public void AtualizaJustificativa(string IdHistorico,string IdJustificativa )
        {
            var Query = "UPDATE historicoshorariosponto  SET idjustificativafirebird="+IdJustificativa+" WHERE idhistorico=" + IdHistorico + ";";
            ConexaoMysql.ExecutaComandoComRetorno(Query);
            
        }
        public bool ValidaFirebirdMysql(string IdFuncionarioFirebird, DateTime DiaValidar)
        {
            var Query = "select count(a.id) as Total FROM historicoshorariosponto a, historicosjustificativaspontos b WHERE a.idhistorico=b.id AND b.data='"+DiaValidar.ToString("yyyy/MM/dd")+"' AND a.idfuncionariofirebird="+IdFuncionarioFirebird+" and b.validacaorh='N';";
           var Retorno= ConexaoMysql.ExecutaComandoComRetorno(Query);
            if (Convert.ToInt32(Retorno[0]["Total"]) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public List<Dictionary<string, string>> RetornaPendenciasSetor(string IdSetor)
        {
            var Query = "select a.id,a.validacaogestor,b.nome,a.data from historicosjustificativaspontos a,funcionarios b where a.validacaogestor='N' and a.validacaorh='N' AND a.idfuncionario=b.id AND b.idsetor=" + IdSetor + ";";
            var DadosJustificativas = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return DadosJustificativas;
        }

        public void AtualizaJustificativaGestor(string IdHistorico)
        {
            var Query = "UPDATE historicosjustificativaspontos  SET validacaogestor='S' WHERE id=" + IdHistorico + ";";
            ConexaoMysql.ExecutaComandoComRetorno(Query);

        }

        public void NegaJustificativa(string IdHistorico)
        {
            var Query = "DELETE from historicoshorariosponto  WHERE idjustificativafirebird!=0 AND idhistorico=" + IdHistorico + ";";
            ConexaoMysql.ExecutaComandoComRetorno(Query);

        }

        public void NegaJustificativaGestor(string IdHistorico)
        {
            var Query = "UPDATE historicosjustificativaspontos set validacaogestor='N'  WHERE id=" + IdHistorico + ";";
            ConexaoMysql.ExecutaComandoComRetorno(Query);

        }

        public List<Dictionary<string,string>> RetornaPendenciaAlerta()
        {
            var Query = "SELECT idfuncionario FROM historicosjustificativaspontos WHERE validacaogestor='N'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }

        public void CadastraAlertaJustificativa(string IdFuncionario,string Descricao)
        {
            var Query = "INSERT INTO alertas(idusuario,idaplicativo,data,descricao) VALUES(" + IdFuncionario +
                        ",9,NOW(),'" + Descricao + "')";
            ConexaoMysql.ExecutaComando(Query);
        }
    }
}