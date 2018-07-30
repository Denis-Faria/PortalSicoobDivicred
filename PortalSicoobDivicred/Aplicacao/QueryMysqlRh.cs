using System;
using System.Collections.Generic;
using System.Web;
using PortalSicoobDivicred.Repositorios;

namespace PortalSicoobDivicred.Aplicacao
{
    public class QueryMysqlRh
    {
        private readonly Conexao _conexaoMysql;


        public QueryMysqlRh()
        {
            _conexaoMysql = new Conexao();
        }

        public bool UsuarioLogado()
        {
            var usuario = HttpContext.Current.Request.Cookies["CookieFarm"];
            if(usuario == null)
                return false;
            return true;
        }

        public void CadastraVagaInterna(string titulo, string descricao, string requisitos)
        {
            var query = "INSERT INTO vagasinternas (titulo,descricao,requisito) VALUES('" + titulo + "','" +
                        descricao + "','" + requisitos + "')";
            _conexaoMysql.ExecutaComandoComRetorno( query );
        }

        public void AtualizaVagaInterna(string titulo, string descricao, string requisitos, string idVaga)
        {
            var query = "UPDATE vagasinternas SET titulo='" + titulo + "', descricao='" + descricao + "',requisito='" +
                        requisitos + "' WHERE id=" + idVaga + "";
            _conexaoMysql.ExecutaComandoComRetorno( query );
        }

        public List<Dictionary<string, string>> RetornaVagaInterna()
        {
            var query = "SELECT * FROM vagasinternas WHERE encerrada='N'";
            var dados = _conexaoMysql.ExecutaComandoComRetorno( query );
            return dados;
        }

        public List<Dictionary<string, string>> RetornaVagaInternaTotal()
        {
            var query = "SELECT * FROM vagasinternas";
            var dados = _conexaoMysql.ExecutaComandoComRetorno( query );
            return dados;
        }

        public List<Dictionary<string, string>> RetornaVaga(string idVaga)
        {
            var query = "SELECT * FROM vagasinternas WHERE id=" + idVaga + "";
            var dados = _conexaoMysql.ExecutaComandoComRetorno( query );
            return dados;
        }

        public List<Dictionary<string, string>> RetornaInteresseVagaInterna(string idFuncionario)
        {
            var query =
                "SELECT a.*,b.titulo FROM processosseletivos a, vagasinternas b WHERE a.idvaga=b.id and idfuncionario=" +
                idFuncionario + "";
            var dados = _conexaoMysql.ExecutaComandoComRetorno( query );
            return dados;
        }

        public void CadastraInteresse(string idVaga, string idFuncionario)
        {
            var query = "INSERT INTO processosseletivos (idvaga,idfuncionario) VALUES(" + idVaga + "," +
                        idFuncionario + ")";
            _conexaoMysql.ExecutaComandoComRetorno( query );
        }

        public List<Dictionary<string, string>> RecuperaFuncionariosVaga(string idVaga)
        {
            var query =
                "select a.id,b.id as idvaga, a.login, a.nome,a.foto,a.idpa, b.titulo,b.descricao,b.encerrada,d.descricao as setorfuncionario, c.aprovado,c.observacao from funcionarios a,vagasinternas b, processosseletivos c, setores d where a.idsetor=d.id AND a.id=c.idfuncionario and b.id=c.idvaga and c.idvaga=" +
                idVaga + "";
            var dados = _conexaoMysql.ExecutaComandoComRetorno( query );
            return dados;
        }

        public void AtualizaStatus(string idVaga, string idFuncionario, bool aprovado, string observacao)
        {
            var query = "UPDATE processosseletivos set aprovado=" + aprovado + ", observacao='" + observacao +
                        "' WHERE idvaga=" + idVaga +
                        " AND idfuncionario=" + idFuncionario + "";
            _conexaoMysql.ExecutaComandoComRetorno( query );
        }

        public void EncerraVaga(string idVaga)
        {
            var query = "UPDATE vagasinternas SET encerrada='S' WHERE id=" + idVaga + ";";
            _conexaoMysql.ExecutaComandoComRetorno( query );
        }

        public string InserirHistoricoJustificativa(string idFuncionario, DateTime dataPendencia)
        {
            var query =
                "INSERT INTO historicosjustificativaspontos (validacaogestor,validacaorh,idfuncionario,data) VALUES('N','N'," +
                idFuncionario + ",'" + dataPendencia.ToString( "yyyy/MM/dd" ) + "')";
            var idHistorico = _conexaoMysql.ExecutaComandoComRetornoId( query );
            return idHistorico;
        }

        public void InserirHistoricoHorario(string idHistorico, TimeSpan horario, string idFuncionario,
            string idJustificativa)
        {
            var query =
                "INSERT INTO historicoshorariosponto (idhistorico,horario,idfuncionariofirebird,idjustificativafirebird) VALUES(" +
                idHistorico + ",'" + horario + "'," + idFuncionario + "," + idJustificativa + ")";
            _conexaoMysql.ExecutaComandoComRetorno( query );
        }

        public List<Dictionary<string, string>> RetornaIdPendenciasNaoJustificada(string idFuncionario)
        {
            var query = "select id,validacaogestor from historicosjustificativaspontos where idfuncionario=" +
                        idFuncionario + " and validacaorh='N';";
            var dadosJustificativas = _conexaoMysql.ExecutaComandoComRetorno( query );
            return dadosJustificativas;
        }


        public List<Dictionary<string, string>> RetornaPendenciasNaoJustificada(string idHistorico)
        {
            var query =
                "select idhistorico,horario,data,idjustificativafirebird,idfuncionariofirebird from historicoshorariosponto  where idhistorico=" +
                idHistorico + " order by horario asc;";
            var dadosJustificativas = _conexaoMysql.ExecutaComandoComRetorno( query );
            return dadosJustificativas;
        }

        public bool VerificaPendencia(string idFuncionarioFireBird)
        {
            var query =
                "select count(a.id) from historicosjustificativaspontos a, historicoshorariosponto b where b.idhistorico=a.id and b.idfuncionariofirebird=" +
                idFuncionarioFireBird + " and a.validacaorh='N'";
            var dadosJustificativas = _conexaoMysql.ExecutaComandoComRetorno( query );

            if(dadosJustificativas.Count > 0)
                return false;
            return true;
        }

        public void InseriJustificativa(string idHistorico, TimeSpan horario, string idfuncionariofirebird,
            string idjustificativafirebird)
        {
            var query =
                "INSERT INTO historicoshorariosponto (idhistorico,horario,idfuncionariofirebird,idjustificativafirebird) VALUES(" +
                idHistorico + ",'" + horario + "'," + idfuncionariofirebird + ", " + idjustificativafirebird + ")";
            _conexaoMysql.ExecutaComandoComRetorno( query );
        }

        public List<Dictionary<string, string>> RetornaPendencias()
        {
            var query =
                "select a.id,a.validacaogestor,b.nome,a.data from historicosjustificativaspontos a,funcionarios b where a.validacaorh='N' AND a.idfuncionario=b.id;";
            var dadosJustificativas = _conexaoMysql.ExecutaComandoComRetorno( query );
            return dadosJustificativas;
        }

        public List<Dictionary<string, string>> RetornaDadosPendencias(string idHistorico)
        {
            var query = "select * from historicoshorariosponto where idhistorico=" + idHistorico +
                        " order by horario asc;";
            var dadosJustificativas = _conexaoMysql.ExecutaComandoComRetorno( query );
            return dadosJustificativas;
        }

        public List<Dictionary<string, string>> RetornaHistoricoPendencias(string dataInicial, string dataFinal)
        {
            var query =
                "select a.id,a.validacaogestor,b.nome,a.data from historicosjustificativaspontos a,funcionarios b where a.idfuncionario=b.id and data between '" +
                dataInicial + "' AND '" + dataFinal + "' ;";
            var dadosJustificativas = _conexaoMysql.ExecutaComandoComRetorno( query );
            return dadosJustificativas;
        }

        public List<Dictionary<string, string>> RetornaPendenciasUsuario(string idUsuario)
        {
            var query =
                "select a.id,a.validacaogestor,b.nome,a.data from historicosjustificativaspontos a,funcionarios b where a.validacaogestor='N' and a.validacaorh='N' AND a.idfuncionario=b.id AND b.id=" +
                idUsuario + ";";
            var dadosJustificativas = _conexaoMysql.ExecutaComandoComRetorno( query );
            return dadosJustificativas;
        }

        public void AtualizaJustificativa(string idHistorico, string idJustificativa)
        {
            var query = "UPDATE historicoshorariosponto  SET idjustificativafirebird=" + idJustificativa +
                        " WHERE idhistorico=" + idHistorico + ";";
            _conexaoMysql.ExecutaComandoComRetorno( query );
        }

        public void AtualizaJustificativaSemFireBird(string idHistorico, string observacao)
        {
            var query = "UPDATE historicoshorariosponto  SET observacao='" + observacao + "' WHERE idhistorico=" +
                        idHistorico + ";";
            _conexaoMysql.ExecutaComandoComRetorno( query );
        }

        public bool ValidaFirebirdMysql(string idFuncionarioFirebird, DateTime diaValidar)
        {
            var query =
                "select count(a.id) as Total FROM historicoshorariosponto a, historicosjustificativaspontos b WHERE a.idhistorico=b.id AND b.data='" +
                diaValidar.ToString( "yyyy/MM/dd" ) + "' AND a.idfuncionariofirebird=" + idFuncionarioFirebird +
                " and b.validacaorh='N';";
            var retorno = _conexaoMysql.ExecutaComandoComRetorno( query );
            if(Convert.ToInt32( retorno[0]["Total"] ) > 0)
                return true;
            return false;
        }

        public List<Dictionary<string, string>> RetornaPendenciasSetor(string idSetor)
        {
            var query =
                "select a.id,a.validacaogestor,b.nome,a.data from historicosjustificativaspontos a,funcionarios b where a.validacaogestor='N' and a.validacaorh='N' AND a.idfuncionario=b.id AND b.idsetor=" +
                idSetor + ";";
            var dadosJustificativas = _conexaoMysql.ExecutaComandoComRetorno( query );
            return dadosJustificativas;
        }

        public void AtualizaJustificativaGestor(string idHistorico)
        {
            var query = "UPDATE historicosjustificativaspontos  SET validacaogestor='S' WHERE id=" + idHistorico + ";";
            _conexaoMysql.ExecutaComandoComRetorno( query );
        }

        public void NegaJustificativa(string idHistorico)
        {
            var queryObservacao = "SELECT * from historicoshorariosponto  WHERE idhistorico=" + idHistorico +
                                  " group by idhistorico;";
            var dados = _conexaoMysql.ExecutaComandoComRetorno( queryObservacao );
            try
            {
                if(dados[0]["observacao"] == null)
                {
                    var query =
                        "DELETE from historicoshorariosponto  WHERE idjustificativafirebird!=0 AND idhistorico=" +
                        idHistorico + ";";
                    _conexaoMysql.ExecutaComandoComRetorno( query );
                }
                else
                {
                    var query = "UPDATE historicoshorariosponto SET observacao=null  WHERE idhistorico=" +
                                idHistorico + ";";
                    _conexaoMysql.ExecutaComandoComRetorno( query );
                }
            }
            catch
            {
                var query =
                    "DELETE from historicoshorariosponto  WHERE idjustificativafirebird!=0 AND idhistorico=" +
                    idHistorico + ";";
                _conexaoMysql.ExecutaComandoComRetorno( query );
            }
        }

        public void NegaJustificativaGestor(string idHistorico)
        {
            var queryObservacao = "SELECT * from historicoshorariosponto  WHERE idhistorico=" + idHistorico +
                                  " group by idhistorico;";
            var dados = _conexaoMysql.ExecutaComandoComRetorno( queryObservacao );
            try
            {
                if(dados[0]["observacao"] == null)
                {
                    var query = "UPDATE historicosjustificativaspontos set validacaogestor='N'  WHERE id=" +
                                idHistorico +
                                ";";
                    _conexaoMysql.ExecutaComandoComRetorno( query );
                }
                else
                {
                    var query = "UPDATE historicoshorariosponto SET observacao=null  WHERE idhistorico=" +
                                idHistorico + ";";
                    _conexaoMysql.ExecutaComandoComRetorno( query );
                }
            }
            catch
            {
                var query = "UPDATE historicosjustificativaspontos set validacaogestor='N'  WHERE id=" +
                            idHistorico +
                            ";";
                _conexaoMysql.ExecutaComandoComRetorno( query );
            }
        }

        public List<Dictionary<string, string>> RetornaPendenciaAlerta()
        {
            var query = "SELECT idfuncionario FROM historicosjustificativaspontos WHERE validacaogestor='N' and validacaorh='N'";
            var dados = _conexaoMysql.ExecutaComandoComRetorno( query );
            return dados;
        }

        public void CadastraAlertaJustificativa(string idFuncionario, string descricao)
        {
            var query = "INSERT INTO alertas(idusuario,idaplicativo,data,descricao) VALUES(" + idFuncionario +
                        ",9,NOW(),'" + descricao + "')";
            _conexaoMysql.ExecutaComando( query );
        }

        public List<Dictionary<string, string>> RetornaReincidentes(string dataInicial, string dataFinal)
        {
            var query =
                "select a.id, a.nome,if(count(b.idfuncionario)>=3,'S','N') as confirma from funcionarios a, historicosjustificativaspontos b,historicoshorariosponto c where a.id=b.idfuncionario and b.data between '" +
                dataInicial + "' and '" + dataFinal +
                "' and b.id=c.idhistorico and c.idjustificativafirebird=6 group by b.idfuncionario; ";
            var dados = _conexaoMysql.ExecutaComandoComRetorno( query );
            return dados;
        }

        public string RetornaDataReincidentes(string idFuncionario, string dataInicial, string dataFinal)
        {
            var query =
                "select a.data from  historicosjustificativaspontos a, historicoshorariosponto b  where a.id=b.idhistorico and b.idjustificativafirebird=6 and  a.data between '" +
                dataInicial + "' and '" + dataFinal + "' and a.idfuncionario=" + idFuncionario + "; ";
            var dados = _conexaoMysql.ExecutaComandoComRetorno( query );
            var dias = "";
            for(var i = 0; i < dados.Count; i++)
                dias = dias + Convert.ToDateTime( dados[i]["data"] ).Date.ToString( "dd/MM/yyyy" ) + " || ";
            return dias;
        }

        public List<Dictionary<string, string>> RetornaCertificacoes()
        {
            var query = "Select id,descricao FROM certificacoesfuncionarios WHERE excluido='N'";
            var dados = _conexaoMysql.ExecutaComandoComRetorno( query );
            return dados;
        }

        public void CadastrarFuncao(string nome, string certificacao)
        {
            var query = "INSERT INTO funcoes (descricao,idcertificacao) VALUES('" + nome + "','" + certificacao + "');";
            _conexaoMysql.ExecutaComando( query );
        }

        public void EditarFuncao(string idFuncao, string nome, string certificacao)
        {
            var query = "UPDATE funcoes SET descricao='" + nome + "', idcertificacao='" + certificacao + "' WHERE id=" +
                        idFuncao + ";";
            _conexaoMysql.ExecutaComando( query );
        }

        public List<Dictionary<string, string>> BuscaFuncao(string nome)
        {
            var query = "Select id,descricao from funcoes where descricao like'%" + nome + "%' AND excluido='N';";
            var dados = _conexaoMysql.ExecutaComandoComRetorno( query );
            return dados;
        }

        public List<Dictionary<string, string>> RecuperaDadosFuncao(string idFuncao)
        {
            var query = "Select id,descricao,idcertificacao from funcoes where id=" + idFuncao + ";";
            var dados = _conexaoMysql.ExecutaComandoComRetorno( query );
            return dados;
        }

        public void ExcluirFuncao(string idFuncao)
        {
            var query = "UPDATE funcoes SET excluido='S' where id=" + idFuncao + ";";
            _conexaoMysql.ExecutaComandoComRetorno( query );
            var query2 = "UPDATE funcionarios SET funcao=1 WHERE funcao=" + idFuncao + "";
            _conexaoMysql.ExecutaComando( query2 );
        }

        public List<Dictionary<string, string>> RecuperaDadosPendenciaConfirmacao(string idPendencia)
        {
            var query =
                "Select a.*,b.data,b.idfuncionario from historicoshorariosponto a, historicosjustificativaspontos b where b.id=a.idhistorico and a.idhistorico=" +
                idPendencia + " ORDER BY a.horario DESC;";
            var dados = _conexaoMysql.ExecutaComandoComRetorno( query );
            return dados;
        }

        public void AtualizaJustificativaRh(string idHistorico)
        {
            var query = "UPDATE historicosjustificativaspontos  SET validacaorh='S' WHERE id=" + idHistorico + ";";
            _conexaoMysql.ExecutaComandoComRetorno( query );
        }

        public void CadastrarCertificacao(string nomeCertificacao)
        {
            var query = "INSERT INTO certificacoesfuncionarios (descricao) VALUES('" + nomeCertificacao + "');";
            _conexaoMysql.ExecutaComandoComRetorno( query );
        }

        public List<Dictionary<string, string>> BuscaCertificacao(string descricaoCertificacao)
        {
            var query = "Select id,descricao from certificacoesfuncionarios where descricao like'%" +
                        descricaoCertificacao + "%' AND excluido='N';";
            var dados = _conexaoMysql.ExecutaComandoComRetorno( query );
            return dados;
        }

        public List<Dictionary<string, string>> RecuperaDadosCertificacao(string idCertificacao)
        {
            var query = "Select id,descricao from certificacoesfuncionarios where id=" + idCertificacao + ";";
            var dados = _conexaoMysql.ExecutaComandoComRetorno( query );
            return dados;
        }

        public void ExcluirCertificacao(string idCertificacao)
        {
            var query = "UPDATE certificacoesfuncionarios SET excluido='S' where id=" + idCertificacao + ";";
            _conexaoMysql.ExecutaComandoComRetorno( query );
        }

        public void EditarCertificacao(string idCertificacao, string nome)
        {
            var query = "UPDATE certificacoesfuncionarios SET descricao='" + nome + "' WHERE id=" + idCertificacao +
                        ";";
            _conexaoMysql.ExecutaComando( query );
        }

        public List<Dictionary<string, string>> RetornaSetores()
        {
            var query = "select id,descricao from setores WHERE excluido='N';";
            var dados = _conexaoMysql.ExecutaComandoComRetorno( query );
            return dados;
        }

        public List<Dictionary<string, string>> RetornaFuncionarios()
        {
            var query = "select id,nome from funcionarios WHERE ativo='S';";
            var dados = _conexaoMysql.ExecutaComandoComRetorno( query );
            return dados;
        }

        public void CadastrarGestor(string funcionario, string setor)
        {
            var query = "UPDATE funcionarios SET gestor='S' , idsetor=" + setor + " WHERE id=" + funcionario + ";";
            _conexaoMysql.ExecutaComando( query );
        }

        public List<Dictionary<string, string>> BuscaGestor(string descricaoGestor)
        {
            var query = "Select id,nome from funcionarios where nome like'%" + descricaoGestor +
                        "%' AND gestor='S' AND ativo='S';";
            var dados = _conexaoMysql.ExecutaComandoComRetorno( query );
            return dados;
        }

        public List<Dictionary<string, string>> RecuperaDadosGestor(string idGestor)
        {
            var query = "Select id,nome,idsetor from funcionarios where id=" + idGestor + ";";
            var dados = _conexaoMysql.ExecutaComandoComRetorno( query );
            return dados;
        }

        public void ExcluirGestor(string idGestor)
        {
            var query = "UPDATE funcionarios SET gestor='N' WHERE id=" + idGestor + ";";
            _conexaoMysql.ExecutaComando( query );
        }

        public void EditarGestor(string funcionario, string setor)
        {
            var query = "UPDATE funcionarios SET gestor='S' , idsetor=" + setor + " WHERE id=" + funcionario + ";";
            _conexaoMysql.ExecutaComando( query );
        }

        public void CadastrarSetor(string nomeSetor)
        {
            var query = "INSERT INTO setores (descricao) VALUES('" + nomeSetor + "');";
            _conexaoMysql.ExecutaComandoComRetorno( query );
        }

        public void EditarSetor(string idSetor, string nome)
        {
            var query = "UPDATE setores SET descricao='" + nome + "' WHERE id=" + idSetor + ";";
            _conexaoMysql.ExecutaComando( query );
        }

        public List<Dictionary<string, string>> BuscaSetor(string descricaoSetor)
        {
            var query = "Select id,descricao from setores where descricao like'%" + descricaoSetor +
                        "%' AND excluido='N';";
            var dados = _conexaoMysql.ExecutaComandoComRetorno( query );
            return dados;
        }

        public List<Dictionary<string, string>> RecuperaDadosSetor(string idSetor)
        {
            var query = "Select id,descricao from setores where id=" + idSetor + ";";
            var dados = _conexaoMysql.ExecutaComandoComRetorno( query );
            return dados;
        }

        public void ExcluirSetor(string idSetor)
        {
            var query = "UPDATE setores SET excluido='S' where id=" + idSetor + ";";
            _conexaoMysql.ExecutaComandoComRetorno( query );
            var query2 = "UPDATE funcionarios SET IDSETOR=0 WHERE idsetor=" + idSetor + "";
            _conexaoMysql.ExecutaComando( query2 );
        }

        public List<Dictionary<string, string>> RetornaBancoHoras()
        {
            var query = "Select * from horasextrasfuncionarios;";
            var dados = _conexaoMysql.ExecutaComandoComRetorno( query );
            return dados;
        }

        public void CadastrarUsuarioPortal(string nome, string cpf, string rg, string dataNascimento, string rua,
            string numero, string bairro, string cidade, string usuario, string pa, string dataAdmissao,
            string vencimentoPeriodico, string pis, string salarioUsuario, string quebraCaixa)
        {
            var query =
                "INSERT INTO funcionarios (nome,sexo,idpa,admissao,cpf,rg,pis,formacaoacademica,salariobase,quebradecaixa,anuenio,ticket,datanascimento,ativo,rua,numero,bairro,cidade,login,senha,trocasenha) VALUES('" +
                nome + "',0,'" + pa + "','" + dataAdmissao + "','" + cpf + "','" + rg + "','" + pis +
                "','NÃO INFORMADO','" + salarioUsuario + "','" + quebraCaixa + "','0.00','827.64','" + dataNascimento +
                "','S','" + rua + "','" + numero + "','" + bairro + "','" + cidade + "','" + usuario +
                "',MD5('123'),'S')";
            _conexaoMysql.ExecutaComandoComRetorno( query );
        }

        public void AtualizaDadosPerfilRh(DateTime dataAdmissaoFuncionario, string vencimentoPeriodico, int idFuncao, double salario, double quebraDeCaixa, double anuenio, double ticket, string estagiario, DateTime dataEstagiario, int idSetor,string login)
        {
            var query =
                "UPDATE funcionarios SET admissao='" + dataAdmissaoFuncionario.ToString("yyyy/MM/dd") + "', vencimentoperiodico='" +
                vencimentoPeriodico + "', funcao=" + idFuncao + ", salariobase='" + salario + "',quebradecaixa='" +
                quebraDeCaixa + "',anuenio='" + anuenio + "',ticket='" + ticket + "',estagiario='" + estagiario +
                "',contratoestagio='" + dataEstagiario.ToString( "yyyy/MM/dd" ) + "',idsetor='" + idSetor + "' WHERE login='" + login + "'";
            _conexaoMysql.ExecutaComandoComRetorno( query );
        }
    }
}