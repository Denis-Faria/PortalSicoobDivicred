﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using PortalSicoobDivicred.Aplicacao;
using PortalSicoobDivicred.Models;
using ElasticEmailClient;

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
                var DadosUsuarioBanco = VerificaDados.RecuperaDadosUsuarios(Login);

                TempData["NomeLateral"] = DadosUsuarioBanco[0]["login"];
                TempData["EmailLateral"] = DadosUsuarioBanco[0]["email"];

                TempData["ImagemPerfil"] = "https://docs.google.com/uc?id=0B2CLuTO3N2_obWdkajEzTmpGeU0";
                return View();
            }
            return RedirectToAction("Login", "Login");
        }

        public ActionResult Dashboard()
        {
            return PartialView("Dashboard");
        }

        public ActionResult Curriculo(string Ordenacao)
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

        public ActionResult GerenciarVaga(string IdVaga)
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

        [HttpPost]
        public ActionResult CadastrarVaga(Vagas Vaga, FormCollection Vagas)
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
                var Email = CadastrarVaga.CadastrarVaga(Vaga.Descricao, Areas, Vaga.Salario.Replace(",", "."), Vaga.Requisitos,
                    Vaga.Titulo, Vaga.Beneficio);
                for (int j = 0; j < Email.Count; j++)
                {
                    TodosEmail = TodosEmail + ";" + Email[j]["email"];
                }
                Api.ApiKey = "edff66b8-adb7-461e-9f3f-fd1649cedefa";
                string[] recipients = { TodosEmail };
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
                    new { Acao = "Curriculo", Mensagem = "Vaga cadastrada com sucesso !" });
            }
            return RedirectToAction("Principal", new { Acao = "Curriculo" });
        }

        public async Task<ActionResult> PerfilCandidato(string cpf)
        {
            var DadosUsuario = new Usuario();
            var VerificaDados = new QuerryMysql();

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
                    TempData["NomeVaga" +i] = DadosProcessosSeletivos[i]["nomevaga"];
                    TempData["Escrita"+i] = DadosProcessosSeletivos[i]["prova"];
                    if (Convert.ToInt32(DadosProcessosSeletivos[i]["prova"]) >= 70)
                    {
                        TempData["CorEscrita"+i] = "green";
                    }
                    else
                    {
                        TempData["CorEscrita"+i] = "red";
                        TempData["Aprovado" + i] = "red";
                    }
                    TempData["Psicologico"+i] = DadosProcessosSeletivos[i]["psicologico"];
                    if (DadosProcessosSeletivos[i]["psicologico"].Equals("Aprovado"))
                    {
                        TempData["CorPsicologico"+i] = "green";
                    }
                    else
                    {
                        TempData["CorPsicologico"+i] = "red";
                        TempData["Aprovado" + i] = "red";
                    }
                    TempData["Gerente"+i] = DadosProcessosSeletivos[i]["gerente"];
                    if (DadosProcessosSeletivos[i]["gerente"].Equals("Aprovado"))
                    {
                        TempData["CorGerente"+i] = "green";
                    }
                    else
                    {
                        TempData["CorGerente"+i] = "red";
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
                var client = new HttpClient();
                var client2 = new WebClient();
                var bytes = await client.GetByteArrayAsync("https://portalsicoobdivicred.com.br/Uploads/" +
                                                           DadosUsuarioBanco[0]["idarquivogoogle"] + "");
                var data = client2.DownloadString("https://portalsicoobdivicred.com.br/Uploads/" +
                                                  DadosUsuarioBanco[0]["idarquivogoogle"] + "");
                var contentType = client2.Headers["Content-Type"];

                TempData["ImagemPerfil"] = "data:" + contentType + ";base64," + Convert.ToBase64String(bytes);
            }
            else
            {
                var client = new HttpClient();

                var bytes = await client.GetByteArrayAsync(
                    "https://docs.google.com/uc?id=0B2CLuTO3N2_obWdkajEzTmpGeU0");

                TempData["ImagemPerfil"] = "data:image/png;base64," + Convert.ToBase64String(bytes);
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
            TempData["estadocivil"] = VerificaDados.RecuperaEstadoCivilCandidato(DadosUsuarioBanco[0]["estadocivil"]);
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

        [HttpPost]
        public ActionResult FiltrarPerfilVaga(FormCollection Filtros)
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
                    Query = Query + " AND  YEAR(FROM_DAYS(TO_DAYS(NOW())-TO_DAYS(a.datanascimento))) BETWEEN 18 and 25";
                else if (FaixaEtaria.Contains("25 - 30"))
                    Query = Query + " AND  YEAR(FROM_DAYS(TO_DAYS(NOW())-TO_DAYS(a.datanascimento))) BETWEEN 25 and 30";
                else if (FaixaEtaria.Contains("30 - 40"))
                    Query = Query + " AND  YEAR(FROM_DAYS(TO_DAYS(NOW())-TO_DAYS(a.datanascimento))) BETWEEN 30 and 40";
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
                Query = Query + " AND (SELECT count(id) FROM dadosescolares where nomecurso like'%" + CursoGraduacao +
                        "%' and idcandidato=a.id)>=1";
            if (Profissional.Length > 0)
            {
                Query = Query + " AND (SELECT count(id) FROM dadosprofissionais where atividadedesempenhada like'%"+Profissional+"%' and idcandidato=a.id)>=1";
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

        public ActionResult EncerrarVaga(string IdVaga)
        {
            var RecuperaDados = new QuerryMysql();
            var Vaga = IdVaga.Split('-');
            RecuperaDados.EncerrarVaga(Vaga[0]);
            return RedirectToAction("Principal", new { Acao = "Curriculo", Mensagem = "Vaga encerrada com sucesso !" });
        }

        public ActionResult AbrirProcesso(FormCollection Curriculos)
        {

            QuerryMysql IniciarProcesso = new QuerryMysql();
            for (int i = 0; i < Curriculos.Count; i++)
            {
                var IdVaga = Curriculos["vaga"];
                var Vaga = IdVaga.Split(',');
                if (!Curriculos.Keys[i].Contains("vaga") && !Curriculos.Keys[i].Equals("Alerta"))
                {
                    IniciarProcesso.IniciarProcessoSeletivo(Curriculos.Keys[i], Vaga[0]);
                    var Email = IniciarProcesso.RecuperaEmail(Curriculos.Keys[i]);
                    Api.ApiKey = "edff66b8-adb7-461e-9f3f-fd1649cedefa";
                    string[] recipients = { Email[0]["email"]+"; rh@divicred.com.br" };
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
                    IniciarProcesso.CadastrarAlertaEspecifico(Curriculos["Alerta"], Email[0]["id"]);
                }
                var DadosCurriculos = IniciarProcesso.RecuperaCurriculosHistorico(Vaga[0]);
                for (int j = 0; j < DadosCurriculos.Count; j++)
                {
                    if (!Curriculos.AllKeys.Contains(DadosCurriculos[j]["cpf"]))
                    {
                        Api.ApiKey = "edff66b8-adb7-461e-9f3f-fd1649cedefa";
                        string[] recipients = {DadosCurriculos[j]["email"] + "; rh@divicred.com.br" };
                        var subject = "Atualização de Status";
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
                }

            }
            return RedirectToAction("Principal",
                new { Acao = "Curriculo", Mensagem = "Processo seletivo inciado com sucesso !" });
        }

        public async Task<ActionResult> PerfilCandidatoProcesso(string IdVaga)
        {
            var RecuperaDados = new QuerryMysql();
            var DadosProcesso = RecuperaDados.RecuperaCurriculosProcesso(IdVaga);
            TempData["IdVaga"] = IdVaga;
            TempData["TotalCurriculo"] = DadosProcesso.Count;

            for (int i = 0; i < DadosProcesso.Count; i++)
            {
                if (DadosProcesso[i]["prova"]==null)
                {
                    TempData["ResultadoProva"] = DadosProcesso[i]["prova"];
                    TempData["EscondePsicologico"] = "hidden disabled";
                    TempData["EscondeGerente"] = "hidden disabled";
                }
                if (DadosProcesso[i]["psicologico"]==null)
                {
                    TempData["ResultadoProva"] = DadosProcesso[i]["prova"];
                    TempData["ResultadoPsicologico"] = DadosProcesso[i]["psicologico"];
                    TempData["EscondeGerente"] = "hidden disabled";
                }
                else
                {
                    TempData["ResultadoProva"] = DadosProcesso[i]["prova"];
                    TempData["ResultadoPsicologico"] = DadosProcesso[i]["psicologico"];
                    TempData["ResultadoGerente"] = DadosProcesso[i]["gerente"];
                    TempData["EscondePsicologico"] = "";
                    TempData["EscondeGerente"] = "";
                }
                TempData["Cpf" + i] = DadosProcesso[i]["cpf"];

                TempData["Email" + i] = DadosProcesso[i]["email"];
                TempData["Nome" + i] = DadosProcesso[i]["nome"];
            }

            return PartialView("PerfilProcesso");
        }

        public ActionResult CadastrarAlerta(FormCollection Alerta)
        {
            var CadastrarAlertas = new QuerryMysql();

            CadastrarAlertas.CadastrarAlerta(Alerta["TextAlerta"]);
            TempData["Opcao"] = "Curriculo";

            return RedirectToAction("Principal",
                new { Acao = "Curriculo", Mensagem = "Alerta cadastrado com sucesso !" });
        }

        public ActionResult CadastrarMensagem(FormCollection Mensagem)
        {
            var CadastrarMensagens = new QuerryMysql();

            CadastrarMensagens.CadastrarMensagem(Mensagem["TextMensagem"]);
            TempData["Opcao"] = "Curriculo";

            return RedirectToAction("Principal",
                new { Acao = "Curriculo", Mensagem = "Mensagem cadastrada com sucesso !" });
        }


        public ActionResult EncerraProcesso(FormCollection Resultado)
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
                    Status.AtualizarProcessoSeletivoStatus(Cpf[1], Resultado["Vaga"], Resultado[i]);
                    if (Resultado[i].Equals("Aprovado"))
                    {
                        var Email = Status.RecuperaEmail(Cpf[1]);
                        Api.ApiKey = "edff66b8-adb7-461e-9f3f-fd1649cedefa";
                        string[] recipients = { Email[0]["email"] + "; rh@divicred.com.br" };
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
                        Status.CadastrarAlertaEspecifico(Resultado["Alerta"],Email[0]["id"]);
                    }
                    else
                    {
                        var Email = Status.RecuperaEmail(Cpf[1]);
                        Api.ApiKey = "edff66b8-adb7-461e-9f3f-fd1649cedefa";
                        string[] recipients = { Email[0]["email"] + "; rh@divicred.com.br" };
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
                new { Acao = "Curriculo", Mensagem = "Processo seletivo encerrado com sucesso !" });
        }

        public ActionResult ResultadoProcesso(string IdVaga)
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
    }
}