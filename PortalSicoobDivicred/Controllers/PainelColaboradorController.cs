using System;
using System.Globalization;
using System.IO;
using System.Web.Mvc;
using PortalSicoobDivicred.Aplicacao;
using PortalSicoobDivicred.Models;

namespace PortalSicoobDivicred.Controllers
{
    public class PainelColaboradorController : Controller
    {
        // GET: PainelColaborador
        public ActionResult Perfil()
        {
            var VerificaDados = new QuerryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
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
                TempData["Funcao"] = FuncaoFuncionario[0]["descricao"];
                TempData["IdFuncao"] = DadosTabelaFuncionario[0]["funcao"];
                TempData["DataAdmissao"] =
                    Convert.ToDateTime(DadosTabelaFuncionario[0]["admissao"]).ToString("dd/MM/yyyy");

                var EstadoCivil = VerificaDados.RetornaEstadoCivil();
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


                return PartialView("Perfil", DadosFuncionario);
            }
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult AtualizarDadosProfissionais(Funcionario DadosFuncionario)
        {
            var VerificaDados = new QuerryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Cookie = Request.Cookies.Get("CookieFarm");
                var Login = Criptografa.Descriptografar(Cookie.Value);

                VerificaDados.AtualizaDadosFuncionarioProfissional(DadosFuncionario.IdSetor.ToString(),
                    DadosFuncionario.IdFuncao.ToString(), Login);

                return RedirectToAction("Principal", "Principal",
                    new
                    {
                        Acao = "Perfil",
                        Mensagem = "Dados Profissionais atualizados com sucesso !",
                        Controlle = "PainelColaborador"
                    });
            }
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult AtualizaDadosPessoais(Funcionario DadosFuncionario, FormCollection Formulario)
        {
            var VerificaDados = new QuerryMysql();
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
                    DadosFuncionario.Rua, DadosFuncionario.Numero, DadosFuncionario.Bairro, DadosFuncionario.Cidade,"S");

                return RedirectToAction("Principal", "Principal",
                    new
                    {
                        Acao = "Perfil",
                        Mensagem = "Dados Pessoais atualizados com sucesso !",
                        Controlle = "PainelColaborador"
                    });
            }
            return RedirectToAction("Login", "Login");
        }

        public ActionResult Upload()
        {
            var InserirFoto = new QuerryMysql();
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
            var InserirFoto = new QuerryMysql();
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
            var VerificaDados = new QuerryMysql();
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

                return RedirectToAction("Principal", "Principal",
                    new
                    {
                        Acao = "Perfil",
                        Mensagem = "Formulário Pessoal atualizado com sucesso !",
                        Controlle = "PainelColaborador"
                    });
            }
            return RedirectToAction("Login", "Login");
        }

        public ActionResult ColaboradorRh()
        {
            var VerificaDados = new QuerryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var CarregaDados = new QuerryMysql();
                var FirebirDados = new QueryFirebird();
                var Ponto = FirebirDados.RetornaListaMArcacao();

                var FuncionariosPonto = FirebirDados.RetornaListaFuncionario();

                var TotalJustifica = 0;
                var TotalSemPendencia = 0;
                for (int j = 0; j < FuncionariosPonto.Count; j++)
                {

                    if (FuncionariosPonto[j]["DATA_DEMISSAO"] == null)
                    {
                        var Falta = FirebirDados.VerificaFalta(FuncionariosPonto[j]["ID_FUNCIONARIO"]);
                        if (Falta.Count > 0)
                        {
                            if (Falta.Count == 4)
                            {
                                TempData["NomePontoCerto" + TotalSemPendencia] = FuncionariosPonto[j]["NOME"];
                                TempData["HoraCerto1" + TotalSemPendencia] = Falta[0]["HORA"];
                                TempData["HoraCerto2" + TotalSemPendencia] = Falta[1]["HORA"];
                                TempData["HoraCerto3" + TotalSemPendencia] = Falta[2]["HORA"];
                                TempData["HoraCerto4" + TotalSemPendencia] = Falta[3]["HORA"];

                                TotalSemPendencia++;
                                DateTime Marcacao1 = DateTime.ParseExact(Falta[0]["HORA"], "HH:mm:ss",
                                        new DateTimeFormatInfo());
                                DateTime Marcacao2 = DateTime.ParseExact(Falta[1]["HORA"], "HH:mm:ss",
                                    new DateTimeFormatInfo());
                                DateTime Marcacao3 = DateTime.ParseExact(Falta[2]["HORA"], "HH:mm:ss",
                                    new DateTimeFormatInfo());
                                DateTime Marcacao4 = DateTime.ParseExact(Falta[3]["HORA"], "HH:mm:ss",
                                    new DateTimeFormatInfo());
                                TimeSpan Jornada = new TimeSpan(8,0,0);
                              


                            }
                        }
                        else
                        {
                            var Afastamento =
                                FirebirDados.VerificaAfastamento(FuncionariosPonto[j]["ID_FUNCIONARIO"]);
                            if (Afastamento.Count == 0)
                            {
                                TempData["NomePonto" + TotalJustifica] = FuncionariosPonto[j]["NOME"];
                                TempData["Hora1" + TotalJustifica] = "--:--";
                                TempData["Hora2" + TotalJustifica] = "--:--";
                                TempData["Hora3" + TotalJustifica] = "--:--";
                                TempData["Hora4" + TotalJustifica] = "--:--";
                                TotalJustifica++;
                            }
                        }
                    }

                }



                for (int i = 0; i < Ponto.Count; i++)
                {
                    if (Ponto[i]["ID_CARGO"].Equals("2") || Ponto[i]["ID_CARGO"].Equals("58"))
                    {
                        TempData["NomePonto" + TotalJustifica] = Ponto[i]["NOME"];
                        var Marcacao = FirebirDados.VerificaFalta(Ponto[i]["ID_FUNCIONARIO"]);
                        if (Marcacao.Count > 2)
                        {
                            TempData["NomePonto" + TotalJustifica] = Ponto[i]["NOME"];
                            for (int k = 0; k < Marcacao.Count; k++)
                            {
                                if (k == 0)
                                {
                                    TempData["Hora1" + TotalJustifica] = Marcacao[k]["HORA"];
                                }
                                else if (k == 1)
                                {
                                    TempData["Hora2" + TotalJustifica] = Marcacao[k]["HORA"];

                                }
                                else if (k == 2)
                                {
                                    TempData["Hora3" + TotalJustifica] = Marcacao[k]["HORA"];
                                }
                                else
                                {
                                    TempData["Hora4" + TotalJustifica] = Marcacao[k]["HORA"];
                                }
                            }

                            TotalJustifica++;
                        }
                        else
                        {
                            if (Marcacao.Count == 2)
                            {
                                DateTime Marcacao1 = DateTime.ParseExact(Marcacao[0]["HORA"], "HH:mm:ss",
                                    new DateTimeFormatInfo());
                                DateTime Marcacao2 = DateTime.ParseExact(Marcacao[1]["HORA"], "HH:mm:ss",
                                    new DateTimeFormatInfo());
                                TimeSpan ts = Marcacao2.Subtract(Marcacao1);

                                if (ts.Hours >= 6 && ts.Minutes > 5)
                                {
                                    TempData["NomePonto" + TotalJustifica] = Ponto[i]["NOME"];
                                    TempData["Hora1" + TotalJustifica] = Marcacao[0]["HORA"];
                                    TempData["Hora2" + TotalJustifica] = Marcacao[1]["HORA"];


                                    TotalJustifica++;
                                }
                            }
                        }

                    }
                    else
                    {
                        if (Ponto[i]["JUSTIFICA"].Equals("Justifica"))
                        {
                            TempData["NomePonto" + TotalJustifica] = Ponto[i]["NOME"];
                            var Marcacao = FirebirDados.VerificaFalta(Ponto[i]["ID_FUNCIONARIO"]);
                            for (int k = 0; k < Marcacao.Count; k++)
                            {
                                if (k == 0)
                                {
                                    TempData["Hora1" + TotalJustifica] = Marcacao[k]["HORA"];
                                }
                                else if (k == 1)
                                {
                                    TempData["Hora2" + TotalJustifica] = Marcacao[k]["HORA"];

                                }
                                else if (k == 2)
                                {
                                    TempData["Hora3" + TotalJustifica] = Marcacao[k]["HORA"];
                                }
                                else
                                {
                                    TempData["Hora4" + TotalJustifica] = Marcacao[k]["HORA"];
                                }
                            }

                            TotalJustifica++;
                        }
                    }

                }
                TempData["TotalPonto"] = TotalJustifica;
                TempData["TotalSemPendencia"] = TotalSemPendencia;

                var DadosColaborador = CarregaDados.RecuperaDadosFuncionarios();

                var QueryRh = new QuerryMysqlRh();
                var VagasInternas = QueryRh.RetornaVagaInternaTotal();

                TempData["TotalColaborador"] = DadosColaborador.Count;
                TempData["Total"] = VagasInternas.Count;

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


                return PartialView("ColaboradorRh");
            }
            return RedirectToAction("Login", "Login");
        }

        public ActionResult PerfilFuncionario(string Login)
        {
            var VerificaDados = new QuerryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
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

                if (DadosTabelaFuncionario[0]["foto"] == null)
                    TempData["Foto"] = "http://bulma.io/images/placeholders/128x128.png";
                else
                    TempData["Foto"] = "/Uploads/" + DadosTabelaFuncionario[0]["foto"];
                TempData["IdEstadoCivil"] = DadosTabelaFuncionario[0]["idestadocivil"];
                TempData["IdSexo"] = DadosTabelaFuncionario[0]["sexo"];
                TempData["IdEtnia"] = DadosTabelaFuncionario[0]["etnia"];
                TempData["IdFormacao"] = DadosTabelaFuncionario[0]["idescolaridade"];
                TempData["IdSetor"] = DadosTabelaFuncionario[0]["idsetor"];
                TempData["NomeFuncionario"] = DadosTabelaFuncionario[0]["nome"];
                TempData["Funcao"] = DadosTabelaFuncionario[0]["funcao"];
                TempData["DataAdmissao"] =
                    Convert.ToDateTime(DadosTabelaFuncionario[0]["admissao"]).ToString("dd/MM/yyyy");

                var EstadoCivil = VerificaDados.RetornaEstadoCivil();
                var Sexo = VerificaDados.RetornaSexo();
                var Etnia = VerificaDados.RetornaEtnia();
                var Formacao = VerificaDados.RetornaFormacao();
                var Setor = VerificaDados.RetornaSetor();

                DadosFuncionario.EstadoCivil = EstadoCivil;
                DadosFuncionario.Sexo = Sexo;
                DadosFuncionario.Etnia = Etnia;
                DadosFuncionario.Formacao = Formacao;
                DadosFuncionario.Setor = Setor;

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


        public ActionResult AtualizarDadosRh(string Login)
        {
            return RedirectToAction("Principal", "Principal",
                new
                {
                    Acao = "ColaboradorRh ",
                    Mensagem = "Formulário Pessoal atualizado com sucesso !",
                    Controlle = "PainelColaborador"
                });
        }

        [HttpPost]
        public ActionResult CadastrarVaga(VagasInternas DadosVaga)
        {
            var VerificaDados = new QuerryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
                if (ModelState.IsValid)
                {
                    VerificaDados.CadastraVagaInterna(DadosVaga.Titulo, DadosVaga.Descricao, DadosVaga.Descricao);

                    return RedirectToAction("Principal", "Principal",
                        new
                        {
                            Acao = "ColaboradorRh ",
                            Mensagem = "Vaga interna criada com sucesso !",
                            Controlle = "PainelColaborador"
                        });
                }
                else
                {
                    return RedirectToAction("Principal", "Principal",
                        new
                        {
                            Acao = "ColaboradorRh ",
                            Mensagem = "Vaga interna não cadastrada !",
                            Controlle = "PainelColaborador"
                        });
                }
            return RedirectToAction("Login", "Login");
        }

        public ActionResult Vaga()
        {
            return PartialView("ModalVagaInterna");
        }


        public ActionResult AlterarVaga(string IdVaga)
        {
            var VerificaDados = new QuerryMysqlCurriculo();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var QueryRh = new QuerryMysqlRh();
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
            var VerificaDados = new QuerryMysqlCurriculo();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
                if (ModelState.IsValid)
                {
                    var QueryRh = new QuerryMysqlRh();

                    QueryRh.AtualizaVagaInterna(DadosVaga.Titulo, DadosVaga.Descricao, DadosVaga.Requisitos,
                        Formulario["IdVaga"]);
                    return RedirectToAction("Principal", "Principal",
                        new
                        {
                            Acao = "ColaboradorRh",
                            Mensagem = "Vaga alterada com sucesso!",
                            Controlle = "PainelColaborador"
                        });
                }
            return RedirectToAction("Login", "Login");
        }

        public ActionResult GerenciarVaga(string IdVaga)
        {
            var VerificaDados = new QuerryMysqlCurriculo();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var RecuperaDados = new QuerryMysqlRh();
                var DadosCurriculos = RecuperaDados.RecuperaFuncionariosVaga(IdVaga);
                TempData["TotalCurriculo"] = DadosCurriculos.Count;
                TempData["QuantidadeCurriculo"] = "N° de candidatos com interesse nesta vaga: " + DadosCurriculos.Count;
                TempData["TituloVaga"] = IdVaga + "-" + DadosCurriculos[0]["titulo"];
                TempData["DescricaoVaga"] = DadosCurriculos[0]["descricao"];
                TempData["ResultadoObservacao"] = DadosCurriculos[0]["observacao"];
                TempData["IdVaga"] = IdVaga;

                for (var i = 0; i < DadosCurriculos.Count; i++)
                {
                    TempData["Id " + i] = DadosCurriculos[i]["id"];
                    TempData["IdFuncionario" + i] = "IdFuncionario | " + DadosCurriculos[i]["id"];
                    TempData["Nome" + i] = DadosCurriculos[i]["nome"];
                    TempData["Login" + i] = DadosCurriculos[i]["login"];
                    TempData["Setor" + i] = DadosCurriculos[i]["setorfuncionario"];
                    TempData["PA" + i] = DadosCurriculos[i]["idpa"];
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
                if (DadosCurriculos[0]["encerrada"].Equals("N"))
                {
                    TempData["Ativa"] = "";
                    TempData["Dica"] = "Clique para encerrar esta vaga.";
                }
                else
                {
                    TempData["Ativa"] = "disabled";
                    TempData["Dica"] = "Esta vaga já esta encerrada.";
                }
                return PartialView("GerenciarVaga");
            }
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult EncerrarVaga(FormCollection Formulario)
        {
            var VerificaDados = new QuerryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                for (var i = 0; i < Formulario.Count; i++)
                    if (Formulario.AllKeys[i].Contains("IdFuncionario"))
                    {
                        var IdFuncionario = Formulario.AllKeys[i].Split('|');
                        var Aprovado = false;
                        var teste = Formulario[i];
                        if (Formulario[i].Equals("on"))
                            Aprovado = true;
                        else
                            Aprovado = false;
                        var Observacao = Formulario["observacao" + IdFuncionario[1]];
                        VerificaDados.AtualizaStatus(Formulario["vaga"], IdFuncionario[1], Aprovado,Observacao.ToString());
                    }
                VerificaDados.EncerraVaga(Formulario["vaga"]);
                return RedirectToAction("Principal", "Principal",
                    new
                    {
                        Acao = "ColaboradorRh ",
                        Mensagem = "Vaga interna encerrada com sucesso !",
                        Controlle = "PainelColaborador"
                    });
            }
            return RedirectToAction("Login", "Login");
        }
    }
}