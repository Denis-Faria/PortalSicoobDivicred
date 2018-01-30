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
            var VerificaDados = new QueryMysql();
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
                TempData["Funcao"] = FuncaoFuncionario;
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
            var VerificaDados = new QueryMysql();
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
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var CarregaDados = new QueryMysql();
                var DadosColaborador = CarregaDados.RecuperaDadosFuncionariosSetor();

                var QueryRh = new QueryMysqlRh();
                var VagasInternas = QueryRh.RetornaVagaInternaTotal();

                TempData["TotalColaborador"] = DadosColaborador.Count;
                TempData["Total"] = VagasInternas.Count;

                var Validacoes = new ValidacoesPonto();
                var TabelasPonto = Validacoes.ValidarPonto();

                #region Preenchimento Pendencias Ponto

                TempData["TotalPonto"] = TabelasPonto[0].Count;
                TempData["ExtraPendente1"] = "hidden";
                TempData["ExtraPendente2"] = "hidden";
                TempData["ExtraJustifica1"] = "hidden";
                TempData["ExtraJustifica2"] = "hidden";
                
                for (int i = 0; i < TabelasPonto[0].Count; i++)
                {
                    TempData["TotalHorarioPonto"+i] = Convert.ToInt32(TabelasPonto[0][i]["TotalHorario"]);

                    TempData["IdPonto" + i] = TabelasPonto[0][i]["IdFuncionario"];

                    TempData["NomePonto" + i] = TabelasPonto[0][i]["NomeFuncionario"];

                    TempData["Dia"] = Convert.ToDateTime(TabelasPonto[0][i]["DataPendencia"]).ToString("dd/MM/yyyy");
                    TempData["TotalHorarioPonto" + i] = Convert.ToInt32(TabelasPonto[0][i]["TotalHorario"]);

                    for (int j = 0; j < Convert.ToInt32(TabelasPonto[0][i]["TotalHorario"]); j++)
                    {
                        TempData["Hora" + j +i] = TabelasPonto[0][i]["Hora" + j];
                    }
                    if (4 - Convert.ToInt32(TabelasPonto[0][i]["TotalHorario"]) > 0)
                    {
                        TempData["TotalHorarioPonto" + i] = 4;
                    }
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

                #region Preenchimento Sem pendencia

                TempData["TotalSemPendencia"] = TabelasPonto[1].Count;
                for (int i = 0; i < TabelasPonto[1].Count; i++)
                {
                    TempData["IdPontoCerto" + i] = TabelasPonto[1][i]["IdFuncionario"];

                    TempData["NomePontoCerto" + i] = TabelasPonto[1][i]["NomeFuncionario"];

                    TempData["DiaCerto"+i] = Convert.ToDateTime(TabelasPonto[1][i]["DataPendencia"]).ToString("dd/MM/yyyy");

                    TempData["HoraCerto0"+i] = TabelasPonto[1][i]["Hora0"];
                    TempData["HoraCerto1"+i] = TabelasPonto[1][i]["Hora1"];
                    TempData["HoraCerto2"+i] = TabelasPonto[1][i]["Hora2"];
                    TempData["HoraCerto3"+i] = TabelasPonto[1][i]["Hora3"];

                    TempData["JornadaCerto"+i]= TabelasPonto[1][i]["Jornada"];
                    if (TimeSpan.Parse(TabelasPonto[1][i]["HoraExtra"]).Hours <= 0 && TimeSpan.Parse(TabelasPonto[1][i]["HoraExtra"]).Minutes<0)
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

                #region Preenchimento pendencias MYSQL

                TempData["TotalSemConfirmar"] = TabelasPonto[2].Count;
                for (int i = 0; i < TabelasPonto[2].Count; i++)
                {
                    TempData["IdPendencia" + i] = TabelasPonto[2][i]["IdPendencia"];


                    TempData["NomePendencia" + i] = TabelasPonto[2][i]["Nome"];


                    TempData["DiaPendencia" + i] = Convert.ToDateTime(TabelasPonto[2][i]["Data"]).ToString("dd/MM/yyyy");

                    TempData["TotalHorarioPendencia" + i] = Convert.ToInt32(TabelasPonto[2][i]["TotalHorario"]);
                    
                    for (int j = 0; j < Convert.ToInt32(TabelasPonto[2][i]["TotalHorario"]); j++)
                    {
                       
                        TempData["Hora" + j + "Pendencia" + i] = TabelasPonto[2][i]["Horario"+j];
                    }
                    if (4 - Convert.ToInt32(TabelasPonto[2][i]["TotalHorario"]) > 0)
                    {
                        TempData["TotalHorarioPendencia" + i] = 4;
                    }

                    if (Convert.ToBoolean(TabelasPonto[2][i]["Justificado"]))
                    {
                        TempData["StatusJustificativa" + i] = "green";
                        TempData["Justificativa" + i] = TabelasPonto[2][i]["Justificativa" + i];
                        
                    }
                    else
                    {
                        TempData["Justificativa" + i] = "NÃO JUSTIFICADO";
                        TempData["StatusJustificativa" + i] = "red";
                        
                    }

                    TempData["StatusJustificativaGetor" + i]= TabelasPonto[2][i]["ConfirmaGestor"];
                    if (Convert.ToInt32(TabelasPonto[2][i]["TotalHorario"]) == 5)
                    {
                        TempData["ExtraJustifica1"] = "";
                    }
                    else if (Convert.ToInt32(TabelasPonto[2][i]["TotalHorario"]) == 6)
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


                return PartialView("ColaboradorRh");
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

                    var bytes = (byte[])DocumentosUpados.Rows[i]["arquivo"];
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
            var VerificaDados = new QueryMysqlCurriculo();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var RecuperaDados = new QueryMysqlRh();
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
            var VerificaDados = new QueryMysqlRh();
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
                        VerificaDados.AtualizaStatus(Formulario["vaga"], IdFuncionario[1], Aprovado, Observacao);
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
                        RecuperaId.RecuperaDadosFuncionariosTabelaFuncionarios(TabelaPendencias[i].Nome.Replace("'",""));
                    try
                    {
                        var IdJustificativa = VerificaDados.InserirHistoricoJustificativa(DadosFuncionario[0]["id"], Convert.ToDateTime(TabelaPendencias[i].Dia));
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
                        if (TabelaPendencias[i].Horario6.Length == 0 || TabelaPendencias[i].Horario4.Contains("--") || TabelaPendencias[i].Horario6.Contains("<"))
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

        public ActionResult NegarJustificativa(object idhistorico)
        {
            throw new NotImplementedException();
        }
    }
}