using System;
using System.Collections;
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
        public ActionResult Chamados(string Mensagem)
        {
            var VerificaDados = new QueryMysqlWebdesk();

            var Logado = VerificaDados.UsuarioLogado();
            if ( Logado )
            {
                TempData ["Mensagem"] = Mensagem;
                var Cookie = Request.Cookies.Get( "CookieFarm" );
                var Login = Criptografa.Descriptografar( Cookie.Value );

                var DadosUsuario = VerificaDados.RecuperaDadosUsuarios( Login );

                var permissao = new QueryMysql();
                if ( permissao.PermissaoCurriculos( Login ) )
                    TempData ["PermissaoCurriculo"] =
                        " ";
                else
                    TempData ["PermissaoCurriculo"] = "display: none";

                if ( DadosUsuario [0] ["foto"] == null )
                    TempData ["ImagemPerfil"] = "http://bulma.io/images/placeholders/128x128.png";
                else
                    TempData ["ImagemPerfil"] = DadosUsuario [0] ["foto"];

                TempData ["NomeLateral"] = DadosUsuario [0] ["login"];

                var ChamadosEmAberto = VerificaDados.RetornaChamadosAbertos( DadosUsuario [0] ["id"] );
                TempData ["TotalChamados"] = ChamadosEmAberto.Count;
                for ( var i = 0; i < ChamadosEmAberto.Count; i++ )
                {
                    if ( ChamadosEmAberto [i] ["cpf"] != "" )
                        TempData ["Titulo" + i] =
                            ChamadosEmAberto [i] ["titulo"] + " CPF/CNPJ: " + ChamadosEmAberto [i] ["cpf"];
                    else
                        TempData ["Titulo" + i] =
                            ChamadosEmAberto [i] ["titulo"];

                    TempData ["Numero" + i] = ChamadosEmAberto [i] ["id"];
                    TempData ["Operador" + i] = ChamadosEmAberto [i] ["operador"];
                    TempData ["Situacao" + i] = ChamadosEmAberto [i] ["situacao"];

                    if ( ChamadosEmAberto [i] ["fimatendimento"] == null )
                    {
                        var Sla = TimeSpan.Parse( ChamadosEmAberto [i] ["sla"] );

                        var Horas = new double();
                        if ( TimeSpan.Parse( ChamadosEmAberto [i] ["tempo"] ).TotalMinutes == 0 )
                        {
                            Horas = 00;
                        }
                        else
                        {
                            Horas = Sla.TotalMinutes * 100 / TimeSpan.Parse( ChamadosEmAberto [i] ["tempo"] ).TotalMinutes;
                        }
                        if ( Horas > 100 )
                            TempData ["StatusCor" + i] = "is-danger";
                        else
                            TempData ["StatusCor" + i] = "is-primary";

                        TempData ["InformacaoSLA" + i] = "TEMPO DECORRIDO:" + Sla.Days + " DIAS, " + Sla.Hours + ":" +
                                                        Sla.Minutes + ":00" + " || TEMPO ESTIMADO: " +
                                                        ChamadosEmAberto [i] ["tempo"];
                        TempData ["Sla" + i] = Convert.ToInt32( Horas );
                    }
                    else
                    {
                        TempData ["InformacaoSLA" + i] = "SOLICITAÇÃO ENCERRADA";
                        TempData ["Sla" + i] = 100;
                    }
                }

                var ChamadosOperador = VerificaDados.RetornaChamadosResponsavel( DadosUsuario [0] ["id"] );

                TempData ["TotalChamadosOperador"] = ChamadosOperador.Count;
                for ( var i = 0; i < ChamadosOperador.Count; i++ )
                {
                    if ( ChamadosOperador [i] ["cpf"] != "" )
                        TempData ["TituloOperador" + i] =
                            ChamadosOperador [i] ["titulo"] + " CPF/CNPJ: " + ChamadosOperador [i] ["cpf"];
                    else
                        TempData ["TituloOperador" + i] =
                            ChamadosOperador [i] ["titulo"];
                    TempData ["NumeroOperador" + i] = ChamadosOperador [i] ["id"];
                    TempData ["OperadorOperador" + i] = ChamadosOperador [i] ["operador"];
                    TempData ["SituacaoOperador" + i] = ChamadosOperador [i] ["situacao"];
                    TempData ["CadastroOperador" + i] = ChamadosOperador [i] ["cadastro"];

                    var Sla = TimeSpan.Parse( ChamadosOperador [i] ["sla"] );

                    var Horas = new double();
                    if ( TimeSpan.Parse( ChamadosOperador [i] ["tempo"] ).TotalMinutes == 0 )
                    {
                        Horas = 00;
                    }
                    else
                    {
                        Horas = Sla.TotalMinutes * 100 / TimeSpan.Parse( ChamadosOperador [i] ["tempo"] ).TotalMinutes;
                    }
                    if ( Horas > 100 )
                        TempData ["StatusCorOperador" + i] = "is-danger";
                    else
                        TempData ["StatusCorOperador" + i] = "is-primary";

                    TempData ["InformacaoSLAOperador" + i] =
                        "TEMPO DECORRIDO:" + Sla.Days + " DIAS, " + Sla.Hours + ":" + Sla.Minutes + ":00" +
                        " || TEMPO ESTIMADO: " + ChamadosOperador [i] ["tempo"];
                    TempData ["SlaOperador" + i] = Convert.ToInt32( Horas );
                }


                var ChamadosSetor = VerificaDados.RetornaChamadosSetor( DadosUsuario [0] ["idsetor"] );

                TempData ["TotalChamadosSetor"] = ChamadosSetor.Count;
                for ( var i = 0; i < ChamadosSetor.Count; i++ )
                {
                    if ( ChamadosSetor [i] ["cpf"] != "" )
                        TempData ["TituloSetor" + i] =
                            ChamadosSetor [i] ["titulo"] + " CPF/CNPJ: " + ChamadosSetor [i] ["cpf"];
                    else
                        TempData ["TituloSetor" + i] =
                            ChamadosSetor [i] ["titulo"];
                    TempData ["NumeroSetor" + i] = ChamadosSetor [i] ["id"];
                    TempData ["OperadorSetor" + i] = ChamadosSetor [i] ["operador"];
                    TempData ["SituacaoSetor" + i] = ChamadosSetor [i] ["situacao"];
                    TempData ["CadastroSetor" + i] = ChamadosSetor [i] ["cadastro"];

                    var Sla = TimeSpan.Parse( ChamadosSetor [i] ["sla"] );
                    var Horas = new double();
                    if ( TimeSpan.Parse( ChamadosSetor [i] ["tempo"] ).TotalMinutes == 0 )
                    {
                        Horas = 00;
                    }
                    else
                    {
                        Horas = Sla.TotalMinutes * 100 / TimeSpan.Parse( ChamadosSetor [i] ["tempo"] ).TotalMinutes;
                    }
                    if ( Horas > 100 )
                        TempData ["StatusCorSetor" + i] = "is-danger";
                    else
                        TempData ["StatusCorSetor" + i] = "is-primary";

                    TempData ["InformacaoSLASetor" + i] = "TEMPO DECORRIDO:" + Sla.Days + " DIAS, " + Sla.Hours + ":" +
                                                         Sla.Minutes + ":00" + " || TEMPO ESTIMADO: " +
                                                         ChamadosSetor [i] ["tempo"];
                    TempData ["SlaSetor" + i] = Convert.ToInt32( Horas );
                }


                if ( DadosUsuario [0] ["gestor"].Equals( "S" ) )
                {
                    TempData ["PermissaoGestor"] = "N";
                    TempData ["AreaGestor"] = "S";
                }
                else
                {
                    TempData ["PermissaoGestor"] = "N";
                    TempData ["AreaGestor"] = "N";
                }

                var Solicitacao = new SolicitacaoWebDesk();
                var Setores = VerificaDados.RetornaSetor();
                Solicitacao.SetorResponsavel = Setores;

                return View( Solicitacao );
            }

            return RedirectToAction( "Login", "Login" );
        }
        [ValidateInput( false )]
        public ActionResult BuscaChamados(string Busca)
        {
            var VerificaDados = new QueryMysqlWebdesk();

            var Logado = VerificaDados.UsuarioLogado();
            if ( Logado )
            {
                var Cookie = Request.Cookies.Get( "CookieFarm" );
                var Login = Criptografa.Descriptografar( Cookie.Value );
                var VerificaDadosUsuario = new QueryMysql();

                var DadosUsuarios = VerificaDadosUsuario.RecuperaDadosFuncionariosTabelaUsuario( Login );
                var DadosFuncionarios = VerificaDadosUsuario.RecuperaDadosUsuarios( Login );

                var BuscaWebdesk = new BuscaElasticSearch();

                if ( DadosFuncionarios [0] ["foto"] == null )
                    TempData ["ImagemPerfil"] = "http://bulma.io/images/placeholders/128x128.png";
                else
                    TempData ["ImagemPerfil"] = DadosFuncionarios [0] ["foto"];

                TempData ["NomeLateral"] = DadosFuncionarios [0] ["login"];

                var permissao = new QueryMysql();
                if ( permissao.PermissaoCurriculos( Login ) )
                    TempData ["PermissaoCurriculo"] =
                        " ";
                else
                    TempData ["PermissaoCurriculo"] = "display: none";


                var ResultadoPEsquisa = new List<Dictionary<string,string>>();
                var DadoResultado = new List<IHit<Webdesk>>();
                if ( VerificaDadosUsuario.PermissaoPesquisaWebdDesk( Login ) )
                {
                    DadoResultado = BuscaWebdesk.PesquisaTotalWebdesk( Busca );
                    ResultadoPEsquisa = VerificaDados.BuscaChamadosTotalAntigo( Busca, DadosUsuarios [0] ["id"]);

                }
                else
                {
                    ResultadoPEsquisa = VerificaDados.BuscaChamadosMeuSetor( Busca, DadosUsuarios [0] ["id"], DadosUsuarios [0] ["idsetor"] );
                    DadoResultado = BuscaWebdesk.PesquisaBasicaWebdesk( Busca, DadosFuncionarios [0] ["idsetor"] );
                }

                TempData ["TotalResultado"] = ResultadoPEsquisa.Count;
                TempData ["TotalResultadoNovo"] = DadoResultado.Count;

                for ( var i = 0; i < ResultadoPEsquisa.Count; i++ )
                {
                    TempData ["NumeroChamado" + i] = ResultadoPEsquisa [i] ["id"];
                    TempData ["TituloChamado" + i] = ResultadoPEsquisa [i] ["titulochamado"];
                    TempData ["UsuarioCadastro" + i] = ResultadoPEsquisa [i] ["CADASTRO"];
                    TempData ["Operador" + i] = ResultadoPEsquisa [i] ["OPERADOR"];
                    var Interacoes = VerificaDados.BuscaInteracaoChamados( ResultadoPEsquisa [i] ["id"] );

                    TempData ["TotalInteracao" + ResultadoPEsquisa [i] ["id"]] = Interacoes.Count;

                    for ( var j = 0; j < Interacoes.Count; j++ )
                    {
                        TempData ["UsuarioInteracao" + ResultadoPEsquisa [i] ["id"] + j] = Interacoes [j] ["nome"];
                        TempData ["TextoInteracao" + ResultadoPEsquisa [i] ["id"] + j] = Interacoes [j] ["textointeracao"];
                        TempData ["DataInteracao" + ResultadoPEsquisa [i] ["id"] + j] = Interacoes [j] ["data"];
                    }
                }



                for ( var i = 0; i < DadoResultado.Count; i++ )
                {
                    TempData ["NumeroChamadoNovo" + i] = DadoResultado [i].Source.idsolicitacao;
                    var DadosChamado = VerificaDados.RetornaDadosChamado( DadoResultado [i].Source.idsolicitacao );
                    TempData ["TituloChamadoNovo" + i] = DadosChamado [0] ["titulo"];
                    TempData ["UsuarioCadastroNovo" + i] = DadosChamado [0] ["cadastro"];
                    TempData ["OperadorNovo" + i] = DadosChamado [0] ["operador"];

                    var Interacoes = VerificaDados.BuscaInteracaoChamadosNovo( DadoResultado [i].Source.idsolicitacao );

                    TempData ["TotalInteracaoNovo" + DadoResultado [i].Source.idsolicitacao] = Interacoes.Count;

                    for ( var j = 0; j < Interacoes.Count; j++ )
                    {
                        TempData ["UsuarioInteracaoNovo" + DadoResultado [i].Source.idsolicitacao + j] = Interacoes [j] ["nome"];
                        TempData ["TextoInteracaoNovo" + DadoResultado [i].Source.idsolicitacao + j] =
                            Interacoes [j] ["textointeracao"];
                        TempData ["DataInteracaoNovo" + DadoResultado [i].Source.idsolicitacao + j] = Interacoes [j] ["data"];
                    }
                }


                return View( "ResultadoPesquisa" );
            }

            return RedirectToAction( "Login", "Login" );
        }

        [HttpPost]
        [ValidateInput( false )]
        public async Task<ActionResult> CadastrarSolicitacao(FormCollection Dados, IEnumerable<HttpPostedFileBase> postedFiles)
        {
            var VerificaDados = new QueryMysqlWebdesk();
            var Logado = VerificaDados.UsuarioLogado();
            if ( Logado )
            {
                var Cookie = Request.Cookies.Get( "CookieFarm" );
                var Login = Criptografa.Descriptografar( Cookie.Value );

                var DadosUsuario = VerificaDados.RecuperaDadosUsuarios( Login );
                var IdInteracao = "";
                if ( Dados ["CpfAbertura"] != "" )
                {
                    IdInteracao = VerificaDados.CadastraSolicitacao( Dados ["IdSetorResponsavel"], Dados ["IdCategoria"],
                        Dados ["IdFuncionarioResponsavel"],
                        Dados ["Descricao"], DadosUsuario [0] ["id"], Dados ["CpfAbertura"] );
                }
                else
                {
                    IdInteracao = VerificaDados.CadastraSolicitacao( Dados ["IdSetorResponsavel"], Dados ["IdCategoria"],
                        Dados ["IdFuncionarioResponsavel"],
                        Dados ["Descricao"], DadosUsuario [0] ["id"], "" );
                }

                for ( int i = 0; i < Dados.Count; i++ )
                {
                    if ( !Dados.GetKey( i ).Equals( "IdSetorResponsavel" ) && !Dados.GetKey( i ).Equals( "IdCategoria" ) &&
                        !Dados.GetKey( i ).Equals( "IdFuncionarioResponsavel" ) && !Dados.GetKey( i ).Equals( "CpfAbertura" ) &&
                        !Dados.GetKey( i ).Equals( "Descricao" ) )
                    {
                        VerificaDados.InserirFormulario( Dados.GetKey( i ), Dados [i], IdInteracao.Split( ';' ) [1] );
                    }
                }

                var lista = postedFiles.ToList();

                for ( var i = 0; i < lista.Count; i++ )
                    if ( lista [i] != null )
                    {
                        var NomeArquivo = Path.GetFileName( lista [i].FileName );
                        byte [] fileData = null;
                        using ( var binaryReader = new BinaryReader( lista [i].InputStream ) )
                        {
                            fileData = binaryReader.ReadBytes( lista [i].ContentLength );
                        }

                        VerificaDados.InserirAnexo( IdInteracao.Split( ';' ) [0], fileData, lista [i].ContentType, NomeArquivo );
                    }

                var Envia = new EnviodeAlertas();
                var CadastroAlerta = new QueryMysql();

                var DadosOperador = VerificaDados.RetornaInformacoesNotificacao( Dados ["IdFuncionarioResponsavel"] );
                if ( DadosOperador [0] ["notificacaoemail"].Equals( "Sim" ) )
                {
                    CadastroAlerta.cadastrarAlert( Dados ["IdFuncionarioResponsavel"], "6", "Foi Aberto um chamado para você." );
                    await Envia.EnviaEmail( DadosOperador [0] ["email"], "Foi aberto um chamado para você." );
                    if ( DadosOperador [0] ["idnotificacao"].ToString().Length > 0 )
                    {
                        Envia.CadastraAlerta( DadosOperador [0] ["idnotificacao"], "Foi Aberto um chamado para você." );
                    }
                }
                else
                {
                    CadastroAlerta.cadastrarAlert( Dados ["IdFuncionarioResponsavel"], "6", "Foi Aberto um chamado para você." );
                    if ( DadosOperador [0] ["idnotificacao"].ToString().Length > 0 )
                    {
                        Envia.CadastraAlerta( DadosOperador [0] ["idnotificacao"], "Foi Aberto um chamado para você." );
                    }
                }
                return RedirectToAction( "Chamados", "Webdesk", new { Mensagem = "Solicitação cadastrada com sucesso!" } );
            }

            return RedirectToAction( "Login", "Login" );
        }

        public ActionResult RetornaCategoriaFuncionario(string IdSetor)
        {
            var Solicitacao = new SolicitacaoWebDesk();
            var VerificaDados = new QueryMysqlWebdesk();
            var Categoria = VerificaDados.RetornaCategoria( IdSetor );
            var Funcionario = VerificaDados.RetornaFuncionario( IdSetor );

            Solicitacao.Categoria = Categoria;
            Solicitacao.FuncionarioResponsavel = Funcionario;

            return View( "CategoriaSetorSolicitacao", Solicitacao );
        }

        [HttpPost]
        public ActionResult RetornaCategoria(string IdCadastro)
        {
            var Solicitacao = new SolicitacaoWebDesk();
            var VerificaDados = new QueryMysqlWebdesk();
            var Cookie = Request.Cookies.Get( "CookieFarm" );
            var Login = Criptografa.Descriptografar( Cookie.Value );
            var DadosUsuario = VerificaDados.RecuperaDadosUsuarios( Login );
            if ( IdCadastro.Equals( DadosUsuario [0] ["id"] ) ) return View( "faltapermissao" );

            var Categoria = VerificaDados.RetornaCategoria( DadosUsuario [0] ["idsetor"] );

            Solicitacao.Categoria = Categoria;

            return View( "CategoriaInteracao", Solicitacao );
        }

        public ActionResult RetornaSetor(string IdCadastro)
        {
            var Solicitacao = new SolicitacaoWebDesk();
            var VerificaDados = new QueryMysqlWebdesk();
            var Cookie = Request.Cookies.Get( "CookieFarm" );
            var Login = Criptografa.Descriptografar( Cookie.Value );
            var DadosUsuario = VerificaDados.RecuperaDadosUsuarios( Login );
            if ( IdCadastro.Equals( DadosUsuario [0] ["id"] ) ) return View( "faltapermissao" );

            var Setor = VerificaDados.RetornaSetor();
            Solicitacao.SetorResponsavel = Setor;

            return View( "SetorInteracao", Solicitacao );
        }

        public ActionResult RetornaFuncionario(string IdSetor)
        {
            var Solicitacao = new SolicitacaoWebDesk();
            var VerificaDados = new QueryMysqlWebdesk();
            var Funcionario = VerificaDados.RetornaFuncionario( IdSetor );

            Solicitacao.FuncionarioResponsavel = Funcionario;

            return View( "FuncionarioInteracao", Solicitacao );
        }

        public ActionResult Documentos(string Anexos)
        {
            return View( "Chamados" );
        }

        public ActionResult InteracaoChamado(string IdChamado, string Mensagem, string TipoChamado)
        {
            if ( TipoChamado.Equals( "Novo" ) )
            {

                var VerificaDados = new QueryMysqlWebdesk();
                var Logado = VerificaDados.UsuarioLogado();
                if ( Logado )
                {

                    var Cookie = Request.Cookies.Get( "CookieFarm" );
                    var Login = Criptografa.Descriptografar( Cookie.Value );

                    var DadosUsuario = VerificaDados.RecuperaDadosUsuarios( Login );
                    var Formulario = VerificaDados.RetornaFormularioChamado( IdChamado );


                    if ( DadosUsuario [0] ["foto"] == null )
                        TempData ["ImagemPerfil"] = "http://bulma.io/images/placeholders/128x128.png";
                    else
                        TempData ["ImagemPerfil"] = DadosUsuario [0] ["foto"];

                    var permissao = new QueryMysql();
                    if ( permissao.PermissaoCurriculos( Login ) )
                        TempData ["PermissaoCurriculo"] =
                            " ";
                    else
                        TempData ["PermissaoCurriculo"] = "display: none";

                    TempData ["NomeLateral"] = DadosUsuario [0] ["login"];
                    TempData ["IdSolicitacao"] = IdChamado;
                    var DadosChamado = VerificaDados.RetornaDadosChamado( IdChamado );



                    TempData ["TotalFormulario"] = Formulario.Count;
                    for ( int i = 0; i < Formulario.Count; i++ )
                    {
                        TempData ["NomeUsuarioFormulario"] = DadosChamado [0] ["cadastro"];
                        TempData ["NomeCampo" + i] = Formulario [i] ["nomecampo"];
                        TempData ["DadoFormulario" + i] = Formulario [i] ["dadoformulario"];

                    }



                    TempData ["Setor"] = DadosChamado [0] ["setor"];
                    TempData ["Situacao"] = DadosChamado [0] ["situacao"];
                    TempData ["Categoria"] = DadosChamado [0] ["titulo"];
                    TempData ["FuncionarioSolicitante"] = DadosChamado [0] ["cadastro"];
                    TempData ["FuncionarioResponsavel"] = DadosChamado [0] ["operador"];
                    TempData ["DataAbertura"] = DadosChamado [0] ["datahoracadastro"];
                    TempData ["IdFuncionarioCadastro"] = DadosChamado [0] ["idcadastro"];

                    if ( DadosChamado [0] ["inicioatendimento"] == null &&
                        !DadosChamado [0] ["cadastro"].Equals( DadosUsuario [0] ["id"] ) )
                    {
                        TempData ["IniciarAtendimento"] = "true";
                        TempData ["InicioAtendimento"] = "NÃO INICIADO";
                    }
                    else
                    {
                        TempData ["IniciarAtendimento"] = "false";
                        TempData ["InicioAtendimento"] = DadosChamado [0] ["inicioatendimento"];
                    }

                    if ( DadosChamado [0] ["fimatendimento"] == null )
                        TempData ["FimATendimento"] = "NÃO FINALIZADO";
                    else
                        TempData ["FimATendimento"] = DadosChamado [0] ["fimatendimento"];

                    if ( DadosChamado [0] ["situacao"].Equals( "CONCLUÍDO" ) )
                        TempData ["Status"] = "is-link";
                    else if ( DadosChamado [0] ["situacao"].Equals( "EM ATENDIMENTO" ) )
                        TempData ["Status"] = "is-info";
                    else
                        TempData ["Status"] = "is-warning";

                    var Interacoes = VerificaDados.RetornaInteracoesChamado( IdChamado );

                    TempData ["TotalInteracao"] = Interacoes.Count;
                    for ( var i = 0; i < Interacoes.Count; i++ )
                    {
                        TempData ["Usuario" + i] = Interacoes [i] ["nome"] + " " +
                                                  Convert.ToDateTime( Interacoes [i] ["datahorainteracao"] )
                                                      .ToString( "dd/MM/yyyy" );
                        TempData ["Interacao" + i] = Interacoes [i] ["textointeracao"];
                        var AnexoInteracao = VerificaDados.RetornaAnexoInteracao( Interacoes [i] ["id"] );
                        TempData ["IdInteracao" + i] = Interacoes [i] ["id"];
                        if ( Interacoes [i] ["acao"].Equals( "S" ) )
                            TempData ["Acao" + i] = "is-selected";
                        else
                            TempData ["Acao" + i] = "";

                        TempData ["TotalAnexos" + Interacoes [i] ["id"]] = AnexoInteracao.Rows.Count;
                        for ( var j = 0; j < AnexoInteracao.Rows.Count; j++ )
                        {
                            var bytes = ( byte [] ) AnexoInteracao.Rows [j] ["arquivo"];
                            var img64 = Convert.ToBase64String( bytes );
                            var img64Url =
                                string.Format( "data:" + AnexoInteracao.Rows [j] ["tipoarquivo"] + ";base64,{0}", img64 );
                            TempData ["NomeAnexo" + Interacoes [i] ["id"] + j] = AnexoInteracao.Rows [j] ["nomearquivo"];
                            TempData ["ImagemAnexo" + Interacoes [i] ["id"] + j] = img64Url;
                        }
                    }

                    TempData ["Mensagem"] = Mensagem;
                    return View();
                }

                return RedirectToAction( "Login", "Login" );
            }
            else
            {
                var VerificaDados = new QueryMysqlWebdesk();
                var Logado = VerificaDados.UsuarioLogado();
                if ( Logado )
                {
                    var Cookie = Request.Cookies.Get( "CookieFarm" );
                    var Login = Criptografa.Descriptografar( Cookie.Value );

                    var DadosUsuario = VerificaDados.RecuperaDadosUsuarios( Login );

                    TempData ["IdSolicitacao"] = IdChamado;
                    var DadosChamado = VerificaDados.RetornaDadosChamadoAntigo( IdChamado );
                    TempData ["Setor"] = DadosChamado [0] ["setor"];
                    TempData ["Situacao"] = DadosChamado [0] ["situacao"];
                    TempData ["Categoria"] = DadosChamado [0] ["titulo"];
                    TempData ["FuncionarioSolicitante"] = DadosChamado [0] ["cadastro"];
                    TempData ["FuncionarioResponsavel"] = DadosChamado [0] ["operador"];
                    TempData ["DataAbertura"] = DadosChamado [0] ["datahoracadastro"];
                    TempData ["IdFuncionarioCadastro"] = DadosChamado [0] ["idcadastro"];

                    TempData ["IniciarAtendimento"] = "false";
                    TempData ["InicioAtendimento"] = DadosChamado [0] ["confirmacaoleitura"];
                    TempData ["FimATendimento"] = DadosChamado [0] ["dataconclusaochamado"];


                    if ( DadosChamado [0] ["situacao"].Equals( "CONCLUÍDO" ) )
                        TempData ["Status"] = "is-link";
                    else if ( DadosChamado [0] ["situacao"].Equals( "EM ATENDIMENTO" ) )
                        TempData ["Status"] = "is-info";
                    else
                        TempData ["Status"] = "is-warning";

                    var Interacoes = VerificaDados.RetornaInteracoesChamadoAntigo( IdChamado );

                    TempData ["TotalInteracao"] = Interacoes.Count;
                    for ( var i = 0; i < Interacoes.Count; i++ )
                    {
                        TempData ["Usuario" + i] = Interacoes [i] ["nome"] + " " +
                                                  Convert.ToDateTime( Interacoes [i] ["datahorainteracao"] )
                                                      .ToString( "dd/MM/yyyy" );
                        TempData ["Interacao" + i] = Interacoes [i] ["textointeracao"];
                        TempData ["IdInteracao" + i] = Interacoes [i] ["id"];
                        if ( Interacoes [i] ["idarquivogoogle"] != null )
                        {
                            TempData ["TotalAnexos" + Interacoes [i] ["id"]] = 1;
                            for ( var j = 0; j < 1; j++ )
                                TempData ["ImagemAnexo" + Interacoes [i] ["id"]] =
                                    "https://drive.google.com/file/d/" + Interacoes [i] ["idarquivogoogle"] + "/edit";
                        }
                        else
                        {
                            TempData ["TotalAnexos" + Interacoes [i] ["id"]] = 0;
                        }
                    }

                    TempData ["Mensagem"] = Mensagem;
                    return View();
                }

                return RedirectToAction( "Login", "Login" );
            }
        }

        [HttpPost]
        [ValidateInput( false )]
        public async Task<ActionResult> CadastrarInteracao(FormCollection Dados, IEnumerable<HttpPostedFileBase> postedFiles)
        {
            var VerificaDados = new QueryMysqlWebdesk();
            var Logado = VerificaDados.UsuarioLogado();
            if ( Logado )
            {
                if ( Dados ["AcaoInteracao"].Equals( "Encerrar" ) )
                {
                    var Cookie = Request.Cookies.Get( "CookieFarm" );
                    var Login = Criptografa.Descriptografar( Cookie.Value );

                    var DadosUsuario = VerificaDados.RecuperaDadosUsuarios( Login );
                    var IdInteracao = VerificaDados.CadastrarInteracao( Dados ["IdSolicitacao"], Dados ["Descricao"],
                        DadosUsuario [0] ["id"], "N" );

                    VerificaDados.CadastrarInteracao( Dados ["IdSolicitacao"],
                        " Solicitação encerrada por " + DadosUsuario [0] ["nome"],
                        DadosUsuario [0] ["id"], "S" );

                    var lista = postedFiles.ToList();

                    for ( var i = 0; i < lista.Count; i++ )
                        if ( lista [i] != null )
                        {
                            var NomeArquivo = Path.GetFileName( lista [i].FileName );
                            byte [] fileData = null;
                            using ( var binaryReader = new BinaryReader( lista [i].InputStream ) )
                            {
                                fileData = binaryReader.ReadBytes( lista [i].ContentLength );
                            }

                            VerificaDados.InserirAnexo( IdInteracao, fileData, lista [i].ContentType, NomeArquivo );
                        }

                    VerificaDados.EncerrarSolicitacao( Dados ["IdSolicitacao"] );

                    var Envia = new EnviodeAlertas();
                    var CadastroAlerta = new QueryMysql();
                    var IdSolicitante = VerificaDados.RetornaIdSolicitantes( Dados ["IdSolicitacao"] );

                    var DadosOperador = VerificaDados.RetornaInformacoesNotificacao( IdSolicitante [0] ["idfuncionariocadastro"] );

                    if ( DadosOperador [0] ["notificacaoemail"].Equals( "Sim" ) )
                    {
                        CadastroAlerta.cadastrarAlert( IdSolicitante [0] ["idfuncionariocadastro"], "6", "Sua solicitação n°" + Dados ["IdSolicitacao"] + " foi encerrada." );
                        await Envia.EnviaEmail( DadosOperador [0] ["email"], "Sua solicitação n°" + Dados ["IdSolicitacao"] + " foi encerrada." );
                        if ( DadosOperador [0] ["idnotificacao"].ToString().Length > 0 )
                        {
                            Envia.CadastraAlerta( DadosOperador [0] ["idnotificacao"], "Sua solicitação n°" + Dados ["IdSolicitacao"] + " foi encerrada." );
                        }
                    }
                    else
                    {
                        CadastroAlerta.cadastrarAlert( IdSolicitante [0] ["idfuncionariocadastro"], "6", "Sua solicitação n°" + Dados ["IdSolicitacao"] + " foi encerrada." );
                        if ( DadosOperador [0] ["idnotificacao"].ToString().Length > 0 )
                        {
                            Envia.CadastraAlerta( DadosOperador [0] ["idnotificacao"], "Sua solicitação n°" + Dados ["IdSolicitacao"] + " foi encerrada." );
                        }
                    }

                    return RedirectToAction( "InteracaoChamado", "Webdesk",
                        new
                        {
                            IdChamado = Dados ["IdSolicitacao"],
                            Mensagem = "Solicitação encerrada com sucesso !",
                            TipoChamado = "Novo"
                        } );
                }

                if ( Dados ["AcaoInteracao"].Equals( "Repassar" ) )
                {
                    var Cookie = Request.Cookies.Get( "CookieFarm" );
                    var Login = Criptografa.Descriptografar( Cookie.Value );

                    var DadosUsuario = VerificaDados.RecuperaDadosUsuarios( Login );
                    var IdInteracao = VerificaDados.CadastrarInteracao( Dados ["IdSolicitacao"], Dados ["Descricao"],
                        DadosUsuario [0] ["id"], "N" );

                    var lista = postedFiles.ToList();

                    var NomeFuncionarioNovo =
                        VerificaDados.RetornaRepasseFuncionarioChamado( Dados ["IdFuncionarioResponsavel"] );

                    VerificaDados.CadastrarInteracao( Dados ["IdSolicitacao"],
                        " Solicitação encaminhada para " + NomeFuncionarioNovo [0] ["nome"] + " por " +
                        DadosUsuario [0] ["nome"],
                        DadosUsuario [0] ["id"], "S" );
                    for ( var i = 0; i < lista.Count; i++ )
                        if ( lista [i] != null )
                        {
                            var NomeArquivo = Path.GetFileName( lista [i].FileName );
                            byte [] fileData = null;
                            using ( var binaryReader = new BinaryReader( lista [i].InputStream ) )
                            {
                                fileData = binaryReader.ReadBytes( lista [i].ContentLength );
                            }

                            VerificaDados.InserirAnexo( IdInteracao, fileData, lista [i].ContentType, NomeArquivo );
                        }

                    #region Enviar para solicitante

                    var Envia = new EnviodeAlertas();
                    var CadastroAlerta = new QueryMysql();
                    var IdSolicitante = VerificaDados.RetornaIdSolicitantes( Dados ["IdSolicitacao"] );

                    var DadosOperador = VerificaDados.RetornaInformacoesNotificacao( IdSolicitante [0] ["idfuncionariocadastro"] );

                    if ( DadosOperador [0] ["notificacaoemail"].Equals( "Sim" ) )
                    {
                        CadastroAlerta.cadastrarAlert( IdSolicitante [0] ["idfuncionariocadastro"], "6", "Sua solicitação n°" + Dados ["IdSolicitacao"] + " foi encaminhada para " + NomeFuncionarioNovo [0] ["nome"] + " por " +
                                                                                                      DadosUsuario [0] ["nome"] );

                        await Envia.EnviaEmail( DadosOperador [0] ["email"], "Sua solicitação n°" + Dados ["IdSolicitacao"] + " foi encaminhada para " + NomeFuncionarioNovo [0] ["nome"] + " por " +
                                                                    DadosUsuario [0] ["nome"] );

                        if ( DadosOperador [0] ["idnotificacao"].ToString().Length > 0 )
                        {
                            Envia.CadastraAlerta( DadosOperador [0] ["idnotificacao"], "Sua solicitação n°" + Dados ["IdSolicitacao"] + " foi encaminhada para " + NomeFuncionarioNovo [0] ["nome"] + " por " +
                                                                                    DadosUsuario [0] ["nome"] );
                        }
                    }
                    else
                    {
                        CadastroAlerta.cadastrarAlert( IdSolicitante [0] ["idfuncionariocadastro"], "6", "Sua solicitação n°" + Dados ["IdSolicitacao"] + " foi encaminhada para " + NomeFuncionarioNovo [0] ["nome"] + " por " +
                                                                                                      DadosUsuario [0] ["nome"] );

                        if ( DadosOperador [0] ["idnotificacao"].ToString().Length > 0 )
                        {
                            Envia.CadastraAlerta( DadosOperador [0] ["idnotificacao"], "Sua solicitação n°" + Dados ["IdSolicitacao"] + " foi encaminhada para " + NomeFuncionarioNovo [0] ["nome"] + " por " +
                                                                                    DadosUsuario [0] ["nome"] );
                        }
                    }

                    #endregion

                    #region Envia para operador novo



                    var DadosOperadorNovo = VerificaDados.RetornaInformacoesNotificacao( Dados ["IdFuncionarioResponsavel"] );

                    if ( DadosOperador [0] ["notificacaoemail"].Equals( "Sim" ) )
                    {
                        CadastroAlerta.cadastrarAlert( Dados ["IdFuncionarioResponsavel"], "6", "A solicitação n°" + Dados ["IdSolicitacao"] + " foi encaminhada para você por " +
                                                                                                      DadosUsuario [0] ["nome"] );

                        await Envia.EnviaEmail( DadosOperadorNovo [0] ["email"], "Sua solicitação n°" + Dados ["IdSolicitacao"] + " foi encaminhada para você por" +
                                                                    DadosUsuario [0] ["nome"] );

                        if ( DadosOperadorNovo [0] ["idnotificacao"].ToString().Length > 0 )
                        {
                            Envia.CadastraAlerta( DadosOperadorNovo [0] ["idnotificacao"], "Sua solicitação n°" + Dados ["IdSolicitacao"] + "  foi encaminhada para você por " +
                                                                                    DadosUsuario [0] ["nome"] );
                        }
                    }
                    else
                    {
                        CadastroAlerta.cadastrarAlert( Dados ["IdFuncionarioResponsavel"], "6", "Sua solicitação n°" + Dados ["IdSolicitacao"] + " foi encaminhada para você por " +
                                                                                                      DadosUsuario [0] ["nome"] );

                        if ( DadosOperadorNovo [0] ["idnotificacao"].ToString().Length > 0 )
                        {
                            Envia.CadastraAlerta( Dados ["IdFuncionarioResponsavel"], "Sua solicitação n°" + Dados ["IdSolicitacao"] + "  foi encaminhada para você por " +
                                                                                    DadosUsuario [0] ["nome"] );
                        }
                    }
                    #endregion

                    VerificaDados.AlterarResponsavelSolicitacao( Dados ["IdSolicitacao"], Dados ["IdSetorResponsavel"],
                        Dados ["IdFuncionarioResponsavel"] );


                    return RedirectToAction( "InteracaoChamado", "Webdesk",
                        new
                        {
                            IdChamado = Dados ["IdSolicitacao"],
                            Mensagem = "Solicitação repassada com sucesso !",
                            TipoChamado = "Novo"
                        } );
                }

                if ( Dados ["AcaoInteracao"].Equals( "Categoria" ) )
                {
                    var Cookie = Request.Cookies.Get( "CookieFarm" );
                    var Login = Criptografa.Descriptografar( Cookie.Value );

                    var DadosUsuario = VerificaDados.RecuperaDadosUsuarios( Login );
                    var IdInteracao = VerificaDados.CadastrarInteracao( Dados ["IdSolicitacao"], Dados ["Descricao"],
                        DadosUsuario [0] ["id"], "N" );

                    var DadosChamado = VerificaDados.RetornaDadosChamado( Dados ["IdSolicitacao"] );
                    var NomeCategoriaNovo =
                        VerificaDados.RetornaRepasseCategoriaChamado( Dados ["IdCategoria"] );

                    VerificaDados.CadastrarInteracao( Dados ["IdSolicitacao"],
                        "Categoria da solicitação alterada de " + DadosChamado [0] ["titulo"] + " para " +
                        NomeCategoriaNovo [0] ["descricao"] + " por " + DadosUsuario [0] ["nome"],
                        DadosUsuario [0] ["id"], "S" );

                    var lista = postedFiles.ToList();

                    for ( var i = 0; i < lista.Count; i++ )
                        if ( lista [i] != null )
                        {
                            var NomeArquivo = Path.GetFileName( lista [i].FileName );
                            byte [] fileData = null;
                            using ( var binaryReader = new BinaryReader( lista [i].InputStream ) )
                            {
                                fileData = binaryReader.ReadBytes( lista [i].ContentLength );
                            }

                            VerificaDados.InserirAnexo( IdInteracao, fileData, lista [i].ContentType, NomeArquivo );
                        }

                    var Envia = new EnviodeAlertas();
                    var CadastroAlerta = new QueryMysql();
                    var IdSolicitante = VerificaDados.RetornaIdSolicitantes( Dados ["IdSolicitacao"] );

                    var DadosOperador = VerificaDados.RetornaInformacoesNotificacao( IdSolicitante [0] ["idfuncionariocadastro"] );

                    if ( DadosOperador [0] ["notificacaoemail"].Equals( "Sim" ) )
                    {
                        CadastroAlerta.cadastrarAlert( IdSolicitante [0] ["idfuncionariocadastro"], "6", "Sua solicitação n°" + Dados ["IdSolicitacao"] + " alterou de categoria." );
                        await Envia.EnviaEmail( DadosOperador [0] ["email"], "Sua solicitação n°" + Dados ["IdSolicitacao"] + " foi encerrada." );
                        if ( DadosOperador [0] ["idnotificacao"].ToString().Length > 0 )
                        {
                            Envia.CadastraAlerta( DadosOperador [0] ["idnotificacao"], "Sua solicitação n°" + Dados ["IdSolicitacao"] + " alterou de categoria." );
                        }
                    }
                    else
                    {
                        CadastroAlerta.cadastrarAlert( IdSolicitante [0] ["idfuncionariocadastro"], "6", "Sua solicitação n°" + Dados ["IdSolicitacao"] + " alterou de categoria." );
                        if ( DadosOperador [0] ["idnotificacao"].ToString().Length > 0 )
                        {
                            Envia.CadastraAlerta( DadosOperador [0] ["idnotificacao"], "Sua solicitação n°" + Dados ["IdSolicitacao"] + " alterou de categoria." );
                        }
                    }



                    VerificaDados.AlterarCategoriaSolicitacao( Dados ["IdSolicitacao"], Dados ["IdCategoria"] );
                    return RedirectToAction( "InteracaoChamado", "Webdesk",
                        new
                        {
                            IdChamado = Dados ["IdSolicitacao"],
                            Mensagem = "Categoria da solicitação alterada com sucesso !",
                            TipoChamado = "Novo"
                        } );
                }

                if ( Dados ["AcaoInteracao"].Equals( "Reabrir" ) )
                {
                    var Cookie = Request.Cookies.Get( "CookieFarm" );
                    var Login = Criptografa.Descriptografar( Cookie.Value );

                    var DadosUsuario = VerificaDados.RecuperaDadosUsuarios( Login );
                    var IdInteracao = VerificaDados.CadastrarInteracao( Dados ["IdSolicitacao"], Dados ["Descricao"],
                        DadosUsuario [0] ["id"], "N" );

                    VerificaDados.CadastrarInteracao( Dados ["IdSolicitacao"],
                        " Solicitação reaberta por " + DadosUsuario [0] ["nome"],
                        DadosUsuario [0] ["id"], "S" );
                    var lista = postedFiles.ToList();

                    for ( var i = 0; i < lista.Count; i++ )
                        if ( lista [i] != null )
                        {
                            var NomeArquivo = Path.GetFileName( lista [i].FileName );
                            byte [] fileData = null;
                            using ( var binaryReader = new BinaryReader( lista [i].InputStream ) )
                            {
                                fileData = binaryReader.ReadBytes( lista [i].ContentLength );
                            }

                            VerificaDados.InserirAnexo( IdInteracao, fileData, lista [i].ContentType, NomeArquivo );
                        }

                    VerificaDados.ReabrirSolicitacao( Dados ["IdSolicitacao"] );

                    var Envia = new EnviodeAlertas();
                    var CadastroAlerta = new QueryMysql();

                    var DadosOperador = VerificaDados.RetornaInformacoesNotificacao( DadosUsuario [0] ["id"] );

                    if ( DadosOperador [0] ["notificacaoemail"].Equals( "Sim" ) )
                    {
                        CadastroAlerta.cadastrarAlert( DadosUsuario [0] ["id"], "6", "A solicitação n°" + Dados ["IdSolicitacao"] + " foi reaberta." );
                        await Envia.EnviaEmail( DadosOperador [0] ["email"], "A solicitação n°" + Dados ["IdSolicitacao"] + " foi reaberta." );
                        if ( DadosOperador [0] ["idnotificacao"].ToString().Length > 0 )
                        {
                            Envia.CadastraAlerta( DadosOperador [0] ["idnotificacao"], "A solicitação n°" + Dados ["IdSolicitacao"] + " foi reaberta." );
                        }
                    }
                    else
                    {
                        CadastroAlerta.cadastrarAlert( DadosUsuario [0] ["id"], "6", "A solicitação n°" + Dados ["IdSolicitacao"] + " foi reaberta." );
                        if ( DadosOperador [0] ["idnotificacao"].ToString().Length > 0 )
                        {
                            Envia.CadastraAlerta( DadosOperador [0] ["idnotificacao"], "A solicitação n°" + Dados ["IdSolicitacao"] + " foi reaberta." );
                        }
                    }

                    return RedirectToAction( "InteracaoChamado", "Webdesk",
                        new
                        {
                            IdChamado = Dados ["IdSolicitacao"],
                            Mensagem = "Solicitação reaberta com sucesso !",
                            TipoChamado = "Novo"
                        } );
                }
                else
                {
                    var Cookie = Request.Cookies.Get( "CookieFarm" );
                    var Login = Criptografa.Descriptografar( Cookie.Value );

                    var DadosUsuario = VerificaDados.RecuperaDadosUsuarios( Login );
                    var IdInteracao = VerificaDados.CadastrarInteracao( Dados ["IdSolicitacao"], Dados ["Descricao"],
                        DadosUsuario [0] ["id"], "N" );

                    var lista = postedFiles.ToList();

                    for ( var i = 0; i < lista.Count; i++ )
                        if ( lista [i] != null )
                        {
                            var NomeArquivo = Path.GetFileName( lista [i].FileName );
                            byte [] fileData = null;
                            using ( var binaryReader = new BinaryReader( lista [i].InputStream ) )
                            {
                                fileData = binaryReader.ReadBytes( lista [i].ContentLength );
                            }

                            VerificaDados.InserirAnexo( IdInteracao, fileData, lista [i].ContentType, NomeArquivo );
                        }

                    var Envia = new EnviodeAlertas();
                    var CadastroAlerta = new QueryMysql();
                    var IdSolicitante = VerificaDados.RetornaIdSolicitantes( Dados ["IdSolicitacao"] );

                    var DadosOperador = VerificaDados.RetornaInformacoesNotificacao( IdSolicitante [0] ["idfuncionariocadastro"] );

                    if ( DadosOperador [0] ["notificacaoemail"].Equals( "Sim" ) )
                    {
                        CadastroAlerta.cadastrarAlert( IdSolicitante [0] ["idfuncionariocadastro"], "6", "Sua solicitação n°" + Dados ["IdSolicitacao"] + " teve interações." );
                        await Envia.EnviaEmail( DadosOperador [0] ["email"], "Sua solicitação n°" + Dados ["IdSolicitacao"] + " teve interações." );
                        if ( DadosOperador [0] ["idnotificacao"].ToString().Length > 0 )
                        {
                            Envia.CadastraAlerta( DadosOperador [0] ["idnotificacao"], "Sua solicitação n°" + Dados ["IdSolicitacao"] + " teve interações." );
                        }
                    }
                    else
                    {
                        CadastroAlerta.cadastrarAlert( IdSolicitante [0] ["idfuncionariocadastro"], "6", "Sua solicitação n°" + Dados ["IdSolicitacao"] + " teve interações." );
                        if ( DadosOperador [0] ["idnotificacao"].ToString().Length > 0 )
                        {
                            Envia.CadastraAlerta( DadosOperador [0] ["idnotificacao"], "Sua solicitação n°" + Dados ["IdSolicitacao"] + " teve interações." );
                        }
                    }


                    return RedirectToAction( "InteracaoChamado", "Webdesk",
                        new
                        {
                            IdChamado = Dados ["IdSolicitacao"],
                            Mensagem = "Interação adicionada com sucesso !",
                            TipoChamado = "Novo"
                        } );
                }
            }

            return RedirectToAction( "Login", "Login" );
        }

        [HttpPost]
        public async Task<ActionResult> IniciarAtendimento(string IdSolicitacao)
        {
            var VerificaDados = new QueryMysqlWebdesk();
            var Cookie = Request.Cookies.Get( "CookieFarm" );
            var Login = Criptografa.Descriptografar( Cookie.Value );
            var DadosUsuario = VerificaDados.RecuperaDadosUsuarios( Login );

            VerificaDados.CadastrarInteracao( IdSolicitacao, " Atendimento iniciado por " + DadosUsuario [0] ["nome"],
                DadosUsuario [0] ["id"], "S" );
            VerificaDados.IniciarAtendimentoSolicitacao( IdSolicitacao );

            var Envia = new EnviodeAlertas();
            var CadastroAlerta = new QueryMysql();
            var IdSolicitante = VerificaDados.RetornaIdSolicitantes( IdSolicitacao );

            var DadosOperador = VerificaDados.RetornaInformacoesNotificacao( IdSolicitante [0] ["idfuncionariocadastro"] );

            if ( DadosOperador [0] ["notificacaoemail"].Equals( "Sim" ) )
            {
                CadastroAlerta.cadastrarAlert( IdSolicitante [0] ["idfuncionariocadastro"], "6", "Sua solicitação n°" + IdSolicitacao + " teve o atendimento iniciado por " + DadosUsuario [0] ["nome"] );
                await Envia.EnviaEmail( DadosOperador [0] ["email"], "Sua solicitação n°" + IdSolicitacao + " teve o atendimento iniciado por " + DadosUsuario [0] ["nome"] );
                if ( DadosOperador [0] ["idnotificacao"].ToString().Length > 0 )
                {
                    Envia.CadastraAlerta( DadosOperador [0] ["idnotificacao"], "Sua solicitação n°" + IdSolicitacao + " teve o atendimento iniciado por " + DadosUsuario [0] ["nome"] );
                }
            }
            else
            {
                CadastroAlerta.cadastrarAlert( IdSolicitante [0] ["idfuncionariocadastro"], "6", "Sua solicitação n°" + IdSolicitacao + " teve o atendimento iniciado por " + DadosUsuario [0] ["nome"] );
                if ( DadosOperador [0] ["idnotificacao"].ToString().Length > 0 )
                {
                    Envia.CadastraAlerta( DadosOperador [0] ["idnotificacao"], "Sua solicitação n°" + IdSolicitacao + " teve o atendimento iniciado por " + DadosUsuario [0] ["nome"] );
                }
            }

            return RedirectToAction( "InteracaoChamado", "Webdesk",
                new { IdChamado = IdSolicitacao, Mensagem = "Interação adicionada com sucesso !", TipoChamado = "Novo" } );
        }

        public ActionResult AreaGestor(string Mensagem)
        {
            TempData ["Mensagem"] = Mensagem;
            var VerificaDados = new QueryMysql();
            var Logado = VerificaDados.UsuarioLogado();
            if ( Logado )
            {
                var Cookie = Request.Cookies.Get( "CookieFarm" );
                var Login = Criptografa.Descriptografar( Cookie.Value );


                var DadosUsuarioBanco = VerificaDados.RecuperaDadosUsuarios( Login );

                if ( VerificaDados.PermissaoCurriculos( DadosUsuarioBanco [0] ["login"] ) )
                    TempData ["PermissaoCurriculo"] =
                        " ";
                else
                    TempData ["PermissaoCurriculo"] = "display: none";

                if ( VerificaDados.PermissaoTesouraria( DadosUsuarioBanco [0] ["login"] ) )
                    TempData ["PermissaoTesouraria"] =
                        " ";
                else
                    TempData ["PermissaoTesouraria"] = "display: none";

                if ( DadosUsuarioBanco [0] ["gestor"].Equals( "S" ) )
                {
                    TempData ["PermissaoGestor"] = "N";
                    TempData ["AreaGestor"] = "S";
                }
                else
                {
                    TempData ["PermissaoGestor"] = "N";
                    TempData ["AreaGestor"] = "N";
                }

                if ( DadosUsuarioBanco [0] ["foto"] == null )
                    TempData ["ImagemPerfil"] = "http://bulma.io/images/placeholders/128x128.png";
                else
                    TempData ["ImagemPerfil"] = DadosUsuarioBanco [0] ["foto"];

                TempData ["NomeLateral"] = DadosUsuarioBanco [0] ["login"];

                return View( "AreaGestor" );
            }

            return RedirectToAction( "Login", "Login" );
        }

        public ActionResult FormularioSetor()
        {
            var VerificaDados = new QueryMysqlWebdesk();
            var Logado = VerificaDados.UsuarioLogado();
            if ( Logado )
            {
                var Cookie = Request.Cookies.Get( "CookieFarm" );
                var Login = Criptografa.Descriptografar( Cookie.Value );

                var DadosUsuario = VerificaDados.RecuperaDadosUsuarios( Login );

                var DadosFormularios = VerificaDados.RetornaFormulariosSetor( DadosUsuario [0] ["idsetor"] );

                TempData ["TotalFormularios"] = DadosFormularios.Count();

                for ( int i = 0; i < DadosFormularios.Count; i++ )
                {
                    TempData ["CategoriaFormulario" + i] = DadosFormularios [i] ["descricao"];
                    TempData ["CamposFormulario" + i] = DadosFormularios [i] ["campo"];
                    TempData ["CamposFormularioObrigatorio" + i] = DadosFormularios [i] ["campoobrigatorio"];
                }

                return PartialView( "FormularioSetor" );
            }

            return RedirectToAction( "Login", "Login" );
        }

        public ActionResult CadastrarFormulario()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult RetornaFormulario(string IdCategoria)
        {
            var VerificaDados = new QueryMysqlWebdesk();
            var Logado = VerificaDados.UsuarioLogado();
            if ( Logado )
            {
                var Formulario = VerificaDados.RetornaFormularioCategoria( IdCategoria );
                if ( Formulario.Count > 0 )
                {
                    var count = 0;
                    var ArrayCombo = new Dictionary<string, string>();
                    var ArrayObrigatoriedadeCombo = new Dictionary<string, string>();


                    foreach ( var Campo in Formulario )
                    {
                        if ( Campo ["campoobrigatorio"].Equals( "S" ) )
                        {
                            TempData ["Obrigatorio" + count] = "required";
                        }
                        else
                        {
                            TempData ["Obrigatorio" + count] = "";
                        }
                        if ( Campo ["combo"].Equals( "S" ) )
                        {
                            if ( ArrayCombo.ContainsKey( Campo ["nomecombo"] ) )
                            {

                                var CamposCombo = ArrayCombo [Campo ["nomecombo"]];
                                CamposCombo = CamposCombo + ";" + Campo ["campo"];
                                ArrayCombo [Campo ["nomecombo"]] = CamposCombo;

                            }
                            else
                            {
                                if ( Campo ["campoobrigatorio"].Equals( "S" ) )
                                {
                                    TempData["Obnrigatorio"+Campo ["nomecombo"]] = "required" ;
                                }
                                else
                                {
                                    TempData ["Obnrigatorio" + Campo ["nomecombo"]] = "required";
                                }
                                ArrayCombo.Add( Campo ["nomecombo"], Campo ["campo"] );

                            }

                        }
                        else
                        {
                            TempData ["NomeCampo" + count] = Campo ["campo"];
                            count++;
                        }


                    }
                    TempData ["TotalCampos"] = count;
                    TempData ["Combos"] = ArrayCombo;
                  

                    return PartialView( "FormularioAberturaChamado" );
                }
                else
                {
                    return null;
                }


            }
            return RedirectToAction( "Login", "Login" );
        }
    }
}