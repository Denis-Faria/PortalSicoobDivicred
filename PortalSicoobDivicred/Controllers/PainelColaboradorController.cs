using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using PortalSicoobDivicred.Aplicacao;
using PortalSicoobDivicred.Models;

namespace PortalSicoobDivicred.Controllers
{
    public class PainelColaboradorController : Controller
    {
        public ActionResult Perfil(string mensagem)
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


                    var dadosTabelaFuncionario = verificaDados.RecuperadadosFuncionariosTabelaFuncionariosPerfil( login );
                    var vinculoExtra = verificaDados.RecuperaVinculoExtra( dadosTabelaFuncionario[0]["id"] );
                    var documentosUpados = verificaDados.RetornaDocumentosFuncionario( login );

                    for(var i = 0; i < documentosUpados.Rows.Count; i++)
                    {
                        TempData["Status" + documentosUpados.Rows[i]["nomearquivo"]] = "is-primary";
                        TempData["Nome" + documentosUpados.Rows[i]["nomearquivo"]] = "Arquivo Enviado";

                        var bytes = (byte[])documentosUpados.Rows[i]["arquivo"];
                        var img64 = Convert.ToBase64String( bytes );
                        ValidaImagem image = new ValidaImagem();
                        if(!image.IsValidImage( bytes ))
                        {
                            var img64Url = string.Format( "data:application/pdf;base64,{0}", img64 );
                            TempData["Imagem" + documentosUpados.Rows[i]["nomearquivo"]] = img64Url;
                            TempData["ValidaTipo" + documentosUpados.Rows[i]["nomearquivo"]] = "pdf";
                        }
                        else
                        {
                            var img64Url = string.Format( "data:image/;base64,{0}", img64 );
                            TempData["Imagem" + documentosUpados.Rows[i]["nomearquivo"]] = img64Url;
                            TempData["ValidaTipo" + documentosUpados.Rows[i]["nomearquivo"]] = "imagem";
                        }
                    }




                    var tipos = dadosTabelaFuncionario[0]["tipodependente"].ToString().Split( ';' );
                    for(var i = 0; i < 10; i++)
                        if(tipos.Contains( i.ToString() ))
                            TempData["Check" + i] = "checked";
                        else
                            TempData["Check" + i] = "";


                    var dadosFuncionario = new Funcionario();
                    if(vinculoExtra.Count > 0)
                    {
                        TempData["MostraVinculo"] = "";
                        dadosFuncionario.MultiploCnpj = vinculoExtra[0]["cnpj"];
                        dadosFuncionario.MultiploNomeEmpresa = vinculoExtra[0]["nomeempresa"];
                        dadosFuncionario.MultiploRemuneracao = vinculoExtra[0]["remuneracao"];
                        dadosFuncionario.MultiploComentario = vinculoExtra[0]["comentario"];
                    }
                    else
                    {
                        TempData["MostraVinculo"] = "style=display:none;";
                    }

                    dadosFuncionario.NomeFuncionario = dadosTabelaFuncionario[0]["nome"];
                    dadosFuncionario.CpfFuncionario = dadosTabelaFuncionario[0]["cpf"];
                    dadosFuncionario.RgFuncionario = dadosTabelaFuncionario[0]["rg"];
                    dadosFuncionario.PisFuncionario = dadosTabelaFuncionario[0]["pis"];
                    dadosFuncionario.DataNascimentoFuncionario =
                        Convert.ToDateTime( dadosTabelaFuncionario[0]["datanascimento"] ).ToString( "dd/MM/yyyy" );
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
                    dadosFuncionario.DescricaoSexo = dadosTabelaFuncionario[0]["descricaosexo"];
                    dadosFuncionario.Matricula = dadosTabelaFuncionario[0]["matricula"];
                    dadosFuncionario.NumeroCtps = dadosTabelaFuncionario[0]["numeroctps"];
                    dadosFuncionario.SerieCtps = dadosTabelaFuncionario[0]["seriectps"];
                    dadosFuncionario.UfCtps = dadosTabelaFuncionario[0]["ufctps"];
                    dadosFuncionario.EmissaoCtps = Convert.ToDateTime( dadosTabelaFuncionario[0]["dataemissaoctps"] ).Date
                        .ToString( "dd/MM/yyyy" );
                    dadosFuncionario.AnoPrimeiroEmprego = dadosTabelaFuncionario[0]["anoprimeiroemprego"];
                    dadosFuncionario.IdTipoConta = Convert.ToInt32( dadosTabelaFuncionario[0]["idtipoconta"] );
                    dadosFuncionario.CodigoBanco = dadosTabelaFuncionario[0]["codigobanco"];
                    dadosFuncionario.Agencia = dadosTabelaFuncionario[0]["agencia"];
                    dadosFuncionario.ContaCorrente = dadosTabelaFuncionario[0]["contacorrente"];
                    dadosFuncionario.DadosDependentes = dadosTabelaFuncionario[0]["informacaodependente"];
                    dadosFuncionario.DependenteIrrf = dadosTabelaFuncionario[0]["dependenteirrpf"];
                    dadosFuncionario.CpfIrrf = dadosTabelaFuncionario[0]["cpfirrf"];
                    dadosFuncionario.DependenteFamilia = dadosTabelaFuncionario[0]["dependentesalariofamilia"];
                    dadosFuncionario.OrgaoEmissorRg = dadosTabelaFuncionario[0]["orgaoemissorrg"];
                    dadosFuncionario.Nacionalidade = dadosTabelaFuncionario[0]["nacionalidade"];
                    dadosFuncionario.LocalNascimento = dadosTabelaFuncionario[0]["localnascimento"];
                    dadosFuncionario.UfNascimento = dadosTabelaFuncionario[0]["ufnascimento"];
                    dadosFuncionario.TelefoneFixo = dadosTabelaFuncionario[0]["telefonefixo"];
                    dadosFuncionario.TelefoneCelular = dadosTabelaFuncionario[0]["telefonecelular"];
                    dadosFuncionario.NomeMae = dadosTabelaFuncionario[0]["nomemae"];
                    dadosFuncionario.NomePai = dadosTabelaFuncionario[0]["nomepai"];

                    dadosFuncionario.EmailSecundario = dadosTabelaFuncionario[0]["emailsecundario"];
                    dadosFuncionario.Cep = dadosTabelaFuncionario[0]["cep"];
                    dadosFuncionario.Complemento = dadosTabelaFuncionario[0]["complemento"];
                    dadosFuncionario.Pais = dadosTabelaFuncionario[0]["pais"];
                    dadosFuncionario.ResidenciaPropria = dadosTabelaFuncionario[0]["residenciapropria"];
                    dadosFuncionario.RecursoFgts = dadosTabelaFuncionario[0]["recursofgts"];
                    dadosFuncionario.DeficienteMotor = dadosTabelaFuncionario[0]["deficientemotor"];
                    dadosFuncionario.DeficienteVisual = dadosTabelaFuncionario[0]["deficientevisual"];
                    dadosFuncionario.DeficienteAuditivo = dadosTabelaFuncionario[0]["deficienteauditivo"];
                    dadosFuncionario.Reabilitado = dadosTabelaFuncionario[0]["reabilitado"];
                    dadosFuncionario.ObservacaoDeficiente = dadosTabelaFuncionario[0]["observacaodeficiente"];
                    dadosFuncionario.DataExpedicaoDocumentoRg =
                        Convert.ToDateTime( dadosTabelaFuncionario[0]["dataemissaorg"] ).Date;
                    dadosFuncionario.IdEstadoCivil = Convert.ToInt32( dadosTabelaFuncionario[0]["idestadocivil"] );
                    try
                    {
                        dadosFuncionario.IdEstadoCivilPais =
                            Convert.ToInt32( dadosTabelaFuncionario[0]["paisdivorciado"] );
                    }
                    catch
                    {
                        dadosFuncionario.IdEstadoCivilPais = 0;
                    }

                    try
                    {
                        dadosFuncionario.IdHorario = Convert.ToInt32( dadosTabelaFuncionario[0]["idhorariotrabalho"] );
                    }
                    catch
                    {
                        dadosFuncionario.IdHorario = 0;
                    }

                    dadosFuncionario.IdSexo = Convert.ToInt32( dadosTabelaFuncionario[0]["sexo"] );
                    dadosFuncionario.IdEtnia = Convert.ToInt32( dadosTabelaFuncionario[0]["etnia"] );
                    dadosFuncionario.IdFormacao = Convert.ToInt32( dadosTabelaFuncionario[0]["idescolaridade"] );
                    dadosFuncionario.IdSetor = Convert.ToInt32( dadosTabelaFuncionario[0]["idsetor"] );
                    dadosFuncionario.IdFuncao = Convert.ToInt32( dadosTabelaFuncionario[0]["funcao"] );
                    dadosFuncionario.NotificacaoEmail = dadosTabelaFuncionario[0]["notificacaoemail"];
                    dadosFuncionario.ContribuicaoSindical = dadosTabelaFuncionario[0]["contribuicaosindical"];

                    var formacoes = verificaDados.RetornaFormacaoFuncionario( dadosTabelaFuncionario[0]["id"] );

                    if(dadosTabelaFuncionario[0]["cnh"].Equals( "" ))
                    {
                        if(dadosTabelaFuncionario[0]["oc"].Equals( "" ))
                        {
                            TempData["ExibeDocumentoExtra"] = "style=display:none;";
                        }
                        else
                        {
                            TempData["ExibeDocumentoExtra"] = "";
                            dadosFuncionario.Cnh = dadosTabelaFuncionario[0]["cnh"];
                            dadosFuncionario.DataExpedicaoDocumentoCnh =
                                Convert.ToDateTime( dadosTabelaFuncionario[0]["dataexpedicaocnh"] ).Date;
                            dadosFuncionario.OrgaoEmissorCnh = dadosTabelaFuncionario[0]["orgaoemissorcnh"];
                            dadosFuncionario.DataValidadeCnh =
                                Convert.ToDateTime( dadosTabelaFuncionario[0]["datavalidadecnh"] );

                            dadosFuncionario.Oc = dadosTabelaFuncionario[0]["oc"];
                            dadosFuncionario.DataExpedicaoOc =
                                Convert.ToDateTime( dadosTabelaFuncionario[0]["dataexpedicaooc"] ).Date;
                            dadosFuncionario.OrgaoEmissorOc = dadosTabelaFuncionario[0]["orgaoemissoroc"];
                            dadosFuncionario.DataValidadeOc =
                                Convert.ToDateTime( dadosTabelaFuncionario[0]["datavalidadeoc"] );
                        }
                    }
                    else
                    {
                        TempData["ExibeDocumentoExtra"] = "";
                        dadosFuncionario.Cnh = dadosTabelaFuncionario[0]["cnh"];
                        dadosFuncionario.DataExpedicaoDocumentoCnh =
                            Convert.ToDateTime( dadosTabelaFuncionario[0]["dataexpedicaocnh"] ).Date;
                        dadosFuncionario.OrgaoEmissorCnh = dadosTabelaFuncionario[0]["orgaoemissorcnh"];
                        dadosFuncionario.DataValidadeCnh =
                            Convert.ToDateTime( dadosTabelaFuncionario[0]["datavalidadecnh"] );

                        dadosFuncionario.Oc = dadosTabelaFuncionario[0]["oc"];
                        dadosFuncionario.DataExpedicaoOc =
                            Convert.ToDateTime( dadosTabelaFuncionario[0]["dataexpedicaooc"] ).Date;
                        dadosFuncionario.OrgaoEmissorOc = dadosTabelaFuncionario[0]["orgaoemissoroc"];
                        dadosFuncionario.DataValidadeOc =
                            Convert.ToDateTime( dadosTabelaFuncionario[0]["datavalidadeoc"] );
                    }

                    TempData["TotalFormacao"] = formacoes.Count;
                    for(var j = 0; j < formacoes.Count; j++)
                    {
                        TempData["IdFormacaoExtra" + j] = "Extra|" + formacoes[j]["id"];
                        TempData["IdTipoFormacao" + j] = "Tipo|" + formacoes[j]["id"];
                        TempData["FormacaoExtra" + j] = formacoes[j]["descricao"];
                        TempData["TipoFormacaoExtra" + j] = formacoes[j]["tipoformacao"];
                    }


                    var certificacoesFuncao =
                        verificaDados.RetornaCertificacaoFuncao( dadosTabelaFuncionario[0]["funcao"] );
                    var idCertificacoes = certificacoesFuncao[0]["idcertificacao"].Split( ';' );

                    TempData["TotalCertificacao"] = idCertificacoes.Length;

                    for(var j = 0; j < idCertificacoes.Length; j++)
                        if(idCertificacoes[j].Length > 0)
                        {
                            var certificacoes = verificaDados.RetornaCertificacao( idCertificacoes[j] );
                            TempData["Certificacao" + j] = certificacoes[0]["descricao"];
                        }


                    if(dadosTabelaFuncionario[0]["confirmacaocertificacao"].Equals( "S" ))
                        TempData["ConfirmaCertificacao"] = "Checked";
                    else
                        TempData["ConfirmaCertificacao"] = "";
                    if(dadosTabelaFuncionario[0]["foto"] == null)
                        TempData["Foto"] = "http://bulma.io/images/placeholders/128x128.png";
                    else
                        TempData["Foto"] = "/Uploads/" + dadosTabelaFuncionario[0]["foto"];
                    var funcaoFuncionario = verificaDados.RetornaFuncaoFuncionario( dadosTabelaFuncionario[0]["funcao"] );
                    TempData["NomeFuncionario"] = dadosTabelaFuncionario[0]["nome"];
                    TempData["Funcao"] = funcaoFuncionario;
                    TempData["DataAdmissao"] =
                        Convert.ToDateTime( dadosTabelaFuncionario[0]["admissao"] ).ToString( "dd/MM/yyyy" );

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

                    if(dadosTabelaFuncionario[0]["estagiario"].Equals( "S" ))
                    {
                        TempData["Estagiario"] = "SIM";
                        TempData["DataEstagio"] = dadosTabelaFuncionario[0]["contratoestagio"];
                    }
                    else
                    {
                        TempData["Estagiario"] = "NÃO";
                        TempData["DataEstagio"] = "-";
                    }


                    var validacoes = new ValidacoesIniciais();

                    validacoes.AlertasUsuario( this, dadosTabelaFuncionario[0]["id"] );
                    validacoes.Permissoes( this, dadosTabelaFuncionario );
                    validacoes.DadosNavBar( this, dadosTabelaFuncionario );

                    return View( "Perfil", dadosFuncionario );
                }
            }

            return RedirectToAction( "Login", "Login" );
        }

        [HttpPost]
        public ActionResult AtualizarDadosProfissionais(Funcionario dadosFuncionario, FormCollection formulario)
        {
            var verificaDados = new QueryMysql();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                var cookie = Request.Cookies.Get( "CookieFarm" );
                if(cookie != null)
                {
                    var login = Criptografa.Descriptografar( cookie.Value );
                    var tiposDependentes = "";
                    var dadosTabelaFuncionario = verificaDados.RecuperadadosFuncionariosTabelaFuncionariosPerfil( login );
                    for(var i = 0; i < 10; i++)
                        try
                        {
                            if(formulario["check" + i].Equals( "true" )) tiposDependentes = tiposDependentes + i + ";";
                        }
                        catch
                        {
                            // ignored
                        }

                    var vinculoExtra = verificaDados.RecuperaVinculoExtra( dadosTabelaFuncionario[0]["id"] );

                    if(vinculoExtra.Count == 0)
                    {
                        if(dadosFuncionario.MultiploNomeEmpresa != null)
                            verificaDados.InserirVinculoEmpregaticio( dadosTabelaFuncionario[0]["id"],
                                dadosFuncionario.MultiploNomeEmpresa, dadosFuncionario.MultiploCnpj,
                                dadosFuncionario.MultiploRemuneracao, dadosFuncionario.MultiploComentario );
                    }
                    else
                    {
                        verificaDados.AtualizaVinculoEmpregaticio( dadosTabelaFuncionario[0]["id"],
                            dadosFuncionario.MultiploNomeEmpresa, dadosFuncionario.MultiploCnpj,
                            dadosFuncionario.MultiploRemuneracao, dadosFuncionario.MultiploComentario );
                    }

                    verificaDados.AtualizadadosFuncionarioProfissional( dadosFuncionario.IdSetor.ToString(),
                        dadosFuncionario.IdFuncao.ToString(), login, dadosFuncionario.NumeroCtps,
                        dadosFuncionario.SerieCtps, dadosFuncionario.UfCtps, dadosFuncionario.IdTipoConta,
                        dadosFuncionario.CodigoBanco,
                        dadosFuncionario.Agencia, dadosFuncionario.ContaCorrente, dadosFuncionario.DependenteIrrf,
                        dadosFuncionario.DependenteFamilia,
                        dadosFuncionario.DadosDependentes, tiposDependentes, dadosFuncionario.Matricula,
                        dadosFuncionario.AnoPrimeiroEmprego, dadosFuncionario.EmissaoCtps,
                        dadosFuncionario.CpfIrrf, dadosFuncionario.IdHorario );
                }


                return RedirectToAction( "Perfil", "PainelColaborador",
                    new { Mensagem = "Dados Profissionais atualizados com sucesso !" } );
            }

            return RedirectToAction( "Login", "Login" );
        }

        [HttpPost]
        public ActionResult AtualizaDadosPessoais(Funcionario dadosFuncionario, FormCollection formulario)
        {
            var verificaDados = new QueryMysql();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                var cookie = Request.Cookies.Get( "CookieFarm" );
                if(cookie != null)
                {
                    var login = Criptografa.Descriptografar( cookie.Value );

                    string descricaoSexo;
                    if(dadosFuncionario.DescricaoSexo == null)
                        descricaoSexo = "NÃO INFORMOU";
                    else
                        descricaoSexo = dadosFuncionario.DescricaoSexo;


                    var dadosTabelaFuncionario = verificaDados.RecuperadadosFuncionariosTabelaFuncionariosPerfil( login );

                    for(var i = 0; i < formulario.Count; i++)
                    {
                        if(formulario.AllKeys[i].Contains( "formacao" ))
                        {
                            var tipo = "Tipo " + formulario.GetKey( i ).Split( ' ' )[1];
                            verificaDados.InserirFormacao( formulario[i], dadosTabelaFuncionario[0]["id"],
                                formulario[tipo] );
                        }

                        if(formulario.AllKeys[i].Contains( "Extra" ))
                        {
                            var idFormacao = formulario.AllKeys[i].Split( '|' );
                            verificaDados.AtualizaFormacao( formulario[i], idFormacao[1],
                                formulario["Tipo|" + idFormacao[1]] );
                        }
                    }


                    verificaDados.AtualizadadosFuncionariodadosPessoais( dadosFuncionario.NomeFuncionario,
                        dadosFuncionario.CpfFuncionario, dadosFuncionario.RgFuncionario,
                        dadosFuncionario.PisFuncionario, dadosFuncionario.DataNascimentoFuncionario,
                        dadosFuncionario.IdSexo.ToString(), descricaoSexo, dadosFuncionario.IdEtnia.ToString(),
                        dadosFuncionario.IdEstadoCivil.ToString(), dadosFuncionario.IdFormacao.ToString(),
                        dadosFuncionario.FormacaoAcademica,
                        login, dadosFuncionario.Email, dadosFuncionario.Pa,
                        dadosFuncionario.Rua, dadosFuncionario.Numero, dadosFuncionario.Bairro, dadosFuncionario.Cidade,
                        "S", dadosFuncionario.Nacionalidade, dadosFuncionario.NomeMae,
                        dadosFuncionario.NomePai, dadosFuncionario.LocalNascimento, dadosFuncionario.UfNascimento,
                        dadosFuncionario.Complemento, dadosFuncionario.Cep, dadosFuncionario.Pais,
                        dadosFuncionario.ResidenciaPropria, dadosFuncionario.RecursoFgts, dadosFuncionario.TelefoneFixo,
                        dadosFuncionario.TelefoneCelular,
                        dadosFuncionario.EmailSecundario, dadosFuncionario.Cnh, dadosFuncionario.OrgaoEmissorCnh,
                        dadosFuncionario.DataExpedicaoDocumentoCnh
                        , dadosFuncionario.DataValidadeCnh, dadosFuncionario.Oc, dadosFuncionario.OrgaoEmissorOc,
                        dadosFuncionario.DataExpedicaoOc,
                        dadosFuncionario.DataValidadeOc, dadosFuncionario.DeficienteMotor,
                        dadosFuncionario.DeficienteVisual, dadosFuncionario.DeficienteAuditivo,
                        dadosFuncionario.Reabilitado, dadosFuncionario.ObservacaoDeficiente,
                        dadosFuncionario.IdEstadoCivilPais.ToString(), dadosFuncionario.OrgaoEmissorRg,
                        dadosFuncionario.DataExpedicaoDocumentoRg, dadosFuncionario.IdHorario );
                }

                return RedirectToAction( "Perfil", "PainelColaborador",
                    new { Mensagem = "Dados Pessoais atualizados com sucesso !" } );
            }

            return RedirectToAction( "Login", "Login" );
        }

        public ActionResult Upload()
        {
            var inserirFoto = new QueryMysql();
            var logado = inserirFoto.UsuarioLogado();
            if(logado)
            {
                var arquivo = Request.Files[0];
                if(arquivo != null)
                {
                    var nomeArquivo = Path.GetFileName( arquivo.FileName );

                    if(nomeArquivo != null)
                    {
                        var caminho = Path.Combine( Server.MapPath( "~/Uploads/" ), nomeArquivo );

                        if(System.IO.File.Exists( caminho ))
                        {
                            var counter = 1;
                            var tempfileName = "";
                            while(System.IO.File.Exists( caminho ))
                            {
                                tempfileName = counter + nomeArquivo;
                                caminho = Path.Combine( Server.MapPath( "~/Uploads/" ), tempfileName );
                                counter++;
                            }

                            caminho = Path.Combine( Server.MapPath( "~/Uploads/" ), tempfileName );
                            arquivo.SaveAs( caminho );

                            var cookie = Request.Cookies.Get( "CookieFarm" );
                            if(cookie != null)
                            {
                                var login = Criptografa.Descriptografar( cookie.Value );
                                inserirFoto.AtualizaFoto( tempfileName, login );
                            }

                            return Content( "Imagem alterada com sucesso!" );
                        }
                        else
                        {
                            arquivo.SaveAs( caminho );
                            var cookie = Request.Cookies.Get( "CookieFarm" );
                            if(cookie != null)
                            {
                                var login = Criptografa.Descriptografar( cookie.Value );
                                inserirFoto.AtualizaFoto( nomeArquivo, login );
                            }

                            return Content( "Imagem alterada com sucesso!" );
                        }
                    }
                }
            }

            return RedirectToAction( "Login", "Login" );
        }

        public ActionResult UploadArquivo(string nome)
        {
            var inserirFoto = new QueryMysql();
            var logado = inserirFoto.UsuarioLogado();
            if(logado)
            {
                var arquivo = Request.Files[0];
                var nomeArquivo = Path.GetFileName( arquivo.FileName );
                var caminho = Path.Combine( Server.MapPath( "~/Uploads/" ), nomeArquivo );
                byte[] fileData = null;
                using(var binaryReader = new BinaryReader( arquivo.InputStream ))
                {
                    fileData = binaryReader.ReadBytes( arquivo.ContentLength );
                }

                var cookie = Request.Cookies.Get( "CookieFarm" );
                var login = Criptografa.Descriptografar( cookie.Value );
                inserirFoto.AtualizarArquivoPessoal( nomeArquivo, fileData, login );
                return Content( "Imagem alterada com sucesso!" );
            }

            return RedirectToAction( "Login", "Login" );
        }

        [HttpPost]
        public ActionResult AtualizarFormularioPessoal(Funcionario dadosFuncionario)
        {
            var verificaDados = new QueryMysql();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                var cookie = Request.Cookies.Get( "CookieFarm" );
                var login = Criptografa.Descriptografar( cookie.Value );
                var dataNascimentoFilho = "";
                if(dadosFuncionario.DataNascimentoFilho == null)
                    dataNascimentoFilho = "NÂO TEM";
                else
                    dataNascimentoFilho = dadosFuncionario.DataNascimentoFilho;
                verificaDados.AtualizadadosFuncionarioPerguntas( login, dadosFuncionario.QuatidadeFilho,
                    dataNascimentoFilho, dadosFuncionario.ContatoEmergencia,
                    dadosFuncionario.PrincipaisHobbies, dadosFuncionario.ComidaFavorita, dadosFuncionario.Viagem,
                    dadosFuncionario.ContribuicaoSindical, dadosFuncionario.NotificacaoEmail );

                return RedirectToAction( "Perfil", "PainelColaborador",
                    new { Mensagem = "Formulário Pessoal atualizado com sucesso !" } );
            }

            return RedirectToAction( "Login", "Login" );
        }

        public ActionResult ColaboradorRh(string mensagem)
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                var carregaDados = new QueryMysql();
                TempData["Mensagem"] = mensagem;

                var cookie = Request.Cookies.Get( "CookieFarm" );

                if(cookie != null)
                {
                    var login = Criptografa.Descriptografar( cookie.Value );
                    if(carregaDados.PrimeiroLogin( login ))
                        return RedirectToAction( "FormularioCadastro", "Principal" );

                    var dadosUsuarioBanco = carregaDados.RecuperaDadosUsuarios( login );


                    var validacoes = new ValidacoesIniciais();

                    validacoes.AlertasUsuario( this, dadosUsuarioBanco[0]["id"] );
                    validacoes.Permissoes( this, dadosUsuarioBanco );
                    validacoes.DadosNavBar( this, dadosUsuarioBanco );
                }


                var dadosColaborador = carregaDados.RecuperadadosFuncionariosSetor();

                var queryRh = new QueryMysqlRh();
                var vagasInternas = queryRh.RetornaVagaInternaTotal();

                TempData["TotalColaborador"] = dadosColaborador.Count;
                TempData["Total"] = vagasInternas.Count;

                var validacoesPonto = new ValidacoesPonto();
                var tabelasPonto = validacoesPonto.RetornaPendenciasMysql();
                TempData["ExtraJustifica1"] = "hidden";
                TempData["ExtraJustifica2"] = "hidden";

                #region Preenchimento pendencias MYSQL

                TempData["TotalSemConfirmar"] = tabelasPonto.Count;
                for(var i = 0; i < tabelasPonto.Count; i++)
                {
                    TempData["IdPendencia" + i] = tabelasPonto[i]["IdPendencia"];


                    TempData["NomePendencia" + i] = tabelasPonto[i]["Nome"];


                    TempData["DiaPendencia" + i] =
                        Convert.ToDateTime( tabelasPonto[i]["Data"] ).ToString( "dd/MM/yyyy" );

                    TempData["TotalHorarioPendencia" + i] = Convert.ToInt32( tabelasPonto[i]["TotalHorario"] );

                    for(var j = 0; j < Convert.ToInt32( tabelasPonto[i]["TotalHorario"] ); j++)
                        TempData["Hora" + j + "Pendencia" + i] = tabelasPonto[i]["Horario" + j];

                    if(4 - Convert.ToInt32( tabelasPonto[i]["TotalHorario"] ) > 0)
                        TempData["TotalHorarioPendencia" + i] = 4;

                    if(Convert.ToBoolean( tabelasPonto[i]["Justificado"] ))
                    {
                        TempData["StatusJustificativa" + i] = "green";
                        try
                        {
                            TempData["Justificativa" + i] = tabelasPonto[i]["Justificativa" + i];
                            TempData["Esconde" + i] = "";
                        }
                        catch
                        {
                            TempData["Justificativa" + i] = "NÃO INFORMADO";
                            TempData["Esconde" + i] = "";
                        }
                    }
                    else
                    {
                        TempData["Justificativa" + i] = "NÃO JUSTIFICADO";
                        TempData["StatusJustificativa" + i] = "red";
                        TempData["Esconde" + i] = "hidden";
                    }

                    TempData["StatusJustificativaGetor" + i] = tabelasPonto[i]["ConfirmaGestor"];
                    if(Convert.ToInt32( tabelasPonto[i]["TotalHorario"] ) == 5)
                    {
                        TempData["ExtraJustifica1"] = "";
                    }
                    else if(Convert.ToInt32( tabelasPonto[i]["TotalHorario"] ) == 6)
                    {
                        TempData["ExtraJustifica1"] = "";
                        TempData["ExtraJustifica2"] = "";
                    }
                }

                #endregion


                for(var j = 0; j < vagasInternas.Count; j++)

                {
                    TempData["Titulo " + j] = vagasInternas[j]["titulo"];
                    TempData["DescricaoVaga " + j] = vagasInternas[j]["descricao"];
                    TempData["IdVaga " + j] = vagasInternas[j]["id"];
                    if(vagasInternas[j]["encerrada"].Equals( "N" ))
                        TempData["StatusVaga " + j] = "green";
                    else
                        TempData["StatusVaga " + j] = "red";
                }

                for(var i = 0; i < dadosColaborador.Count; i++)
                {
                    TempData["Nome" + i] = dadosColaborador[i]["nome"];
                    TempData["Setor" + i] = dadosColaborador[i]["setor"];
                    TempData["PA" + i] = dadosColaborador[i]["idpa"];
                    TempData["Login" + i] = dadosColaborador[i]["login"];
                    try
                    {
                        if(dadosColaborador[i]["foto"].Equals( "0" ) || dadosColaborador[i]["foto"] == null)
                            TempData["Imagem" + i] = "https://docs.google.com/uc?id=0B2CLuTO3N2_obWdkajEzTmpGeU0";
                        else
                            TempData["Imagem" + i] = "/Uploads/" + dadosColaborador[i]["foto"] + "";
                    }
                    catch
                    {
                        TempData["Imagem" + i] = "https://docs.google.com/uc?id=0B2CLuTO3N2_obWdkajEzTmpGeU0";
                    }
                }


                return View( "ColaboradorRh" );
            }

            return RedirectToAction( "Login", "Login" );
        }

        public ActionResult PerfilFuncionario(string login)
        {
            var verificaDados = new QueryMysql();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                var dadosTabelaFuncionario = verificaDados.RecuperadadosFuncionariosTabelaFuncionariosPerfil( login );


                var documentosUpados = verificaDados.RetornaDocumentosFuncionario( login );

                for(var i = 0; i < documentosUpados.Rows.Count; i++)
                {
                    TempData["Status" + documentosUpados.Rows[i]["nomearquivo"]] = "is-success";
                    TempData["Nome" + documentosUpados.Rows[i]["nomearquivo"]] = "Arquivo Enviado";

                    var bytes = (byte[])documentosUpados.Rows[i]["arquivo"];
                    var img64 = Convert.ToBase64String( bytes );
                    ValidaImagem image = new ValidaImagem();
                    if(!image.IsValidImage( bytes ))
                    {
                        var img64Url = string.Format( "data:application/pdf;base64,{0}", img64 );
                        TempData["Imagem" + documentosUpados.Rows[i]["nomearquivo"]] = img64Url;
                        TempData["ValidaTipo" + documentosUpados.Rows[i]["nomearquivo"]] = "pdf";
                    }
                    else
                    {
                        var img64Url = string.Format( "data:image/;base64,{0}", img64 );
                        TempData["Imagem" + documentosUpados.Rows[i]["nomearquivo"]] = img64Url;
                        TempData["ValidaTipo" + documentosUpados.Rows[i]["nomearquivo"]] = "imagem";
                    }
                }

                var vinculoExtra = verificaDados.RecuperaVinculoExtra( dadosTabelaFuncionario[0]["id"] );

                var tipos = dadosTabelaFuncionario[0]["tipodependente"].ToString().Split( ';' );
                for(var i = 0; i < 10; i++)
                    if(tipos.Contains( i.ToString() ))
                        TempData["Check" + i] = "checked";
                    else
                        TempData["Check" + i] = "";


                var dadosFuncionario = new Funcionario();
                if(vinculoExtra.Count > 0)
                {
                    TempData["MostraVinculo"] = "";
                    dadosFuncionario.MultiploCnpj = vinculoExtra[0]["cnpj"];
                    dadosFuncionario.MultiploNomeEmpresa = vinculoExtra[0]["nomeempresa"];
                    dadosFuncionario.MultiploRemuneracao = vinculoExtra[0]["remuneracao"];
                    dadosFuncionario.MultiploComentario = vinculoExtra[0]["comentario"];
                }
                else
                {
                    TempData["MostraVinculo"] = "style=display:none;";
                }

                dadosFuncionario.Salario = Convert.ToDouble( dadosTabelaFuncionario[0]["salariobase"] ).ToString();
                dadosFuncionario.Anuenio = Convert.ToDouble( dadosTabelaFuncionario[0]["anuenio"] ).ToString();
                dadosFuncionario.QuebraDeCaixa = Convert.ToDouble( dadosTabelaFuncionario[0]["quebradecaixa"] ).ToString();
                dadosFuncionario.Ticket = Convert.ToDouble( dadosTabelaFuncionario[0]["ticket"] ).ToString();
                dadosFuncionario.Estagiario = dadosTabelaFuncionario[0]["estagiario"];
                dadosFuncionario.DataEstagiario = Convert.ToDateTime( dadosTabelaFuncionario[0]["contratoestagio"] ).Date;
                dadosFuncionario.DataAdmissaoFuncionario = Convert.ToDateTime( dadosTabelaFuncionario[0]["admissao"] ).Date;
                dadosFuncionario.VencimentoPeriodico = dadosTabelaFuncionario[0]["vencimentoperiodico"];


                dadosFuncionario.NomeFuncionario = dadosTabelaFuncionario[0]["nome"];
                dadosFuncionario.CpfFuncionario = dadosTabelaFuncionario[0]["cpf"];
                dadosFuncionario.RgFuncionario = dadosTabelaFuncionario[0]["rg"];
                dadosFuncionario.PisFuncionario = dadosTabelaFuncionario[0]["pis"];
                dadosFuncionario.DataNascimentoFuncionario =
                    Convert.ToDateTime( dadosTabelaFuncionario[0]["datanascimento"] ).ToString( "dd/MM/yyyy" );
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
                dadosFuncionario.DescricaoSexo = dadosTabelaFuncionario[0]["descricaosexo"];
                dadosFuncionario.Matricula = dadosTabelaFuncionario[0]["matricula"];
                dadosFuncionario.NumeroCtps = dadosTabelaFuncionario[0]["numeroctps"];
                dadosFuncionario.SerieCtps = dadosTabelaFuncionario[0]["seriectps"];
                dadosFuncionario.UfCtps = dadosTabelaFuncionario[0]["ufctps"];
                dadosFuncionario.EmissaoCtps = Convert.ToDateTime( dadosTabelaFuncionario[0]["dataemissaoctps"] ).Date
                    .ToString( "dd/MM/yyyy" );
                dadosFuncionario.AnoPrimeiroEmprego = dadosTabelaFuncionario[0]["anoprimeiroemprego"];
                dadosFuncionario.IdTipoConta = Convert.ToInt32( dadosTabelaFuncionario[0]["idtipoconta"] );
                dadosFuncionario.CodigoBanco = dadosTabelaFuncionario[0]["codigobanco"];
                dadosFuncionario.Agencia = dadosTabelaFuncionario[0]["agencia"];
                dadosFuncionario.ContaCorrente = dadosTabelaFuncionario[0]["contacorrente"];
                dadosFuncionario.DadosDependentes = dadosTabelaFuncionario[0]["informacaodependente"];
                dadosFuncionario.DependenteIrrf = dadosTabelaFuncionario[0]["dependenteirrpf"];
                dadosFuncionario.CpfIrrf = dadosTabelaFuncionario[0]["cpfirrf"];
                dadosFuncionario.DependenteFamilia = dadosTabelaFuncionario[0]["dependentesalariofamilia"];
                dadosFuncionario.OrgaoEmissorRg = dadosTabelaFuncionario[0]["orgaoemissorrg"];
                dadosFuncionario.Nacionalidade = dadosTabelaFuncionario[0]["nacionalidade"];
                dadosFuncionario.LocalNascimento = dadosTabelaFuncionario[0]["localnascimento"];
                dadosFuncionario.UfNascimento = dadosTabelaFuncionario[0]["ufnascimento"];
                dadosFuncionario.TelefoneFixo = dadosTabelaFuncionario[0]["telefonefixo"];
                dadosFuncionario.TelefoneCelular = dadosTabelaFuncionario[0]["telefonecelular"];
                dadosFuncionario.NomeMae = dadosTabelaFuncionario[0]["nomemae"];
                dadosFuncionario.NomePai = dadosTabelaFuncionario[0]["nomepai"];

                dadosFuncionario.EmailSecundario = dadosTabelaFuncionario[0]["emailsecundario"];
                dadosFuncionario.Cep = dadosTabelaFuncionario[0]["cep"];
                dadosFuncionario.Complemento = dadosTabelaFuncionario[0]["complemento"];
                dadosFuncionario.Pais = dadosTabelaFuncionario[0]["pais"];
                dadosFuncionario.ResidenciaPropria = dadosTabelaFuncionario[0]["residenciapropria"];
                dadosFuncionario.RecursoFgts = dadosTabelaFuncionario[0]["recursofgts"];
                dadosFuncionario.DeficienteMotor = dadosTabelaFuncionario[0]["deficientemotor"];
                dadosFuncionario.DeficienteVisual = dadosTabelaFuncionario[0]["deficientevisual"];
                dadosFuncionario.DeficienteAuditivo = dadosTabelaFuncionario[0]["deficienteauditivo"];
                dadosFuncionario.Reabilitado = dadosTabelaFuncionario[0]["reabilitado"];
                dadosFuncionario.ObservacaoDeficiente = dadosTabelaFuncionario[0]["observacaodeficiente"];
                dadosFuncionario.DataExpedicaoDocumentoRg =
                    Convert.ToDateTime( dadosTabelaFuncionario[0]["dataemissaorg"] ).Date;
                dadosFuncionario.IdEstadoCivil = Convert.ToInt32( dadosTabelaFuncionario[0]["idestadocivil"] );
                try
                {
                    dadosFuncionario.IdEstadoCivilPais =
                        Convert.ToInt32( dadosTabelaFuncionario[0]["paisdivorciado"] );
                }
                catch
                {
                    dadosFuncionario.IdEstadoCivilPais = 0;
                }

                try
                {
                    dadosFuncionario.IdHorario = Convert.ToInt32( dadosTabelaFuncionario[0]["idhorariotrabalho"] );
                }
                catch
                {
                    dadosFuncionario.IdHorario = 0;
                }

                dadosFuncionario.IdSexo = Convert.ToInt32( dadosTabelaFuncionario[0]["sexo"] );
                dadosFuncionario.IdEtnia = Convert.ToInt32( dadosTabelaFuncionario[0]["etnia"] );
                dadosFuncionario.IdFormacao = Convert.ToInt32( dadosTabelaFuncionario[0]["idescolaridade"] );
                dadosFuncionario.IdSetor = Convert.ToInt32( dadosTabelaFuncionario[0]["idsetor"] );
                dadosFuncionario.IdFuncao = Convert.ToInt32( dadosTabelaFuncionario[0]["funcao"] );
                dadosFuncionario.NotificacaoEmail = dadosTabelaFuncionario[0]["notificacaoemail"];
                dadosFuncionario.ContribuicaoSindical = dadosTabelaFuncionario[0]["contribuicaosindical"];

                var formacoes = verificaDados.RetornaFormacaoFuncionario( dadosTabelaFuncionario[0]["id"] );

                if(dadosTabelaFuncionario[0]["cnh"].Equals( "" ))
                {
                    if(dadosTabelaFuncionario[0]["oc"].Equals( "" ))
                    {
                        TempData["ExibeDocumentoExtra"] = "style=display:none;";
                    }
                    else
                    {
                        TempData["ExibeDocumentoExtra"] = "";
                        dadosFuncionario.Cnh = dadosTabelaFuncionario[0]["cnh"];
                        dadosFuncionario.DataExpedicaoDocumentoCnh =
                            Convert.ToDateTime( dadosTabelaFuncionario[0]["dataexpedicaocnh"] ).Date;
                        dadosFuncionario.OrgaoEmissorCnh = dadosTabelaFuncionario[0]["orgaoemissorcnh"];
                        dadosFuncionario.DataValidadeCnh =
                            Convert.ToDateTime( dadosTabelaFuncionario[0]["datavalidadecnh"] );

                        dadosFuncionario.Oc = dadosTabelaFuncionario[0]["oc"];
                        dadosFuncionario.DataExpedicaoOc =
                            Convert.ToDateTime( dadosTabelaFuncionario[0]["dataexpedicaooc"] ).Date;
                        dadosFuncionario.OrgaoEmissorOc = dadosTabelaFuncionario[0]["orgaoemissoroc"];
                        dadosFuncionario.DataValidadeOc =
                            Convert.ToDateTime( dadosTabelaFuncionario[0]["datavalidadeoc"] );
                    }
                }
                else
                {
                    TempData["ExibeDocumentoExtra"] = "";
                    dadosFuncionario.Cnh = dadosTabelaFuncionario[0]["cnh"];
                    dadosFuncionario.DataExpedicaoDocumentoCnh =
                        Convert.ToDateTime( dadosTabelaFuncionario[0]["dataexpedicaocnh"] ).Date;
                    dadosFuncionario.OrgaoEmissorCnh = dadosTabelaFuncionario[0]["orgaoemissorcnh"];
                    dadosFuncionario.DataValidadeCnh =
                        Convert.ToDateTime( dadosTabelaFuncionario[0]["datavalidadecnh"] );

                    dadosFuncionario.Oc = dadosTabelaFuncionario[0]["oc"];
                    dadosFuncionario.DataExpedicaoOc =
                        Convert.ToDateTime( dadosTabelaFuncionario[0]["dataexpedicaooc"] ).Date;
                    dadosFuncionario.OrgaoEmissorOc = dadosTabelaFuncionario[0]["orgaoemissoroc"];
                    dadosFuncionario.DataValidadeOc =
                        Convert.ToDateTime( dadosTabelaFuncionario[0]["datavalidadeoc"] );
                }

                TempData["TotalFormacao"] = formacoes.Count;
                for(var j = 0; j < formacoes.Count; j++)
                {
                    TempData["IdFormacaoExtra" + j] = "Extra|" + formacoes[j]["id"];
                    TempData["IdTipoFormacao" + j] = "Tipo|" + formacoes[j]["id"];
                    TempData["FormacaoExtra" + j] = formacoes[j]["descricao"];
                    TempData["TipoFormacaoExtra" + j] = formacoes[j]["tipoformacao"];
                }


                var certificacoesFuncao =
                    verificaDados.RetornaCertificacaoFuncao( dadosTabelaFuncionario[0]["funcao"] );
                var idCertificacoes = certificacoesFuncao[0]["idcertificacao"].Split( ';' );

                TempData["TotalCertificacao"] = idCertificacoes.Length;

                for(var j = 0; j < idCertificacoes.Length; j++)
                    if(idCertificacoes[j].Length > 0)
                    {
                        var certificacoes = verificaDados.RetornaCertificacao( idCertificacoes[j] );
                        TempData["Certificacao" + j] = certificacoes[0]["descricao"];
                    }


                if(dadosTabelaFuncionario[0]["confirmacaocertificacao"].Equals( "S" ))
                    TempData["ConfirmaCertificacao"] = "Checked";
                else
                    TempData["ConfirmaCertificacao"] = "";
                if(dadosTabelaFuncionario[0]["foto"] == null)
                    TempData["Foto"] = "http://bulma.io/images/placeholders/128x128.png";
                else
                    TempData["Foto"] = "/Uploads/" + dadosTabelaFuncionario[0]["foto"];
                var funcaoFuncionario = verificaDados.RetornaFuncaoFuncionario( dadosTabelaFuncionario[0]["funcao"] );
                TempData["NomeFuncionario"] = dadosTabelaFuncionario[0]["nome"];
                TempData["Funcao"] = funcaoFuncionario;
                TempData["DataAdmissao"] =
                    Convert.ToDateTime( dadosTabelaFuncionario[0]["admissao"] ).ToString( "dd/MM/yyyy" );

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

                if(dadosTabelaFuncionario[0]["estagiario"].Equals( "S" ))
                {
                    TempData["Estagiario"] = "SIM";
                    TempData["DataEstagio"] = dadosTabelaFuncionario[0]["contratoestagio"];
                }
                else
                {
                    TempData["Estagiario"] = "NÃO";
                    TempData["DataEstagio"] = "-";
                }

                return PartialView( "ModalPerfil", dadosFuncionario );
            }

            return RedirectToAction( "Login", "Login" );
        }


        [HttpPost]
        public ActionResult CadastrarVaga(VagasInternas dadosVaga)
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
                if(ModelState.IsValid)
                {
                    verificaDados.CadastraVagaInterna( dadosVaga.Titulo, dadosVaga.Descricao, dadosVaga.Descricao );

                    return RedirectToAction( "ColaboradorRH", "PainelColaborador",
                        new { Mensagem = "Vaga interna criada com sucesso !" } );
                }
                else
                {
                    return RedirectToAction( "ColaboradorRh", "PainelColaborador",
                        new { Mensagem = "Vaga interna não cadastrada !" } );
                }

            return RedirectToAction( "Login", "Login" );
        }

        public ActionResult Vaga()
        {
            return PartialView( "ModalVagaInterna" );
        }

        public ActionResult AlterarVaga(string idVaga)
        {
            var verificaDados = new QueryMysqlCurriculo();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                var queryRh = new QueryMysqlRh();
                var dadosVaga = queryRh.RetornaVaga( idVaga );
                var vaga = new VagasInternas();
                vaga.Titulo = dadosVaga[0]["titulo"];
                vaga.Descricao = dadosVaga[0]["descricao"];
                vaga.Requisitos = dadosVaga[0]["requisito"];
                TempData["IdVaga"] = idVaga;

                if(dadosVaga[0]["encerrada"].Equals( "N" ))
                {
                    TempData["Ativa"] = "checked";
                    TempData["Status"] = "Ativa";
                }
                else
                {
                    TempData["Ativa"] = "";
                    TempData["Status"] = "Desativada";
                }

                return PartialView( "ModalEditarVagaInterna", vaga );
            }

            return RedirectToAction( "Login", "Login" );
        }

        [HttpPost]
        public ActionResult AtualizarVaga(VagasInternas dadosVaga, FormCollection formulario)
        {
            var verificaDados = new QueryMysqlCurriculo();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
                if(ModelState.IsValid)
                {
                    var queryRh = new QueryMysqlRh();

                    queryRh.AtualizaVagaInterna( dadosVaga.Titulo, dadosVaga.Descricao, dadosVaga.Requisitos,
                        formulario["IdVaga"] );
                    return RedirectToAction( "ColaboradorRh", "PainelColaborador",
                        new { Mensagem = "Vaga alterada com sucesso!" } );
                }

            return RedirectToAction( "Login", "Login" );
        }

        public ActionResult GerenciarVaga(string idVaga, string mensagem)
        {
            var verificaDados = new QueryMysqlCurriculo();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                TempData["Mensagem"] = mensagem;

                var recuperaDados = new QueryMysqlRh();
                var dadosCurriculos = recuperaDados.RecuperaFuncionariosVaga( idVaga );
                var dadosVaga = recuperaDados.RetornaVaga( idVaga );
                TempData["TotalCurriculo"] = dadosCurriculos.Count;
                TempData["QuantidadeCurriculo"] = "N° de candidatos com interesse nesta vaga: " + dadosCurriculos.Count;
                TempData["TituloVaga"] = idVaga + "-" + dadosVaga[0]["titulo"];
                TempData["DescricaoVaga"] = dadosVaga[0]["descricao"];
                TempData["IdVaga"] = idVaga;

                for(var i = 0; i < dadosCurriculos.Count; i++)
                {
                    TempData["Id " + i] = dadosCurriculos[i]["id"];
                    TempData["IdFuncionario" + i] = "IdFuncionario | " + dadosCurriculos[i]["id"];
                    TempData["Nome" + i] = dadosCurriculos[i]["nome"];
                    TempData["Login" + i] = dadosCurriculos[i]["login"];
                    TempData["Setor" + i] = dadosCurriculos[i]["setorfuncionario"];
                    TempData["PA" + i] = dadosCurriculos[i]["idpa"];
                    TempData["ResultadoObservacao" + i] = dadosCurriculos[0]["observacao"];
                    if(Convert.ToBoolean( dadosCurriculos[i]["aprovado"] ))
                        TempData["Resultado" + i] = "checked";
                    else
                        TempData["Resultado" + i] = "";

                    if(dadosCurriculos[i]["foto"].Equals( "0" ))
                        TempData["Imagem" + i] = "https://docs.google.com/uc?id=0B2CLuTO3N2_obWdkajEzTmpGeU0";
                    else
                        TempData["Imagem" + i] = "/Uploads/" +
                                                 dadosCurriculos[i]["foto"] + "";
                }

                if(dadosVaga[0]["encerrada"].Equals( "N" ))
                {
                    TempData["Ativa"] = "";
                    TempData["Dica"] = "Clique para encerrar esta vaga.";
                }
                else
                {
                    TempData["Ativa"] = "disabled";
                    TempData["Dica"] = "Esta vaga já esta encerrada.";
                }

                return View( "GerenciarVaga" );
            }

            return RedirectToAction( "Login", "Login" );
        }

        [HttpPost]
        public ActionResult EncerrarVaga(FormCollection formulario)
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                for(var i = 0; i < formulario.Count; i++)
                    if(formulario.AllKeys[i].Contains( "IdFuncionario" ))
                    {
                        var idFuncionario = formulario[i];
                        bool aprovado;
                        try
                        {
                            if(formulario["Aprovado " + idFuncionario].Equals( "on" ))
                                aprovado = true;
                            else
                                aprovado = false;
                        }
                        catch
                        {
                            aprovado = false;
                        }

                        var observacao = formulario["observacao " + idFuncionario];
                        verificaDados.AtualizaStatus( formulario["vaga"], idFuncionario, aprovado, observacao );
                    }

                verificaDados.EncerraVaga( formulario["vaga"] );
                return RedirectToAction( "GerenciarVaga", "PainelColaborador",
                    new { IdVaga = formulario["vaga"], Mensagem = "Vaga interna encerrada com sucesso !" } );
            }

            return RedirectToAction( "Login", "Login" );
        }

        [HttpPost]
        public async Task<JsonResult> ConfirmarPendencia(Item[] tabelaPendencias)
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                for(var i = 0; i < tabelaPendencias.Length; i++)
                {
                    var recuperaId = new QueryMysql();
                    var dadosFuncionario =
                        recuperaId.RecuperadadosFuncionariosTabelaFuncionarios(
                            tabelaPendencias[i].Nome.Replace( "'", "" ) );
                    try
                    {
                        var idJustificativa = verificaDados.InserirHistoricoJustificativa( dadosFuncionario[0]["id"],
                            Convert.ToDateTime( tabelaPendencias[i].Dia ) );
                        if(tabelaPendencias[i].Horario1.Length == 0 || tabelaPendencias[i].Horario1.Contains( "--" ))
                        {
                        }
                        else
                        {
                            verificaDados.InserirHistoricoHorario( idJustificativa,
                                TimeSpan.Parse( tabelaPendencias[i].Horario1 ), tabelaPendencias[i].Id, "0" );
                        }

                        if(tabelaPendencias[i].Horario2.Length == 0 || tabelaPendencias[i].Horario2.Contains( "--" ))
                        {
                        }
                        else
                        {
                            verificaDados.InserirHistoricoHorario( idJustificativa,
                                TimeSpan.Parse( tabelaPendencias[i].Horario2 ), tabelaPendencias[i].Id, "0" );
                        }

                        if(tabelaPendencias[i].Horario3.Length == 0 || tabelaPendencias[i].Horario3.Contains( "--" ))
                        {
                        }
                        else
                        {
                            verificaDados.InserirHistoricoHorario( idJustificativa,
                                TimeSpan.Parse( tabelaPendencias[i].Horario3 ), tabelaPendencias[i].Id, "0" );
                        }

                        if(tabelaPendencias[i].Horario4.Length == 0 || tabelaPendencias[i].Horario4.Contains( "--" ))
                        {
                        }
                        else
                        {
                            verificaDados.InserirHistoricoHorario( idJustificativa,
                                TimeSpan.Parse( tabelaPendencias[i].Horario4 ), tabelaPendencias[i].Id, "0" );
                        }

                        if(tabelaPendencias[i].Horario5.Length == 0 || tabelaPendencias[i].Horario5.Contains( "--" ))
                        {
                        }
                        else
                        {
                            verificaDados.InserirHistoricoHorario( idJustificativa,
                                TimeSpan.Parse( tabelaPendencias[i].Horario5 ), tabelaPendencias[i].Id, "0" );
                        }

                        if(tabelaPendencias[i].Horario6.Length == 0 || tabelaPendencias[i].Horario4.Contains( "--" ) ||
                            tabelaPendencias[i].Horario6.Contains( "<" ))
                        {
                        }
                        else
                        {
                            verificaDados.InserirHistoricoHorario( idJustificativa,
                                TimeSpan.Parse( tabelaPendencias[i].Horario6 ), tabelaPendencias[i].Id, "0" );
                        }
                    }
                    catch
                    {
                        // ignored
                    }

                    try
                    {
                        var envia = new EnviodeAlertas();
                        var cadastroAlerta = new QueryMysql();

                        var dadosOperador =
                            cadastroAlerta.RetornaInformacoesNotificacao( dadosFuncionario[0]["id"] );
                        if(dadosOperador != null)
                        {
                            await envia.EnviaAlertaFuncionario( dadosOperador[0], "Você tem justificativas pendentes.",
                                "11" );
                            if(dadosOperador[0]["idsetor"] != null &&
                                dadosFuncionario[0]["idsetor"].ToString().Length > 0)
                            {
                                var dadosGestor = cadastroAlerta.RetornaInformacoesGestor( dadosOperador[0]["idsetor"] );
                                if(dadosGestor != null && dadosGestor.Count > 0)
                                    await envia.EnviaAlertaFuncionario( dadosGestor[0],
                                        "O funcionário " + dadosOperador[0]["nome"] + " tem justificativas pendentes",
                                        "11" );
                            }
                        }
                    }
                    catch { }
                }

                return Json( "Ok" );
            }

            return Json( "Ok" );
        }

        public ActionResult NegarJustificativa(string idHistorico)
        {
            var queryRh = new QueryMysqlRh();
            queryRh.NegaJustificativa( idHistorico );
            queryRh.NegaJustificativaGestor( idHistorico );
            return RedirectToAction( "ColaboradorRh", "PainelColaborador",
                new { Mensagem = "Pendencia negada com sucesso !" } );
        }

        [HttpPost]
        public async Task<ActionResult> CadastraAlerta(FormCollection dados)
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                try
                {
                    var funcionarioPendentes = verificaDados.RetornaPendenciaAlerta();
                    for(var i = 0; i < funcionarioPendentes.Count; i++)
                    {
                        verificaDados.CadastraAlertaJustificativa( funcionarioPendentes[i]["idfuncionario"],
                            dados["TextAlerta"] );

                        var envia = new EnviodeAlertas();
                        var cadastroAlerta = new QueryMysql();

                        var dadosOperador =
                            cadastroAlerta.RetornaInformacoesNotificacao( funcionarioPendentes[i]["idfuncionario"] );
                        if(dadosOperador != null)
                        {
                            await envia.EnviaAlertaFuncionario( dadosOperador[0], dados["TextAlerta"],
                                "11" );

                            var dadosGestor = cadastroAlerta.RetornaInformacoesGestor( dadosOperador[0]["idsetor"] );
                            if(dadosGestor.Count > 0)
                                await envia.EnviaAlertaFuncionario( dadosGestor[0], dados["TextAlerta"],
                                    "11" );
                        }
                    }
                }
                catch(Exception e)
                {

                }

                return RedirectToAction( "ColaboradorRh", "PainelColaborador",
                    new { Mensagem = "Alerta cadastrado com sucesso !" } );
            }

            return RedirectToAction( "Login", "Login" );
        }

        public ActionResult ApontarPendencia(Item[] tabelaPendencias)
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                for(var i = 0; i < tabelaPendencias.Length; i++)
                {
                    var recuperaId = new QueryMysql();
                    var dadosFuncionario =
                        recuperaId.RecuperadadosFuncionariosTabelaFuncionarios(
                            tabelaPendencias[i].Nome.Replace( "'", "" ) );
                    try
                    {
                        var idJustificativa = verificaDados.InserirHistoricoJustificativa( dadosFuncionario[0]["id"],
                            Convert.ToDateTime( tabelaPendencias[i].Dia ) );
                        if(tabelaPendencias[i].Horario1.Length == 0 || tabelaPendencias[i].Horario1.Contains( "--" ))
                        {
                        }
                        else
                        {
                            verificaDados.InserirHistoricoHorario( idJustificativa,
                                TimeSpan.Parse( tabelaPendencias[i].Horario1 ), tabelaPendencias[i].Id, "0" );
                        }

                        if(tabelaPendencias[i].Horario2.Length == 0 || tabelaPendencias[i].Horario2.Contains( "--" ))
                        {
                        }
                        else
                        {
                            verificaDados.InserirHistoricoHorario( idJustificativa,
                                TimeSpan.Parse( tabelaPendencias[i].Horario2 ), tabelaPendencias[i].Id, "0" );
                        }

                        if(tabelaPendencias[i].Horario3.Length == 0 || tabelaPendencias[i].Horario3.Contains( "--" ))
                        {
                        }
                        else
                        {
                            verificaDados.InserirHistoricoHorario( idJustificativa,
                                TimeSpan.Parse( tabelaPendencias[i].Horario3 ), tabelaPendencias[i].Id, "0" );
                        }

                        if(tabelaPendencias[i].Horario4.Length == 0 || tabelaPendencias[i].Horario4.Contains( "--" ))
                        {
                        }
                        else
                        {
                            verificaDados.InserirHistoricoHorario( idJustificativa,
                                TimeSpan.Parse( tabelaPendencias[i].Horario4 ), tabelaPendencias[i].Id, "0" );
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                }

                return Json( "Ok" );
            }

            return Json( "Ok" );
        }

        public ActionResult HistoricoPendencia()
        {
            return View();
        }

        [HttpPost]
        public ActionResult HistoricoPendencia(FormCollection datas)
        {
            var validacoes = new ValidacoesPonto();


            var dadosPendencias = validacoes.RetornaHistoricoPendencias( datas["DataInicial"], datas["DataFinal"] );

            TempData["ExtraJustifica1"] = "hidden";
            TempData["ExtraJustifica2"] = "hidden";
            TempData["TotalSemConfirmar"] = dadosPendencias.Count;
            for(var i = 0; i < dadosPendencias.Count; i++)
            {
                TempData["IdPendencia" + i] = dadosPendencias[i]["IdPendencia"];


                TempData["NomePendencia" + i] = dadosPendencias[i]["Nome"];


                TempData["DiaPendencia" + i] =
                    Convert.ToDateTime( dadosPendencias[i]["Data"] ).ToString( "dd/MM/yyyy" );

                TempData["TotalHorarioPendencia" + i] = Convert.ToInt32( dadosPendencias[i]["TotalHorario"] );

                for(var j = 0; j < Convert.ToInt32( dadosPendencias[i]["TotalHorario"] ); j++)
                    TempData["Hora" + j + "Pendencia" + i] = dadosPendencias[i]["Horario" + j];

                if(4 - Convert.ToInt32( dadosPendencias[i]["TotalHorario"] ) > 0)
                    TempData["TotalHorarioPendencia" + i] = 4;

                if(Convert.ToBoolean( dadosPendencias[i]["Justificado"] ))
                    TempData["Justificativa" + i] = dadosPendencias[i]["Justificativa" + i];
                else
                    TempData["Justificativa" + i] = "NÃO JUSTIFICADO";

                if(Convert.ToInt32( dadosPendencias[i]["TotalHorario"] ) == 5)
                {
                    TempData["ExtraJustifica1"] = "";
                }
                else if(Convert.ToInt32( dadosPendencias[i]["TotalHorario"] ) == 6)
                {
                    TempData["ExtraJustifica1"] = "";
                    TempData["ExtraJustifica2"] = "";
                }
            }

            return View();
        }

        public ActionResult ReincidentePendencia()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReincidentePendencia(FormCollection datas)
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                var reincidentes = verificaDados.RetornaReincidentes( datas["DataInicial"], datas["DataFinal"] );
                var totalReincidente = 0;
                for(var i = 0; i < reincidentes.Count; i++)
                    if(reincidentes[i]["confirma"].Equals( "S" ))
                    {
                        var dias = verificaDados.RetornaDataReincidentes( reincidentes[i]["id"], datas["DataInicial"],
                            datas["DataFinal"] );
                        TempData["Nome" + totalReincidente] = reincidentes[i]["nome"];
                        TempData["IdFuncionario" + totalReincidente] = reincidentes[i]["id"];
                        TempData["Dia" + totalReincidente] = dias;
                        totalReincidente++;
                    }

                TempData["TotalReincidente"] = totalReincidente;
                return View();
            }

            return RedirectToAction( "Login", "Login" );
        }

        public ActionResult Paremetros()
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if(logado) return PartialView( "Parametros" );

            return RedirectToAction( "Login", "Login" );
        }

        public ActionResult Funcao()
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                var certificacoes = verificaDados.RetornaCertificacoes();
                TempData["TotalCertificacao"] = certificacoes.Count;
                for(var i = 0; i < certificacoes.Count; i++)
                {
                    TempData["DescricaoCertificacao" + i] = certificacoes[i]["descricao"];
                    TempData["IdCertificacao" + i] = certificacoes[i]["id"];
                }

                return PartialView( "Funcao" );
            }

            return RedirectToAction( "Login", "Login" );
        }

        [HttpPost]
        public ActionResult Funcao(Funcao dadosCadastro, FormCollection dados)
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                var certificacoes = "";
                for(var i = 0; i < dados.Count; i++)
                    if(dados.AllKeys[i].Contains( "Certificacao" ))
                        certificacoes = certificacoes + dados[i] + ";";

                verificaDados.CadastrarFuncao( dadosCadastro.NomeFuncao, certificacoes );
                return RedirectToAction( "ColaboradorRh", "PainelColaborador",
                    new { Mensagem = "Função cadastrada com sucesso !" } );
            }

            return RedirectToAction( "Login", "Login" );
        }

        [HttpPost]
        public ActionResult EditarFuncao(Funcao dadosCadastro, FormCollection dados)
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                var certificacoes = "";
                for(var i = 0; i < dados.Count; i++)
                    if(dados.AllKeys[i].Contains( "Certificacao" ))
                        certificacoes = certificacoes + dados[i] + ";";

                verificaDados.EditarFuncao( dados["IdFuncao"], dadosCadastro.NomeFuncao, certificacoes );
                return RedirectToAction( "ColaboradorRh", "PainelColaborador",
                    new { Mensagem = "Função alterada com sucesso !" } );
            }

            return RedirectToAction( "Login", "Login" );
        }

        [HttpPost]
        public ActionResult BuscarFuncao(string descricaoFuncao)
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                var funcoes = verificaDados.BuscaFuncao( descricaoFuncao );
                for(var i = 0; i < funcoes.Count; i++)
                {
                    TempData["Id" + i] = funcoes[i]["id"];
                    TempData["Descricao" + i] = funcoes[i]["descricao"];
                }

                TempData["TotalResultado"] = funcoes.Count;
                TempData["Editar"] = "EditarFuncao";
                return PartialView( "ResultadoPesquisa" );
            }

            return RedirectToAction( "Login", "Login" );
        }

        [HttpPost]
        public ActionResult RecuperaFuncao(string idFuncao)
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                var certificacoes = verificaDados.RetornaCertificacoes();
                var dadosFuncao = verificaDados.RecuperaDadosFuncao( idFuncao );
                var funcaoRecupera = new Funcao();


                var certificacoesFuncao = dadosFuncao[0]["idcertificacao"].Split( ';' );
                TempData["TotalCertificacaoEditar"] = certificacoes.Count;
                var existe = false;
                for(var i = 0; i < certificacoes.Count; i++)
                {
                    for(var j = 0; j < certificacoesFuncao.Length; j++)
                        if(certificacoes[i]["id"].Equals( certificacoesFuncao[j] ))
                            existe = true;

                    if(existe)
                    {
                        TempData["DescricaoCertificacaoEditar" + i] = certificacoes[i]["descricao"];
                        TempData["IdCertificacaoEditar" + i] = certificacoes[i]["id"];
                        TempData["CertificaFuncao" + i] = "checked";
                    }
                    else
                    {
                        TempData["DescricaoCertificacaoEditar" + i] = certificacoes[i]["descricao"];
                        TempData["IdCertificacaoEditar" + i] = certificacoes[i]["id"];
                        TempData["CertificaFuncao" + i] = "";
                    }

                    existe = false;
                }

                funcaoRecupera.NomeFuncao = dadosFuncao[0]["descricao"];
                TempData["IdFuncao"] = dadosFuncao[0]["id"];
                return PartialView( "EditarFuncao", funcaoRecupera );
            }

            return RedirectToAction( "Login", "Login" );
        }

        public ActionResult BuscaPendencia(string dataConsulta)
        {
            var validacoes = new ValidacoesPonto();
            var tabelasPonto = validacoes.ValidarPonto( Convert.ToDateTime( dataConsulta ) );

            #region Preenchimento Pendencias Ponto

            TempData["TotalPonto"] = tabelasPonto[0].Count;
            TempData["ExtraPendente1"] = "hidden";
            TempData["ExtraPendente2"] = "hidden";


            for(var i = 0; i < tabelasPonto[0].Count; i++)
            {
                TempData["TotalHorarioPonto" + i] = Convert.ToInt32( tabelasPonto[0][i]["TotalHorario"] );

                TempData["IdPonto" + i] = tabelasPonto[0][i]["IdFuncionario"];

                TempData["NomePonto" + i] = tabelasPonto[0][i]["NomeFuncionario"];

                TempData["Dia"] = Convert.ToDateTime( tabelasPonto[0][i]["DataPendencia"] ).ToString( "dd/MM/yyyy" );
                TempData["TotalHorarioPonto" + i] = Convert.ToInt32( tabelasPonto[0][i]["TotalHorario"] );

                for(var j = 0; j < Convert.ToInt32( tabelasPonto[0][i]["TotalHorario"] ); j++)
                    TempData["Hora" + j + i] = tabelasPonto[0][i]["Hora" + j];

                if(4 - Convert.ToInt32( tabelasPonto[0][i]["TotalHorario"] ) > 0) TempData["TotalHorarioPonto" + i] = 4;

                if(Convert.ToInt32( tabelasPonto[0][i]["TotalHorario"] ) == 5)
                {
                    TempData["ExtraPendente1"] = "";
                }
                else if(Convert.ToInt32( tabelasPonto[0][i]["TotalHorario"] ) == 6)
                {
                    TempData["ExtraPendente1"] = "";
                    TempData["ExtraPendente2"] = "";
                }
            }

            #endregion


            return PartialView( "ValidacaoPonto" );
        }

        public ActionResult BuscaSemPendencia(string dataConsulta)
        {
            var validacoes = new ValidacoesPonto();
            var tabelasPonto = validacoes.ValidarPonto( Convert.ToDateTime( dataConsulta ) );


            #region Preenchimento Sem pendencia

            TempData["TotalSemPendencia"] = tabelasPonto[1].Count;
            for(var i = 0; i < tabelasPonto[1].Count; i++)
            {
                TempData["IdPontoCerto" + i] = tabelasPonto[1][i]["IdFuncionario"];

                TempData["NomePontoCerto" + i] = tabelasPonto[1][i]["NomeFuncionario"];

                TempData["DiaCerto" + i] =
                    Convert.ToDateTime( tabelasPonto[1][i]["DataPendencia"] ).ToString( "dd/MM/yyyy" );
                try
                {
                    TempData["HoraCerto0" + i] = tabelasPonto[1][i]["Hora0"];
                    TempData["HoraCerto1" + i] = tabelasPonto[1][i]["Hora1"];
                    TempData["HoraCerto2" + i] = tabelasPonto[1][i]["Hora2"];
                    TempData["HoraCerto3" + i] = tabelasPonto[1][i]["Hora3"];
                }
                catch
                {
                    TempData["HoraCerto0" + i] = tabelasPonto[1][i]["Hora0"];
                    TempData["HoraCerto1" + i] = tabelasPonto[1][i]["Hora1"];
                }

                TempData["JornadaCerto" + i] = tabelasPonto[1][i]["Jornada"];
                if(TimeSpan.Parse( tabelasPonto[1][i]["HoraExtra"] ).Hours <= 0 &&
                    TimeSpan.Parse( tabelasPonto[1][i]["HoraExtra"] ).Minutes < 0)
                {
                    TempData["DebitoCerto" + i] = tabelasPonto[1][i]["HoraExtra"];
                    TempData["HoraExtraCerto" + i] = "";
                }
                else
                {
                    TempData["DebitoCerto" + i] = "";
                    TempData["HoraExtraCerto" + i] = tabelasPonto[1][i]["HoraExtra"];
                }
            }

            #endregion


            return PartialView( "SemPendencia" );
        }

        public async Task<ActionResult> ConfirmarJustificativa(string idPendencia)
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                var firebird = new QueryFirebird();

                var dadosPendencia = verificaDados.RecuperaDadosPendenciaConfirmacao( idPendencia );
                var dadosPendenciaFire = firebird.RetornaDadosMarcacao( dadosPendencia[0]["data"],
                    dadosPendencia[0]["idfuncionariofirebird"] );

                var envia = new EnviodeAlertas();
                var cadastroAlerta = new QueryMysql();

                var dadosOperador = cadastroAlerta.RetornaInformacoesNotificacao( dadosPendencia[0]["idfuncionario"] );
                if(dadosOperador != null)
                {
                    await envia.EnviaAlertaFuncionario( dadosOperador[0], "Sua justificativa foi confirmada.",
                        "11" );

                    var dadosGestor = cadastroAlerta.RetornaInformacoesGestor( dadosOperador[0]["idsetor"] );
                    if(dadosGestor.Count > 0)
                        await envia.EnviaAlertaFuncionario( dadosGestor[0],
                            "A justificativa do funcionário: " + dadosOperador[0]["nome"] + "  foi confirmada.", "11" );
                }


                bool existe;
                var negaFireBird = false;
                for(var i = 0; i < dadosPendencia.Count; i++)
                {
                    existe = false;
                    if(dadosPendencia[i]["observacao"] == null)
                    {
                        if(dadosPendencia[i]["horario"].Equals( "00:00:00" ))
                        {
                            negaFireBird = true;
                        }
                        else
                        {
                            for(var j = 0; j < dadosPendenciaFire.Count; j++)
                                if(dadosPendenciaFire[j]["HORA"].Equals( dadosPendencia[i]["horario"] ))
                                {
                                    existe = true;
                                    break;
                                }

                            if(!existe)
                            {
                                firebird.InserirMarcacao( dadosPendencia[i]["idfuncionariofirebird"],
                                    dadosPendencia[i]["idjustificativafirebird"], dadosPendencia[i]["data"],
                                    dadosPendencia[i]["horario"] );
                                verificaDados.AtualizaJustificativaRh( dadosPendencia[i]["idhistorico"] );
                            }
                        }
                    }
                    else
                    {
                        negaFireBird = true;
                    }
                }

                if(negaFireBird) verificaDados.AtualizaJustificativaRh( dadosPendencia[0]["idhistorico"] );


                return RedirectToAction( "ColaboradorRh", "PainelColaborador",
                    new { Mensagem = "Justificativa confirmada com sucesso !" } );
            }

            return RedirectToAction( "Login", "Login" );
        }

        public ActionResult ExcluirFuncao(string idFuncao)
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                verificaDados.ExcluirFuncao( idFuncao );
                return RedirectToAction( "ColaboradorRh", "PainelColaborador",
                    new { Mensagem = "Função excluida com sucesso !" } );
            }

            return RedirectToAction( "Login", "Login" );
        }

        public ActionResult Certificacao()
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if(logado) return PartialView( "Certificacao" );

            return RedirectToAction( "Login", "Login" );
        }

        [HttpPost]
        public ActionResult Certificacao(Certificacao dadosCadastro)
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                verificaDados.CadastrarCertificacao( dadosCadastro.NomeCertificacao );
                return RedirectToAction( "ColaboradorRh", "PainelColaborador",
                    new { Mensagem = "Certificação cadastrada com sucesso !" } );
            }

            return RedirectToAction( "Login", "Login" );
        }

        public ActionResult EditarCertificacao(Certificacao dadosCadastro, FormCollection dados)
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                verificaDados.EditarCertificacao( dados["IdCertificacao"], dadosCadastro.NomeCertificacao );
                return RedirectToAction( "ColaboradorRh", "PainelColaborador",
                    new { Mensagem = "Certificação atualizada com sucesso !" } );
            }

            return RedirectToAction( "Login", "Login" );
        }

        public ActionResult ExcluirCertificacao(string idFuncao)
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                verificaDados.ExcluirCertificacao( idFuncao );
                return RedirectToAction( "ColaboradorRh", "PainelColaborador",
                    new { Mensagem = "Certificação excluida com sucesso !" } );
            }

            return RedirectToAction( "Login", "Login" );
        }

        [HttpPost]
        public ActionResult BuscarCertificacao(string descricaoCertificacao)
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                var certificacao = verificaDados.BuscaCertificacao( descricaoCertificacao );
                for(var i = 0; i < certificacao.Count; i++)
                {
                    TempData["Id" + i] = certificacao[i]["id"];
                    TempData["Descricao" + i] = certificacao[i]["descricao"];
                }

                TempData["TotalResultado"] = certificacao.Count;
                TempData["Editar"] = "EditarCertificacao";
                return PartialView( "ResultadoPesquisaCertificacao" );
            }

            return RedirectToAction( "Login", "Login" );
        }

        [HttpPost]
        public ActionResult RecuperaCertificacao(string idCertificacao)
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                var dadosCertificacao = verificaDados.RecuperaDadosCertificacao( idCertificacao );
                var certificacaoRecupera = new Certificacao();

                certificacaoRecupera.NomeCertificacao = dadosCertificacao[0]["descricao"];
                TempData["IdCertificacao"] = dadosCertificacao[0]["id"];
                return PartialView( "EditarCertificacao", certificacaoRecupera );
            }

            return RedirectToAction( "Login", "Login" );
        }

        public ActionResult Gestor()
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                var setores = verificaDados.RetornaSetores();
                TempData["TotalSetores"] = setores.Count;
                for(var i = 0; i < setores.Count; i++)
                {
                    TempData["DescricaoSetor" + i] = setores[i]["descricao"];
                    TempData["IdSetor" + i] = setores[i]["id"];
                }

                var funcionarios = verificaDados.RetornaFuncionarios();
                TempData["TotalFuncionarios"] = setores.Count;
                for(var i = 0; i < funcionarios.Count; i++)
                {
                    TempData["Nome" + i] = funcionarios[i]["nome"];
                    TempData["IdFuncionario" + i] = funcionarios[i]["id"];
                }

                return PartialView( "Gestor" );
            }

            return RedirectToAction( "Login", "Login" );
        }

        [HttpPost]
        public ActionResult Gestor(FormCollection dadosCadastro)
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                verificaDados.CadastrarGestor( dadosCadastro["Funcionario"], dadosCadastro["Setor"] );
                return RedirectToAction( "ColaboradorRh", "PainelColaborador",
                    new { Mensagem = "Gestor cadastrado com sucesso !" } );
            }

            return RedirectToAction( "Login", "Login" );
        }

        public ActionResult EditarGestor(FormCollection dados)
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                verificaDados.EditarGestor( dados["FuncionarioEdicao"], dados["SetorEdicao"] );
                return RedirectToAction( "ColaboradorRh", "PainelColaborador",
                    new { Mensagem = "Certificação atualizada com sucesso !" } );
            }

            return RedirectToAction( "Login", "Login" );
        }

        public ActionResult ExcluirGestor(string idGestor)
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                verificaDados.ExcluirGestor( idGestor );
                return RedirectToAction( "ColaboradorRh", "PainelColaborador",
                    new { Mensagem = "Gestor removido com sucesso !" } );
            }

            return RedirectToAction( "Login", "Login" );
        }

        [HttpPost]
        public ActionResult BuscarGestor(string descricaoGestor)
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                var gestor = verificaDados.BuscaGestor( descricaoGestor );
                for(var i = 0; i < gestor.Count; i++)
                {
                    TempData["Id" + i] = gestor[i]["id"];
                    TempData["Descricao" + i] = gestor[i]["nome"];
                }

                TempData["TotalResultado"] = gestor.Count;
                TempData["Editar"] = "EditarGestor";

                return PartialView( "ResultadoPesquisaGestor" );
            }

            return RedirectToAction( "Login", "Login" );
        }

        [HttpPost]
        public ActionResult RecuperaGestor(string idGestor)
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                var dadosGestor = verificaDados.RecuperaDadosGestor( idGestor );

                var setores = verificaDados.RetornaSetores();
                TempData["TotalSetores"] = setores.Count;
                for(var i = 0; i < setores.Count; i++)
                {
                    TempData["DescricaoSetor" + i] = setores[i]["descricao"];
                    TempData["IdSetor" + i] = setores[i]["id"];
                    if(setores[i]["id"].Equals( dadosGestor[0]["idsetor"] ))
                        TempData["ValorSetor" + i] = "checked";
                    else
                        TempData["ValorSetor" + i] = "";
                }

                var funcionarios = verificaDados.RetornaFuncionarios();
                TempData["TotalFuncionarios"] = setores.Count;
                for(var i = 0; i < funcionarios.Count; i++)
                {
                    TempData["Nome" + i] = funcionarios[i]["nome"];
                    TempData["IdFuncionario" + i] = funcionarios[i]["id"];
                    if(funcionarios[i]["id"].Equals( dadosGestor[0]["id"] ))
                        TempData["ValorGestor" + i] = "checked";
                    else
                        TempData["ValorGestor" + i] = "";
                }

                TempData["ExcluirIdGestor"] = dadosGestor[0]["id"];
                TempData["ExcluirIdSetor"] = dadosGestor[0]["idsetor"];

                return PartialView( "EditarGestor" );
            }

            return RedirectToAction( "Login", "Login" );
        }


        public ActionResult Setor()
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if(logado) return PartialView( "Setor" );

            return RedirectToAction( "Login", "Login" );
        }

        [HttpPost]
        public ActionResult Setor(Setor dadosCadastro)
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                verificaDados.CadastrarSetor( dadosCadastro.NomeSetor );
                return RedirectToAction( "ColaboradorRh", "PainelColaborador",
                    new { Mensagem = "Setor cadastrado com sucesso !" } );
            }

            return RedirectToAction( "Login", "Login" );
        }

        public ActionResult EditarSetor(Setor dadosCadastro, FormCollection dados)
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                verificaDados.EditarSetor( dados["IdSetor"], dadosCadastro.NomeSetor );
                return RedirectToAction( "ColaboradorRh", "PainelColaborador",
                    new { Mensagem = "Setor atualizada com sucesso !" } );
            }

            return RedirectToAction( "Login", "Login" );
        }

        public ActionResult ExcluirSetor(string idSetor)
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                verificaDados.ExcluirSetor( idSetor );
                return RedirectToAction( "ColaboradorRh", "PainelColaborador",
                    new { Mensagem = "Setor excluido com sucesso !" } );
            }

            return RedirectToAction( "Login", "Login" );
        }

        [HttpPost]
        public ActionResult BuscarSetor(string descricaoSetor)
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                var setor = verificaDados.BuscaSetor( descricaoSetor );

                for(var i = 0; i < setor.Count; i++)
                {
                    TempData["Id" + i] = setor[i]["id"];
                    TempData["Descricao" + i] = setor[i]["descricao"];
                }

                TempData["TotalResultado"] = setor.Count;
                TempData["Editar"] = "EditarSetor";

                return PartialView( "ResultadoPesquisaSetor" );
            }

            return RedirectToAction( "Login", "Login" );
        }

        [HttpPost]
        public ActionResult RecuperaSetor(string idSetor)
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                var dadosCertificacao = verificaDados.RecuperaDadosSetor( idSetor );
                var setorRecupera = new Setor();

                setorRecupera.NomeSetor = dadosCertificacao[0]["descricao"];
                TempData["IdSetor"] = dadosCertificacao[0]["id"];
                return PartialView( "EditarSetor", setorRecupera );
            }

            return RedirectToAction( "Login", "Login" );
        }


        public ActionResult Indicadores()
        {
            return View( "Indicadores" );
        }

        public ActionResult OitoHoras()
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                var fire = new QueryFirebird();
                var acima = 0;
                var abaixo = 0;
                var acima12 = 0;
                var count = 0;

                var bancoDeHoras = verificaDados.RetornaBancoHoras();
                TempData["ValorUAD"] = 0;
                TempData["ValorMatriz"] = 0;
                TempData["ValorParana"] = 0;
                TempData["ValorCajuru"] = 0;
                TempData["ValorClara"] = 0;
                TempData["ValorBh"] = 0;
                TempData["ValorBetim"] = 0;
                TempData["ValorContagem"] = 0;
                TempData["ValorGoias"] = 0;
                TempData["ValorBarreiro"] = 0;
                var totalMatriz = new TimeSpan();
                var totalFuncionariosMatriz = 0;

                for(var i = 0; i < bancoDeHoras.Count; i++)
                {
                    var nome = fire.RetornaFuncionarioMatricula( bancoDeHoras[i]["crachafirebird"] );


                    try
                    {
                        if(nome[0]["ID_CENTRO_CUSTO"].Equals( "1" ))
                        {
                            totalMatriz = totalMatriz.Add( TimeSpan.Parse( bancoDeHoras[i]["hora"] ) );
                            totalFuncionariosMatriz++;
                        }
                        else if(nome[0]["ID_CENTRO_CUSTO"].Equals( "2" ))
                        {
                        }
                        else if(nome[0]["ID_CENTRO_CUSTO"].Equals( "3" ))
                        {
                        }
                        else if(nome[0]["ID_CENTRO_CUSTO"].Equals( "4" ))
                        {
                        }
                        else if(nome[0]["ID_CENTRO_CUSTO"].Equals( "5" ))
                        {
                        }
                        else if(nome[0]["ID_CENTRO_CUSTO"].Equals( "6" ))
                        {
                        }
                        else if(nome[0]["ID_CENTRO_CUSTO"].Equals( "7" ))
                        {
                        }
                        else if(nome[0]["ID_CENTRO_CUSTO"].Equals( "8" ))
                        {
                        }
                        else if(nome[0]["ID_CENTRO_CUSTO"].Equals( "10" ))
                        {
                        }
                        else if(nome[0]["ID_CENTRO_CUSTO"].Equals( "11" ))
                        {
                        }
                    }
                    catch
                    {
                        // ignored
                    }

                    try
                    {
                        if(Convert.ToInt32( bancoDeHoras[i]["hora"].Split( ':' )[0] ) >= 8 &&
                            Convert.ToInt32( bancoDeHoras[i]["hora"].Split( ':' )[0] ) <= 11)
                        {
                            if(nome.Count > 0)
                            {
                                TempData["Nome" + count] = nome[0]["NOME"];
                                TempData["Data" + count] = Convert.ToDateTime( bancoDeHoras[i]["datareferencia"] )
                                    .ToString( "dd/MM/yyyy" );
                                TempData["TotalHoras" + count] = bancoDeHoras[i]["hora"];
                                acima++;
                                count++;
                            }
                        }
                        else if(Convert.ToInt32( bancoDeHoras[i]["hora"].Split( ':' )[0] ) >= 12)
                        {
                            if(nome.Count > 0)
                            {
                                TempData["Nome" + count] = nome[0]["NOME"];
                                TempData["Data" + count] = Convert.ToDateTime( bancoDeHoras[i]["datareferencia"] )
                                    .ToString( "dd/MM/yyyy" );
                                TempData["TotalHoras" + count] = bancoDeHoras[i]["hora"];
                                acima12++;
                                count++;
                            }
                        }
                        else
                        {
                            abaixo++;
                        }
                    }
                    catch
                    {
                        if(nome.Count > 0)
                        {
                            TempData["Nome" + count] = nome[0]["NOME"];
                            TempData["Data" + count] = Convert.ToDateTime( bancoDeHoras[i]["datareferencia"] )
                                .ToString( "dd/MM/yyyy" );
                            TempData["TotalHoras" + count] = bancoDeHoras[i]["hora"];
                            acima++;
                        }
                    }
                }

                TempData["Total"] = count;
                TempData["Valor1"] = abaixo;
                TempData["Valor2"] = acima;
                TempData["Valor3"] = acima12;
                TempData["ValorMatriz"] = totalMatriz.Hours / totalFuncionariosMatriz;


                return PartialView( "IndicadorOitoHoras" );
            }

            return RedirectToAction( "Login", "Login" );
        }
        [HttpPost]
        public ActionResult AtualizarDadosRh(Funcionario dadosFuncionario)
        {
            var verificaDados = new QueryMysqlRh();
            var logado = verificaDados.UsuarioLogado();
            if(logado)
            {
                var cookie = Request.Cookies.Get( "CookieFarm" );
                if(cookie != null)
                {
                    var login = Criptografa.Descriptografar( cookie.Value );
                    try
                    {
                        verificaDados.AtualizaDadosPerfilRh( dadosFuncionario.DataAdmissaoFuncionario,
                            dadosFuncionario.VencimentoPeriodico, dadosFuncionario.IdFuncao,
                            Convert.ToDouble( dadosFuncionario.Salario ),
                            Convert.ToDouble( dadosFuncionario.QuebraDeCaixa ),
                            Convert.ToDouble( dadosFuncionario.Anuenio ), Convert.ToDouble( dadosFuncionario.Ticket ),
                            dadosFuncionario.Estagiario, dadosFuncionario.DataEstagiario, dadosFuncionario.IdSetor,
                            login );
                    }
                    catch
                    {
                        verificaDados.AtualizaDadosPerfilRh( dadosFuncionario.DataAdmissaoFuncionario,
                            dadosFuncionario.VencimentoPeriodico, dadosFuncionario.IdFuncao,0,0,0, 0,
                            dadosFuncionario.Estagiario, dadosFuncionario.DataEstagiario, dadosFuncionario.IdSetor,
                            login );
                    }

                }

                return RedirectToAction( "ColaboradorRh", "PainelColaborador",
                    new { Mensagem = "Dados atualizados com sucesso !" } );

            }
            return RedirectToAction( "Login", "Login" );
        }
    }
}