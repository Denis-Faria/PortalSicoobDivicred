using System;
using System.Collections.Generic;
using System.Web;
using PortalSicoobDivicred.Repositorios;

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

        public string InserirHistoricoJustificativa(string IdFuncionario, DateTime DataPendencia)
        {
            var Query =
                "INSERT INTO historicosjustificativaspontos (validacaogestor,validacaorh,idfuncionario,data) VALUES('N','N'," +
                IdFuncionario + ",'" + DataPendencia.ToString("yyyy/MM/dd") + "')";
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

        public void InseriJustificativa(string IdHistorico, TimeSpan Horario, string Idfuncionariofirebird,
            string Idjustificativafirebird)
        {
            var Query =
                "INSERT INTO historicoshorariosponto (idhistorico,horario,idfuncionariofirebird,idjustificativafirebird) VALUES(" +
                IdHistorico + ",'" + Horario + "'," + Idfuncionariofirebird + ", " + Idjustificativafirebird + ")";
            ConexaoMysql.ExecutaComandoComRetorno(Query);
        }

        public List<Dictionary<string, string>> RetornaPendencias()
        {
            var Query =
                "select a.id,a.validacaogestor,b.nome,a.data from historicosjustificativaspontos a,funcionarios b where a.validacaorh='N' AND a.idfuncionario=b.id;";
            var DadosJustificativas = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return DadosJustificativas;
        }

        public List<Dictionary<string, string>> RetornaDadosPendencias(string IdHistorico)
        {
            var Query = "select * from historicoshorariosponto where idhistorico=" + IdHistorico +
                        " order by horario asc;";
            var DadosJustificativas = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return DadosJustificativas;
        }

        public List<Dictionary<string, string>> RetornaHistoricoPendencias(string DataInicial, string DataFinal)
        {
            var Query =
                "select a.id,a.validacaogestor,b.nome,a.data from historicosjustificativaspontos a,funcionarios b where a.idfuncionario=b.id and data between '" +
                DataInicial + "' AND '" + DataFinal + "' ;";
            var DadosJustificativas = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return DadosJustificativas;
        }

        public List<Dictionary<string, string>> RetornaPendenciasUsuario(string IdUsuario)
        {
            var Query =
                "select a.id,a.validacaogestor,b.nome,a.data from historicosjustificativaspontos a,funcionarios b where a.validacaogestor='N' and a.validacaorh='N' AND a.idfuncionario=b.id AND b.id=" +
                IdUsuario + ";";
            var DadosJustificativas = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return DadosJustificativas;
        }

        public void AtualizaJustificativa(string IdHistorico, string IdJustificativa)
        {
            var Query = "UPDATE historicoshorariosponto  SET idjustificativafirebird=" + IdJustificativa +
                        " WHERE idhistorico=" + IdHistorico + ";";
            ConexaoMysql.ExecutaComandoComRetorno(Query);
        }

        public void AtualizaJustificativaSemFireBird(string IdHistorico, string Observacao)
        {
            var Query = "UPDATE historicoshorariosponto  SET observacao='" + Observacao + "' WHERE idhistorico=" +
                        IdHistorico + ";";
            ConexaoMysql.ExecutaComandoComRetorno(Query);
        }

        public bool ValidaFirebirdMysql(string IdFuncionarioFirebird, DateTime DiaValidar)
        {
            var Query =
                "select count(a.id) as Total FROM historicoshorariosponto a, historicosjustificativaspontos b WHERE a.idhistorico=b.id AND b.data='" +
                DiaValidar.ToString("yyyy/MM/dd") + "' AND a.idfuncionariofirebird=" + IdFuncionarioFirebird +
                " and b.validacaorh='N';";
            var Retorno = ConexaoMysql.ExecutaComandoComRetorno(Query);
            if (Convert.ToInt32(Retorno[0]["Total"]) > 0)
                return true;
            return false;
        }

        public List<Dictionary<string, string>> RetornaPendenciasSetor(string IdSetor)
        {
            var Query =
                "select a.id,a.validacaogestor,b.nome,a.data from historicosjustificativaspontos a,funcionarios b where a.validacaogestor='N' and a.validacaorh='N' AND a.idfuncionario=b.id AND b.idsetor=" +
                IdSetor + ";";
            var DadosJustificativas = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return DadosJustificativas;
        }

        public void AtualizaJustificativaGestor(string idHistorico)
        {
            var Query = "UPDATE historicosjustificativaspontos  SET validacaogestor='S' WHERE id=" + idHistorico + ";";
            ConexaoMysql.ExecutaComandoComRetorno(Query);
        }

        public void NegaJustificativa(string idHistorico)
        {
            var queryObservacao = "SELECT * from historicoshorariosponto  WHERE idhistorico=" + idHistorico +
                                  " group by idhistorico;";
            var dados = ConexaoMysql.ExecutaComandoComRetorno(queryObservacao);
            try
            {
                if (dados[0]["observacao"] == null)
                {
                    var query =
                        "DELETE from historicoshorariosponto  WHERE idjustificativafirebird!=0 AND idhistorico=" +
                        idHistorico + ";";
                    ConexaoMysql.ExecutaComandoComRetorno(query);
                }
                else
                {
                    var query = "UPDATE historicoshorariosponto SET observacao=null  WHERE idhistorico=" +
                                idHistorico + ";";
                    ConexaoMysql.ExecutaComandoComRetorno(query);
                }
            }
            catch
            {
                var query =
                    "DELETE from historicoshorariosponto  WHERE idjustificativafirebird!=0 AND idhistorico=" +
                    idHistorico + ";";
                ConexaoMysql.ExecutaComandoComRetorno(query);
            }
        }

        public void NegaJustificativaGestor(string idHistorico)
        {
            var queryObservacao = "SELECT * from historicoshorariosponto  WHERE idhistorico=" + idHistorico +
                                  " group by idhistorico;";
            var dados = ConexaoMysql.ExecutaComandoComRetorno(queryObservacao);
            try
            {
                if (dados[0]["observacao"] == null)
                {
                    var query = "UPDATE historicosjustificativaspontos set validacaogestor='N'  WHERE id=" +
                                idHistorico +
                                ";";
                    ConexaoMysql.ExecutaComandoComRetorno(query);
                }
                else
                {
                    var query = "UPDATE historicoshorariosponto SET observacao=null  WHERE idhistorico=" +
                                idHistorico + ";";
                    ConexaoMysql.ExecutaComandoComRetorno(query);
                }
            }
            catch
            {
                var query = "UPDATE historicosjustificativaspontos set validacaogestor='N'  WHERE id=" +
                            idHistorico +
                            ";";
                ConexaoMysql.ExecutaComandoComRetorno(query);
            }
        }

        public List<Dictionary<string, string>> RetornaPendenciaAlerta()
        {
            var Query = "SELECT idfuncionario FROM historicosjustificativaspontos WHERE validacaogestor='N'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }

        public void CadastraAlertaJustificativa(string IdFuncionario, string Descricao)
        {
            var Query = "INSERT INTO alertas(idusuario,idaplicativo,data,descricao) VALUES(" + IdFuncionario +
                        ",9,NOW(),'" + Descricao + "')";
            ConexaoMysql.ExecutaComando(Query);
        }

        public List<Dictionary<string, string>> RetornaReincidentes(string DataInicial, string DataFinal)
        {
            var Query =
                "select a.id, a.nome,if(count(b.idfuncionario)>=3,'S','N') as confirma from funcionarios a, historicosjustificativaspontos b,historicoshorariosponto c where a.id=b.idfuncionario and b.data between '" +
                DataInicial + "' and '" + DataFinal +
                "' and b.id=c.idhistorico and c.idjustificativafirebird=6 group by b.idfuncionario; ";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }

        public string RetornaDataReincidentes(string IdFuncionario, string DataInicial, string DataFinal)
        {
            var Query =
                "select a.data from  historicosjustificativaspontos a, historicoshorariosponto b  where a.id=b.idhistorico and b.idjustificativafirebird=6 and  a.data between '" +
                DataInicial + "' and '" + DataFinal + "' and a.idfuncionario=" + IdFuncionario + "; ";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            var Dias = "";
            for (var i = 0; i < Dados.Count; i++)
                Dias = Dias + Convert.ToDateTime(Dados[i]["data"]).Date.ToString("dd/MM/yyyy") + " || ";
            return Dias;
        }

        public List<Dictionary<string, string>> RetornaCertificacoes()
        {
            var Query = "Select id,descricao FROM certificacoesfuncionarios WHERE excluido='N'";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }

        public void CadastrarFuncao(string Nome, string Certificacao)
        {
            var Query = "INSERT INTO funcoes (descricao,idcertificacao) VALUES('" + Nome + "','" + Certificacao + "');";
            ConexaoMysql.ExecutaComando(Query);
        }

        public void EditarFuncao(string IdFuncao, string Nome, string Certificacao)
        {
            var Query = "UPDATE funcoes SET descricao='" + Nome + "', idcertificacao='" + Certificacao + "' WHERE id=" +
                        IdFuncao + ";";
            ConexaoMysql.ExecutaComando(Query);
        }

        public List<Dictionary<string, string>> BuscaFuncao(string Nome)
        {
            var Query = "Select id,descricao from funcoes where descricao like'%" + Nome + "%' AND excluido='N';";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }

        public List<Dictionary<string, string>> RecuperaDadosFuncao(string IdFuncao)
        {
            var Query = "Select id,descricao,idcertificacao from funcoes where id=" + IdFuncao + ";";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }

        public void ExcluirFuncao(string IdFuncao)
        {
            var Query = "UPDATE funcoes SET excluido='S' where id=" + IdFuncao + ";";
            ConexaoMysql.ExecutaComandoComRetorno(Query);
            var Query2 = "UPDATE funcionarios SET funcao=1 WHERE funcao=" + IdFuncao + "";
            ConexaoMysql.ExecutaComando(Query2);
        }

        public List<Dictionary<string, string>> RecuperaDadosPendenciaConfirmacao(string IdPendencia)
        {
            var Query =
                "Select a.*,b.data,b.idfuncionario from historicoshorariosponto a, historicosjustificativaspontos b where b.id=a.idhistorico and a.idhistorico=" +
                IdPendencia + " ORDER BY a.horario DESC;";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }

        public void AtualizaJustificativaRh(string IdHistorico)
        {
            var Query = "UPDATE historicosjustificativaspontos  SET validacaorh='S' WHERE id=" + IdHistorico + ";";
            ConexaoMysql.ExecutaComandoComRetorno(Query);
        }

        public void CadastrarCertificacao(string NomeCertificacao)
        {
            var Query = "INSERT INTO certificacoesfuncionarios (descricao) VALUES('" + NomeCertificacao + "');";
            ConexaoMysql.ExecutaComandoComRetorno(Query);
        }

        public List<Dictionary<string, string>> BuscaCertificacao(string DescricaoCertificacao)
        {
            var Query = "Select id,descricao from certificacoesfuncionarios where descricao like'%" +
                        DescricaoCertificacao + "%' AND excluido='N';";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }

        public List<Dictionary<string, string>> RecuperaDadosCertificacao(string IdCertificacao)
        {
            var Query = "Select id,descricao from certificacoesfuncionarios where id=" + IdCertificacao + ";";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }

        public void ExcluirCertificacao(string IdCertificacao)
        {
            var Query = "UPDATE certificacoesfuncionarios SET excluido='S' where id=" + IdCertificacao + ";";
            ConexaoMysql.ExecutaComandoComRetorno(Query);
        }

        public void EditarCertificacao(string IdCertificacao, string Nome)
        {
            var Query = "UPDATE certificacoesfuncionarios SET descricao='" + Nome + "' WHERE id=" + IdCertificacao +
                        ";";
            ConexaoMysql.ExecutaComando(Query);
        }

        public List<Dictionary<string, string>> RetornaSetores()
        {
            var Query = "select id,descricao from setores WHERE excluido='N';";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }

        public List<Dictionary<string, string>> RetornaFuncionarios()
        {
            var Query = "select id,nome from funcionarios WHERE ativo='S';";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }

        public void CadastrarGestor(string Funcionario, string Setor)
        {
            var Query = "UPDATE funcionarios SET gestor='S' , idsetor=" + Setor + " WHERE id=" + Funcionario + ";";
            ConexaoMysql.ExecutaComando(Query);
        }

        public List<Dictionary<string, string>> BuscaGestor(string DescricaoGestor)
        {
            var Query = "Select id,nome from funcionarios where nome like'%" + DescricaoGestor +
                        "%' AND gestor='S' AND ativo='S';";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }

        public List<Dictionary<string, string>> RecuperaDadosGestor(string IdGestor)
        {
            var Query = "Select id,nome,idsetor from funcionarios where id=" + IdGestor + ";";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }

        public void ExcluirGestor(string IdGestor)
        {
            var Query = "UPDATE funcionarios SET gestor='N' WHERE id=" + IdGestor + ";";
            ConexaoMysql.ExecutaComando(Query);
        }

        public void EditarGestor(string Funcionario, string Setor)
        {
            var Query = "UPDATE funcionarios SET gestor='S' , idsetor=" + Setor + " WHERE id=" + Funcionario + ";";
            ConexaoMysql.ExecutaComando(Query);
        }

        public void CadastrarSetor(string NomeSetor)
        {
            var Query = "INSERT INTO setores (descricao) VALUES('" + NomeSetor + "');";
            ConexaoMysql.ExecutaComandoComRetorno(Query);
        }

        public void EditarSetor(string IdSetor, string Nome)
        {
            var Query = "UPDATE setores SET descricao='" + Nome + "' WHERE id=" + IdSetor + ";";
            ConexaoMysql.ExecutaComando(Query);
        }

        public List<Dictionary<string, string>> BuscaSetor(string DescricaoSetor)
        {
            var Query = "Select id,descricao from setores where descricao like'%" + DescricaoSetor +
                        "%' AND excluido='N';";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }

        public List<Dictionary<string, string>> RecuperaDadosSetor(string IdSetor)
        {
            var Query = "Select id,descricao from setores where id=" + IdSetor + ";";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }

        public void ExcluirSetor(string IdSetor)
        {
            var Query = "UPDATE setores SET excluido='S' where id=" + IdSetor + ";";
            ConexaoMysql.ExecutaComandoComRetorno(Query);
            var Query2 = "UPDATE funcionarios SET IDSETOR=0 WHERE idsetor=" + IdSetor + "";
            ConexaoMysql.ExecutaComando(Query2);
        }

        public List<Dictionary<string, string>> RetornaBancoHoras()
        {
            var Query = "Select * from horasextrasfuncionarios;";
            var Dados = ConexaoMysql.ExecutaComandoComRetorno(Query);
            return Dados;
        }

        public void CadastrarUsuarioPortal(string Nome, string Cpf, string Rg, string DataNascimento, string Rua,
            string Numero, string Bairro, string Cidade, string Usuario, string Pa, string DataAdmissao,
            string VencimentoPeriodico, string Pis, string SalarioUsuario, string QuebraCaixa)
        {
            var Query =
                "INSERT INTO funcionarios (nome,sexo,idpa,admissao,cpf,rg,pis,formacaoacademica,salariobase,quebradecaixa,anuenio,ticket,datanascimento,ativo,rua,numero,bairro,cidade,login,senha,trocasenha) VALUES('" +
                Nome + "',0,'" + Pa + "','" + DataAdmissao + "','" + Cpf + "','" + Rg + "','" + Pis +
                "','NÃO INFORMADO','" + SalarioUsuario + "','" + QuebraCaixa + "','0.00','827.64','" + DataNascimento +
                "','S','" + Rua + "','" + Numero + "','" + Bairro + "','" + Cidade + "','" + Usuario +
                "',MD5('123'),'S')";
            ConexaoMysql.ExecutaComandoComRetorno(Query);
        }
    }
}