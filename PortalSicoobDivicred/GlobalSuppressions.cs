using System.Diagnostics.CodeAnalysis;

[assembly:
    SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Scope = "type",
        Target = "Port.Repositorios.Conexao")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2100:Revisar as consultas SQL em busca de vulnerabilidades de segurança",
        Scope = "member", Target = "Port.Repositorios.Conexao.#ComandoArquivo(System.String)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2100:Revisar as consultas SQL em busca de vulnerabilidades de segurança",
        Scope = "member", Target = "Port.Repositorios.Conexao.#ComandoArquivoWebDesk(System.String)")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2100:Revisar as consultas SQL em busca de vulnerabilidades de segurança",
        Scope = "member", Target = "Port.Repositorios.Conexao.#CriarComando(System.String)")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Scope = "member",
        Target = "Port.Repositorios.Conexao.#Dispose()")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2100:Revisar as consultas SQL em busca de vulnerabilidades de segurança",
        Scope = "member", Target = "Port.Repositorios.Conexao.#ExecutaComandoArquivo(System.String,System.Byte[])")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Scope = "type",
        Target = "PortalSicoobDivicred.Aplicacao.QueryFirebird")]
[assembly:
    SuppressMessage("Microsoft.Security", "CA2100:Revisar as consultas SQL em busca de vulnerabilidades de segurança",
        Scope = "member", Target = "PortalSicoobDivicred.Aplicacao.QueryFirebird.#CriarComandoSQL(System.String)")]
[assembly:
    SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "con", Scope = "member",
        Target = "PortalSicoobDivicred.Aplicacao.QueryFirebird.#Dispose()")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Scope = "member",
        Target = "PortalSicoobDivicred.Aplicacao.QueryFirebird.#Dispose()")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Scope = "type",
        Target = "PortalSicoobDivicred.Aplicacao.QueryMysql")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Scope = "type",
        Target = "PortalSicoobDivicred.Aplicacao.QueryMysqlCurriculo")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Scope = "type",
        Target = "PortalSicoobDivicred.Aplicacao.QueryMysqlRh")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Scope = "type",
        Target = "PortalSicoobDivicred.Aplicacao.QueryMysqlWebdesk")]
[assembly:
    SuppressMessage("Microsoft.Usage", "CA2202:Não descartar objetos várias vezes", Scope = "member",
        Target = "PortalSicoobDivicred.Controllers.LoginController.#EsqueciMinhaSenha(System.Web.Mvc.FormCollection)")]
[assembly:
    SuppressMessage("Microsoft.Usage", "CA2200:RethrowToPreserveStackDetails", Scope = "member",
        Target = "PortalSicoobDivicred.Models.Criptografa.#Criptografar(System.String)")]
[assembly:
    SuppressMessage("Microsoft.Usage", "CA2200:RethrowToPreserveStackDetails", Scope = "member",
        Target = "PortalSicoobDivicred.Models.Criptografa.#Descriptografar(System.String)")]
// Este arquivo é usado pela Análise de Código para manter os atributos de SuppressMessage 
// aplicados a este projeto.
// As supressões no nível do projeto não têm destino ou recebem um destino específico para 
// com escopo para um namespace, tipo, membro etc.
//
// Para adicionar uma supressão a este arquivo, clique com o botão direito do mouse na mensagem dos resultados da Análise de Código 
//, aponte para "Suprimir Mensagem" e clique em 
// "No Arquivo de Supressão".
// Você não precisa adicionar supressões a este arquivo manualmente.