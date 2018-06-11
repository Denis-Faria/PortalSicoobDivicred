using System;
using System.Collections;
using System.Linq;
using System.Web.Mvc;
using PortalSicoobDivicred.Aplicacao;
using PortalSicoobDivicred.Models;

namespace PortalSicoobDivicred.Controllers
{
    public class PrincipalController : Controller
    {
        public ActionResult Principal()
        {


            var Alerta = new EnviodeAlertas();
 
            var VerificaDados = new QueryMysql();
            var Logado = VerificaDados.UsuarioLogado();
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
                    TempData["PermissaoCurriculo"] = "display: none";

                if (VerificaDados.PermissaoTesouraria(DadosUsuarioBanco[0]["login"]))
                    TempData["PermissaoTesouraria"] =
                        " ";
                else
                    TempData["PermissaoTesouraria"] = "display: none";

                if (DadosUsuarioBanco[0]["gestor"].Equals("S"))
                {
                    TempData["PermissaoGestor"] = "N";
                    TempData["AreaGestor"] = "S";
                }
                else
                {
                    TempData["PermissaoGestor"] = "N";
                    TempData["AreaGestor"] = "N";
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

        public ActionResult VagasInternas(string Participa)
        {
            var VerificaDados = new QueryMysql();
            var QueryRh = new QueryMysqlRh();

            #region VagasInternas

            {
                var VagasInternas = QueryRh.RetornaVagaInterna();

                TempData["TotalVagasInternas"] = VagasInternas.Count();

                for (var i = 0; i < VagasInternas.Count; i++)

                {
                    TempData["TituloVagaInterna " + i] = VagasInternas[i]["titulo"];
                    TempData["Descricao " + i] = VagasInternas[i]["descricao"];
                    TempData["Requisito " + i] = VagasInternas[i]["requisito"];
                    TempData["IdVaga " + i] = VagasInternas[i]["id"];
                }

                if (Participa != null)
                    if (Participa.Equals("SIM"))
                        TempData["Interesse " + 0] = "OK";
            }

            #endregion

            return View();
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
                    VerificaDados.RecuperaDadosFuncionariosTabelaFuncionariosLogin(Login);

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

                var TiposDependentes = "";

                for (int i = 0; i < 10; i++)
                {
                    try
                    {
                        if (Formulario["check" + i].Equals("true"))
                        {
                            TiposDependentes = TiposDependentes + i + ";";
                        }
                    }
                    catch
                    {

                    }
                }
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
                        var TipoConta = VerificaDados.RetornaTipoConta();

                        DadosFuncionario.Conta = TipoConta;
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
                    var TipoConta = VerificaDados.RetornaTipoConta();

                    DadosFuncionario.Conta = TipoConta;
                    DadosFuncionario.EstadoCivil = EstadoCivil;
                    DadosFuncionario.Sexo = Sexo;
                    DadosFuncionario.Etnia = Etnia;
                    DadosFuncionario.Formacao = Formacao;
                    DadosFuncionario.Setor = Setor;
                    DadosFuncionario.Funcao = Funcao;
                    ModelState.AddModelError("", "Favor confirmar que suas informações são verdadeiras");
                }

                var erro = new ArrayList();
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        erro.Add(error);
                    }
                }
                if (ModelState.IsValid)
                {
                    var Cookie = Request.Cookies.Get("CookieFarm");
                    var Login = Criptografa.Descriptografar(Cookie.Value);

                    var DadosTabelaUsuario = VerificaDados.RecuperaDadosFuncionariosTabelaFuncionariosLogin(Login);
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
                        Confirma, "S", DadosFuncionario.Nacionalidade, DadosFuncionario.NomeMae,
                        DadosFuncionario.NomePai, DadosFuncionario.LocalNascimento, DadosFuncionario.UfNascimento,
                        DadosFuncionario.Complemento, DadosFuncionario.Cep, DadosFuncionario.Pais,
                        DadosFuncionario.ResidenciaPropria, DadosFuncionario.RecursoFgts, DadosFuncionario.NumeroCTPS,
                        DadosFuncionario.SerieCTPS, DadosFuncionario.UfCTPS, DadosFuncionario.TelefoneFixo, DadosFuncionario.TelefoneCelular,
                        DadosFuncionario.EmailSecundario, DadosFuncionario.Cnh, DadosFuncionario.OrgaoEmissorCnh, DadosFuncionario.DataExpedicaoDocumentoCnh
                        , DadosFuncionario.DataValidadeCnh, DadosFuncionario.Oc, DadosFuncionario.OrgaoEmissorOc, DadosFuncionario.DataExpedicaoOc,
                        DadosFuncionario.DataValidadeOc, DadosFuncionario.DeficienteMotor, DadosFuncionario.DeficienteVisual, DadosFuncionario.DeficienteAuditivo,
                        DadosFuncionario.Reabilitado, DadosFuncionario.ObservacaoDeficiente, DadosFuncionario.IdTipoConta, DadosFuncionario.CodigoBanco,
                        DadosFuncionario.Agencia, DadosFuncionario.ContaCorrente, DadosFuncionario.DependenteIrrf, DadosFuncionario.DependenteFamilia,
                        DadosFuncionario.DadosDependentes, TiposDependentes, DadosFuncionario.Matricula, DadosFuncionario.AnoPrimeiroEmprego, DadosFuncionario.EmissaoCtps,
                        DadosFuncionario.PaisDivorciados, DadosFuncionario.OrgaoEmissorRg, DadosFuncionario.DataExpedicaoDocumentoRg, DadosFuncionario.CpfIrrf,
                        DadosFuncionario.NotificacaoEmail,DadosFuncionario.ContribuicaoSindical);

                    if (DadosFuncionario.MultiploNomeEmpresa != null )
                    {
                        VerificaDados.InserirVinculoEmpregaticio(DadosTabelaFuncionario[0]["id"],
                            DadosFuncionario.MultiploNomeEmpresa, DadosFuncionario.MultiploCnpj,
                            DadosFuncionario.MultiploRemuneracao, DadosFuncionario.MultiploComentario);
                    }

                    return RedirectToAction("Principal", "Principal");
                }
                else
                {
                    var EstadoCivil = VerificaDados.RetornaEstadoCivil();
                    var Sexo = VerificaDados.RetornaSexo();
                    var Etnia = VerificaDados.RetornaEtnia();
                    var Formacao = VerificaDados.RetornaFormacao();
                    var Setor = VerificaDados.RetornaSetor();
                    var Funcao = VerificaDados.RetornaFuncao();
                    var TipoConta = VerificaDados.RetornaTipoConta();

                    DadosFuncionario.Conta = TipoConta;
                    DadosFuncionario.EstadoCivil = EstadoCivil;
                    DadosFuncionario.Sexo = Sexo;
                    DadosFuncionario.Etnia = Etnia;
                    DadosFuncionario.Formacao = Formacao;
                    DadosFuncionario.Setor = Setor;
                    DadosFuncionario.Funcao = Funcao;

                    return View(DadosFuncionario);
                }
            }

            return RedirectToAction("Login", "Login");
        }
        public ActionResult RetornaOutroVinculo()
        {
            return PartialView("MultiplosVinculos");
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
                    try
                    {
                        var Certificacoes = VerificaDados.RetornaCertificacao(IdCertificacoes[j]);

                        TempData["Certificacao" + j] = Certificacoes[0]["descricao"];
                    }
                    catch
                    {
                        TempData["Certificacao" + j] = "NENHUMA CERTIFICAÇÂO OBRIGATÓRIA";
                    }
                }

                return PartialView("CertificacaoFuncao");
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult TenhoInteresse(FormCollection Dados)
        {
            var VerificaDados = new QueryMysql();

            var QueryRh = new QueryMysqlRh();

            var DadosTabelaFuncionario = VerificaDados.RecuperaDadosFuncionariosTabelaFuncionarios(Dados["nome"]);
            var Interesse = QueryRh.RetornaInteresseVagaInterna(DadosTabelaFuncionario[0]["id"]);
            var valor = true;
            for (var j = 0; j < Interesse.Count; j++)
                if (Interesse[j]["idvaga"].Equals(Dados["idvaga"]))
                    valor = false;
            if (valor)
            {
                QueryRh.CadastraInteresse(Dados["idvaga"], DadosTabelaFuncionario[0]["id"]);
                return RedirectToAction("VagasInternas", "Principal", new { Participa = "SIM" });
            }

            return RedirectToAction("VagasInternas", "Principal", new { Participa = "SIM" });
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

            for (var j = 0; j < JustificativasFirebird.Count; j++)
            {
                TempData["Justificativa" + j] = JustificativasFirebird[j]["DESCRICAO"];
                TempData["IdJustificativa" + j] = JustificativasFirebird[j]["ID_JUSTIFICATIVA"];
            }


            TempData["Extra1"] = "hidden";
            TempData["Extra2"] = "hidden";
            for (var i = 0; i < DadosPendencias.Count; i++)
                if (!Convert.ToBoolean(DadosPendencias[i]["ConfirmaGestor"]))
                {
                    TempData["IdPendencia" + i] = DadosPendencias[i]["IdPendencia"];
                    TempData["DiaPendencia" + i] =
                        Convert.ToDateTime(DadosPendencias[i]["Data"]).ToString("dd/MM/yyyy");
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

                    for (var j = 0; j < Convert.ToInt32(DadosPendencias[i]["TotalHorario"]); j++)
                        TempData["Hora" + j + "Pendencia" + i] = DadosPendencias[i]["Horario" + j];


                    if (Convert.ToBoolean(DadosPendencias[i]["Justificado"]))
                        TempData["Esconde" + i] = "hidden";
                    else
                        TempData["Esconde" + i] = "";
                }

            return PartialView("JustificativaPonto");
        }

        [HttpPost]
        public ActionResult Justificativas(JustificativaPonto Justificativa, FormCollection Formulario)
        {
            var VerificaDados = new QueryMysqlRh();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Keys = Formulario.AllKeys;

                for (var i = 0; i < Formulario.Count; i++)
                    if (Formulario.AllKeys[i].Contains("Id"))
                    {
                        var IdHistorico = Formulario[i];
                        var TemHorario = true;
                        if (Formulario.AllKeys.Contains("Observacao"))
                        {
                            VerificaDados.AtualizaJustificativaSemFireBird(IdHistorico,
                                Formulario["Observacao"]);
                        }
                        else
                        {
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
                                VerificaDados.AtualizaJustificativa(IdHistorico,
                                    Formulario["JustificativaFire " + IdHistorico + ""]);
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
            for (var i = 0; i < DadosPendencias.Count; i++)
                if (!Convert.ToBoolean(DadosPendencias[i]["ConfirmaGestor"]))
                {
                    TempData["IdPendencia" + i] = DadosPendencias[i]["IdPendencia"];
                    TempData["DiaPendencia" + i] =
                        Convert.ToDateTime(DadosPendencias[i]["Data"]).ToString("dd/MM/yyyy");
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

                    for (var j = 0; j < Convert.ToInt32(DadosPendencias[i]["TotalHorario"]); j++)
                        TempData["Hora" + j + "Pendencia" + i] = DadosPendencias[i]["Horario" + j];
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
                for (var i = 0; i < Formulario.Count; i++)
                    if (Formulario.AllKeys[i].Contains("Id"))
                    {
                        var IdHistorico = Formulario[i];
                        VerificaDados.AtualizaJustificativaGestor(IdHistorico);
                    }

                return RedirectToAction("Principal", "Principal",
                    new
                    {
                        Acao = "Dashboard",
                        Mensagem = "Justificativa confirmada com sucesso !",
                        Controlle = "Principal"
                    });
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

        public ActionResult BancoHora()
        {
            var VerificaDados = new QueryMysql();
            var VerificaDadosFire = new QueryFirebird();
            var Cookie = Request.Cookies.Get("CookieFarm");
            var Login = Criptografa.Descriptografar(Cookie.Value);

            var DadosFuncionario = VerificaDados.RecuperaDadosUsuarios(Login);

            if (DadosFuncionario[0]["gestor"].Equals("N"))
            {
                TempData["Gestor"] = "N";
                var result = VerificaDados.RemoveAccents(DadosFuncionario[0]["nome"]);
                var Cracha = VerificaDadosFire.RetornaCrachaFuncionario(result);
                var Horas = VerificaDados.RetornaHoras(Cracha[0]["MATRICULA"]);
                var HoraTotal = TimeSpan.Parse(Horas[0]["hora"]);
                if (HoraTotal.Hours < 0)
                {
                    TempData["NomeFuncionario"] = DadosFuncionario[0]["nome"];
                    TempData["SaldoHoras"] = Horas[0]["hora"];
                    TempData["DataReferencia"] = Convert.ToDateTime(Horas[0]["datareferencia"]).ToString("dd/MM/yyyy");
                    TempData["cor"] = "red";
                }
                else
                {
                    TempData["NomeFuncionario"] = DadosFuncionario[0]["nome"];
                    TempData["SaldoHoras"] = Horas[0]["hora"];
                    TempData["DataReferencia"] = Convert.ToDateTime(Horas[0]["datareferencia"]).ToString("dd/MM/yyyy");
                    TempData["cor"] = "black";
                }
            }
            else
            {
                TempData["Gestor"] = "S";
                var TodosFuncionariosSetor = VerificaDados.RetornaFuncionariosSetor(DadosFuncionario[0]["idsetor"]);
                TempData["TotalEquipe"] = TodosFuncionariosSetor.Count;
                for (var i = 0; i < TodosFuncionariosSetor.Count; i++)
                {
                    var result = VerificaDados.RemoveAccents(TodosFuncionariosSetor[i]["nome"]);
                    var Cracha = VerificaDadosFire.RetornaCrachaFuncionario(result);
                    var Horas = VerificaDados.RetornaHoras(Cracha[0]["MATRICULA"]);
                    var HoraTotal = TimeSpan.Parse(Horas[0]["hora"]);
                    if (HoraTotal.Hours < 0 || HoraTotal.Minutes < 0)
                    {
                        TempData["NomeFuncionarioEquipe" + i] = TodosFuncionariosSetor[i]["nome"];
                        TempData["SaldoHorasEquipe" + i] = Horas[0]["hora"];
                        TempData["DataReferencia" + i] =
                            Convert.ToDateTime(Horas[0]["datareferencia"]).ToString("dd/MM/yyyy");
                        TempData["CorEquipe" + i] = "red";
                    }
                    else
                    {
                        TempData["NomeFuncionarioEquipe" + i] = TodosFuncionariosSetor[i]["nome"];
                        TempData["SaldoHorasEquipe" + i] = Horas[0]["hora"];
                        TempData["DataReferencia" + i] =
                            Convert.ToDateTime(Horas[0]["datareferencia"]).ToString("dd/MM/yyyy");
                        TempData["CorEquipe" + i] = "black";
                    }
                }
            }

            return PartialView("BancoHoras");
        }

        public ActionResult ShowdePremios(string Cpf)
        {
            var CpfLimpo = Cpf;
            CpfLimpo = CpfLimpo.Replace(".", "").Replace("-", "").Replace("/", "");
            var RecuperaDados = new QueryMysql();

            var NumeroDaSorte = RecuperaDados.BuscaNumerodaSorte();
            if (NumeroDaSorte.Count > 0)
                TempData["NumeroDaSorte"] = "O último número da sorte foi: " + NumeroDaSorte[0]["numerodasorte"] +
                                            " na data: " + NumeroDaSorte[0]["data"];
            else
                TempData["NumeroDaSorte"] = "Nenhum sorteio foi realizado até o momento.";


            var Extrato = RecuperaDados.RetornaExtrato(CpfLimpo);
            TempData["TotalCupons"] = Extrato.Count;
            for (var i = 0; i < Extrato.Count; i++)
            {
                TempData["Numero" + i] = Extrato[i]["id"];
                TempData["Motivo" + i] = Extrato[i]["motivo"];
                TempData["Data" + i] = Extrato[i]["data"];
            }

            return PartialView("ShowdePremios");
        }
        public void CadastroIdNotificacao(string IdNotificacao)
        {
            var Cookie = Request.Cookies.Get("CookieFarm");
            var Login = Criptografa.Descriptografar(Cookie.Value);
            var Atualiza = new QueryMysql();
            Atualiza.CadastraIdNotificacao(IdNotificacao,Login);
        }
        public ActionResult ControleNumerario()
        {
            return PartialView("ControleNumerario");
        }
    }
}