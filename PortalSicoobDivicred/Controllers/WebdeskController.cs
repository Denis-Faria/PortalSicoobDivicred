using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Nest;
using PortalSicoobDivicred.Aplicacao;
using PortalSicoobDivicred.Models;

namespace PortalSicoobDivicred.Controllers
{
    public class WebdeskController : Controller
    {
        public ActionResult Chamados(string mensagem)
        {
            var verificaDados = new QueryMysqlWebdesk();

            var logado = verificaDados.UsuarioLogado();
            if (logado)
            {
                TempData["Mensagem"] = mensagem;
                var cookie = Request.Cookies.Get("CookieFarm");
                if (cookie != null)
                {
                    var login = Criptografa.Descriptografar(cookie.Value);

                    var dadosUsuario = verificaDados.RecuperaDadosUsuarios(login);

                    TempData["usuarioTI"] = dadosUsuario[0]["idgrupo"];

                    var permissao = new QueryMysql();
                    if (permissao.PermissaoCurriculos(login))
                        TempData["PermissaoCurriculo"] =
                            " ";
                    else
                        TempData["PermissaoCurriculo"] = "display: none";

                    if (permissao.PermissaoTesouraria(login))
                        TempData["PermissaoTesouraria"] =
                            " ";
                    else
                        TempData["PermissaoTesouraria"] = "display: none";

                    if (dadosUsuario[0]["foto"] == null)
                        TempData["ImagemPerfil"] = "http://bulma.io/images/placeholders/128x128.png";
                    else
                        TempData["ImagemPerfil"] = dadosUsuario[0]["foto"];

                    TempData["NomeLateral"] = dadosUsuario[0]["login"];

                    var chamadosEmAberto = verificaDados.RetornaChamadosAbertos(dadosUsuario[0]["id"]);
                    TempData["TotalChamados"] = chamadosEmAberto.Count;
                    for (var i = 0; i < chamadosEmAberto.Count; i++)
                    {
                        if (chamadosEmAberto[i]["cpf"] != "")
                            TempData["Titulo" + i] =
                                chamadosEmAberto[i]["titulo"] + " CPF/CNPJ: " + chamadosEmAberto[i]["cpf"];
                        else
                            TempData["Titulo" + i] =
                                chamadosEmAberto[i]["titulo"];

                        TempData["Numero" + i] = chamadosEmAberto[i]["id"];
                        TempData["Operador" + i] = chamadosEmAberto[i]["operador"];
                        TempData["Situacao" + i] = chamadosEmAberto[i]["situacao"];
                        TempData["DataHoraCadastro" + i] = chamadosEmAberto[i]["datahoracadastro"];

                        if (chamadosEmAberto[i]["fimatendimento"] == null)
                        {
                            var sla = TimeSpan.Parse(chamadosEmAberto[i]["sla"]);

                            double horas;
                            if (Convert.ToInt32(TimeSpan.Parse(chamadosEmAberto[i]["tempo"]).TotalMinutes) == 0)
                                horas = 00;
                            else
                                horas = sla.TotalMinutes * 100 /
                                        TimeSpan.Parse(chamadosEmAberto[i]["tempo"]).TotalMinutes;

                            if (horas > 100)
                                TempData["StatusCor" + i] = "is-danger";
                            else
                                TempData["StatusCor" + i] = "is-primary";

                            TempData["InformacaoSLA" + i] =
                                "TEMPO DECORRIDO:" + sla.Days + " DIAS, " + sla.Hours + ":" +
                                sla.Minutes + ":00" + " || TEMPO ESTIMADO: " +
                                chamadosEmAberto[i]["tempo"];
                            TempData["Sla" + i] = Convert.ToInt32(horas);
                        }
                        else
                        {
                            TempData["InformacaoSLA" + i] = "SOLICITAÇÃO ENCERRADA";
                            TempData["Sla" + i] = 100;
                        }
                    }

                    var chamadosOperador = verificaDados.RetornaChamadosResponsavel(dadosUsuario[0]["id"]);

                    TempData["TotalChamadosOperador"] = chamadosOperador.Count;
                    for (var i = 0; i < chamadosOperador.Count; i++)
                    {
                        if (chamadosOperador[i]["cpf"] != "")
                            TempData["TituloOperador" + i] =
                                chamadosOperador[i]["titulo"] + " CPF/CNPJ: " + chamadosOperador[i]["cpf"];
                        else
                            TempData["TituloOperador" + i] =
                                chamadosOperador[i]["titulo"];
                        TempData["NumeroOperador" + i] = chamadosOperador[i]["id"];
                        TempData["OperadorOperador" + i] = chamadosOperador[i]["operador"];
                        TempData["SituacaoOperador" + i] = chamadosOperador[i]["situacao"];
                        TempData["CadastroOperador" + i] = chamadosOperador[i]["cadastro"];
                        TempData["DataHoraCadastroOperador" + i] = chamadosOperador[i]["datahoracadastro"];

                        var sla = TimeSpan.Parse(chamadosOperador[i]["sla"]);

                        double horas;
                        if (Convert.ToInt32(TimeSpan.Parse(chamadosOperador[i]["tempo"]).TotalMinutes) == 0)
                            horas = 00;
                        else
                            horas = sla.TotalMinutes * 100 / TimeSpan.Parse(chamadosOperador[i]["tempo"]).TotalMinutes;

                        if (horas > 100)
                            TempData["StatusCorOperador" + i] = "is-danger";
                        else
                            TempData["StatusCorOperador" + i] = "is-primary";

                        TempData["InformacaoSLAOperador" + i] =
                            "TEMPO DECORRIDO:" + sla.Days + " DIAS, " + sla.Hours + ":" + sla.Minutes + ":00" +
                            " || TEMPO ESTIMADO: " + chamadosOperador[i]["tempo"];
                        TempData["SlaOperador" + i] = Convert.ToInt32(horas);
                    }


                    var chamadosSetor = verificaDados.RetornaChamadosSetor(dadosUsuario[0]["idsetor"]);

                    TempData["TotalChamadosSetor"] = chamadosSetor.Count;
                    for (var i = 0; i < chamadosSetor.Count; i++)
                    {
                        if (chamadosSetor[i]["cpf"] != "")
                            TempData["TituloSetor" + i] =
                                chamadosSetor[i]["titulo"] + " CPF/CNPJ: " + chamadosSetor[i]["cpf"];
                        else
                            TempData["TituloSetor" + i] =
                                chamadosSetor[i]["titulo"];
                        TempData["NumeroSetor" + i] = chamadosSetor[i]["id"];
                        TempData["OperadorSetor" + i] = chamadosSetor[i]["operador"];
                        TempData["SituacaoSetor" + i] = chamadosSetor[i]["situacao"];
                        TempData["CadastroSetor" + i] = chamadosSetor[i]["cadastro"];
                        TempData["DataHoraSetor" + i] = chamadosSetor[i]["datahoracadastro"];

                        var sla = TimeSpan.Parse(chamadosSetor[i]["sla"]);
                        double horas;
                        try
                        {
                            if (Convert.ToInt32(TimeSpan.Parse(chamadosSetor[i]["tempo"]).TotalMinutes) == 0)
                                horas = 00;
                            else
                                horas = sla.TotalMinutes * 100 / TimeSpan.Parse(chamadosSetor[i]["tempo"]).TotalMinutes;
                        }
                        catch
                        {
                            horas = sla.TotalMinutes * 100 / TimeSpan.Parse( chamadosSetor[i]["tempo"] ).TotalMinutes;
                        }

                        if (horas > 100)
                            TempData["StatusCorSetor" + i] = "is-danger";
                        else
                            TempData["StatusCorSetor" + i] = "is-primary";

                        TempData["InformacaoSLASetor" + i] =
                            "TEMPO DECORRIDO:" + sla.Days + " DIAS, " + sla.Hours + ":" +
                            sla.Minutes + ":00" + " || TEMPO ESTIMADO: " +
                            chamadosSetor[i]["tempo"];
                        TempData["SlaSetor" + i] = Convert.ToInt32(horas);
                    }


                    if (dadosUsuario[0]["gestor"].Equals("S"))
                    {
                        TempData["PermissaoGestor"] = "N";
                        TempData["AreaGestor"] = "S";
                    }
                    else
                    {
                        TempData["PermissaoGestor"] = "N";
                        TempData["AreaGestor"] = "N";
                    }
                }

                var solicitacao = new SolicitacaoWebDesk();
                var setores = verificaDados.RetornaSetor();
                solicitacao.SetorResponsavel = setores;

                return View(solicitacao);
            }

            return RedirectToAction("Login", "Login");
        }

        [ValidateInput(false)]
        public ActionResult BuscaChamados(string busca)
        {
            var verificaDados = new QueryMysqlWebdesk();

            var logado = verificaDados.UsuarioLogado();
            if (logado)
            {
                var cookie = Request.Cookies.Get("CookieFarm");
                if (cookie != null)
                {
                    var login = Criptografa.Descriptografar(cookie.Value);
                    var verificaDadosUsuario = new QueryMysql();

                    var dadosUsuarios = verificaDadosUsuario.RecuperadadosFuncionariosTabelausuario(login);
                    var dadosFuncionarios = verificaDadosUsuario.RecuperaDadosUsuarios(login);

                    var buscaWebdesk = new BuscaElasticSearch();

                    if (dadosFuncionarios[0]["foto"] == null)
                        TempData["ImagemPerfil"] = "http://bulma.io/images/placeholders/128x128.png";
                    else
                        TempData["ImagemPerfil"] = dadosFuncionarios[0]["foto"];

                    TempData["NomeLateral"] = dadosFuncionarios[0]["login"];

                    var permissao = new QueryMysql();
                    if (permissao.PermissaoCurriculos(login))
                        TempData["PermissaoCurriculo"] =
                            " ";
                    else
                        TempData["PermissaoCurriculo"] = "display: none";

                    if (permissao.PermissaoTesouraria(login))
                        TempData["PermissaoTesouraria"] =
                            " ";
                    else
                        TempData["PermissaoTesouraria"] = "display: none";

                    List<Dictionary<string, string>> resultadoPEsquisa;
                    List<IHit<Webdesk>> dadoResultado;
                    if (verificaDadosUsuario.PermissaoPesquisaWebdDesk(login))
                    {
                        dadoResultado = buscaWebdesk.PesquisaTotalWebdesk(busca);
                        resultadoPEsquisa = verificaDados.BuscaChamadosTotalAntigo(busca, dadosUsuarios[0]["id"]);
                    }
                    else
                    {
                        resultadoPEsquisa = verificaDados.BuscaChamadosMeuSetor(busca, dadosUsuarios[0]["id"],
                            dadosUsuarios[0]["idsetor"]);
                        dadoResultado = buscaWebdesk.PesquisaBasicaWebdesk(busca, dadosFuncionarios[0]["idsetor"]);
                    }

                    TempData["TotalResultado"] = resultadoPEsquisa.Count;
                    TempData["TotalResultadoNovo"] = dadoResultado.Count;

                    for (var i = 0; i < resultadoPEsquisa.Count; i++)
                    {
                        TempData["NumeroChamado" + i] = resultadoPEsquisa[i]["id"];
                        TempData["TituloChamado" + i] = resultadoPEsquisa[i]["titulochamado"];
                        TempData["UsuarioCadastro" + i] = resultadoPEsquisa[i]["CADASTRO"];
                        TempData["Operador" + i] = resultadoPEsquisa[i]["OPERADOR"];
                        var interacoes = verificaDados.BuscaInteracaoChamados(resultadoPEsquisa[i]["id"]);

                        TempData["TotalInteracao" + resultadoPEsquisa[i]["id"]] = interacoes.Count;

                        for (var j = 0; j < interacoes.Count; j++)
                        {
                            TempData["UsuarioInteracao" + resultadoPEsquisa[i]["id"] + j] = interacoes[j]["nome"];
                            TempData["TextoInteracao" + resultadoPEsquisa[i]["id"] + j] =
                                interacoes[j]["textointeracao"];
                            TempData["DataInteracao" + resultadoPEsquisa[i]["id"] + j] = interacoes[j]["data"];
                        }
                    }


                    for (var i = 0; i < dadoResultado.Count; i++)
                    {
                        TempData["NumeroChamadoNovo" + i] = dadoResultado[i].Source.idsolicitacao;
                        var dadosChamado = verificaDados.RetornaDadosChamado(dadoResultado[i].Source.idsolicitacao);
                        TempData["TituloChamadoNovo" + i] = dadosChamado[0]["titulo"];
                        TempData["UsuarioCadastroNovo" + i] = dadosChamado[0]["cadastro"];
                        TempData["OperadorNovo" + i] = dadosChamado[0]["operador"];

                        var interacoes =
                            verificaDados.BuscaInteracaoChamadosNovo(dadoResultado[i].Source.idsolicitacao);

                        TempData["TotalInteracaoNovo" + dadoResultado[i].Source.idsolicitacao] = interacoes.Count;

                        for (var j = 0; j < interacoes.Count; j++)
                        {
                            TempData["UsuarioInteracaoNovo" + dadoResultado[i].Source.idsolicitacao + j] =
                                interacoes[j]["nome"];
                            TempData["TextoInteracaoNovo" + dadoResultado[i].Source.idsolicitacao + j] =
                                interacoes[j]["textointeracao"];
                            TempData["DataInteracaoNovo" + dadoResultado[i].Source.idsolicitacao + j] =
                                interacoes[j]["data"];
                        }
                    }
                }


                return View("ResultadoPesquisa");
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> CadastrarSolicitacao(FormCollection dados,
            IEnumerable<HttpPostedFileBase> postedFiles)
        {
            var verificaDados = new QueryMysqlWebdesk();
            var logado = verificaDados.UsuarioLogado();
            if (logado)
            {
                var cookie = Request.Cookies.Get("CookieFarm");
                if (cookie != null)
                {
                    var login = Criptografa.Descriptografar(cookie.Value);

                    var dadosUsuario = verificaDados.RecuperaDadosUsuarios(login);
                    string idInteracao;
                    if (dados["IdSetorResponsavel"].Equals("42"))
                    {
                        if (dados["CpfAbertura"] != "")
                            idInteracao = verificaDados.CadastraSolicitacao(dados["IdSetorResponsavel"],
                                dados["IdCategoria"],
                                "0",
                                dados["Descricao"], dadosUsuario[0]["id"], dados["CpfAbertura"]);
                        else
                            idInteracao = verificaDados.CadastraSolicitacao(dados["IdSetorResponsavel"],
                                dados["IdCategoria"],
                                "0",
                                dados["Descricao"], dadosUsuario[0]["id"], "");
                    }
                    else
                    {
                        if (dados["CpfAbertura"] != "")
                            idInteracao = verificaDados.CadastraSolicitacao(dados["IdSetorResponsavel"],
                                dados["IdCategoria"],
                                dados["IdFuncionarioResponsavel"],
                                dados["Descricao"], dadosUsuario[0]["id"], dados["CpfAbertura"]);
                        else
                            idInteracao = verificaDados.CadastraSolicitacao(dados["IdSetorResponsavel"],
                                dados["IdCategoria"],
                                dados["IdFuncionarioResponsavel"],
                                dados["Descricao"], dadosUsuario[0]["id"], "");
                    }

                    for (var i = 0; i < dados.Count; i++)
                        if (!dados.GetKey(i).Equals("IdSetorResponsavel") && !dados.GetKey(i).Equals("IdCategoria") &&
                            !dados.GetKey(i).Equals("IdFuncionarioResponsavel") &&
                            !dados.GetKey(i).Equals("CpfAbertura") &&
                            !dados.GetKey(i).Equals("Descricao"))
                            verificaDados.InserirFormulario(dados.GetKey(i), dados[i], idInteracao.Split(';')[1]);

                    var lista = postedFiles.ToList();

                    for (var i = 0; i < lista.Count; i++)
                        if (lista[i] != null)
                        {
                            var nomeArquivo = Path.GetFileName(lista[i].FileName);
                            byte[] fileData;
                            using (var binaryReader = new BinaryReader(lista[i].InputStream))
                            {
                                fileData = binaryReader.ReadBytes(lista[i].ContentLength);
                            }

                            verificaDados.InserirAnexo(idInteracao.Split(';')[0], fileData, lista[i].ContentType,
                                nomeArquivo);
                        }

                    var envia = new EnviodeAlertas();
                    var idSolicitante = verificaDados.RetornaIdSolicitantes(idInteracao.Split(';')[1]);

                    var dadosOperador =
                        verificaDados.RetornaInformacoesNotificacao(idSolicitante[0]["idfuncionariocadastro"]);

                    await envia.EnviaAlertaFuncionario(dadosOperador[0],
                        "Foi Aberto um chamado para você.", "6");

                    return RedirectToAction("Chamados", "Webdesk",
                        new {Mensagem = "Solicitação cadastrada com sucesso!"});
                }
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult RetornaCategoriaFuncionario(string idSetor)
        {
            var solicitacao = new SolicitacaoWebDesk();
            var verificaDados = new QueryMysqlWebdesk();
            var categoria = verificaDados.RetornaCategoria(idSetor);
            var funcionario = verificaDados.RetornaFuncionario(idSetor);

            solicitacao.Categoria = categoria;
            solicitacao.FuncionarioResponsavel = funcionario;

            return View("CategoriaSetorSolicitacao", solicitacao);
        }

        public ActionResult RetornaCategoriaAberturaChamado(string idSetor)
        {
            var solicitacao = new SolicitacaoWebDesk();
            var verificaDados = new QueryMysqlWebdesk();
            var categoria = verificaDados.RetornaCategoria(idSetor);

            solicitacao.Categoria = categoria;

            return View("CategoriaInteracao", solicitacao);
        }

        [HttpPost]
        public ActionResult RetornaCategoria(string idCadastro)
        {
            var solicitacao = new SolicitacaoWebDesk();
            var verificaDados = new QueryMysqlWebdesk();
            var cookie = Request.Cookies.Get("CookieFarm");
            if (cookie != null)
            {
                var login = Criptografa.Descriptografar(cookie.Value);
                var dadosUsuario = verificaDados.RecuperaDadosUsuarios(login);
                if (idCadastro.Equals(dadosUsuario[0]["id"])) return View("faltapermissao");

                var categoria = verificaDados.RetornaCategoria(dadosUsuario[0]["idsetor"]);

                solicitacao.Categoria = categoria;
            }

            return View("CategoriaInteracao", solicitacao);
        }


        public ActionResult RetornaSetor(string idCadastro)
        {
            var solicitacao = new SolicitacaoWebDesk();
            var verificaDados = new QueryMysqlWebdesk();
            var cookie = Request.Cookies.Get("CookieFarm");
            if (cookie != null)
            {
                var login = Criptografa.Descriptografar(cookie.Value);
                var dadosUsuario = verificaDados.RecuperaDadosUsuarios(login);
                if (idCadastro.Equals(dadosUsuario[0]["id"])) return View("faltapermissao");
            }

            var setor = verificaDados.RetornaSetor();
            solicitacao.SetorResponsavel = setor;

            return View("SetorInteracao", solicitacao);
        }

        public ActionResult RetornaFuncionario(string idSetor)
        {
            var solicitacao = new SolicitacaoWebDesk();
            var verificaDados = new QueryMysqlWebdesk();
            var funcionario = verificaDados.RetornaFuncionario(idSetor);

            solicitacao.FuncionarioResponsavel = funcionario;

            return View("FuncionarioInteracao", solicitacao);
        }

        public ActionResult Documentos(string anexos)
        {
            return View("Chamados");
        }

        public ActionResult InteracaoChamado(string idChamado, string mensagem, string tipoChamado)
        {
            if (tipoChamado.Equals("Novo"))
            {
                var verificaDados = new QueryMysqlWebdesk();
                var logado = verificaDados.UsuarioLogado();
                if (logado)
                {
                    var cookie = Request.Cookies.Get("CookieFarm");
                    if (cookie != null)
                    {
                        var login = Criptografa.Descriptografar(cookie.Value);

                        var dadosUsuario = verificaDados.RecuperaDadosUsuarios(login);
                        var formulario = verificaDados.RetornaFormularioChamado(idChamado);


                        if (dadosUsuario[0]["foto"] == null)
                            TempData["ImagemPerfil"] = "http://bulma.io/images/placeholders/128x128.png";
                        else
                            TempData["ImagemPerfil"] = dadosUsuario[0]["foto"];

                        var permissao = new QueryMysql();
                        if (permissao.PermissaoCurriculos(login))
                            TempData["PermissaoCurriculo"] =
                                " ";
                        else
                            TempData["PermissaoCurriculo"] = "display: none";

                        if (permissao.PermissaoTesouraria(login))
                            TempData["PermissaoTesouraria"] =
                                " ";
                        else
                            TempData["PermissaoTesouraria"] = "display: none";

                        TempData["NomeLateral"] = dadosUsuario[0]["login"];
                        TempData["IdSolicitacao"] = idChamado;
                        var dadosChamado = verificaDados.RetornaDadosChamado(idChamado);


                        TempData["TotalFormulario"] = formulario.Count;
                        for (var i = 0; i < formulario.Count; i++)
                        {
                            TempData["NomeUsuarioFormulario"] = dadosChamado[0]["cadastro"];
                            TempData["NomeCampo" + i] = formulario[i]["nomecampo"];
                            TempData["DadoFormulario" + i] = formulario[i]["dadoformulario"];
                        }


                        TempData["Setor"] = dadosChamado[0]["setor"];
                        TempData["Situacao"] = dadosChamado[0]["situacao"];
                        TempData["Categoria"] = dadosChamado[0]["titulo"];
                        TempData["FuncionarioSolicitante"] = dadosChamado[0]["cadastro"];
                        TempData["FuncionarioResponsavel"] = dadosChamado[0]["operador"];
                        TempData["DataAbertura"] = dadosChamado[0]["datahoracadastro"];
                        TempData["IdFuncionarioCadastro"] = dadosChamado[0]["idcadastro"];

                        if (dadosChamado[0]["inicioatendimento"] == null &&
                            !dadosChamado[0]["cadastro"].Equals(dadosUsuario[0]["nome"]))
                        {
                            TempData["IniciarAtendimento"] = "true";
                            TempData["InicioAtendimento"] = "NÃO INICIADO";
                        }
                        else
                        {
                            TempData["IniciarAtendimento"] = "false";
                            TempData["InicioAtendimento"] = dadosChamado[0]["inicioatendimento"];
                        }

                        if (dadosChamado[0]["fimatendimento"] == null)
                            TempData["FimATendimento"] = "NÃO FINALIZADO";
                        else
                            TempData["FimATendimento"] = dadosChamado[0]["fimatendimento"];

                        if (dadosChamado[0]["situacao"].Equals("CONCLUÍDO"))
                            TempData["Status"] = "is-link";
                        else if (dadosChamado[0]["situacao"].Equals("EM ATENDIMENTO"))
                            TempData["Status"] = "is-info";
                        else if (dadosChamado[0]["situacao"].Equals("PENDÊNCIA"))
                            TempData["Status"] = "is-warning";
                        else
                            TempData["Status"] = "is-warning";
                    }

                    var interacoes = verificaDados.RetornaInteracoesChamado(idChamado);

                    TempData["TotalInteracao"] = interacoes.Count;
                    for (var i = 0; i < interacoes.Count; i++)
                    {
                        TempData["Usuario" + i] = interacoes[i]["nome"] + " " +
                                                  Convert.ToDateTime(interacoes[i]["datahorainteracao"])
                                                      .ToString("dd/MM/yyyy");
                        TempData["Interacao" + i] = interacoes[i]["textointeracao"];
                        var anexoInteracao = verificaDados.RetornaAnexoInteracao(interacoes[i]["id"]);
                        TempData["IdInteracao" + i] = interacoes[i]["id"];
                        if (interacoes[i]["acao"].Equals("S"))
                            TempData["Acao" + i] = "is-selected";
                        else
                            TempData["Acao" + i] = "";

                        TempData["TotalAnexos" + interacoes[i]["id"]] = anexoInteracao.Rows.Count;
                        for (var j = 0; j < anexoInteracao.Rows.Count; j++)
                        {
                            var bytes = (byte[]) anexoInteracao.Rows[j]["arquivo"];
                            var img64 = Convert.ToBase64String(bytes);
                            var img64Url =
                                string.Format("data:" + anexoInteracao.Rows[j]["tipoarquivo"] + ";base64,{0}", img64);
                            TempData["NomeAnexo" + interacoes[i]["id"] + j] = anexoInteracao.Rows[j]["nomearquivo"];
                            TempData["ImagemAnexo" + interacoes[i]["id"] + j] = img64Url;
                        }
                    }

                    TempData["Mensagem"] = mensagem;
                    return View();
                }

                return RedirectToAction("Login", "Login");
            }
            else
            {
                var verificaDados = new QueryMysqlWebdesk();
                var logado = verificaDados.UsuarioLogado();
                if (logado)
                {
                    TempData["IdSolicitacao"] = idChamado;
                    var dadosChamado = verificaDados.RetornaDadosChamadoAntigo(idChamado);
                    TempData["Setor"] = dadosChamado[0]["setor"];
                    TempData["Situacao"] = dadosChamado[0]["situacao"];
                    TempData["Categoria"] = dadosChamado[0]["titulo"];
                    TempData["FuncionarioSolicitante"] = dadosChamado[0]["cadastro"];
                    TempData["FuncionarioResponsavel"] = dadosChamado[0]["operador"];
                    TempData["DataAbertura"] = dadosChamado[0]["datahoracadastro"];
                    TempData["IdFuncionarioCadastro"] = dadosChamado[0]["idcadastro"];

                    TempData["IniciarAtendimento"] = "false";
                    TempData["InicioAtendimento"] = dadosChamado[0]["confirmacaoleitura"];
                    TempData["FimATendimento"] = dadosChamado[0]["dataconclusaochamado"];


                    if (dadosChamado[0]["situacao"].Equals("CONCLUÍDO"))
                        TempData["Status"] = "is-link";
                    else if (dadosChamado[0]["situacao"].Equals("EM ATENDIMENTO"))
                        TempData["Status"] = "is-info";
                    else
                        TempData["Status"] = "is-warning";

                    var interacoes = verificaDados.RetornaInteracoesChamadoAntigo(idChamado);

                    TempData["TotalInteracao"] = interacoes.Count;
                    for (var i = 0; i < interacoes.Count; i++)
                    {
                        TempData["Usuario" + i] = interacoes[i]["nome"] + " " +
                                                  Convert.ToDateTime(interacoes[i]["datahorainteracao"])
                                                      .ToString("dd/MM/yyyy");
                        TempData["Interacao" + i] = interacoes[i]["textointeracao"];
                        TempData["IdInteracao" + i] = interacoes[i]["id"];
                        if (interacoes[i]["idarquivogoogle"] != null)
                        {
                            TempData["TotalAnexos" + interacoes[i]["id"]] = 1;
                            for (var j = 0; j < 1; j++)
                                TempData["ImagemAnexo" + interacoes[i]["id"]] =
                                    "https://drive.google.com/file/d/" + interacoes[i]["idarquivogoogle"] + "/edit";
                        }
                        else
                        {
                            TempData["TotalAnexos" + interacoes[i]["id"]] = 0;
                        }
                    }

                    TempData["Mensagem"] = mensagem;
                    return View();
                }

                return RedirectToAction("Login", "Login");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> CadastrarInteracao(FormCollection dados,
            IEnumerable<HttpPostedFileBase> postedFiles)
        {
            var verificaDados = new QueryMysqlWebdesk();
            var logado = verificaDados.UsuarioLogado();
            if (logado)
            {
                if (dados["AcaoInteracao"].Equals("Encerrar"))
                {
                    var cookie = Request.Cookies.Get("CookieFarm");
                    if (cookie != null)
                    {
                        var login = Criptografa.Descriptografar(cookie.Value);

                        var dadosUsuario = verificaDados.RecuperaDadosUsuarios(login);
                        var idInteracao = verificaDados.CadastrarInteracao(dados["IdSolicitacao"], dados["Descricao"],
                            dadosUsuario[0]["id"], "N");

                        verificaDados.CadastrarInteracao(dados["IdSolicitacao"],
                            " Solicitação encerrada por " + dadosUsuario[0]["nome"],
                            dadosUsuario[0]["id"], "S");

                        var lista = postedFiles.ToList();

                        for (var i = 0; i < lista.Count; i++)
                            if (lista[i] != null)
                            {
                                var nomeArquivo = Path.GetFileName(lista[i].FileName);
                                byte[] fileData;
                                using (var binaryReader = new BinaryReader(lista[i].InputStream))
                                {
                                    fileData = binaryReader.ReadBytes(lista[i].ContentLength);
                                }

                                verificaDados.InserirAnexo(idInteracao, fileData, lista[i].ContentType, nomeArquivo);
                            }
                    }

                    verificaDados.EncerrarSolicitacao(dados["IdSolicitacao"]);

                    var envia = new EnviodeAlertas();
                    var idSolicitante = verificaDados.RetornaIdSolicitantes(dados["IdSolicitacao"]);
                    var dadosOperador =
                        verificaDados.RetornaInformacoesNotificacao(idSolicitante[0]["idfuncionariocadastro"]);

                    await envia.EnviaAlertaFuncionario(dadosOperador[0],
                        "Sua solicitação n°" + dados["IdSolicitacao"] + " foi encerrada.", "6");

                    return RedirectToAction("InteracaoChamado", "Webdesk",
                        new
                        {
                            IdChamado = dados["IdSolicitacao"],
                            Mensagem = "Solicitação encerrada com sucesso !",
                            TipoChamado = "Novo"
                        });
                }

                if (dados["AcaoInteracao"].Equals("Repassar"))
                {
                    var cookie = Request.Cookies.Get("CookieFarm");
                    if (cookie != null)
                    {
                        var login = Criptografa.Descriptografar(cookie.Value);

                        var dadosUsuario = verificaDados.RecuperaDadosUsuarios(login);
                        var idInteracao = verificaDados.CadastrarInteracao(dados["IdSolicitacao"], dados["Descricao"],
                            dadosUsuario[0]["id"], "N");

                        var lista = postedFiles.ToList();

                        var nomeFuncionarioNovo =
                            verificaDados.RetornaRepasseFuncionarioChamado(dados["IdFuncionarioResponsavel"]);

                        verificaDados.CadastrarInteracao(dados["IdSolicitacao"],
                            " Solicitação encaminhada para " + nomeFuncionarioNovo[0]["nome"] + " por " +
                            dadosUsuario[0]["nome"],
                            dadosUsuario[0]["id"], "S");
                        for (var i = 0; i < lista.Count; i++)
                            if (lista[i] != null)
                            {
                                var nomeArquivo = Path.GetFileName(lista[i].FileName);
                                byte[] fileData;
                                using (var binaryReader = new BinaryReader(lista[i].InputStream))
                                {
                                    fileData = binaryReader.ReadBytes(lista[i].ContentLength);
                                }

                                verificaDados.InserirAnexo(idInteracao, fileData, lista[i].ContentType, nomeArquivo);
                            }

                        var envia = new EnviodeAlertas();
                        var idSolicitante = verificaDados.RetornaIdSolicitantes(dados["IdSolicitacao"]);

                        var dadosOperador =
                            verificaDados.RetornaInformacoesNotificacao(idSolicitante[0]["idfuncionariocadastro"]);

                        await envia.EnviaAlertaFuncionario(dadosOperador[0],
                            "Sua solicitação n°" + dados["IdSolicitacao"] + " foi encaminhada para " +
                            nomeFuncionarioNovo[0]["nome"] + " por " + dadosUsuario[0]["nome"], "6");


                        var dadosOperadorNovo =
                            verificaDados.RetornaInformacoesNotificacao(dados["IdFuncionarioResponsavel"]);

                        await envia.EnviaAlertaFuncionario(dadosOperadorNovo[0],
                            "A solicitação n°" + dados["IdSolicitacao"] + " foi encaminhada para você por " +
                            dadosUsuario[0]["nome"], "6");
                    }


                    verificaDados.AlterarResponsavelSolicitacao(dados["IdSolicitacao"], dados["IdSetorResponsavel"],
                        dados["IdFuncionarioResponsavel"]);


                    return RedirectToAction("InteracaoChamado", "Webdesk",
                        new
                        {
                            IdChamado = dados["IdSolicitacao"],
                            Mensagem = "Solicitação repassada com sucesso !",
                            TipoChamado = "Novo"
                        });
                }

                if (dados["AcaoInteracao"].Equals("Categoria"))
                {
                    var cookie = Request.Cookies.Get("CookieFarm");
                    if (cookie != null)
                    {
                        var login = Criptografa.Descriptografar(cookie.Value);

                        var dadosUsuario = verificaDados.RecuperaDadosUsuarios(login);
                        var idInteracao = verificaDados.CadastrarInteracao(dados["IdSolicitacao"], dados["Descricao"],
                            dadosUsuario[0]["id"], "N");

                        var dadosChamado = verificaDados.RetornaDadosChamado(dados["IdSolicitacao"]);
                        var nomeCategoriaNovo =
                            verificaDados.RetornaRepasseCategoriaChamado(dados["IdCategoria"]);

                        verificaDados.CadastrarInteracao(dados["IdSolicitacao"],
                            "Categoria da solicitação alterada de " + dadosChamado[0]["titulo"] + " para " +
                            nomeCategoriaNovo[0]["descricao"] + " por " + dadosUsuario[0]["nome"],
                            dadosUsuario[0]["id"], "S");

                        var lista = postedFiles.ToList();

                        for (var i = 0; i < lista.Count; i++)
                            if (lista[i] != null)
                            {
                                var nomeArquivo = Path.GetFileName(lista[i].FileName);
                                byte[] fileData;
                                using (var binaryReader = new BinaryReader(lista[i].InputStream))
                                {
                                    fileData = binaryReader.ReadBytes(lista[i].ContentLength);
                                }

                                verificaDados.InserirAnexo(idInteracao, fileData, lista[i].ContentType, nomeArquivo);
                            }
                    }

                    var envia = new EnviodeAlertas();
                    var idSolicitante = verificaDados.RetornaIdSolicitantes(dados["IdSolicitacao"]);

                    var dadosOperador =
                        verificaDados.RetornaInformacoesNotificacao(idSolicitante[0]["idfuncionariocadastro"]);

                    await envia.EnviaAlertaFuncionario(dadosOperador[0],
                        "Sua solicitação n°" + dados["IdSolicitacao"] + " alterou de categoria.", "6");


                    verificaDados.AlterarCategoriaSolicitacao(dados["IdSolicitacao"], dados["IdCategoria"]);

                    return RedirectToAction("InteracaoChamado", "Webdesk",
                        new
                        {
                            IdChamado = dados["IdSolicitacao"],
                            Mensagem = "Categoria da solicitação alterada com sucesso !",
                            TipoChamado = "Novo"
                        });
                }

                if (dados["AcaoInteracao"].Equals("Reabrir"))
                {
                    var cookie = Request.Cookies.Get("CookieFarm");
                    if (cookie != null)
                    {
                        var login = Criptografa.Descriptografar(cookie.Value);

                        var dadosUsuario = verificaDados.RecuperaDadosUsuarios(login);
                        var idInteracao = verificaDados.CadastrarInteracao(dados["IdSolicitacao"], dados["Descricao"],
                            dadosUsuario[0]["id"], "N");

                        verificaDados.CadastrarInteracao(dados["IdSolicitacao"],
                            " Solicitação reaberta por " + dadosUsuario[0]["nome"],
                            dadosUsuario[0]["id"], "S");
                        var lista = postedFiles.ToList();

                        for (var i = 0; i < lista.Count; i++)
                            if (lista[i] != null)
                            {
                                var nomeArquivo = Path.GetFileName(lista[i].FileName);
                                byte[] fileData;
                                using (var binaryReader = new BinaryReader(lista[i].InputStream))
                                {
                                    fileData = binaryReader.ReadBytes(lista[i].ContentLength);
                                }

                                verificaDados.InserirAnexo(idInteracao, fileData, lista[i].ContentType, nomeArquivo);
                            }
                    }

                    verificaDados.ReabrirSolicitacao(dados["IdSolicitacao"]);


                    var envia = new EnviodeAlertas();
                    var idSolicitante = verificaDados.RetornaIdSolicitantes(dados["IdSolicitacao"]);

                    var dadosOperador =
                        verificaDados.RetornaInformacoesNotificacao(idSolicitante[0]["idfuncionariocadastro"]);

                    await envia.EnviaAlertaFuncionario(dadosOperador[0],
                        "A solicitação n°" + dados["IdSolicitacao"] + " foi reaberta.", "6");


                    return RedirectToAction("InteracaoChamado", "Webdesk",
                        new
                        {
                            IdChamado = dados["IdSolicitacao"],
                            Mensagem = "Solicitação reaberta com sucesso !",
                            TipoChamado = "Novo"
                        });
                }

                if (dados["AcaoInteracao"].Equals("Pendente"))
                {
                    var cookie = Request.Cookies.Get("CookieFarm");
                    if (cookie != null)
                    {
                        var login = Criptografa.Descriptografar(cookie.Value);

                        var dadosUsuario = verificaDados.RecuperaDadosUsuarios(login);
                        var idInteracao = verificaDados.CadastrarInteracao(dados["IdSolicitacao"], dados["Descricao"],
                            dadosUsuario[0]["id"], "N");

                        verificaDados.CadastrarInteracao(dados["IdSolicitacao"],
                            "Pendência aberta por " + dadosUsuario[0]["nome"],
                            dadosUsuario[0]["id"], "S");
                        var lista = postedFiles.ToList();

                        for (var i = 0; i < lista.Count; i++)
                            if (lista[i] != null)
                            {
                                var nomeArquivo = Path.GetFileName(lista[i].FileName);
                                byte[] fileData;
                                using (var binaryReader = new BinaryReader(lista[i].InputStream))
                                {
                                    fileData = binaryReader.ReadBytes(lista[i].ContentLength);
                                }

                                verificaDados.InserirAnexo(idInteracao, fileData, lista[i].ContentType, nomeArquivo);
                            }
                    }

                    verificaDados.PendenciaSolicitacao(dados["IdSolicitacao"]);


                    var envia = new EnviodeAlertas();
                    var idSolicitante = verificaDados.RetornaIdSolicitantes(dados["IdSolicitacao"]);

                    var dadosOperador =
                        verificaDados.RetornaInformacoesNotificacao(idSolicitante[0]["idfuncionariocadastro"]);

                    await envia.EnviaAlertaFuncionario(dadosOperador[0],
                        "A solicitação n°" + dados["IdSolicitacao"] + " cotém pendências .", "6");


                    return RedirectToAction("InteracaoChamado", "Webdesk",
                        new
                        {
                            IdChamado = dados["IdSolicitacao"],
                            Mensagem = "Pendência aberta com sucesso !",
                            TipoChamado = "Novo"
                        });
                }

                if (dados["AcaoInteracao"].Equals("Solucionar"))
                {
                    var cookie = Request.Cookies.Get("CookieFarm");
                    if (cookie != null)
                    {
                        var login = Criptografa.Descriptografar(cookie.Value);

                        var dadosUsuario = verificaDados.RecuperaDadosUsuarios(login);
                        var idInteracao = verificaDados.CadastrarInteracao(dados["IdSolicitacao"], dados["Descricao"],
                            dadosUsuario[0]["id"], "N");

                        verificaDados.CadastrarInteracao(dados["IdSolicitacao"],
                            "Pendência solucionada por " + dadosUsuario[0]["nome"],
                            dadosUsuario[0]["id"], "S");
                        var lista = postedFiles.ToList();

                        for (var i = 0; i < lista.Count; i++)
                            if (lista[i] != null)
                            {
                                var nomeArquivo = Path.GetFileName(lista[i].FileName);
                                byte[] fileData;
                                using (var binaryReader = new BinaryReader(lista[i].InputStream))
                                {
                                    fileData = binaryReader.ReadBytes(lista[i].ContentLength);
                                }

                                verificaDados.InserirAnexo(idInteracao, fileData, lista[i].ContentType, nomeArquivo);
                            }
                    }

                    verificaDados.SolucionaSolicitacao(dados["IdSolicitacao"]);


                    var envia = new EnviodeAlertas();
                    var idSolicitante = verificaDados.RetornaIdSolicitantes(dados["IdSolicitacao"]);

                    var dadosOperadorNovo =
                        verificaDados.RetornaInformacoesNotificacao(idSolicitante[0]["idfuncionarioresponsavel"]);

                    await envia.EnviaAlertaFuncionario(dadosOperadorNovo[0],
                        "A solicitação n°" + dados["IdSolicitacao"] + " teve sua pendência solucionada", "6");


                    return RedirectToAction("InteracaoChamado", "Webdesk",
                        new
                        {
                            IdChamado = dados["IdSolicitacao"],
                            Mensagem = "Pendência solucionada com sucesso !",
                            TipoChamado = "Novo"
                        });
                }
                else
                {
                    var cookie = Request.Cookies.Get("CookieFarm");
                    if (cookie != null)
                    {
                        var login = Criptografa.Descriptografar(cookie.Value);

                        var dadosUsuario = verificaDados.RecuperaDadosUsuarios(login);
                        var idInteracao = verificaDados.CadastrarInteracao(dados["IdSolicitacao"], dados["Descricao"],
                            dadosUsuario[0]["id"], "N");

                        var lista = postedFiles.ToList();

                        for (var i = 0; i < lista.Count; i++)
                            if (lista[i] != null)
                            {
                                var nomeArquivo = Path.GetFileName(lista[i].FileName);
                                byte[] fileData;
                                using (var binaryReader = new BinaryReader(lista[i].InputStream))
                                {
                                    fileData = binaryReader.ReadBytes(lista[i].ContentLength);
                                }

                                verificaDados.InserirAnexo(idInteracao, fileData, lista[i].ContentType, nomeArquivo);
                            }
                    }

                    var envia = new EnviodeAlertas();
                    var idSolicitante = verificaDados.RetornaIdSolicitantes(dados["IdSolicitacao"]);

                    var dadosOperador =
                        verificaDados.RetornaInformacoesNotificacao(idSolicitante[0]["idfuncionariocadastro"]);

                    await envia.EnviaAlertaFuncionario(dadosOperador[0],
                        "Sua solicitação n°" + dados["IdSolicitacao"] + " teve interações.", "6");


                    return RedirectToAction("InteracaoChamado", "Webdesk",
                        new
                        {
                            IdChamado = dados["IdSolicitacao"],
                            Mensagem = "Interação adicionada com sucesso !",
                            TipoChamado = "Novo"
                        });
                }
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public async Task<ActionResult> IniciarAtendimento(string idSolicitacao)
        {
            var verificaDados = new QueryMysqlWebdesk();
            var cookie = Request.Cookies.Get("CookieFarm");
            if (cookie != null)
            {
                var login = Criptografa.Descriptografar(cookie.Value);
                var dadosUsuario = verificaDados.RecuperaDadosUsuarios(login);

                verificaDados.AlterarResponsavelSolicitacao(idSolicitacao, dadosUsuario[0]["idsetor"],
                    dadosUsuario[0]["id"]);
                verificaDados.CadastrarInteracao(idSolicitacao, " Atendimento iniciado por " + dadosUsuario[0]["nome"],
                    dadosUsuario[0]["id"], "S");
                verificaDados.IniciarAtendimentoSolicitacao(idSolicitacao);

                var envia = new EnviodeAlertas();
                var idSolicitante = verificaDados.RetornaIdSolicitantes(idSolicitacao);

                var dadosOperador =
                    verificaDados.RetornaInformacoesNotificacao(idSolicitante[0]["idfuncionariocadastro"]);

                await envia.EnviaAlertaFuncionario(dadosOperador[0],
                    "Sua solicitação n°" + idSolicitacao + " teve o atendimento iniciado por " +
                    dadosUsuario[0]["nome"], "6");
            }

            return RedirectToAction("InteracaoChamado", "Webdesk",
                new {IdChamado = idSolicitacao, Mensagem = "Interação adicionada com sucesso !", TipoChamado = "Novo"});
        }

        public ActionResult AreaGestor(string mensagem)
        {
            TempData["Mensagem"] = mensagem;
            var verificaDados = new QueryMysql();
            var logado = verificaDados.UsuarioLogado();
            if (logado)
            {
                var cookie = Request.Cookies.Get("CookieFarm");
                if (cookie != null)
                {
                    var login = Criptografa.Descriptografar(cookie.Value);


                    var dadosUsuarioBanco = verificaDados.RecuperaDadosUsuarios(login);

                    if (verificaDados.PermissaoCurriculos(dadosUsuarioBanco[0]["login"]))
                        TempData["PermissaoCurriculo"] =
                            " ";
                    else
                        TempData["PermissaoCurriculo"] = "display: none";

                    if (verificaDados.PermissaoTesouraria(dadosUsuarioBanco[0]["login"]))
                        TempData["PermissaoTesouraria"] =
                            " ";
                    else
                        TempData["PermissaoTesouraria"] = "display: none";

                    if (dadosUsuarioBanco[0]["gestor"].Equals("S"))
                    {
                        TempData["PermissaoGestor"] = "N";
                        TempData["AreaGestor"] = "S";
                    }
                    else
                    {
                        TempData["PermissaoGestor"] = "N";
                        TempData["AreaGestor"] = "N";
                    }

                    if (dadosUsuarioBanco[0]["foto"] == null)
                        TempData["ImagemPerfil"] = "http://bulma.io/images/placeholders/128x128.png";
                    else
                        TempData["ImagemPerfil"] = dadosUsuarioBanco[0]["foto"];

                    TempData["NomeLateral"] = dadosUsuarioBanco[0]["login"];
                }

                return View("AreaGestor");
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult FormularioSetor()
        {
            var verificaDados = new QueryMysqlWebdesk();
            var logado = verificaDados.UsuarioLogado();
            if (logado)
            {
                var cookie = Request.Cookies.Get("CookieFarm");
                if (cookie != null)
                {
                    var login = Criptografa.Descriptografar(cookie.Value);

                    var dadosUsuario = verificaDados.RecuperaDadosUsuarios(login);

                    var dadosFormularios = verificaDados.RetornaFormulariosSetor(dadosUsuario[0]["idsetor"]);

                    TempData["TotalFormularios"] = dadosFormularios.Count;

                    for (var i = 0; i < dadosFormularios.Count; i++)
                    {
                        TempData["CategoriaFormulario" + i] = dadosFormularios[i]["descricao"];
                        TempData["CamposFormulario" + i] = dadosFormularios[i]["campo"];
                        TempData["CamposFormularioObrigatorio" + i] = dadosFormularios[i]["campoobrigatorio"];
                    }
                }

                return PartialView("FormularioSetor");
            }

            return RedirectToAction("Login", "Login");
        }


        [HttpPost]
        public ActionResult RetornaFormulario(string idCategoria)
        {
            var verificaDados = new QueryMysqlWebdesk();
            var logado = verificaDados.UsuarioLogado();
            if (logado)
            {
                var formulario = verificaDados.RetornaFormularioCategoria(idCategoria);
                if (formulario.Count > 0)
                {
                    var count = 0;
                    var arrayCombo = new Dictionary<string, string>();

                    foreach (var campo in formulario)
                    {
                        if (campo["campoobrigatorio"].Equals("S"))
                            TempData["Obrigatorio" + count] = "required";
                        else
                            TempData["Obrigatorio" + count] = "";

                        if (campo["combo"].Equals("S"))
                        {
                            if (arrayCombo.ContainsKey(campo["nomecombo"]))
                            {
                                var camposCombo = arrayCombo[campo["nomecombo"]];
                                camposCombo = camposCombo + ";" + campo["campo"];
                                arrayCombo[campo["nomecombo"]] = camposCombo;
                            }
                            else
                            {
                                if (campo["campoobrigatorio"].Equals("S"))
                                    TempData["Obnrigatorio" + campo["nomecombo"]] = "required";
                                else
                                    TempData["Obnrigatorio" + campo["nomecombo"]] = "required";

                                arrayCombo.Add(campo["nomecombo"], campo["campo"]);
                            }
                        }
                        else
                        {
                            TempData["NomeCampo" + count] = campo["campo"];
                            count++;
                        }
                    }

                    TempData["TotalCampos"] = count;
                    TempData["Combos"] = arrayCombo;


                    return PartialView("FormularioAberturaChamado");
                }

                return null;
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult Funcionario()
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if (logado)
            {
                var certificacoes = verificaDados.RetornaCertificacoes();
                TempData["TotalCertificacao"] = certificacoes.Count;
                for (var i = 0; i < certificacoes.Count; i++)
                {
                    TempData["DescricaoCertificacao" + i] = certificacoes[i]["descricao"];
                    TempData["IdCertificacao" + i] = certificacoes[i]["id"];
                }

                return PartialView("ParametrosFuncionario");
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult Funcionario(Funcao dadosCadastro, FormCollection dados)
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if (logado)
            {
                var certificacoes = "";
                for (var i = 0; i < dados.Count; i++)
                    if (dados.AllKeys[i].Contains("Certificacao"))
                        certificacoes = certificacoes + dados[i] + ";";

                verificaDados.CadastrarFuncao(dadosCadastro.NomeFuncao, certificacoes);
                return RedirectToAction("ColaboradorRh", "PainelColaborador",
                    new {Mensagem = "Função cadastrada com sucesso !"});
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult CadastrarCategoria(Categorias categoria)
        {
            var verificaDados = new QueryMysqlWebdesk();
            var logado = verificaDados.UsuarioLogado();
            if (logado)
            {
                var cookie = Request.Cookies.Get("CookieFarm");
                if (cookie != null)
                {
                    var login = Criptografa.Descriptografar(cookie.Value);


                    var dadosFuncionarioBanco = verificaDados.RecuperaDadosUsuarios(login);
                    if (Convert.ToInt32(categoria.TempoCategoria.Split(':')[0]) > 24)
                    {
                        var controle = true;
                        var dias = 0;
                        var totalHoras = Convert.ToInt32(categoria.TempoCategoria.Split(':')[0]);
                        while (controle)
                        {
                            totalHoras = totalHoras - 24;
                            dias++;
                            if (totalHoras <= 24) controle = false;
                        }

                        verificaDados.CadastrarCategoria(categoria.DescricaoCategoria,
                            dias + "." + totalHoras + ":" + Convert.ToInt32(categoria.TempoCategoria.Split(':')[1]),
                            dadosFuncionarioBanco[0]["idsetor"]);
                    }
                    else
                    {
                        verificaDados.CadastrarCategoria(categoria.DescricaoCategoria,
                            TimeSpan.Parse(categoria.TempoCategoria).ToString(),
                            dadosFuncionarioBanco[0]["idsetor"]);
                    }

                    return RedirectToAction("AreaGestor", "Webdesk",
                        new {mensagem = "Interação adicionada com sucesso !"});
                }
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult CadastrarFormulario()
        {
            throw new NotImplementedException();
        }

        public ActionResult EditarCategoria()
        {
            throw new NotImplementedException();
        }
    }
}