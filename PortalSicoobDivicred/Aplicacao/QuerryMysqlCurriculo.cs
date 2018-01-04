using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Port.Repositorios;

namespace PortalSicoobDivicred.Aplicacao
{
    public class QuerryMysqlCurriculo
    {
        private readonly Conexao contexto;


        public QuerryMysqlCurriculo()
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
        public List<Dictionary<string, string>> RecuperaCurriculos()
        {
            var QuerrySelecionaCurriculo =
                "select a.nome,a.email,b.descricao,if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Reprovado')>0,'N',if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Ausente')>0,'A',if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Excedente')>0,'E','S'))) as status, a.idarquivogoogle,a.cpf,a.cidade from candidatos a , areasinteresses b where a.id=b.idcandidato";
            var DadosCurriculos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaCurriculo);
            return DadosCurriculos;
        }
        public List<Dictionary<string, string>> FormularioRecrutamentoSelecaoVaga(string IdVaga)
        {
            var QuerrySelecionaCurriculo =
                "select titulo,DATE_FORMAT(datainicio,'%d/%m/%Y') as datainicio from vagas where id=" + IdVaga + " ;";
            var DadosCurriculos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaCurriculo);
            return DadosCurriculos;
        }
        public List<Dictionary<string, string>> FormularioRecrutamentoSelecaoHistorico(string IdVaga)
        {
            var QuerrySelecionaCurriculo =
                "select count(id) as count from historicos where idvaga=" + IdVaga + "";
            var DadosCurriculos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaCurriculo);
            return DadosCurriculos;
        }
        public List<Dictionary<string, string>> FormularioRecrutamentoSelecaoProcesso(string IdVaga)
        {
            var QuerrySelecionaCurriculo =
                "select count(id) as count from processosseletivos where idvaga=" + IdVaga + ";";
            var DadosCurriculos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaCurriculo);
            return DadosCurriculos;
        }
        public List<Dictionary<string, string>> ExisteFormularioRecrutamentoSelecaoProcesso(string IdVaga)
        {
            var QuerrySelecionaCurriculo =
                "select count(id) as count from recrutamentoselecao where idvaga=" + IdVaga + ";";
            var DadosCurriculos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaCurriculo);
            return DadosCurriculos;
        }
        public List<Dictionary<string, string>> FormularioRecrutamentoSelecao(string IdVaga)
        {
            var QuerrySelecionaCurriculo =
                "select * from recrutamentoselecao where idvaga=" + IdVaga + ";";
            var DadosCurriculos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaCurriculo);
            return DadosCurriculos;
        }

        public void InserirFormularioRecrutamentoSelecao(string IdVaga, string ClasseCargo, string NivelCargo, string NumeroVaga, string ChefiaImediata, string MesAdmissao, string MotivoSelecao, string EmpregadoSubstituido, string PrevisaoOrcamento, string FormaRecrutamento, string DinamicaGrupo, string DinamicaNumero, string DinamicaPreNumero, string ConhecimentoTeste, string ConhecimentoNumero, string ConhecimentoPreNumero, string PsicologicoTeste, string PsicologicoNumero, string PsicologicoPreNumero, string Psicologaentrevistador, string PsicologaNumero, string PsicologaPreNumero, string Setor)
        {
            var QuerrySelecionaCurriculo = "INSERT INTO recrutamentoselecao (idvaga,classecargo,nivelcargo,numerovaga,chefiaimediata,mesadmissao,motivoselecao,nomeempregadosubstituido,previsaoorcamento,formarecrutamento,dinamicagrupo,dinamicanumero,dinamicaprenumero,conhecimentoteste,conhecimentonumero,conhecimentoprenumero,psicologicoteste,psicologiconumero,psicologicoprenumero,psicologaentrevistador,psicologanumero,psicologaprenumero,setor) VALUES(" + IdVaga + ",'" + ClasseCargo + "','" + NivelCargo + "'," + NumeroVaga + ",'" + ChefiaImediata + "','" + MesAdmissao + "','" + MotivoSelecao + "','" + EmpregadoSubstituido + "','" + PrevisaoOrcamento + "','" + FormaRecrutamento + "','" + DinamicaGrupo + "'," + DinamicaNumero + "," + DinamicaPreNumero + ",'" + ConhecimentoTeste + "'," + ConhecimentoNumero + "," + ConhecimentoPreNumero + ",'" + PsicologicoTeste + "'," + PsicologicoNumero + "," + PsicologicoPreNumero + ",'" + Psicologaentrevistador + "'," + PsicologaNumero + "," + PsicologaPreNumero + ", '" + Setor + "');";
            var DadosCurriculos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaCurriculo);

        }
        public void AtualizarFormularioRecrutamentoSelecao(string IdVaga, string ClasseCargo, string NivelCargo, string NumeroVaga, string ChefiaImediata, string MesAdmissao, string MotivoSelecao, string EmpregadoSubstituido, string PrevisaoOrcamento, string FormaRecrutamento, string DinamicaGrupo, string DinamicaNumero, string DinamicaPreNumero, string ConhecimentoTeste, string ConhecimentoNumero, string ConhecimentoPreNumero, string PsicologicoTeste, string PsicologicoNumero, string PsicologicoPreNumero, string Psicologaentrevistador, string PsicologaNumero, string PsicologaPreNumero, string Setor)
        {
            var QuerrySelecionaCurriculo = "UPDATE recrutamentoselecao set classecargo='" + ClasseCargo + "',nivelcargo='" + NivelCargo + "',numerovaga=" + NumeroVaga + ",chefiaimediata='" + ChefiaImediata + "',mesadmissao='" + MesAdmissao + "',motivoselecao='" + MotivoSelecao + "',nomeempregadosubstituido='" + EmpregadoSubstituido + "',previsaoorcamento='" + PrevisaoOrcamento + "',formarecrutamento='" + FormaRecrutamento + "',dinamicagrupo='" + DinamicaGrupo + "',dinamicanumero=" + DinamicaNumero + ",dinamicaprenumero=" + DinamicaPreNumero + ",conhecimentoteste='" + ConhecimentoTeste + "',conhecimentonumero=" + ConhecimentoNumero + ",conhecimentoprenumero=" + ConhecimentoPreNumero + ",psicologicoteste='" + PsicologicoTeste + "',psicologiconumero=" + PsicologicoNumero + ",psicologicoprenumero=" + PsicologicoPreNumero + ",psicologaentrevistador='" + Psicologaentrevistador + "',psicologanumero=" + PsicologaNumero + ",psicologaprenumero=" + PsicologaPreNumero + ",setor='" + Setor + "' WHERE idvaga=" + IdVaga + ";";
            var DadosCurriculos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaCurriculo);

        }



        public List<Dictionary<string, string>> FormularioParecerProcessoDadosPessoais(string Cpf, string IdVaga)
        {
            var QuerrySelecionaCurriculo = "select a.nome,DATE_FORMAT(a.datanascimento,'%d/%m/%Y') as nascimento, b.descricao as escolaridade, c.titulo,c.salario from candidatos a, tiposescolaridades b,vagas c, processosseletivos d where a.idtipoescolaridade=b.id and a.id=d.idcandidato and c.id=d.idvaga and a.cpf='" + Cpf + "' and d.idvaga=" + IdVaga + "";
            var DadosCurriculos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaCurriculo);
            return DadosCurriculos;
        }
        public List<Dictionary<string, string>> FormularioParecerProcessoDadosEscolares(string Cpf)
        {
            var QuerrySelecionaCurriculo = "select b.nomecurso, b.anofim from candidatos a, dadosescolares b where a.cpf='" +
                                           Cpf + "' and b.idcandidato=a.id";
            var DadosCurriculos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaCurriculo);
            return DadosCurriculos;
        }
        public List<Dictionary<string, string>> FormularioParecerProcessoDadosProfissionais(string Cpf)
        {
            var QuerrySelecionaCurriculo = "select b.nomeempresa, b.nomecargo from candidatos a, dadosprofissionais b where a.cpf='" + Cpf + "' and b.idcandidato=a.id";
            var DadosCurriculos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaCurriculo);
            return DadosCurriculos;
        }
        public void InserirFormularioParecerProcesso(string Solicitante, string metodologiaprocesso, string demandaparecer, string TipoRecrutamento, string PerfilTecnico, string Conclusao, string Cpf, string IdVaga)
        {
            var QuerrySelecionaIdCandidato =
                "select id FROM candidatos where cpf='" + Cpf + "';";
            var DadosCandidatos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaIdCandidato);

            var QuerrySelecionaCurriculo = "INSERT INTO parecerprocessoseletivo (idcandidato,solicitante,idvaga,metodologiaprocesso,demandaparecer,tiporecrutamento,perfiltecnico,conclusao) VALUES(" + DadosCandidatos[0]["id"] + ",'" + Solicitante + "'," + IdVaga + ",'" + metodologiaprocesso + "','" + demandaparecer + "','" + TipoRecrutamento + "','" + PerfilTecnico + "','" + Conclusao + "')";
            contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaCurriculo);

        }

        public void AtualizarFormularioParecerProcesso(string Solicitante, string metodologiaprocesso, string demandaparecer, string TipoRecrutamento, string PerfilTecnico, string Conclusao, string Cpf, string IdVaga)
        {
            var QuerrySelecionaIdCandidato =
                "select id FROM candidatos where cpf='" + Cpf + "';";
            var DadosCandidatos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaIdCandidato);

            var QuerrySelecionaCurriculo = "UPDATE parecerprocessoseletivo SET solicitante='" + Solicitante + "',metodologiaprocesso='" + metodologiaprocesso + "',demandaparecer='" + demandaparecer + "',tiporecrutamento='" + TipoRecrutamento + "',perfiltecnico='" + PerfilTecnico + "',conclusao='" + Conclusao + "' WHERE idcandidato=" + DadosCandidatos[0]["id"] + " AND idvaga=" + IdVaga + ";";
            contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaCurriculo);

        }
        public List<Dictionary<string, string>> RecuperaFormularioParecerProcesso(string Cpf, string IdVaga)
        {
            var QuerrySelecionaIdCandidato =
                "select id FROM candidatos where cpf='" + Cpf + "';";
            var DadosCandidatos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaIdCandidato);

            var QuerrySelecionaCurriculo = "select * from  parecerprocessoseletivo WHERE idcandidato=" + DadosCandidatos[0]["id"] + " AND idvaga=" + IdVaga + ";";
            var Dados = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaCurriculo);
            return Dados;

        }

        public List<Dictionary<string, string>> RecuperaVagaEspecifica()
        {
            var QuerrySelecionaCurriculo =
                "select id,descricao from vagas where areadeinteresse='Especifica' AND ativa='S' ";
            var DadosCurriculos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaCurriculo);
            return DadosCurriculos;
        }

        public void AtribuirVaga(string Cpf, string IdVaga)
        {
            var QuerrySelecionaIdCandidato =
                "select id FROM candidatos where cpf='" + Cpf + "';";
            var DadosCandidatos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaIdCandidato);
            var QuerrySelecionaCurriculo =
                "INSERT INTO historicos (idvaga,idcandidato,feedback) VALUES(" + IdVaga + "," + DadosCandidatos[0]["id"] + ",'Obrigado pelo interesse entraremos em contato em breve') ";

            contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaCurriculo);

        }

        public List<Dictionary<string, string>> RecuperaCurriculosArea(string Area, string Cidade, string Certificacao)
        {
            var QuerrySelecionaCurriculo =
                "select a.nome,a.email,b.descricao,if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Reprovado')>0,'N',if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Ausente')>0,'A',if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Excedente')>0,'E','S'))) as status, a.idarquivogoogle,a.cpf,a.cidade from candidatos a , areasinteresses b where b.descricao like'%" +
                Area + "%' AND a.id=b.idcandidato AND a.cidade like'%" + Cidade + "%' AND a.certificacao like'%" + Certificacao + "%'";
            var DadosCurriculos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaCurriculo);
            return DadosCurriculos;
        }


        public List<Dictionary<string, string>> RecuperaCurriculosProcesso(string IdVaga)
        {
            var QuerrySelecionaCurriculo =
                "select a.nome,a.cpf,a.email,b.* from candidatos a, processosseletivos b where a.id=b.idcandidato and b.idvaga=" + IdVaga + ";";
            var DadosCurriculos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaCurriculo);
            return DadosCurriculos;
        }

        public bool VerificaProcesso(string IdVaga)
        {
            var QuerrySelecionaCurriculo =
                "select * from processosseletivos where idvaga=" + IdVaga + "";
            var DadosVaga = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaCurriculo);
            if (DadosVaga.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void AtualizarVaga(string IdVaga, string Descricao, string Salario, string Requisito, string Titulo, string Beneficio, string Ativa, string Areas)
        {
            var QuerrySelecionaCurriculo = "UPDATE vagas SET descricao='" + Descricao + "',salario='" + Salario.Replace(",", ".") + "',requisito='" + Requisito + "',titulo='" + Titulo + "', beneficio='" + Beneficio + "', ativa='" + Ativa + "', areadeinteresse='" + Areas + "' where id=" + IdVaga + ";";
            contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaCurriculo);

        }
        public List<Dictionary<string, string>> RecuperaDadosVaga(string IdVaga)
        {
            var QuerrySelecionaCurriculo = "SELECT * from vagas where id=" + IdVaga + ";";
            var DadosCurriculos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaCurriculo);
            return DadosCurriculos;
        }
        public List<Dictionary<string, string>> RecuperaProcesso(string IdCandidato)
        {
            var QuerrySelecionaCurriculo =
                "SELECT a.*,b.titulo as nomevaga from processosseletivos a , vagas b where a.idvaga=b.id and idcandidato=" + IdCandidato + ";";
            var DadosCurriculos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaCurriculo);
            return DadosCurriculos;
        }
        public List<Dictionary<string, string>> RecuperaFormulario(string IdCandidato)
        {
            var QuerrySelecionaCurriculo =
                "SELECT * from formulariosiniciais where  idcandidato=" + IdCandidato + ";";
            var DadosCurriculos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaCurriculo);
            return DadosCurriculos;
        }

        public List<Dictionary<string, string>> RecuperaCurriculosAlfabetico()
        {
            var QuerrySelecionaCurriculo =
                "select a.nome,a.email,b.descricao,if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Reprovado')>0,'N',if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Ausente')>0,'A',if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Excedente')>0,'E','S'))) as status, a.idarquivogoogle,a.cpf,a.cidade from candidatos a , areasinteresses b where a.id=b.idcandidato  order by a.nome asc  ";
            var DadosCurriculos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaCurriculo);
            return DadosCurriculos;
        }

        public List<Dictionary<string, string>> RecuperaCurriculosStatus()
        {
            var QuerrySelecionaCurriculo =
                "select a.nome,a.email,b.descricao,if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Reprovado')>0,'N',if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Ausente')>0,'A',if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='Excedente')>0,'E','S'))) as status, a.idarquivogoogle,a.cpf,a.cidade from candidatos a , areasinteresses b where a.id=b.idcandidato order by status asc ";
            var DadosCurriculos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaCurriculo);
            return DadosCurriculos;
        }


        public List<Dictionary<string, string>> RecuperaCurriculosHistorico(string IdVaga)
        {
            var QuerrySelecionaCurriculo =
                "select a.cpf, a.nome,a.email, a.idarquivogoogle,a.cidade,a.certificacao,a.telefoneprincipal from historicos c LEFT JOIN candidatos a on c.idcandidato=a.id INNER JOIN candidatos u2 on (c.idcandidato=u2.id) where c.idvaga=" +
                IdVaga + "";
            var DadosCurriculos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaCurriculo);
            return DadosCurriculos;
        }


        public List<Dictionary<string, string>> RecuperaVagas()
        {
            var QuerrySelecionaVagas =
                "select id,titulo,descricao,areadeinteresse,ativa FROM vagas ORDER BY ativa DESC";
            var DadosVagas = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaVagas);
            return DadosVagas;
        }
        public void CriaBalao(string Cpf)
        {
            var QuerryAdcionaBalao =
                "UPDATE candidatos SET balao='S' WHERE cpf='" + Cpf + "';";
            contexto.ExecutaComandoComRetornoPortal(QuerryAdcionaBalao);

        }

        public List<Dictionary<string, string>> RecuperaVagasId(string Id)
        {
            var QuerrySelecionaVagas =
                "select id,titulo,descricao,areadeinteresse,ativa FROM vagas where id=" + Id + "";
            var DadosVagas = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaVagas);
            return DadosVagas;
        }

        public List<Dictionary<string, string>> CadastrarVaga(string Descricao, string AreaInteresse, string Salario, string Requisito,
           string Titulo, string Beneficio)
        {
            var QuerrySelecionaCurriculo =
                "INSERT INTO vagas (descricao,areadeinteresse,salario,requisito,titulo,beneficio,datainicio) VALUES('" +
                Descricao + "','" + AreaInteresse + "','" + Salario + "','" + Requisito + "','" + Titulo + "','" +
                Beneficio + "',now())";
            contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaCurriculo);
            var QuerrySelecionaEmailVagas =
                "select a.email,a.telefoneprincipal from candidatos a, areasinteresses b WHERE a.id=b.idcandidato AND MATCH(b.descricao) AGAINST('" + AreaInteresse + "')";
            var Email = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaEmailVagas);
            return Email;
        }

        public void IniciarProcessoSeletivo(string Cpf, string IdVaga)
        {
            var QuerrySelecionaIdCandidato =
                "select id FROM candidatos where cpf='" + Cpf + "';";
            var DadosCandidatos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaIdCandidato);
            var QuerryIniciaProcesso = "INSERT INTO processosseletivos (idvaga,idcandidato) VALUES(" + IdVaga + "," +
                                       DadosCandidatos[0]["id"] + ")";
            contexto.ExecutaComandoComRetornoPortal(QuerryIniciaProcesso);
        }
        public void AtualizarProcessoSeletivoStatus(string Cpf, string IdVaga, string Resultado, string Restricao)
        {
            var QuerrySelecionaIdCandidato = "select id FROM candidatos where cpf='" + Cpf + "';";
            var DadosCandidatos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaIdCandidato);
            try
            {
                if (Restricao.Length > 0 || Restricao != null)
                {
                    var QuerryIniciaProcesso = "Update processosseletivos set aprovado='" + Resultado + "' WHERE idvaga=" + IdVaga + " AND idcandidato=" + DadosCandidatos[0]["id"] + ", restricao='" + Restricao + "';"; ;
                    contexto.ExecutaComandoComRetornoPortal(QuerryIniciaProcesso);
                }
                else
                {
                    var QuerryIniciaProcesso = "Update processosseletivos set aprovado='" + Resultado + "' WHERE idvaga=" + IdVaga + " AND idcandidato=" + DadosCandidatos[0]["id"] + ";";
                    contexto.ExecutaComandoComRetornoPortal(QuerryIniciaProcesso);
                }
            }
            catch
            {
                var QuerryIniciaProcesso = "Update processosseletivos set aprovado='" + Resultado + "' WHERE idvaga=" + IdVaga + " AND idcandidato=" + DadosCandidatos[0]["id"] + ";";
                contexto.ExecutaComandoComRetornoPortal(QuerryIniciaProcesso);
            }

            var QueryMudaDataFim = "UPDATE vagas SET datafim=NOW() WHERE id=" + IdVaga + "";
            contexto.ExecutaComandoComRetornoPortal(QueryMudaDataFim);

        }
        public void AtualizarProcessoSeletivoGerente(string Cpf, string IdVaga, string Resultado)
        {
            var QuerrySelecionaIdCandidato =
                "select id FROM candidatos where cpf='" + Cpf + "';";
            var DadosCandidatos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaIdCandidato);
            var QuerryIniciaProcesso = "Update processosseletivos set gerente= '" + Resultado + "' WHERE idvaga = " + IdVaga + " AND idcandidato = " + DadosCandidatos[0]["id"] + "; ";
            contexto.ExecutaComandoComRetornoPortal(QuerryIniciaProcesso);
        }
        public void AtualizaProcessoSeletivoPsicologico(string Cpf, string IdVaga, string Resultado)
        {
            var QuerrySelecionaIdCandidato =
                "select id FROM candidatos where cpf='" + Cpf + "';";
            var DadosCandidatos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaIdCandidato);
            var QuerryIniciaProcesso = "Update processosseletivos set psicologico='" + Resultado + "' WHERE idvaga=" + IdVaga + " AND idcandidato=" + DadosCandidatos[0]["id"] + ";";
            contexto.ExecutaComandoComRetornoPortal(QuerryIniciaProcesso);
        }
        public void AtualizaProcessoSeletivoTeorico(string Cpf, string IdVaga, string Resultado)
        {
            var QuerrySelecionaIdCandidato =
                "select id FROM candidatos where cpf='" + Cpf + "';";
            var DadosCandidatos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaIdCandidato);
            var QuerryIniciaProcesso = "Update processosseletivos set prova='" + Resultado + "' WHERE idvaga=" + IdVaga + " AND idcandidato=" + DadosCandidatos[0]["id"] + ";";
            contexto.ExecutaComandoComRetornoPortal(QuerryIniciaProcesso);
        }
        public void CadastrarAlerta(string Alerta)
        {
            var QuerrySelecionaVagas =
                "select id FROM candidatos";
            var DadosCandidatos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaVagas);

            for (var i = 0; i < DadosCandidatos.Count; i++)
            {
                var QuerrySelecionaCurriculo =
                    "INSERT INTO alertas (alerta,idcandidato) VALUES('" + Alerta + "'," + DadosCandidatos[i]["id"] +
                    ")";
                contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaCurriculo);
            }
        }
        public void CadastrarAlertaEspecifico(string Alerta, string IdCandidato)
        {


            var QuerrySelecionaCurriculo =
                "INSERT INTO alertas (alerta,idcandidato) VALUES('" + Alerta + "'," + IdCandidato +
                ")";
            contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaCurriculo);

        }

        public void CadastrarMensagem(string Mensagem)
        {
            var QuerrySelecionaVagas =
                "select id FROM candidatos";
            var DadosCandidatos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaVagas);

            for (var i = 0; i < DadosCandidatos.Count; i++)
            {
                var QuerrySelecionaCurriculo =
                    "INSERT INTO mensagens (mensagem,idcandidato) VALUES('" + Mensagem + "'," +
                    DadosCandidatos[i]["id"] + ")";
                contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaCurriculo);
            }
        }

        public void EncerrarVaga(string IdVaga)
        {
            var QuerrySelecionaCurriculo = "UPDATE vagas SET ativa='N' WHERE id=" + IdVaga + "";
            contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaCurriculo);
        }
        public List<Dictionary<string, string>> RecuperaDadosCandidato(string Cpf)
        {
            var QuerryRecuperaUsuario =
                "SELECT a.*,b.descricao as descricaoestadocivil, c.descricao as escolaridade FROM candidatos a, tiposestadoscivis b, tiposescolaridades c WHERE b.id = a.estadocivil AND c.id = a.idtipoescolaridade AND a.cpf = '" +
                Cpf + "'";


            var rows = contexto.ExecutaComandoComRetornoPortal(QuerryRecuperaUsuario);


            return rows;
        }

        public List<Dictionary<string, string>> RecuperaQuestionario(string IdCandidato)
        {
            var QuerryRecuperaUsuario =
                "SELECT * FROM formulariosiniciais WHERE idcandidato='" +
                IdCandidato + "'";


            var rows = contexto.ExecutaComandoComRetornoPortal(QuerryRecuperaUsuario);


            return rows;
        }

        public List<Dictionary<string, string>> RecuperaDadosUsuariosProissional(string IdCandidato)
        {
            var QuerryRecuperaProfissional = "SELECT * FROM dadosprofissionais WHERE idcandidato='" + IdCandidato + "'";


            var rows = contexto.ExecutaComandoComRetornoPortal(QuerryRecuperaProfissional);


            return rows;
        }

        public List<Dictionary<string, string>> RecuperaDadosUsuariosEducacional(string IdCandidato)
        {
            var QuerryRecuperaEducacional = "SELECT * FROM dadosescolares WHERE idcandidato='" + IdCandidato + "'";


            var rows = contexto.ExecutaComandoComRetornoPortal(QuerryRecuperaEducacional);


            return rows;
        }
        public List<Dictionary<string, string>> RecuperaEmail(string Cpf)
        {
            var QuerryRecuperaEducacional = "SELECT id,email,telefoneprincipal FROM candidatos WHERE cpf='" + Cpf + "'";


            var rows = contexto.ExecutaComandoComRetornoPortal(QuerryRecuperaEducacional);


            return rows;
        }
        public List<Dictionary<string, string>> RecuperaAreaInteresseUsuarios(string IdCandidato)
        {
            var QuerryRecuperaAreasInteresse = "SELECT * FROM areasinteresses WHERE idcandidato=" + IdCandidato + "";


            var rows = contexto.ExecutaComandoComRetornoPortal(QuerryRecuperaAreasInteresse);


            return rows;
        }

        public string RecuperaEstadoCivilCandidato(string Id)
        {
            var QuerryRecuperaAreasInteresse = "SELECT * FROM tiposestadoscivis WHERE id=" + Id + "";


            var rows = contexto.ExecutaComandoComRetornoPortal(QuerryRecuperaAreasInteresse);


            return rows[0]["descricao"];
        }

        public string RecuperaEscolaridadeCandidato(string Id)
        {
            var QuerryRecuperaAreasInteresse = "SELECT * FROM tiposescolaridades WHERE id=" + Id + "";


            var rows = contexto.ExecutaComandoComRetornoPortal(QuerryRecuperaAreasInteresse);


            return rows[0]["descricao"];
        }


        public List<Dictionary<string, string>> RecuperaIdEscolaridade(string Descricao)
        {
            var QuerryRecuperaAreasInteresse =
                "SELECT * FROM tiposescolaridades WHERE descricao like'%" + Descricao + "%'";


            var rows = contexto.ExecutaComandoComRetornoPortal(QuerryRecuperaAreasInteresse);


            return rows;
        }

        public List<Dictionary<string, string>> FiltroVaga(string Query)
        {
            var DadosCurriculos = contexto.ExecutaComandoComRetornoPortal(Query);
            return DadosCurriculos;
        }



    }
}