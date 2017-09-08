using System;
using System.Collections.Generic;
using System.Web.Mvc;
using PortalSicoobDivicred.Aplicacao;
using PortalSicoobDivicred.Models;

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
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult Dashboard()
        {
            return PartialView("Dashboard");
        }

        public ActionResult Curriculo()
        {
            QuerryMysql CarregaDados = new QuerryMysql();
            var DadosCurriculos = CarregaDados.RecuperaCurriculos();
            var DadosVagas = CarregaDados.RecuperaVagas();

            TempData["TotalVagas"] = DadosVagas.Count;
            TempData["TotalCurriculo"] = DadosCurriculos.Count;

            for (int i = 0; i < DadosCurriculos.Count; i++)
            {
                TempData["Nome" + i] = DadosCurriculos[i]["nome"];
                TempData["Cpf" + i] = DadosCurriculos[i]["cpf"];
                TempData["Email" + i] = DadosCurriculos[i]["email"];
                TempData["Cidade"+i]= DadosCurriculos[i]["cidade"];
                TempData["Area" + i] = DadosCurriculos[i]["descricao"].Replace(";", " ");
                if (DadosCurriculos[i]["status"].Equals("S"))
                {
                    TempData["Status" + i] = "green";
                }
                else
                {
                    TempData["Status" + i] = "red";
                }

                if (DadosCurriculos[i]["idarquivogoogle"].Equals("0"))
                {
                    TempData["Imagem" + i] = "https://docs.google.com/uc?id=0B2CLuTO3N2_obWdkajEzTmpGeU0";
                }
                else
                {
                    TempData["Imagem" + i] = "https://portalsicoobdivicred.com.br/Uploads/" + DadosCurriculos[i]["idarquivogoogle"] + "";
                }
            }

            for (int i = 0; i < DadosVagas.Count; i++)
            {
                TempData["IdVaga" + i] = DadosVagas[i]["id"];
                TempData["Titulo" + i] = DadosVagas[i]["titulo"];
                TempData["Descricao" + i] = DadosVagas[i]["descricao"];
                TempData["AreaVaga" + i] = DadosVagas[i]["areadeinteresse"].Replace(";", " ");
                if (DadosVagas[i]["ativa"].Equals("S"))
                {
                    TempData["StatusVaga" + i] = "green";
                }
                else
                {
                    TempData["StatusVaga" + i] = "red";
                }

            }


            return PartialView("Curriculo");
        }

        public ActionResult GerenciarVaga(string IdVaga)
        {
            QuerryMysql RecuperaDados = new QuerryMysql();
            var DadosVagas = RecuperaDados.RecuperaVagasId(IdVaga);

            TempData["TituloVaga"] = IdVaga+"-"+DadosVagas[0]["titulo"];
            TempData["DescricaoVaga"] = DadosVagas[0]["descricao"];
            TempData["IdVaga"] = IdVaga;
            

            var DadosCurriculos = RecuperaDados.RecuperaCurriculosHistorico(IdVaga);
            TempData["TotalCurriculo"] = DadosCurriculos.Count;


            for (int i = 0; i < DadosCurriculos.Count; i++)
            {
                TempData["Nome" + i] = DadosCurriculos[i]["nome"];
                TempData["Email" + i] = DadosCurriculos[i]["email"];
                TempData["Cidade" + i] = DadosCurriculos[i]["cidade"];
                TempData["Certificacao" + i] = DadosCurriculos[i]["certificacao"];


                if (DadosCurriculos[i]["idarquivogoogle"].Equals("0"))
                {
                    TempData["Imagem" + i] = "https://docs.google.com/uc?id=0B2CLuTO3N2_obWdkajEzTmpGeU0";
                }
                else
                {
                    TempData["Imagem" + i] = "https://portalsicoobdivicred.com.br/Uploads/" + DadosCurriculos[i]["idarquivogoogle"] + "";
                }
            }
            if (DadosVagas[0]["ativa"].Equals("S"))
            {
                TempData["Ativa"] = "";
                TempData["Dica"] = "Clique para encerrar esta vaga.";
                TempData["DicaProcesso"] = "Clique para iniciar um processo seletivo";
            }
            else
            {
                TempData["Ativa"] = "disabled";
                TempData["Dica"] = "Esta vaga já esta encerrada.";
            }
            return PartialView("GerenciarVaga");
        }

        [HttpPost]
        public ActionResult CadastrarVaga(Vagas Vaga, FormCollection Vagas)
        {
            QuerryMysql CadastrarVaga = new QuerryMysql();

            if (Vagas.Count == 6)
            {
                ModelState.AddModelError("", "Favor selecionar uma área de interesse !");
            }
            if (Vagas["DataProcesso"].Equals("null"))
            {
                ModelState.AddModelError("", "Favor informar a data do processo seletivo!");
            }

            if (ModelState.IsValid)
            {
                string Areas = "";
                for (int i = 6; i < Vagas.Count; i++)
                {
                    Areas = Areas + Vagas[i] + ";";
                }
                CadastrarVaga.CadastrarVaga(Vaga.Descricao, Areas, Vaga.Salario.Replace(",", "."), Vaga.Requisitos, Vaga.Titulo, Convert.ToDateTime(Vagas["DataProcesso"].ToString()), Vaga.Local);
                TempData["Opcao"] = "Curriculo";

                return RedirectToAction("Principal", new { Acao = "Curriculo", Mensagem = "Vaga cadastrada com sucesso !" });
            }
            else
            {
                return RedirectToAction("Principal", new { Acao = "Curriculo" });
            }






        }

        public ActionResult PerfilCandidato(string cpf)
        {
            var DadosUsuario = new Usuario();
            QuerryMysql VerificaDados = new QuerryMysql();

            var DadosUsuarioBanco = VerificaDados.RecuperaDadosCandidato(cpf);

            if (!DadosUsuarioBanco[0]["idarquivogoogle"].Equals("0"))
                TempData["ImagemPerfil"] = "https://portalsicoobdivicred.com.br/Uploads/" + DadosUsuarioBanco[0]["idarquivogoogle"] + "";
            else
                TempData["ImagemPerfil"] = "https://docs.google.com/uc?id=0B2CLuTO3N2_obWdkajEzTmpGeU0";

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
            TempData["Escolaridade"] = VerificaDados.RecuperaEscolaridadeCandidato(DadosUsuarioBanco[0]["idtipoescolaridade"]);
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

            return View(DadosUsuario);
        }

        [HttpPost]
        public ActionResult FiltrarPerfilVaga(FormCollection Filtros)
        {
            QuerryMysql Filtrar = new QuerryMysql();
            var Sexo = Filtros["FiltroSexo"];
            var Cidade = Filtros["FiltroCidade"];
            var Graduacao = Filtros["FiltroGraduacao"];
            var AnoFormacao = Filtros["FiltroAnoFormacao"];
            var FaixaEtaria = Filtros["FiltroFaixaEtaria"];
            var Certificacao = Filtros["FiltroCertificacao"];
            var IdVaga = Filtros["TituloVaga"].ToString().Split('-');
            var Query = "select a.nome,a.email, a.idarquivogoogle,a.cidade,a.certificacao from historicos c LEFT JOIN candidatos a on c.idcandidato=a.id INNER JOIN candidatos u2 on (c.idcandidato=u2.id) where c.idvaga="+IdVaga[0]+"";
            if (!Sexo.Equals("Sexo"))
            {
                Query = Query + " AND a.sexo = '"+Sexo+"'";
            }
            if (Cidade.Length > 0)
            {
                Query = Query + " AND a.cidade like'%"+Cidade+"%'";
            }
            if (Certificacao.Length > 0)
            {
                Query = Query + " AND a.certificacao like'%"+Certificacao+"%'";
            }
            if (!FaixaEtaria.Equals("Faixa Etária"))
            {
                if (FaixaEtaria.Contains(" < 18"))
                {
                    Query = Query + " AND  YEAR(FROM_DAYS(TO_DAYS(NOW())-TO_DAYS(a.datanascimento)))<18";
                }
                else if (FaixaEtaria.Contains("18 - 25"))
                {
                    Query = Query + " AND  YEAR(FROM_DAYS(TO_DAYS(NOW())-TO_DAYS(a.datanascimento))) BETWEEN 18 and 25";
                }
                else if (FaixaEtaria.Contains("25 - 30"))
                {
                    Query = Query + " AND  YEAR(FROM_DAYS(TO_DAYS(NOW())-TO_DAYS(a.datanascimento))) BETWEEN 25 and 30";
                }
                else if (FaixaEtaria.Contains("30 - 40"))
                {
                    Query = Query + " AND  YEAR(FROM_DAYS(TO_DAYS(NOW())-TO_DAYS(a.datanascimento))) BETWEEN 30 and 40";
                }
                else if (FaixaEtaria.Contains(" 40 > "))
                {
                    Query = Query + " AND  YEAR(FROM_DAYS(TO_DAYS(NOW())-TO_DAYS(a.datanascimento)))>40";
                }
            }
            if (!Graduacao.Equals("Graduação"))
            {
                var Escolaridade = Graduacao.Split('|');
                var IdsEscolaridade = Filtrar.RecuperaIdEscolaridade(Escolaridade[0]);
                var escolaridades = " and (a.idtipoescolaridade="+IdsEscolaridade[0]["id"];

                for (int i = 1; i < IdsEscolaridade.Count; i++)
                {
                    escolaridades=escolaridades+" or "+IdsEscolaridade[i]["id"];
                }
                Query = Query + escolaridades + ")";
            }
            if (AnoFormacao.Length > 0)
            {
                Query = Query + " AND (SELECT count(id) FROM dadosescolares where anofim <="+AnoFormacao+ " and idcandidato=a.id)>=1";
            }

            var DadosCurriculos = Filtrar.FiltroVaga(Query);
            
            var DadosVagas = Filtrar.RecuperaVagasId(IdVaga[0]);

            TempData["TituloVaga"] = IdVaga[0] + "-"+DadosVagas[0]["titulo"];
            TempData["DescricaoVaga"] = DadosVagas[0]["descricao"];
            TempData["TotalCurriculo"] = DadosCurriculos.Count;


            for (int i = 0; i < DadosCurriculos.Count; i++)
            {
                TempData["Nome" + i] = DadosCurriculos[i]["nome"];
                TempData["Email" + i] = DadosCurriculos[i]["email"];
                TempData["Cidade" + i] = DadosCurriculos[i]["cidade"];
                TempData["Certificacao" + i] = DadosCurriculos[i]["certificacao"];


                if (DadosCurriculos[i]["idarquivogoogle"].Equals("0"))
                {
                    TempData["Imagem" + i] = "https://docs.google.com/uc?id=0B2CLuTO3N2_obWdkajEzTmpGeU0";
                }
                else
                {
                    TempData["Imagem" + i] = "https://portalsicoobdivicred.com.br/Uploads/" + DadosCurriculos[i]["idarquivogoogle"] + "";
                }
            }
            return PartialView("GerenciarVaga");
        }

        public ActionResult EncerrarVaga(string IdVaga)
        {
            QuerryMysql RecuperaDados = new QuerryMysql();
            var Vaga = IdVaga.Split('-');
            RecuperaDados.EncerrarVaga(Vaga[0]);
            return RedirectToAction("Principal", new {Acao = "Curriculo", Mensagem = "Vaga encerrada com sucesso !"});
        }

        public void AbrirProcesso(string IdVaga)
        {
            
        }
    }
}