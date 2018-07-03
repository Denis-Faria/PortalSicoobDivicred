using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ElasticEmailClient;
using OneApi.Client.Impl;
using OneApi.Config;
using OneApi.Model;
using PortalSicoobDivicred.Aplicacao;
using PortalSicoobDivicred.Models;

namespace PortalSicoobDivicred.Controllers
{
    public class CurriculoController : Controller
    {
        public ActionResult Curriculo(string ordenacao, string mensagem)
        {
            var verificaDados = new QueryMysqlCurriculo();
            var queryFuncionario = new QueryMysql();
            var logado = verificaDados.UsuarioLogado();
            if (logado)
            {

                TempData["Mensagem"] = mensagem;
                var carregaDados = new QueryMysqlCurriculo();
                List<Dictionary<string, string>> dadosCurriculos = null;
                try
                {
                    if (ordenacao.Length == 0)
                        dadosCurriculos = carregaDados.RecuperaCurriculos();
                    if (ordenacao.Equals("Alfabetico"))
                        dadosCurriculos = carregaDados.RecuperaCurriculosAlfabetico();
                    if (ordenacao.Equals("Status"))
                        dadosCurriculos = carregaDados.RecuperaCurriculosStatus();
                }
                catch
                {
                    dadosCurriculos = carregaDados.RecuperaCurriculos();
                }

                var dadosVagas = carregaDados.RecuperaVagas();

                TempData["TotalVagas"] = dadosVagas.Count;
                if (dadosCurriculos != null)
                {
                    TempData["TotalCurriculo"] = dadosCurriculos.Count;

                    for (var i = 0; i < dadosCurriculos.Count; i++)
                    {
                        TempData["Nome" + i] = dadosCurriculos[i]["nome"];
                        TempData["Cpf" + i] = dadosCurriculos[i]["cpf"];
                        TempData["Email" + i] = dadosCurriculos[i]["email"];
                        TempData["Cidade" + i] = dadosCurriculos[i]["cidade"];
                        TempData["Area" + i] = dadosCurriculos[i]["descricao"].Replace(";", " ");
                        if (dadosCurriculos[i]["status"].Equals("S"))
                            TempData["Status" + i] = "green";
                        if (dadosCurriculos[i]["status"].Equals("N"))
                            TempData["Status" + i] = "red";
                        if (dadosCurriculos[i]["status"].Equals("E"))
                            TempData["Status" + i] = "blue";
                        if (dadosCurriculos[i]["status"].Equals("A"))
                            TempData["Status" + i] = "yellow";

                        if (dadosCurriculos[i]["idarquivogoogle"].Equals("0"))
                            TempData["Imagem" + i] = "https://docs.google.com/uc?id=0B2CLuTO3N2_obWdkajEzTmpGeU0";
                        else
                            TempData["Imagem" + i] = "https://portalsicoobdivicred.com.br/Uploads/" +
                                                     dadosCurriculos[i]["idarquivogoogle"] + "";
                    }
                }

                for (var i = 0; i < dadosVagas.Count; i++)
                {
                    TempData["IdVaga" + i] = dadosVagas[i]["id"];
                    TempData["Titulo" + i] = dadosVagas[i]["titulo"];
                    TempData["Descricao" + i] = dadosVagas[i]["descricao"];
                    TempData["AreaVaga" + i] = dadosVagas[i]["areadeinteresse"].Replace(";", " ");
                    if (dadosVagas[i]["ativa"].Equals("S"))
                        TempData["StatusVaga" + i] = "green";
                    else
                        TempData["StatusVaga" + i] = "red";
                }

                var cookie = Request.Cookies.Get("CookieFarm");

                if (cookie != null)
                {
                    var login = Criptografa.Descriptografar(cookie.Value);
                    if (queryFuncionario.PrimeiroLogin(login))
                        return RedirectToAction("FormularioCadastro", "Principal");
                    var dadosUsuarioBanco = queryFuncionario.RecuperaDadosUsuarios(login);

                    if (queryFuncionario.PermissaoCurriculos(dadosUsuarioBanco[0]["login"]))
                        TempData["PermissaoCurriculo"] =
                            " <a  href='javascript: Curriculo(); void(0); ' class='item' style='color: #38d5c5;' data-balloon='Curriculos' data-balloon-pos='right'><span class='icon'><i class='fa fa-book'></i></span><span class='name'></span></a>";
                    else
                        TempData["PermissaoCurriculo"] = "";
                    if (dadosUsuarioBanco[0]["gestor"].Equals("S"))
                    {
                        TempData["PermissaoGestor"] = "";
                        TempData["AreaGestor"] = "";
                    }
                    else
                    {
                        TempData["PermissaoGestor"] = "hidden";
                        TempData["AreaGestor"] = "hidden";
                    }


                    TempData["NomeLateral"] = dadosUsuarioBanco[0]["login"];
                    TempData["EmailLateral"] = dadosUsuarioBanco[0]["email"];
                    if (dadosUsuarioBanco[0]["foto"] == null)
                        TempData["ImagemPerfil"] = "https://docs.google.com/uc?id=0B2CLuTO3N2_obWdkajEzTmpGeU0";
                    else
                        TempData["ImagemPerfil"] = dadosUsuarioBanco[0]["foto"];
                }

                return View("Curriculo");
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult CurriculoArea(FormCollection filtros)
        {
            var verificaDados = new QueryMysqlCurriculo();
            var queryFuncionario = new QueryMysql();
            var logado = verificaDados.UsuarioLogado();
            if (logado)
            {
                var carregaDados = new QueryMysqlCurriculo();
                List<Dictionary<string, string>> dadosCurriculos;
                try
                {
                    if (filtros["FiltroArea"].Contains("Filtrar Área"))
                    {
                        if (filtros["FiltroFormacao"].Contains("Filtrar Formação"))
                            dadosCurriculos = carregaDados.RecuperaCurriculosArea("", filtros["FiltroCidade"],
                                filtros["FiltroCertificacao"], filtros["FiltroOrdenacao"], "");
                        else
                            dadosCurriculos = carregaDados.RecuperaCurriculosArea("", filtros["FiltroCidade"],
                                filtros["FiltroCertificacao"], filtros["FiltroOrdenacao"], filtros["FiltroFormacao"]);
                    }
                    else if (filtros["FiltroFormacao"].Contains("Filtrar Formação"))
                    {
                        dadosCurriculos = carregaDados.RecuperaCurriculosArea("", filtros["FiltroCidade"],
                            filtros["FiltroCertificacao"], filtros["FiltroOrdenacao"], "");
                    }
                    else
                    {
                        dadosCurriculos = carregaDados.RecuperaCurriculosArea(filtros["FiltroArea"],
                            filtros["FiltroCidade"], filtros["FiltroCertificacao"], filtros["FiltroOrdenacao"],
                            filtros["FiltroFormacao"]);
                    }
                }
                catch
                {
                    dadosCurriculos = carregaDados.RecuperaCurriculos();
                }

                var dadosVagas = carregaDados.RecuperaVagas();

                TempData["TotalVagas"] = dadosVagas.Count;
                TempData["TotalCurriculo"] = dadosCurriculos.Count;

                for (var i = 0; i < dadosCurriculos.Count; i++)
                {
                    TempData["Nome" + i] = dadosCurriculos[i]["nome"];
                    TempData["Cpf" + i] = dadosCurriculos[i]["cpf"];
                    TempData["Email" + i] = dadosCurriculos[i]["email"];
                    TempData["Cidade" + i] = dadosCurriculos[i]["cidade"];
                    TempData["Area" + i] = dadosCurriculos[i]["descricao"].Replace(";", " ");
                    if (dadosCurriculos[i]["status"].Equals("S"))
                        TempData["Status" + i] = "green";
                    if (dadosCurriculos[i]["status"].Equals("N"))
                        TempData["Status" + i] = "red";
                    if (dadosCurriculos[i]["status"].Equals("E"))
                        TempData["Status" + i] = "blue";
                    if (dadosCurriculos[i]["status"].Equals("A"))
                        TempData["Status" + i] = "yellow";

                    if (dadosCurriculos[i]["idarquivogoogle"].Equals("0"))
                        TempData["Imagem" + i] = "https://docs.google.com/uc?id=0B2CLuTO3N2_obWdkajEzTmpGeU0";
                    else
                        TempData["Imagem" + i] = "https://portalsicoobdivicred.com.br/Uploads/" +
                                                 dadosCurriculos[i]["idarquivogoogle"] + "";
                }

                for (var i = 0; i < dadosVagas.Count; i++)
                {
                    TempData["IdVaga" + i] = dadosVagas[i]["id"];
                    TempData["Titulo" + i] = dadosVagas[i]["titulo"];
                    TempData["Descricao" + i] = dadosVagas[i]["descricao"];
                    TempData["AreaVaga" + i] = dadosVagas[i]["areadeinteresse"].Replace(";", " ");
                    if (dadosVagas[i]["ativa"].Equals("S"))
                        TempData["StatusVaga" + i] = "green";
                    else
                        TempData["StatusVaga" + i] = "red";
                }

                var cookie = Request.Cookies.Get("CookieFarm");

                if (cookie != null)
                {
                    var login = Criptografa.Descriptografar(cookie.Value);
                    if (queryFuncionario.PrimeiroLogin(login))
                        return RedirectToAction("FormularioCadastro", "Principal");
                    var dadosUsuarioBanco = queryFuncionario.RecuperaDadosUsuarios(login);

                    if (queryFuncionario.PermissaoCurriculos(dadosUsuarioBanco[0]["login"]))
                        TempData["PermissaoCurriculo"] =
                            " <a  href='javascript: Curriculo(); void(0); ' class='item' style='color: #38d5c5;' data-balloon='Curriculos' data-balloon-pos='right'><span class='icon'><i class='fa fa-book'></i></span><span class='name'></span></a>";
                    else
                        TempData["PermissaoCurriculo"] = "";
                    if (dadosUsuarioBanco[0]["gestor"].Equals("S"))
                    {
                        TempData["PermissaoGestor"] = "";
                        TempData["AreaGestor"] = "";
                    }
                    else
                    {
                        TempData["PermissaoGestor"] = "hidden";
                        TempData["AreaGestor"] = "hidden";
                    }


                    TempData["NomeLateral"] = dadosUsuarioBanco[0]["login"];
                    TempData["EmailLateral"] = dadosUsuarioBanco[0]["email"];
                    if (dadosUsuarioBanco[0]["foto"] == null)
                        TempData["ImagemPerfil"] = "https://docs.google.com/uc?id=0B2CLuTO3N2_obWdkajEzTmpGeU0";
                    else
                        TempData["ImagemPerfil"] = dadosUsuarioBanco[0]["foto"];
                }

                return View("Curriculo");
            }

            return RedirectToAction("Login", "Login");
        }


        public ActionResult GerenciarVaga(string idVaga, string mensagem)
        {
            var verificaDados = new QueryMysqlCurriculo();
            var queryFuncionario = new QueryMysql();
            var logado = verificaDados.UsuarioLogado();
            if (logado)
            {
                TempData["Mensagem"] = mensagem;
                var recuperaDados = new QueryMysqlCurriculo();
                var dadosCurriculos = recuperaDados.RecuperaCurriculosHistorico(idVaga);
                TempData["TotalCurriculo"] = dadosCurriculos.Count;
                var dadosVagas = recuperaDados.RecuperaVagasId(idVaga);
                TempData["QuantidadeCurriculo"] = "N° de candidatos com interesse nesta vaga: " + dadosCurriculos.Count;
                TempData["TituloVaga"] = idVaga + "-" + dadosVagas[0]["titulo"];
                TempData["DescricaoVaga"] = dadosVagas[0]["descricao"];
                TempData["IdVaga"] = idVaga;
                var processoAberto = recuperaDados.VerificaProcesso(idVaga);

                if (processoAberto)
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


                for (var i = 0; i < dadosCurriculos.Count; i++)
                {
                    TempData["Nome" + i] = dadosCurriculos[i]["nome"];
                    TempData["Cpf" + i] = dadosCurriculos[i]["cpf"];
                    TempData["Email" + i] = dadosCurriculos[i]["email"];
                    TempData["Cidade" + i] = dadosCurriculos[i]["cidade"];
                    TempData["Certificacao" + i] = dadosCurriculos[i]["certificacao"];
                    if (dadosCurriculos[i]["idarquivogoogle"].Equals("0"))
                        TempData["Imagem" + i] = "https://docs.google.com/uc?id=0B2CLuTO3N2_obWdkajEzTmpGeU0";
                    else
                        TempData["Imagem" + i] = "https://portalsicoobdivicred.com.br/Uploads/" +
                                                 dadosCurriculos[i]["idarquivogoogle"] + "";
                }

                if (dadosVagas[0]["ativa"].Equals("S"))
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

                var cookie = Request.Cookies.Get("CookieFarm");

                if (cookie != null)
                {
                    var login = Criptografa.Descriptografar(cookie.Value);
                    if (queryFuncionario.PrimeiroLogin(login))
                        return RedirectToAction("FormularioCadastro", "Principal");
                    var dadosUsuarioBanco = queryFuncionario.RecuperaDadosUsuarios(login);

                    if (queryFuncionario.PermissaoCurriculos(dadosUsuarioBanco[0]["login"]))
                        TempData["PermissaoCurriculo"] =
                            " <a  href='javascript: Curriculo(); void(0); ' class='item' style='color: #38d5c5;' data-balloon='Curriculos' data-balloon-pos='right'><span class='icon'><i class='fa fa-book'></i></span><span class='name'></span></a>";
                    else
                        TempData["PermissaoCurriculo"] = "";
                    if (dadosUsuarioBanco[0]["gestor"].Equals("S"))
                    {
                        TempData["PermissaoGestor"] = "";
                        TempData["AreaGestor"] = "";
                    }
                    else
                    {
                        TempData["PermissaoGestor"] = "hidden";
                        TempData["AreaGestor"] = "hidden";
                    }


                    TempData["NomeLateral"] = dadosUsuarioBanco[0]["login"];
                    TempData["EmailLateral"] = dadosUsuarioBanco[0]["email"];
                    if (dadosUsuarioBanco[0]["foto"] == null)
                        TempData["ImagemPerfil"] = "https://docs.google.com/uc?id=0B2CLuTO3N2_obWdkajEzTmpGeU0";
                    else
                        TempData["ImagemPerfil"] = dadosUsuarioBanco[0]["foto"];
                }

                return View("GerenciarVaga");
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult CadastrarVaga(Vagas vaga, FormCollection vagas)
        {
            var verificaDados = new QueryMysqlCurriculo();
            var logado = verificaDados.UsuarioLogado();
            if (logado)
            {
                var cadastrarVaga = new QueryMysqlCurriculo();

                if (vagas.Count == 5)
                    ModelState.AddModelError("", "Favor selecionar uma área de interesse !");
                if (ModelState.IsValid)
                {
                    var areas = "";
                    for (var i = 6; i < vagas.Count; i++)
                        areas = areas + vagas[i] + ";";
                    var todosEmail = "";

                    var emailCelular = cadastrarVaga.CadastrarVaga(vaga.Descricao, areas,
                        vaga.Salario.Replace(",", "."),
                        vaga.Requisitos,
                        vaga.Titulo, vaga.Beneficio);
                    var envioSms = new string[emailCelular.Count];
                    for (var j = 0; j < emailCelular.Count; j++)
                    {
                        todosEmail = todosEmail + ";" + emailCelular[j]["email"];
                        var certo = "55" + emailCelular[j]["telefoneprincipal"].Replace(')', ' ').Replace('(', ' ')
                                        .Replace('-', ' ');
                        envioSms[j] = certo.Replace(" ", "");
                    }

                    var configuration = new Configuration("Divicred", "Euder17!");
                    var smsClient = new SMSClient(configuration);
                    var smsRequest = new SMSRequest("Portal Sicoob Divicred",
                        "Portal Sicoob Divicred, Novas vagas cadastradas.Entre e confira. ", envioSms);
                    var requestId = smsClient.SmsMessagingClient.SendSMS(smsRequest);


                    Api.ApiKey = "0db9e3fa-7f61-40c5-a4ed-bc6c4951fdcd";
                    string[] recipients = {todosEmail};
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

                    return RedirectToAction("Curriculo", "Curriculo", new {Mensagem = "Vaga cadastrada com sucesso !"});
                }

                return View("Curriculo", vaga);
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult PerfilCandidato(string cpf)
        {
            var verificaDados = new QueryMysqlCurriculo();
            var logado = verificaDados.UsuarioLogado();
            if (logado)
            {
                var vagasEspecificas = verificaDados.RecuperaVagaEspecifica();
                TempData["TotalVaga"] = vagasEspecificas.Count;
                for (var f = 0; f < vagasEspecificas.Count; f++)
                {
                    TempData["IdVaga" + f] = vagasEspecificas[f]["id"];
                    TempData["NomeVaga" + f] = vagasEspecificas[f]["descricao"];
                }

                var dadosUsuario = new Usuario();
                var dadosUsuarioBanco = verificaDados.RecuperaDadosCandidato(cpf);

                var dadosProcessosSeletivos = verificaDados.RecuperaProcesso(dadosUsuarioBanco[0]["id"]);
                var formulario = verificaDados.RecuperaFormulario(dadosUsuarioBanco[0]["id"]);

                if (formulario.Count > 0)
                {
                    TempData["ExisteFormulario"] = true;
                    TempData["IdFormulario"] = formulario[0]["id"];
                }
                else
                {
                    TempData["ExisteFormulario"] = false;
                    TempData["IdFormulario"] = 0;
                }

                if (dadosProcessosSeletivos.Count > 0)
                {
                    TempData["ExisteProcesso"] = true;
                    TempData["TotalProcessos"] = dadosProcessosSeletivos.Count;
                    for (var i = 0; i < dadosProcessosSeletivos.Count; i++)
                    {
                        TempData["NomeVaga" + i] = dadosProcessosSeletivos[i]["nomevaga"];
                        TempData["IdVaga" + i] = dadosProcessosSeletivos[i]["idvaga"];
                        TempData["Escrita" + i] = dadosProcessosSeletivos[i]["prova"];
                        try
                        {
                            if (Convert.ToDecimal(dadosProcessosSeletivos[i]["prova"].Replace(",", ".")) >= 60)
                            {
                                TempData["CorEscrita" + i] = "green";
                            }
                            else
                            {
                                TempData["CorEscrita" + i] = "red";
                                TempData["Aprovado" + i] = "red";
                            }
                        }
                        catch
                        {
                            TempData["CorEscrita" + i] = "red";
                            TempData["Aprovado" + i] = "red";
                        }
                        TempData["Psicologico" + i] = dadosProcessosSeletivos[i]["psicologico"];
                        try
                        {
                            if (dadosProcessosSeletivos[i]["psicologico"].Equals("Aprovado"))
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
                            TempData["Gerente" + i] = dadosProcessosSeletivos[i]["gerente"];
                            if (dadosProcessosSeletivos[i]["gerente"].Equals("Aprovado"))
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

                if (!dadosUsuarioBanco[0]["idarquivogoogle"].Equals("0"))
                    TempData["ImagemPerfil"] = "https://portalsicoobdivicred.com.br/Uploads/" +
                                               dadosUsuarioBanco[0]["idarquivogoogle"];
                else
                    TempData["ImagemPerfil"] = "https://docs.google.com/uc?id=0B2CLuTO3N2_obWdkajEzTmpGeU0";
                var areasInteresse = verificaDados.RecuperaAreaInteresseUsuarios(dadosUsuarioBanco[0]["id"]);

                dadosUsuario.NomeCompleto = dadosUsuarioBanco[0]["nome"];
                dadosUsuario.TelefonePrincipal = dadosUsuarioBanco[0]["telefoneprincipal"];
                dadosUsuario.TelefoneSecundario = dadosUsuarioBanco[0]["telefonesecundario"];
                dadosUsuario.Cep = dadosUsuarioBanco[0]["cep"];
                dadosUsuario.Rua = dadosUsuarioBanco[0]["endereco"];
                dadosUsuario.Numero = dadosUsuarioBanco[0]["numero"];
                dadosUsuario.Bairro = dadosUsuarioBanco[0]["bairro"];
                dadosUsuario.Cidade = dadosUsuarioBanco[0]["cidade"];
                dadosUsuario.Uf = dadosUsuarioBanco[0]["uf"];
                dadosUsuario.Email = dadosUsuarioBanco[0]["email"];
                dadosUsuario.TipoDeficiencia = dadosUsuarioBanco[0]["tipodeficiencia"];
                dadosUsuario.Sexo = dadosUsuarioBanco[0]["sexo"];
                dadosUsuario.Informatica = dadosUsuarioBanco[0]["informatica"];
                dadosUsuario.Idade = Convert.ToDateTime(dadosUsuarioBanco[0]["datanascimento"]);
                dadosUsuario.Cpf = dadosUsuarioBanco[0]["cpf"];
                dadosUsuario.Identidade = dadosUsuarioBanco[0]["identidade"];
                dadosUsuario.Complemento = dadosUsuarioBanco[0]["complemento"];
                dadosUsuario.Resumo = dadosUsuarioBanco[0]["resumo"];
                dadosUsuario.Certificacao = dadosUsuarioBanco[0]["certificacao"];
                TempData["Conhecido"] = dadosUsuarioBanco[0]["conhecido"];


                if (dadosUsuarioBanco[0]["disponibilidadeviagem"].Equals("S"))
                    TempData["Disponibilidades"] = "SIM";
                else
                    TempData["Disponibilidades"] = "NÃO";

                dadosUsuario.QuantidadeFilho = dadosUsuarioBanco[0]["quantidadefilhos"];
                dadosUsuario.TipoDeficiencia = dadosUsuarioBanco[0]["tipodeficiencia"];
                dadosUsuario.Cnh = dadosUsuarioBanco[0]["cnh"];
                dadosUsuario.PrimeiraCnh = dadosUsuarioBanco[0]["dataprimeiracnh"];
                dadosUsuario.CatCnh = dadosUsuarioBanco[0]["categoriacnh"];
                TempData["estadocivil"] =
                    verificaDados.RecuperaEstadoCivilCandidato(dadosUsuarioBanco[0]["estadocivil"]);
                TempData["Escolaridade"] =
                    verificaDados.RecuperaEscolaridadeCandidato(dadosUsuarioBanco[0]["idtipoescolaridade"]);
                var profissional = verificaDados.RecuperaDadosUsuariosProissional(dadosUsuarioBanco[0]["id"]);

                var nomeEmpresa = new List<string>();
                var nomeCargo = new List<string>();
                var dataEntrada = new List<string>();
                var dataSaida = new List<string>();
                var atividades = new List<string>();

                for (var i = 0; i < profissional.Count; i++)
                {
                    nomeEmpresa.Add(profissional[i]["nomeempresa"]);
                    nomeCargo.Add(profissional[i]["nomecargo"]);
                    dataEntrada.Add(Convert.ToDateTime(profissional[i]["dataentrada"]).Date.ToString());
                    dataSaida.Add(Convert.ToDateTime(profissional[i]["datasaida"]).Date.ToString());
                    atividades.Add(profissional[i]["atividadedesempenhada"]);
                    if (profissional[i]["empregoatual"].Equals("S"))
                        TempData["EmpregoAtual" + i] = "SIM";
                    else
                        TempData["EmpregoAtual" + i] = "NÃO";
                }

                dadosUsuario.NomeEmpresa = nomeEmpresa;
                dadosUsuario.NomeCargo = nomeCargo;
                dadosUsuario.DataEntrada = dataEntrada;
                dadosUsuario.DataSaida = dataSaida;
                dadosUsuario.Atividades = atividades;


                var educacional = verificaDados.RecuperaDadosUsuariosEducacional(dadosUsuarioBanco[0]["id"]);

                var nomeInstituicao = new List<string>();
                var tipoFormacao = new List<string>();
                var nomeCurso = new List<string>();
                var anoInicio = new List<string>();
                var anoFim = new List<string>();


                for (var i = 0; i < educacional.Count; i++)
                {
                    nomeInstituicao.Add(educacional[i]["nomeinstituicao"]);
                    nomeCurso.Add(educacional[i]["nomecurso"]);
                    anoInicio.Add(educacional[i]["anoinicio"]);
                    anoFim.Add(educacional[i]["anofim"]);
                    tipoFormacao.Add(educacional[i]["tipoformacao"]);
                }

                dadosUsuario.NomeInstituicao = nomeInstituicao;
                dadosUsuario.NomeCurso = nomeCurso;
                dadosUsuario.AnoInicio = anoInicio;
                dadosUsuario.AnoTermino = anoFim;
                dadosUsuario.TipoFormacao = tipoFormacao;


                for (var i = 1; i <= 17; i++)
                    TempData["Area" + i] = "";

                var areas = areasInteresse[0]["descricao"].Split(';');

                TempData["Area"] = areas;

                return PartialView("ModalPerfil", dadosUsuario);
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult FiltrarPerfilVaga(FormCollection filtros)
        {
            var verificaDados = new QueryMysqlCurriculo();
            var logado = verificaDados.UsuarioLogado();
            if (logado)
            {
                var filtrar = new QueryMysqlCurriculo();
                var sexo = filtros["FiltroSexo"];
                var cidade = filtros["FiltroCidade"];
                var graduacao = filtros["FiltroGraduacao"];
                var anoFormacao = filtros["FiltroAnoFormacao"];
                var faixaEtaria = filtros["FiltroFaixaEtaria"];
                var certificacao = filtros["FiltroCertificacao"];
                var cursoGraduacao = filtros["FiltroCurso"];
                var profissional = filtros["FiltroProfissional"];
                var idVaga = filtros["TituloVaga"].Split('-');
                var query =
                    "select a.cpf,a.nome,a.email, a.idarquivogoogle,a.cidade,a.certificacao from historicos c LEFT JOIN candidatos a on c.idcandidato=a.id INNER JOIN candidatos u2 on (c.idcandidato=u2.id) where c.idvaga=" +
                    idVaga[0] + "";

                if (!sexo.Equals("Sexo"))
                    query = query + " AND a.sexo = '" + sexo + "'";
                if (cidade.Length > 0)
                    query = query + " AND a.cidade like'%" + cidade + "%'";
                if (certificacao.Length > 0)
                    query = query + " AND a.certificacao like'%" + certificacao + "%'";
                if (!faixaEtaria.Equals("Faixa Etária"))
                    if (faixaEtaria.Contains("< 18"))
                        query = query + " AND  YEAR(FROM_DAYS(TO_DAYS(NOW())-TO_DAYS(a.datanascimento)))<18";
                    else if (faixaEtaria.Contains("18 - 25"))
                        query = query +
                                " AND  YEAR(FROM_DAYS(TO_DAYS(NOW())-TO_DAYS(a.datanascimento))) BETWEEN 18 and 25";
                    else if (faixaEtaria.Contains("25 - 30"))
                        query = query +
                                " AND  YEAR(FROM_DAYS(TO_DAYS(NOW())-TO_DAYS(a.datanascimento))) BETWEEN 25 and 30";
                    else if (faixaEtaria.Contains("30 - 40"))
                        query = query +
                                " AND  YEAR(FROM_DAYS(TO_DAYS(NOW())-TO_DAYS(a.datanascimento))) BETWEEN 30 and 40";
                    else if (faixaEtaria.Contains(" 40 > "))
                        query = query + " AND  YEAR(FROM_DAYS(TO_DAYS(NOW())-TO_DAYS(a.datanascimento)))>40";
                if (!graduacao.Equals("Graduação"))
                {
                    var escolaridade = graduacao.Split('|');
                    var idsEscolaridade = filtrar.RecuperaIdEscolaridade(escolaridade[0]);
                    var escolaridades = " and (a.idtipoescolaridade=" + idsEscolaridade[0]["id"];

                    for (var i = 1; i < idsEscolaridade.Count; i++)
                        escolaridades = escolaridades + " or a.idtipoescolaridade=" + idsEscolaridade[i]["id"];
                    query = query + escolaridades + ")";
                }

                if (anoFormacao.Length > 0)
                    query = query + " AND (SELECT count(id) FROM dadosescolares where anofim <=" + anoFormacao +
                            " and idcandidato=a.id)>=1";
                if (cursoGraduacao.Length > 0)
                    query = query + " AND (SELECT count(id) FROM dadosescolares where nomecurso like'%" +
                            cursoGraduacao +
                            "%' and idcandidato=a.id)>=1";
                if (profissional.Length > 0)
                    query = query +
                            " AND (SELECT count(id) FROM dadosprofissionais where atividadedesempenhada like'%" +
                            profissional + "%' and idcandidato=a.id)>=1";
                var dadosCurriculos = filtrar.FiltroVaga(query);

                var dadosVagas = filtrar.RecuperaVagasId(idVaga[0]);
                TempData["IdVaga"] = idVaga[0];
                TempData["TituloVaga"] = idVaga[0] + "-" + dadosVagas[0]["titulo"];
                TempData["DescricaoVaga"] = dadosVagas[0]["descricao"];
                TempData["TotalCurriculo"] = dadosCurriculos.Count;
                TempData["QuantidadeCurriculo"] = "N° de candidatos com interesse nesta vaga: " + dadosCurriculos.Count;

                var processoAberto = filtrar.VerificaProcesso(idVaga[0]);

                if (processoAberto)
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


                for (var i = 0; i < dadosCurriculos.Count; i++)
                {
                    TempData["Nome" + i] = dadosCurriculos[i]["nome"];
                    TempData["Cpf" + i] = dadosCurriculos[i]["cpf"];
                    TempData["Email" + i] = dadosCurriculos[i]["email"];
                    TempData["Cidade" + i] = dadosCurriculos[i]["cidade"];
                    TempData["Certificacao" + i] = dadosCurriculos[i]["certificacao"];
                    if (dadosCurriculos[i]["idarquivogoogle"].Equals("0"))
                        TempData["Imagem" + i] = "https://docs.google.com/uc?id=0B2CLuTO3N2_obWdkajEzTmpGeU0";
                    else
                        TempData["Imagem" + i] = "https://portalsicoobdivicred.com.br/Uploads/" +
                                                 dadosCurriculos[i]["idarquivogoogle"] + "";
                }

                if (dadosVagas[0]["ativa"].Equals("S"))
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

                return View("GerenciarVaga");
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult EncerrarVaga(string idVaga)
        {
            var verificaDados = new QueryMysqlCurriculo();
            var logado = verificaDados.UsuarioLogado();
            if (logado)
            {
                var RecuperaDados = new QueryMysqlCurriculo();
                var Vaga = idVaga.Split('-');
                RecuperaDados.EncerrarVaga(Vaga[0]);
                return RedirectToAction("GerenciarVaga", "Curriculo",
                    new {IdVaga = idVaga, Mensagem = "Vaga encerrada com sucesso !"});
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult AbrirProcesso(FormCollection curriculos)
        {
            var verificaDados = new QueryMysqlCurriculo();
            var logado = verificaDados.UsuarioLogado();
            if (logado)
            {
                var idVaga = curriculos["vaga"];
                var vaga = idVaga.Split(',');
                var iniciarProcesso = new QueryMysqlCurriculo();
                for (var i = 0; i < curriculos.Count; i++)
                    if (!curriculos.Keys[i].Contains("vaga") && !curriculos.Keys[i].Equals("Alerta"))
                    {
                        iniciarProcesso.IniciarProcessoSeletivo(curriculos.Keys[i], vaga[0]);
                        var EmailCelular = iniciarProcesso.RecuperaEmail(curriculos.Keys[i]);
                        var Certo = "";
                        if (EmailCelular[0]["telefoneprincipal"] != null)
                            Certo = "55" + EmailCelular[0]["telefoneprincipal"].Replace(')', ' ').Replace('(', ' ')
                                        .Replace('-', ' ');
                        var configuration = new Configuration("Divicred", "Euder17!");
                        var smsClient = new SMSClient(configuration);
                        var smsRequest = new SMSRequest("Portal Sicoob Divicred", curriculos["Alerta"],
                            Certo.Replace(" ", ""));
                        var requestId = smsClient.SmsMessagingClient.SendSMS(smsRequest);

                        Api.ApiKey = "0db9e3fa-7f61-40c5-a4ed-bc6c4951fdcd";
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

                        iniciarProcesso.CadastrarAlertaEspecifico(curriculos["Alerta"], EmailCelular[0]["id"]);
                    }

                var dadosCurriculos = iniciarProcesso.RecuperaCurriculosHistorico(vaga[0]);
                var emails = "";
                var smsNegado = new string[dadosCurriculos.Count];
                var count = 0;
                for (var j = 0; j < dadosCurriculos.Count; j++)
                    if (!curriculos.AllKeys.Contains(dadosCurriculos[j]["cpf"]))
                    {
                        emails = emails + ";" + dadosCurriculos[j]["email"];
                        if (dadosCurriculos[j]["telefoneprincipal"] != null ||
                            dadosCurriculos[j]["telefoneprincipal"].Length > 0)
                        {
                            var telefoneCerto = "55" + dadosCurriculos[j]["telefoneprincipal"].Replace(')', ' ')
                                                    .Replace('(', ' ').Replace('-', ' ');
                            smsNegado[count] = telefoneCerto.Replace(" ", "");
                            count++;
                        }
                    }


                /*Configuration configuration2 = new Configuration("Divicred", "Euder17!");
                SMSClient smsClient2 = new SMSClient(configuration2);
                SMSRequest smsRequest2 = new SMSRequest("Portal Sicoob Divicred","Portal Sicoob Divicred. Infelizmente você não foi selecionado para proxima etapa do processo seletivo.", SmsNegado);
                SendMessageResult requestId2 = smsClient2.SmsMessagingClient.SendSMS(smsRequest2);*/

                Api.ApiKey = "0db9e3fa-7f61-40c5-a4ed-bc6c4951fdcd";
                string[] recipients2 = {emails};
                var subject2 = "Atualização de Status";
                var fromEmail2 = "correio@divicred.com.br";
                var fromName2 = "Sicoob Divicred";
                var bodyText2 = "";
                var bodyHtml2 = Api.Template.LoadTemplate(5394).BodyHtml;

                try
                {
                    Api.Email.Send(subject2, fromEmail2, fromName2, to: recipients2,
                        bodyText: bodyText2, bodyHtml: bodyHtml2);
                }
                catch
                {
                    // ignored
                }


                return RedirectToAction("GerenciarVaga", "Curriculo",
                    new {IdVaga = vaga[0], Mensagem = "Processo seletivo aberto sucesso !"});
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult PerfilCandidatoProcesso(string IdVaga)
        {
            var verificaDados = new QueryMysqlCurriculo();
            var logado = verificaDados.UsuarioLogado();
            if (logado)
            {
                var recuperaDados = new QueryMysqlCurriculo();
                var dadosProcesso = recuperaDados.RecuperaCurriculosProcesso(IdVaga);
                TempData["IdVaga"] = IdVaga;
                TempData["TotalCurriculo"] = dadosProcesso.Count;

                for (var i = 0; i < dadosProcesso.Count; i++)
                {
                    var prova = dadosProcesso[i]["prova"];
                    if (prova.Equals(""))
                    {
                        TempData["ResultadoProva" + i] = dadosProcesso[i]["prova"];
                        TempData["EscondePsicologico" + i] = "hidden disabled";
                        TempData["EscondeGerente" + i] = "hidden disabled";
                    }
                    else if (Convert.ToDecimal(prova.Replace(",", ".")) < 60)
                    {
                        TempData["ResultadoProva" + i] = dadosProcesso[i]["prova"];
                        TempData["EscondePsicologico" + i] = "hidden disabled";
                        TempData["EscondeGerente" + i] = "hidden disabled";
                    }

                    if (dadosProcesso[i]["psicologico"] == null)
                    {
                        TempData["ResultadoProva" + i] = dadosProcesso[i]["prova"];
                        TempData["ResultadoPsicologico" + i] = dadosProcesso[i]["psicologico"];
                        TempData["ResultadoGerente" + i] = dadosProcesso[i]["gerente"];
                        TempData["EscondeGerente"] = "hidden disabled";
                        TempData["EscondeGerente"] = "";
                    }
                    else
                    {
                        TempData["ResultadoProva" + i] = dadosProcesso[i]["prova"];
                        TempData["ResultadoPsicologico" + i] = dadosProcesso[i]["psicologico"];
                        TempData["EscondePsicologico"] = "";
                    }

                    try
                    {
                        if (dadosProcesso[i]["aprovado"].Equals("Aprovado") ||
                            dadosProcesso[i]["aprovado"].Equals("Excedente")||
                            dadosProcesso[i]["aprovado"] == null)
                        {
                            TempData["EscondePsicologico" + i] = "";
                            TempData["EscondeGerente" + i] = "";
                        }
                        else
                        {
                            TempData["EscondePsicologico" + i] = "hidden disabled";
                            TempData["EscondeGerente" + i] = "hidden disabled";
                        }
                    }
                    catch
                    {
                        TempData["EscondePsicologico" + i] = "";
                        TempData["EscondeGerente" + i] = "";
                    }

                    if (dadosProcesso[i]["aprovado"] == null)
                        TempData["Status" + i] = "";
                    else
                        TempData["Status" + i] = dadosProcesso[i]["aprovado"];

                    TempData["Restricao" + i] = dadosProcesso[i]["restricao"];
                    TempData["Cpf" + i] = dadosProcesso[i]["cpf"];

                    TempData["Email" + i] = dadosProcesso[i]["email"];
                    TempData["Nome" + i] = dadosProcesso[i]["nome"];
                }

                return PartialView("PerfilProcesso");
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult CadastrarAlerta(FormCollection Alerta)
        {
            var verificaDados = new QueryMysqlCurriculo();
            var Logado = verificaDados.UsuarioLogado();
            if (Logado)
            {
                var CadastrarAlertas = new QueryMysqlCurriculo();

                CadastrarAlertas.CadastrarAlerta(Alerta["TextAlerta"]);
                TempData["Opcao"] = "Curriculo";

                return RedirectToAction("Curriculo", "Curriculo", new {Mensagem = "Alerta cadastrado com sucesso !"});
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult CadastrarMensagem(FormCollection Mensagem)
        {
            var VerificaDados = new QueryMysqlCurriculo();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var CadastrarMensagens = new QueryMysqlCurriculo();

                CadastrarMensagens.CadastrarMensagem(Mensagem["TextMensagem"]);
                TempData["Opcao"] = "Curriculo";

                return RedirectToAction("Curriculo", "Curriculo", new {Mensagem = "Mensagem cadastrada com sucesso !"});
            }

            return RedirectToAction("Login", "Login");
        }


        public ActionResult EncerraProcesso(FormCollection Resultado)
        {
            var VerificaDados = new QueryMysqlCurriculo();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Status = new QueryMysqlCurriculo();
                for (var i = 0; i < Resultado.Count; i++)
                {
                    var Cpf = Resultado.Keys[i].Split(' ');
                    if (Cpf[0].Contains("Teorica"))
                        Status.AtualizaProcessoSeletivoTeorico(Cpf[1], Resultado["Vaga"], Resultado[i]);
                    if (Cpf[0].Contains("Gerencial"))
                        Status.AtualizarProcessoSeletivoGerente(Cpf[1], Resultado["Vaga"], Resultado[i]);
                    if (Cpf[0].Contains("Psicologico"))
                        Status.AtualizaProcessoSeletivoPsicologico(Cpf[1], Resultado["Vaga"], Resultado[i]);
                    if (Cpf[0].Contains("Status"))
                    {
                        Status.AtualizarProcessoSeletivoStatus(Cpf[1], Resultado["Vaga"], Resultado[i],
                            Resultado["Restricao" + i]);
                        if (Resultado[i].Equals("Aprovado"))
                        {
                            var EmailCelular = Status.RecuperaEmail(Cpf[1]);
                            var EnvioSms = new string[1];
                            var certo = "55" + EmailCelular[0]["telefoneprincipal"].Replace(')', ' ').Replace('(', ' ')
                                            .Replace('-', ' ');
                            EnvioSms[0] = certo.Replace(" ", "");
                            var configuration = new Configuration("Divicred", "Euder17!");
                            var smsClient = new SMSClient(configuration);
                            var smsRequest = new SMSRequest("Portal Sicoob Divicred",
                                "Portal Sicoob Divicred, Parabén você foi aprovado para proxima etapa. ", EnvioSms);
                            var requestId = smsClient.SmsMessagingClient.SendSMS(smsRequest);


                            Api.ApiKey = "0db9e3fa-7f61-40c5-a4ed-bc6c4951fdcd";
                            string[] recipients = {EmailCelular[0]["email"] + ";"};
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
                        else if (Resultado[i].Equals("Reprovado"))
                        {
                            var EmailCelular = Status.RecuperaEmail(Cpf[1]);
                            var EnvioSms = new string[1];
                            var certo = "55" + EmailCelular[0]["telefoneprincipal"].Replace(')', ' ').Replace('(', ' ')
                                            .Replace('-', ' ');
                            EnvioSms[0] = certo.Replace(" ", "");

                            var configuration = new Configuration("Divicred", "Euder17!");
                            var smsClient = new SMSClient(configuration);
                            var smsRequest = new SMSRequest("Portal Sicoob Divicred",
                                "Portal Sicoob Divicred, Infelizmente você não foi aprovado para proxima etapa.Agradecemos sua participacao.",
                                EnvioSms);
                            var requestId = smsClient.SmsMessagingClient.SendSMS(smsRequest);


                            Api.ApiKey = "0db9e3fa-7f61-40c5-a4ed-bc6c4951fdcd";
                            string[] recipients = {EmailCelular[0]["email"] + ";"};
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
                            var certo = "55" + EmailCelular[0]["telefoneprincipal"].Replace(')', ' ').Replace('(', ' ')
                                            .Replace('-', ' ');
                            EnvioSms[0] = certo.Replace(" ", "");

                            var configuration = new Configuration("Divicred", "Euder17!");
                            var smsClient = new SMSClient(configuration);
                            var smsRequest = new SMSRequest("Portal Sicoob Divicred",
                                "Portal Sicoob Divicred, Infelizmente você não foi aprovado para proxima etapa.Agradecemos sua participacao.",
                                EnvioSms);
                            var requestId = smsClient.SmsMessagingClient.SendSMS(smsRequest);


                            Api.ApiKey = "0db9e3fa-7f61-40c5-a4ed-bc6c4951fdcd";
                            string[] recipients = {EmailCelular[0]["email"] + ";"};
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

                return RedirectToAction("GerenciarVaga", "Curriculo",
                    new {IdVaga = Resultado["Vaga"], Mensagem = "Processo seleivo encerrado com sucesso !"});
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult ResultadoProcesso(string idVaga)
        {
            var verificaDados = new QueryMysqlCurriculo();
            var logado = verificaDados.UsuarioLogado();
            if (logado)
            {
                var recuperaDados = new QueryMysqlCurriculo();
                var dadosProcesso = recuperaDados.RecuperaCurriculosProcesso(idVaga);
                TempData["IdVaga"] = idVaga;
                TempData["TotalCurriculo"] = dadosProcesso.Count;

                for (var i = 0; i < dadosProcesso.Count; i++)
                {
                    if (dadosProcesso[i]["aprovado"].Equals("Aprovado"))
                        TempData["Aprovado" + i] = "is-selected";

                    TempData["Cpf" + i] = dadosProcesso[i]["cpf"];

                    TempData["Email" + i] = dadosProcesso[i]["email"];
                    TempData["Nome" + i] = dadosProcesso[i]["nome"];
                }

                return PartialView("ResultadoProcesso");
            }

            return RedirectToAction("Login", "Login");
        }


        public ActionResult FormularioInicial(string cpf)
        {
            var verificaDados = new QueryMysqlCurriculo();
            var logado = verificaDados.UsuarioLogado();
            if (logado)
            {
                var recuperaDados = new QueryMysqlCurriculo();
                var dadosUsuario = recuperaDados.RecuperaDadosCandidato(cpf);
                var dadosQuestionario = recuperaDados.RecuperaQuestionario(dadosUsuario[0]["id"]);
                if (!dadosUsuario[0]["idarquivogoogle"].Equals("0"))
                    TempData["ImagemPerfil"] = "https://portalsicoobdivicred.com.br/Uploads/" +
                                               dadosUsuario[0]["idarquivogoogle"];
                else
                    TempData["ImagemPerfil"] = "https://docs.google.com/uc?id=0B2CLuTO3N2_obWdkajEzTmpGeU0";
                TempData["Nome"] = dadosUsuario[0]["nome"];
                TempData["EnderecoCompleto"] = dadosUsuario[0]["endereco"] + ", n°" + dadosUsuario[0]["numero"] + ", " +
                                               dadosUsuario[0]["bairro"] + ", " + dadosUsuario[0]["cidade"] + "-" +
                                               dadosUsuario[0]["uf"];
                TempData["NivelFormacao"] = dadosUsuario[0]["escolaridade"];

                var Nascimento = Convert.ToDateTime(dadosUsuario[0]["datanascimento"]);
                var Hoje = DateTime.Now;
                var Idade = DateTime.Now.Year - Convert.ToDateTime(dadosUsuario[0]["datanascimento"]).Year;
                if (Nascimento > Hoje.AddYears(-Idade)) Idade--;

                TempData["Idade"] = Idade + " Anos";
                TempData["Naturalidade"] = dadosUsuario[0]["cidade"];
                TempData["EstadoCivil"] = dadosUsuario[0]["descricaoestadocivil"];

                TempData["Questao1"] = dadosQuestionario[0]["questao1"];
                TempData["Questao2"] = dadosQuestionario[0]["questao2"];
                TempData["Questao3"] = dadosQuestionario[0]["questao3"];
                TempData["Questao4"] = dadosQuestionario[0]["questao4"];
                TempData["Questao5"] = dadosQuestionario[0]["questao5"];
                TempData["Questao6"] = dadosQuestionario[0]["questao6"];
                TempData["Questao7"] = dadosQuestionario[0]["questao7"];
                TempData["Questao8"] = dadosQuestionario[0]["questao8"];
                TempData["Questao9"] = dadosQuestionario[0]["questao9"];
                TempData["Questao10"] = dadosQuestionario[0]["questao10"];
                TempData["Questao11"] = dadosQuestionario[0]["questao11"];
                TempData["Questao12"] = dadosQuestionario[0]["questao12"];
                TempData["Questao13"] = dadosQuestionario[0]["questao13"];
                TempData["Questao14"] = dadosQuestionario[0]["questao14"];
                TempData["Questao15"] = dadosQuestionario[0]["questao15"];
                TempData["Questao16"] = dadosQuestionario[0]["questao16"];
                TempData["Questao17"] = dadosQuestionario[0]["questao17"];
                TempData["Questao18"] = dadosQuestionario[0]["questao18"];
                TempData["Questao19"] = dadosQuestionario[0]["questao19"];
                TempData["Questao20"] = dadosQuestionario[0]["questao20"];
                TempData["Questao21"] = dadosQuestionario[0]["questao21"];


                return View();
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult ImprimirTodos(string IdVaga)
        {
            var VerificaDados = new QueryMysqlCurriculo();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var DadosCurriculos = VerificaDados.RecuperaCurriculosHistorico(IdVaga);
                var DadosUsuario = new Usuario[DadosCurriculos.Count];
                TempData["TotalCurriculos"] = DadosCurriculos.Count;


                for (var l = 0; l < DadosCurriculos.Count; l++)
                {
                    var DadosUsuarioBanco = VerificaDados.RecuperaDadosCandidato(DadosCurriculos[l]["cpf"]);
                    DadosUsuario[l] = new Usuario();
                    if (!DadosUsuarioBanco[0]["idarquivogoogle"].Equals("0"))
                        TempData["ImagemPerfil" + l] = "https://portalsicoobdivicred.com.br/Uploads/" +
                                                       DadosUsuarioBanco[0]["idarquivogoogle"];
                    else
                        TempData["ImagemPerfil" + l] = "https://docs.google.com/uc?id=0B2CLuTO3N2_obWdkajEzTmpGeU0";
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

            return RedirectToAction("Login", "Login");
        }


        public ActionResult CadastrarVagaEspecifica(Vagas Vaga)
        {
            var VerificaDados = new QueryMysqlCurriculo();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var CadastrarVaga = new QueryMysqlCurriculo();

                if (ModelState.IsValid)
                {
                    CadastrarVaga.CadastrarVaga(Vaga.Descricao, "Especifica", Vaga.Salario.Replace(",", "."),
                        Vaga.Requisitos,
                        Vaga.Titulo, Vaga.Beneficio);

                    TempData["Opcao"] = "Curriculo";

                    return RedirectToAction("Curriculo", "Curriculo",
                        new {Mensagem = "Vaga específica cadastrada com sucesso !"});
                }

                return RedirectToAction("Curriculo");
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult AtribuirVagaEspecifica(FormCollection Vaga)
        {
            var VerificaDados = new QueryMysqlCurriculo();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var CadastrarVaga = new QueryMysqlCurriculo();


                CadastrarVaga.AtribuirVaga(Vaga["CpfAtribui"], Vaga["VagaEspecifica"]);

                TempData["Opcao"] = "Curriculo";

                return RedirectToAction("Curriculo", "Curriculo", new {Mensagem = "Vaga atribuida com sucesso !"});
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult AlterarVaga(string IdVaga)
        {
            var VerificaDados = new QueryMysqlCurriculo();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var DadosVaga = VerificaDados.RecuperaDadosVaga(IdVaga);
                var Vaga = new Vagas();
                Vaga.Titulo = DadosVaga[0]["titulo"];
                Vaga.Descricao = DadosVaga[0]["descricao"];
                Vaga.Beneficio = DadosVaga[0]["beneficio"];
                Vaga.Salario = DadosVaga[0]["salario"];
                Vaga.Requisitos = DadosVaga[0]["requisito"];
                TempData["IdVaga"] = IdVaga;

                if (DadosVaga[0]["ativa"].Equals("S"))
                {
                    TempData["Ativa"] = "checked";
                    TempData["Status"] = "Ativa";
                }
                else
                {
                    TempData["Ativa"] = "";
                    TempData["Status"] = "Desativada";
                }

                var Areas = DadosVaga[0]["areadeinteresse"].Split(';');

                for (var j = 0; j < Areas.Length; j++)
                    switch (Areas[j])
                    {
                        case "Administrativo":
                            TempData["Area1"] = "checked";
                            break;
                        case "Contabilidade":
                            TempData["Area2"] = "checked";
                            break;
                        case "Recursos Humanos":
                            TempData["Area3"] = "checked";
                            break;
                        case "Gerência":
                            TempData["Area4"] = "checked";
                            break;
                        case "Atendimento":
                            TempData["Area5"] = "checked";
                            break;
                        case "Departamento Pessoal":
                            TempData["Area6"] = "checked";
                            break;
                        case "Caixas":
                            TempData["Area7"] = "checked";
                            break;
                        case "Retaguarda":
                            TempData["Area8"] = "checked";
                            break;
                        case "Tecnologia da Informação":
                            TempData["Area9"] = "checked";
                            break;
                        case "Jurídico":
                            TempData["Area10"] = "checked";
                            break;
                        case "Crédito":
                            TempData["Area11"] = "checked";
                            break;
                        case "Produtos e Serviços":
                            TempData["Area12"] = "checked";
                            break;
                        case "Cadastro":
                            TempData["Area13"] = "checked";
                            break;
                        case "Marketing":
                            TempData["Area14"] = "checked";
                            break;
                        case "Controle Interno":
                            TempData["Area15"] = "checked";
                            break;
                        case "Recuperação de Crédito":
                            TempData["Area16"] = "checked";
                            break;
                        case "Tesouraria":
                            TempData["Area17"] = "checked";
                            break;
                        case "Aprendiz":
                            TempData["Area18"] = "checked";
                            break;
                    }


                return PartialView("ModalEditarVaga", Vaga);
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult AtualizarVaga(Vagas DadosVaga, FormCollection Dados)
        {
            var VerificaDados = new QueryMysqlCurriculo();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Ativa = "";
                var Areas = "";
                for (var i = 6; i < Dados.Count; i++)
                    Areas = Areas + Dados[i] + ";";

                try
                {
                    if (Dados["Ativa"].Equals("Ativa"))
                        Ativa = "S";
                }
                catch
                {
                    Ativa = "N";
                }


                VerificaDados.AtualizarVaga(Dados["IdVaga"], DadosVaga.Descricao, DadosVaga.Salario,
                    DadosVaga.Requisitos, DadosVaga.Titulo, DadosVaga.Beneficio, Ativa, Areas);

                return RedirectToAction("Curriculo", "Curriculo", new {Mensagem = "Vaga atualizada com sucesso !"});
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult CriarUsuario(FormCollection DadosUsuario)
        {
            var VerificaDados = new QueryMysqlCurriculo();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var DadosUsuarioBanco = VerificaDados.RecuperaDadosCandidato(DadosUsuario["Cpf"]);

                var InserirUsuario = new QueryMysqlRh();

                InserirUsuario.CadastrarUsuarioPortal(DadosUsuarioBanco[0]["nome"], DadosUsuarioBanco[0]["cpf"],
                    DadosUsuarioBanco[0]["identidade"],
                    Convert.ToDateTime(DadosUsuarioBanco[0]["datanascimento"]).ToString("yyyy/MM/dd"),
                    DadosUsuarioBanco[0]["endereco"], DadosUsuarioBanco[0]["numero"], DadosUsuarioBanco[0]["bairro"],
                    DadosUsuarioBanco[0]["cidade"], DadosUsuario["LoginUsuario"], DadosUsuario["IdPa"],
                    Convert.ToDateTime(DadosUsuario["DataAdmissao"]).ToString("yyyy/MM/dd"),
                    DadosUsuario["VencimentoPeriodico"], DadosUsuario["Pis"],
                    DadosUsuario["SalarioUsuario"].Replace(',', '.'),
                    DadosUsuario["QuebradeCaixaUsuario"].Replace(',', '.'));

                return RedirectToAction("Curriculo", "Curriculo", new {Mensagem = "Usuário cadastrado com sucesso !"});
            }

            return RedirectToAction("Login", "Login");
        }
    }
}