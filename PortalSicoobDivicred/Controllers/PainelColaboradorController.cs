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
    public class PainelColaboradorController : Controller
    {
        // GET: PainelColaborador
        public ActionResult Perfil()
        {
            var VerificaDados = new QuerryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Cookie = Request.Cookies.Get("CookieFarm");
                var Login = Criptografa.Descriptografar(Cookie.Value);


                var DadosTabelaFuncionario = VerificaDados.RecuperaDadosFuncionariosTabelaFuncionariosPerfil(Login);
                var DocumentosUpados = VerificaDados.RecuperaDocumentosFuncionario(Login);

                for (int i = 0; i < DocumentosUpados.Count; i++)
                {
                    TempData["Status" + DocumentosUpados[i]["nomearquivo"]] = "is-primary";
                    TempData["Nome" + DocumentosUpados[i]["nomearquivo"]] = "Arquivo Enviado";
                }
                Funcionario DadosFuncionario = new Funcionario();

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
                {
                    TempData["Foto"] = "http://bulma.io/images/placeholders/128x128.png";
                }
                else
                {
                    TempData["Foto"] = "/Uploads/" + DadosTabelaFuncionario[0]["foto"];
                }
                TempData["IdEstadoCivil"] = DadosTabelaFuncionario[0]["idestadocivil"];
                TempData["IdSexo"] = DadosTabelaFuncionario[0]["sexo"];
                TempData["IdEtnia"] = DadosTabelaFuncionario[0]["etnia"];
                TempData["IdFormacao"] = DadosTabelaFuncionario[0]["idescolaridade"];
                TempData["IdSetor"] = DadosTabelaFuncionario[0]["idsetor"];
                TempData["NomeFuncionario"] = DadosTabelaFuncionario[0]["nome"];
                TempData["Funcao"] = DadosTabelaFuncionario[0]["funcao"];
                TempData["DataAdmissao"] =
                    Convert.ToDateTime(DadosTabelaFuncionario[0]["admissao"]).ToString("dd/MM/yyyy");

                var EstadoCivil = VerificaDados.RetornaEstadoCivil();
                var Sexo = VerificaDados.RetornaSexo();
                var Etnia = VerificaDados.RetornaEtnia();
                var Formacao = VerificaDados.RetornaFormacao();
                var Setor = VerificaDados.RetornaSetor();

                DadosFuncionario.EstadoCivil = EstadoCivil;
                DadosFuncionario.Sexo = Sexo;
                DadosFuncionario.Etnia = Etnia;
                DadosFuncionario.Formacao = Formacao;
                DadosFuncionario.Setor = Setor;

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
            var VerificaDados = new QuerryMysql();
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
        public ActionResult AtualizaDadosPessoais(Funcionario DadosFuncionario)
        {
            var VerificaDados = new QuerryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Cookie = Request.Cookies.Get("CookieFarm");
                var Login = Criptografa.Descriptografar(Cookie.Value);

                var DescricaoSexo = "";
                if (DadosFuncionario.DescricaoSexo == null)
                {
                    DescricaoSexo = "NÃO INFORMOU";
                }
                else
                {
                    DescricaoSexo = DadosFuncionario.DescricaoSexo;
                }
                VerificaDados.AtualizaDadosFuncionarioDadosPessoais(DadosFuncionario.NomeFuncionario,
                    DadosFuncionario.CpfFuncionario, DadosFuncionario.RgFuncionario,
                    DadosFuncionario.PisFuncionario, DadosFuncionario.DataNascimentoFuncionario,
                    DadosFuncionario.IdSexo.ToString(), DescricaoSexo, DadosFuncionario.IdEtnia.ToString(),
                    DadosFuncionario.IdEstadoCivil.ToString(), DadosFuncionario.IdFormacao.ToString(),
                    DadosFuncionario.FormacaoAcademica,
                    Login, DadosFuncionario.Email, DadosFuncionario.PA,
                    DadosFuncionario.Rua, DadosFuncionario.Numero, DadosFuncionario.Bairro, DadosFuncionario.Cidade);

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
            var InserirFoto = new QuerryMysql();
            var Logado = InserirFoto.UsuarioLogado();
            if (Logado)
            {
                var Arquivo = Request.Files[0];
                var NomeArquivo = Path.GetFileName(Arquivo.FileName);
                var Caminho = Path.Combine(Server.MapPath("~/Uploads/"), NomeArquivo);
                if (System.IO.File.Exists(Caminho))
                {
                    int counter = 1;
                    var tempfileName = "";
                    while (System.IO.File.Exists(Caminho))
                    {

                        tempfileName = counter.ToString() + NomeArquivo;
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
            var InserirFoto = new QuerryMysql();
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
            var VerificaDados = new QuerryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var Cookie = Request.Cookies.Get("CookieFarm");
                var Login = Criptografa.Descriptografar(Cookie.Value);
                var DataNascimentoFilho = "";
                if (DadosFuncionario.DataNascimentoFilho == null)
                {
                    DataNascimentoFilho = "NÂO TEM";
                }
                else
                {
                    DataNascimentoFilho = DadosFuncionario.DataNascimentoFilho;
                }
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
            var VerificaDados = new QuerryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {
                var CarregaDados = new QuerryMysql();
                var DadosColaborador = CarregaDados.RecuperaDadosFuncionarios();

                TempData["TotalColaborador"] = DadosColaborador.Count;

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
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult PerfilFuncionario(string Login)
        {
            var VerificaDados = new QuerryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if (Logado)
            {

                var DadosTabelaFuncionario = VerificaDados.RecuperaDadosFuncionariosTabelaFuncionariosPerfil(Login);
                var DocumentosUpados = VerificaDados.RecuperaDocumentosFuncionario(Login);

                for (int i = 0; i < DocumentosUpados.Count; i++)
                {
                    TempData["Status" + DocumentosUpados[i]["nomearquivo"]] = "is-primary";
                    TempData["Nome" + DocumentosUpados[i]["nomearquivo"]] = "Arquivo Enviado";
                }
                Funcionario DadosFuncionario = new Funcionario();

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
                {
                    TempData["Foto"] = "http://bulma.io/images/placeholders/128x128.png";
                }
                else
                {
                    TempData["Foto"] = "/Uploads/" + DadosTabelaFuncionario[0]["foto"];
                }
                TempData["IdEstadoCivil"] = DadosTabelaFuncionario[0]["idestadocivil"];
                TempData["IdSexo"] = DadosTabelaFuncionario[0]["sexo"];
                TempData["IdEtnia"] = DadosTabelaFuncionario[0]["etnia"];
                TempData["IdFormacao"] = DadosTabelaFuncionario[0]["idescolaridade"];
                TempData["IdSetor"] = DadosTabelaFuncionario[0]["idsetor"];
                TempData["NomeFuncionario"] = DadosTabelaFuncionario[0]["nome"];
                TempData["Funcao"] = DadosTabelaFuncionario[0]["funcao"];
                TempData["DataAdmissao"] =
                    Convert.ToDateTime(DadosTabelaFuncionario[0]["admissao"]).ToString("dd/MM/yyyy");

                var EstadoCivil = VerificaDados.RetornaEstadoCivil();
                var Sexo = VerificaDados.RetornaSexo();
                var Etnia = VerificaDados.RetornaEtnia();
                var Formacao = VerificaDados.RetornaFormacao();
                var Setor = VerificaDados.RetornaSetor();

                DadosFuncionario.EstadoCivil = EstadoCivil;
                DadosFuncionario.Sexo = Sexo;
                DadosFuncionario.Etnia = Etnia;
                DadosFuncionario.Formacao = Formacao;
                DadosFuncionario.Setor = Setor;

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


        public ActionResult AtualizarDadosRh(string Login)
        {
            return RedirectToAction("Principal", "Principal",
                new
                {
                    Acao = "Perfil",
                    Mensagem = "Formulário Pessoal atualizado com sucesso !",
                    Controlle = "PainelColaborador"
                });
        }
    }
}