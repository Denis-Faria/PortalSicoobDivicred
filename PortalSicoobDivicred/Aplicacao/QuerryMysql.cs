﻿using System;
using System.Collections.Generic;
using System.Web;
using Port.Repositorios;
using PortalSicoobDivicred.Models;

namespace PortalSicoobDivicred.Aplicacao
{
    public class QuerryMysql
    {
        private readonly Conexao contexto;


        public QuerryMysql()
        {
            contexto = new Conexao();
        }

        public bool ConfirmaLogin(string Usuario, string Senha)
        {
            var QuerryConfirmaLogin = "SELECT login FROM usuarios WHERE login='" + Usuario + "' AND senha=MD5('" +
                                      Senha +
                                      "')";

            var rows = contexto.ExecutaComandoComRetorno(QuerryConfirmaLogin);

            if (rows.Count == 0)
                return false;
            var CookieUsuario = new HttpCookie("CookieFarm");
            CookieUsuario.Value = Criptografa.Criptografar(rows[0]["login"]);
            CookieUsuario.Expires = DateTime.Now.AddHours(1);
            HttpContext.Current.Response.Cookies.Add(CookieUsuario);
            return true;
        }


        public List<Dictionary<string, string>> RecuperaCurriculos()
        {
            var QuerrySelecionaCurriculo =
                "select a.nome,a.email,b.descricao,if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='N')>0,'N','S') as status, a.idarquivogoogle,a.cpf,a.cidade from candidatos a , areasinteresses b where a.id=b.idcandidato";
            var DadosCurriculos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaCurriculo);
            return DadosCurriculos;
        }
        public List<Dictionary<string, string>> RecuperaCurriculosProcesso(string IdVaga)
        {
            var QuerrySelecionaCurriculo =
                "select a.nome,a.cpf,a.email,b.* from candidatos a, processosseletivos b where a.id=b.idcandidato and b.idvaga="+IdVaga+ " and (aprovado!='Reprovado' or aprovado is null);";
            var DadosCurriculos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaCurriculo);
            return DadosCurriculos;
        }

        public bool VerificaProcesso(string IdVaga)
        {
            var QuerrySelecionaCurriculo =
                "select * from processosseletivos where idvaga="+IdVaga+"";
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
        public List<Dictionary<string, string>> RecuperaProcesso(string IdCandidato)
        {
            var QuerrySelecionaCurriculo =
                "SELECT a.*,b.titulo as nomevaga from processosseletivos a , vagas b where a.idvaga=b.id and idcandidato="+IdCandidato+";";
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
                "select a.nome,a.email,b.descricao,if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='N')>0,'N','S') as status, a.idarquivogoogle,a.cpf,a.cidade from candidatos a , areasinteresses b where a.id=b.idcandidato  order by a.nome asc  ";
            var DadosCurriculos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaCurriculo);
            return DadosCurriculos;
        }

        public List<Dictionary<string, string>> RecuperaCurriculosStatus()
        {
            var QuerrySelecionaCurriculo =
                "select a.nome,a.email,b.descricao,if((select count(id) from processosseletivos where idcandidato=a.id AND aprovado='N')>0,'N','S') as status, a.idarquivogoogle,a.cpf,a.cidade from candidatos a , areasinteresses b where a.id=b.idcandidato order by status asc ";
            var DadosCurriculos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaCurriculo);
            return DadosCurriculos;
        }


        public List<Dictionary<string, string>> RecuperaCurriculosHistorico(string IdVaga)
        {
            var QuerrySelecionaCurriculo =
                "select a.cpf, a.nome,a.email, a.idarquivogoogle,a.cidade,a.certificacao from historicos c LEFT JOIN candidatos a on c.idcandidato=a.id INNER JOIN candidatos u2 on (c.idcandidato=u2.id) where c.idvaga=" +
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
                "UPDATE candidatos SET balao='S' WHERE cpf="+Cpf+";";
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
                "select a.email from candidatos a, areasinteresses b WHERE a.id=b.idcandidato AND b.descricao like'%"+AreaInteresse+"%'";
            var Email = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaEmailVagas);
            return Email;
        }

        public void IniciarProcessoSeletivo(string Cpf,string IdVaga)
        {
            var QuerrySelecionaIdCandidato =
                "select id FROM candidatos where cpf='"+Cpf+"';";
            var DadosCandidatos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaIdCandidato);
            var QuerryIniciaProcesso = "INSERT INTO processosseletivos (idvaga,idcandidato) VALUES(" + IdVaga + "," +
                                       DadosCandidatos[0]["id"] + ")";
            contexto.ExecutaComandoComRetornoPortal(QuerryIniciaProcesso);
        }
        public void AtualizarProcessoSeletivoStatus(string Cpf, string IdVaga,string Resultado)
        {
            var QuerrySelecionaIdCandidato = "select id FROM candidatos where cpf='" + Cpf + "';";
            var DadosCandidatos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaIdCandidato);

            var QuerryIniciaProcesso = "Update processosseletivos set aprovado='" + Resultado + "' WHERE idvaga=" + IdVaga + " AND idcandidato=" + DadosCandidatos[0]["id"] + ";"; ;
            contexto.ExecutaComandoComRetornoPortal(QuerryIniciaProcesso);

            var QueryMudaDataFim = "UPDATE vagas SET datafim=NOW() WHERE id="+IdVaga+"";
            contexto.ExecutaComandoComRetornoPortal(QueryMudaDataFim);

        }
        public void AtualizarProcessoSeletivoGerente(string Cpf, string IdVaga,string Resultado)
        {
            var QuerrySelecionaIdCandidato =
                "select id FROM candidatos where cpf='" + Cpf + "';";
            var DadosCandidatos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaIdCandidato);
            var QuerryIniciaProcesso = "Update processosseletivos set gerente= '"+Resultado+"' WHERE idvaga = "+IdVaga+" AND idcandidato = "+ DadosCandidatos[0]["id"] + "; ";
            contexto.ExecutaComandoComRetornoPortal(QuerryIniciaProcesso);
        }
        public void AtualizaProcessoSeletivoPsicologico(string Cpf, string IdVaga,string Resultado)
        {
            var QuerrySelecionaIdCandidato =
                "select id FROM candidatos where cpf='" + Cpf + "';";
            var DadosCandidatos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaIdCandidato);
            var QuerryIniciaProcesso = "Update processosseletivos set psicologico='" + Resultado + "' WHERE idvaga=" + IdVaga + " AND idcandidato=" + DadosCandidatos[0]["id"] + ";";
            contexto.ExecutaComandoComRetornoPortal(QuerryIniciaProcesso);
        }
        public void AtualizaProcessoSeletivoTeorico(string Cpf, string IdVaga,string Resultado)
        {
            var QuerrySelecionaIdCandidato =
                "select id FROM candidatos where cpf='" + Cpf + "';";
            var DadosCandidatos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaIdCandidato);
            var QuerryIniciaProcesso = "Update processosseletivos set prova="+Resultado+" WHERE idvaga="+IdVaga+" AND idcandidato="+ DadosCandidatos[0]["id"] + ";";
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
        public void CadastrarAlertaEspecifico(string Alerta,string IdCandidato)
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


        public bool UsuarioLogado()
        {
            var Usuario = HttpContext.Current.Request.Cookies["CookieFarm"];
            if (Usuario == null)
                return false;
            return true;
        }

        public List<Dictionary<string, string>> RecuperaDadosUsuarios(string Login)
        {
            var QuerryRecuperaUsuario =
                "SELECT * FROM usuarios WHERE login='" + Login + "'";


            var rows = contexto.ExecutaComandoComRetorno(QuerryRecuperaUsuario);


            return rows;
        }

        public List<Dictionary<string, string>> RecuperaDadosCandidato(string Cpf)
        {
            var QuerryRecuperaUsuario =
                "SELECT a.*,b.descricao as descricaoestadocivil FROM candidatos a, tiposestadoscivis b WHERE b.id=a.estadocivil AND a.cpf='" +
                Cpf + "'";


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
            var QuerryRecuperaEducacional = "SELECT id,email FROM candidatos WHERE cpf='" + Cpf + "'";


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