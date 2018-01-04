using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Microsoft.SqlServer.Server;
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

        public bool PrimeiroLogin(string Usuario)
        {
            var QuerryConfirmaLogin = "SELECT perfilcompleto FROM funcionarios WHERE login='" + Usuario + "';";

            var rows = contexto.ExecutaComandoComRetorno(QuerryConfirmaLogin);

            if (rows[0]["perfilcompleto"].Equals("S"))
                return false;
            else
                return true;
        }

        public bool PermissaoCurriculos(string Usuario)
        {
            var QuerryConfirmaLogin = "select a.valor from permissoesgrupo a, usuarios b, grupos c where a.idgrupo = c.id and b.idgrupo = c.id and b.login='" + Usuario + "' and a.idaplicativo=9";

            var rows = contexto.ExecutaComandoComRetorno(QuerryConfirmaLogin);
            try
            {
                if (rows[0]["valor"].Equals("S"))
                {

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public List<Dictionary<string, string>> RecuperaDocumentosFuncionario(string Login)
        {

            var QuerrySelecionaCurriculo =
                "SELECT * FROM documentospessoaisfuncionarios WHERE idfuncionario=(SELECT id FROM funcionarios where login='"+Login+"')";
            var DadosCurriculos = contexto.ExecutaComandoComRetorno(QuerrySelecionaCurriculo);
            return DadosCurriculos;
        }

        public void InserirFormacao(string Formacao, string IdFuncionario)
        {
            var QuerryFormacao = "INSERT INTO formacoesfuncionarios (idfuncionario,descricao) VALUES('" +
                                 IdFuncionario + "','" + Formacao + "')";
            contexto.ExecutaComandoComRetorno(QuerryFormacao);
        }

        public void AtualizaFormacao(string Formacao, string Id)
        {
            var QuerryFormacao = "UPDATE formacoesfuncionarios  SET descricao='" + Formacao + "' WHERE id='" +Id + "'";
            contexto.ExecutaComandoComRetorno(QuerryFormacao);
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
                "SELECT * FROM funcionarios  WHERE login='" + Login + "'";


            var rows = contexto.ExecutaComandoComRetorno(QuerryRecuperaUsuario);


            return rows;
        }

        public List<Dictionary<string, string>> RecuperaDadosFuncionarios()
        {
            var QuerryRecuperaUsuario =
                "SELECT a.*,b.descricao as setor  FROM funcionarios a, setores b where a.idsetor=b.id and a.ativo='S'";


            var rows = contexto.ExecutaComandoComRetorno(QuerryRecuperaUsuario);


            return rows;
        }

        public List<Dictionary<string, string>> RecuperaDadosFuncionariosTabelaUsuario(string Usuario)
        {
            var QuerryRecuperaAreasInteresse =
                "SELECT * FROM usuarios WHERE login='" + Usuario + "'";
            var rows = contexto.ExecutaComandoComRetorno(QuerryRecuperaAreasInteresse);


            return rows;
        }

        public List<Dictionary<string, string>> RecuperaDadosFuncionariosTabelaFuncionarios(string NomeFuncionario)
        {
            var QuerryRecuperaAreasInteresse =
                "SELECT * FROM funcionarios WHERE nome like'%" + NomeFuncionario + "%'";
            var rows = contexto.ExecutaComandoComRetorno(QuerryRecuperaAreasInteresse);


            return rows;
        }

        public List<Dictionary<string, string>> RecuperaDadosFuncionariosTabelaFuncionariosPerfil(string UsuarioSistema)
        {
            var QuerryRecuperaAreasInteresse =
                "SELECT * FROM funcionarios WHERE login='" + UsuarioSistema + "'";
            var rows = contexto.ExecutaComandoComRetorno(QuerryRecuperaAreasInteresse);


            return rows;
        }

        public List<Dictionary<string, string>> RetornaCertificacao(string IdCertificao)
        {
            var QuerryRecuperaAreasInteresse =
                "SELECT descricao FROM certificacoesfuncionarios WHERE id='" + IdCertificao + "'";
            var rows = contexto.ExecutaComandoComRetorno(QuerryRecuperaAreasInteresse);


            return rows;
        }

        public List<Dictionary<string, string>> RetornaCertificacaoFuncao(string IdFuncao)
        {
            var QuerryRecuperaAreasInteresse =
                "SELECT idcertificacao FROM funcoes WHERE id='" + IdFuncao + "'";
            var rows = contexto.ExecutaComandoComRetorno(QuerryRecuperaAreasInteresse);


            return rows;
        }

        public List<Dictionary<string, string>> RetornaFormacaoFuncionario(string IdFuncionario)
        {
            var QuerryRecuperaAreasInteresse =
                "SELECT * FROM formacoesfuncionarios WHERE idfuncionario='" + IdFuncionario + "'";
            var rows = contexto.ExecutaComandoComRetorno(QuerryRecuperaAreasInteresse);


            return rows;
        }

        public List<SelectListItem> RetornaEstadoCivil()
        {
            var EstadoCivil = new List<SelectListItem>();

            const string QuerryRetornaEstadoCivil = "SELECT id,descricao FROM tiposestadoscivis";

            var rows = contexto.ExecutaComandoComRetorno(QuerryRetornaEstadoCivil);
            foreach (var row in rows)
                EstadoCivil.Add(new SelectListItem
                {
                    Value = row["id"],
                    Text = row["descricao"]
                });

            return EstadoCivil;
        }

        public List<SelectListItem> RetornaSexo()
        {
            var EstadoCivil = new List<SelectListItem>();

            const string QuerryRetornaSexo = "SELECT id,descricao FROM tipossexos";

            var rows = contexto.ExecutaComandoComRetorno(QuerryRetornaSexo);
            foreach (var row in rows)
                EstadoCivil.Add(new SelectListItem
                {
                    Value = row["id"],
                    Text = row["descricao"]
                });

            return EstadoCivil;
        }

        public List<SelectListItem> RetornaEtnia()
        {
            var EstadoCivil = new List<SelectListItem>();

            const string QuerryRetornaEtnia = "SELECT id,descricao FROM tiposetnias";

            var rows = contexto.ExecutaComandoComRetorno(QuerryRetornaEtnia);
            foreach (var row in rows)
                EstadoCivil.Add(new SelectListItem
                {
                    Value = row["id"],
                    Text = row["descricao"]
                });

            return EstadoCivil;
        }

        public List<SelectListItem> RetornaFormacao()
        {
            var EstadoCivil = new List<SelectListItem>();

            const string QuerryRetornaFormacao = "SELECT id,descricao FROM tiposformacoes";

            var rows = contexto.ExecutaComandoComRetorno(QuerryRetornaFormacao);
            foreach (var row in rows)
                EstadoCivil.Add(new SelectListItem
                {
                    Value = row["id"],
                    Text = row["descricao"]
                });

            return EstadoCivil;
        }

        public List<SelectListItem> RetornaSetor()
        {
            var EstadoCivil = new List<SelectListItem>();

            const string QuerryRetornaSetor = "SELECT id,descricao FROM setores";

            var rows = contexto.ExecutaComandoComRetorno(QuerryRetornaSetor);
            foreach (var row in rows)
                EstadoCivil.Add(new SelectListItem
                {
                    Value = row["id"],
                    Text = row["descricao"]
                });

            return EstadoCivil;
        }

        public List<SelectListItem> RetornaFuncao()
        {
            var EstadoCivil = new List<SelectListItem>();

            const string QuerryRetornaSetor = "SELECT id,descricao FROM funcoes";

            var rows = contexto.ExecutaComandoComRetorno(QuerryRetornaSetor);
            foreach (var row in rows)
                EstadoCivil.Add(new SelectListItem
                {
                    Value = row["id"],
                    Text = row["descricao"]
                });

            return EstadoCivil;
        }

        public void AtualizaDadosFuncionarioFormulario(string Nome, string Cpf, string Rg, string Pis,
            string DataNascimentoFuncionario, string Sexo, string DescricaoSexo, string Etnia, string EstadoCivil,
            string Formacao, string FormacaoAcademica, string UsuarioSistema, string Email, string PA, string Rua,
            string Numero, string Bairro, string Cidade, string Setor, string Funcao, string QuatidadeFilhos,
            string DataNascimentoFilhos, string Emergencia, string PrincipaisHobbies, string ComidaFavorita,
            string Viagem,string ConfirmacaoCertificacao)
        {
            string QuerryAtualizaFuncionario = "UPDATE funcionarios SET nome='" + Nome + "', cpf='" + Cpf + "',rg='" +
                                               Rg + "', pis='" + Pis + "',datanascimento='" +
                                               Convert.ToDateTime(DataNascimentoFuncionario).ToString("yyyy/MM/dd") +
                                               "', sexo=" + Sexo + ",descricaosexo='" + DescricaoSexo + "',etnia=" +
                                               Etnia + ",idestadocivil=" + EstadoCivil + ",  idescolaridade=" +
                                               Formacao + ", formacaoacademica='" + FormacaoAcademica + "', login='" +
                                               UsuarioSistema + "', email='" + Email + "', idpa=" + PA + ", rua='" +
                                               Rua + "',numero=" + Numero + ",bairro='" + Bairro + "',cidade='" +
                                               Cidade + "', idsetor=" + Setor + ", funcao='" + Funcao +
                                               "', quantidadefilho='" + QuatidadeFilhos + "',datanascimentofilho='" +
                                               DataNascimentoFilhos + "', contatoemergencia='" + Emergencia +
                                               "', principalhobbie='" + PrincipaisHobbies + "', comidafavorita='" +
                                               ComidaFavorita + "',viagem='" + Viagem + "', perfilcompleto='S',confirmacaocertificacao='" + ConfirmacaoCertificacao+"' WHERE nome='" + Nome + "'";
            contexto.ExecutaComandoComRetorno(QuerryAtualizaFuncionario);
        }

        public void AtualizaDadosFuncionarioDadosPessoais(string Nome, string Cpf, string Rg, string Pis,
            string DataNascimentoFuncionario, string Sexo, string DescricaoSexo, string Etnia, string EstadoCivil,
            string Formacao, string FormacaoAcademica, string UsuarioSistema, string Email, string PA, string Rua,
            string Numero, string Bairro, string Cidade)
        {
            string QuerryAtualizaFuncionario = "UPDATE funcionarios SET nome='" + Nome + "', cpf='" + Cpf + "',rg='" +
                                               Rg + "', pis='" + Pis + "',datanascimento='" +
                                               Convert.ToDateTime(DataNascimentoFuncionario).ToString("yyyy/MM/dd") +
                                               "', sexo=" + Sexo + ",descricaosexo='" + DescricaoSexo + "',etnia=" +
                                               Etnia + ",idestadocivil=" + EstadoCivil + ",  idescolaridade=" +
                                               Formacao + ", formacaoacademica='" + FormacaoAcademica + "', login='" +
                                               UsuarioSistema + "', email='" + Email + "', idpa=" + PA + ", rua='" +
                                               Rua + "',numero=" + Numero + ",bairro='" + Bairro + "',cidade='" +
                                               Cidade + "' WHERE login='" + UsuarioSistema + "'";
            contexto.ExecutaComandoComRetorno(QuerryAtualizaFuncionario);
        }

        public void AtualizaDadosFuncionarioProfissional(string Setor, string Funcao, string UsuarioSistema)
        {
            string QuerryAtualizaFuncionario = "UPDATE funcionarios SET idsetor=" + Setor + ", funcao='" + Funcao + "' WHERE login='" + UsuarioSistema + "'";
            contexto.ExecutaComandoComRetorno(QuerryAtualizaFuncionario);
        }

        public void AtualizaFoto(string Foto, string UsuarioSistema)
        {
            string QuerryAtualizaFuncionario = "UPDATE funcionarios SET foto='" + Foto + "' WHERE login='" + UsuarioSistema + "'";
            contexto.ExecutaComandoComRetorno(QuerryAtualizaFuncionario);
        }

        public void AtualizaDadosFuncionarioPerguntas(string UsuarioSistema, string QuatidadeFilhos, string DataNascimentoFilhos, string Emergencia, string PrincipaisHobbies, string ComidaFavorita, string Viagem)
        {
            string QuerryAtualizaFuncionario = "UPDATE funcionarios SET quantidadefilho='" + QuatidadeFilhos +
                                               "',datanascimentofilho='" +
                                               DataNascimentoFilhos + "', contatoemergencia='" + Emergencia +
                                               "', principalhobbie='" + PrincipaisHobbies + "', comidafavorita='" +
                                               ComidaFavorita + "',viagem='" + Viagem +
                                               "', perfilcompleto='S' WHERE login='" + UsuarioSistema + "'";
            contexto.ExecutaComandoComRetorno(QuerryAtualizaFuncionario);
        }

        public void AtualizarArquivoPessoal(string NomeArquivo,byte[] Foto ,string Login)
        {

            string QueryIdFuncionario = "SELECT id FROM funcionarios WHERE login='" + Login + "'";
            var row = contexto.ExecutaComandoComRetorno(QueryIdFuncionario);

            string QueryArquivoUsuario = "SELECT count(id) as count FROM documentospessoaisfuncionarios WHERE idfuncionario='" + row[0]["id"] + "' and nomearquivo='"+NomeArquivo+"'";
            var row2 = contexto.ExecutaComandoComRetorno(QueryArquivoUsuario);

            if (Convert.ToInt32(row2[0]["count"]) == 0)
            {
                string QuerryAtualizaFuncionario =
                    "INSERT INTO documentospessoaisfuncionarios (idfuncionario,nomearquivo,arquivo,dataupload) VALUES(" +
                    row[0]["id"] + ",'" + NomeArquivo + "',@image ,NOW()) ";
                contexto.ExecutaComandoArquivo(QuerryAtualizaFuncionario, Foto);
            }
            else
            {
                string QuerryAtualizaFuncionario =
                    "UPDATE documentospessoaisfuncionarios SET arquivo=@image and dataupload=NOW() WHERE idfuncionario=" + row[0]["id"] + " AND nomearquivo='" + NomeArquivo + "'";
                contexto.ExecutaComandoArquivo(QuerryAtualizaFuncionario, Foto);
            }
        }

        public List<Dictionary<string, string>> RecuperaTodosArquivos(string Login)
        {
            string QueryIdFuncionario = "SELECT nomearquivo, dataupload FROM documentospessoaisfuncionarios WHERE login='" + Login + "'";
            var row = contexto.ExecutaComandoComRetorno(QueryIdFuncionario);
            return row;
        }







    }
}