using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mail;
using System.Web.Mvc;
using PortalSicoobDivicred.Aplicacao;
using PortalSicoobDivicred.Models;
using ElasticEmailClient;
using OneApi.Client.Impl;
using OneApi.Config;
using OneApi.Model;
using RestSharp;

namespace PortalSicoobDivicred.Controllers
{
    public class PrincipalController : Controller
    {
        // GET: Principal
        public ActionResult Principal(string Acao, string Mensagem)
        {
            var VerificaDados = new QuerryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            try
            {
                if (!Acao.Equals("Dashboard"))

                {
                    TempData["Opcao"] = Acao;
                    TempData["Mensagem"] = Mensagem;
                    ModelState.Clear();
                }
                else
                {
                    TempData["Opcao"] = "Dashboard";
                }
            }
            catch
            {
                TempData["Opcao"] = "Dashboard";
            }
            if (Logado)
            {



                var Cookie = Request.Cookies.Get("CookieFarm");

                var Login = Criptografa.Descriptografar(Cookie.Value);
               if (VerificaDados.PrimeiroLogin(Login))
                {
                    return View("FormularioCadastro");

                }
                else
                {
                    var DadosUsuarioBanco = VerificaDados.RecuperaDadosUsuarios(Login);

                    if (VerificaDados.PermissaoCurriculos(DadosUsuarioBanco[0]["login"]))
                    {
                        TempData["PermissaoCurriculo"] =
                            " <a  href='javascript: Curriculo(); void(0); ' class='item' style='color: #38d5c5;'><span class='icon'><i class='fa fa-book'></i></span><span class='name'> Currículo</span></a>";
                    }
                    else
                    {
                        TempData["PermissaoCurriculo"] = "";

                    }
                    TempData["NomeLateral"] = DadosUsuarioBanco[0]["login"];
                    TempData["EmailLateral"] = DadosUsuarioBanco[0]["email"];

                    TempData["ImagemPerfil"] = "https://docs.google.com/uc?id=0B2CLuTO3N2_obWdkajEzTmpGeU0";
                    return View();
                }
            }

            return RedirectToAction("Login", "Login");
            }
        

        public ActionResult Dashboard()
        {
            var VerificaDados = new QuerryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                return PartialView("Dashboard");
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult Curriculo(string Ordenacao)
        {
            var VerificaDados = new QuerryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var CarregaDados = new QuerryMysql();
                List<Dictionary<string, string>> DadosCurriculos = null;
                try
                {
                    if (Ordenacao.Length == 0)
                        DadosCurriculos = CarregaDados.RecuperaCurriculos();
                    if (Ordenacao.Equals("Alfabetico"))
                        DadosCurriculos = CarregaDados.RecuperaCurriculosAlfabetico();
                    if (Ordenacao.Equals("Status"))
                        DadosCurriculos = CarregaDados.RecuperaCurriculosStatus();
                }
                catch
                {
                    DadosCurriculos = CarregaDados.RecuperaCurriculos();
                }
                var DadosVagas = CarregaDados.RecuperaVagas();

                TempData["TotalVagas"] = DadosVagas.Count;
                TempData["TotalCurriculo"] = DadosCurriculos.Count;

                for (var i = 0; i < DadosCurriculos.Count; i++)
                {
                    TempData["Nome" + i] = DadosCurriculos[i]["nome"];
                    TempData["Cpf" + i] = DadosCurriculos[i]["cpf"];
                    TempData["Email" + i] = DadosCurriculos[i]["email"];
                    TempData["Cidade" + i] = DadosCurriculos[i]["cidade"];
                    TempData["Area" + i] = DadosCurriculos[i]["descricao"].Replace(";", " ");
                    if (DadosCurriculos[i]["status"].Equals("S"))
                        TempData["Status" + i] = "green";
                    if (DadosCurriculos[i]["status"].Equals("N"))
                        TempData["Status" + i] = "red";
                    if (DadosCurriculos[i]["status"].Equals("E"))
                        TempData["Status" + i] = "blue";
                    if (DadosCurriculos[i]["status"].Equals("A"))
                        TempData["Status" + i] = "yellow";

                    if (DadosCurriculos[i]["idarquivogoogle"].Equals("0"))
                        TempData["Imagem" + i] = "https://docs.google.com/uc?id=0B2CLuTO3N2_obWdkajEzTmpGeU0";
                    else
                        TempData["Imagem" + i] = "https://portalsicoobdivicred.com.br/Uploads/" +
                                                 DadosCurriculos[i]["idarquivogoogle"] + "";
                }

                for (var i = 0; i < DadosVagas.Count; i++)
                {
                    TempData["IdVaga" + i] = DadosVagas[i]["id"];
                    TempData["Titulo" + i] = DadosVagas[i]["titulo"];
                    TempData["Descricao" + i] = DadosVagas[i]["descricao"];
                    TempData["AreaVaga" + i] = DadosVagas[i]["areadeinteresse"].Replace(";", " ");
                    if (DadosVagas[i]["ativa"].Equals("S"))
                        TempData["StatusVaga" + i] = "green";
                    else
                        TempData["StatusVaga" + i] = "red";
                }


                return PartialView("Curriculo");
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult CurriculoArea(string Ordenacao, string Cidade, string Certificacao)
        {
            var VerificaDados = new QuerryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var CarregaDados = new QuerryMysql();
                List<Dictionary<string, string>> DadosCurriculos = null;
                try
                {
                    if (Ordenacao.Length == 0)
                        DadosCurriculos = CarregaDados.RecuperaCurriculos();
                    else
                    {
                        DadosCurriculos = CarregaDados.RecuperaCurriculosArea(Ordenacao, Cidade, Certificacao);
                    }
                }
                catch
                {
                    DadosCurriculos = CarregaDados.RecuperaCurriculos();
                }
                var DadosVagas = CarregaDados.RecuperaVagas();

                TempData["TotalVagas"] = DadosVagas.Count;
                TempData["TotalCurriculo"] = DadosCurriculos.Count;

                for (var i = 0; i < DadosCurriculos.Count; i++)
                {
                    TempData["Nome" + i] = DadosCurriculos[i]["nome"];
                    TempData["Cpf" + i] = DadosCurriculos[i]["cpf"];
                    TempData["Email" + i] = DadosCurriculos[i]["email"];
                    TempData["Cidade" + i] = DadosCurriculos[i]["cidade"];
                    TempData["Area" + i] = DadosCurriculos[i]["descricao"].Replace(";", " ");
                    if (DadosCurriculos[i]["status"].Equals("S"))
                        TempData["Status" + i] = "green";
                    else
                        TempData["Status" + i] = "red";

                    if (DadosCurriculos[i]["idarquivogoogle"].Equals("0"))
                        TempData["Imagem" + i] = "https://docs.google.com/uc?id=0B2CLuTO3N2_obWdkajEzTmpGeU0";
                    else
                        TempData["Imagem" + i] = "https://portalsicoobdivicred.com.br/Uploads/" +
                                                 DadosCurriculos[i]["idarquivogoogle"] + "";
                }

                for (var i = 0; i < DadosVagas.Count; i++)
                {
                    TempData["IdVaga" + i] = DadosVagas[i]["id"];
                    TempData["Titulo" + i] = DadosVagas[i]["titulo"];
                    TempData["Descricao" + i] = DadosVagas[i]["descricao"];
                    TempData["AreaVaga" + i] = DadosVagas[i]["areadeinteresse"].Replace(";", " ");
                    if (DadosVagas[i]["ativa"].Equals("S"))
                        TempData["StatusVaga" + i] = "green";
                    else
                        TempData["StatusVaga" + i] = "red";
                }


                return PartialView("Curriculo");
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }


        public ActionResult GerenciarVaga(string IdVaga)
        {
            var VerificaDados = new QuerryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var RecuperaDados = new QuerryMysql();
                var DadosCurriculos = RecuperaDados.RecuperaCurriculosHistorico(IdVaga);
                TempData["TotalCurriculo"] = DadosCurriculos.Count;
                var DadosVagas = RecuperaDados.RecuperaVagasId(IdVaga);
                TempData["QuantidadeCurriculo"] = "N° de candidatos com interesse nesta vaga: " + DadosCurriculos.Count;
                TempData["TituloVaga"] = IdVaga + "-" + DadosVagas[0]["titulo"];
                TempData["DescricaoVaga"] = DadosVagas[0]["descricao"];
                TempData["IdVaga"] = IdVaga;
                var ProcessoAberto = RecuperaDados.VerificaProcesso(IdVaga);

                if (ProcessoAberto)
                {
                    TempData["ProcessoAtivo"] = "disabled";
                    TempData["DicaProcesso"] = "Já existe um processo seletivo aberto";
                    TempData["DicaEncerramento"] = "Clique para encerrar o processo seletivo";
                    TempData["EncerraAtivo"] = "";
                }
                else
                {
                    TempData["ProcessoAtivo"] = "";
                    TempData["DicaProcesso"] = "Clique para iniciar um processo seletivo";
                    TempData["DicaEncerramento"] = "Ainda não existe processo seletivo em aberto";
                    TempData["EncerraAtivo"] = "disabled";

                }


                for (var i = 0; i < DadosCurriculos.Count; i++)
                {
                    TempData["Nome" + i] = DadosCurriculos[i]["nome"];
                    TempData["Cpf" + i] = DadosCurriculos[i]["cpf"];
                    TempData["Email" + i] = DadosCurriculos[i]["email"];
                    TempData["Cidade" + i] = DadosCurriculos[i]["cidade"];
                    TempData["Certificacao" + i] = DadosCurriculos[i]["certificacao"];
                    if (DadosCurriculos[i]["idarquivogoogle"].Equals("0"))
                        TempData["Imagem" + i] = "https://docs.google.com/uc?id=0B2CLuTO3N2_obWdkajEzTmpGeU0";
                    else
                        TempData["Imagem" + i] = "https://portalsicoobdivicred.com.br/Uploads/" +
                                                 DadosCurriculos[i]["idarquivogoogle"] + "";
                }
                if (DadosVagas[0]["ativa"].Equals("S"))
                {
                    TempData["Ativa"] = "";
                    TempData["Dica"] = "Clique para encerrar esta vaga.";

                }
                else
                {
                    TempData["ProcessoAtivo"] = "";
                    TempData["DicaProcesso"] = "Clique para iniciar um processo seletivo.";
                    TempData["DicaEncerramento"] = "Clique para encerrar o processo seletivo";
                    TempData["EncerraAtivo"] = "";
                    TempData["Ativa"] = "disabled";
                    TempData["Dica"] = "Esta vaga já esta encerrada.";

                }
                return PartialView("GerenciarVaga");
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        [HttpPost]
        public ActionResult CadastrarVaga(Vagas Vaga, FormCollection Vagas)
        {
            var VerificaDados = new QuerryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var CadastrarVaga = new QuerryMysql();

                if (Vagas.Count == 6)
                    ModelState.AddModelError("", "Favor selecionar uma área de interesse !");
                if (ModelState.IsValid)
                {
                    var Areas = "";
                    for (var i = 6; i < Vagas.Count; i++)
                    {
                        Areas = Areas + Vagas[i] + ";";
                    }
                    var TodosEmail = "";
                    
                    var EmailCelular = CadastrarVaga.CadastrarVaga(Vaga.Descricao, Areas, Vaga.Salario.Replace(",", "."),
                        Vaga.Requisitos,
                        Vaga.Titulo, Vaga.Beneficio);
                    var EnvioSms = new string[EmailCelular.Count];
                    for (int j = 0; j < EmailCelular.Count; j++)
                    {
                        TodosEmail = TodosEmail + ";" + EmailCelular[j]["email"];
                        var Certo = "55" + EmailCelular[j]["telefoneprincipal"].Replace(')', ' ').Replace('(', ' ').Replace('-', ' ');
                        EnvioSms[j] = Certo.Replace(" ", "");
                    }
                    Configuration configuration = new Configuration("Divicred", "Euder17!");
                    SMSClient smsClient = new SMSClient(configuration);
                    SMSRequest smsRequest = new SMSRequest("Portal Sicoob Divicred", "Portal Sicoob Divicred, Novas vagas cadastradas.Entre e confira. ", EnvioSms);
                    SendMessageResult requestId = smsClient.SmsMessagingClient.SendSMS(smsRequest);



                    Api.ApiKey = "edff66b8-adb7-461e-9f3f-fd1649cedefa";
                    string[] recipients = {TodosEmail};
                    var subject = "Novas Vagas";
                    var fromEmail = "correio@divicred.com.br";
                    var fromName = "Sicoob Divicred";
                    var bodyText = "";
                    var bodyHtml = Api.Template.LoadTemplate(5381).BodyHtml;
                    ApiTypes.EmailSend result = null;

                    try
                    {
                        result = Api.Email.Send(subject, fromEmail, fromName, to: recipients,
                            bodyText: bodyText, bodyHtml: bodyHtml);
                    }
                    catch
                    {
                    }
                    TempData["Opcao"] = "Curriculo";

                    return RedirectToAction("Principal",
                        new {Acao = "Curriculo", Mensagem = "Vaga cadastrada com sucesso !"});
                }
                return RedirectToAction("Principal", new {Acao = "Curriculo"});
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public async Task<ActionResult> PerfilCandidato(string cpf)
        {
            var VerificaDados = new QuerryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var VagasEspecificas = VerificaDados.RecuperaVagaEspecifica();
                TempData["TotalVaga"] = VagasEspecificas.Count;
                for (int f = 0; f < VagasEspecificas.Count; f++)
                {
                    TempData["IdVaga" + f] = VagasEspecificas[f]["id"];
                    TempData["NomeVaga" + f] = VagasEspecificas[f]["descricao"];
                }

                var DadosUsuario = new Usuario();
                var DadosUsuarioBanco = VerificaDados.RecuperaDadosCandidato(cpf);

                var DadosProcessosSeletivos = VerificaDados.RecuperaProcesso(DadosUsuarioBanco[0]["id"]);
                var Formulario = VerificaDados.RecuperaFormulario(DadosUsuarioBanco[0]["id"]);

                if (Formulario.Count > 0)
                {
                    TempData["ExisteFormulario"] = true;
                    TempData["IdFormulario"] = Formulario[0]["id"];
                }
                else
                {
                    TempData["ExisteFormulario"] = false;
                    TempData["IdFormulario"] = 0;
                }

                if (DadosProcessosSeletivos.Count > 0)
                {
                    TempData["ExisteProcesso"] = true;
                    TempData["TotalProcessos"] = DadosProcessosSeletivos.Count;
                    for (int i = 0; i < DadosProcessosSeletivos.Count; i++)
                    {
                        TempData["NomeVaga" + i] = DadosProcessosSeletivos[i]["nomevaga"];
                        TempData["IdVaga" + i] = DadosProcessosSeletivos[i]["idvaga"];
                        TempData["Escrita" + i] = DadosProcessosSeletivos[i]["prova"];
                        if (Convert.ToDecimal(DadosProcessosSeletivos[i]["prova"]) >= 60)
                        {
                            TempData["CorEscrita" + i] = "green";
                        }
                        else
                        {
                            TempData["CorEscrita" + i] = "red";
                            TempData["Aprovado" + i] = "red";
                        }
                        TempData["Psicologico" + i] = DadosProcessosSeletivos[i]["psicologico"];
                        try
                        {
                            if (DadosProcessosSeletivos[i]["psicologico"].Equals("Aprovado"))
                            {
                                TempData["CorPsicologico" + i] = "green";
                            }
                            else
                            {
                                TempData["CorPsicologico" + i] = "red";
                                TempData["Aprovado" + i] = "red";
                            }
                        }
                        catch
                        {
                            TempData["CorPsicologico" + i] = "red";
                            TempData["Aprovado" + i] = "red";
                        }
                        try
                        {
                            TempData["Gerente" + i] = DadosProcessosSeletivos[i]["gerente"];
                            if (DadosProcessosSeletivos[i]["gerente"].Equals("Aprovado"))
                            {
                                TempData["CorGerente" + i] = "green";
                            }
                            else
                            {
                                TempData["CorGerente" + i] = "red";
                                TempData["Aprovado" + i] = "red";
                            }
                        }
                        catch
                        {
                            TempData["CorGerente" + i] = "red";
                            TempData["Aprovado" + i] = "red";
                        }
                    }

                }
                else
                {
                    TempData["ExisteProcesso"] = false;
                }

                if (!DadosUsuarioBanco[0]["idarquivogoogle"].Equals("0"))
                {
                    TempData["ImagemPerfil"] = "https://portalsicoobdivicred.com.br/Uploads/" +
                                               DadosUsuarioBanco[0]["idarquivogoogle"];
                }
                else
                {
                    TempData["ImagemPerfil"] = "https://docs.google.com/uc?id=0B2CLuTO3N2_obWdkajEzTmpGeU0";
                }
                var AreasInteresse = VerificaDados.RecuperaAreaInteresseUsuarios(DadosUsuarioBanco[0]["id"]);

                DadosUsuario.NomeCompleto = DadosUsuarioBanco[0]["nome"];
                DadosUsuario.TelefonePrincipal = DadosUsuarioBanco[0]["telefoneprincipal"];
                DadosUsuario.TelefoneSecundario = DadosUsuarioBanco[0]["telefonesecundario"];
                DadosUsuario.Cep = DadosUsuarioBanco[0]["cep"];
                DadosUsuario.Rua = DadosUsuarioBanco[0]["endereco"];
                DadosUsuario.Numero = DadosUsuarioBanco[0]["numero"];
                DadosUsuario.Bairro = DadosUsuarioBanco[0]["bairro"];
                DadosUsuario.Cidade = DadosUsuarioBanco[0]["cidade"];
                DadosUsuario.Uf = DadosUsuarioBanco[0]["uf"];
                DadosUsuario.Email = DadosUsuarioBanco[0]["email"];
                DadosUsuario.TipoDeficiencia = DadosUsuarioBanco[0]["tipodeficiencia"];
                DadosUsuario.Sexo = DadosUsuarioBanco[0]["sexo"];
                DadosUsuario.Informatica = DadosUsuarioBanco[0]["informatica"];
                DadosUsuario.Idade = Convert.ToDateTime(DadosUsuarioBanco[0]["datanascimento"]);
                DadosUsuario.Cpf = DadosUsuarioBanco[0]["cpf"];
                DadosUsuario.Identidade = DadosUsuarioBanco[0]["identidade"];
                DadosUsuario.Complemento = DadosUsuarioBanco[0]["complemento"];
                DadosUsuario.Resumo = DadosUsuarioBanco[0]["resumo"];
                DadosUsuario.Certificacao = DadosUsuarioBanco[0]["certificacao"];
                TempData["Conhecido"] = DadosUsuarioBanco[0]["conhecido"];


                if (DadosUsuarioBanco[0]["disponibilidadeviagem"].Equals("S"))
                    TempData["Disponibilidades"] = "SIM";
                else
                    TempData["Disponibilidades"] = "NÃO";

                DadosUsuario.QuantidadeFilho = DadosUsuarioBanco[0]["quantidadefilhos"];
                DadosUsuario.TipoDeficiencia = DadosUsuarioBanco[0]["tipodeficiencia"];
                DadosUsuario.Cnh = DadosUsuarioBanco[0]["cnh"];
                DadosUsuario.PrimeiraCnh = DadosUsuarioBanco[0]["dataprimeiracnh"];
                DadosUsuario.CatCnh = DadosUsuarioBanco[0]["categoriacnh"];
                TempData["estadocivil"] =
                    VerificaDados.RecuperaEstadoCivilCandidato(DadosUsuarioBanco[0]["estadocivil"]);
                TempData["Escolaridade"] =
                    VerificaDados.RecuperaEscolaridadeCandidato(DadosUsuarioBanco[0]["idtipoescolaridade"]);
                var Profissional = VerificaDados.RecuperaDadosUsuariosProissional(DadosUsuarioBanco[0]["id"]);

                var NomeEmpresa = new List<string>();
                var NomeCargo = new List<string>();
                var DataEntrada = new List<string>();
                var DataSaida = new List<string>();
                var Atividades = new List<string>();

                for (var i = 0; i < Profissional.Count; i++)
                {
                    NomeEmpresa.Add(Profissional[i]["nomeempresa"]);
                    NomeCargo.Add(Profissional[i]["nomecargo"]);
                    DataEntrada.Add(Convert.ToDateTime(Profissional[i]["dataentrada"]).Date.ToString());
                    DataSaida.Add(Convert.ToDateTime(Profissional[i]["datasaida"]).Date.ToString());
                    Atividades.Add(Profissional[i]["atividadedesempenhada"]);
                    if (Profissional[i]["empregoatual"].Equals("S"))
                        TempData["EmpregoAtual" + i] = "SIM";
                    else
                        TempData["EmpregoAtual" + i] = "NÃO";
                }

                DadosUsuario.NomeEmpresa = NomeEmpresa;
                DadosUsuario.NomeCargo = NomeCargo;
                DadosUsuario.DataEntrada = DataEntrada;
                DadosUsuario.DataSaida = DataSaida;
                DadosUsuario.Atividades = Atividades;


                var Educacional = VerificaDados.RecuperaDadosUsuariosEducacional(DadosUsuarioBanco[0]["id"]);

                var NomeInstituicao = new List<string>();
                var TipoFormacao = new List<string>();
                var NomeCurso = new List<string>();
                var AnoInicio = new List<string>();
                var AnoFim = new List<string>();


                for (var i = 0; i < Educacional.Count; i++)
                {
                    NomeInstituicao.Add(Educacional[i]["nomeinstituicao"]);
                    NomeCurso.Add(Educacional[i]["nomecurso"]);
                    AnoInicio.Add(Educacional[i]["anoinicio"]);
                    AnoFim.Add(Educacional[i]["anofim"]);
                    TipoFormacao.Add(Educacional[i]["tipoformacao"]);
                }

                DadosUsuario.NomeInstituicao = NomeInstituicao;
                DadosUsuario.NomeCurso = NomeCurso;
                DadosUsuario.AnoInicio = AnoInicio;
                DadosUsuario.AnoTermino = AnoFim;
                DadosUsuario.TipoFormacao = TipoFormacao;


                for (var i = 1; i <= 17; i++)
                    TempData["Area" + i] = "";

                var Areas = AreasInteresse[0]["descricao"].Split(';');

                TempData["Area"] = Areas;

                return PartialView("ModalPerfil", DadosUsuario);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        [HttpPost]
        public ActionResult FiltrarPerfilVaga(FormCollection Filtros)
        {
            var VerificaDados = new QuerryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Filtrar = new QuerryMysql();
                var Sexo = Filtros["FiltroSexo"];
                var Cidade = Filtros["FiltroCidade"];
                var Graduacao = Filtros["FiltroGraduacao"];
                var AnoFormacao = Filtros["FiltroAnoFormacao"];
                var FaixaEtaria = Filtros["FiltroFaixaEtaria"];
                var Certificacao = Filtros["FiltroCertificacao"];
                var CursoGraduacao = Filtros["FiltroCurso"];
                var Profissional = Filtros["FiltroProfissional"];
                var IdVaga = Filtros["TituloVaga"].Split('-');
                var Query =
                    "select a.cpf,a.nome,a.email, a.idarquivogoogle,a.cidade,a.certificacao from historicos c LEFT JOIN candidatos a on c.idcandidato=a.id INNER JOIN candidatos u2 on (c.idcandidato=u2.id) where c.idvaga=" +
                    IdVaga[0] + "";

                if (!Sexo.Equals("Sexo"))
                    Query = Query + " AND a.sexo = '" + Sexo + "'";
                if (Cidade.Length > 0)
                    Query = Query + " AND a.cidade like'%" + Cidade + "%'";
                if (Certificacao.Length > 0)
                    Query = Query + " AND a.certificacao like'%" + Certificacao + "%'";
                if (!FaixaEtaria.Equals("Faixa Etária"))
                    if (FaixaEtaria.Contains("< 18"))
                        Query = Query + " AND  YEAR(FROM_DAYS(TO_DAYS(NOW())-TO_DAYS(a.datanascimento)))<18";
                    else if (FaixaEtaria.Contains("18 - 25"))
                        Query = Query +
                                " AND  YEAR(FROM_DAYS(TO_DAYS(NOW())-TO_DAYS(a.datanascimento))) BETWEEN 18 and 25";
                    else if (FaixaEtaria.Contains("25 - 30"))
                        Query = Query +
                                " AND  YEAR(FROM_DAYS(TO_DAYS(NOW())-TO_DAYS(a.datanascimento))) BETWEEN 25 and 30";
                    else if (FaixaEtaria.Contains("30 - 40"))
                        Query = Query +
                                " AND  YEAR(FROM_DAYS(TO_DAYS(NOW())-TO_DAYS(a.datanascimento))) BETWEEN 30 and 40";
                    else if (FaixaEtaria.Contains(" 40 > "))
                        Query = Query + " AND  YEAR(FROM_DAYS(TO_DAYS(NOW())-TO_DAYS(a.datanascimento)))>40";
                if (!Graduacao.Equals("Graduação"))
                {
                    var Escolaridade = Graduacao.Split('|');
                    var IdsEscolaridade = Filtrar.RecuperaIdEscolaridade(Escolaridade[0]);
                    var escolaridades = " and (a.idtipoescolaridade=" + IdsEscolaridade[0]["id"];

                    for (var i = 1; i < IdsEscolaridade.Count; i++)
                        escolaridades = escolaridades + " or a.idtipoescolaridade=" + IdsEscolaridade[i]["id"];
                    Query = Query + escolaridades + ")";
                }
                if (AnoFormacao.Length > 0)
                    Query = Query + " AND (SELECT count(id) FROM dadosescolares where anofim <=" + AnoFormacao +
                            " and idcandidato=a.id)>=1";
                if (CursoGraduacao.Length > 0)
                    Query = Query + " AND (SELECT count(id) FROM dadosescolares where nomecurso like'%" +
                            CursoGraduacao +
                            "%' and idcandidato=a.id)>=1";
                if (Profissional.Length > 0)
                {
                    Query = Query +
                            " AND (SELECT count(id) FROM dadosprofissionais where atividadedesempenhada like'%" +
                            Profissional + "%' and idcandidato=a.id)>=1";
                }
                var DadosCurriculos = Filtrar.FiltroVaga(Query);

                var DadosVagas = Filtrar.RecuperaVagasId(IdVaga[0]);
                TempData["IdVaga"] = IdVaga[0];
                TempData["TituloVaga"] = IdVaga[0] + "-" + DadosVagas[0]["titulo"];
                TempData["DescricaoVaga"] = DadosVagas[0]["descricao"];
                TempData["TotalCurriculo"] = DadosCurriculos.Count;
                TempData["QuantidadeCurriculo"] = "N° de candidatos com interesse nesta vaga: " + DadosCurriculos.Count;

                var ProcessoAberto = Filtrar.VerificaProcesso(IdVaga[0]);

                if (ProcessoAberto)
                {
                    TempData["ProcessoAtivo"] = "disabled";
                    TempData["DicaProcesso"] = "Já existe um processo seletivo aberto";
                    TempData["DicaEncerramento"] = "Clique para encerrar o processo seletivo";
                    TempData["EncerraAtivo"] = "";
                }
                else
                {
                    TempData["ProcessoAtivo"] = "";
                    TempData["DicaProcesso"] = "Clique para iniciar um processo seletivo";
                    TempData["DicaEncerramento"] = "Ainda não existe processo seletivo em aberto";
                    TempData["EncerraAtivo"] = "disabled";

                }


                for (var i = 0; i < DadosCurriculos.Count; i++)
                {
                    TempData["Nome" + i] = DadosCurriculos[i]["nome"];
                    TempData["Cpf" + i] = DadosCurriculos[i]["cpf"];
                    TempData["Email" + i] = DadosCurriculos[i]["email"];
                    TempData["Cidade" + i] = DadosCurriculos[i]["cidade"];
                    TempData["Certificacao" + i] = DadosCurriculos[i]["certificacao"];
                    if (DadosCurriculos[i]["idarquivogoogle"].Equals("0"))
                        TempData["Imagem" + i] = "https://docs.google.com/uc?id=0B2CLuTO3N2_obWdkajEzTmpGeU0";
                    else
                        TempData["Imagem" + i] = "https://portalsicoobdivicred.com.br/Uploads/" +
                                                 DadosCurriculos[i]["idarquivogoogle"] + "";
                }
                if (DadosVagas[0]["ativa"].Equals("S"))
                {
                    TempData["Ativa"] = "";
                    TempData["Dica"] = "Clique para encerrar esta vaga.";

                }
                else
                {
                    TempData["ProcessoAtivo"] = "disabled";
                    TempData["DicaProcesso"] = "Esta vaga está encerrada.";
                    TempData["DicaEncerramento"] = "Esta vaga está encerrada";
                    TempData["EncerraAtivo"] = "disabled";
                    TempData["Ativa"] = "disabled";
                    TempData["Dica"] = "Esta vaga já esta encerrada.";

                }
                return PartialView("GerenciarVaga");
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult EncerrarVaga(string IdVaga)
        {
            var VerificaDados = new QuerryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var RecuperaDados = new QuerryMysql();
                var Vaga = IdVaga.Split('-');
                RecuperaDados.EncerrarVaga(Vaga[0]);
                return RedirectToAction("Principal",
                    new {Acao = "Curriculo", Mensagem = "Vaga encerrada com sucesso !"});
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult AbrirProcesso(FormCollection Curriculos)
        {
            var VerificaDados = new QuerryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var IdVaga = Curriculos["vaga"];
                var Vaga = IdVaga.Split(',');
                QuerryMysql IniciarProcesso = new QuerryMysql();
                for (int i = 0; i < Curriculos.Count; i++)
                {
                    if (!Curriculos.Keys[i].Contains("vaga") && !Curriculos.Keys[i].Equals("Alerta"))
                    {


                        IniciarProcesso.IniciarProcessoSeletivo(Curriculos.Keys[i], Vaga[0]);
                        var EmailCelular = IniciarProcesso.RecuperaEmail(Curriculos.Keys[i]);

                        var Certo = "55" + EmailCelular[0]["telefoneprincipal"].Replace(')', ' ').Replace('(', ' ').Replace('-', ' ');
                        Configuration configuration = new Configuration("Divicred", "Euder17!");
                        SMSClient smsClient = new SMSClient(configuration);
                        SMSRequest smsRequest = new SMSRequest("Portal Sicoob Divicred", Curriculos["Alerta"], Certo.Replace(" ",""));
                        SendMessageResult requestId = smsClient.SmsMessagingClient.SendSMS(smsRequest);

                        Api.ApiKey = "edff66b8-adb7-461e-9f3f-fd1649cedefa";
                        string[] recipients = {EmailCelular[0]["email"]};
                        var subject = "Parabéns";
                        var fromEmail = "correio@divicred.com.br";
                        var fromName = "Sicoob Divicred";
                        var bodyText = "";
                        var bodyHtml = Api.Template.LoadTemplate(5448).BodyHtml;

                        ApiTypes.EmailSend result = null;

                        try
                        {
                            result = Api.Email.Send(subject, fromEmail, fromName, to: recipients,
                                bodyText: bodyText, bodyHtml: bodyHtml);
                        }
                        catch
                        {
                        }
                        IniciarProcesso.CadastrarAlertaEspecifico(Curriculos["Alerta"], EmailCelular[0]["id"]);
                    }
                }

                var DadosCurriculos = IniciarProcesso.RecuperaCurriculosHistorico(Vaga[0]);
                var Emails = "";
                var SmsNegado = new string[DadosCurriculos.Count];
                var Count = 0;
                for (int j = 0; j < DadosCurriculos.Count; j++)
                {
                    if (!Curriculos.AllKeys.Contains(DadosCurriculos[j]["cpf"]))
                    {
                        Emails = Emails + ";" + DadosCurriculos[j]["email"];
                        var TelefoneCerto = "55" + DadosCurriculos[j]["telefoneprincipal"].Replace(')', ' ').Replace('(', ' ').Replace('-', ' ');
                        SmsNegado[Count] = TelefoneCerto.Replace(" ", "");
                        Count++;
                    }
                }

                

                Configuration configuration2 = new Configuration("Divicred", "Euder17!");
                SMSClient smsClient2 = new SMSClient(configuration2);
                SMSRequest smsRequest2 = new SMSRequest("Portal Sicoob Divicred","Portal Sicoob Divicred. Infelizmente você não foi selecionado para proxima etapa do processo seletivo.", SmsNegado);
                SendMessageResult requestId2 = smsClient2.SmsMessagingClient.SendSMS(smsRequest2);

                Api.ApiKey = "edff66b8-adb7-461e-9f3f-fd1649cedefa";
                string[] recipients2 = {Emails};
                var subject2 = "Atualização de Status";
                var fromEmail2 = "correio@divicred.com.br";
                var fromName2 = "Sicoob Divicred";
                var bodyText2 = "";
                var bodyHtml2 = Api.Template.LoadTemplate(5394).BodyHtml;

                ApiTypes.EmailSend result2 = null;

                try
                {
                    result2 = Api.Email.Send(subject2, fromEmail2, fromName2, to: recipients2,
                        bodyText: bodyText2, bodyHtml: bodyHtml2);
                }
                catch
                {
                }


                return RedirectToAction("Principal",
                    new {Acao = "Curriculo", Mensagem = "Processo seletivo inciado com sucesso !"});
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public async Task<ActionResult> PerfilCandidatoProcesso(string IdVaga)
        {
            var VerificaDados = new QuerryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var RecuperaDados = new QuerryMysql();
                var DadosProcesso = RecuperaDados.RecuperaCurriculosProcesso(IdVaga);
                TempData["IdVaga"] = IdVaga;
                TempData["TotalCurriculo"] = DadosProcesso.Count;

                for (int i = 0; i < DadosProcesso.Count; i++)
                {
                    if (DadosProcesso[i]["prova"] == null || Convert.ToDecimal(DadosProcesso[i]["prova"].Replace(',','.')) < 60)
                    {
                        TempData["ResultadoProva"+i] = DadosProcesso[i]["prova"];
                        TempData["EscondePsicologico"+i] = "hidden disabled";
                        TempData["EscondeGerente"+i] = "hidden disabled";
                    }
                    if (DadosProcesso[i]["psicologico"] == null)
                    {
                        TempData["ResultadoProva"+i] = DadosProcesso[i]["prova"];
                        TempData["ResultadoPsicologico" +i] = DadosProcesso[i]["psicologico"];
                        TempData["EscondeGerente"] = "hidden disabled";
                    }
                    else
                    {
                        TempData["ResultadoProva"+i] = DadosProcesso[i]["prova"];
                        TempData["ResultadoPsicologico"+i] = DadosProcesso[i]["psicologico"];
                        TempData["ResultadoGerente"+i] = DadosProcesso[i]["gerente"];
                        TempData["EscondePsicologico"] = "";
                        TempData["EscondeGerente"] = "";
                    }
                    if (DadosProcesso[i]["aprovado"].Equals("Aprovado") ||
                        DadosProcesso[i]["aprovado"].Equals("Excedente"))
                    {
                        TempData["EscondePsicologico" + i] = "";
                        TempData["EscondeGerente" + i] = "";
                    }
                    else
                    {
                        TempData["EscondePsicologico" + i] = "hidden disabled";
                        TempData["EscondeGerente" + i] = "hidden disabled";
                    }
                    TempData["Status"+i]= DadosProcesso[i]["aprovado"];
                    TempData["Restricao" + i] = DadosProcesso[i]["restricao"];
                    TempData["Cpf" + i] = DadosProcesso[i]["cpf"];

                    TempData["Email" + i] = DadosProcesso[i]["email"];
                    TempData["Nome" + i] = DadosProcesso[i]["nome"];
                }

                return PartialView("PerfilProcesso");
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult CadastrarAlerta(FormCollection Alerta)
        {
            var VerificaDados = new QuerryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var CadastrarAlertas = new QuerryMysql();

                CadastrarAlertas.CadastrarAlerta(Alerta["TextAlerta"]);
                TempData["Opcao"] = "Curriculo";

                return RedirectToAction("Principal",
                    new {Acao = "Curriculo", Mensagem = "Alerta cadastrado com sucesso !"});
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult CadastrarMensagem(FormCollection Mensagem)
        {
            var VerificaDados = new QuerryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var CadastrarMensagens = new QuerryMysql();

                CadastrarMensagens.CadastrarMensagem(Mensagem["TextMensagem"]);
                TempData["Opcao"] = "Curriculo";

                return RedirectToAction("Principal",
                    new {Acao = "Curriculo", Mensagem = "Mensagem cadastrada com sucesso !"});
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }


        public ActionResult EncerraProcesso(FormCollection Resultado)
        {
            var VerificaDados = new QuerryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Status = new QuerryMysql();
                for (int i = 0; i < Resultado.Count; i++)
                {

                    var Cpf = Resultado.Keys[i].ToString().Split(' ');
                    if (Cpf[0].Contains("Teorica"))
                    {

                        Status.AtualizaProcessoSeletivoTeorico(Cpf[1], Resultado["Vaga"], Resultado[i]);

                    }
                    if (Cpf[0].Contains("Gerencial"))
                    {
                        Status.AtualizarProcessoSeletivoGerente(Cpf[1], Resultado["Vaga"], Resultado[i]);
                    }
                    if (Cpf[0].Contains("Psicologico"))
                    {
                        Status.AtualizaProcessoSeletivoPsicologico(Cpf[1], Resultado["Vaga"], Resultado[i]);
                    }
                    if (Cpf[0].Contains("Status"))
                    {
                        Status.AtualizarProcessoSeletivoStatus(Cpf[1], Resultado["Vaga"], Resultado[i],Resultado["Restricao"+i]);
                        if (Resultado[i].Equals("Aprovado"))
                        {

                            var EmailCelular = Status.RecuperaEmail(Cpf[1]);
                            var EnvioSms = new string[1];
                            var certo = "55" + EmailCelular[0]["telefoneprincipal"].Replace(')', ' ').Replace('(', ' ').Replace('-', ' ');
                            EnvioSms[0] = certo.Replace(" ", "");
                            Configuration configuration = new Configuration("Divicred", "Euder17!");
                            SMSClient smsClient = new SMSClient(configuration);
                            SMSRequest smsRequest = new SMSRequest("Portal Sicoob Divicred", "Portal Sicoob Divicred, Parabén você foi aprovado para proxima etapa. ", EnvioSms);
                            SendMessageResult requestId = smsClient.SmsMessagingClient.SendSMS(smsRequest);

                            
                            Api.ApiKey = "edff66b8-adb7-461e-9f3f-fd1649cedefa";
                            string[] recipients = {EmailCelular[0]["email"] + "; rh@divicred.com.br"};
                            var subject = "Parabéns";
                            var fromEmail = "correio@divicred.com.br";
                            var fromName = "Sicoob Divicred";
                            var bodyText = "";
                            var bodyHtml = Api.Template.LoadTemplate(5393).BodyHtml;

                            ApiTypes.EmailSend result = null;

                            try
                            {
                                result = Api.Email.Send(subject, fromEmail, fromName, to: recipients,
                                    bodyText: bodyText, bodyHtml: bodyHtml);
                            }
                            catch
                            {
                            }
                            Status.CadastrarAlertaEspecifico(Resultado["Alerta"], EmailCelular[0]["id"]);
                        }
                        if (Resultado[i].Equals("Reprovado"))
                        {
                            var EmailCelular = Status.RecuperaEmail(Cpf[1]);
                            var EnvioSms = new string[1];
                            var certo = "55" + EmailCelular[0]["telefoneprincipal"].Replace(')', ' ').Replace('(', ' ').Replace('-', ' ');
                            EnvioSms[0] = certo.Replace(" ", "");

                            Configuration configuration = new Configuration("Divicred", "Euder17!");
                            SMSClient smsClient = new SMSClient(configuration);
                            SMSRequest smsRequest = new SMSRequest("Portal Sicoob Divicred", "Portal Sicoob Divicred, Infelizmente você não foi aprovado para proxima etapa.Agradecemos sua participacao.", EnvioSms);
                            SendMessageResult requestId = smsClient.SmsMessagingClient.SendSMS(smsRequest);


                            
                            Api.ApiKey = "edff66b8-adb7-461e-9f3f-fd1649cedefa";
                            string[] recipients = {EmailCelular[0]["email"] + "; rh@divicred.com.br"};
                            var subject = "Atualização de processo seletivo";
                            var fromEmail = "correio@divicred.com.br";
                            var fromName = "Sicoob Divicred";
                            var bodyText = "";
                            var bodyHtml = Api.Template.LoadTemplate(5394).BodyHtml;

                            ApiTypes.EmailSend result = null;

                            try
                            {
                                result = Api.Email.Send(subject, fromEmail, fromName, to: recipients,
                                    bodyText: bodyText, bodyHtml: bodyHtml);
                            }
                            catch
                            {
                            }
                        }
                        else
                        {
                            var EmailCelular = Status.RecuperaEmail(Cpf[1]);
                            var EnvioSms = new string[1];
                            var certo = "55" + EmailCelular[0]["telefoneprincipal"].Replace(')', ' ').Replace('(', ' ').Replace('-', ' ');
                            EnvioSms[0] = certo.Replace(" ", "");

                            Configuration configuration = new Configuration("Divicred", "Euder17!");
                            SMSClient smsClient = new SMSClient(configuration);
                            SMSRequest smsRequest = new SMSRequest("Portal Sicoob Divicred", "Portal Sicoob Divicred, Infelizmente você não foi aprovado para proxima etapa.Agradecemos sua participacao.", EnvioSms);
                            SendMessageResult requestId = smsClient.SmsMessagingClient.SendSMS(smsRequest);

                            
                            Api.ApiKey = "edff66b8-adb7-461e-9f3f-fd1649cedefa";
                            string[] recipients = {EmailCelular[0]["email"] + "; rh@divicred.com.br"};
                            var subject = "Atualização de processo seletivo";
                            var fromEmail = "correio@divicred.com.br";
                            var fromName = "Sicoob Divicred";
                            var bodyText = "";
                            var bodyHtml = Api.Template.LoadTemplate(5394).BodyHtml;

                            ApiTypes.EmailSend result = null;

                            try
                            {
                                result = Api.Email.Send(subject, fromEmail, fromName, to: recipients,
                                    bodyText: bodyText, bodyHtml: bodyHtml);
                            }
                            catch
                            {
                            }
                            Status.CriaBalao(Cpf[1]);

                        }
                    }

                }

                return RedirectToAction("Principal",
                    new {Acao = "Curriculo", Mensagem = "Processo seletivo encerrado com sucesso !"});
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult ResultadoProcesso(string IdVaga)
        {
            var VerificaDados = new QuerryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var RecuperaDados = new QuerryMysql();
                var DadosProcesso = RecuperaDados.RecuperaCurriculosProcesso(IdVaga);
                TempData["IdVaga"] = IdVaga;
                TempData["TotalCurriculo"] = DadosProcesso.Count;

                for (int i = 0; i < DadosProcesso.Count; i++)
                {
                    if (DadosProcesso[i]["aprovado"].Equals("Aprovado"))
                    {
                        TempData["Aprovado" + i] = "is-selected";
                    }

                    TempData["Cpf" + i] = DadosProcesso[i]["cpf"];

                    TempData["Email" + i] = DadosProcesso[i]["email"];
                    TempData["Nome" + i] = DadosProcesso[i]["nome"];
                }

                return PartialView("ResultadoProcesso");
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public async Task<ActionResult> FormularioInicial(string Cpf)
        {
            var VerificaDados = new QuerryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {

                var RecuperaDados = new QuerryMysql();
                var DadosUsuario = RecuperaDados.RecuperaDadosCandidato(Cpf);
                var DadosQuestionario = RecuperaDados.RecuperaQuestionario(DadosUsuario[0]["id"]);
                if (!DadosUsuario[0]["idarquivogoogle"].Equals("0"))
                {
                    TempData["ImagemPerfil"] = "https://portalsicoobdivicred.com.br/Uploads/" +
                                               DadosUsuario[0]["idarquivogoogle"];
                }
                else
                {
                    TempData["ImagemPerfil"] = "https://docs.google.com/uc?id=0B2CLuTO3N2_obWdkajEzTmpGeU0";
                }
                TempData["Nome"] = DadosUsuario[0]["nome"];
                TempData["EnderecoCompleto"] = DadosUsuario[0]["endereco"] + ", n°" + DadosUsuario[0]["numero"] + ", " +
                                               DadosUsuario[0]["bairro"] + ", " + DadosUsuario[0]["cidade"] + "-" +
                                               DadosUsuario[0]["uf"];
                TempData["NivelFormacao"] = DadosUsuario[0]["escolaridade"];

                var Nascimento = Convert.ToDateTime(DadosUsuario[0]["datanascimento"]);
                var Hoje = DateTime.Now;
                var Idade = DateTime.Now.Year - Convert.ToDateTime(DadosUsuario[0]["datanascimento"]).Year;
                if (Nascimento > Hoje.AddYears(-Idade)) Idade--;

                TempData["Idade"] = Idade + " Anos";
                TempData["Naturalidade"] = DadosUsuario[0]["cidade"];
                TempData["EstadoCivil"] = DadosUsuario[0]["descricaoestadocivil"];

                TempData["Questao1"] = DadosQuestionario[0]["questao1"];
                TempData["Questao2"] = DadosQuestionario[0]["questao2"];
                TempData["Questao3"] = DadosQuestionario[0]["questao3"];
                TempData["Questao4"] = DadosQuestionario[0]["questao4"];
                TempData["Questao5"] = DadosQuestionario[0]["questao5"];
                TempData["Questao6"] = DadosQuestionario[0]["questao6"];
                TempData["Questao7"] = DadosQuestionario[0]["questao7"];
                TempData["Questao8"] = DadosQuestionario[0]["questao8"];
                TempData["Questao9"] = DadosQuestionario[0]["questao9"];
                TempData["Questao10"] = DadosQuestionario[0]["questao10"];
                TempData["Questao11"] = DadosQuestionario[0]["questao11"];
                TempData["Questao12"] = DadosQuestionario[0]["questao12"];
                TempData["Questao13"] = DadosQuestionario[0]["questao13"];
                TempData["Questao14"] = DadosQuestionario[0]["questao14"];
                TempData["Questao15"] = DadosQuestionario[0]["questao15"];
                TempData["Questao16"] = DadosQuestionario[0]["questao16"];
                TempData["Questao17"] = DadosQuestionario[0]["questao17"];
                TempData["Questao18"] = DadosQuestionario[0]["questao18"];
                TempData["Questao19"] = DadosQuestionario[0]["questao19"];
                TempData["Questao20"] = DadosQuestionario[0]["questao20"];
                TempData["Questao21"] = DadosQuestionario[0]["questao21"];








                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult ImprimirTodos(string IdVaga)
        {
            var VerificaDados = new QuerryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var DadosCurriculos = VerificaDados.RecuperaCurriculosHistorico(IdVaga);
                var DadosUsuario = new Usuario[DadosCurriculos.Count];
                TempData["TotalCurriculos"] = DadosCurriculos.Count;



                for (int l = 0; l < DadosCurriculos.Count; l++)
                {
                    var DadosUsuarioBanco = VerificaDados.RecuperaDadosCandidato(DadosCurriculos[l]["cpf"]);
                    DadosUsuario[l] = new Usuario();
                    if (!DadosUsuarioBanco[0]["idarquivogoogle"].Equals("0"))
                    {
                        TempData["ImagemPerfil" + l] = "https://portalsicoobdivicred.com.br/Uploads/" +
                                                       DadosUsuarioBanco[0]["idarquivogoogle"];
                    }
                    else
                    {
                        TempData["ImagemPerfil" + l] = "https://docs.google.com/uc?id=0B2CLuTO3N2_obWdkajEzTmpGeU0";
                    }
                    var AreasInteresse = VerificaDados.RecuperaAreaInteresseUsuarios(DadosUsuarioBanco[0]["id"]);

                    DadosUsuario[l].NomeCompleto = DadosUsuarioBanco[0]["nome"];
                    DadosUsuario[l].TelefonePrincipal = DadosUsuarioBanco[0]["telefoneprincipal"];
                    DadosUsuario[l].TelefoneSecundario = DadosUsuarioBanco[0]["telefonesecundario"];
                    DadosUsuario[l].Cep = DadosUsuarioBanco[0]["cep"];
                    DadosUsuario[l].Rua = DadosUsuarioBanco[0]["endereco"];
                    DadosUsuario[l].Numero = DadosUsuarioBanco[0]["numero"];
                    DadosUsuario[l].Bairro = DadosUsuarioBanco[0]["bairro"];
                    DadosUsuario[l].Cidade = DadosUsuarioBanco[0]["cidade"];
                    DadosUsuario[l].Uf = DadosUsuarioBanco[0]["uf"];
                    DadosUsuario[l].Email = DadosUsuarioBanco[0]["email"];
                    DadosUsuario[l].TipoDeficiencia = DadosUsuarioBanco[0]["tipodeficiencia"];
                    DadosUsuario[l].Sexo = DadosUsuarioBanco[0]["sexo"];
                    DadosUsuario[l].Informatica = DadosUsuarioBanco[0]["informatica"];
                    DadosUsuario[l].Idade = Convert.ToDateTime(DadosUsuarioBanco[0]["datanascimento"]);
                    DadosUsuario[l].Cpf = DadosUsuarioBanco[0]["cpf"];
                    DadosUsuario[l].Identidade = DadosUsuarioBanco[0]["identidade"];
                    DadosUsuario[l].Complemento = DadosUsuarioBanco[0]["complemento"];
                    DadosUsuario[l].Resumo = DadosUsuarioBanco[0]["resumo"];
                    DadosUsuario[l].Certificacao = DadosUsuarioBanco[0]["certificacao"];
                    TempData["Conhecido" + l] = DadosUsuarioBanco[0]["conhecido"];


                    if (DadosUsuarioBanco[0]["disponibilidadeviagem"].Equals("S"))
                        TempData["Disponibilidades" + l] = "SIM";
                    else
                        TempData["Disponibilidades" + l] = "NÃO";

                    DadosUsuario[l].QuantidadeFilho = DadosUsuarioBanco[0]["quantidadefilhos"];
                    DadosUsuario[l].TipoDeficiencia = DadosUsuarioBanco[0]["tipodeficiencia"];
                    DadosUsuario[l].Cnh = DadosUsuarioBanco[0]["cnh"];
                    DadosUsuario[l].PrimeiraCnh = DadosUsuarioBanco[0]["dataprimeiracnh"];
                    DadosUsuario[l].CatCnh = DadosUsuarioBanco[0]["categoriacnh"];
                    TempData["estadocivil" + l] =
                        VerificaDados.RecuperaEstadoCivilCandidato(DadosUsuarioBanco[0]["estadocivil"]);
                    TempData["Escolaridade" + l] =
                        VerificaDados.RecuperaEscolaridadeCandidato(DadosUsuarioBanco[0]["idtipoescolaridade"]);
                    var Profissional = VerificaDados.RecuperaDadosUsuariosProissional(DadosUsuarioBanco[0]["id"]);

                    var NomeEmpresa = new List<string>();
                    var NomeCargo = new List<string>();
                    var DataEntrada = new List<string>();
                    var DataSaida = new List<string>();
                    var Atividades = new List<string>();
                    var EmpregoAtual = new List<string>();

                    for (var i = 0; i < Profissional.Count; i++)
                    {
                        NomeEmpresa.Add(Profissional[i]["nomeempresa"]);
                        NomeCargo.Add(Profissional[i]["nomecargo"]);
                        DataEntrada.Add(Convert.ToDateTime(Profissional[i]["dataentrada"]).Date.ToString());
                        DataSaida.Add(Convert.ToDateTime(Profissional[i]["datasaida"]).Date.ToString());
                        Atividades.Add(Profissional[i]["atividadedesempenhada"]);
                        if (Profissional[i]["empregoatual"].Equals("S"))
                            EmpregoAtual.Add("SIM");
                        else
                            EmpregoAtual.Add("NÃO");
                    }

                    DadosUsuario[l].NomeEmpresa = NomeEmpresa;
                    DadosUsuario[l].NomeCargo = NomeCargo;
                    DadosUsuario[l].DataEntrada = DataEntrada;
                    DadosUsuario[l].DataSaida = DataSaida;
                    DadosUsuario[l].Atividades = Atividades;
                    DadosUsuario[l].EmpregoAtual = EmpregoAtual;


                    var Educacional = VerificaDados.RecuperaDadosUsuariosEducacional(DadosUsuarioBanco[0]["id"]);

                    var NomeInstituicao = new List<string>();
                    var TipoFormacao = new List<string>();
                    var NomeCurso = new List<string>();
                    var AnoInicio = new List<string>();
                    var AnoFim = new List<string>();


                    for (var i = 0; i < Educacional.Count; i++)
                    {
                        NomeInstituicao.Add(Educacional[i]["nomeinstituicao"]);
                        NomeCurso.Add(Educacional[i]["nomecurso"]);
                        AnoInicio.Add(Educacional[i]["anoinicio"]);
                        AnoFim.Add(Educacional[i]["anofim"]);
                        TipoFormacao.Add(Educacional[i]["tipoformacao"]);
                    }

                    DadosUsuario[l].NomeInstituicao = NomeInstituicao;
                    DadosUsuario[l].NomeCurso = NomeCurso;
                    DadosUsuario[l].AnoInicio = AnoInicio;
                    DadosUsuario[l].AnoTermino = AnoFim;
                    DadosUsuario[l].TipoFormacao = TipoFormacao;


                    for (var i = 1; i <= 17; i++)
                        TempData["Area" + i] = "";

                    var Areas = AreasInteresse[0]["descricao"].Split(';');

                    TempData["Area" + l] = Areas;
                }
                return View("ImprimirTodos", DadosUsuario);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }


        public ActionResult CadastrarVagaEspecifica(Vagas Vaga)
        {
            var VerificaDados = new QuerryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var CadastrarVaga = new QuerryMysql();

                if (ModelState.IsValid)
                {
                    CadastrarVaga.CadastrarVaga(Vaga.Descricao, "Especifica", Vaga.Salario.Replace(",", "."),
                        Vaga.Requisitos,
                        Vaga.Titulo, Vaga.Beneficio);

                    TempData["Opcao"] = "Curriculo";

                    return RedirectToAction("Principal",
                        new {Acao = "Curriculo", Mensagem = "Vaga cadastrada com sucesso !"});
                }
                return RedirectToAction("Principal", new {Acao = "Curriculo"});
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult AtribuirVagaEspecifica(FormCollection Vaga)
        {
            var VerificaDados = new QuerryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var CadastrarVaga = new QuerryMysql();


                CadastrarVaga.AtribuirVaga(Vaga["CpfAtribui"], Vaga["VagaEspecifica"]);

                TempData["Opcao"] = "Curriculo";

                return RedirectToAction("Principal",
                    new {Acao = "Curriculo", Mensagem = "Atribuição feita com sucesso!"});

            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }


        public void SMS()
        {
            string Celular = "(37) 99988-3728";
            var EnvioSms = new string[1];
            var certo = "55" + Celular.Replace(')', ' ').Replace('(', ' ').Replace('-', ' ');
       EnvioSms[0] =certo.Replace(" ","");
        
        Configuration configuration = new Configuration("Divicred", "Euder17!");
        SMSClient smsClient = new SMSClient(configuration);
        SMSRequest smsRequest = new SMSRequest("Portal Sicoob Divicred", "Portal Sicoob Divicred, Novas vagas cadastradas. ", EnvioSms);
        SendMessageResult requestId = smsClient.SmsMessagingClient.SendSMS(smsRequest);


    }
    }
}