using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OFXSharp;
using PortalSicoobDivicred.Aplicacao;
using PortalSicoobDivicred.Models;

namespace PortalSicoobDivicred.Controllers
{
    public class TesourariaController : Controller
    {
        private DateTime dataescolhida;

        private DataSet mDataSet;

        // GET: Tesouraria
        public ActionResult Tesouraria(string MensagemValidacao, string Erro)
        {
            TempData["MensagemValidacao"] = MensagemValidacao;
            TempData["Erro"] = Erro;
            var insereDados = new QueryMysql();
            var Logado = insereDados.UsuarioLogado();
            if (Logado)
            {
                var cookie = Request.Cookies.Get("CookieFarm");
                if (cookie != null)
                {
                    var login = Criptografa.Descriptografar(cookie.Value);
                    var dadosUsuarioBanco = insereDados.RecuperaDadosUsuarios(login);
                    var validacoes = new ValidacoesIniciais();

                    validacoes.AlertasUsuario( this, dadosUsuarioBanco[0]["id"] );
                    validacoes.Permissoes( this, dadosUsuarioBanco );
                    validacoes.DadosNavBar( this, dadosUsuarioBanco );
                }

                return View("Tesouraria");
            }

            return RedirectToAction("Login", "Login");
        }


        public ActionResult ProcessaArquivos()
        {
            return PartialView("ViewArquivos");
        }

        [HttpPost]
        public ActionResult ProcessaArquivos(IEnumerable<HttpPostedFileBase> file, Tesouraria Dados,
            FormCollection receberForm)
        {
            var consultaValorNR = new QueryMysqlTesouraria();

            var valorNR = Convert.ToDouble(consultaValorNR.ConsultaValorNr());

            var valorJudicial = 0.0;
            var valor6192 = 0.0;
            var valorNr1 = Convert.ToDouble(receberForm["valorCampoNr1"].Replace('.', ','));
            var valorNr2 = Convert.ToDouble(receberForm["valorCampoNr2"].Replace('.', ','));
            var valorNr3 = Convert.ToDouble(receberForm["valorCampoNr3"].Replace('.', ','));
            //var valorNr4 = Convert.ToDouble(receberForm["valorCampoNr4"].ToString().Replace('.', ','));
            TempData["valorTotalNr"] = Math.Round(valorNr1 + valorNr2 + valorNr3 + valorNR, 2);

            if (receberForm["valorJudicial"].Length == 0)
                valorJudicial = 0;
            else
                valorJudicial = Convert.ToDouble(receberForm["valorJudicial"].Replace('.', ','));

            if (receberForm["valor6192"].Length == 0)
                valor6192 = 0;
            else
                valor6192 = Convert.ToDouble(receberForm["valor6192"].Replace('.', ','));

            TempData["data"] = receberForm["data"];

            var insereValorCampos = new QueryMysqlTesouraria();
            insereValorCampos.InsereValoresCampos(valorJudicial.ToString(), valor6192.ToString(), valorNr1.ToString(),
                valorNr2.ToString(), valorNr3.ToString(), Convert.ToDateTime(TempData["data"]), DateTime.Now);

            var Maior = "";
            var historico = "";
            double arqext1 = 0;
            double arqext2 = 0;
            double arqext3 = 0;
            double arqext4 = 0;
            double arqext5 = 0;
            double arqext6 = 0;
            double arqext7 = 0;
            double arqext8 = 0;
            double arqext9 = 0;
            double arqext10 = 0;
            double arqext11 = 0;
            double arqext12 = 0;
            var
                auxNR = 0; //quando 1, o usuário selecionou a opção de não usar o arquivo de op de caixa. Assim apenas o valor do NRDEV aparece no campo de extrato.


            var parser = new OFXDocumentParser();


            mDataSet = new DataSet();
            var conectaPlanilha = new ConexaoExcel();
            var inicio = new ManipularPlanilha();
            var arquivos = file.ToList();
            var nomeArquivo = "";
            var nomeArquivo1 = "";
            var caminho = "";
            var caminho1 = "";
            var dataValida = 0;

            var inicioPlanilha1 = 0;
            var inicioPlanilha2 = 0;
            var inicioPlanilha3 = 0;
            var inicioPlanilha4 = 0;

            var aux = 0;

            if (arquivos.Count > 3)
            {
                var consultaFeriado = new QueryMysqlTesouraria();


                var output = new DataSet();
                var dados1 = new Dictionary<string, double>();
                var dados2 = new Dictionary<string, double>();
                var dados3 = new Dictionary<string, double>();
                var dados4 = new Dictionary<string, double>();
                var dados5 = new Dictionary<string, double>();
                var i = 0;
                var dataSelecionadaValidar = new DateTime();
                var dataExtratoDiaValidar = new DateTime();
                var uploadrealizado = 0;

                var caminho01 = "";
                var caminho02 = "";
                var caminho03 = "";
                var caminho04 = "";
                var caminho05 = "";
                var nomeArquivo01 = "";
                var nomeArquivo02 = "";
                var nomeArquivo03 = "";
                var nomeArquivo04 = "";
                var nomeArquivo05 = "";
                var aux01 = 0;
                var aux02 = 0;
                var aux03 = 0;
                var aux04 = 0;
                var aux05 = 0;

                var inicioMes = 0;

                var dataSelecionada = new DateTime();
                var dataExtratoDia = new DateTime();


                while (i < arquivos.Count)
                {
                    if (i == 4)
                    {
                        if (arquivos[i] == null)
                        {
                            auxNR = 1;
                            i = 5;
                        }
                        else
                        {
                            i = 4;
                        }
                    }

                    nomeArquivo = Path.GetFileName(arquivos[i].FileName);
                    caminho = Path.Combine(Server.MapPath("~/Uploads/"), nomeArquivo);
                    if (i == 0) arquivos[i].SaveAs(caminho);


                    if (uploadrealizado == 0)
                    {
                        nomeArquivo01 = Path.GetFileName(arquivos[1].FileName);
                        caminho01 = Path.Combine(Server.MapPath("~/Uploads/"),
                            DateTime.Now.Hour + DateTime.Now.Minute.ToString() + nomeArquivo01);
                        arquivos[1].SaveAs(caminho01);

                        nomeArquivo02 = Path.GetFileName(arquivos[2].FileName);
                        caminho02 = Path.Combine(Server.MapPath("~/Uploads/"),
                            DateTime.Now.Hour + DateTime.Now.Minute.ToString() + nomeArquivo02);
                        arquivos[2].SaveAs(caminho02);

                        nomeArquivo03 = Path.GetFileName(arquivos[3].FileName);
                        caminho03 = Path.Combine(Server.MapPath("~/Uploads/"),
                            DateTime.Now.Hour + DateTime.Now.Minute.ToString() + nomeArquivo03);
                        arquivos[3].SaveAs(caminho03);

                        nomeArquivo04 = Path.GetFileName(arquivos[4].FileName);
                        caminho04 = Path.Combine(Server.MapPath("~/Uploads/"),
                            DateTime.Now.Hour + DateTime.Now.Minute.ToString() + nomeArquivo04);
                        arquivos[4].SaveAs(caminho04);

                        nomeArquivo05 = Path.GetFileName(arquivos[5].FileName);
                        caminho05 = Path.Combine(Server.MapPath("~/Uploads/"),
                            DateTime.Now.Hour + DateTime.Now.Minute.ToString() + nomeArquivo05);
                        arquivos[5].SaveAs(caminho05);
                        uploadrealizado = 1;
                    }


                    if (i != 0)
                    {
                        if (i == 1 && aux01 == 0)
                        {
                            output.Tables.Add(conectaPlanilha.ImportarExcel(caminho01, i.ToString()));
                            aux01 = 1;
                        }
                        else if (i == 2 && aux02 == 0)
                        {
                            output.Tables.Add(conectaPlanilha.ImportarExcel(caminho02, i.ToString()));
                            aux02 = 1;
                        }
                        else if (i == 3 && aux03 == 0)
                        {
                            output.Tables.Add(conectaPlanilha.ImportarExcel(caminho03, i.ToString()));
                            aux03 = 1;
                        }
                        else if (i == 4 && aux04 == 0)
                        {
                            output.Tables.Add(conectaPlanilha.ImportarExcel(caminho04, i.ToString()));
                            aux04 = 1;
                        }
                        else if (i == 5 && aux05 == 0)
                        {
                            output.Tables.Add(conectaPlanilha.ImportarExcel(caminho05, i.ToString()));
                            aux05 = 1;
                        }

                        //dataSelecionada = DateTime.Now;
                        //dataExtratoDia = DateTime.Now;
                    }
                    else
                    {
                        var ofxdocument = parser.Import(new FileStream(caminho, FileMode.Open));
                        var transacoes = ofxdocument.Transactions;
                        dataSelecionada = Convert.ToDateTime(TempData["data"]);
                        dataExtratoDia = transacoes[0].Date;

                        dataSelecionadaValidar = Convert.ToDateTime(TempData["data"]);
                        dataExtratoDiaValidar = transacoes[0].Date;
                    }

                    var auxdias = 0;
                    for (var m = 1; m <= 7; m++)
                        if (consultaFeriado.VerificaFeriadoDivinopolis(dataSelecionada.Date.AddDays(-m)) == 0 &&
                            dataSelecionada.Date.AddDays(-m).DayOfWeek != DayOfWeek.Sunday &&
                            dataSelecionada.Date.AddDays(-m).DayOfWeek != DayOfWeek.Saturday)
                        {
                            auxdias = m;
                            break;
                        }

                    var teste = dataSelecionada.AddDays(-auxdias);
                    var mes1 = dataSelecionada.ToString().Substring(3, 2);
                    var mes2 = teste.ToString().Substring(3, 2);
                    //dataExtratoDia.Date.AddDays(auxdias)

                    if (mes1 != mes2) inicioMes = 1;


                    if (dataValida == 0 && i != 0)
                    {
                        int k;
                        var arquivosAUX = file.ToList();
                        for (k = 0; k < arquivosAUX.Count; k++)
                        {
                            nomeArquivo1 = Path.GetFileName(arquivosAUX[k].FileName);

                            if (k == 1)
                            {
                                var posicao = "F4";
                                var valeData = inicio.validaDataRelatorio(posicao, k, caminho01);

                                if (dataSelecionadaValidar == Convert.ToDateTime(valeData) || inicioMes == 1)
                                {
                                }
                                else
                                {
                                    dataValida = 1;
                                    return RedirectToAction("Tesouraria",
                                        new {Erro = "Data Invalida do Relatório Lançamento"});
                                }
                            }

                            if (k == 2)
                            {
                                var posicao = "F3";
                                var valeData = inicio.validaDataRelatorio(posicao, k, caminho02);

                                if (valeData == "0")
                                {
                                    dataValida = 1;
                                    return RedirectToAction("Tesouraria",
                                        new {Erro = "Período do Relatório Enviadas e Recebidas são diferentes"});
                                }

                                if (dataSelecionadaValidar == Convert.ToDateTime(valeData) || inicioMes == 1)
                                {
                                }
                                else
                                {
                                    dataValida = 1;
                                    return RedirectToAction("Tesouraria",
                                        new {Erro = "Data Invalida do Relatório Enviadas e Recebidas"});
                                }
                            }

                            if (k == 3)
                            {
                                var posicao = "F4";
                                var valeData = inicio.validaDataRelatorio(posicao, k, caminho03);

                                if (dataSelecionadaValidar == Convert.ToDateTime(valeData) || inicioMes == 1)
                                {
                                }
                                else
                                {
                                    dataValida = 1;
                                    return RedirectToAction("Tesouraria",
                                        new {Erro = "Data Invalida no Relatório de Devolução de Cheques"});
                                }
                            }

                            if (k == 4)
                            {
                                var posicao = "F15";
                                var valeData = inicio.validaDataRelatorio(posicao, k, caminho04);
                                if (valeData == "0")
                                {
                                    dataValida = 1;
                                    return RedirectToAction("Tesouraria",
                                        new
                                        {
                                            Erro =
                                                "Período do Relatório Operação de Caixa(Dia Anterior) são diferentes!!!!"
                                        });
                                }

                                if (dataExtratoDiaValidar == Convert.ToDateTime(valeData) || inicioMes == 1)
                                {
                                }
                                else
                                {
                                    dataValida = 1;
                                    return RedirectToAction("Tesouraria",
                                        new {Erro = "Data Invalida do Relatório Operação de Caixa(Dia Anterior)"});
                                }
                            }
                            //Lembrar de deletar o que for para o banco de dados

                            if (k == 5)
                            {
                                var posicao = "F11";
                                var valeData = inicio.validaDataRelatorio(posicao, k, caminho05);
                                if (valeData == "0")
                                {
                                    dataValida = 1;
                                    return RedirectToAction("Tesouraria",
                                        new
                                        {
                                            Erro =
                                                "Período do Relatório Cheques Devolvidos(Dia Anterior) são diferentes"
                                        });
                                }

                                if (dataExtratoDiaValidar == Convert.ToDateTime(valeData) || inicioMes == 1)
                                {
                                }
                                else
                                {
                                    dataValida = 1;
                                    return RedirectToAction("Tesouraria",
                                        new {Erro = "Data Invalida do Relatório Cheques Devolvidos(Dia Anterior)"});
                                }
                            }

                            //--
                        }
                    }


                    if (dataValida == 0)
                    {
                        //fim teste
                        if (dataExtratoDia == dataSelecionada ||
                            dataSelecionada == dataExtratoDia.Date.AddDays(auxdias) || inicioMes == 1)
                        {
                            var insereConferencia1 = new QueryMysqlTesouraria();
                            switch (i)
                            {
                                case 0:
                                    var ofxdocument = parser.Import(new FileStream(caminho, FileMode.Open));
                                    var transacoes = ofxdocument.Transactions;

                                    for (var j = 0; j < transacoes.Count; j++)
                                    {
                                        historico = transacoes[j].Memo;
                                        var dataExtrato = transacoes[j].Date;

                                        if (dataExtrato == dataSelecionada)
                                        {
                                            if (historico.Contains("SRS ELE") || historico.Contains("SRI ELE") ||
                                                historico.Contains("SR CHQ ROUBADO SUP ELETR"))
                                            {
                                                //var teste = Math.Abs(Convert.ToDouble(transacoes[j].Amount));
                                                arqext1 = arqext1 + Math.Abs(Convert.ToDouble(transacoes[j].Amount));
                                                arqext1 = Math.Round(arqext1, 2);
                                                TempData["Extrato-3/4/5"] = arqext1;
                                            }
                                            else if (historico.Contains("NICA NOITE") ||
                                                     historico.Contains("SR DEV CONVENCIONAL DIA"))
                                            {
                                                arqext2 = arqext2 + Math.Abs(Convert.ToDouble(transacoes[j].Amount));
                                                arqext2 = Math.Round(arqext2, 2);
                                                TempData["Extrato-6/192"] = arqext2;
                                            }
                                            else if (historico.Contains("O TED - SPB") ||
                                                     historico.Contains("BITO TED CONTA SAL") ||
                                                     historico.Contains("O TED BANCOOB - SPB") ||
                                                     historico.Contains("O TED BANCOOB MESMA TITULAR"))
                                            {
                                                arqext3 = arqext3 + Math.Abs(Convert.ToDouble(transacoes[j].Amount));
                                                arqext3 = Math.Round(arqext3, 2);
                                                TempData["Extrato-5472/5473/5474/232/233/234/235"] = arqext3;
                                            }
                                            else if (historico.Contains("DITO TED RECEBIDA -SPB") ||
                                                     historico.Contains("DITO TED BANCOOB RECEBIDA -SPB") ||
                                                     historico.Contains("DITO DEC-CONTA SALARIO") ||
                                                     historico.Contains("SUA REMESSA TEC - ELETR") ||
                                                     historico.Contains("DITO TED CONTA SAL"))
                                            {
                                                arqext4 = arqext4 + Math.Abs(Convert.ToDouble(transacoes[j].Amount));
                                                arqext4 = Math.Round(arqext4, 2);
                                                TempData["Extrato-821/2/7286/7336/847"] = arqext4;
                                            }
                                            else if (historico.Contains("O TED EMITIDA-SPB"))
                                            {
                                                arqext12 = arqext12 + Math.Abs(Convert.ToDouble(transacoes[j].Amount));
                                                arqext12 = Math.Round(arqext12, 2);
                                            }
                                            else if (historico.Contains("DEV.TIT.PG.AUTOATENDIMENTO"))
                                            {
                                                arqext5 = arqext5 + Math.Abs(Convert.ToDouble(transacoes[j].Amount));
                                                arqext5 = Math.Round(arqext5, 2);
                                                TempData["Extrato-7027"] = arqext5;
                                            }
                                            else if (historico.Contains("NRI ELETR") ||
                                                     historico.Contains("NRS ELETR") ||
                                                     historico.Contains("NR CHQ ROUBADO SUP ELETR"))
                                            {
                                                arqext6 = arqext6 + Math.Abs(Convert.ToDouble(transacoes[j].Amount));
                                                TempData["ExtratoCampos"] = Math.Round(arqext6, 2);

                                                Maior = inicio.Diferenciar(arqext6,
                                                    Math.Round(Convert.ToDouble(TempData["valorTotalNr"]), 2));
                                                TempData["Diferenca7"] = Maior.Split(';')[1];
                                                TempData["DiferencaTexto7"] = Maior.Split(';')[0];
                                            }
                                            else if (historico.Contains("A VLB-INF"))
                                            {
                                                arqext7 = arqext7 + Math.Abs(Convert.ToDouble(transacoes[j].Amount));
                                                arqext7 = Math.Round(arqext7, 2);
                                            }
                                            else if (historico.Contains("BITO EMP. - REPES COTAS PARTES CDI") ||
                                                     historico.Contains("O COTAS PARTES"))
                                            {
                                                arqext8 = arqext8 + Math.Abs(Convert.ToDouble(transacoes[j].Amount));
                                                arqext8 = Math.Round(arqext8, 2);
                                                TempData["Extrato-5761"] = arqext8;
                                            }
                                            else if (historico.Contains("SR DOC ELETR"))
                                            {
                                                arqext9 = arqext9 + Math.Abs(Convert.ToDouble(transacoes[j].Amount));
                                                arqext9 = Math.Round(arqext9, 2);
                                                TempData["Extrato-820"] = arqext9;
                                            }

                                            else if (historico.Contains("NR DOC ELETR"))
                                            {
                                                arqext10 = arqext10 + Math.Abs(Convert.ToDouble(transacoes[j].Amount));
                                                arqext10 = Math.Round(arqext10, 2);
                                                TempData["Extrato-257/258"] = arqext10;
                                            }
                                        }
                                        else if (historico.Contains("NR DEV ELETR"))
                                        {
                                            arqext11 = arqext11 + Math.Abs(Convert.ToDouble(transacoes[j].Amount));
                                            arqext11 = Math.Round(arqext11, 2);
                                            TempData["Extrato-NRDEVELETRONICA"] = arqext11;
                                        }
                                    }

                                    break;


                                case 1:

                                    var inicioPlanilha0 = 0;
                                    inicioPlanilha0 = inicio.InicioPlanilha(caminho01, i.ToString());
                                    dados1 = inicio.Calculo1(caminho01, i.ToString(), inicioPlanilha0);

                                    TempData["3/4/5"] = Convert.ToDouble(dados1["3/4/5"]);
                                    Maior = inicio.Diferenciar(Math.Round(arqext1, 2),
                                        Math.Round(Convert.ToDouble(dados1["3/4/5"]), 2));
                                    TempData["Diferenca1"] = Maior.Split(';')[1];
                                    TempData["DiferencaTexto1"] = Maior.Split(';')[0];
                                    insereConferencia1.InsereConferencia(Dados.Data.ToString("yyyy/MM/dd"),
                                        "1 - Cheques 4030", arqext1.ToString(), TempData["3/4/5"].ToString(),
                                        TempData["Diferenca1"].ToString());

                                    TempData["5761"] = Convert.ToDouble(dados1["5761"]);
                                    Maior = inicio.Diferenciar(arqext8,
                                        Math.Round(Convert.ToDouble(dados1["5761"]), 2));
                                    TempData["Diferenca8"] = Maior.Split(';')[1];
                                    TempData["DiferencaTexto8"] = Maior.Split(';')[0];
                                    insereConferencia1.InsereConferencia(Dados.Data.ToString("yyyy/MM/dd"),
                                        "7 - Cotas Partes", arqext8.ToString(), TempData["5761"].ToString(),
                                        TempData["Diferenca8"].ToString());

                                    TempData["820"] = Convert.ToDouble(dados1["820"]);
                                    Maior = inicio.Diferenciar(arqext9, Math.Round(Convert.ToDouble(dados1["820"]), 2));
                                    TempData["Diferenca9"] = Maior.Split(';')[1];
                                    TempData["DiferencaTexto9"] = Maior.Split(';')[0];
                                    insereConferencia1.InsereConferencia(Dados.Data.ToString("yyyy/MM/dd"),
                                        "8 - Docs Recebidos", arqext9.ToString(), TempData["820"].ToString(),
                                        TempData["Diferenca9"].ToString());

                                    TempData["257/258"] = Convert.ToDouble(dados1["257/258"]);
                                    Maior = inicio.Diferenciar(arqext10,
                                        Math.Round(Convert.ToDouble(dados1["257/258"]), 2));
                                    TempData["Diferenca10"] = Maior.Split(';')[1];
                                    TempData["DiferencaTexto10"] = Maior.Split(';')[0];
                                    insereConferencia1.InsereConferencia(Dados.Data.ToString("yyyy/MM/dd"),
                                        "9 - Docs Enviados", arqext10.ToString(), TempData["257/258"].ToString(),
                                        TempData["Diferenca10"].ToString());

                                    TempData["5472/5473/5474/232/233/234/235"] =
                                        Convert.ToDouble(dados1["5472/5473/5474/232/233/234/235"]) +
                                        Convert.ToDouble(valorJudicial);
                                    Maior = inicio.Diferenciar(arqext3,
                                        Math.Round(
                                            Convert.ToDouble(dados1["5472/5473/5474/232/233/234/235"]) +
                                            Convert.ToDouble(valorJudicial), 2));

                                    TempData["821/2/7286/7336/847"] =
                                        Convert.ToDouble(dados1["821/2/7286/7336/847"]) + Convert.ToDouble(arqext12);
                                    Maior = inicio.Diferenciar(arqext4,
                                        Math.Round(
                                            Convert.ToDouble(dados1["821/2/7286/7336/847"]) +
                                            Convert.ToDouble(arqext12), 2));
                                    TempData["Diferenca4"] = Maior.Split(';')[1];
                                    TempData["DiferencaTexto4"] = Maior.Split(';')[0];
                                    insereConferencia1.InsereConferencia(Dados.Data.ToString("yyyy/MM/dd"),
                                        "3 - TED´s Recebidas", arqext4.ToString(),
                                        TempData["821/2/7286/7336/847"].ToString(), TempData["Diferenca4"].ToString());
                                    TempData["6/192"] = Convert.ToDouble(dados1["6/192"]) - Convert.ToDouble(valor6192);

                                    TempData["7027"] = Convert.ToDouble(dados1["7027"]);
                                    Maior = inicio.Diferenciar(arqext5,
                                        Math.Round(Convert.ToDouble(dados1["7027"]), 2));
                                    TempData["Diferenca6"] = Maior.Split(';')[1];
                                    TempData["DiferencaTexto6"] = Maior.Split(';')[0];
                                    insereConferencia1.InsereConferencia(Dados.Data.ToString("yyyy/MM/dd"),
                                        "5 - Boletos Rejeitados", arqext5.ToString(), TempData["7027"].ToString(),
                                        TempData["Diferenca6"].ToString());

                                    insereConferencia1.InsereConferencia(Dados.Data.ToString("yyyy/MM/dd"),
                                        "6 - Cheques Compensados", arqext6.ToString(), TempData["valorTotalNr"].ToString(),
                                        TempData["Diferenca7"].ToString());

                                    break;

                                case 2:

                                    inicioPlanilha1 = inicio.InicioPlanilha(caminho02, i.ToString());
                                    dados2 = inicio.Calculo1(caminho02, i.ToString(), inicioPlanilha1);
                                    TempData["5472/5473/5474/232/233/234/235-FINAL"] = Math.Round(
                                        Convert.ToDouble(TempData["5472/5473/5474/232/233/234/235"]) +
                                        Convert.ToDouble(dados2["Arquivo2"]), 2);
                                    TempData["5472/5473/5474/232/233/234/235-FINALB"] =
                                        Math.Round(
                                            Convert.ToDouble(TempData["5472/5473/5474/232/233/234/235"]) +
                                            Convert.ToDouble(dados2["Arquivo2"]), 2);
                                    Maior = inicio.Diferenciar(arqext3,
                                        Math.Round(Convert.ToDouble(TempData["5472/5473/5474/232/233/234/235-FINALB"]),
                                            2));
                                    TempData["Diferenca3"] = Maior.Split(';')[1];
                                    TempData["DiferencaTexto3"] = Maior.Split(';')[0];

                                    insereConferencia1.InsereConferencia(Dados.Data.ToString("yyyy/MM/dd"),
                                        "2 - TED´s Enviadas", arqext3.ToString(),
                                        TempData["5472/5473/5474/232/233/234/235-FINAL"].ToString(),
                                        TempData["Diferenca3"].ToString());
                                    break;

                                case 3:


                                    inicioPlanilha2 = inicio.InicioPlanilha(caminho03, i.ToString());
                                    dados3 = inicio.Calculo1(caminho03, i.ToString(), inicioPlanilha2);

                                    Maior = inicio.Diferenciar(arqext2,
                                        Math.Round(Convert.ToDouble(TempData["6/192"]), 2));

                                    TempData["Diferenca5"] =
                                        Convert.ToDouble(Maior.Split(';')[1]) - Convert.ToDouble(dados3["Arquivo3"]);
                                    TempData["DiferencaTexto5"] = Maior.Split(';')[0];
                                    TempData["6/192-FINAL"] = arqext2 + Convert.ToDouble(TempData["Diferenca5"]);
                                    TempData["6/192-FINALB"] =
                                        arqext2 - Convert.ToDouble(Maior.Split(';')[1]) -
                                        Convert.ToDouble(dados3["Arquivo3"]);

                                    var atualizaValorNR = new QueryMysqlTesouraria();
                                    atualizaValorNR.InsereValorNr(dados3["Arquivo3"].ToString(),
                                        dataSelecionada.ToString("yyyy-MM-dd 00:00:00"));

                                    insereConferencia1.InsereConferencia(Dados.Data.ToString("yyyy/MM/dd"),
                                        "4 - Cheques Dep./TD Devolvidos", arqext2.ToString(),
                                        TempData["6/192-FINAL"].ToString(), TempData["Diferenca5"].ToString());

                                    break;

                                case 4:

                                    inicioPlanilha3 = inicio.InicioPlanilha(caminho04, i.ToString());
                                    dados4 = inicio.Calculo1(caminho04, i.ToString(), inicioPlanilha3);
                                    TempData["NRDEVELETRONICA-FINAL"] = arqext11 + Convert.ToDouble(dados4["Arquivo4"]);
                                    
                                    break;

                                case 5:

                                    if (auxNR == 1) TempData["NRDEVELETRONICA-FINAL"] = arqext11 + 0;
                                    inicioPlanilha4 = inicio.InicioPlanilha(caminho05, i.ToString());
                                    dados5 = inicio.Calculo1(caminho05, i.ToString(), inicioPlanilha4);
                                    TempData["CHEQUEDEVOLVIDO"] = Convert.ToDouble(dados5["Arquivo5"]);
                                    Maior = inicio.Diferenciar(Convert.ToDouble(TempData["NRDEVELETRONICA-FINAL"]),
                                        Math.Round(Convert.ToDouble(TempData["CHEQUEDEVOLVIDO"]), 2));
                                    TempData["Diferenca11"] = Maior.Split(';')[1];
                                    TempData["DiferencaTexto11"] = Maior.Split(';')[0];
                                    insereConferencia1.InsereConferencia(Dados.Data.ToString("yyyy/MM/dd"),
                                        "10 - Cheques Devolvidos 4030", TempData["NRDEVELETRONICA-FINAL"].ToString(),
                                        TempData["CHEQUEDEVOLVIDO"].ToString(), TempData["Diferenca11"].ToString());
                                    break;
                            }
                        }
                        else
                        {
                            i = arquivos.Count();
                            break;
                        }

                        i++;
                        if (i == 6) break;
                    }
                }
            }

            if (aux == 0)
                return RedirectToAction("Tesouraria", new {MensagemValidacao = "Registro salvo com sucesso!!"});
            return RedirectToAction("Tesouraria",
                new {Erro = "Data selecionada não e a mesma do extrato.Refaça o processo."});
        }


        public ActionResult Resultado()
        {
            return PartialView("ViewResultado");
        }


        [HttpPost]
        public ActionResult Resultado(FormCollection receberForm)
        {
            var insereJustificativa = new QueryMysqlTesouraria();

            if (receberForm.Count > 0)
            {
                //string teste = dataescolhida.ToString();
                //  string data = TempData["data"].ToString();
                var data = receberForm["dataescolhida"];
                var justificativa = receberForm["justificativa"];
                insereJustificativa.InsereJustificativa(data, justificativa);
            }

            return RedirectToAction("Tesouraria");
        }

        public ActionResult Pesquisa()
        {
            TempData["RetornaValor"] = 1;
            return View("ViewPesquisar");
        }

        [HttpPost]
        public ActionResult Pesquisa(string data)
        {
            var dadosTableCon = new QueryMysqlTesouraria();
            var DadosTable = dadosTableCon.RecuperaDadosTabela(data);
            if (DadosTable.Count > 0)
            {
                TempData["RetornaValor"] = 0;
                TempData["historico"] = DadosTable[0]["historico"];
                TempData["extrato"] = DadosTable[0]["extrato"];
                TempData["arquivos"] = DadosTable[0]["arquivos"];
                TempData["diferenca"] = DadosTable[0]["diferenca"];

                TempData["historico1"] = DadosTable[1]["historico"];
                TempData["extrato1"] = DadosTable[1]["extrato"];
                TempData["arquivos1"] = DadosTable[1]["arquivos"];
                TempData["diferenca1"] = DadosTable[1]["diferenca"];

                TempData["historico2"] = DadosTable[2]["historico"];
                TempData["extrato2"] = DadosTable[2]["extrato"];
                TempData["arquivos2"] = DadosTable[2]["arquivos"];
                TempData["diferenca2"] = DadosTable[2]["diferenca"];

                TempData["historico3"] = DadosTable[3]["historico"];
                TempData["extrato3"] = DadosTable[3]["extrato"];
                TempData["arquivos3"] = DadosTable[3]["arquivos"];
                TempData["diferenca3"] = DadosTable[3]["diferenca"];

                TempData["historico4"] = DadosTable[4]["historico"];
                TempData["extrato4"] = DadosTable[4]["extrato"];
                TempData["arquivos4"] = DadosTable[4]["arquivos"];
                TempData["diferenca4"] = DadosTable[4]["diferenca"];

                TempData["historico5"] = DadosTable[5]["historico"];
                TempData["extrato5"] = DadosTable[5]["extrato"];
                TempData["arquivos5"] = DadosTable[5]["arquivos"];
                TempData["diferenca5"] = DadosTable[5]["diferenca"];

                TempData["historico6"] = DadosTable[6]["historico"];
                TempData["extrato6"] = DadosTable[6]["extrato"];
                TempData["arquivos6"] = DadosTable[6]["arquivos"];
                TempData["diferenca6"] = DadosTable[6]["diferenca"];

                TempData["historico7"] = DadosTable[7]["historico"];
                TempData["extrato7"] = DadosTable[7]["extrato"];
                TempData["arquivos7"] = DadosTable[7]["arquivos"];
                TempData["diferenca7"] = DadosTable[7]["diferenca"];

                TempData["historico8"] = DadosTable[8]["historico"];
                TempData["extrato8"] = DadosTable[8]["extrato"];
                TempData["arquivos8"] = DadosTable[8]["arquivos"];
                TempData["diferenca8"] = DadosTable[8]["diferenca"];

                TempData["historico9"] = DadosTable[9]["historico"];
                TempData["extrato9"] = DadosTable[9]["extrato"];
                TempData["arquivos9"] = DadosTable[9]["arquivos"];
                TempData["diferenca9"] = DadosTable[9]["diferenca"];


                TempData["data"] = data;

                var dadosJustificativa = dadosTableCon.RecuperaDadosJustificativa(data);
                var modelTesouraria = new Tesouraria();

                modelTesouraria.Justificativa = dadosJustificativa[0]["justificativa"];
                TempData["dataPesquisa"] = data;

                return PartialView("ViewTabelaPesquisar", modelTesouraria);
            }

            TempData["RetornaValor"] = 1;
            return PartialView("ViewTabelaPesquisar");
        }


        public ActionResult AtualizaJustificativa()
        {
            return PartialView("ViewPesquisar");
        }

        [HttpPost]
        public ActionResult AtualizaJustificativa(string data, FormCollection receberForm)
        {
            var atualizaJustificativa = new QueryMysqlTesouraria();

            atualizaJustificativa.AtualizaJustificativa(TempData["data"].ToString(), receberForm["justificativa"]);
            return RedirectToAction("Tesouraria", new {MensagemValidacao = "Registro alterado com sucesso!!"});
        }


        public ActionResult VerificaData()
        {
            return PartialView("ViewPesquisar");
        }

        [HttpPost]
        public ActionResult VerificaData(string data)
        {
            var verificarData = new QueryMysqlTesouraria();

            var retornaData = Convert.ToInt32(verificarData.VerificaData(data));
            if (retornaData != 0)
                return RedirectToAction("Tesouraria", new {Erro = "Já existe conferência para essa data."});
            return RedirectToAction("Tesouraria");
        }


        public ActionResult DeletaProducao()
        {
            return PartialView("ViewPesquisar");
        }

        [HttpPost]
        public ActionResult DeletaProducao(string data)
        {
            var deletarProducao = new QueryMysqlTesouraria();

            deletarProducao.DeletaProducao(TempData["data"].ToString());
            return RedirectToAction("Tesouraria", new {MensagemValidacao = "Registro  com sucesso!!"});
        }
    }
}