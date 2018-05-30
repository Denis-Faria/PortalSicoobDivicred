using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PortalSicoobDivicred.Aplicacao;
using PortalSicoobDivicred.Models;

namespace PortalSicoobDivicred.Controllers
{
    public class WebdeskController : Controller
    {
        public ActionResult Chamados(string Mensagem)
        {
            var VerificaDados = new QueryMysqlWebdesk();

            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                TempData["Mensagem"] = Mensagem;
                var Cookie = Request.Cookies.Get("CookieFarm");
                var Login = Criptografa.Descriptografar(Cookie.Value);

                var DadosUsuario = VerificaDados.RecuperaDadosUsuarios(Login);

                var permissao = new QueryMysql();
                if (permissao.PermissaoCurriculos(Login))
                    TempData["PermissaoCurriculo"] =
                        " ";
                else
                    TempData["PermissaoCurriculo"] = "display: none";

                if (DadosUsuario[0]["foto"] == null)
                    TempData["ImagemPerfil"] = "http://bulma.io/images/placeholders/128x128.png";
                else
                    TempData["ImagemPerfil"] = DadosUsuario[0]["foto"];

                TempData["NomeLateral"] = DadosUsuario[0]["login"];

                var ChamadosEmAberto = VerificaDados.RetornaChamadosAbertos(DadosUsuario[0]["id"]);
                TempData["TotalChamados"] = ChamadosEmAberto.Count;
                for (var i = 0; i < ChamadosEmAberto.Count; i++)
                {
                    if (ChamadosEmAberto[i]["cpf"] != "")
                        TempData["Titulo" + i] =
                            ChamadosEmAberto[i]["titulo"] + " CPF/CNPJ: " + ChamadosEmAberto[i]["cpf"];
                    else
                        TempData["Titulo" + i] =
                            ChamadosEmAberto[i]["titulo"];

                    TempData["Numero" + i] = ChamadosEmAberto[i]["id"];
                    TempData["Operador" + i] = ChamadosEmAberto[i]["operador"];
                    TempData["Situacao" + i] = ChamadosEmAberto[i]["situacao"];

                    if (ChamadosEmAberto[i]["fimatendimento"] == null)
                    {
                        var Sla = TimeSpan.Parse(ChamadosEmAberto[i]["sla"]);

                        var Horas = Sla.TotalMinutes * 100 / TimeSpan.Parse(ChamadosEmAberto[i]["tempo"]).TotalMinutes;
                        if (Horas > 100)
                            TempData["StatusCor" + i] = "is-danger";
                        else
                            TempData["StatusCor" + i] = "is-primary";

                        TempData["InformacaoSLA" + i] = "TEMPO DECORRIDO:" + Sla.Days + " DIAS, " + Sla.Hours + ":" +
                                                        Sla.Minutes + ":00" + " || TEMPO ESTIMADO: " +
                                                        ChamadosEmAberto[i]["tempo"];
                        TempData["Sla" + i] = Convert.ToInt32(Horas);
                    }
                    else
                    {
                        TempData["InformacaoSLA" + i] = "SOLICITAÇÃO ENCERRADA";
                        TempData["Sla" + i] = 100;
                    }
                }

                var ChamadosOperador = VerificaDados.RetornaChamadosResponsavel(DadosUsuario[0]["id"]);

                TempData["TotalChamadosOperador"] = ChamadosOperador.Count;
                for (var i = 0; i < ChamadosOperador.Count; i++)
                {
                    if (ChamadosOperador[i]["cpf"] != "")
                        TempData["TituloOperador" + i] =
                            ChamadosOperador[i]["titulo"] + " CPF/CNPJ: " + ChamadosOperador[i]["cpf"];
                    else
                        TempData["TituloOperador" + i] =
                            ChamadosOperador[i]["titulo"];
                    TempData["NumeroOperador" + i] = ChamadosOperador[i]["id"];
                    TempData["OperadorOperador" + i] = ChamadosOperador[i]["operador"];
                    TempData["SituacaoOperador" + i] = ChamadosOperador[i]["situacao"];
                    TempData["CadastroOperador" + i] = ChamadosOperador[i]["cadastro"];

                    var Sla = TimeSpan.Parse(ChamadosOperador[i]["sla"]);

                    var Horas = Sla.TotalMinutes * 100 / TimeSpan.Parse(ChamadosOperador[i]["tempo"]).TotalMinutes;
                    if (Horas > 100)
                        TempData["StatusCorOperador" + i] = "is-danger";
                    else
                        TempData["StatusCorOperador" + i] = "is-primary";

                    TempData["InformacaoSLAOperador" + i] =
                        "TEMPO DECORRIDO:" + Sla.Days + " DIAS, " + Sla.Hours + ":" + Sla.Minutes + ":00" +
                        " || TEMPO ESTIMADO: " + ChamadosOperador[i]["tempo"];
                    TempData["SlaOperador" + i] = Convert.ToInt32(Horas);
                }


                var ChamadosSetor = VerificaDados.RetornaChamadosSetor(DadosUsuario[0]["idsetor"]);

                TempData["TotalChamadosSetor"] = ChamadosSetor.Count;
                for (var i = 0; i < ChamadosSetor.Count; i++)
                {
                    if (ChamadosSetor[i]["cpf"] != "")
                        TempData["TituloSetor" + i] =
                            ChamadosSetor[i]["titulo"] + " CPF/CNPJ: " + ChamadosSetor[i]["cpf"];
                    else
                        TempData["TituloSetor" + i] =
                            ChamadosSetor[i]["titulo"];
                    TempData["NumeroSetor" + i] = ChamadosSetor[i]["id"];
                    TempData["OperadorSetor" + i] = ChamadosSetor[i]["operador"];
                    TempData["SituacaoSetor" + i] = ChamadosSetor[i]["situacao"];
                    TempData["CadastroSetor" + i] = ChamadosSetor[i]["cadastro"];

                    var Sla = TimeSpan.Parse(ChamadosSetor[i]["sla"]);

                    var Horas = Sla.TotalMinutes * 100 / TimeSpan.Parse(ChamadosSetor[i]["tempo"]).TotalMinutes;
                    if (Horas > 100)
                        TempData["StatusCorSetor" + i] = "is-danger";
                    else
                        TempData["StatusCorSetor" + i] = "is-primary";

                    TempData["InformacaoSLASetor" + i] = "TEMPO DECORRIDO:" + Sla.Days + " DIAS, " + Sla.Hours + ":" +
                                                         Sla.Minutes + ":00" + " || TEMPO ESTIMADO: " +
                                                         ChamadosSetor[i]["tempo"];
                    TempData["SlaSetor" + i] = Convert.ToInt32(Horas);
                }

                var Solicitacao = new SolicitacaoWebDesk();
                var Setores = VerificaDados.RetornaSetor();
                Solicitacao.SetorResponsavel = Setores;

                return View(Solicitacao);
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult BuscaChamados(string Busca)
        {
            var VerificaDados = new QueryMysqlWebdesk();

            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Cookie = Request.Cookies.Get("CookieFarm");
                var Login = Criptografa.Descriptografar(Cookie.Value);
                var VerificaDadosUsuario = new QueryMysql();

                var DadosUsuarios = VerificaDadosUsuario.RecuperaDadosFuncionariosTabelaUsuario(Login);
                var DadosFuncionarios = VerificaDadosUsuario.RecuperaDadosUsuarios(Login);

                if (DadosFuncionarios[0]["foto"] == null)
                    TempData["ImagemPerfil"] = "http://bulma.io/images/placeholders/128x128.png";
                else
                    TempData["ImagemPerfil"] = DadosFuncionarios[0]["foto"];

                TempData["NomeLateral"] = DadosFuncionarios[0]["login"];

                var permissao = new QueryMysql();
                if (permissao.PermissaoCurriculos(Login))
                    TempData["PermissaoCurriculo"] =
                        " ";
                else
                    TempData["PermissaoCurriculo"] = "display: none";


                var ResultadoPEsquisa = VerificaDados.BuscaChamadosMeuSetor(Busca, DadosUsuarios[0]["id"],DadosUsuarios[0]["idsetor"]);

                var ResultadoPesquisaNova = VerificaDados.BuscaChamadosMeuSetorNovo(Busca, DadosFuncionarios[0]["id"],DadosFuncionarios[0]["idsetor"]);

                TempData["TotalResultado"] = ResultadoPEsquisa.Count;
                TempData["TotalResultadoNovo"] = ResultadoPesquisaNova.Count;

                for (var i = 0; i < ResultadoPEsquisa.Count; i++)
                {
                    TempData["NumeroChamado" + i] = ResultadoPEsquisa[i]["id"];
                    TempData["TituloChamado" + i] = ResultadoPEsquisa[i]["titulochamado"];
                    TempData["UsuarioCadastro" + i] = ResultadoPEsquisa[i]["CADASTRO"];
                    TempData["Operador" + i] = ResultadoPEsquisa[i]["OPERADOR"];
                    var Interacoes = VerificaDados.BuscaInteracaoChamados(ResultadoPEsquisa[i]["id"]);

                    TempData["TotalInteracao" + ResultadoPEsquisa[i]["id"]] = Interacoes.Count;

                    for (var j = 0; j < Interacoes.Count; j++)
                    {
                        TempData["UsuarioInteracao" + ResultadoPEsquisa[i]["id"] + j] = Interacoes[j]["nome"];
                        TempData["TextoInteracao" + ResultadoPEsquisa[i]["id"] + j] = Interacoes[j]["textointeracao"];
                        TempData["DataInteracao" + ResultadoPEsquisa[i]["id"] + j] = Interacoes[j]["data"];
                    }
                }

                for (var i = 0; i < ResultadoPesquisaNova.Count; i++)
                {
                    TempData["NumeroChamadoNovo" + i] = ResultadoPesquisaNova[i]["id"];
                    TempData["TituloChamadoNovo" + i] = ResultadoPesquisaNova[i]["titulo"];
                    TempData["UsuarioCadastroNovo" + i] = ResultadoPesquisaNova[i]["CADASTRO"];
                    TempData["OperadorNovo" + i] = ResultadoPesquisaNova[i]["OPERADOR"];
                    var Interacoes = VerificaDados.BuscaInteracaoChamadosNovo(ResultadoPesquisaNova[i]["id"]);

                    TempData["TotalInteracaoNovo" + ResultadoPesquisaNova[i]["id"]] = Interacoes.Count;

                    for (var j = 0; j < Interacoes.Count; j++)
                    {
                        TempData["UsuarioInteracaoNovo" + ResultadoPesquisaNova[i]["id"] + j] = Interacoes[j]["nome"];
                        TempData["TextoInteracaoNovo" + ResultadoPesquisaNova[i]["id"] + j] =
                            Interacoes[j]["textointeracao"];
                        TempData["DataInteracaoNovo" + ResultadoPesquisaNova[i]["id"] + j] = Interacoes[j]["data"];
                    }
                }


                return View("ResultadoPesquisa");
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CadastrarSolicitacao(FormCollection Dados, IEnumerable<HttpPostedFileBase> postedFiles)
        {
            var VerificaDados = new QueryMysqlWebdesk();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Cookie = Request.Cookies.Get("CookieFarm");
                var Login = Criptografa.Descriptografar(Cookie.Value);

                var DadosUsuario = VerificaDados.RecuperaDadosUsuarios(Login);
                var IdInteracao = "";
                if (Dados["CpfAbertura"]!="")
                    IdInteracao = VerificaDados.CadastraSolicitacao(Dados["IdSetorResponsavel"], Dados["IdCategoria"],
                        Dados["IdFuncionarioResponsavel"],
                        Dados["Descricao"], DadosUsuario[0]["id"], Dados["CpfAbertura"]);
                else
                    IdInteracao = VerificaDados.CadastraSolicitacao(Dados["IdSetorResponsavel"], Dados["IdCategoria"],
                        Dados["IdFuncionarioResponsavel"],
                        Dados["Descricao"], DadosUsuario[0]["id"], "");


                var lista = postedFiles.ToList();

                for (var i = 0; i < lista.Count; i++)
                    if (lista[i] != null)
                    {
                        var NomeArquivo = Path.GetFileName(lista[i].FileName);
                        byte[] fileData = null;
                        using (var binaryReader = new BinaryReader(lista[i].InputStream))
                        {
                            fileData = binaryReader.ReadBytes(lista[i].ContentLength);
                        }

                        VerificaDados.InserirAnexo(IdInteracao, fileData, lista[i].ContentType, NomeArquivo);
                    }

                return RedirectToAction("Chamados", "Webdesk", new {Mensagem = "Solicitação cadastrada com sucesso!"});
            }

            return RedirectToAction("Login", "Login");
        }

        public ActionResult RetornaCategoriaFuncionario(string IdSetor)
        {
            var Solicitacao = new SolicitacaoWebDesk();
            var VerificaDados = new QueryMysqlWebdesk();
            var Categoria = VerificaDados.RetornaCategoria(IdSetor);
            var Funcionario = VerificaDados.RetornaFuncionario(IdSetor);

            Solicitacao.Categoria = Categoria;
            Solicitacao.FuncionarioResponsavel = Funcionario;

            return View("CategoriaSetorSolicitacao", Solicitacao);
        }

        [HttpPost]
        public ActionResult RetornaCategoria(string IdCadastro)
        {
            var Solicitacao = new SolicitacaoWebDesk();
            var VerificaDados = new QueryMysqlWebdesk();
            var Cookie = Request.Cookies.Get("CookieFarm");
            var Login = Criptografa.Descriptografar(Cookie.Value);
            var DadosUsuario = VerificaDados.RecuperaDadosUsuarios(Login);
            if (IdCadastro.Equals(DadosUsuario[0]["id"])) return View("faltapermissao");

            var Categoria = VerificaDados.RetornaCategoria(DadosUsuario[0]["idsetor"]);

            Solicitacao.Categoria = Categoria;

            return View("CategoriaInteracao", Solicitacao);
        }

        public ActionResult RetornaSetor(string IdCadastro)
        {
            var Solicitacao = new SolicitacaoWebDesk();
            var VerificaDados = new QueryMysqlWebdesk();
            var Cookie = Request.Cookies.Get("CookieFarm");
            var Login = Criptografa.Descriptografar(Cookie.Value);
            var DadosUsuario = VerificaDados.RecuperaDadosUsuarios(Login);
            if (IdCadastro.Equals(DadosUsuario[0]["id"])) return View("faltapermissao");

            var Setor = VerificaDados.RetornaSetor();
            Solicitacao.SetorResponsavel = Setor;

            return View("SetorInteracao", Solicitacao);
        }

        public ActionResult RetornaFuncionario(string IdSetor)
        {
            var Solicitacao = new SolicitacaoWebDesk();
            var VerificaDados = new QueryMysqlWebdesk();
            var Funcionario = VerificaDados.RetornaFuncionario(IdSetor);

            Solicitacao.FuncionarioResponsavel = Funcionario;

            return View("FuncionarioInteracao", Solicitacao);
        }

        public ActionResult Documentos(string Anexos)
        {
            return View("Chamados");
        }

        public ActionResult InteracaoChamado(string IdChamado, string Mensagem, string TipoChamado)
        {
            if (TipoChamado.Equals("Novo"))
            {
                var VerificaDados = new QueryMysqlWebdesk();
                var Logado = VerificaDados.UsuarioLogado();
                if (Logado)
                {
                    var Cookie = Request.Cookies.Get("CookieFarm");
                    var Login = Criptografa.Descriptografar(Cookie.Value);

                    var DadosUsuario = VerificaDados.RecuperaDadosUsuarios(Login);

                    if (DadosUsuario[0]["foto"] == null)
                        TempData["ImagemPerfil"] = "http://bulma.io/images/placeholders/128x128.png";
                    else
                        TempData["ImagemPerfil"] = DadosUsuario[0]["foto"];

                    var permissao = new QueryMysql();
                    if (permissao.PermissaoCurriculos(Login))
                        TempData["PermissaoCurriculo"] =
                            " ";
                    else
                        TempData["PermissaoCurriculo"] = "display: none";

                    TempData["NomeLateral"] = DadosUsuario[0]["login"];
                    TempData["IdSolicitacao"] = IdChamado;
                    var DadosChamado = VerificaDados.RetornaDadosChamado(IdChamado);
                    TempData["Setor"] = DadosChamado[0]["setor"];
                    TempData["Situacao"] = DadosChamado[0]["situacao"];
                    TempData["Categoria"] = DadosChamado[0]["titulo"];
                    TempData["FuncionarioSolicitante"] = DadosChamado[0]["cadastro"];
                    TempData["FuncionarioResponsavel"] = DadosChamado[0]["operador"];
                    TempData["DataAbertura"] = DadosChamado[0]["datahoracadastro"];
                    TempData["IdFuncionarioCadastro"] = DadosChamado[0]["idcadastro"];

                    if (DadosChamado[0]["inicioatendimento"] == null &&
                        !DadosChamado[0]["cadastro"].Equals(DadosUsuario[0]["id"]))
                    {
                        TempData["IniciarAtendimento"] = "true";
                        TempData["InicioAtendimento"] = "NÃO INICIADO";
                    }
                    else
                    {
                        TempData["IniciarAtendimento"] = "false";
                        TempData["InicioAtendimento"] = DadosChamado[0]["inicioatendimento"];
                    }

                    if (DadosChamado[0]["fimatendimento"] == null)
                        TempData["FimATendimento"] = "NÃO FINALIZADO";
                    else
                        TempData["FimATendimento"] = DadosChamado[0]["fimatendimento"];

                    if (DadosChamado[0]["situacao"].Equals("CONCLUÍDO"))
                        TempData["Status"] = "is-link";
                    else if (DadosChamado[0]["situacao"].Equals("EM ATENDIMENTO"))
                        TempData["Status"] = "is-info";
                    else
                        TempData["Status"] = "is-warning";

                    var Interacoes = VerificaDados.RetornaInteracoesChamado(IdChamado);

                    TempData["TotalInteracao"] = Interacoes.Count;
                    for (var i = 0; i < Interacoes.Count; i++)
                    {
                        TempData["Usuario" + i] = Interacoes[i]["nome"] + " " +
                                                  Convert.ToDateTime(Interacoes[i]["datahorainteracao"])
                                                      .ToString("dd/MM/yyyy");
                        TempData["Interacao" + i] = Interacoes[i]["textointeracao"];
                        var AnexoInteracao = VerificaDados.RetornaAnexoInteracao(Interacoes[i]["id"]);
                        TempData["IdInteracao" + i] = Interacoes[i]["id"];
                        if (Interacoes[i]["acao"].Equals("S"))
                            TempData["Acao" + i] = "is-selected";
                        else
                            TempData["Acao" + i] = "";

                        TempData["TotalAnexos" + Interacoes[i]["id"]] = AnexoInteracao.Rows.Count;
                        for (var j = 0; j < AnexoInteracao.Rows.Count; j++)
                        {
                            var bytes = (byte[]) AnexoInteracao.Rows[j]["arquivo"];
                            var img64 = Convert.ToBase64String(bytes);
                            var img64Url =
                                string.Format("data:" + AnexoInteracao.Rows[j]["tipoarquivo"] + ";base64,{0}", img64);
                            TempData["NomeAnexo" + Interacoes[i]["id"] + j] = AnexoInteracao.Rows[j]["nomearquivo"];
                            TempData["ImagemAnexo" + Interacoes[i]["id"] + j] = img64Url;
                        }
                    }

                    TempData["Mensagem"] = Mensagem;
                    return View();
                }

                return RedirectToAction("Login", "Login");
            }
            else
            {
                var VerificaDados = new QueryMysqlWebdesk();
                var Logado = VerificaDados.UsuarioLogado();
                if (Logado)
                {
                    var Cookie = Request.Cookies.Get("CookieFarm");
                    var Login = Criptografa.Descriptografar(Cookie.Value);

                    var DadosUsuario = VerificaDados.RecuperaDadosUsuarios(Login);

                    TempData["IdSolicitacao"] = IdChamado;
                    var DadosChamado = VerificaDados.RetornaDadosChamadoAntigo(IdChamado);
                    TempData["Setor"] = DadosChamado[0]["setor"];
                    TempData["Situacao"] = DadosChamado[0]["situacao"];
                    TempData["Categoria"] = DadosChamado[0]["titulo"];
                    TempData["FuncionarioSolicitante"] = DadosChamado[0]["cadastro"];
                    TempData["FuncionarioResponsavel"] = DadosChamado[0]["operador"];
                    TempData["DataAbertura"] = DadosChamado[0]["datahoracadastro"];
                    TempData["IdFuncionarioCadastro"] = DadosChamado[0]["idcadastro"];

                    TempData["IniciarAtendimento"] = "false";
                    TempData["InicioAtendimento"] = DadosChamado[0]["confirmacaoleitura"];
                    TempData["FimATendimento"] = DadosChamado[0]["dataconclusaochamado"];


                    if (DadosChamado[0]["situacao"].Equals("CONCLUÍDO"))
                        TempData["Status"] = "is-link";
                    else if (DadosChamado[0]["situacao"].Equals("EM ATENDIMENTO"))
                        TempData["Status"] = "is-info";
                    else
                        TempData["Status"] = "is-warning";

                    var Interacoes = VerificaDados.RetornaInteracoesChamadoAntigo(IdChamado);

                    TempData["TotalInteracao"] = Interacoes.Count;
                    for (var i = 0; i < Interacoes.Count; i++)
                    {
                        TempData["Usuario" + i] = Interacoes[i]["nome"] + " " +
                                                  Convert.ToDateTime(Interacoes[i]["datahorainteracao"])
                                                      .ToString("dd/MM/yyyy");
                        TempData["Interacao" + i] = Interacoes[i]["textointeracao"];
                        TempData["IdInteracao" + i] = Interacoes[i]["id"];
                        if (Interacoes[i]["idarquivogoogle"] != null)
                        {
                            TempData["TotalAnexos" + Interacoes[i]["id"]] = 1;
                            for (var j = 0; j < 1; j++)
                                TempData["ImagemAnexo" + Interacoes[i]["id"]] =
                                    "https://drive.google.com/file/d/" + Interacoes[i]["idarquivogoogle"] + "/edit";
                        }
                        else
                        {
                            TempData["TotalAnexos" + Interacoes[i]["id"]] = 0;
                        }
                    }

                    TempData["Mensagem"] = Mensagem;
                    return View();
                }

                return RedirectToAction("Login", "Login");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CadastrarInteracao(FormCollection Dados, IEnumerable<HttpPostedFileBase> postedFiles)
        {
            var VerificaDados = new QueryMysqlWebdesk();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                if (Dados["AcaoInteracao"].Equals("Encerrar"))
                {
                    var Cookie = Request.Cookies.Get("CookieFarm");
                    var Login = Criptografa.Descriptografar(Cookie.Value);

                    var DadosUsuario = VerificaDados.RecuperaDadosUsuarios(Login);
                    var IdInteracao = VerificaDados.CadastrarInteracao(Dados["IdSolicitacao"], Dados["Descricao"],
                        DadosUsuario[0]["id"], "N");

                    VerificaDados.CadastrarInteracao(Dados["IdSolicitacao"],
                        " Solicitação encerrada por " + DadosUsuario[0]["nome"],
                        DadosUsuario[0]["id"], "S");

                    var lista = postedFiles.ToList();

                    for (var i = 0; i < lista.Count; i++)
                        if (lista[i] != null)
                        {
                            var NomeArquivo = Path.GetFileName(lista[i].FileName);
                            byte[] fileData = null;
                            using (var binaryReader = new BinaryReader(lista[i].InputStream))
                            {
                                fileData = binaryReader.ReadBytes(lista[i].ContentLength);
                            }

                            VerificaDados.InserirAnexo(IdInteracao, fileData, lista[i].ContentType, NomeArquivo);
                        }

                    VerificaDados.EncerrarSolicitacao(Dados["IdSolicitacao"]);


                    return RedirectToAction("InteracaoChamado", "Webdesk",
                        new
                        {
                            IdChamado = Dados["IdSolicitacao"],
                            Mensagem = "Solicitação encerrada com sucesso !",
                            TipoChamado = "Novo"
                        });
                }

                if (Dados["AcaoInteracao"].Equals("Repassar"))
                {
                    var Cookie = Request.Cookies.Get("CookieFarm");
                    var Login = Criptografa.Descriptografar(Cookie.Value);

                    var DadosUsuario = VerificaDados.RecuperaDadosUsuarios(Login);
                    var IdInteracao = VerificaDados.CadastrarInteracao(Dados["IdSolicitacao"], Dados["Descricao"],
                        DadosUsuario[0]["id"], "N");

                    var lista = postedFiles.ToList();

                    var NomeFuncionarioNovo =
                        VerificaDados.RetornaRepasseFuncionarioChamado(Dados["IdFuncionarioResponsavel"]);

                    VerificaDados.CadastrarInteracao(Dados["IdSolicitacao"],
                        " Solicitação encaminhada para " + NomeFuncionarioNovo[0]["nome"] + " por " +
                        DadosUsuario[0]["nome"],
                        DadosUsuario[0]["id"], "S");
                    for (var i = 0; i < lista.Count; i++)
                        if (lista[i] != null)
                        {
                            var NomeArquivo = Path.GetFileName(lista[i].FileName);
                            byte[] fileData = null;
                            using (var binaryReader = new BinaryReader(lista[i].InputStream))
                            {
                                fileData = binaryReader.ReadBytes(lista[i].ContentLength);
                            }

                            VerificaDados.InserirAnexo(IdInteracao, fileData, lista[i].ContentType, NomeArquivo);
                        }

                    VerificaDados.AlterarResponsavelSolicitacao(Dados["IdSolicitacao"], Dados["IdSetorResponsavel"],
                        Dados["IdFuncionarioResponsavel"]);


                    return RedirectToAction("InteracaoChamado", "Webdesk",
                        new
                        {
                            IdChamado = Dados["IdSolicitacao"],
                            Mensagem = "Solicitação repassada com sucesso !",
                            TipoChamado = "Novo"
                        });
                }

                if (Dados["AcaoInteracao"].Equals("Categoria"))
                {
                    var Cookie = Request.Cookies.Get("CookieFarm");
                    var Login = Criptografa.Descriptografar(Cookie.Value);

                    var DadosUsuario = VerificaDados.RecuperaDadosUsuarios(Login);
                    var IdInteracao = VerificaDados.CadastrarInteracao(Dados["IdSolicitacao"], Dados["Descricao"],
                        DadosUsuario[0]["id"], "N");

                    var DadosChamado = VerificaDados.RetornaDadosChamado(Dados["IdSolicitacao"]);
                    var NomeCategoriaNovo =
                        VerificaDados.RetornaRepasseCategoriaChamado(Dados["IdCategoria"]);

                    VerificaDados.CadastrarInteracao(Dados["IdSolicitacao"],
                        "Categoria da solicitação alterada de " + DadosChamado[0]["titulo"] + " para " +
                        NomeCategoriaNovo[0]["descricao"] + " por " + DadosUsuario[0]["nome"],
                        DadosUsuario[0]["id"], "S");

                    var lista = postedFiles.ToList();

                    for (var i = 0; i < lista.Count; i++)
                        if (lista[i] != null)
                        {
                            var NomeArquivo = Path.GetFileName(lista[i].FileName);
                            byte[] fileData = null;
                            using (var binaryReader = new BinaryReader(lista[i].InputStream))
                            {
                                fileData = binaryReader.ReadBytes(lista[i].ContentLength);
                            }

                            VerificaDados.InserirAnexo(IdInteracao, fileData, lista[i].ContentType, NomeArquivo);
                        }

                    VerificaDados.AlterarCategoriaSolicitacao(Dados["IdSolicitacao"], Dados["IdCategoria"]);
                    return RedirectToAction("InteracaoChamado", "Webdesk",
                        new
                        {
                            IdChamado = Dados["IdSolicitacao"],
                            Mensagem = "Categoria da solicitação alterada com sucesso !",
                            TipoChamado = "Novo"
                        });
                }

                if (Dados["AcaoInteracao"].Equals("Reabrir"))
                {
                    var Cookie = Request.Cookies.Get("CookieFarm");
                    var Login = Criptografa.Descriptografar(Cookie.Value);

                    var DadosUsuario = VerificaDados.RecuperaDadosUsuarios(Login);
                    var IdInteracao = VerificaDados.CadastrarInteracao(Dados["IdSolicitacao"], Dados["Descricao"],
                        DadosUsuario[0]["id"], "N");

                    VerificaDados.CadastrarInteracao(Dados["IdSolicitacao"],
                        " Solicitação reaberta por " + DadosUsuario[0]["nome"],
                        DadosUsuario[0]["id"], "S");
                    var lista = postedFiles.ToList();

                    for (var i = 0; i < lista.Count; i++)
                        if (lista[i] != null)
                        {
                            var NomeArquivo = Path.GetFileName(lista[i].FileName);
                            byte[] fileData = null;
                            using (var binaryReader = new BinaryReader(lista[i].InputStream))
                            {
                                fileData = binaryReader.ReadBytes(lista[i].ContentLength);
                            }

                            VerificaDados.InserirAnexo(IdInteracao, fileData, lista[i].ContentType, NomeArquivo);
                        }

                    VerificaDados.ReabrirSolicitacao(Dados["IdSolicitacao"]);


                    return RedirectToAction("InteracaoChamado", "Webdesk",
                        new
                        {
                            IdChamado = Dados["IdSolicitacao"],
                            Mensagem = "Solicitação encerrada com sucesso !",
                            TipoChamado = "Novo"
                        });
                }
                else
                {
                    var Cookie = Request.Cookies.Get("CookieFarm");
                    var Login = Criptografa.Descriptografar(Cookie.Value);

                    var DadosUsuario = VerificaDados.RecuperaDadosUsuarios(Login);
                    var IdInteracao = VerificaDados.CadastrarInteracao(Dados["IdSolicitacao"], Dados["Descricao"],
                        DadosUsuario[0]["id"], "N");

                    var lista = postedFiles.ToList();

                    for (var i = 0; i < lista.Count; i++)
                        if (lista[i] != null)
                        {
                            var NomeArquivo = Path.GetFileName(lista[i].FileName);
                            byte[] fileData = null;
                            using (var binaryReader = new BinaryReader(lista[i].InputStream))
                            {
                                fileData = binaryReader.ReadBytes(lista[i].ContentLength);
                            }

                            VerificaDados.InserirAnexo(IdInteracao, fileData, lista[i].ContentType, NomeArquivo);
                        }


                    return RedirectToAction("InteracaoChamado", "Webdesk",
                        new
                        {
                            IdChamado = Dados["IdSolicitacao"],
                            Mensagem = "Interação adicionada com sucesso !",
                            TipoChamado = "Novo"
                        });
                }
            }

            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult IniciarAtendimento(string IdSolicitacao)
        {
            var VerificaDados = new QueryMysqlWebdesk();
            var Cookie = Request.Cookies.Get("CookieFarm");
            var Login = Criptografa.Descriptografar(Cookie.Value);
            var DadosUsuario = VerificaDados.RecuperaDadosUsuarios(Login);

            VerificaDados.CadastrarInteracao(IdSolicitacao, " Atendimento iniciado por " + DadosUsuario[0]["nome"],
                DadosUsuario[0]["id"], "S");
            VerificaDados.IniciarAtendimentoSolicitacao(IdSolicitacao);


            return RedirectToAction("InteracaoChamado", "Webdesk",
                new {IdChamado = IdSolicitacao, Mensagem = "Interação adicionada com sucesso !", TipoChamado = "Novo"});
        }
    }
}