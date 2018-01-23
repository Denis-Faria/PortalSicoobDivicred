using System.Collections.Generic;
using System.Web;
using Port.Repositorios;

namespace PortalSicoobDivicred.Aplicacao
{
    public class QueryMysqlCurriculo
    {
        private readonly Conexao ConexaoMysql;


        public QueryMysqlCurriculo()
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

        public List<Dictionary<string, string>> RecuperaCurriculos()
        {
            var QuerySelecionaCurriculo =
                "select a.nome,a.email,b.descricao,if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Reprovado')>0,'N',if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Ausente')>0,'A',if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Excedente')>0,'E','S'))) as status, a.idarquivogoogle,a.cpf,a.cidade from candidatos a , areasinteresses b where a.id=b.idcandidato";
            var DadosCurriculos = ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaCurriculo);
            return DadosCurriculos;
        }

        public List<Dictionary<string, string>> FormularioRecrutamentoSelecaoVaga(string IdVaga)
        {
            var QuerySelecionaCurriculo =
                "select titulo,DATE_FORMAT(datainicio,'%d/%m/%Y') as datainicio from vagas where id=" + IdVaga + " ;";
            var DadosCurriculos = ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaCurriculo);
            return DadosCurriculos;
        }

        public List<Dictionary<string, string>> FormularioRecrutamentoSelecaoHistorico(string IdVaga)
        {
            var QuerySelecionaCurriculo =
                "select count(id) as count from historicos where idvaga=" + IdVaga + "";
            var DadosCurriculos = ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaCurriculo);
            return DadosCurriculos;
        }

        public List<Dictionary<string, string>> FormularioRecrutamentoSelecaoProcesso(string IdVaga)
        {
            var QuerySelecionaCurriculo =
                "select count(id) as count from processosseletivos where idvaga=" + IdVaga + ";";
            var DadosCurriculos = ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaCurriculo);
            return DadosCurriculos;
        }

        public List<Dictionary<string, string>> ExisteFormularioRecrutamentoSelecaoProcesso(string IdVaga)
        {
            var QuerySelecionaCurriculo =
                "select count(id) as count from recrutamentoselecao where idvaga=" + IdVaga + ";";
            var DadosCurriculos = ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaCurriculo);
            return DadosCurriculos;
        }

        public List<Dictionary<string, string>> FormularioRecrutamentoSelecao(string IdVaga)
        {
            var QuerySelecionaCurriculo =
                "select * from recrutamentoselecao where idvaga=" + IdVaga + ";";
            var DadosCurriculos = ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaCurriculo);
            return DadosCurriculos;
        }

        public void InserirFormularioRecrutamentoSelecao(string IdVaga, string ClasseCargo, string NivelCargo,
            string NumeroVaga, string ChefiaImediata, string MesAdmissao, string MotivoSelecao,
            string EmpregadoSubstituido, string PrevisaoOrcamento, string FormaRecrutamento, string DinamicaGrupo,
            string DinamicaNumero, string DinamicaPreNumero, string ConhecimentoTeste, string ConhecimentoNumero,
            string ConhecimentoPreNumero, string PsicologicoTeste, string PsicologicoNumero,
            string PsicologicoPreNumero, string Psicologaentrevistador, string PsicologaNumero,
            string PsicologaPreNumero, string Setor)
        {
            var QuerySelecionaCurriculo =
                "INSERT INTO recrutamentoselecao (idvaga,classecargo,nivelcargo,numerovaga,chefiaimediata,mesadmissao,motivoselecao,nomeempregadosubstituido,previsaoorcamento,formarecrutamento,dinamicagrupo,dinamicanumero,dinamicaprenumero,conhecimentoteste,conhecimentonumero,conhecimentoprenumero,psicologicoteste,psicologiconumero,psicologicoprenumero,psicologaentrevistador,psicologanumero,psicologaprenumero,setor) VALUES(" +
                IdVaga + ",'" + ClasseCargo + "','" + NivelCargo + "'," + NumeroVaga + ",'" + ChefiaImediata + "','" +
                MesAdmissao + "','" + MotivoSelecao + "','" + EmpregadoSubstituido + "','" + PrevisaoOrcamento + "','" +
                FormaRecrutamento + "','" + DinamicaGrupo + "'," + DinamicaNumero + "," + DinamicaPreNumero + ",'" +
                ConhecimentoTeste + "'," + ConhecimentoNumero + "," + ConhecimentoPreNumero + ",'" + PsicologicoTeste +
                "'," + PsicologicoNumero + "," + PsicologicoPreNumero + ",'" + Psicologaentrevistador + "'," +
                PsicologaNumero + "," + PsicologaPreNumero + ", '" + Setor + "');";
            var DadosCurriculos = ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaCurriculo);
        }

        public void AtualizarFormularioRecrutamentoSelecao(string IdVaga, string ClasseCargo, string NivelCargo,
            string NumeroVaga, string ChefiaImediata, string MesAdmissao, string MotivoSelecao,
            string EmpregadoSubstituido, string PrevisaoOrcamento, string FormaRecrutamento, string DinamicaGrupo,
            string DinamicaNumero, string DinamicaPreNumero, string ConhecimentoTeste, string ConhecimentoNumero,
            string ConhecimentoPreNumero, string PsicologicoTeste, string PsicologicoNumero,
            string PsicologicoPreNumero, string Psicologaentrevistador, string PsicologaNumero,
            string PsicologaPreNumero, string Setor)
        {
            var QuerySelecionaCurriculo = "UPDATE recrutamentoselecao set classecargo='" + ClasseCargo +
                                           "',nivelcargo='" + NivelCargo + "',numerovaga=" + NumeroVaga +
                                           ",chefiaimediata='" + ChefiaImediata + "',mesadmissao='" + MesAdmissao +
                                           "',motivoselecao='" + MotivoSelecao + "',nomeempregadosubstituido='" +
                                           EmpregadoSubstituido + "',previsaoorcamento='" + PrevisaoOrcamento +
                                           "',formarecrutamento='" + FormaRecrutamento + "',dinamicagrupo='" +
                                           DinamicaGrupo + "',dinamicanumero=" + DinamicaNumero +
                                           ",dinamicaprenumero=" + DinamicaPreNumero + ",conhecimentoteste='" +
                                           ConhecimentoTeste + "',conhecimentonumero=" + ConhecimentoNumero +
                                           ",conhecimentoprenumero=" + ConhecimentoPreNumero + ",psicologicoteste='" +
                                           PsicologicoTeste + "',psicologiconumero=" + PsicologicoNumero +
                                           ",psicologicoprenumero=" + PsicologicoPreNumero +
                                           ",psicologaentrevistador='" + Psicologaentrevistador + "',psicologanumero=" +
                                           PsicologaNumero + ",psicologaprenumero=" + PsicologaPreNumero + ",setor='" +
                                           Setor + "' WHERE idvaga=" + IdVaga + ";";
            var DadosCurriculos = ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaCurriculo);
        }


        public List<Dictionary<string, string>> FormularioParecerProcessoDadosPessoais(string Cpf, string IdVaga)
        {
            var QuerySelecionaCurriculo =
                "select a.nome,DATE_FORMAT(a.datanascimento,'%d/%m/%Y') as nascimento, b.descricao as escolaridade, c.titulo,c.salario from candidatos a, tiposescolaridades b,vagas c, processosseletivos d where a.idtipoescolaridade=b.id and a.id=d.idcandidato and c.id=d.idvaga and a.cpf='" +
                Cpf + "' and d.idvaga=" + IdVaga + "";
            var DadosCurriculos = ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaCurriculo);
            return DadosCurriculos;
        }

        public List<Dictionary<string, string>> FormularioParecerProcessoDadosEscolares(string Cpf)
        {
            var QuerySelecionaCurriculo =
                "select b.nomecurso, b.anofim from candidatos a, dadosescolares b where a.cpf='" +
                Cpf + "' and b.idcandidato=a.id";
            var DadosCurriculos = ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaCurriculo);
            return DadosCurriculos;
        }

        public List<Dictionary<string, string>> FormularioParecerProcessoDadosProfissionais(string Cpf)
        {
            var QuerySelecionaCurriculo =
                "select b.nomeempresa, b.nomecargo from candidatos a, dadosprofissionais b where a.cpf='" + Cpf +
                "' and b.idcandidato=a.id";
            var DadosCurriculos = ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaCurriculo);
            return DadosCurriculos;
        }

        public void InserirFormularioParecerProcesso(string Solicitante, string metodologiaprocesso,
            string demandaparecer, string TipoRecrutamento, string PerfilTecnico, string Conclusao, string Cpf,
            string IdVaga)
        {
            var QuerySelecionaIdCandidato =
                "select id FROM candidatos where cpf='" + Cpf + "';";
            var DadosCandidatos = ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaIdCandidato);

            var QuerySelecionaCurriculo =
                "INSERT INTO parecerprocessoseletivo (idcandidato,solicitante,idvaga,metodologiaprocesso,demandaparecer,tiporecrutamento,perfiltecnico,conclusao) VALUES(" +
                DadosCandidatos[0]["id"] + ",'" + Solicitante + "'," + IdVaga + ",'" + metodologiaprocesso + "','" +
                demandaparecer + "','" + TipoRecrutamento + "','" + PerfilTecnico + "','" + Conclusao + "')";
            ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaCurriculo);
        }

        public void AtualizarFormularioParecerProcesso(string Solicitante, string metodologiaprocesso,
            string demandaparecer, string TipoRecrutamento, string PerfilTecnico, string Conclusao, string Cpf,
            string IdVaga)
        {
            var QuerySelecionaIdCandidato =
                "select id FROM candidatos where cpf='" + Cpf + "';";
            var DadosCandidatos = ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaIdCandidato);

            var QuerySelecionaCurriculo = "UPDATE parecerprocessoseletivo SET solicitante='" + Solicitante +
                                           "',metodologiaprocesso='" + metodologiaprocesso + "',demandaparecer='" +
                                           demandaparecer + "',tiporecrutamento='" + TipoRecrutamento +
                                           "',perfiltecnico='" + PerfilTecnico + "',conclusao='" + Conclusao +
                                           "' WHERE idcandidato=" + DadosCandidatos[0]["id"] + " AND idvaga=" + IdVaga +
                                           ";";
            ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaCurriculo);
        }

        public List<Dictionary<string, string>> RecuperaFormularioParecerProcesso(string Cpf, string IdVaga)
        {
            var QuerySelecionaIdCandidato =
                "select id FROM candidatos where cpf='" + Cpf + "';";
            var DadosCandidatos = ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaIdCandidato);

            var QuerySelecionaCurriculo = "select * from  parecerprocessoseletivo WHERE idcandidato=" +
                                           DadosCandidatos[0]["id"] + " AND idvaga=" + IdVaga + ";";
            var Dados = ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaCurriculo);
            return Dados;
        }

        public List<Dictionary<string, string>> RecuperaVagaEspecifica()
        {
            var QuerySelecionaCurriculo =
                "select id,descricao from vagas where areadeinteresse='Especifica' AND ativa='S' ";
            var DadosCurriculos = ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaCurriculo);
            return DadosCurriculos;
        }

        public void AtribuirVaga(string Cpf, string IdVaga)
        {
            var QuerySelecionaIdCandidato =
                "select id FROM candidatos where cpf='" + Cpf + "';";
            var DadosCandidatos = ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaIdCandidato);
            var QuerySelecionaCurriculo =
                "INSERT INTO historicos (idvaga,idcandidato,feedback) VALUES(" + IdVaga + "," +
                DadosCandidatos[0]["id"] + ",'Obrigado pelo interesse entraremos em contato em breve') ";

            ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaCurriculo);
        }

        public List<Dictionary<string, string>> RecuperaCurriculosArea(string Area, string Cidade, string Certificacao)
        {
            var QuerySelecionaCurriculo =
                "select a.nome,a.email,b.descricao,if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Reprovado')>0,'N',if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Ausente')>0,'A',if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Excedente')>0,'E','S'))) as status, a.idarquivogoogle,a.cpf,a.cidade from candidatos a , areasinteresses b where b.descricao like'%" +
                Area + "%' AND a.id=b.idcandidato AND a.cidade like'%" + Cidade + "%' AND a.certificacao like'%" +
                Certificacao + "%'";
            var DadosCurriculos = ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaCurriculo);
            return DadosCurriculos;
        }


        public List<Dictionary<string, string>> RecuperaCurriculosProcesso(string IdVaga)
        {
            var QuerySelecionaCurriculo =
                "select a.nome,a.cpf,a.email,b.* from candidatos a, processosseletivos b where a.id=b.idcandidato and b.idvaga=" +
                IdVaga + ";";
            var DadosCurriculos = ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaCurriculo);
            return DadosCurriculos;
        }

        public bool VerificaProcesso(string IdVaga)
        {
            var QuerySelecionaCurriculo =
                "select * from processosseletivos where idvaga=" + IdVaga + "";
            var DadosVaga = ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaCurriculo);
            if (DadosVaga.Count > 0)
                return true;
            return false;
        }

        public void AtualizarVaga(string IdVaga, string Descricao, string Salario, string Requisito, string Titulo,
            string Beneficio, string Ativa, string Areas)
        {
            var QuerySelecionaCurriculo = "UPDATE vagas SET descricao='" + Descricao + "',salario='" +
                                           Salario.Replace(",", ".") + "',requisito='" + Requisito + "',titulo='" +
                                           Titulo + "', beneficio='" + Beneficio + "', ativa='" + Ativa +
                                           "', areadeinteresse='" + Areas + "' where id=" + IdVaga + ";";
            ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaCurriculo);
        }

        public List<Dictionary<string, string>> RecuperaDadosVaga(string IdVaga)
        {
            var QuerySelecionaCurriculo = "SELECT * from vagas where id=" + IdVaga + ";";
            var DadosCurriculos = ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaCurriculo);
            return DadosCurriculos;
        }

        public List<Dictionary<string, string>> RecuperaProcesso(string IdCandidato)
        {
            var QuerySelecionaCurriculo =
                "SELECT a.*,b.titulo as nomevaga from processosseletivos a , vagas b where a.idvaga=b.id and idcandidato=" +
                IdCandidato + ";";
            var DadosCurriculos = ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaCurriculo);
            return DadosCurriculos;
        }

        public List<Dictionary<string, string>> RecuperaFormulario(string IdCandidato)
        {
            var QuerySelecionaCurriculo =
                "SELECT * from formulariosiniciais where  idcandidato=" + IdCandidato + ";";
            var DadosCurriculos = ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaCurriculo);
            return DadosCurriculos;
        }

        public List<Dictionary<string, string>> RecuperaCurriculosAlfabetico()
        {
            var QuerySelecionaCurriculo =
                "select a.nome,a.email,b.descricao,if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Reprovado')>0,'N',if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Ausente')>0,'A',if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Excedente')>0,'E','S'))) as status, a.idarquivogoogle,a.cpf,a.cidade from candidatos a , areasinteresses b where a.id=b.idcandidato  order by a.nome asc  ";
            var DadosCurriculos = ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaCurriculo);
            return DadosCurriculos;
        }

        public List<Dictionary<string, string>> RecuperaCurriculosStatus()
        {
            var QuerySelecionaCurriculo =
                "select a.nome,a.email,b.descricao,if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Reprovado')>0,'N',if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Ausente')>0,'A',if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Excedente')>0,'E','S'))) as status, a.idarquivogoogle,a.cpf,a.cidade from candidatos a , areasinteresses b where a.id=b.idcandidato order by status asc ";
            var DadosCurriculos = ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaCurriculo);
            return DadosCurriculos;
        }


        public List<Dictionary<string, string>> RecuperaCurriculosHistorico(string IdVaga)
        {
            var QuerySelecionaCurriculo =
                "select a.cpf, a.nome,a.email, a.idarquivogoogle,a.cidade,a.certificacao,a.telefoneprincipal from historicos c LEFT JOIN candidatos a on c.idcandidato=a.id INNER JOIN candidatos u2 on (c.idcandidato=u2.id) where c.idvaga=" +
                IdVaga + "";
            var DadosCurriculos = ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaCurriculo);
            return DadosCurriculos;
        }


        public List<Dictionary<string, string>> RecuperaVagas()
        {
            var QuerySelecionaVagas =
                "select id,titulo,descricao,areadeinteresse,ativa FROM vagas ORDER BY ativa DESC";
            var DadosVagas = ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaVagas);
            return DadosVagas;
        }

        public void CriaBalao(string Cpf)
        {
            var QueryAdcionaBalao =
                "UPDATE candidatos SET balao='S' WHERE cpf='" + Cpf + "';";
            ConexaoMysql.ExecutaComandoComRetornoPortal(QueryAdcionaBalao);
        }

        public List<Dictionary<string, string>> RecuperaVagasId(string Id)
        {
            var QuerySelecionaVagas =
                "select id,titulo,descricao,areadeinteresse,ativa FROM vagas where id=" + Id + "";
            var DadosVagas = ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaVagas);
            return DadosVagas;
        }

        public List<Dictionary<string, string>> CadastrarVaga(string Descricao, string AreaInteresse, string Salario,
            string Requisito,
            string Titulo, string Beneficio)
        {
            var QuerySelecionaCurriculo =
                "INSERT INTO vagas (descricao,areadeinteresse,salario,requisito,titulo,beneficio,datainicio) VALUES('" +
                Descricao + "','" + AreaInteresse + "','" + Salario + "','" + Requisito + "','" + Titulo + "','" +
                Beneficio + "',now())";
            ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaCurriculo);
            var QuerySelecionaEmailVagas =
                "select a.email,a.telefoneprincipal from candidatos a, areasinteresses b WHERE a.id=b.idcandidato AND MATCH(b.descricao) AGAINST('" +
                AreaInteresse + "')";
            var Email = ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaEmailVagas);
            return Email;
        }

        public void IniciarProcessoSeletivo(string Cpf, string IdVaga)
        {
            var QuerySelecionaIdCandidato =
                "select id FROM candidatos where cpf='" + Cpf + "';";
            var DadosCandidatos = ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaIdCandidato);
            var QueryIniciaProcesso = "INSERT INTO processosseletivos (idvaga,idcandidato) VALUES(" + IdVaga + "," +
                                       DadosCandidatos[0]["id"] + ")";
            ConexaoMysql.ExecutaComandoComRetornoPortal(QueryIniciaProcesso);
        }

        public void AtualizarProcessoSeletivoStatus(string Cpf, string IdVaga, string Resultado, string Restricao)
        {
            var QuerySelecionaIdCandidato = "select id FROM candidatos where cpf='" + Cpf + "';";
            var DadosCandidatos = ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaIdCandidato);
            try
            {
                if (Restricao.Length > 0 || Restricao != null)
                {
                    var QueryIniciaProcesso = "Update processosseletivos set aprovado='" + Resultado +
                                               "' WHERE idvaga=" + IdVaga + " AND idcandidato=" +
                                               DadosCandidatos[0]["id"] + ", restricao='" + Restricao + "';";
                    ;
                    ConexaoMysql.ExecutaComandoComRetornoPortal(QueryIniciaProcesso);
                }
                else
                {
                    var QueryIniciaProcesso = "Update processosseletivos set aprovado='" + Resultado +
                                               "' WHERE idvaga=" + IdVaga + " AND idcandidato=" +
                                               DadosCandidatos[0]["id"] + ";";
                    ConexaoMysql.ExecutaComandoComRetornoPortal(QueryIniciaProcesso);
                }
            }
            catch
            {
                var QueryIniciaProcesso = "Update processosseletivos set aprovado='" + Resultado + "' WHERE idvaga=" +
                                           IdVaga + " AND idcandidato=" + DadosCandidatos[0]["id"] + ";";
                ConexaoMysql.ExecutaComandoComRetornoPortal(QueryIniciaProcesso);
            }

            var QueryMudaDataFim = "UPDATE vagas SET datafim=NOW() WHERE id=" + IdVaga + "";
            ConexaoMysql.ExecutaComandoComRetornoPortal(QueryMudaDataFim);
        }

        public void AtualizarProcessoSeletivoGerente(string Cpf, string IdVaga, string Resultado)
        {
            var QuerySelecionaIdCandidato =
                "select id FROM candidatos where cpf='" + Cpf + "';";
            var DadosCandidatos = ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaIdCandidato);
            var QueryIniciaProcesso = "Update processosseletivos set gerente= '" + Resultado + "' WHERE idvaga = " +
                                       IdVaga + " AND idcandidato = " + DadosCandidatos[0]["id"] + "; ";
            ConexaoMysql.ExecutaComandoComRetornoPortal(QueryIniciaProcesso);
        }

        public void AtualizaProcessoSeletivoPsicologico(string Cpf, string IdVaga, string Resultado)
        {
            var QuerySelecionaIdCandidato =
                "select id FROM candidatos where cpf='" + Cpf + "';";
            var DadosCandidatos = ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaIdCandidato);
            var QueryIniciaProcesso = "Update processosseletivos set psicologico='" + Resultado + "' WHERE idvaga=" +
                                       IdVaga + " AND idcandidato=" + DadosCandidatos[0]["id"] + ";";
            ConexaoMysql.ExecutaComandoComRetornoPortal(QueryIniciaProcesso);
        }

        public void AtualizaProcessoSeletivoTeorico(string Cpf, string IdVaga, string Resultado)
        {
            var QuerySelecionaIdCandidato =
                "select id FROM candidatos where cpf='" + Cpf + "';";
            var DadosCandidatos = ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaIdCandidato);
            var QueryIniciaProcesso = "Update processosseletivos set prova='" + Resultado + "' WHERE idvaga=" +
                                       IdVaga + " AND idcandidato=" + DadosCandidatos[0]["id"] + ";";
            ConexaoMysql.ExecutaComandoComRetornoPortal(QueryIniciaProcesso);
        }

        public void CadastrarAlerta(string Alerta)
        {
            var QuerySelecionaVagas =
                "select id FROM candidatos";
            var DadosCandidatos = ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaVagas);

            for (var i = 0; i < DadosCandidatos.Count; i++)
            {
                var QuerySelecionaCurriculo =
                    "INSERT INTO alertas (alerta,idcandidato) VALUES('" + Alerta + "'," + DadosCandidatos[i]["id"] +
                    ")";
                ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaCurriculo);
            }
        }

        public void CadastrarAlertaEspecifico(string Alerta, string IdCandidato)
        {
            var QuerySelecionaCurriculo =
                "INSERT INTO alertas (alerta,idcandidato) VALUES('" + Alerta + "'," + IdCandidato +
                ")";
            ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaCurriculo);
        }

        public void CadastrarMensagem(string Mensagem)
        {
            var QuerySelecionaVagas =
                "select id FROM candidatos";
            var DadosCandidatos = ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaVagas);

            for (var i = 0; i < DadosCandidatos.Count; i++)
            {
                var QuerySelecionaCurriculo =
                    "INSERT INTO mensagens (mensagem,idcandidato) VALUES('" + Mensagem + "'," +
                    DadosCandidatos[i]["id"] + ")";
                ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaCurriculo);
            }
        }

        public void EncerrarVaga(string IdVaga)
        {
            var QuerySelecionaCurriculo = "UPDATE vagas SET ativa='N' WHERE id=" + IdVaga + "";
            ConexaoMysql.ExecutaComandoComRetornoPortal(QuerySelecionaCurriculo);
        }

        public List<Dictionary<string, string>> RecuperaDadosCandidato(string Cpf)
        {
            var QueryRecuperaUsuario =
                "SELECT a.*,b.descricao as descricaoestadocivil, c.descricao as escolaridade FROM candidatos a, tiposestadoscivis b, tiposescolaridades c WHERE b.id = a.estadocivil AND c.id = a.idtipoescolaridade AND a.cpf = '" +
                Cpf + "'";


            var Dados = ConexaoMysql.ExecutaComandoComRetornoPortal(QueryRecuperaUsuario);


            return Dados;
        }

        public List<Dictionary<string, string>> RecuperaQuestionario(string IdCandidato)
        {
            var QueryRecuperaUsuario =
                "SELECT * FROM formulariosiniciais WHERE idcandidato='" +
                IdCandidato + "'";


            var Dados = ConexaoMysql.ExecutaComandoComRetornoPortal(QueryRecuperaUsuario);


            return Dados;
        }

        public List<Dictionary<string, string>> RecuperaDadosUsuariosProissional(string IdCandidato)
        {
            var QueryRecuperaProfissional = "SELECT * FROM dadosprofissionais WHERE idcandidato='" + IdCandidato + "'";


            var Dados = ConexaoMysql.ExecutaComandoComRetornoPortal(QueryRecuperaProfissional);


            return Dados;
        }

        public List<Dictionary<string, string>> RecuperaDadosUsuariosEducacional(string IdCandidato)
        {
            var QueryRecuperaEducacional = "SELECT * FROM dadosescolares WHERE idcandidato='" + IdCandidato + "'";


            var Dados = ConexaoMysql.ExecutaComandoComRetornoPortal(QueryRecuperaEducacional);


            return Dados;
        }

        public List<Dictionary<string, string>> RecuperaEmail(string Cpf)
        {
            var QueryRecuperaEducacional = "SELECT id,email,telefoneprincipal FROM candidatos WHERE cpf='" + Cpf + "'";


            var Dados = ConexaoMysql.ExecutaComandoComRetornoPortal(QueryRecuperaEducacional);


            return Dados;
        }

        public List<Dictionary<string, string>> RecuperaAreaInteresseUsuarios(string IdCandidato)
        {
            var QueryRecuperaAreasInteresse = "SELECT * FROM areasinteresses WHERE idcandidato=" + IdCandidato + "";


            var Dados = ConexaoMysql.ExecutaComandoComRetornoPortal(QueryRecuperaAreasInteresse);


            return Dados;
        }

        public string RecuperaEstadoCivilCandidato(string Id)
        {
            var QueryRecuperaAreasInteresse = "SELECT * FROM tiposestadoscivis WHERE id=" + Id + "";


            var Dados = ConexaoMysql.ExecutaComandoComRetornoPortal(QueryRecuperaAreasInteresse);


            return Dados[0]["descricao"];
        }

        public string RecuperaEscolaridadeCandidato(string Id)
        {
            var QueryRecuperaAreasInteresse = "SELECT * FROM tiposescolaridades WHERE id=" + Id + "";


            var Dados = ConexaoMysql.ExecutaComandoComRetornoPortal(QueryRecuperaAreasInteresse);


            return Dados[0]["descricao"];
        }


        public List<Dictionary<string, string>> RecuperaIdEscolaridade(string Descricao)
        {
            var QueryRecuperaAreasInteresse =
                "SELECT * FROM tiposescolaridades WHERE descricao like'%" + Descricao + "%'";


            var Dados = ConexaoMysql.ExecutaComandoComRetornoPortal(QueryRecuperaAreasInteresse);


            return Dados;
        }

        public List<Dictionary<string, string>> FiltroVaga(string Query)
        {
            var DadosCurriculos = ConexaoMysql.ExecutaComandoComRetornoPortal(Query);
            return DadosCurriculos;
        }
    }
}