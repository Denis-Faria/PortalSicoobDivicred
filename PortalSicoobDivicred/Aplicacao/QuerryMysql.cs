using System;
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


        public List<Dictionary<string, string>> RecuperaCurriculosHistorico(string IdVaga)
        {
            var QuerrySelecionaCurriculo =
                "select a.nome,a.email, a.idarquivogoogle,a.cidade,a.certificacao from historicos c LEFT JOIN candidatos a on c.idcandidato=a.id INNER JOIN candidatos u2 on (c.idcandidato=u2.id) where c.idvaga="+IdVaga+"";
            var DadosCurriculos = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaCurriculo);
            return DadosCurriculos;
        }



        public List<Dictionary<string, string>> RecuperaVagas()
        {
            var QuerrySelecionaVagas =
                "select id,titulo,descricao,areadeinteresse,ativa FROM vagas";
            var DadosVagas = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaVagas);
            return DadosVagas;
        }

        public List<Dictionary<string, string>> RecuperaVagasId(string Id)
        {
            var QuerrySelecionaVagas =
                "select id,titulo,descricao,areadeinteresse,ativa FROM vagas where id="+Id+"";
            var DadosVagas = contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaVagas);
            return DadosVagas;
        }


        public void CadastrarVaga(string Descricao,string AreaInteresse,string Salario,string Requisito,string Titulo,DateTime Data,string Local)
        {
            var QuerrySelecionaCurriculo =
                "INSERT INTO vagas (descricao,areadeinteresse,salario,requisito,titulo,data,localentrevista) VALUES('"+Descricao+"','"+AreaInteresse+"','"+Salario+"','"+Requisito+"','"+Titulo+"','"+Data.Date.ToString("yyyy/MM/dd")+"','"+Local+"')";
            contexto.ExecutaComandoComRetornoPortal(QuerrySelecionaCurriculo);
            
        }

        public void EncerrarVaga(string IdVaga)
        {
            var QuerrySelecionaCurriculo ="UPDATE vagas SET ativa='N' WHERE id="+IdVaga+"";
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
            var QuerryRecuperaAreasInteresse = "SELECT * FROM tiposescolaridades WHERE descricao like'%" + Descricao + "%'";


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