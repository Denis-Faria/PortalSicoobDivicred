using System;
using System.IO;
using System.Web.Mvc;
using PortalSicoobDivicred.Aplicacao;
using PortalSicoobDivicred.Models;

namespace PortalSicoobDivicred.Controllers
{
    public class PainelColaboradorController : Controller
    {
        public ActionResult Perfil(string Mensagem)
        {
            var VerificaDados = new QueryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                TempData["Mensagem"] = Mensagem;
                var Cookie = Request.Cookies.Get("CookieFarm");
                var Login = Criptografa.Descriptografar(Cookie.Value);


                var DadosTabelaFuncionario = VerificaDados.RecuperaDadosFuncionariosTabelaFuncionariosPerfil(Login);
                var DocumentosUpados = VerificaDados.RecuperaDocumentosFuncionario(Login);

                for (var i = 0; i < DocumentosUpados.Count; i++)
                {
                    TempData["Status" + DocumentosUpados[i]["nomearquivo"]] = "is-primary";
                    TempData["Nome" + DocumentosUpados[i]["nomearquivo"]] = "Arquivo Enviado";
                }

                var DadosFuncionario = new Funcionario();
                DadosFuncionario.NomeFuncionario = DadosTabelaFuncionario[0]["nome"];
                DadosFuncionario.CpfFuncionario = DadosTabelaFuncionario[0]["cpf"];
                DadosFuncionario.RgFuncionario = DadosTabelaFuncionario[0]["rg"];
                DadosFuncionario.PisFuncionario = DadosTabelaFuncionario[0]["pis"];
                DadosFuncionario.DataNascimentoFuncionario =
                    Convert.ToDateTime(DadosTabelaFuncionario[0]["datanascimento"]).ToString("dd/MM/yyyy");
                DadosFuncionario.FormacaoAcademica = DadosTabelaFuncionario[0]["formacaoacademica"];
                DadosFuncionario.UsuarioSistema = DadosTabelaFuncionario[0]["login"];
                DadosFuncionario.Email = DadosTabelaFuncionario[0]["email"];
                DadosFuncionario.PA = DadosTabelaFuncionario[0]["idpa"];
                DadosFuncionario.Rua = DadosTabelaFuncionario[0]["rua"];
                DadosFuncionario.Numero = DadosTabelaFuncionario[0]["numero"];
                DadosFuncionario.Bairro = DadosTabelaFuncionario[0]["bairro"];
                DadosFuncionario.Cidade = DadosTabelaFuncionario[0]["cidade"];
                DadosFuncionario.QuatidadeFilho = DadosTabelaFuncionario[0]["quantidadefilho"];
                DadosFuncionario.DataNascimentoFilho = DadosTabelaFuncionario[0]["datanascimentofilho"];
                DadosFuncionario.ContatoEmergencia = DadosTabelaFuncionario[0]["contatoemergencia"];
                DadosFuncionario.PrincipaisHobbies = DadosTabelaFuncionario[0]["principalhobbie"];
                DadosFuncionario.ComidaFavorita = DadosTabelaFuncionario[0]["comidafavorita"];
                DadosFuncionario.Viagem = DadosTabelaFuncionario[0]["viagem"];
                DadosFuncionario.DescricaoSexo = DadosTabelaFuncionario[0]["descricaosexo"];

                var Formacoes = VerificaDados.RetornaFormacaoFuncionario(DadosTabelaFuncionario[0]["id"]);

                TempData["TotalFormacao"] = Formacoes.Count;
                for (var j = 0; j < Formacoes.Count; j++)
                {
                    TempData["IdFormacaoExtra" + j] = "Extra|" + Formacoes[j]["id"];
                    TempData["FormacaoExtra" + j] = Formacoes[j]["descricao"];
                }


                var CertificacoesFuncao = VerificaDados.RetornaCertificacaoFuncao(DadosTabelaFuncionario[0]["funcao"]);
                var IdCertificacoes = CertificacoesFuncao[0]["idcertificacao"].Split(';');

                TempData["TotalCertificacao"] = IdCertificacoes.Length;

                for (var j = 0; j < IdCertificacoes.Length; j++)
                    if (IdCertificacoes[j].Length > 0)
                    {
                        var Certificacoes = VerificaDados.RetornaCertificacao(IdCertificacoes[j]);
                        TempData["Certificacao" + j] = Certificacoes[0]["descricao"];
                    }


                if (DadosTabelaFuncionario[0]["confirmacaocertificacao"].Equals("S"))
                    TempData["ConfirmaCertificacao"] = "Checked";
                else
                    TempData["ConfirmaCertificacao"] = "";
                if (DadosTabelaFuncionario[0]["foto"] == null)
                    TempData["Foto"] = "http://bulma.io/images/placeholders/128x128.png";
                else
                    TempData["Foto"] = "/Uploads/" + DadosTabelaFuncionario[0]["foto"];
                var FuncaoFuncionario = VerificaDados.RetornaFuncaoFuncionario(DadosTabelaFuncionario[0]["funcao"]);
                TempData["IdEstadoCivil"] = DadosTabelaFuncionario[0]["idestadocivil"];
                TempData["IdSexo"] = DadosTabelaFuncionario[0]["sexo"];
                TempData["IdEtnia"] = DadosTabelaFuncionario[0]["etnia"];
                TempData["IdFormacao"] = DadosTabelaFuncionario[0]["idescolaridade"];
                TempData["IdSetor"] = DadosTabelaFuncionario[0]["idsetor"];
                TempData["NomeFuncionario"] = DadosTabelaFuncionario[0]["nome"];
                TempData["Funcao"] = FuncaoFuncionario;
                TempData["IdFuncao"] = DadosTabelaFuncionario[0]["funcao"];
                TempData["DataAdmissao"] =
                    Convert.ToDateTime(DadosTabelaFuncionario[0]["admissao"]).ToString("dd/MM/yyyy");

                var EstadoCivil = VerificaDados.RetornaEstadoCivil();
                var TipoConta = VerificaDados.RetornaTipoConta();
                var Sexo = VerificaDados.RetornaSexo();
                var Etnia = VerificaDados.RetornaEtnia();
                var Formacao = VerificaDados.RetornaFormacao();
                var Setor = VerificaDados.RetornaSetor();
                var Funcao = VerificaDados.RetornaFuncao();

                DadosFuncionario.EstadoCivil = EstadoCivil;
                DadosFuncionario.Sexo = Sexo;
                DadosFuncionario.Etnia = Etnia;
                DadosFuncionario.Formacao = Formacao;
                DadosFuncionario.Setor = Setor;
                DadosFuncionario.Funcao = Funcao;
                DadosFuncionario.Conta = TipoConta;


                TempData["Salario"] = DadosTabelaFuncionario[0]["salariobase"];
                TempData["QuebraCaixa"] = DadosTabelaFuncionario[0]["quebradecaixa"];
                TempData["Anuenio"] = DadosTabelaFuncionario[0]["anuenio"];
                TempData["Ticket"] = DadosTabelaFuncionario[0]["ticket"];

                if (DadosTabelaFuncionario[0]["estagiario"].Equals("S"))
                {
                    TempData["Estagiario"] = "SIM";
                    TempData["DataEstagio"] = DadosTabelaFuncionario[0]["contratoestagio"];
                }
                else
                {
                    TempData["Estagiario"] = "NÃO";
                    TempData["DataEstagio"] = "-";
                }


                if (VerificaDados.PermissaoCurriculos(Login))
                    TempData["PermissaoCurriculo"] =
                        " ";
                else
                    TempData["PermissaoCurriculo"] = "display: none";
                if (DadosTabelaFuncionario[0]["gestor"].Equals("S"))
                {
                    TempData["PermissaoGestor"] = "";
                    TempData["AreaGestor"] = "";
                }
                else
                {
                    TempData["PermissaoGestor"] = "hidden";
                    TempData["AreaGestor"] = "hidden";
                }

                TempData["NomeLateral"] = DadosTabelaFuncionario[0]["login"];
                TempData["EmailLateral"] = DadosTabelaFuncionario[0]["email"];
                if (DadosTabelaFuncionario[0]["foto"] == null)
                    TempData["ImagemPerfil"] = "https://docs.google.com/uc?id=0B2CLuTO3N2_obWdkajEzTmpGeU0";
                else
                    TempData["ImagemPerfil"] = DadosTabelaFuncionario[0]["foto"];
                return View("Perfil", DadosFuncionario);
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult AtualizarDadosProfissionais(Funcionario DadosFuncionario)
        {
            var VerificaDados = new QueryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Cookie = Request.Cookies.Get("CookieFarm");
                var Login = Criptografa.Descriptografar(Cookie.Value);

                VerificaDados.AtualizaDadosFuncionarioProfissional(DadosFuncionario.IdSetor.ToString(),
                    DadosFuncionario.IdFuncao.ToString(), Login);

                return RedirectToAction("Perfil", "PainelColaborador",
                    new {Mensagem = "Dados Profissionais atualizados com sucesso !"});
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult AtualizaDadosPessoais(Funcionario DadosFuncionario, FormCollection Formulario)
        {
            var VerificaDados = new QueryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Cookie = Request.Cookies.Get("CookieFarm");
                var Login = Criptografa.Descriptografar(Cookie.Value);

                var DescricaoSexo = "";
                if (DadosFuncionario.DescricaoSexo == null)
                    DescricaoSexo = "NÃO INFORMOU";
                else
                    DescricaoSexo = DadosFuncionario.DescricaoSexo;


                var DadosTabelaFuncionario = VerificaDados.RecuperaDadosFuncionariosTabelaFuncionariosPerfil(Login);

                for (var i = 0; i < Formulario.Count; i++)
                {
                    if (Formulario.AllKeys[i].Contains("formacao"))
                        VerificaDados.InserirFormacao(Formulario[i], DadosTabelaFuncionario[0]["id"]);
                    if (Formulario.AllKeys[i].Contains("Extra"))
                    {
                        var IdFormacao = Formulario.AllKeys[i].Split('|');
                        VerificaDados.AtualizaFormacao(Formulario[i], IdFormacao[1]);
                    }
                }


                VerificaDados.AtualizaDadosFuncionarioDadosPessoais(DadosFuncionario.NomeFuncionario,
                    DadosFuncionario.CpfFuncionario, DadosFuncionario.RgFuncionario,
                    DadosFuncionario.PisFuncionario, DadosFuncionario.DataNascimentoFuncionario,
                    DadosFuncionario.IdSexo.ToString(), DescricaoSexo, DadosFuncionario.IdEtnia.ToString(),
                    DadosFuncionario.IdEstadoCivil.ToString(), DadosFuncionario.IdFormacao.ToString(),
                    DadosFuncionario.FormacaoAcademica,
                    Login, DadosFuncionario.Email, DadosFuncionario.PA,
                    DadosFuncionario.Rua, DadosFuncionario.Numero, DadosFuncionario.Bairro, DadosFuncionario.Cidade,
                    "S");

                return RedirectToAction("Perfil", "PainelColaborador",
                    new {Mensagem = "Dados Pessoais atualizados com sucesso !"});
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult Upload()
        {
            var InserirFoto = new QueryMysql();
            var Logado = InserirFoto.UsuarioLogado();
            if (Logado)
            {
                var Arquivo = Request.Files[0];
                var NomeArquivo = Path.GetFileName(Arquivo.FileName);
                var Caminho = Path.Combine(Server.MapPath("~/Uploads/"), NomeArquivo);

                if (System.IO.File.Exists(Caminho))
                {
                    var counter = 1;
                    var tempfileName = "";
                    while (System.IO.File.Exists(Caminho))
                    {
                        tempfileName = counter + NomeArquivo;
                        Caminho = Path.Combine(Server.MapPath("~/Uploads/"), tempfileName);
                        counter++;
                    }

                    Caminho = Path.Combine(Server.MapPath("~/Uploads/"), tempfileName);
                    Arquivo.SaveAs(Caminho);

                    var cookie = Request.Cookies.Get("CookieFarm");
                    var Login = Criptografa.Descriptografar(cookie.Value);
                    InserirFoto.AtualizaFoto(tempfileName, Login);
                    return Content("Imagem alterada com sucesso!");
                }
                else
                {
                    Arquivo.SaveAs(Caminho);
                    var cookie = Request.Cookies.Get("CookieFarm");
                    var Login = Criptografa.Descriptografar(cookie.Value);
                    InserirFoto.AtualizaFoto(NomeArquivo, Login);
                    return Content("Imagem alterada com sucesso!");
                }
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult UploadArquivo(string Nome)
        {
            var InserirFoto = new QueryMysql();
            var Logado = InserirFoto.UsuarioLogado();
            if (Logado)
            {
                var Arquivo = Request.Files[0];
                var NomeArquivo = Path.GetFileName(Arquivo.FileName);
                var Caminho = Path.Combine(Server.MapPath("~/Uploads/"), NomeArquivo);
                byte[] fileData = null;
                using (var binaryReader = new BinaryReader(Arquivo.InputStream))
                {
                    fileData = binaryReader.ReadBytes(Arquivo.ContentLength);
                }

                var cookie = Request.Cookies.Get("CookieFarm");
                var Login = Criptografa.Descriptografar(cookie.Value);
                InserirFoto.AtualizarArquivoPessoal(NomeArquivo, fileData, Login);
                return Content("Imagem alterada com sucesso!");
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult AtualizarFormularioPessoal(Funcionario DadosFuncionario)
        {
            var VerificaDados = new QueryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Cookie = Request.Cookies.Get("CookieFarm");
                var Login = Criptografa.Descriptografar(Cookie.Value);
                var DataNascimentoFilho = "";
                if (DadosFuncionario.DataNascimentoFilho == null)
                    DataNascimentoFilho = "NÂO TEM";
                else
                    DataNascimentoFilho = DadosFuncionario.DataNascimentoFilho;
                VerificaDados.AtualizaDadosFuncionarioPerguntas(Login, DadosFuncionario.QuatidadeFilho,
                    DataNascimentoFilho, DadosFuncionario.ContatoEmergencia,
                    DadosFuncionario.PrincipaisHobbies, DadosFuncionario.ComidaFavorita, DadosFuncionario.Viagem);

                return RedirectToAction("Perfil", "PainelColaborador",
                    new {Mensagem = "Formulário Pessoal atualizado com sucesso !"});
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult ColaboradorRh(string Mensagem)
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var CarregaDados = new QueryMysql();
                TempData["Mensagem"] = Mensagem;

                var Cookie = Request.Cookies.Get("CookieFarm");

                var Login = Criptografa.Descriptografar(Cookie.Value);
                if (CarregaDados.PrimeiroLogin(Login))
                    return RedirectToAction("FormularioCadastro", "Principal");
                var DadosUsuarioBanco = CarregaDados.RecuperaDadosUsuarios(Login);


                if (CarregaDados.PermissaoCurriculos(DadosUsuarioBanco[0]["login"]))
                    TempData["PermissaoCurriculo"] =
                        " <a  href='javascript: Curriculo(); void(0); ' class='item' style='color: #38d5c5;' data-balloon='Curriculos' data-balloon-pos='right'><span class='icon'><i class='fa fa-book'></i></span><span class='name'></span></a>";
                else
                    TempData["PermissaoCurriculo"] = "";
                if (DadosUsuarioBanco[0]["gestor"].Equals("S"))
                {
                    TempData["PermissaoGestor"] = "";
                    TempData["AreaGestor"] = "";
                }
                else
                {
                    TempData["PermissaoGestor"] = "hidden";
                    TempData["AreaGestor"] = "hidden";
                }

                TempData["NomeLateral"] = DadosUsuarioBanco[0]["login"];
                TempData["EmailLateral"] = DadosUsuarioBanco[0]["email"];
                if (DadosUsuarioBanco[0]["foto"] == null)
                    TempData["ImagemPerfil"] = "https://docs.google.com/uc?id=0B2CLuTO3N2_obWdkajEzTmpGeU0";
                else
                    TempData["ImagemPerfil"] = DadosUsuarioBanco[0]["foto"];


                var DadosColaborador = CarregaDados.RecuperaDadosFuncionariosSetor();

                var QueryRh = new QueryMysqlRh();
                var VagasInternas = QueryRh.RetornaVagaInternaTotal();

                TempData["TotalColaborador"] = DadosColaborador.Count;
                TempData["Total"] = VagasInternas.Count;

                var Validacoes = new ValidacoesPonto();
                var TabelasPonto = Validacoes.RetornaPendenciasMysql();
                TempData["ExtraJustifica1"] = "hidden";
                TempData["ExtraJustifica2"] = "hidden";

                #region Preenchimento pendencias MYSQL

                TempData["TotalSemConfirmar"] = TabelasPonto.Count;
                for (var i = 0; i < TabelasPonto.Count; i++)
                {
                    TempData["IdPendencia" + i] = TabelasPonto[i]["IdPendencia"];


                    TempData["NomePendencia" + i] = TabelasPonto[i]["Nome"];


                    TempData["DiaPendencia" + i] =
                        Convert.ToDateTime(TabelasPonto[i]["Data"]).ToString("dd/MM/yyyy");

                    TempData["TotalHorarioPendencia" + i] = Convert.ToInt32(TabelasPonto[i]["TotalHorario"]);

                    for (var j = 0; j < Convert.ToInt32(TabelasPonto[i]["TotalHorario"]); j++)
                        TempData["Hora" + j + "Pendencia" + i] = TabelasPonto[i]["Horario" + j];

                    if (4 - Convert.ToInt32(TabelasPonto[i]["TotalHorario"]) > 0)
                        TempData["TotalHorarioPendencia" + i] = 4;

                    if (Convert.ToBoolean(TabelasPonto[i]["Justificado"]))
                    {
                        TempData["StatusJustificativa" + i] = "green";
                        TempData["Justificativa" + i] = TabelasPonto[i]["Justificativa" + i];
                        TempData["Esconde" + i] = "";
                    }
                    else
                    {
                        TempData["Justificativa" + i] = "NÃO JUSTIFICADO";
                        TempData["StatusJustificativa" + i] = "red";
                        TempData["Esconde" + i] = "hidden";
                    }

                    TempData["StatusJustificativaGetor" + i] = TabelasPonto[i]["ConfirmaGestor"];
                    if (Convert.ToInt32(TabelasPonto[i]["TotalHorario"]) == 5)
                    {
                        TempData["ExtraJustifica1"] = "";
                    }
                    else if (Convert.ToInt32(TabelasPonto[i]["TotalHorario"]) == 6)
                    {
                        TempData["ExtraJustifica1"] = "";
                        TempData["ExtraJustifica2"] = "";
                    }
                }

                #endregion


                for (var j = 0; j < VagasInternas.Count; j++)

                {
                    TempData["Titulo " + j] = VagasInternas[j]["titulo"];
                    TempData["DescricaoVaga " + j] = VagasInternas[j]["descricao"];
                    TempData["IdVaga " + j] = VagasInternas[j]["id"];
                    if (VagasInternas[j]["encerrada"].Equals("N"))
                        TempData["StatusVaga " + j] = "green";
                    else
                        TempData["StatusVaga " + j] = "red";
                }

                for (var i = 0; i < DadosColaborador.Count; i++)
                {
                    TempData["Nome" + i] = DadosColaborador[i]["nome"];
                    TempData["Setor" + i] = DadosColaborador[i]["setor"];
                    TempData["PA" + i] = DadosColaborador[i]["idpa"];
                    TempData["Login" + i] = DadosColaborador[i]["login"];
                    try
                    {
                        if (DadosColaborador[i]["foto"].Equals("0") || DadosColaborador[i]["foto"] == null)
                            TempData["Imagem" + i] = "https://docs.google.com/uc?id=0B2CLuTO3N2_obWdkajEzTmpGeU0";
                        else
                            TempData["Imagem" + i] = "/Uploads/" + DadosColaborador[i]["foto"] + "";
                    }
                    catch
                    {
                        TempData["Imagem" + i] = "https://docs.google.com/uc?id=0B2CLuTO3N2_obWdkajEzTmpGeU0";
                    }
                }


                return View("ColaboradorRh");
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult PerfilFuncionario(string Login)
        {
            var VerificaDados = new QueryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var DadosTabelaFuncionario = VerificaDados.RecuperaDadosFuncionariosTabelaFuncionariosPerfil(Login);


                var DocumentosUpados = VerificaDados.RetornaDocumentosFuncionario(Login);

                for (var i = 0; i < DocumentosUpados.Rows.Count; i++)
                {
                    TempData["Status" + DocumentosUpados.Rows[i]["nomearquivo"]] = "is-success";
                    TempData["Nome" + DocumentosUpados.Rows[i]["nomearquivo"]] = "Arquivo Enviado";

                    var bytes = (byte[]) DocumentosUpados.Rows[i]["arquivo"];
                    var img64 = Convert.ToBase64String(bytes);
                    var img64Url = string.Format("data:image/;base64,{0}", img64);
                    TempData["Imagem" + DocumentosUpados.Rows[i]["nomearquivo"]] = img64Url;
                }

                var DadosFuncionario = new Funcionario();


                DadosFuncionario.NomeFuncionario = DadosTabelaFuncionario[0]["nome"];
                DadosFuncionario.CpfFuncionario = DadosTabelaFuncionario[0]["cpf"];
                DadosFuncionario.RgFuncionario = DadosTabelaFuncionario[0]["rg"];
                DadosFuncionario.PisFuncionario = DadosTabelaFuncionario[0]["pis"];
                DadosFuncionario.DataNascimentoFuncionario =
                    Convert.ToDateTime(DadosTabelaFuncionario[0]["datanascimento"]).ToString("dd/MM/yyyy");
                DadosFuncionario.FormacaoAcademica = DadosTabelaFuncionario[0]["formacaoacademica"];
                DadosFuncionario.UsuarioSistema = DadosTabelaFuncionario[0]["login"];
                DadosFuncionario.Email = DadosTabelaFuncionario[0]["email"];
                DadosFuncionario.PA = DadosTabelaFuncionario[0]["idpa"];
                DadosFuncionario.Rua = DadosTabelaFuncionario[0]["rua"];
                DadosFuncionario.Numero = DadosTabelaFuncionario[0]["numero"];
                DadosFuncionario.Bairro = DadosTabelaFuncionario[0]["bairro"];
                DadosFuncionario.Cidade = DadosTabelaFuncionario[0]["cidade"];
                DadosFuncionario.QuatidadeFilho = DadosTabelaFuncionario[0]["quantidadefilho"];
                DadosFuncionario.DataNascimentoFilho = DadosTabelaFuncionario[0]["datanascimentofilho"];
                DadosFuncionario.ContatoEmergencia = DadosTabelaFuncionario[0]["contatoemergencia"];
                DadosFuncionario.PrincipaisHobbies = DadosTabelaFuncionario[0]["principalhobbie"];
                DadosFuncionario.ComidaFavorita = DadosTabelaFuncionario[0]["comidafavorita"];
                DadosFuncionario.Viagem = DadosTabelaFuncionario[0]["viagem"];
                DadosFuncionario.DescricaoSexo = DadosTabelaFuncionario[0]["descricaosexo"];

                if (DadosTabelaFuncionario[0]["foto"] == null)
                    TempData["Foto"] = "http://bulma.io/images/placeholders/128x128.png";
                else
                    TempData["Foto"] = "/Uploads/" + DadosTabelaFuncionario[0]["foto"];

                TempData["DataAdmissao"] =
                    Convert.ToDateTime(DadosTabelaFuncionario[0]["admissao"]).ToString("dd/MM/yyyy");

                TempData["Genero"] = VerificaDados.RetornaGeneroFuncionario(DadosTabelaFuncionario[0]["sexo"]);
                TempData["Setor"] = VerificaDados.RetornaSetorFuncionario(DadosTabelaFuncionario[0]["idsetor"]);
                TempData["Funcao"] = VerificaDados.RetornaFuncaoFuncionario(DadosTabelaFuncionario[0]["funcao"]);
                TempData["Educacional"] =
                    VerificaDados.RetornaEscolaridadeFuncionario(DadosTabelaFuncionario[0]["idescolaridade"]);
                TempData["EstadoCivil"] =
                    VerificaDados.RetornaEstadoCivilFuncionario(DadosTabelaFuncionario[0]["idestadocivil"]);
                TempData["Etinia"] = VerificaDados.RetornaEtiniaFuncionario(DadosTabelaFuncionario[0]["etnia"]);

                TempData["NomeFuncionario"] = DadosTabelaFuncionario[0]["nome"];

                var formacao = VerificaDados.RetornaFormacaoFuncionario(DadosTabelaFuncionario[0]["id"]);
                TempData["TotalFormacao"] = formacao.Count;
                for (var i = 0; i < formacao.Count; i++)
                    TempData["Formacao " + i] = formacao[0]["descricao"];

                TempData["Salario"] = DadosTabelaFuncionario[0]["salariobase"];
                TempData["QuebraCaixa"] = DadosTabelaFuncionario[0]["quebradecaixa"];
                TempData["Anuenio"] = DadosTabelaFuncionario[0]["anuenio"];
                TempData["Ticket"] = DadosTabelaFuncionario[0]["ticket"];

                if (DadosTabelaFuncionario[0]["estagiario"].Equals("S"))
                {
                    TempData["Estagiario"] = "SIM";
                    TempData["DataEstagio"] = DadosTabelaFuncionario[0]["contratoestagio"];
                }
                else
                {
                    TempData["Estagiario"] = "NÃO";
                    TempData["DataEstagio"] = "-";
                }

                return PartialView("ModalPerfil", DadosFuncionario);
            }

            return RedirectToAction("Login", "Login");
        }


        [HttpPost]
        public ActionResult CadastrarVaga(VagasInternas DadosVaga)
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
                if (ModelState.IsValid)
                {
                    VerificaDados.CadastraVagaInterna(DadosVaga.Titulo, DadosVaga.Descricao, DadosVaga.Descricao);

                    return RedirectToAction("ColaboradorRH", "PainelColaborador",
                        new {Mensagem = "Vaga interna criada com sucesso !"});
                }
                else
                {
                    return RedirectToAction("ColaboradorRh", "PainelColaborador",
                        new {Mensagem = "Vaga interna não cadastrada !"});
                }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult Vaga()
        {
            return PartialView("ModalVagaInterna");
        }


        public ActionResult AlterarVaga(string IdVaga)
        {
            var VerificaDados = new QueryMysqlCurriculo();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var QueryRh = new QueryMysqlRh();
                var DadosVaga = QueryRh.RetornaVaga(IdVaga);
                var Vaga = new VagasInternas();
                Vaga.Titulo = DadosVaga[0]["titulo"];
                Vaga.Descricao = DadosVaga[0]["descricao"];
                Vaga.Requisitos = DadosVaga[0]["requisito"];
                TempData["IdVaga"] = IdVaga;

                if (DadosVaga[0]["encerrada"].Equals("N"))
                {
                    TempData["Ativa"] = "checked";
                    TempData["Status"] = "Ativa";
                }
                else
                {
                    TempData["Ativa"] = "";
                    TempData["Status"] = "Desativada";
                }

                return PartialView("ModalEditarVagaInterna", Vaga);
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult AtualizarVaga(VagasInternas DadosVaga, FormCollection Formulario)
        {
            var VerificaDados = new QueryMysqlCurriculo();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
                if (ModelState.IsValid)
                {
                    var QueryRh = new QueryMysqlRh();

                    QueryRh.AtualizaVagaInterna(DadosVaga.Titulo, DadosVaga.Descricao, DadosVaga.Requisitos,
                        Formulario["IdVaga"]);
                    return RedirectToAction("ColaboradorRh", "PainelColaborador",
                        new {Mensagem = "Vaga alterada com sucesso!"});
                }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult GerenciarVaga(string IdVaga, string Mensagem)
        {
            var VerificaDados = new QueryMysqlCurriculo();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                TempData["Mensagem"] = Mensagem;

                var RecuperaDados = new QueryMysqlRh();
                var DadosCurriculos = RecuperaDados.RecuperaFuncionariosVaga(IdVaga);
                var DadosVaga = RecuperaDados.RetornaVaga(IdVaga);
                TempData["TotalCurriculo"] = DadosCurriculos.Count;
                TempData["QuantidadeCurriculo"] = "N° de candidatos com interesse nesta vaga: " + DadosCurriculos.Count;
                TempData["TituloVaga"] = IdVaga + "-" + DadosVaga[0]["titulo"];
                TempData["DescricaoVaga"] = DadosVaga[0]["descricao"];
                TempData["IdVaga"] = IdVaga;

                for (var i = 0; i < DadosCurriculos.Count; i++)
                {
                    TempData["Id " + i] = DadosCurriculos[i]["id"];
                    TempData["IdFuncionario" + i] = "IdFuncionario | " + DadosCurriculos[i]["id"];
                    TempData["Nome" + i] = DadosCurriculos[i]["nome"];
                    TempData["Login" + i] = DadosCurriculos[i]["login"];
                    TempData["Setor" + i] = DadosCurriculos[i]["setorfuncionario"];
                    TempData["PA" + i] = DadosCurriculos[i]["idpa"];
                    TempData["ResultadoObservacao" + i] = DadosCurriculos[0]["observacao"];
                    if (Convert.ToBoolean(DadosCurriculos[i]["aprovado"]))
                        TempData["Resultado" + i] = "checked";
                    else
                        TempData["Resultado" + i] = "";

                    if (DadosCurriculos[i]["foto"].Equals("0"))
                        TempData["Imagem" + i] = "https://docs.google.com/uc?id=0B2CLuTO3N2_obWdkajEzTmpGeU0";
                    else
                        TempData["Imagem" + i] = "/Uploads/" +
                                                 DadosCurriculos[i]["foto"] + "";
                }

                if (DadosVaga[0]["encerrada"].Equals("N"))
                {
                    TempData["Ativa"] = "";
                    TempData["Dica"] = "Clique para encerrar esta vaga.";
                }
                else
                {
                    TempData["Ativa"] = "disabled";
                    TempData["Dica"] = "Esta vaga já esta encerrada.";
                }

                return View("GerenciarVaga");
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult EncerrarVaga(FormCollection Formulario)
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                for (var i = 0; i < Formulario.Count; i++)
                    if (Formulario.AllKeys[i].Contains("IdFuncionario"))
                    {
                        var IdFuncionario = Formulario[i];
                        var Aprovado = false;
                        try
                        {
                            if (Formulario["Aprovado " + IdFuncionario].Equals("on"))
                                Aprovado = true;
                            else
                                Aprovado = false;
                        }
                        catch
                        {
                            Aprovado = false;
                        }

                        var Observacao = Formulario["observacao " + IdFuncionario];
                        VerificaDados.AtualizaStatus(Formulario["vaga"], IdFuncionario, Aprovado, Observacao);
                    }

                VerificaDados.EncerraVaga(Formulario["vaga"]);
                return RedirectToAction("GerenciarVaga", "PainelColaborador",
                    new {IdVaga = Formulario["vaga"], Mensagem = "Vaga interna encerrada com sucesso !"});
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public JsonResult ConfirmarPendencia(Item[] TabelaPendencias)
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                for (var i = 0; i < TabelaPendencias.Length; i++)
                {
                    var RecuperaId = new QueryMysql();
                    var DadosFuncionario =
                        RecuperaId.RecuperaDadosFuncionariosTabelaFuncionarios(
                            TabelaPendencias[i].Nome.Replace("'", ""));
                    try
                    {
                        var IdJustificativa = VerificaDados.InserirHistoricoJustificativa(DadosFuncionario[0]["id"],
                            Convert.ToDateTime(TabelaPendencias[i].Dia));
                        if (TabelaPendencias[i].Horario1.Length == 0 || TabelaPendencias[i].Horario1.Contains("--"))
                        {
                        }
                        else
                        {
                            VerificaDados.InserirHistoricoHorario(IdJustificativa,
                                TimeSpan.Parse(TabelaPendencias[i].Horario1), TabelaPendencias[i].Id, "0");
                        }

                        if (TabelaPendencias[i].Horario2.Length == 0 || TabelaPendencias[i].Horario2.Contains("--"))
                        {
                        }
                        else
                        {
                            VerificaDados.InserirHistoricoHorario(IdJustificativa,
                                TimeSpan.Parse(TabelaPendencias[i].Horario2), TabelaPendencias[i].Id, "0");
                        }

                        if (TabelaPendencias[i].Horario3.Length == 0 || TabelaPendencias[i].Horario3.Contains("--"))
                        {
                        }
                        else
                        {
                            VerificaDados.InserirHistoricoHorario(IdJustificativa,
                                TimeSpan.Parse(TabelaPendencias[i].Horario3), TabelaPendencias[i].Id, "0");
                        }

                        if (TabelaPendencias[i].Horario4.Length == 0 || TabelaPendencias[i].Horario4.Contains("--"))
                        {
                        }
                        else
                        {
                            VerificaDados.InserirHistoricoHorario(IdJustificativa,
                                TimeSpan.Parse(TabelaPendencias[i].Horario4), TabelaPendencias[i].Id, "0");
                        }

                        if (TabelaPendencias[i].Horario5.Length == 0 || TabelaPendencias[i].Horario5.Contains("--"))
                        {
                        }
                        else
                        {
                            VerificaDados.InserirHistoricoHorario(IdJustificativa,
                                TimeSpan.Parse(TabelaPendencias[i].Horario5), TabelaPendencias[i].Id, "0");
                        }

                        if (TabelaPendencias[i].Horario6.Length == 0 || TabelaPendencias[i].Horario4.Contains("--") ||
                            TabelaPendencias[i].Horario6.Contains("<"))
                        {
                        }
                        else
                        {
                            VerificaDados.InserirHistoricoHorario(IdJustificativa,
                                TimeSpan.Parse(TabelaPendencias[i].Horario6), TabelaPendencias[i].Id, "0");
                        }
                    }
                    catch
                    {
                    }
                }

                return Json("Ok");
            }

            return Json("Ok");
        }

        public ActionResult NegarJustificativa(string IdHistorico)
        {
            var QueryRh = new QueryMysqlRh();
            QueryRh.NegaJustificativa(IdHistorico);
            QueryRh.NegaJustificativaGestor(IdHistorico);
            return RedirectToAction("ColaboradorRh", "PainelColaborador",
                new {Mensagem = "Pendencia negada com sucesso !"});
        }

        [HttpPost]
        public ActionResult CadastraAlerta(FormCollection Dados)
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var FuncionarioPendentes = VerificaDados.RetornaPendenciaAlerta();
                for (var i = 0; i < FuncionarioPendentes.Count; i++)
                    VerificaDados.CadastraAlertaJustificativa(FuncionarioPendentes[i]["idfuncionario"],
                        Dados["TextAlerta"]);

                return RedirectToAction("ColaboradorRh", "PainelColaborador",
                    new {Mensagem = "Alerta cadastrado com sucesso !"});
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult ApontarPendencia(Item[] TabelaPendencias)
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                for (var i = 0; i < TabelaPendencias.Length; i++)
                {
                    var RecuperaId = new QueryMysql();
                    var DadosFuncionario =
                        RecuperaId.RecuperaDadosFuncionariosTabelaFuncionarios(
                            TabelaPendencias[i].Nome.Replace("'", ""));
                    try
                    {
                        var IdJustificativa = VerificaDados.InserirHistoricoJustificativa(DadosFuncionario[0]["id"],
                            Convert.ToDateTime(TabelaPendencias[i].Dia));
                        if (TabelaPendencias[i].Horario1.Length == 0 || TabelaPendencias[i].Horario1.Contains("--"))
                        {
                        }
                        else
                        {
                            VerificaDados.InserirHistoricoHorario(IdJustificativa,
                                TimeSpan.Parse(TabelaPendencias[i].Horario1), TabelaPendencias[i].Id, "0");
                        }

                        if (TabelaPendencias[i].Horario2.Length == 0 || TabelaPendencias[i].Horario2.Contains("--"))
                        {
                        }
                        else
                        {
                            VerificaDados.InserirHistoricoHorario(IdJustificativa,
                                TimeSpan.Parse(TabelaPendencias[i].Horario2), TabelaPendencias[i].Id, "0");
                        }

                        if (TabelaPendencias[i].Horario3.Length == 0 || TabelaPendencias[i].Horario3.Contains("--"))
                        {
                        }
                        else
                        {
                            VerificaDados.InserirHistoricoHorario(IdJustificativa,
                                TimeSpan.Parse(TabelaPendencias[i].Horario3), TabelaPendencias[i].Id, "0");
                        }

                        if (TabelaPendencias[i].Horario4.Length == 0 || TabelaPendencias[i].Horario4.Contains("--"))
                        {
                        }
                        else
                        {
                            VerificaDados.InserirHistoricoHorario(IdJustificativa,
                                TimeSpan.Parse(TabelaPendencias[i].Horario4), TabelaPendencias[i].Id, "0");
                        }
                    }
                    catch
                    {
                    }
                }

                return Json("Ok");
            }

            return Json("Ok");
        }

        public ActionResult HistoricoPendencia()
        {
            return View();
        }

        [HttpPost]
        public ActionResult HistoricoPendencia(FormCollection Datas)
        {
            var Validacoes = new ValidacoesPonto();


            var DadosPendencias = Validacoes.RetornaHistoricoPendencias(Datas["DataInicial"], Datas["DataFinal"]);

            TempData["ExtraJustifica1"] = "hidden";
            TempData["ExtraJustifica2"] = "hidden";
            TempData["TotalSemConfirmar"] = DadosPendencias.Count;
            for (var i = 0; i < DadosPendencias.Count; i++)
            {
                TempData["IdPendencia" + i] = DadosPendencias[i]["IdPendencia"];


                TempData["NomePendencia" + i] = DadosPendencias[i]["Nome"];


                TempData["DiaPendencia" + i] =
                    Convert.ToDateTime(DadosPendencias[i]["Data"]).ToString("dd/MM/yyyy");

                TempData["TotalHorarioPendencia" + i] = Convert.ToInt32(DadosPendencias[i]["TotalHorario"]);

                for (var j = 0; j < Convert.ToInt32(DadosPendencias[i]["TotalHorario"]); j++)
                    TempData["Hora" + j + "Pendencia" + i] = DadosPendencias[i]["Horario" + j];

                if (4 - Convert.ToInt32(DadosPendencias[i]["TotalHorario"]) > 0)
                    TempData["TotalHorarioPendencia" + i] = 4;

                if (Convert.ToBoolean(DadosPendencias[i]["Justificado"]))
                    TempData["Justificativa" + i] = DadosPendencias[i]["Justificativa" + i];
                else
                    TempData["Justificativa" + i] = "NÃO JUSTIFICADO";

                if (Convert.ToInt32(DadosPendencias[i]["TotalHorario"]) == 5)
                {
                    TempData["ExtraJustifica1"] = "";
                }
                else if (Convert.ToInt32(DadosPendencias[i]["TotalHorario"]) == 6)
                {
                    TempData["ExtraJustifica1"] = "";
                    TempData["ExtraJustifica2"] = "";
                }
            }

            return View();
        }

        public ActionResult ReincidentePendencia()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReincidentePendencia(FormCollection Datas)
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Reincidentes = VerificaDados.RetornaReincidentes(Datas["DataInicial"], Datas["DataFinal"]);
                var TotalReincidente = 0;
                for (var i = 0; i < Reincidentes.Count; i++)
                    if (Reincidentes[i]["confirma"].Equals("S"))
                    {
                        var Dias = VerificaDados.RetornaDataReincidentes(Reincidentes[i]["id"], Datas["DataInicial"],
                            Datas["DataFinal"]);
                        TempData["Nome" + TotalReincidente] = Reincidentes[i]["nome"];
                        TempData["IdFuncionario" + TotalReincidente] = Reincidentes[i]["id"];
                        TempData["Dia" + TotalReincidente] = Dias;
                        TotalReincidente++;
                    }

                TempData["TotalReincidente"] = TotalReincidente;
                return View();
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult Paremetros()
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado) return PartialView("Parametros");

            return RedirectToAction("Login", "Login");
        }

        public ActionResult Funcao()
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Certificacoes = VerificaDados.RetornaCertificacoes();
                TempData["TotalCertificacao"] = Certificacoes.Count;
                for (var i = 0; i < Certificacoes.Count; i++)
                {
                    TempData["DescricaoCertificacao" + i] = Certificacoes[i]["descricao"];
                    TempData["IdCertificacao" + i] = Certificacoes[i]["id"];
                }

                return PartialView("Funcao");
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult Funcao(Funcao DadosCadastro, FormCollection Dados)
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Certificacoes = "";
                for (var i = 0; i < Dados.Count; i++)
                    if (Dados.AllKeys[i].Contains("Certificacao"))
                        Certificacoes = Certificacoes + Dados[i] + ";";

                VerificaDados.CadastrarFuncao(DadosCadastro.NomeFuncao, Certificacoes);
                return RedirectToAction("ColaboradorRh", "PainelColaborador",
                    new {Mensagem = "Função cadastrada com sucesso !"});
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult EditarFuncao(Funcao DadosCadastro, FormCollection Dados)
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Certificacoes = "";
                for (var i = 0; i < Dados.Count; i++)
                    if (Dados.AllKeys[i].Contains("Certificacao"))
                        Certificacoes = Certificacoes + Dados[i] + ";";

                VerificaDados.EditarFuncao(Dados["IdFuncao"], DadosCadastro.NomeFuncao, Certificacoes);
                return RedirectToAction("ColaboradorRh", "PainelColaborador",
                    new {Mensagem = "Função alterada com sucesso !"});
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult BuscarFuncao(string DescricaoFuncao)
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Funcoes = VerificaDados.BuscaFuncao(DescricaoFuncao);
                for (var i = 0; i < Funcoes.Count; i++)
                {
                    TempData["Id" + i] = Funcoes[i]["id"];
                    TempData["Descricao" + i] = Funcoes[i]["descricao"];
                }

                TempData["TotalResultado"] = Funcoes.Count;
                TempData["Editar"] = "EditarFuncao";
                return PartialView("ResultadoPesquisa");
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult RecuperaFuncao(string IdFuncao)
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Certificacoes = VerificaDados.RetornaCertificacoes();
                var DadosFuncao = VerificaDados.RecuperaDadosFuncao(IdFuncao);
                var FuncaoRecupera = new Funcao();


                var CertificacoesFuncao = DadosFuncao[0]["idcertificacao"].Split(';');
                TempData["TotalCertificacaoEditar"] = Certificacoes.Count;
                var Existe = false;
                for (var i = 0; i < Certificacoes.Count; i++)
                {
                    for (var j = 0; j < CertificacoesFuncao.Length; j++)
                        if (Certificacoes[i]["id"].Equals(CertificacoesFuncao[j]))
                            Existe = true;

                    if (Existe)
                    {
                        TempData["DescricaoCertificacaoEditar" + i] = Certificacoes[i]["descricao"];
                        TempData["IdCertificacaoEditar" + i] = Certificacoes[i]["id"];
                        TempData["CertificaFuncao" + i] = "checked";
                    }
                    else
                    {
                        TempData["DescricaoCertificacaoEditar" + i] = Certificacoes[i]["descricao"];
                        TempData["IdCertificacaoEditar" + i] = Certificacoes[i]["id"];
                        TempData["CertificaFuncao" + i] = "";
                    }

                    Existe = false;
                }

                FuncaoRecupera.NomeFuncao = DadosFuncao[0]["descricao"];
                TempData["IdFuncao"] = DadosFuncao[0]["id"];
                return PartialView("EditarFuncao", FuncaoRecupera);
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult BuscaPendencia(string DataConsulta)
        {
            var Validacoes = new ValidacoesPonto();
            var TabelasPonto = Validacoes.ValidarPonto(Convert.ToDateTime(DataConsulta));

            #region Preenchimento Pendencias Ponto

            TempData["TotalPonto"] = TabelasPonto[0].Count;
            TempData["ExtraPendente1"] = "hidden";
            TempData["ExtraPendente2"] = "hidden";


            for (var i = 0; i < TabelasPonto[0].Count; i++)
            {
                TempData["TotalHorarioPonto" + i] = Convert.ToInt32(TabelasPonto[0][i]["TotalHorario"]);

                TempData["IdPonto" + i] = TabelasPonto[0][i]["IdFuncionario"];

                TempData["NomePonto" + i] = TabelasPonto[0][i]["NomeFuncionario"];

                TempData["Dia"] = Convert.ToDateTime(TabelasPonto[0][i]["DataPendencia"]).ToString("dd/MM/yyyy");
                TempData["TotalHorarioPonto" + i] = Convert.ToInt32(TabelasPonto[0][i]["TotalHorario"]);

                for (var j = 0; j < Convert.ToInt32(TabelasPonto[0][i]["TotalHorario"]); j++)
                    TempData["Hora" + j + i] = TabelasPonto[0][i]["Hora" + j];

                if (4 - Convert.ToInt32(TabelasPonto[0][i]["TotalHorario"]) > 0) TempData["TotalHorarioPonto" + i] = 4;

                if (Convert.ToInt32(TabelasPonto[0][i]["TotalHorario"]) == 5)
                {
                    TempData["ExtraPendente1"] = "";
                }
                else if (Convert.ToInt32(TabelasPonto[0][i]["TotalHorario"]) == 6)
                {
                    TempData["ExtraPendente1"] = "";
                    TempData["ExtraPendente2"] = "";
                }
            }

            #endregion


            return PartialView("ValidacaoPonto");
        }

        public ActionResult BuscaSemPendencia(string DataConsulta)
        {
            var Validacoes = new ValidacoesPonto();
            var TabelasPonto = Validacoes.ValidarPonto(Convert.ToDateTime(DataConsulta));


            #region Preenchimento Sem pendencia

            TempData["TotalSemPendencia"] = TabelasPonto[1].Count;
            for (var i = 0; i < TabelasPonto[1].Count; i++)
            {
                TempData["IdPontoCerto" + i] = TabelasPonto[1][i]["IdFuncionario"];

                TempData["NomePontoCerto" + i] = TabelasPonto[1][i]["NomeFuncionario"];

                TempData["DiaCerto" + i] =
                    Convert.ToDateTime(TabelasPonto[1][i]["DataPendencia"]).ToString("dd/MM/yyyy");

                TempData["HoraCerto0" + i] = TabelasPonto[1][i]["Hora0"];
                TempData["HoraCerto1" + i] = TabelasPonto[1][i]["Hora1"];
                TempData["HoraCerto2" + i] = TabelasPonto[1][i]["Hora2"];
                TempData["HoraCerto3" + i] = TabelasPonto[1][i]["Hora3"];

                TempData["JornadaCerto" + i] = TabelasPonto[1][i]["Jornada"];
                if (TimeSpan.Parse(TabelasPonto[1][i]["HoraExtra"]).Hours <= 0 &&
                    TimeSpan.Parse(TabelasPonto[1][i]["HoraExtra"]).Minutes < 0)
                {
                    TempData["DebitoCerto" + i] = TabelasPonto[1][i]["HoraExtra"];
                    TempData["HoraExtraCerto" + i] = "";
                }
                else
                {
                    TempData["DebitoCerto" + i] = "";
                    TempData["HoraExtraCerto" + i] = TabelasPonto[1][i]["HoraExtra"];
                }
            }

            #endregion


            return PartialView("SemPendencia");
        }

        public ActionResult ConfirmarJustificativa(string IdPendencia)
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Firebird = new QueryFirebird();

                var DadosPendencia = VerificaDados.RecuperaDadosPendenciaConfirmacao(IdPendencia);
                var DadosPendenciaFire = Firebird.RetornaDadosMarcacao(DadosPendencia[0]["data"],
                    DadosPendencia[0]["idfuncionariofirebird"]);

                var Existe = false;
                var NegaFireBird = false;
                for (var i = 0; i < DadosPendencia.Count; i++)
                {
                    Existe = false;
                    if (DadosPendencia[i]["observacao"] == null)
                    {
                        if (DadosPendencia[i]["horario"].Equals("00:00:00"))
                        {
                            NegaFireBird = true;
                        }
                        else
                        {
                            for (var j = 0; j < DadosPendenciaFire.Count; j++)
                                if (DadosPendenciaFire[j]["HORA"].Equals(DadosPendencia[i]["horario"]))
                                {
                                    Existe = true;
                                    break;
                                }
                                else
                                {
                                    Existe = false;
                                }

                            if (!Existe)
                            {
                                Firebird.InserirMarcacao(DadosPendencia[i]["idfuncionariofirebird"],
                                    DadosPendencia[i]["idjustificativafirebird"], DadosPendencia[i]["data"],
                                    DadosPendencia[i]["horario"]);
                                VerificaDados.AtualizaJustificativaRh(DadosPendencia[i]["idhistorico"]);
                            }
                        }
                    }
                    else
                    {
                        NegaFireBird = true;
                    }
                }

                if (NegaFireBird) VerificaDados.AtualizaJustificativaRh(DadosPendencia[0]["idhistorico"]);


                return RedirectToAction("ColaboradorRh", "PainelColaborador",
                    new {Mensagem = "Justificativa confirmada com sucesso !"});
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult ExcluirFuncao(string IdFuncao)
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                VerificaDados.ExcluirFuncao(IdFuncao);
                return RedirectToAction("ColaboradorRh", "PainelColaborador",
                    new {Mensagem = "Função excluida com sucesso !"});
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult Certificacao()
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado) return PartialView("Certificacao");

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult Certificacao(Certificacao DadosCadastro)
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                VerificaDados.CadastrarCertificacao(DadosCadastro.NomeCertificacao);
                return RedirectToAction("ColaboradorRh", "PainelColaborador",
                    new {Mensagem = "Certificação cadastrada com sucesso !"});
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult EditarCertificacao(Certificacao DadosCadastro, FormCollection Dados)
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                VerificaDados.EditarCertificacao(Dados["IdCertificacao"], DadosCadastro.NomeCertificacao);
                return RedirectToAction("ColaboradorRh", "PainelColaborador",
                    new {Mensagem = "Certificação atualizada com sucesso !"});
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult ExcluirCertificacao(string IdFuncao)
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                VerificaDados.ExcluirCertificacao(IdFuncao);
                return RedirectToAction("ColaboradorRh", "PainelColaborador",
                    new {Mensagem = "Certificação excluida com sucesso !"});
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult BuscarCertificacao(string DescricaoCertificacao)
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Certificacao = VerificaDados.BuscaCertificacao(DescricaoCertificacao);
                for (var i = 0; i < Certificacao.Count; i++)
                {
                    TempData["Id" + i] = Certificacao[i]["id"];
                    TempData["Descricao" + i] = Certificacao[i]["descricao"];
                }

                TempData["TotalResultado"] = Certificacao.Count;
                TempData["Editar"] = "EditarCertificacao";
                return PartialView("ResultadoPesquisaCertificacao");
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult RecuperaCertificacao(string IdCertificacao)
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var DadosCertificacao = VerificaDados.RecuperaDadosCertificacao(IdCertificacao);
                var CertificacaoRecupera = new Certificacao();

                CertificacaoRecupera.NomeCertificacao = DadosCertificacao[0]["descricao"];
                TempData["IdCertificacao"] = DadosCertificacao[0]["id"];
                return PartialView("EditarCertificacao", CertificacaoRecupera);
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult Gestor()
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Setores = VerificaDados.RetornaSetores();
                TempData["TotalSetores"] = Setores.Count;
                for (var i = 0; i < Setores.Count; i++)
                {
                    TempData["DescricaoSetor" + i] = Setores[i]["descricao"];
                    TempData["IdSetor" + i] = Setores[i]["id"];
                }

                var Funcionarios = VerificaDados.RetornaFuncionarios();
                TempData["TotalFuncionarios"] = Setores.Count;
                for (var i = 0; i < Funcionarios.Count; i++)
                {
                    TempData["Nome" + i] = Funcionarios[i]["nome"];
                    TempData["IdFuncionario" + i] = Funcionarios[i]["id"];
                }

                return PartialView("Gestor");
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult Gestor(FormCollection DadosCadastro)
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                VerificaDados.CadastrarGestor(DadosCadastro["Funcionario"], DadosCadastro["Setor"]);
                return RedirectToAction("ColaboradorRh", "PainelColaborador",
                    new {Mensagem = "Gestor cadastrado com sucesso !"});
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult EditarGestor(FormCollection Dados)
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                VerificaDados.EditarGestor(Dados["FuncionarioEdicao"], Dados["SetorEdicao"]);
                return RedirectToAction("ColaboradorRh", "PainelColaborador",
                    new {Mensagem = "Certificação atualizada com sucesso !"});
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult ExcluirGestor(string IdGestor)
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                VerificaDados.ExcluirGestor(IdGestor);
                return RedirectToAction("ColaboradorRh", "PainelColaborador",
                    new {Mensagem = "Gestor removido com sucesso !"});
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult BuscarGestor(string DescricaoGestor)
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Gestor = VerificaDados.BuscaGestor(DescricaoGestor);
                for (var i = 0; i < Gestor.Count; i++)
                {
                    TempData["Id" + i] = Gestor[i]["id"];
                    TempData["Descricao" + i] = Gestor[i]["nome"];
                }

                TempData["TotalResultado"] = Gestor.Count;
                TempData["Editar"] = "EditarGestor";

                return PartialView("ResultadoPesquisaGestor");
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult RecuperaGestor(string IdGestor)
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var DadosGestor = VerificaDados.RecuperaDadosGestor(IdGestor);

                var Setores = VerificaDados.RetornaSetores();
                TempData["TotalSetores"] = Setores.Count;
                for (var i = 0; i < Setores.Count; i++)
                {
                    TempData["DescricaoSetor" + i] = Setores[i]["descricao"];
                    TempData["IdSetor" + i] = Setores[i]["id"];
                    if (Setores[i]["id"].Equals(DadosGestor[0]["idsetor"]))
                        TempData["ValorSetor" + i] = "checked";
                    else
                        TempData["ValorSetor" + i] = "";
                }

                var Funcionarios = VerificaDados.RetornaFuncionarios();
                TempData["TotalFuncionarios"] = Setores.Count;
                for (var i = 0; i < Funcionarios.Count; i++)
                {
                    TempData["Nome" + i] = Funcionarios[i]["nome"];
                    TempData["IdFuncionario" + i] = Funcionarios[i]["id"];
                    if (Funcionarios[i]["id"].Equals(DadosGestor[0]["id"]))
                        TempData["ValorGestor" + i] = "checked";
                    else
                        TempData["ValorGestor" + i] = "";
                }

                TempData["ExcluirIdGestor"] = DadosGestor[0]["id"];
                TempData["ExcluirIdSetor"] = DadosGestor[0]["idsetor"];

                return PartialView("EditarGestor");
            }

            return RedirectToAction("Login", "Login");
        }


        public ActionResult Setor()
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado) return PartialView("Setor");

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult Setor(Setor DadosCadastro)
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                VerificaDados.CadastrarSetor(DadosCadastro.NomeSetor);
                return RedirectToAction("ColaboradorRh", "PainelColaborador",
                    new {Mensagem = "Setor cadastrado com sucesso !"});
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult EditarSetor(Setor DadosCadastro, FormCollection Dados)
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                VerificaDados.EditarSetor(Dados["IdSetor"], DadosCadastro.NomeSetor);
                return RedirectToAction("ColaboradorRh", "PainelColaborador",
                    new {Mensagem = "Setor atualizada com sucesso !"});
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult ExcluirSetor(string IdSetor)
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                VerificaDados.ExcluirSetor(IdSetor);
                return RedirectToAction("ColaboradorRh", "PainelColaborador",
                    new {Mensagem = "Setor excluido com sucesso !"});
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult BuscarSetor(string DescricaoSetor)
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Setor = VerificaDados.BuscaSetor(DescricaoSetor);

                for (var i = 0; i < Setor.Count; i++)
                {
                    TempData["Id" + i] = Setor[i]["id"];
                    TempData["Descricao" + i] = Setor[i]["descricao"];
                }

                TempData["TotalResultado"] = Setor.Count;
                TempData["Editar"] = "EditarSetor";

                return PartialView("ResultadoPesquisaSetor");
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult RecuperaSetor(string IdSetor)
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var DadosCertificacao = VerificaDados.RecuperaDadosSetor(IdSetor);
                var SetorRecupera = new Setor();

                SetorRecupera.NomeSetor = DadosCertificacao[0]["descricao"];
                TempData["IdSetor"] = DadosCertificacao[0]["id"];
                return PartialView("EditarSetor", SetorRecupera);
            }

            return RedirectToAction("Login", "Login");
        }


        public ActionResult Indicadores()
        {
            return View("Indicadores");
        }

        public ActionResult OitoHoras()
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Fire = new QueryFirebird();
                var Acima = 0;
                var Abaixo = 0;
                var Acima12 = 0;
                var count = 0;

                var BancoDeHoras = VerificaDados.RetornaBancoHoras();
                TempData["ValorUAD"] = 0;
                TempData["ValorMatriz"] = 0;
                TempData["ValorParana"] = 0;
                TempData["ValorCajuru"] = 0;
                TempData["ValorClara"] = 0;
                TempData["ValorBh"] = 0;
                TempData["ValorBetim"] = 0;
                TempData["ValorContagem"] = 0;
                TempData["ValorGoias"] = 0;
                TempData["ValorBarreiro"] = 0;
                var TotalMatriz = new TimeSpan();
                var TotalFuncionariosMatriz = 0;

                for (var i = 0; i < BancoDeHoras.Count; i++)
                {
                    var Nome = Fire.RetornaFuncionarioMatricula(BancoDeHoras[i]["crachafirebird"]);


                    try
                    {
                        if (Nome[0]["ID_CENTRO_CUSTO"].Equals("1"))
                        {
                            TotalMatriz = TotalMatriz.Add(TimeSpan.Parse(BancoDeHoras[i]["hora"]));
                            TotalFuncionariosMatriz++;
                        }
                        else if (Nome[0]["ID_CENTRO_CUSTO"].Equals("2"))
                        {
                        }
                        else if (Nome[0]["ID_CENTRO_CUSTO"].Equals("3"))
                        {
                        }
                        else if (Nome[0]["ID_CENTRO_CUSTO"].Equals("4"))
                        {
                        }
                        else if (Nome[0]["ID_CENTRO_CUSTO"].Equals("5"))
                        {
                        }
                        else if (Nome[0]["ID_CENTRO_CUSTO"].Equals("6"))
                        {
                        }
                        else if (Nome[0]["ID_CENTRO_CUSTO"].Equals("7"))
                        {
                        }
                        else if (Nome[0]["ID_CENTRO_CUSTO"].Equals("8"))
                        {
                        }
                        else if (Nome[0]["ID_CENTRO_CUSTO"].Equals("10"))
                        {
                        }
                        else if (Nome[0]["ID_CENTRO_CUSTO"].Equals("11"))
                        {
                        }
                    }
                    catch
                    {
                    }

                    try
                    {
                        if (Convert.ToInt32(BancoDeHoras[i]["hora"].Split(':')[0]) >= 8 &&
                            Convert.ToInt32(BancoDeHoras[i]["hora"].Split(':')[0]) <= 11)
                        {
                            if (Nome.Count > 0)
                            {
                                TempData["Nome" + count] = Nome[0]["NOME"];
                                TempData["Data" + count] = Convert.ToDateTime(BancoDeHoras[i]["datareferencia"])
                                    .ToString("dd/MM/yyyy");
                                TempData["TotalHoras" + count] = BancoDeHoras[i]["hora"];
                                Acima++;
                                count++;
                            }
                        }
                        else if (Convert.ToInt32(BancoDeHoras[i]["hora"].Split(':')[0]) >= 12)
                        {
                            if (Nome.Count > 0)
                            {
                                TempData["Nome" + count] = Nome[0]["NOME"];
                                TempData["Data" + count] = Convert.ToDateTime(BancoDeHoras[i]["datareferencia"])
                                    .ToString("dd/MM/yyyy");
                                TempData["TotalHoras" + count] = BancoDeHoras[i]["hora"];
                                Acima12++;
                                count++;
                            }
                        }
                        else
                        {
                            Abaixo++;
                        }
                    }
                    catch
                    {
                        if (Nome.Count > 0)
                        {
                            TempData["Nome" + count] = Nome[0]["NOME"];
                            TempData["Data" + count] = Convert.ToDateTime(BancoDeHoras[i]["datareferencia"])
                                .ToString("dd/MM/yyyy");
                            TempData["TotalHoras" + count] = BancoDeHoras[i]["hora"];
                            Acima++;
                        }
                    }
                }

                TempData["Total"] = count;
                TempData["Valor1"] = Abaixo;
                TempData["Valor2"] = Acima;
                TempData["Valor3"] = Acima12;
                TempData["ValorMatriz"] = TotalMatriz.Hours / TotalFuncionariosMatriz;


                return PartialView("IndicadorOitoHoras");
            }

            return RedirectToAction("Login", "Login");
        }
    }
}