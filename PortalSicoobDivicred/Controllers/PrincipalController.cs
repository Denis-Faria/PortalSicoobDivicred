using System;
using System.Linq;
using System.Web.Mvc;
using OneApi.Client.Impl;
using OneApi.Config;
using OneApi.Model;
using PortalSicoobDivicred.Aplicacao;
using PortalSicoobDivicred.Models;

namespace PortalSicoobDivicred.Controllers
{
    public class PrincipalController : Controller
    {
        // GET: Principal
        public ActionResult Principal(string Acao, string Mensagem, string Controlle)
        {
            var VerificaDados = new QueryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            try
            {
                if (!Acao.Equals("Dashboard"))

                {
                    TempData["Opcao"] = Acao;
                    TempData["Mensagem"] = Mensagem;
                    TempData["Controlle"] = Controlle;
                    ModelState.Clear();
                }
                else
                {
                    TempData["Opcao"] = "Dashboard";
                    TempData["Controle"] = "Principal";
                }
            }
            catch
            {
                TempData["Opcao"] = "Dashboard";
                TempData["Controle"] = "Principal";
            }
            if (Logado)
            {
                var Cookie = Request.Cookies.Get("CookieFarm");

                var Login = Criptografa.Descriptografar(Cookie.Value);
                if (VerificaDados.PrimeiroLogin(Login))
                    return RedirectToAction("FormularioCadastro", "Principal");
                var DadosUsuarioBanco = VerificaDados.RecuperaDadosUsuarios(Login);

                if (VerificaDados.PermissaoCurriculos(DadosUsuarioBanco[0]["login"]))
                    TempData["PermissaoCurriculo"] =
                        " ";
                else
                    TempData["PermissaoCurriculo"] = "";
                if (DadosUsuarioBanco[0]["gestor"].Equals("S"))
                {
                    TempData["PermissaoGestor"] = "hidden";
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
                return View();
            }

            return RedirectToAction("Login", "Login");
        }


        public ActionResult Dashboard()
        {
            var VerificaDados = new QueryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var QueryRh = new QueryMysqlRh();
                var Cookie = Request.Cookies.Get("CookieFarm");
                var Login = Criptografa.Descriptografar(Cookie.Value);
                var DadosTabelaFuncionario = VerificaDados.RecuperaDadosFuncionariosTabelaFuncionariosPerfil(Login);


                #region VagasInternas

                {
                    var VagasInternas = QueryRh.RetornaVagaInterna();

                    TempData["TotalVagasInternas"] = VagasInternas.Count();
                    var Interesse = QueryRh.RetornaInteresseVagaInterna(DadosTabelaFuncionario[0]["id"]);
                    for (var i = 0; i < VagasInternas.Count; i++)

                    {
                        TempData["TituloVagaInterna " + i] = VagasInternas[i]["titulo"];
                        TempData["Descricao " + i] = VagasInternas[i]["descricao"];
                        TempData["Requisito " + i] = VagasInternas[i]["requisito"];
                        TempData["IdVaga " + i] = VagasInternas[i]["id"];


                        for (var j = 0; j < Interesse.Count; j++)
                            if (Interesse[j]["idvaga"].Equals(VagasInternas[i]["id"]))
                                TempData["Interesse " + i] = "Ok";
                    }
                    TempData["TotalProcessos"] = Interesse.Count;
                    for (var j = 0; j < Interesse.Count; j++)
                    {
                        TempData["Aprovado " + j] = Interesse[j]["aprovado"];
                        TempData["TituloVagaInternaProcesso " + j] = Interesse[j]["titulo"];
                    }
                }

                #endregion



                return PartialView("Dashboard");
            }
            return RedirectToAction("Login", "Login");
        }

        public void SMS()
        {
            var Celular = "(37) 99988-3728";
            var EnvioSms = new string[1];
            var certo = "55" + Celular.Replace(')', ' ').Replace('(', ' ').Replace('-', ' ');
            EnvioSms[0] = certo.Replace(" ", "");

            var configuration = new Configuration("Divicred", "Euder17!");
            var smsClient = new SMSClient(configuration);
            var smsRequest = new SMSRequest("Portal Sicoob Divicred",
                "Portal Sicoob Divicred, Novas vagas cadastradas. ", EnvioSms);
            var requestId = smsClient.SmsMessagingClient.SendSMS(smsRequest);
        }

        public ActionResult FormularioCadastro()
        {
            var VerificaDados = new QueryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Cookie = Request.Cookies.Get("CookieFarm");
                var Login = Criptografa.Descriptografar(Cookie.Value);

                var DadosTabelaUsuario = VerificaDados.RecuperaDadosFuncionariosTabelaUsuario(Login);
                var DadosTabelaFuncionario =
                    VerificaDados.RecuperaDadosFuncionariosTabelaFuncionarios(DadosTabelaUsuario[0]["nome"]);

                var DadosFuncionario = new Funcionario();
                DadosFuncionario.NomeFuncionario = DadosTabelaFuncionario[0]["nome"];
                DadosFuncionario.CpfFuncionario = DadosTabelaFuncionario[0]["cpf"];
                DadosFuncionario.RgFuncionario = DadosTabelaFuncionario[0]["rg"];
                DadosFuncionario.PisFuncionario = DadosTabelaFuncionario[0]["pis"];
                DadosFuncionario.DataNascimentoFuncionario =
                    Convert.ToDateTime(DadosTabelaFuncionario[0]["datanascimento"]).ToString("dd/MM/yyyy");
                DadosFuncionario.FormacaoAcademica = DadosTabelaFuncionario[0]["formacaoacademica"];
                DadosFuncionario.UsuarioSistema = DadosTabelaUsuario[0]["login"];
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

                return View(DadosFuncionario);
            }
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult FormularioCadastro(Funcionario DadosFuncionario, FormCollection Formulario)
        {
            var VerificaDados = new QueryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                try
                {
                    if (!Formulario["ConfirmacaoDados"].Equals("on"))
                    {
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
                        ModelState.AddModelError("", "Favor confirmar que suas informações são verdadeiras");
                    }
                }
                catch
                {
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
                    ModelState.AddModelError("", "Favor confirmar que suas informações são verdadeiras");
                }
                if (ModelState.IsValid)
                {
                    var Cookie = Request.Cookies.Get("CookieFarm");
                    var Login = Criptografa.Descriptografar(Cookie.Value);

                    var DadosTabelaUsuario = VerificaDados.RecuperaDadosFuncionariosTabelaUsuario(Login);
                    var DadosTabelaFuncionario =
                        VerificaDados.RecuperaDadosFuncionariosTabelaFuncionarios(DadosTabelaUsuario[0]["nome"]);

                    var DescricaoSexo = "";
                    var DataNascimentoFilho = "";
                    if (DadosFuncionario.DescricaoSexo == null)
                        DescricaoSexo = "NÃO INFORMOU";
                    else
                        DescricaoSexo = DadosFuncionario.DescricaoSexo;
                    if (DadosFuncionario.DataNascimentoFilho == null)
                        DataNascimentoFilho = "NÂO TEM";
                    else
                        DataNascimentoFilho = DadosFuncionario.DataNascimentoFilho;
                    var Confirma = "";

                    try
                    {
                        if (Formulario["ConfirmacaoCertificacao"].Equals("on"))
                            Confirma = "S";
                        else
                            Confirma = "N";
                    }
                    catch
                    {
                        Confirma = "N";
                    }

                    for (var i = 0; i < Formulario.Count; i++)
                        if (Formulario.AllKeys[i].Contains("formacao"))
                            VerificaDados.InserirFormacao(Formulario[i], DadosTabelaFuncionario[0]["id"]);

                    VerificaDados.AtualizaDadosFuncionarioFormulario(DadosFuncionario.NomeFuncionario,
                        DadosFuncionario.CpfFuncionario, DadosFuncionario.RgFuncionario,
                        DadosFuncionario.PisFuncionario, DadosFuncionario.DataNascimentoFuncionario,
                        DadosFuncionario.IdSexo.ToString(), DescricaoSexo, DadosFuncionario.IdEtnia.ToString(),
                        DadosFuncionario.IdEstadoCivil.ToString(), DadosFuncionario.IdFormacao.ToString(),
                        DadosFuncionario.FormacaoAcademica,
                        DadosFuncionario.UsuarioSistema, DadosFuncionario.Email, DadosFuncionario.PA,
                        DadosFuncionario.Rua, DadosFuncionario.Numero, DadosFuncionario.Bairro,
                        DadosFuncionario.Cidade,
                        DadosFuncionario.IdSetor.ToString(), DadosFuncionario.IdFuncao.ToString(),
                        DadosFuncionario.QuatidadeFilho,
                        DataNascimentoFilho, DadosFuncionario.ContatoEmergencia,
                        DadosFuncionario.PrincipaisHobbies, DadosFuncionario.ComidaFavorita,
                        DadosFuncionario.Viagem,
                        Confirma, "S");
                    return RedirectToAction("Principal", "Principal");
                }
                return View(DadosFuncionario);
            }
            return RedirectToAction("Login", "Login");
        }

        public ActionResult BuscaCertificacao(string IdFuncao)
        {
            var VerificaDados = new QueryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var CertificacoesFuncao = VerificaDados.RetornaCertificacaoFuncao(IdFuncao);
                var IdCertificacoes = CertificacoesFuncao[0]["idcertificacao"].Split(';');

                TempData["TotalCertificacao"] = IdCertificacoes.Length;

                for (var j = 0; j < IdCertificacoes.Length; j++)
                {
                    var Certificacoes = VerificaDados.RetornaCertificacao(IdCertificacoes[j]);
                    TempData["Certificacao" + j] = Certificacoes[0]["descricao"];
                }
                return PartialView("CertificacaoFuncao");
            }
            return RedirectToAction("Login", "Login");
        }

        public ActionResult TenhoInteresse(string IdVaga)
        {
            var VerificaDados = new QueryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var QueryRh = new QueryMysqlRh();
                var Cookie = Request.Cookies.Get("CookieFarm");
                var Login = Criptografa.Descriptografar(Cookie.Value);

                var DadosTabelaFuncionario = VerificaDados.RecuperaDadosFuncionariosTabelaFuncionariosPerfil(Login);
                QueryRh.CadastraInteresse(IdVaga, DadosTabelaFuncionario[0]["id"]);
                return RedirectToAction("Principal", "Principal");
            }
            return RedirectToAction("Login", "Login");
        }

        public ActionResult Justificativas()
        {
            var Validacoes = new ValidacoesPonto();
            var QueryFire = new QueryFirebird();
            var Cookie = Request.Cookies.Get("CookieFarm");
            var Login = Criptografa.Descriptografar(Cookie.Value);
            var VerificaDados = new QueryMysql();

            var DadosTabelaFuncionario = VerificaDados.RecuperaDadosFuncionariosTabelaFuncionariosPerfil(Login);
            var DadosPendencias = Validacoes.RetornaPendenciasFuncionario(DadosTabelaFuncionario[0]["id"]);


            var JustificativasFirebird = QueryFire.RecuperaJustificativas();


            TempData["TotalJustificativas"] = JustificativasFirebird.Count;
            TempData["TotalPonto"] = DadosPendencias.Count;

            for (int j = 0; j < JustificativasFirebird.Count; j++)
            {
                TempData["Justificativa" + j] = JustificativasFirebird[j]["DESCRICAO"];
                TempData["IdJustificativa" + j] = JustificativasFirebird[j]["ID_JUSTIFICATIVA"];
            }


            TempData["Extra1"] = "hidden";
            TempData["Extra2"] = "hidden";
            for (int i = 0; i < DadosPendencias.Count; i++)
            {
                if (!Convert.ToBoolean(DadosPendencias[i]["ConfirmaGestor"]))
                {

                    TempData["IdPendencia" + i] = DadosPendencias[i]["IdPendencia"];
                    TempData["DiaPendencia" + i] = Convert.ToDateTime(DadosPendencias[i]["Data"]).ToString("dd/MM/yyyy");
                    TempData["NomePendencia" + i] = DadosPendencias[i]["Nome"];
                    TempData["IdFuncionario" + i] = DadosPendencias[0]["IdFuncionarioFireBird"];
                    TempData["TotalHorarioPendencia" + i] = DadosPendencias[i]["TotalHorario"];

                    if (4 - Convert.ToInt32(DadosPendencias[i]["TotalHorario"]) > 0)
                    {
                        TempData["TotalTextBox" + i] = 4 - Convert.ToInt32(DadosPendencias[i]["TotalHorario"]);
                    }
                    else
                    {
                        if (Convert.ToInt32(DadosPendencias[i]["TotalHorario"]) == 5)
                        {
                            TempData["Extra1"] = "";
                        }
                        else if (Convert.ToInt32(DadosPendencias[i]["TotalHorario"]) == 6)
                        {
                            TempData["Extra1"] = "";
                            TempData["Extra2"] = "";
                        }
                        TempData["TotalTextBox" + i] = 0;
                    }

                    for (int j = 0; j < Convert.ToInt32(DadosPendencias[i]["TotalHorario"]); j++)
                    {

                        TempData["Hora" + j + "Pendencia" + i] = DadosPendencias[i]["Horario" + j];


                    }


                    if (Convert.ToBoolean(DadosPendencias[i]["Justificado"]))
                    {
                        TempData["Esconde" + i] = "hidden";
                    }
                    else
                    {
                        TempData["Esconde" + i] = "";
                    }
                }
            }
            return PartialView("JustificativaPonto");
        }
        [HttpPost]
        public ActionResult Justificativas(JustificativaPonto Justificativa,FormCollection Formulario)
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {



                var Keys = Formulario.AllKeys;
                for (int i = 0; i < Formulario.Count; i++)
                {
                    if (Formulario.AllKeys[i].Contains("Id"))
                    {
                        var IdHistorico = Formulario[i];
                        var TemHorario = true;
                        
                        
                            if (Keys.Contains("Hora 0 " + IdHistorico))
                            {
                                VerificaDados.InseriJustificativa(IdHistorico,
                                    TimeSpan.Parse(Formulario["Hora 0 " + IdHistorico]),
                                    Formulario["Funcionario " + IdHistorico + ""],
                                    Formulario["JustificativaFire " + IdHistorico + ""]);
                                TemHorario = false;
                            }
                            if (Formulario.AllKeys.Contains("Hora 1 " + IdHistorico))
                            {
                                VerificaDados.InseriJustificativa(IdHistorico,
                                    TimeSpan.Parse(Formulario["Hora 1 " + IdHistorico]),
                                    Formulario["Funcionario " + IdHistorico + ""],
                                    Formulario["JustificativaFire " + IdHistorico + ""]);
                                TemHorario = false;
                            }
                            if (Keys.Contains("Hora 2 " + IdHistorico))
                            {
                                VerificaDados.InseriJustificativa(IdHistorico,
                                    TimeSpan.Parse(Formulario["Hora 2 " + IdHistorico]),
                                    Formulario["Funcionario " + IdHistorico + ""],
                                    Formulario["JustificativaFire " + IdHistorico + ""]);
                                TemHorario = false;
                            }
                            if (Formulario.AllKeys.Contains("Hora 3 " + IdHistorico))
                            {
                                VerificaDados.InseriJustificativa(IdHistorico,
                                    TimeSpan.Parse(Formulario["Hora 3 " + IdHistorico]),
                                    Formulario["Funcionario " + IdHistorico + ""],
                                    Formulario["JustificativaFire " + IdHistorico + ""]);
                                TemHorario = false;
                            }
                            if (TemHorario)
                            {
                                VerificaDados.AtualizaJustificativa(IdHistorico,
                                    Formulario["JustificativaFire " + IdHistorico + ""]);
                            }

                        }
                  

                }
                return RedirectToAction("Principal", "Principal",
                    new
                    {
                        Acao = "Dashboard",
                        Mensagem = "!",
                        Controlle = "Principal"
                    });

            }
            return RedirectToAction("Login", "Login");

        }

        public ActionResult JustificativasSetor()
        {
            var Validacoes = new ValidacoesPonto();
            var Cookie = Request.Cookies.Get("CookieFarm");
            var Login = Criptografa.Descriptografar(Cookie.Value);
            var VerificaDados = new QueryMysql();

            var DadosTabelaFuncionario = VerificaDados.RecuperaDadosFuncionariosTabelaFuncionariosPerfil(Login);
            var DadosPendencias = Validacoes.RetornaPendenciasSetor(DadosTabelaFuncionario[0]["idsetor"]);

            TempData["TotalPonto"] = DadosPendencias.Count;


            TempData["Extra1"] = "hidden";
            TempData["Extra2"] = "hidden";
            for (int i = 0; i < DadosPendencias.Count; i++)
            {
                if (!Convert.ToBoolean(DadosPendencias[i]["ConfirmaGestor"]))
                {

                    TempData["IdPendencia" + i] = DadosPendencias[i]["IdPendencia"];
                    TempData["DiaPendencia" + i] = Convert.ToDateTime(DadosPendencias[i]["Data"]).ToString("dd/MM/yyyy");
                    TempData["NomePendencia" + i] = DadosPendencias[i]["Nome"];
                    TempData["IdFuncionario" + i] = DadosPendencias[0]["IdFuncionarioFireBird"];
                    TempData["TotalHorarioPendencia" + i] = DadosPendencias[i]["TotalHorario"];

                    if (4 - Convert.ToInt32(DadosPendencias[i]["TotalHorario"]) > 0)
                    {
                        TempData["TotalTextBox" + i] = 4 - Convert.ToInt32(DadosPendencias[i]["TotalHorario"]);
                    }
                    else
                    {
                        if (Convert.ToInt32(DadosPendencias[i]["TotalHorario"]) == 5)
                        {
                            TempData["Extra1"] = "";
                        }
                        else if (Convert.ToInt32(DadosPendencias[i]["TotalHorario"]) == 6)
                        {
                            TempData["Extra1"] = "";
                            TempData["Extra2"] = "";
                        }
                        TempData["TotalTextBox" + i] = 0;
                    }

                    for (int j = 0; j < Convert.ToInt32(DadosPendencias[i]["TotalHorario"]); j++)
                    {

                        TempData["Hora" + j + "Pendencia" + i] = DadosPendencias[i]["Horario" + j];


                    }
                    if (Convert.ToBoolean(DadosPendencias[i]["Justificado"]))
                    {
                        TempData["StatusJustificativa" + i] = "green";
                        TempData["Justificativa" + i] = DadosPendencias[i]["Justificativa" + i];
                        TempData["Esconde" + i] = "";
                    }
                    else
                    {
                        TempData["Justificativa" + i] = "NÃO JUSTIFICADO";
                        TempData["StatusJustificativa" + i] = "red";
                        TempData["Esconde" + i] = "hidden";
                    }
                }
            }
            return PartialView("JustificativaSetor");
        }

        [HttpPost]
        public ActionResult JustificativasGestor(FormCollection Formulario)
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Keys = Formulario.AllKeys;
                for (int i = 0; i < Formulario.Count; i++)
                {
                    if (Formulario.AllKeys[i].Contains("Id"))
                    {
                        var IdHistorico = Formulario[i];
                        VerificaDados.AtualizaJustificativaGestor(IdHistorico);

                    }

                }

                return RedirectToAction("Principal", "Principal",
                    new { Acao = "Dashboard", Mensagem = "Justificativa confirmada com sucesso !", Controlle = "Principal" });
            }
            return RedirectToAction("Login", "Login");

        }

        public ActionResult NegarJustificativa(string IdHistorico)
        {
            var QueryRh = new QueryMysqlRh();
            QueryRh.NegaJustificativa(IdHistorico);
            return RedirectToAction("Principal", "Principal",
                new
                {
                    Acao = "Dashboard",
                    Mensagem = "Justificativa negada com sucesso!",
                    Controlle = "Principal"
                });
        }
    }
}