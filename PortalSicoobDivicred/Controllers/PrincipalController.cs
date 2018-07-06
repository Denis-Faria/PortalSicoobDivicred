using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using PortalSicoobDivicred.Aplicacao;
using PortalSicoobDivicred.Models;

namespace PortalSicoobDivicred.Controllers
{
    public class PrincipalController : Controller
    {
        private SentryTracking track = new SentryTracking();
        public ActionResult Principal(string mensagem)
        {
            var verificaDados = new QueryMysql();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                TempData["Mensagem"] = mensagem;
                var cookie = Request.Cookies.Get( "CookieFarm" );

                if(cookie != null)
                {
                    var login = Criptografa.Descriptografar( cookie.Value );

                    if(verificaDados.PrimeiroLogin( login ))
                        return RedirectToAction( "FormularioCadastro", "Principal" );
                    var dadosUsuarioBanco = verificaDados.RecuperaDadosUsuarios( login );

                    TempData["ValidaBanco"] = dadosUsuarioBanco[0]["validabanco"];

                    if(verificaDados.PermissaoCurriculos( dadosUsuarioBanco[0]["login"] ))
                        TempData["PermissaoCurriculo"] =
                            " ";
                    else
                        TempData["PermissaoCurriculo"] = "display: none";

                    if(verificaDados.PermissaoTesouraria( dadosUsuarioBanco[0]["login"] ))
                        TempData["PermissaoTesouraria"] =
                            " ";
                    else
                        TempData["PermissaoTesouraria"] = "display: none";

                    if(dadosUsuarioBanco[0]["gestor"].Equals( "S" ))
                    {
                        TempData["PermissaoGestor"] = "N";
                        TempData["AreaGestor"] = "S";
                    }
                    else
                    {
                        TempData["PermissaoGestor"] = "N";
                        TempData["AreaGestor"] = "N";
                    }

                    if(verificaDados.PermissaoControleFuncionario( dadosUsuarioBanco[0]["login"] ))
                        TempData["PermissaoNumerario"] =
                            " ";
                    else if(verificaDados.PermissaoControleTesouraria( dadosUsuarioBanco[0]["login"] ))
                        TempData["PermissaoNumerario"] =
                            " ";
                    else
                        TempData["PermissaoNumerario"] = "display: none";

                    TempData["NomeLateral"] = dadosUsuarioBanco[0]["login"];
                    TempData["EmailLateral"] = dadosUsuarioBanco[0]["email"];
                    if(dadosUsuarioBanco[0]["foto"] == null)
                        TempData["ImagemPerfil"] = "https://docs.google.com/uc?id=0B2CLuTO3N2_obWdkajEzTmpGeU0";
                    else
                        TempData["ImagemPerfil"] = dadosUsuarioBanco[0]["foto"];
                }

                return View();
            }

            return RedirectToAction( "Login", "Login" );
        }

        public ActionResult VagasInternas(string participa)
        {
            var queryRh = new QueryMysqlRh();

            #region VagasInternas

            {
                var vagasInternas = queryRh.RetornaVagaInterna();

                TempData["TotalVagasInternas"] = vagasInternas.Count();

                for(var i = 0; i < vagasInternas.Count; i++)

                {
                    TempData["TituloVagaInterna " + i] = vagasInternas[i]["titulo"];
                    TempData["Descricao " + i] = vagasInternas[i]["descricao"];
                    TempData["Requisito " + i] = vagasInternas[i]["requisito"];
                    TempData["IdVaga " + i] = vagasInternas[i]["id"];
                }

                if(participa != null)
                    if(participa.Equals( "SIM" ))
                        TempData["Interesse " + 0] = "OK";
            }

            #endregion

            return View();
        }

        public ActionResult Dashboard()
        {
            var verificaDados = new QueryMysql();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                var queryRh = new QueryMysqlRh();
                var cookie = Request.Cookies.Get( "CookieFarm" );
                if(cookie != null)
                {
                    var login = Criptografa.Descriptografar( cookie.Value );
                    var dadosTabelaFuncionario = verificaDados.RecuperaDadosFuncionariosTabelaFuncionariosPerfil( login );


                    #region VagasInternas

                    {
                        var vagasInternas = queryRh.RetornaVagaInterna();

                        TempData["TotalVagasInternas"] = vagasInternas.Count();
                        var interesse = queryRh.RetornaInteresseVagaInterna( dadosTabelaFuncionario[0]["id"] );
                        for(var i = 0; i < vagasInternas.Count; i++)

                        {
                            TempData["TituloVagaInterna " + i] = vagasInternas[i]["titulo"];
                            TempData["Descricao " + i] = vagasInternas[i]["descricao"];
                            TempData["Requisito " + i] = vagasInternas[i]["requisito"];
                            TempData["IdVaga " + i] = vagasInternas[i]["id"];


                            for(var j = 0; j < interesse.Count; j++)
                                if(interesse[j]["idvaga"].Equals( vagasInternas[i]["id"] ))
                                    TempData["Interesse " + i] = "Ok";
                        }

                        TempData["TotalProcessos"] = interesse.Count;
                        for(var j = 0; j < interesse.Count; j++)
                        {
                            TempData["Aprovado " + j] = interesse[j]["aprovado"];
                            TempData["TituloVagaInternaProcesso " + j] = interesse[j]["titulo"];
                        }
                    }
                }

                #endregion


                return PartialView( "Dashboard" );
            }

            return RedirectToAction( "Login", "Login" );
        }

        public ActionResult FormularioCadastro()
        {
            var verificaDados = new QueryMysql();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                try
                {
                    var cookie = Request.Cookies.Get("CookieFarm");
                    if (cookie != null)
                    {
                        var login = Criptografa.Descriptografar(cookie.Value);

                        var dadosTabelaFuncionario =
                            verificaDados.RecuperaDadosFuncionariosTabelaFuncionariosLogin(login);

                        var dadosFuncionario = new Funcionario();
                        dadosFuncionario.NomeFuncionario = dadosTabelaFuncionario[0]["nome"];
                        dadosFuncionario.CpfFuncionario = dadosTabelaFuncionario[0]["cpf"];
                        dadosFuncionario.RgFuncionario = dadosTabelaFuncionario[0]["rg"];
                        dadosFuncionario.PisFuncionario = dadosTabelaFuncionario[0]["pis"];
                        dadosFuncionario.DataNascimentoFuncionario =
                            Convert.ToDateTime(dadosTabelaFuncionario[0]["datanascimento"]).ToString("dd/MM/yyyy");
                        dadosFuncionario.FormacaoAcademica = dadosTabelaFuncionario[0]["formacaoacademica"];
                        dadosFuncionario.UsuarioSistema = dadosTabelaFuncionario[0]["login"];
                        dadosFuncionario.Email = dadosTabelaFuncionario[0]["email"];
                        dadosFuncionario.Pa = dadosTabelaFuncionario[0]["idpa"];
                        dadosFuncionario.Rua = dadosTabelaFuncionario[0]["rua"];
                        dadosFuncionario.Numero = dadosTabelaFuncionario[0]["numero"];
                        dadosFuncionario.Bairro = dadosTabelaFuncionario[0]["bairro"];
                        dadosFuncionario.Cidade = dadosTabelaFuncionario[0]["cidade"];
                        dadosFuncionario.QuatidadeFilho = dadosTabelaFuncionario[0]["quantidadefilho"];
                        dadosFuncionario.DataNascimentoFilho = dadosTabelaFuncionario[0]["datanascimentofilho"];
                        dadosFuncionario.ContatoEmergencia = dadosTabelaFuncionario[0]["contatoemergencia"];
                        dadosFuncionario.PrincipaisHobbies = dadosTabelaFuncionario[0]["principalhobbie"];
                        dadosFuncionario.ComidaFavorita = dadosTabelaFuncionario[0]["comidafavorita"];
                        dadosFuncionario.Viagem = dadosTabelaFuncionario[0]["viagem"];


                        var estadoCivil = verificaDados.RetornaEstadoCivil();
                        var tipoConta = verificaDados.RetornaTipoConta();
                        var sexo = verificaDados.RetornaSexo();
                        var etnia = verificaDados.RetornaEtnia();
                        var formacao = verificaDados.RetornaFormacao();
                        var setor = verificaDados.RetornaSetor();
                        var funcao = verificaDados.RetornaFuncao();
                        var estadosCivisPais = verificaDados.RetornaEstadoCivilPais();
                        var horariosTrabalhos = verificaDados.RetornaHorarioTrabalho();

                        dadosFuncionario.EstadoCivil = estadoCivil;
                        dadosFuncionario.Sexo = sexo;
                        dadosFuncionario.Etnia = etnia;
                        dadosFuncionario.Formacao = formacao;
                        dadosFuncionario.Setor = setor;
                        dadosFuncionario.Funcao = funcao;
                        dadosFuncionario.Conta = tipoConta;
                        dadosFuncionario.PaisDivorciados = estadosCivisPais;
                        dadosFuncionario.HorarioTrabalho = horariosTrabalhos;

                        if (dadosTabelaFuncionario[0]["estagiario"].Equals("S"))
                        {
                            TempData["Estagiario"] = "SIM";
                            TempData["DataEstagio"] = dadosTabelaFuncionario[0]["contratoestagio"];
                        }
                        else
                        {
                            TempData["Estagiario"] = "NÃO";
                            TempData["DataEstagio"] = "-";
                        }

                        return View(dadosFuncionario);
                    }
                }
                catch(Exception e)
                {
                    track.GeraLog( e );
                    return View();
                }
            }

            return RedirectToAction( "Login", "Login" );
        }

        [HttpPost]
        public ActionResult FormularioCadastro(Funcionario dadosFuncionario, FormCollection formulario)
        {
            var verificaDados = new QueryMysql();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                try
                {
                    var tiposDependentes = "";

                    for (int i = 0; i < 10; i++)
                    {
                        try
                        {
                            if (formulario["check" + i].Equals("true"))
                            {
                                tiposDependentes = tiposDependentes + i + ";";
                            }
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    try
                    {
                        if (!formulario["ConfirmacaoDados"].Equals("on"))
                        {

                            var estadoCivil = verificaDados.RetornaEstadoCivil();
                            var tipoConta = verificaDados.RetornaTipoConta();
                            var sexo = verificaDados.RetornaSexo();
                            var etnia = verificaDados.RetornaEtnia();
                            var formacao = verificaDados.RetornaFormacao();
                            var setor = verificaDados.RetornaSetor();
                            var funcao = verificaDados.RetornaFuncao();
                            var estadosCivisPais = verificaDados.RetornaEstadoCivilPais();
                            var horariosTrabalhos = verificaDados.RetornaHorarioTrabalho();

                            dadosFuncionario.EstadoCivil = estadoCivil;
                            dadosFuncionario.Sexo = sexo;
                            dadosFuncionario.Etnia = etnia;
                            dadosFuncionario.Formacao = formacao;
                            dadosFuncionario.Setor = setor;
                            dadosFuncionario.Funcao = funcao;
                            dadosFuncionario.Conta = tipoConta;
                            dadosFuncionario.PaisDivorciados = estadosCivisPais;
                            dadosFuncionario.HorarioTrabalho = horariosTrabalhos;
                            ModelState.AddModelError("", "Favor confirmar que suas informações são verdadeiras");
                        }
                    }
                    catch
                    {

                        var estadoCivil = verificaDados.RetornaEstadoCivil();
                        var tipoConta = verificaDados.RetornaTipoConta();
                        var sexo = verificaDados.RetornaSexo();
                        var etnia = verificaDados.RetornaEtnia();
                        var formacao = verificaDados.RetornaFormacao();
                        var setor = verificaDados.RetornaSetor();
                        var funcao = verificaDados.RetornaFuncao();
                        var estadosCivisPais = verificaDados.RetornaEstadoCivilPais();
                        var horariosTrabalhos = verificaDados.RetornaHorarioTrabalho();

                        dadosFuncionario.EstadoCivil = estadoCivil;
                        dadosFuncionario.Sexo = sexo;
                        dadosFuncionario.Etnia = etnia;
                        dadosFuncionario.Formacao = formacao;
                        dadosFuncionario.Setor = setor;
                        dadosFuncionario.Funcao = funcao;
                        dadosFuncionario.Conta = tipoConta;
                        dadosFuncionario.PaisDivorciados = estadosCivisPais;
                        dadosFuncionario.HorarioTrabalho = horariosTrabalhos;
                        ModelState.AddModelError("", "Favor confirmar que suas informações são verdadeiras");
                    }

                    if (ModelState.IsValid)
                    {
                        var cookie = Request.Cookies.Get("CookieFarm");
                        if (cookie != null)
                        {
                            var login = Criptografa.Descriptografar(cookie.Value);

                            var dadosTabelaUsuario =
                                verificaDados.RecuperaDadosFuncionariosTabelaFuncionariosLogin(login);
                            var dadosTabelaFuncionario =
                                verificaDados.RecuperaDadosFuncionariosTabelaFuncionarios(
                                    dadosTabelaUsuario[0]["nome"]);

                            string descricaoSexo;
                            string dataNascimentoFilho;
                            if (dadosFuncionario.DescricaoSexo == null)
                                descricaoSexo = "NÃO INFORMOU";
                            else
                                descricaoSexo = dadosFuncionario.DescricaoSexo;
                            if (dadosFuncionario.DataNascimentoFilho == null)
                                dataNascimentoFilho = "NÂO TEM";
                            else
                                dataNascimentoFilho = dadosFuncionario.DataNascimentoFilho;
                            string confirma;
                            try
                            {
                                if (formulario["ConfirmacaoCertificacao"].Equals("on"))
                                    confirma = "S";
                                else
                                    confirma = "N";
                            }
                            catch
                            {
                                confirma = "N";
                            }

                            for (var i = 0; i < formulario.Count; i++)
                            {
                                if (formulario.AllKeys[i].Contains("formacao"))
                                {
                                    var tipo = "Tipo " + formulario.GetKey(i).Split(' ')[1];
                                    verificaDados.InserirFormacao(formulario[i], dadosTabelaFuncionario[0]["id"],
                                        formulario[tipo]);
                                }
                            }


                            verificaDados.AtualizaDadosFuncionarioFormulario(dadosFuncionario.NomeFuncionario,
                                dadosFuncionario.CpfFuncionario, dadosFuncionario.RgFuncionario,
                                dadosFuncionario.PisFuncionario, dadosFuncionario.DataNascimentoFuncionario,
                                dadosFuncionario.IdSexo.ToString(), descricaoSexo, dadosFuncionario.IdEtnia.ToString(),
                                dadosFuncionario.IdEstadoCivil.ToString(), dadosFuncionario.IdFormacao.ToString(),
                                dadosFuncionario.FormacaoAcademica,
                                dadosFuncionario.UsuarioSistema, dadosFuncionario.Email, dadosFuncionario.Pa,
                                dadosFuncionario.Rua, dadosFuncionario.Numero, dadosFuncionario.Bairro,
                                dadosFuncionario.Cidade,
                                dadosFuncionario.IdSetor.ToString(), dadosFuncionario.IdFuncao.ToString(),
                                dadosFuncionario.QuatidadeFilho,
                                dataNascimentoFilho, dadosFuncionario.ContatoEmergencia,
                                dadosFuncionario.PrincipaisHobbies, dadosFuncionario.ComidaFavorita,
                                dadosFuncionario.Viagem,
                                confirma, "S", dadosFuncionario.Nacionalidade, dadosFuncionario.NomeMae,
                                dadosFuncionario.NomePai, dadosFuncionario.LocalNascimento,
                                dadosFuncionario.UfNascimento,
                                dadosFuncionario.Complemento, dadosFuncionario.Cep, dadosFuncionario.Pais,
                                dadosFuncionario.ResidenciaPropria, dadosFuncionario.RecursoFgts,
                                dadosFuncionario.NumeroCtps,
                                dadosFuncionario.SerieCtps, dadosFuncionario.UfCtps, dadosFuncionario.TelefoneFixo,
                                dadosFuncionario.TelefoneCelular,
                                dadosFuncionario.EmailSecundario, dadosFuncionario.Cnh,
                                dadosFuncionario.OrgaoEmissorCnh, dadosFuncionario.DataExpedicaoDocumentoCnh
                                , dadosFuncionario.DataValidadeCnh, dadosFuncionario.Oc,
                                dadosFuncionario.OrgaoEmissorOc, dadosFuncionario.DataExpedicaoOc,
                                dadosFuncionario.DataValidadeOc, dadosFuncionario.DeficienteMotor,
                                dadosFuncionario.DeficienteVisual, dadosFuncionario.DeficienteAuditivo,
                                dadosFuncionario.Reabilitado, dadosFuncionario.ObservacaoDeficiente,
                                dadosFuncionario.IdTipoConta, dadosFuncionario.CodigoBanco,
                                dadosFuncionario.Agencia, dadosFuncionario.ContaCorrente,
                                dadosFuncionario.DependenteIrrf, dadosFuncionario.DependenteFamilia,
                                dadosFuncionario.DadosDependentes, tiposDependentes, dadosFuncionario.Matricula,
                                dadosFuncionario.AnoPrimeiroEmprego, dadosFuncionario.EmissaoCtps,
                                dadosFuncionario.IdEstadoCivilPais.ToString(), dadosFuncionario.OrgaoEmissorRg,
                                dadosFuncionario.DataExpedicaoDocumentoRg, dadosFuncionario.CpfIrrf,
                                dadosFuncionario.NotificacaoEmail, dadosFuncionario.ContribuicaoSindical);

                            if (dadosFuncionario.MultiploNomeEmpresa != null)
                            {
                                verificaDados.InserirVinculoEmpregaticio(dadosTabelaFuncionario[0]["id"],
                                    dadosFuncionario.MultiploNomeEmpresa, dadosFuncionario.MultiploCnpj,
                                    dadosFuncionario.MultiploRemuneracao, dadosFuncionario.MultiploComentario);
                            }
                        }

                        return RedirectToAction("Principal", "Principal");
                    }
                    else
                    {

                        var estadoCivil = verificaDados.RetornaEstadoCivil();
                        var tipoConta = verificaDados.RetornaTipoConta();
                        var sexo = verificaDados.RetornaSexo();
                        var etnia = verificaDados.RetornaEtnia();
                        var formacao = verificaDados.RetornaFormacao();
                        var setor = verificaDados.RetornaSetor();
                        var funcao = verificaDados.RetornaFuncao();
                        var estadosCivisPais = verificaDados.RetornaEstadoCivilPais();
                        var horariosTrabalhos = verificaDados.RetornaHorarioTrabalho();

                        dadosFuncionario.EstadoCivil = estadoCivil;
                        dadosFuncionario.Sexo = sexo;
                        dadosFuncionario.Etnia = etnia;
                        dadosFuncionario.Formacao = formacao;
                        dadosFuncionario.Setor = setor;
                        dadosFuncionario.Funcao = funcao;
                        dadosFuncionario.Conta = tipoConta;
                        dadosFuncionario.PaisDivorciados = estadosCivisPais;
                        dadosFuncionario.HorarioTrabalho = horariosTrabalhos;

                        return View(dadosFuncionario);
                    }
                }
                catch(Exception e)
                {
                    track.GeraLog( e );
                    return View(dadosFuncionario);

                }
            }

            return RedirectToAction( "Login", "Login" );
        }
        public ActionResult RetornaOutroVinculo()
        {
            return PartialView( "MultiplosVinculos" );
        }

        public ActionResult BuscaCertificacao(string idFuncao)
        {
            var verificaDados = new QueryMysql();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                var certificacoesFuncao = verificaDados.RetornaCertificacaoFuncao( idFuncao );
                var idCertificacoes = certificacoesFuncao[0]["idcertificacao"].Split( ';' );

                TempData["TotalCertificacao"] = idCertificacoes.Length;

                for(var j = 0; j < idCertificacoes.Length; j++)
                {
                    try
                    {
                        var certificacoes = verificaDados.RetornaCertificacao( idCertificacoes[j] );

                        TempData["Certificacao" + j] = certificacoes[0]["descricao"];
                    }
                    catch
                    {
                        TempData["Certificacao" + j] = "NENHUMA CERTIFICAÇÂO OBRIGATÓRIA";
                    }
                }

                return PartialView( "CertificacaoFuncao" );
            }

            return RedirectToAction( "Login", "Login" );
        }

        [HttpPost]
        public ActionResult TenhoInteresse(FormCollection dados)
        {
            var verificaDados = new QueryMysql();

            var queryRh = new QueryMysqlRh();

            var dadosTabelaFuncionario = verificaDados.RecuperaDadosFuncionariosTabelaFuncionarios( dados["nome"] );
            var interesse = queryRh.RetornaInteresseVagaInterna( dadosTabelaFuncionario[0]["id"] );
            var valor = true;
            for(var j = 0; j < interesse.Count; j++)
                if(interesse[j]["idvaga"].Equals( dados["idvaga"] ))
                    valor = false;
            if(valor)
            {
                queryRh.CadastraInteresse( dados["idvaga"], dadosTabelaFuncionario[0]["id"] );
                return RedirectToAction( "VagasInternas", "Principal", new { Participa = "SIM" } );
            }

            return RedirectToAction( "VagasInternas", "Principal", new { Participa = "SIM" } );
        }

        public ActionResult Justificativas()
        {
            var validacoes = new ValidacoesPonto();
            var queryFire = new QueryFirebird();
            var cookie = Request.Cookies.Get( "CookieFarm" );
            if(cookie != null)
            {
                var login = Criptografa.Descriptografar( cookie.Value );
                var verificaDados = new QueryMysql();

                var dadosTabelaFuncionario = verificaDados.RecuperaDadosFuncionariosTabelaFuncionariosPerfil( login );
                var dadosPendencias = validacoes.RetornaPendenciasFuncionario( dadosTabelaFuncionario[0]["id"] );


                var justificativasFirebird = queryFire.RecuperaJustificativas();


                TempData["TotalJustificativas"] = justificativasFirebird.Count;
                TempData["TotalPonto"] = dadosPendencias.Count;

                for(var j = 0; j < justificativasFirebird.Count; j++)
                {
                    TempData["Justificativa" + j] = justificativasFirebird[j]["DESCRICAO"];
                    TempData["IdJustificativa" + j] = justificativasFirebird[j]["ID_JUSTIFICATIVA"];
                }


                TempData["Extra1"] = "hidden";
                TempData["Extra2"] = "hidden";
                for(var i = 0; i < dadosPendencias.Count; i++)
                    if(!Convert.ToBoolean( dadosPendencias[i]["ConfirmaGestor"] ))
                    {
                        TempData["IdPendencia" + i] = dadosPendencias[i]["IdPendencia"];
                        TempData["DiaPendencia" + i] =
                            Convert.ToDateTime( dadosPendencias[i]["Data"] ).ToString( "dd/MM/yyyy" );
                        TempData["NomePendencia" + i] = dadosPendencias[i]["Nome"];
                        TempData["IdFuncionario" + i] = dadosPendencias[0]["IdFuncionarioFireBird"];
                        TempData["TotalHorarioPendencia" + i] = dadosPendencias[i]["TotalHorario"];

                        if(4 - Convert.ToInt32( dadosPendencias[i]["TotalHorario"] ) > 0)
                        {
                            TempData["TotalTextBox" + i] = 4 - Convert.ToInt32( dadosPendencias[i]["TotalHorario"] );
                        }
                        else
                        {
                            if(Convert.ToInt32( dadosPendencias[i]["TotalHorario"] ) == 5)
                            {
                                TempData["Extra1"] = "";
                            }
                            else if(Convert.ToInt32( dadosPendencias[i]["TotalHorario"] ) == 6)
                            {
                                TempData["Extra1"] = "";
                                TempData["Extra2"] = "";
                            }

                            TempData["TotalTextBox" + i] = 0;
                        }

                        for(var j = 0; j < Convert.ToInt32( dadosPendencias[i]["TotalHorario"] ); j++)
                            TempData["Hora" + j + "Pendencia" + i] = dadosPendencias[i]["Horario" + j];


                        if(Convert.ToBoolean( dadosPendencias[i]["Justificado"] ))
                            TempData["Esconde" + i] = "hidden";
                        else
                            TempData["Esconde" + i] = "";

                        if(dadosTabelaFuncionario[0]["estagiario"].Equals( "S" ))
                        {
                            TempData["MostraCampoLivre"] = true;
                        }
                        else
                        {
                            TempData["MostraCampoLivre"] = false;
                        }
                    }
            }


            return PartialView( "JustificativaPonto" );
        }

        [HttpPost]
        public ActionResult Justificativas(JustificativaPonto justificativa, FormCollection formulario)
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                var keys = formulario.AllKeys;

                for(var i = 0; i < formulario.Count; i++)
                    if(formulario.AllKeys[i].Contains( "Id" ))
                    {
                        var idHistorico = formulario[i];
                        var temHorario = true;
                        if(formulario.AllKeys.Contains( "Observacao" ))
                        {
                            verificaDados.AtualizaJustificativaSemFireBird( idHistorico,
                                formulario["Observacao"] );
                        }
                        else
                        {
                            if(keys.Contains( "Hora 0 " + idHistorico ))
                            {
                                verificaDados.InseriJustificativa( idHistorico,
                                    TimeSpan.Parse( formulario["Hora 0 " + idHistorico] ),
                                    formulario["Funcionario " + idHistorico + ""],
                                    formulario["JustificativaFire " + idHistorico + ""] );
                                temHorario = false;
                            }

                            if(formulario.AllKeys.Contains( "Hora 1 " + idHistorico ))
                            {
                                verificaDados.InseriJustificativa( idHistorico,
                                    TimeSpan.Parse( formulario["Hora 1 " + idHistorico] ),
                                    formulario["Funcionario " + idHistorico + ""],
                                    formulario["JustificativaFire " + idHistorico + ""] );
                                temHorario = false;
                            }

                            if(keys.Contains( "Hora 2 " + idHistorico ))
                            {
                                verificaDados.InseriJustificativa( idHistorico,
                                    TimeSpan.Parse( formulario["Hora 2 " + idHistorico] ),
                                    formulario["Funcionario " + idHistorico + ""],
                                    formulario["JustificativaFire " + idHistorico + ""] );
                                temHorario = false;
                            }

                            if(formulario.AllKeys.Contains( "Hora 3 " + idHistorico ))
                            {
                                verificaDados.InseriJustificativa( idHistorico,
                                    TimeSpan.Parse( formulario["Hora 3 " + idHistorico] ),
                                    formulario["Funcionario " + idHistorico + ""],
                                    formulario["JustificativaFire " + idHistorico + ""] );
                                temHorario = false;
                            }

                            if(temHorario)
                                verificaDados.AtualizaJustificativa( idHistorico,
                                    formulario["JustificativaFire " + idHistorico + ""] );
                        }
                    }

                return RedirectToAction( "Principal", "Principal",
                    new
                    {
                        Acao = "Dashboard",
                        Mensagem = "Pendência justificada com sucesso!",
                        Controlle = "Principal"
                    } );
            }

            return RedirectToAction( "Login", "Login" );
        }

        public ActionResult JustificativasSetor()
        {
            var validacoes = new ValidacoesPonto();
            var cookie = Request.Cookies.Get( "CookieFarm" );
            if(cookie != null)
            {
                var login = Criptografa.Descriptografar( cookie.Value );
                var verificaDados = new QueryMysql();

                var dadosTabelaFuncionario = verificaDados.RecuperaDadosFuncionariosTabelaFuncionariosPerfil( login );
                var dadosPendencias = validacoes.RetornaPendenciasSetor( dadosTabelaFuncionario[0]["idsetor"] );

                TempData["TotalPonto"] = dadosPendencias.Count;


                TempData["Extra1"] = "hidden";
                TempData["Extra2"] = "hidden";
                for(var i = 0; i < dadosPendencias.Count; i++)
                    if(!Convert.ToBoolean( dadosPendencias[i]["ConfirmaGestor"] ))
                    {
                        TempData["IdPendencia" + i] = dadosPendencias[i]["IdPendencia"];
                        TempData["DiaPendencia" + i] =
                            Convert.ToDateTime( dadosPendencias[i]["Data"] ).ToString( "dd/MM/yyyy" );
                        TempData["NomePendencia" + i] = dadosPendencias[i]["Nome"];
                        TempData["IdFuncionario" + i] = dadosPendencias[0]["IdFuncionarioFireBird"];
                        TempData["TotalHorarioPendencia" + i] = dadosPendencias[i]["TotalHorario"];

                        if(4 - Convert.ToInt32( dadosPendencias[i]["TotalHorario"] ) > 0)
                        {
                            TempData["TotalTextBox" + i] = 4 - Convert.ToInt32( dadosPendencias[i]["TotalHorario"] );
                        }
                        else
                        {
                            if(Convert.ToInt32( dadosPendencias[i]["TotalHorario"] ) == 5)
                            {
                                TempData["Extra1"] = "";
                            }
                            else if(Convert.ToInt32( dadosPendencias[i]["TotalHorario"] ) == 6)
                            {
                                TempData["Extra1"] = "";
                                TempData["Extra2"] = "";
                            }

                            TempData["TotalTextBox" + i] = 0;
                        }

                        for(var j = 0; j < Convert.ToInt32( dadosPendencias[i]["TotalHorario"] ); j++)
                            TempData["Hora" + j + "Pendencia" + i] = dadosPendencias[i]["Horario" + j];
                        if(Convert.ToBoolean( dadosPendencias[i]["Justificado"] ))
                        {
                            TempData["StatusJustificativa" + i] = "green";
                            TempData["Justificativa" + i] = dadosPendencias[i]["Justificativa" + i];
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

            return PartialView( "JustificativaSetor" );
        }

        [HttpPost]
        public ActionResult JustificativasGestor(FormCollection formulario)
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                for(var i = 0; i < formulario.Count; i++)
                    if(formulario.AllKeys[i].Contains( "Id" ))
                    {
                        var idHistorico = formulario[i];
                        verificaDados.AtualizaJustificativaGestor( idHistorico );
                    }

                return RedirectToAction( "Principal", "Principal",
                    new
                    {
                        Acao = "Dashboard",
                        Mensagem = "Justificativa confirmada com sucesso !",
                        Controlle = "Principal"
                    } );
            }

            return RedirectToAction( "Login", "Login" );
        }

        public ActionResult NegarJustificativa(string idHistorico)
        {
            var queryRh = new QueryMysqlRh();
            queryRh.NegaJustificativa( idHistorico );
            return RedirectToAction( "Principal", "Principal",
                new
                {
                    Acao = "Dashboard",
                    Mensagem = "Justificativa negada com sucesso!",
                    Controlle = "Principal"
                } );
        }

        public ActionResult BancoHora()
        {
            var verificaDados = new QueryMysql();
            var verificaDadosFire = new QueryFirebird();
            var cookie = Request.Cookies.Get( "CookieFarm" );
            if(cookie != null)
            {
                var login = Criptografa.Descriptografar( cookie.Value );

                var dadosFuncionario = verificaDados.RecuperaDadosUsuarios( login );

                if(dadosFuncionario[0]["gestor"].Equals( "N" ))
                {
                    TempData["Gestor"] = "N";
                    var result = verificaDados.RemoveAccents( dadosFuncionario[0]["nome"] );
                    var cracha = verificaDadosFire.RetornaCrachaFuncionario( result );
                    var horas = verificaDados.RetornaHoras( cracha[0]["MATRICULA"] );
                    var horaTotal = TimeSpan.Parse( horas[0]["hora"] );
                    if(horaTotal.Hours < 0)
                    {
                        TempData["NomeFuncionario"] = dadosFuncionario[0]["nome"];
                        TempData["SaldoHoras"] = horas[0]["hora"];
                        TempData["DataReferencia"] =
                            Convert.ToDateTime( horas[0]["datareferencia"] ).ToString( "dd/MM/yyyy" );
                        TempData["cor"] = "red";
                    }
                    else
                    {
                        TempData["NomeFuncionario"] = dadosFuncionario[0]["nome"];
                        TempData["SaldoHoras"] = horas[0]["hora"];
                        TempData["DataReferencia"] =
                            Convert.ToDateTime( horas[0]["datareferencia"] ).ToString( "dd/MM/yyyy" );
                        TempData["cor"] = "black";
                    }
                }
                else
                {
                    TempData["Gestor"] = "S";
                    var todosFuncionariosSetor =
                        verificaDados.RetornaFuncionariosSetor( dadosFuncionario[0]["idsetor"] );
                    TempData["TotalEquipe"] = todosFuncionariosSetor.Count;
                    for(var i = 0; i < todosFuncionariosSetor.Count; i++)
                    {
                        var result = verificaDados.RemoveAccents( todosFuncionariosSetor[i]["nome"] );
                        var cracha = verificaDadosFire.RetornaCrachaFuncionario( result );
                        var horas = verificaDados.RetornaHoras( cracha[0]["MATRICULA"] );
                        var horaTotal = TimeSpan.Parse( horas[0]["hora"] );
                        if(horaTotal.Hours < 0 || horaTotal.Minutes < 0)
                        {
                            TempData["NomeFuncionarioEquipe" + i] = todosFuncionariosSetor[i]["nome"];
                            TempData["SaldoHorasEquipe" + i] = horas[0]["hora"];
                            TempData["DataReferencia" + i] =
                                Convert.ToDateTime( horas[0]["datareferencia"] ).ToString( "dd/MM/yyyy" );
                            TempData["CorEquipe" + i] = "red";
                        }
                        else
                        {
                            TempData["NomeFuncionarioEquipe" + i] = todosFuncionariosSetor[i]["nome"];
                            TempData["SaldoHorasEquipe" + i] = horas[0]["hora"];
                            TempData["DataReferencia" + i] =
                                Convert.ToDateTime( horas[0]["datareferencia"] ).ToString( "dd/MM/yyyy" );
                            TempData["CorEquipe" + i] = "black";
                        }
                    }
                }

            }

            return PartialView( "BancoHoras" );
        }

        public ActionResult ShowdePremios(string cpf)
        {
            var cpfLimpo = cpf;
            cpfLimpo = cpfLimpo.Replace( ".", "" ).Replace( "-", "" ).Replace( "/", "" );
            var recuperaDados = new QueryMysql();

            var numeroDaSorte = recuperaDados.BuscaNumerodaSorte();
            if(numeroDaSorte.Count > 0)
                TempData["NumeroDaSorte"] = "O último número da sorte foi: " + numeroDaSorte[0]["numerodasorte"] +
                                            " na data: " + numeroDaSorte[0]["data"];
            else
                TempData["NumeroDaSorte"] = "Nenhum sorteio foi realizado até o momento.";


            var extrato = recuperaDados.RetornaExtrato( cpfLimpo );
            TempData["TotalCupons"] = extrato.Count;
            for(var i = 0; i < extrato.Count; i++)
            {
                TempData["Numero" + i] = extrato[i]["id"];
                TempData["Motivo" + i] = extrato[i]["motivo"];
                TempData["Data" + i] = extrato[i]["data"];
            }

            return PartialView( "ShowdePremios" );
        }
        public void CadastroIdNotificacao(string idNotificacao)
        {
            var cookie = Request.Cookies.Get( "CookieFarm" );
            if(cookie != null)
            {
                var login = Criptografa.Descriptografar( cookie.Value );
                var atualiza = new QueryMysql();
                atualiza.CadastraIdNotificacao( idNotificacao, login );
            }
        }
        public ActionResult ControleNumerario()
        {
            var verificaDados = new QueryMysql();
            for(int i = 0; i < 9; i++)
            {
                var dadosNumerarios = verificaDados.RetornaInformacoesNumerario( i.ToString() );
                if(i == 0)
                {
                    TempData["ValorMatriz"] = string.Format( "{0:N}", dadosNumerarios[0]["valor"] );
                    TempData["ObservacaoMatriz"] = dadosNumerarios[0]["observacao"];
                    TempData["DataMatriz"] = dadosNumerarios[0]["dataalteracao"];
                    TempData["AlteradoMatriz"] = verificaDados.RetornaNomeFuncionario( dadosNumerarios[0]["idfuncionarioalteracao"] );
                }
                else if(i == 1)
                {
                    TempData["ValorParana"] = string.Format( "{0:N}", dadosNumerarios[0]["valor"] );
                    TempData["ObservacaoParana"] = dadosNumerarios[0]["observacao"];
                    TempData["DataParana"] = dadosNumerarios[0]["dataalteracao"];
                    TempData["AlteradoParana"] = verificaDados.RetornaNomeFuncionario( dadosNumerarios[0]["idfuncionarioalteracao"] );

                }
                else if(i == 2)
                {
                    TempData["ValorCajuru"] = string.Format( "{0:N}", dadosNumerarios[0]["valor"] );
                    TempData["ObservacaoCajuru"] = dadosNumerarios[0]["observacao"];
                    TempData["DataCajuru"] = dadosNumerarios[0]["dataalteracao"];
                    TempData["AlteradoCajuru"] = verificaDados.RetornaNomeFuncionario( dadosNumerarios[0]["idfuncionarioalteracao"] );
                }
                else if(i == 3)
                {
                    TempData["ValorStClara"] = string.Format( "{0:N}", dadosNumerarios[0]["valor"] );
                    TempData["ObservacaoStClara"] = dadosNumerarios[0]["observacao"];
                    TempData["DataStClara"] = dadosNumerarios[0]["dataalteracao"];
                    TempData["AlteradoStClara"] = verificaDados.RetornaNomeFuncionario( dadosNumerarios[0]["idfuncionarioalteracao"] );
                }
                else if(i == 4)
                {
                    TempData["ValorBH"] = string.Format( "{0:N}", dadosNumerarios[0]["valor"] );
                    TempData["ObservacaoBH"] = dadosNumerarios[0]["observacao"];
                    TempData["DataBH"] = dadosNumerarios[0]["dataalteracao"];
                    TempData["AlteradoBH"] = verificaDados.RetornaNomeFuncionario( dadosNumerarios[0]["idfuncionarioalteracao"] );
                }
                else if(i == 5)
                {
                    TempData["ValorBetim"] = string.Format( "{0:N}", dadosNumerarios[0]["valor"] );
                    TempData["ObservacaoBetim"] = dadosNumerarios[0]["observacao"];
                    TempData["DataBetim"] = dadosNumerarios[0]["dataalteracao"];
                    TempData["AlteradoBetim"] = verificaDados.RetornaNomeFuncionario( dadosNumerarios[0]["idfuncionarioalteracao"] );
                }
                else if(i == 6)
                {
                    TempData["ValorContagem"] = string.Format( "{0:N}", dadosNumerarios[0]["valor"] );
                    TempData["ObservacaoContagem"] = dadosNumerarios[0]["observacao"];
                    TempData["DataContagem"] = dadosNumerarios[0]["dataalteracao"];
                    TempData["AlteradoContagem"] = verificaDados.RetornaNomeFuncionario( dadosNumerarios[0]["idfuncionarioalteracao"] );
                }
                else if(i == 7)
                {
                    TempData["ValorGoias"] = string.Format( "{0:N}", dadosNumerarios[0]["valor"] );
                    TempData["ObservacaoGoias"] = dadosNumerarios[0]["observacao"];
                    TempData["DataGoias"] = dadosNumerarios[0]["dataalteracao"];
                    TempData["AlteradoGoias"] = verificaDados.RetornaNomeFuncionario( dadosNumerarios[0]["idfuncionarioalteracao"] );
                }
                else if(i == 8)
                {
                    TempData["ValorBarreiro"] = string.Format( "{0:N}", dadosNumerarios[0]["valor"] );
                    TempData["ObservacaoBarreiro"] = dadosNumerarios[0]["observacao"];
                    TempData["DataBarreiro"] = dadosNumerarios[0]["dataalteracao"];
                    TempData["AlteradoBarreiro"] = verificaDados.RetornaNomeFuncionario( dadosNumerarios[0]["idfuncionarioalteracao"] );
                }
            }

            return PartialView( "ControleNumerario" );
        }

        [HttpPost]
        public async Task<JsonResult> SalvaNumerario(string valor, string observacao, string agencia)
        {
            var verificaDados = new QueryMysql();
            var cookie = Request.Cookies.Get( "CookieFarm" );
            if(cookie != null)
            {
                var login = Criptografa.Descriptografar( cookie.Value );

                var dadosFuncionario = verificaDados.RecuperaDadosFuncionariosTabelaFuncionariosLogin( login );
                var funcionariosSetor = verificaDados.RetornaFuncionariosSetor( "53" );

                if(agencia.Equals( "Matriz" ))
                {
                    verificaDados.AtualizaNumerario( string.Format( "{0:N}", valor ), observacao, "0", dadosFuncionario[0]["id"] );

                    for(int i = 0; i < funcionariosSetor.Count; i++)
                    {
                        var envia = new EnviodeAlertas();

                        await envia.EnviaAlertaFuncionario( funcionariosSetor[i], "Foi feita uma alteração no numerário do P.A " + agencia + ".", "12" );

                    }


                }
                else if(agencia.Equals( "Parana" ))
                {
                    verificaDados.AtualizaNumerario( string.Format( "{0:N}", valor ), observacao, "1", dadosFuncionario[0]["id"] );


                    for(int i = 0; i < funcionariosSetor.Count; i++)
                    {
                        var envia = new EnviodeAlertas();

                        await envia.EnviaAlertaFuncionario( funcionariosSetor[i], "Foi feita uma alteração no numerário do P.A " + agencia + ".", "12" );

                    }
                }
                else if(agencia.Equals( "Cajuru" ))
                {
                    verificaDados.AtualizaNumerario( string.Format( "{0:N}", valor ), observacao, "2", dadosFuncionario[0]["id"] );


                    for(int i = 0; i < funcionariosSetor.Count; i++)
                    {
                        var envia = new EnviodeAlertas();

                        await envia.EnviaAlertaFuncionario( funcionariosSetor[i], "Foi feita uma alteração no numerário do P.A " + agencia + ".", "12" );

                    }
                }
                else if(agencia.Equals( "StClara" ))
                {
                    verificaDados.AtualizaNumerario( string.Format( "{0:N}", valor ), observacao, "3", dadosFuncionario[0]["id"] );


                    for(int i = 0; i < funcionariosSetor.Count; i++)
                    {
                        var envia = new EnviodeAlertas();

                        await envia.EnviaAlertaFuncionario( funcionariosSetor[i], "Foi feita uma alteração no numerário do P.A " + agencia + ".", "12" );

                    }
                }
                else if(agencia.Equals( "BH" ))
                {
                    verificaDados.AtualizaNumerario( string.Format( "{0:N}", valor ), observacao, "4", dadosFuncionario[0]["id"] );


                    for(int i = 0; i < funcionariosSetor.Count; i++)
                    {
                        var envia = new EnviodeAlertas();

                        await envia.EnviaAlertaFuncionario( funcionariosSetor[i], "Foi feita uma alteração no numerário do P.A " + agencia + ".", "12" );

                    }
                }
                else if(agencia.Equals( "Betim" ))
                {
                    verificaDados.AtualizaNumerario( string.Format( "{0:N}", valor ), observacao, "5", dadosFuncionario[0]["id"] );


                    for(int i = 0; i < funcionariosSetor.Count; i++)
                    {
                        var envia = new EnviodeAlertas();

                        await envia.EnviaAlertaFuncionario( funcionariosSetor[i], "Foi feita uma alteração no numerário do P.A " + agencia + ".", "12" );

                    }
                }
                else if(agencia.Equals( "Contagem" ))
                {
                    verificaDados.AtualizaNumerario( string.Format( "{0:N}", valor ), observacao, "6", dadosFuncionario[0]["id"] );

                    for(int i = 0; i < funcionariosSetor.Count; i++)
                    {
                        var envia = new EnviodeAlertas();

                        await envia.EnviaAlertaFuncionario( funcionariosSetor[i], "Foi feita uma alteração no numerário do P.A " + agencia + ".", "12" );

                    }
                }
                else if(agencia.Equals( "Goias" ))
                {
                    verificaDados.AtualizaNumerario( string.Format( "{0:N}", valor ), observacao, "7", dadosFuncionario[0]["id"] );


                    for(int i = 0; i < funcionariosSetor.Count; i++)
                    {
                        var envia = new EnviodeAlertas();

                        await envia.EnviaAlertaFuncionario( funcionariosSetor[i], "Foi feita uma alteração no numerário do P.A " + agencia + ".", "12" );

                    }
                }
                else if(agencia.Equals( "Barreiro" ))
                {
                    verificaDados.AtualizaNumerario( string.Format( "{0:N}", valor ), observacao, "8", dadosFuncionario[0]["id"] );


                    for(int i = 0; i < funcionariosSetor.Count; i++)
                    {
                        var envia = new EnviodeAlertas();

                        await envia.EnviaAlertaFuncionario( funcionariosSetor[i], "Foi feita uma alteração no numerário do P.A " + agencia + ".", "12" );

                    }
                }
            }

            return Json( "ok" );
        }
    }
}