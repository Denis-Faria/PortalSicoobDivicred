using System.Collections.Generic;
using System.Web;
using PortalSicoobDivicred.Repositorios;

namespace PortalSicoobDivicred.Aplicacao
{
    public class QueryMysqlCurriculo
    {
        private readonly Conexao _conexaoMysql;


        public QueryMysqlCurriculo()
        {
            _conexaoMysql = new Conexao();
        }

        public bool UsuarioLogado()
        {
            var usuario = HttpContext.Current.Request.Cookies["CookieFarm"];
            if (usuario == null)
                return false;
            return true;
        }

        public List<Dictionary<string, string>> RecuperaCurriculos()
        {
            var querySelecionaCurriculo =
                "select a.nome,a.email,b.descricao,if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Reprovado')>0,'N',if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Ausente')>0,'A',if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Excedente')>0,'E','S'))) as status, a.idarquivogoogle,a.cpf,a.cidade from candidatos a , areasinteresses b where a.id=b.idcandidato";
            var dadosCurriculos = _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaCurriculo);
            return dadosCurriculos;
        }

        public List<Dictionary<string, string>> FormularioRecrutamentoSelecaoVaga(string idVaga)
        {
            var querySelecionaCurriculo =
                "select titulo,DATE_FORMAT(datainicio,'%d/%m/%Y') as datainicio from vagas where id=" + idVaga + " ;";
            var dadosCurriculos = _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaCurriculo);
            return dadosCurriculos;
        }

        public List<Dictionary<string, string>> FormularioRecrutamentoSelecaoHistorico(string idVaga)
        {
            var querySelecionaCurriculo =
                "select count(id) as count from historicos where idvaga=" + idVaga + "";
            var dadosCurriculos = _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaCurriculo);
            return dadosCurriculos;
        }

        public List<Dictionary<string, string>> FormularioRecrutamentoSelecaoProcesso(string idVaga)
        {
            var querySelecionaCurriculo =
                "select count(id) as count from processosseletivos where idvaga=" + idVaga + ";";
            var dadosCurriculos = _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaCurriculo);
            return dadosCurriculos;
        }

        public List<Dictionary<string, string>> ExisteFormularioRecrutamentoSelecaoProcesso(string idVaga)
        {
            var querySelecionaCurriculo =
                "select count(id) as count from recrutamentoselecao where idvaga=" + idVaga + ";";
            var dadosCurriculos = _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaCurriculo);
            return dadosCurriculos;
        }

        public List<Dictionary<string, string>> FormularioRecrutamentoSelecao(string idVaga)
        {
            var querySelecionaCurriculo =
                "select * from recrutamentoselecao where idvaga=" + idVaga + ";";
            var dadosCurriculos = _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaCurriculo);
            return dadosCurriculos;
        }

        public void InserirFormularioRecrutamentoSelecao(string idVaga, string classeCargo, string nivelCargo,
            string numeroVaga, string chefiaImediata, string mesAdmissao, string motivoSelecao,
            string empregadoSubstituido, string previsaoOrcamento, string formaRecrutamento, string dinamicaGrupo,
            string dinamicaNumero, string dinamicaPreNumero, string conhecimentoTeste, string conhecimentoNumero,
            string conhecimentoPreNumero, string psicologicoTeste, string psicologicoNumero,
            string psicologicoPreNumero, string psicologaentrevistador, string psicologaNumero,
            string psicologaPreNumero, string setor)
        {
            var querySelecionaCurriculo =
                "INSERT INTO recrutamentoselecao (idvaga,classecargo,nivelcargo,numerovaga,chefiaimediata,mesadmissao,motivoselecao,nomeempregadosubstituido,previsaoorcamento,formarecrutamento,dinamicagrupo,dinamicanumero,dinamicaprenumero,conhecimentoteste,conhecimentonumero,conhecimentoprenumero,psicologicoteste,psicologiconumero,psicologicoprenumero,psicologaentrevistador,psicologanumero,psicologaprenumero,setor) VALUES(" +
                idVaga + ",'" + classeCargo + "','" + nivelCargo + "'," + numeroVaga + ",'" + chefiaImediata + "','" +
                mesAdmissao + "','" + motivoSelecao + "','" + empregadoSubstituido + "','" + previsaoOrcamento + "','" +
                formaRecrutamento + "','" + dinamicaGrupo + "'," + dinamicaNumero + "," + dinamicaPreNumero + ",'" +
                conhecimentoTeste + "'," + conhecimentoNumero + "," + conhecimentoPreNumero + ",'" + psicologicoTeste +
                "'," + psicologicoNumero + "," + psicologicoPreNumero + ",'" + psicologaentrevistador + "'," +
                psicologaNumero + "," + psicologaPreNumero + ", '" + setor + "');";

          _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaCurriculo);
        }

        public void AtualizarFormularioRecrutamentoSelecao(string idVaga, string classeCargo, string nivelCargo,
            string numeroVaga, string chefiaImediata, string mesAdmissao, string motivoSelecao,
            string empregadoSubstituido, string previsaoOrcamento, string formaRecrutamento, string dinamicaGrupo,
            string dinamicaNumero, string dinamicaPreNumero, string conhecimentoTeste, string conhecimentoNumero,
            string conhecimentoPreNumero, string psicologicoTeste, string psicologicoNumero,
            string psicologicoPreNumero, string psicologaentrevistador, string psicologaNumero,
            string psicologaPreNumero, string setor)
        {
            var querySelecionaCurriculo = "UPDATE recrutamentoselecao set classecargo='" + classeCargo +
                                          "',nivelcargo='" + nivelCargo + "',numerovaga=" + numeroVaga +
                                          ",chefiaimediata='" + chefiaImediata + "',mesadmissao='" + mesAdmissao +
                                          "',motivoselecao='" + motivoSelecao + "',nomeempregadosubstituido='" +
                                          empregadoSubstituido + "',previsaoorcamento='" + previsaoOrcamento +
                                          "',formarecrutamento='" + formaRecrutamento + "',dinamicagrupo='" +
                                          dinamicaGrupo + "',dinamicanumero=" + dinamicaNumero +
                                          ",dinamicaprenumero=" + dinamicaPreNumero + ",conhecimentoteste='" +
                                          conhecimentoTeste + "',conhecimentonumero=" + conhecimentoNumero +
                                          ",conhecimentoprenumero=" + conhecimentoPreNumero + ",psicologicoteste='" +
                                          psicologicoTeste + "',psicologiconumero=" + psicologicoNumero +
                                          ",psicologicoprenumero=" + psicologicoPreNumero +
                                          ",psicologaentrevistador='" + psicologaentrevistador + "',psicologanumero=" +
                                          psicologaNumero + ",psicologaprenumero=" + psicologaPreNumero + ",setor='" +
                                          setor + "' WHERE idvaga=" + idVaga + ";";

            _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaCurriculo);
        }


        public List<Dictionary<string, string>> FormularioParecerProcessoDadosPessoais(string cpf, string idVaga)
        {
            var querySelecionaCurriculo =
                "select a.nome,DATE_FORMAT(a.datanascimento,'%d/%m/%Y') as nascimento, b.descricao as escolaridade, c.titulo,c.salario from candidatos a, tiposescolaridades b,vagas c, processosseletivos d where a.idtipoescolaridade=b.id and a.id=d.idcandidato and c.id=d.idvaga and a.cpf='" +
                cpf + "' and d.idvaga=" + idVaga + "";
            var dadosCurriculos = _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaCurriculo);
            return dadosCurriculos;
        }

        public List<Dictionary<string, string>> FormularioParecerProcessoDadosEscolares(string cpf)
        {
            var querySelecionaCurriculo =
                "select b.nomecurso, b.anofim from candidatos a, dadosescolares b where a.cpf='" +
                cpf + "' and b.idcandidato=a.id";
            var dadosCurriculos = _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaCurriculo);
            return dadosCurriculos;
        }

        public List<Dictionary<string, string>> FormularioParecerProcessoDadosProfissionais(string cpf)
        {
            var querySelecionaCurriculo =
                "select b.nomeempresa, b.nomecargo from candidatos a, dadosprofissionais b where a.cpf='" + cpf +
                "' and b.idcandidato=a.id";
            var dadosCurriculos = _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaCurriculo);
            return dadosCurriculos;
        }

        public void InserirFormularioParecerProcesso(string solicitante, string metodologiaprocesso,
            string demandaparecer, string tipoRecrutamento, string perfilTecnico, string conclusao, string cpf,
            string idVaga)
        {
            var querySelecionaIdCandidato =
                "select id FROM candidatos where cpf='" + cpf + "';";
            var dadosCandidatos = _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaIdCandidato);

            var querySelecionaCurriculo =
                "INSERT INTO parecerprocessoseletivo (idcandidato,solicitante,idvaga,metodologiaprocesso,demandaparecer,tiporecrutamento,perfiltecnico,conclusao) VALUES(" +
                dadosCandidatos[0]["id"] + ",'" + solicitante + "'," + idVaga + ",'" + metodologiaprocesso + "','" +
                demandaparecer + "','" + tipoRecrutamento + "','" + perfilTecnico + "','" + conclusao + "')";
            _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaCurriculo);
        }

        public void AtualizarFormularioParecerProcesso(string solicitante, string metodologiaprocesso,
            string demandaparecer, string tipoRecrutamento, string perfilTecnico, string conclusao, string cpf,
            string idVaga)
        {
            var querySelecionaIdCandidato =
                "select id FROM candidatos where cpf='" + cpf + "';";
            var dadosCandidatos = _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaIdCandidato);

            var querySelecionaCurriculo = "UPDATE parecerprocessoseletivo SET solicitante='" + solicitante +
                                          "',metodologiaprocesso='" + metodologiaprocesso + "',demandaparecer='" +
                                          demandaparecer + "',tiporecrutamento='" + tipoRecrutamento +
                                          "',perfiltecnico='" + perfilTecnico + "',conclusao='" + conclusao +
                                          "' WHERE idcandidato=" + dadosCandidatos[0]["id"] + " AND idvaga=" + idVaga +
                                          ";";
            _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaCurriculo);
        }

        public List<Dictionary<string, string>> RecuperaFormularioParecerProcesso(string cpf, string idVaga)
        {
            var querySelecionaIdCandidato =
                "select id FROM candidatos where cpf='" + cpf + "';";
            var dadosCandidatos = _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaIdCandidato);

            var querySelecionaCurriculo = "select * from  parecerprocessoseletivo WHERE idcandidato=" +
                                          dadosCandidatos[0]["id"] + " AND idvaga=" + idVaga + ";";
            var dados = _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaCurriculo);
            return dados;
        }

        public List<Dictionary<string, string>> RecuperaVagaEspecifica()
        {
            var querySelecionaCurriculo =
                "select id,descricao from vagas where areadeinteresse='Especifica' AND ativa='S' ";
            var dadosCurriculos = _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaCurriculo);
            return dadosCurriculos;
        }

        public void AtribuirVaga(string cpf, string idVaga)
        {
            var querySelecionaIdCandidato =
                "select id FROM candidatos where cpf='" + cpf + "';";
            var dadosCandidatos = _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaIdCandidato);
            var querySelecionaCurriculo =
                "INSERT INTO historicos (idvaga,idcandidato,feedback) VALUES(" + idVaga + "," +
                dadosCandidatos[0]["id"] + ",'Obrigado pelo interesse entraremos em contato em breve') ";

            _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaCurriculo);
        }

        public List<Dictionary<string, string>> RecuperaCurriculosArea(string area, string cidade, string certificacao,
            string ordenacao, string formacao)
        {
            if (ordenacao.Equals("Alfabetico"))
            {
                var querySelecionaCurriculo =
                    "select a.nome,a.email,b.descricao,if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Reprovado')>0,'N',if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Ausente')>0,'A',if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Excedente')>0,'E','S'))) as status, a.idarquivogoogle,a.cpf,a.cidade from candidatos a , areasinteresses b where a.idtipoescolaridade=" +
                    formacao + " AND b.descricao like'%" +
                    area + "%' AND a.id=b.idcandidato AND a.cidade like'%" + cidade + "%' AND a.certificacao like'%" +
                    certificacao + "%' ORDER BY nome ASC";
                var dadosCurriculos = _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaCurriculo);
                return dadosCurriculos;
            }

            if (ordenacao.Equals("Status"))
            {
                var querySelecionaCurriculo =
                    "select a.nome,a.email,b.descricao,if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Reprovado')>0,'N',if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Ausente')>0,'A',if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Excedente')>0,'E','S'))) as status, a.idarquivogoogle,a.cpf,a.cidade from candidatos a , areasinteresses b where a.idtipoescolaridade=" +
                    formacao + " and b.descricao like'%" +
                    area + "%' AND a.id=b.idcandidato AND a.cidade like'%" + cidade + "%' AND a.certificacao like'%" +
                    certificacao + "%' ORDER BY status asc ";
                var dadosCurriculos = _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaCurriculo);
                return dadosCurriculos;
            }
            else
            {
                var querySelecionaCurriculo =
                    "select a.nome,a.email,b.descricao,if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Reprovado')>0,'N',if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Ausente')>0,'A',if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Excedente')>0,'E','S'))) as status, a.idarquivogoogle,a.cpf,a.cidade from candidatos a , areasinteresses b where a.idtipoescolaridade=" +
                    formacao + " and b.descricao like'%" +
                    area + "%' AND a.id=b.idcandidato AND a.cidade like'%" + cidade + "%' AND a.certificacao like'%" +
                    certificacao + "%'";
                var dadosCurriculos = _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaCurriculo);
                return dadosCurriculos;
            }
        }


        public List<Dictionary<string, string>> RecuperaCurriculosProcesso(string idVaga)
        {
            var querySelecionaCurriculo =
                "select a.nome,a.cpf,a.email,b.* from candidatos a, processosseletivos b where a.id=b.idcandidato and b.idvaga=" +
                idVaga + ";";
            var dadosCurriculos = _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaCurriculo);
            return dadosCurriculos;
        }

        public bool VerificaProcesso(string idVaga)
        {
            var querySelecionaCurriculo =
                "select * from processosseletivos where idvaga=" + idVaga + "";
            var dadosVaga = _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaCurriculo);
            if (dadosVaga.Count > 0)
                return true;
            return false;
        }

        public void AtualizarVaga(string idVaga, string descricao, string salario, string requisito, string titulo,
            string beneficio, string ativa, string areas)
        {
            var querySelecionaCurriculo = "UPDATE vagas SET descricao='" + descricao + "',salario='" +
                                          salario.Replace(",", ".") + "',requisito='" + requisito + "',titulo='" +
                                          titulo + "', beneficio='" + beneficio + "', ativa='" + ativa +
                                          "', areadeinteresse='" + areas + "' where id=" + idVaga + ";";
            _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaCurriculo);
        }

        public List<Dictionary<string, string>> RecuperaDadosVaga(string idVaga)
        {
            var querySelecionaCurriculo = "SELECT * from vagas where id=" + idVaga + ";";
            var dadosCurriculos = _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaCurriculo);
            return dadosCurriculos;
        }

        public List<Dictionary<string, string>> RecuperaProcesso(string idCandidato)
        {
            var querySelecionaCurriculo =
                "SELECT a.*,b.titulo as nomevaga from processosseletivos a , vagas b where a.idvaga=b.id and idcandidato=" +
                idCandidato + ";";
            var dadosCurriculos = _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaCurriculo);
            return dadosCurriculos;
        }

        public List<Dictionary<string, string>> RecuperaFormulario(string idCandidato)
        {
            var querySelecionaCurriculo =
                "SELECT * from formulariosiniciais where  idcandidato=" + idCandidato + ";";
            var dadosCurriculos = _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaCurriculo);
            return dadosCurriculos;
        }

        public List<Dictionary<string, string>> RecuperaCurriculosAlfabetico()
        {
            var querySelecionaCurriculo =
                "select a.nome,a.email,b.descricao,if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Reprovado')>0,'N',if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Ausente')>0,'A',if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Excedente')>0,'E','S'))) as status, a.idarquivogoogle,a.cpf,a.cidade from candidatos a , areasinteresses b where a.id=b.idcandidato  order by a.nome asc  ";
            var dadosCurriculos = _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaCurriculo);
            return dadosCurriculos;
        }

        public List<Dictionary<string, string>> RecuperaCurriculosStatus()
        {
            var querySelecionaCurriculo =
                "select a.nome,a.email,b.descricao,if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Reprovado')>0,'N',if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Ausente')>0,'A',if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Excedente')>0,'E','S'))) as status, a.idarquivogoogle,a.cpf,a.cidade from candidatos a , areasinteresses b where a.id=b.idcandidato order by status asc ";
            var dadosCurriculos = _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaCurriculo);
            return dadosCurriculos;
        }


        public List<Dictionary<string, string>> RecuperaCurriculosHistorico(string idVaga)
        {
            var querySelecionaCurriculo =
                "select a.cpf, a.nome,a.email, a.idarquivogoogle,a.cidade,a.certificacao,a.telefoneprincipal from historicos c LEFT JOIN candidatos a on c.idcandidato=a.id INNER JOIN candidatos u2 on (c.idcandidato=u2.id) where c.idvaga=" +
                idVaga + "";
            var dadosCurriculos = _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaCurriculo);
            return dadosCurriculos;
        }


        public List<Dictionary<string, string>> RecuperaVagas()
        {
            var querySelecionaVagas =
                "select id,titulo,descricao,areadeinteresse,ativa FROM vagas ORDER BY ativa DESC";
            var dadosVagas = _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaVagas);
            return dadosVagas;
        }

        public void CriaBalao(string cpf)
        {
            var queryAdcionaBalao =
                "UPDATE candidatos SET balao='S' WHERE cpf='" + cpf + "';";
            _conexaoMysql.ExecutaComandoComRetornoPortal(queryAdcionaBalao);
        }

        public List<Dictionary<string, string>> RecuperaVagasId(string id)
        {
            var querySelecionaVagas =
                "select id,titulo,descricao,areadeinteresse,ativa FROM vagas where id=" + id + "";
            var dadosVagas = _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaVagas);
            return dadosVagas;
        }

        public List<Dictionary<string, string>> CadastrarVaga(string descricao, string areaInteresse, string salario,
            string requisito,
            string titulo, string beneficio)
        {
            var querySelecionaCurriculo =
                "INSERT INTO vagas (descricao,areadeinteresse,salario,requisito,titulo,beneficio,datainicio) VALUES('" +
                descricao + "','" + areaInteresse + "','" + salario + "','" + requisito + "','" + titulo + "','" +
                beneficio + "',now())";
            _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaCurriculo);
            var querySelecionaEmailVagas =
                "select a.email,a.telefoneprincipal from candidatos a, areasinteresses b WHERE a.id=b.idcandidato AND a.balao='N' AND MATCH(b.descricao) AGAINST('" +
                areaInteresse + "' )";
            var email = _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaEmailVagas);
            return email;
        }

        public void IniciarProcessoSeletivo(string cpf, string idVaga)
        {
            var querySelecionaIdCandidato =
                "select id FROM candidatos where cpf='" + cpf + "';";
            var dadosCandidatos = _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaIdCandidato);
            var queryIniciaProcesso = "INSERT INTO processosseletivos (idvaga,idcandidato) VALUES(" + idVaga + "," +
                                      dadosCandidatos[0]["id"] + ")";
            _conexaoMysql.ExecutaComandoComRetornoPortal(queryIniciaProcesso);
        }

        public void AtualizarProcessoSeletivoStatus(string cpf, string idVaga, string resultado, string restricao)
        {
            var querySelecionaIdCandidato = "select id FROM candidatos where cpf='" + cpf + "';";
            var dadosCandidatos = _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaIdCandidato);
            try
            {
                
                    var queryIniciaProcesso = "Update processosseletivos set aprovado='" + resultado +
                                              "' WHERE idvaga=" + idVaga + " AND idcandidato=" +
                                              dadosCandidatos[0]["id"] + ", restricao='" + restricao + "';";
                    
                    _conexaoMysql.ExecutaComandoComRetornoPortal(queryIniciaProcesso);
                
            }
            catch
            {
                var queryIniciaProcesso = "Update processosseletivos set aprovado='" + resultado + "' WHERE idvaga=" +
                                          idVaga + " AND idcandidato=" + dadosCandidatos[0]["id"] + ";";
                _conexaoMysql.ExecutaComandoComRetornoPortal(queryIniciaProcesso);
            }

            var queryMudaDataFim = "UPDATE vagas SET datafim=NOW() WHERE id=" + idVaga + "";
            _conexaoMysql.ExecutaComandoComRetornoPortal(queryMudaDataFim);
        }

        public void AtualizarProcessoSeletivoGerente(string cpf, string idVaga, string resultado)
        {
            var querySelecionaIdCandidato =
                "select id FROM candidatos where cpf='" + cpf + "';";
            var dadosCandidatos = _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaIdCandidato);
            var queryIniciaProcesso = "Update processosseletivos set gerente= '" + resultado + "' WHERE idvaga = " +
                                      idVaga + " AND idcandidato = " + dadosCandidatos[0]["id"] + "; ";
            _conexaoMysql.ExecutaComandoComRetornoPortal(queryIniciaProcesso);
        }

        public void AtualizaProcessoSeletivoPsicologico(string cpf, string idVaga, string resultado)
        {
            var querySelecionaIdCandidato =
                "select id FROM candidatos where cpf='" + cpf + "';";
            var dadosCandidatos = _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaIdCandidato);
            var queryIniciaProcesso = "Update processosseletivos set psicologico='" + resultado + "' WHERE idvaga=" +
                                      idVaga + " AND idcandidato=" + dadosCandidatos[0]["id"] + ";";
            _conexaoMysql.ExecutaComandoComRetornoPortal(queryIniciaProcesso);
        }

        public void AtualizaProcessoSeletivoTeorico(string cpf, string idVaga, string resultado)
        {
            var querySelecionaIdCandidato =
                "select id FROM candidatos where cpf='" + cpf + "';";
            var dadosCandidatos = _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaIdCandidato);
            var queryIniciaProcesso = "Update processosseletivos set prova='" + resultado + "' WHERE idvaga=" +
                                      idVaga + " AND idcandidato=" + dadosCandidatos[0]["id"] + ";";
            _conexaoMysql.ExecutaComandoComRetornoPortal(queryIniciaProcesso);
        }

        public void CadastrarAlerta(string alerta)
        {
            var querySelecionaVagas =
                "select id FROM candidatos";
            var dadosCandidatos = _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaVagas);

            for (var i = 0; i < dadosCandidatos.Count; i++)
            {
                var querySelecionaCurriculo =
                    "INSERT INTO alertas (alerta,idcandidato) VALUES('" + alerta + "'," + dadosCandidatos[i]["id"] +
                    ")";
                _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaCurriculo);
            }
        }

        public void CadastrarAlertaEspecifico(string alerta, string idCandidato)
        {
            var querySelecionaCurriculo =
                "INSERT INTO alertas (alerta,idcandidato) VALUES('" + alerta + "'," + idCandidato +
                ")";
            _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaCurriculo);
        }

        public void CadastrarMensagem(string mensagem)
        {
            var querySelecionaVagas =
                "select id FROM candidatos";
            var dadosCandidatos = _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaVagas);

            for (var i = 0; i < dadosCandidatos.Count; i++)
            {
                var querySelecionaCurriculo =
                    "INSERT INTO mensagens (mensagem,idcandidato) VALUES('" + mensagem + "'," +
                    dadosCandidatos[i]["id"] + ")";
                _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaCurriculo);
            }
        }

        public void EncerrarVaga(string idVaga)
        {
            var querySelecionaCurriculo = "UPDATE vagas SET ativa='N' WHERE id=" + idVaga + "";
            _conexaoMysql.ExecutaComandoComRetornoPortal(querySelecionaCurriculo);
        }

        public List<Dictionary<string, string>> RecuperaDadosCandidato(string cpf)
        {
            var queryRecuperaUsuario =
                "SELECT a.*,b.descricao as descricaoestadocivil, c.descricao as escolaridade FROM candidatos a, tiposestadoscivis b, tiposescolaridades c WHERE b.id = a.estadocivil AND c.id = a.idtipoescolaridade AND a.cpf = '" +
                cpf + "'";


            var dados = _conexaoMysql.ExecutaComandoComRetornoPortal(queryRecuperaUsuario);


            return dados;
        }

        public List<Dictionary<string, string>> RecuperaQuestionario(string idCandidato)
        {
            var queryRecuperaUsuario =
                "SELECT * FROM formulariosiniciais WHERE idcandidato='" +
                idCandidato + "'";


            var dados = _conexaoMysql.ExecutaComandoComRetornoPortal(queryRecuperaUsuario);


            return dados;
        }

        public List<Dictionary<string, string>> RecuperaDadosUsuariosProissional(string idCandidato)
        {
            var queryRecuperaProfissional = "SELECT * FROM dadosprofissionais WHERE idcandidato='" + idCandidato + "'";


            var dados = _conexaoMysql.ExecutaComandoComRetornoPortal(queryRecuperaProfissional);


            return dados;
        }

        public List<Dictionary<string, string>> RecuperaDadosUsuariosEducacional(string idCandidato)
        {
            var queryRecuperaEducacional = "SELECT * FROM dadosescolares WHERE idcandidato='" + idCandidato + "'";


            var dados = _conexaoMysql.ExecutaComandoComRetornoPortal(queryRecuperaEducacional);


            return dados;
        }

        public List<Dictionary<string, string>> RecuperaEmail(string cpf)
        {
            var queryRecuperaEducacional = "SELECT id,email,telefoneprincipal FROM candidatos WHERE cpf='" + cpf + "'";


            var dados = _conexaoMysql.ExecutaComandoComRetornoPortal(queryRecuperaEducacional);


            return dados;
        }

        public List<Dictionary<string, string>> RecuperaAreaInteresseUsuarios(string idCandidato)
        {
            var queryRecuperaAreasInteresse = "SELECT * FROM areasinteresses WHERE idcandidato=" + idCandidato + "";


            var dados = _conexaoMysql.ExecutaComandoComRetornoPortal(queryRecuperaAreasInteresse);


            return dados;
        }

        public string RecuperaEstadoCivilCandidato(string id)
        {
            var queryRecuperaAreasInteresse = "SELECT * FROM tiposestadoscivis WHERE id=" + id + "";


            var dados = _conexaoMysql.ExecutaComandoComRetornoPortal(queryRecuperaAreasInteresse);


            return dados[0]["descricao"];
        }

        public string RecuperaEscolaridadeCandidato(string id)
        {
            var queryRecuperaAreasInteresse = "SELECT * FROM tiposescolaridades WHERE id=" + id + "";


            var dados = _conexaoMysql.ExecutaComandoComRetornoPortal(queryRecuperaAreasInteresse);


            return dados[0]["descricao"];
        }


        public List<Dictionary<string, string>> RecuperaIdEscolaridade(string descricao)
        {
            var queryRecuperaAreasInteresse =
                "SELECT * FROM tiposescolaridades WHERE descricao like'%" + descricao + "%'";


            var dados = _conexaoMysql.ExecutaComandoComRetornoPortal(queryRecuperaAreasInteresse);


            return dados;
        }

        public List<Dictionary<string, string>> FiltroVaga(string query)
        {
            var dadosCurriculos = _conexaoMysql.ExecutaComandoComRetornoPortal(query);
            return dadosCurriculos;
        }
    }
}