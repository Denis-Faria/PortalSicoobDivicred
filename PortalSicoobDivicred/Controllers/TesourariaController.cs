﻿using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Data;
using OFXSharp;
using PortalSicoobDivicred.Models;
using PortalSicoobDivicred.Aplicacao;
using System.Collections.Generic;
using System.Linq;

namespace PortalSicoobDivicred.Controllers
{


    public class TesourariaController : Controller
    {
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
                var Cookie = Request.Cookies.Get("CookieFarm");
                var Login = Criptografa.Descriptografar(Cookie.Value);
                var DadosUsuarioBanco = insereDados.RecuperaDadosUsuarios(Login);
                if (insereDados.PermissaoTesouraria(DadosUsuarioBanco[0]["login"]))
                    TempData["PermissaoTesouraria"] =
                        " ";
                else
                    TempData["PermissaoTesouraria"] = "display: none";

                string idUsuario = insereDados.RecuperaUsuario(Login);
                TempData["NomeLateral"] = DadosUsuarioBanco[0]["login"];
                TempData["EmailLateral"] = DadosUsuarioBanco[0]["email"];

                return View("Tesouraria");
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

        }

        //  public ActionResult ValidaDataArquivo()
        //    {
        //        return PartialView("ViewArquivos");
        //   }

        /*   [HttpPost]
           public ActionResult ValidaDataArquivo(FormCollection receberForm)
           {

               int qtdarquivos = 5;
               ManipularPlanilha validarData = new ManipularPlanilha();

               switch (qtdarquivos)
               {
                   case 5:
                       string posicao = "F11";
                       validarData.validaDataRelatorio(posicao,qtdarquivos);

                       break;
               }
               return PartialView("Tesouraria");
          }*/

        public ActionResult ProcessaArquivos()
        {
            return PartialView("ViewArquivos");
        }

        [HttpPost]
        public ActionResult ProcessaArquivos(IEnumerable<HttpPostedFileBase> file, Tesouraria Dados, FormCollection receberForm)
        {
            var consultaValorNR = new QueryMysqlTesouraria();

            double valorNR = Convert.ToDouble(consultaValorNR.consultaValorNR());

            var valorJudicial = 0.0;
            var valor6192 = 0.0;
            var valorNr1 = Convert.ToDouble(receberForm["valorCampoNr1"].ToString().Replace('.', ','));
            var valorNr2 = Convert.ToDouble(receberForm["valorCampoNr2"].ToString().Replace('.', ','));
            var valorNr3 = Convert.ToDouble(receberForm["valorCampoNr3"].ToString().Replace('.', ','));
            //var valorNr4 = Convert.ToDouble(receberForm["valorCampoNr4"].ToString().Replace('.', ','));
            TempData["valorTotalNr"] = Math.Round(valorNr1 + valorNr2 + valorNr3 + valorNR, 2);

            if (receberForm["valorJudicial"].ToString().Length == 0)
            {
                valorJudicial = 0;
            }
            else
            {
                valorJudicial = Convert.ToDouble(receberForm["valorJudicial"].ToString().Replace('.', ','));
            }

            if (receberForm["valor6192"].ToString().Length == 0)
            {
                valor6192 = 0;
            }
            else
            {
                valor6192 = Convert.ToDouble(receberForm["valor6192"].ToString().Replace('.', ','));
            }

            TempData["data"] = receberForm["data"].ToString();

            var insereValorCampos = new QueryMysqlTesouraria();
            insereValorCampos.insereValoresCampos(valorJudicial.ToString(), valor6192.ToString(), valorNr1.ToString(), valorNr2.ToString(), valorNr3.ToString(), Convert.ToDateTime(TempData["data"]), DateTime.Now);

            string Maior = "";
            string historico = "";
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
            int auxNR = 0;//quando 1, o usuário selecionou a opção de não usar o arquivo de op de caixa. Assim apenas o valor do NRDEV aparece no campo de extrato.


            var parser = new OFXDocumentParser();


            mDataSet = new DataSet();
            Conexao conectaPlanilha = new Conexao();
            ManipularPlanilha inicio = new ManipularPlanilha();
            var arquivos = file.ToList();
            var nomeArquivo = "";
            var nomeArquivo1 = "";
            var caminho = "";
            var caminho1 = "";
            int dataValida = 0;

            int inicioPlanilha1 = 0;
            int inicioPlanilha2 = 0;
            int inicioPlanilha3 = 0;
            int inicioPlanilha4 = 0;

            var aux = 0;

            if (arquivos.Count > 3)
            {
                var consultaFeriado = new QueryMysqlTesouraria();


                DataSet output = new DataSet();
                Dictionary<string, double> dados1 = new Dictionary<string, double>();
                Dictionary<string, double> dados2 = new Dictionary<string, double>();
                Dictionary<string, double> dados3 = new Dictionary<string, double>();
                Dictionary<string, double> dados4 = new Dictionary<string, double>();
                Dictionary<string, double> dados5 = new Dictionary<string, double>();
                int i = 0;
                var dataSelecionadaValidar = new DateTime();
                var dataExtratoDiaValidar = new DateTime();
                while (i < arquivos.Count)
                {
                    var dataSelecionada = new DateTime();
                    var dataExtratoDia = new DateTime();

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
                    arquivos[i].SaveAs(caminho);
                    string caminho01 = "";
                    string caminho02 = "";
                    string caminho03 = "";
                    string caminho04 = "";
                    string caminho05 = "";
                    string nomeArquivo01 = "";
                    string nomeArquivo02 = "";
                    string nomeArquivo03 = "";
                    string nomeArquivo04 = "";
                    string nomeArquivo05 = "";


                    nomeArquivo01 = Path.GetFileName(arquivos[1].FileName);
                    caminho01 = Path.Combine(Server.MapPath("~/Uploads/"), (DateTime.Now.Hour).ToString()+ (DateTime.Now.Minute).ToString() + nomeArquivo01);
                    arquivos[1].SaveAs(caminho01);

                    nomeArquivo02 = Path.GetFileName(arquivos[2].FileName);
                    caminho02 = Path.Combine(Server.MapPath("~/Uploads/"), (DateTime.Now.Hour).ToString() + (DateTime.Now.Minute).ToString() + nomeArquivo02);
                    arquivos[2].SaveAs(caminho02);

                    nomeArquivo03 = Path.GetFileName(arquivos[3].FileName);
                    caminho03 = Path.Combine(Server.MapPath("~/Uploads/"), (DateTime.Now.Hour).ToString() + (DateTime.Now.Minute).ToString() + nomeArquivo03);
                    arquivos[3].SaveAs(caminho03);

                    nomeArquivo04 = Path.GetFileName(arquivos[4].FileName);
                    caminho04 = Path.Combine(Server.MapPath("~/Uploads/"), (DateTime.Now.Hour).ToString() + (DateTime.Now.Minute).ToString() + nomeArquivo04);
                    arquivos[4].SaveAs(caminho04);

                    nomeArquivo05 = Path.GetFileName(arquivos[5].FileName);
                    caminho05 = Path.Combine(Server.MapPath("~/Uploads/"), (DateTime.Now.Hour).ToString() + (DateTime.Now.Minute).ToString() + nomeArquivo05);
                    arquivos[5].SaveAs(caminho05);




                    if (i != 0)
                    {
                        output.Tables.Add(conectaPlanilha.importarExcel(caminho, i.ToString()));

                        dataSelecionada = DateTime.Now;
                        dataExtratoDia = DateTime.Now;
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

                    int auxdias = 0;
                    for (int m = 1; m <= 7; m++)
                    {
                        if ((consultaFeriado.VerificaFeriadoDivinopolis(dataSelecionada.Date.AddDays(-m))) == 0 && dataSelecionada.Date.AddDays(-m).DayOfWeek != DayOfWeek.Sunday && dataSelecionada.Date.AddDays(-m).DayOfWeek != DayOfWeek.Saturday)
                        {
                            auxdias = m;
                            break;
                        }
                    }

                    if (dataValida == 0 && i != 0)
                    {
                        int k;
                        var arquivosAUX = file.ToList();
                        for (k = 0; k < arquivosAUX.Count; k++)
                        {
                            nomeArquivo1 = Path.GetFileName(arquivosAUX[k].FileName);
                           // caminho1 = Path.Combine(Server.MapPath("~/Uploads/"), nomeArquivo);
                            //arquivos[k].SaveAs(caminho);

                            //--
                            //Tratamento Validação Data
                            if (k == 1)
                            {
                                string posicao = "F4";
                                string valeData = inicio.validaDataRelatorio(posicao, k, caminho01);

                                if (dataSelecionadaValidar == Convert.ToDateTime(valeData))
                                {

                                }
                                else
                                {
                                    dataValida = 1;
                                    return RedirectToAction("Tesouraria", new { Erro = "Data Invalida do Relatório Lançamento" });

                                }

                            }
                            if (k == 2)
                            {
                                string posicao = "F3";
                                string valeData = inicio.validaDataRelatorio(posicao, k, caminho02);

                                if (valeData == "0")
                                {
                                    dataValida = 1;
                                    return RedirectToAction("Tesouraria", new { Erro = "Período do Relatório Enviadas e Recebidas são diferentes" });
                                }
                                else
                                {
                                    if (dataSelecionadaValidar == Convert.ToDateTime(valeData))
                                    {

                                    }
                                    else
                                    {
                                        dataValida = 1;
                                        return RedirectToAction("Tesouraria", new { Erro = "Data Invalida do Relatório Enviadas e Recebidas" });
                                    }
                                }

                            }

                            if (k == 3)
                            {
                                string posicao = "F4";
                                string valeData = inicio.validaDataRelatorio(posicao, k, caminho03);

                                if (dataSelecionadaValidar == Convert.ToDateTime(valeData))
                                {

                                }
                                else
                                {
                                    dataValida = 1;
                                    return RedirectToAction("Tesouraria", new { Erro = "Data Invalida no Relatório de Devolução de Cheques" });
                                }

                            }

                            if (k == 4)
                            {
                                string posicao = "F15";
                                string valeData = inicio.validaDataRelatorio(posicao, k, caminho04);
                                if (valeData == "0")
                                {
                                    dataValida = 1;
                                    return RedirectToAction("Tesouraria", new { Erro = "Período do Relatório Operação de Caixa(Dia Anterior) são diferentes!!!!" });
                                }
                                else
                                {
                                    if (dataExtratoDiaValidar == Convert.ToDateTime(valeData))
                                    {

                                    }
                                    else
                                    {
                                        dataValida = 1;
                                        return RedirectToAction("Tesouraria", new { Erro = "Data Invalida do Relatório Operação de Caixa(Dia Anterior)" });
                                    }
                                }

                            }
                            //Lembrar de deletar o que for para o banco de dados

                            if (k == 5)
                            {
                                string posicao = "F11";
                                string valeData = inicio.validaDataRelatorio(posicao, k, caminho05);
                                if (valeData == "0")
                                {
                                    dataValida = 1;
                                    return RedirectToAction("Tesouraria", new { Erro = "Período do Relatório Cheques Devolvidos(Dia Anterior) são diferentes" });
                                }
                                else
                                {
                                    if (dataExtratoDiaValidar == Convert.ToDateTime(valeData))
                                    {

                                    }
                                    else
                                    {
                                        dataValida = 1;
                                        return RedirectToAction("Tesouraria", new { Erro = "Data Invalida do Relatório Cheques Devolvidos(Dia Anterior)" });
                                    }

                                }

                            }
                            //--
                        }
                    }



                    if (dataValida == 0)
                    {
                        //fim teste
                        if (dataExtratoDia == dataSelecionada || dataSelecionada == dataExtratoDia.Date.AddDays(auxdias))
                        {
                            var insereConferencia1 = new QueryMysqlTesouraria();
                            switch (i)
                            {
                                case 0:
                                    var ofxdocument = parser.Import(new FileStream(caminho, FileMode.Open));
                                    var transacoes = ofxdocument.Transactions;

                                    for (int j = 0; j < transacoes.Count; j++)
                                    {
                                        historico = transacoes[j].Memo;
                                        var dataExtrato = transacoes[j].Date;

                                        if (dataExtrato == dataSelecionada)
                                        {
                                            if (historico.Contains("SRS ELE") || historico.Contains("SRI ELE") || historico.Contains("SR CHQ ROUBADO SUP ELETR"))
                                            {
                                                var teste = Math.Abs(Convert.ToDouble(transacoes[j].Amount));
                                                arqext1 = arqext1 + Math.Abs(Convert.ToDouble(transacoes[j].Amount));
                                                arqext1 = Math.Round(arqext1, 2);
                                                TempData["Extrato-3/4/5"] = arqext1;
                                            }
                                            else if (historico.Contains("NICA NOITE") || historico.Contains("SR DEV CONVENCIONAL DIA"))
                                            {
                                                arqext2 = arqext2 + Math.Abs(Convert.ToDouble(transacoes[j].Amount));
                                                arqext2 = Math.Round(arqext2, 2);
                                                TempData["Extrato-6/192"] = arqext2;
                                            }
                                            else if (historico.Contains("O TED - SPB") || historico.Contains("BITO TED CONTA SAL") || historico.Contains("O TED BANCOOB - SPB") || historico.Contains("O TED BANCOOB MESMA TITULAR"))
                                            {
                                                arqext3 = arqext3 + Math.Abs(Convert.ToDouble(transacoes[j].Amount));
                                                arqext3 = Math.Round(arqext3, 2);
                                                TempData["Extrato-5472/5473/5474/232/233/234/235"] = arqext3;
                                            }
                                            else if (historico.Contains("DITO TED RECEBIDA -SPB") || historico.Contains("DITO TED BANCOOB RECEBIDA -SPB") || historico.Contains("DITO DEC-CONTA SALARIO") || historico.Contains("SUA REMESSA TEC - ELETR") || historico.Contains("DITO TED CONTA SAL"))
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
                                            else if (historico.Contains("NRI ELETR") || historico.Contains("NRS ELETR") || historico.Contains("NR CHQ ROUBADO SUP ELETR"))
                                            {
                                                arqext6 = arqext6 + Math.Abs(Convert.ToDouble(transacoes[j].Amount));
                                                TempData["ExtratoCampos"] = Math.Round(arqext6, 2);

                                                Maior = inicio.diferenciar(arqext6, Math.Round(Convert.ToDouble(TempData["valorTotalNr"]), 2));
                                                TempData["Diferenca7"] = Maior.Split(';')[1];
                                                TempData["DiferencaTexto7"] = Maior.Split(';')[0];

                                            }
                                            else if (historico.Contains("A VLB-INF"))
                                            {
                                                arqext7 = arqext7 + Math.Abs(Convert.ToDouble(transacoes[j].Amount));
                                                arqext7 = Math.Round(arqext7, 2);
                                            }
                                            else if (historico.Contains("BITO EMP. - REPES COTAS PARTES CDI") || historico.Contains("O COTAS PARTES"))
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
                                        else
                                        if (historico.Contains("NR DEV ELETR"))
                                        {
                                            arqext11 = arqext11 + Math.Abs(Convert.ToDouble(transacoes[j].Amount));
                                            arqext11 = Math.Round(arqext11, 2);
                                            TempData["Extrato-NRDEVELETRONICA"] = arqext11;

                                        }
                                    }

                                    break;


                                case 1:

                                    int inicioPlanilha0 = 0;
                                    inicioPlanilha0 = inicio.InicioPlanilha(caminho, i.ToString());
                                    dados1 = inicio.calculo1(caminho, i.ToString(), inicioPlanilha0);

                                    TempData["3/4/5"] = Convert.ToDouble(dados1["3/4/5"]);
                                    Maior = inicio.diferenciar(Math.Round(arqext1, 2), Math.Round(Convert.ToDouble(dados1["3/4/5"]), 2));
                                    TempData["Diferenca1"] = Maior.Split(';')[1];
                                    TempData["DiferencaTexto1"] = Maior.Split(';')[0];
                                    insereConferencia1.InsereConferencia((Dados.data).ToString("yyyy/MM/dd"), "Cheques 4030", arqext1.ToString(), TempData["3/4/5"].ToString(), TempData["Diferenca1"].ToString());

                                    TempData["5761"] = Convert.ToDouble(dados1["5761"]);
                                    Maior = inicio.diferenciar(arqext8, Math.Round(Convert.ToDouble(dados1["5761"]), 2));
                                    TempData["Diferenca8"] = Maior.Split(';')[1];
                                    TempData["DiferencaTexto8"] = Maior.Split(';')[0];
                                    insereConferencia1.InsereConferencia((Dados.data).ToString("yyyy/MM/dd"), "Cotas Partes", arqext8.ToString(), TempData["5761"].ToString(), TempData["Diferenca8"].ToString());

                                    TempData["820"] = Convert.ToDouble(dados1["820"]);
                                    Maior = inicio.diferenciar(arqext9, Math.Round(Convert.ToDouble(dados1["820"]), 2));
                                    TempData["Diferenca9"] = Maior.Split(';')[1];
                                    TempData["DiferencaTexto9"] = Maior.Split(';')[0];
                                    insereConferencia1.InsereConferencia((Dados.data).ToString("yyyy/MM/dd"), "Docs Recebidos", arqext9.ToString(), TempData["820"].ToString(), TempData["Diferenca9"].ToString());

                                    TempData["257/258"] = Convert.ToDouble(dados1["257/258"]);
                                    Maior = inicio.diferenciar(arqext10, Math.Round(Convert.ToDouble(dados1["257/258"]), 2));
                                    TempData["Diferenca10"] = Maior.Split(';')[1];
                                    TempData["DiferencaTexto10"] = Maior.Split(';')[0];
                                    insereConferencia1.InsereConferencia((Dados.data).ToString("yyyy/MM/dd"), "Docs Enviados", arqext10.ToString(), TempData["257/258"].ToString(), TempData["Diferenca10"].ToString());

                                    TempData["5472/5473/5474/232/233/234/235"] = Convert.ToDouble(dados1["5472/5473/5474/232/233/234/235"]) + Convert.ToDouble(valorJudicial);
                                    Maior = inicio.diferenciar(arqext3, Math.Round(Convert.ToDouble(dados1["5472/5473/5474/232/233/234/235"]) + Convert.ToDouble(valorJudicial), 2));

                                    TempData["821/2/7286/7336/847"] = Convert.ToDouble(dados1["821/2/7286/7336/847"]) + Convert.ToDouble(arqext12);
                                    Maior = inicio.diferenciar(arqext4, Math.Round(Convert.ToDouble(dados1["821/2/7286/7336/847"]) + Convert.ToDouble(arqext12), 2));
                                    TempData["Diferenca4"] = Maior.Split(';')[1];
                                    TempData["DiferencaTexto4"] = Maior.Split(';')[0];
                                    insereConferencia1.InsereConferencia((Dados.data).ToString("yyyy/MM/dd"), "TED´s Recebidas", arqext4.ToString(), TempData["821/2/7286/7336/847"].ToString(), TempData["Diferenca4"].ToString());
                                    TempData["6/192"] = Convert.ToDouble(dados1["6/192"]) - Convert.ToDouble(valor6192);

                                    TempData["7027"] = Convert.ToDouble(dados1["7027"]);
                                    Maior = inicio.diferenciar(arqext5, Math.Round(Convert.ToDouble(dados1["7027"]), 2));
                                    TempData["Diferenca6"] = Maior.Split(';')[1];
                                    TempData["DiferencaTexto6"] = Maior.Split(';')[0];
                                    insereConferencia1.InsereConferencia((Dados.data).ToString("yyyy/MM/dd"), "Boletos Rejeitados", arqext5.ToString(), TempData["7027"].ToString(), TempData["Diferenca6"].ToString());

                                    insereConferencia1.InsereConferencia((Dados.data).ToString("yyyy/MM/dd"), "Cheques Compensados", arqext6.ToString(), TempData["valorTotalNr"].ToString(), TempData["Diferenca7"].ToString());

                                    break;

                                case 2:

                                    inicioPlanilha1 = inicio.InicioPlanilha(caminho, i.ToString());
                                    dados2 = inicio.calculo1(caminho, i.ToString(), inicioPlanilha1);
                                    TempData["5472/5473/5474/232/233/234/235-FINAL"] = Math.Round((Convert.ToDouble(TempData["5472/5473/5474/232/233/234/235"]) + Convert.ToDouble(dados2["Arquivo2"])), 2);
                                    TempData["5472/5473/5474/232/233/234/235-FINALB"] = Math.Round(Convert.ToDouble(TempData["5472/5473/5474/232/233/234/235"]) + Convert.ToDouble(dados2["Arquivo2"]), 2);
                                    Maior = inicio.diferenciar(arqext3, Math.Round(Convert.ToDouble(TempData["5472/5473/5474/232/233/234/235-FINALB"]), 2));
                                    TempData["Diferenca3"] = Maior.Split(';')[1];
                                    TempData["DiferencaTexto3"] = Maior.Split(';')[0];

                                    insereConferencia1.InsereConferencia((Dados.data).ToString("yyyy/MM/dd"), "TED´s Enviadas", arqext3.ToString(), TempData["5472/5473/5474/232/233/234/235-FINAL"].ToString(), TempData["Diferenca3"].ToString());
                                    break;

                                case 3:



                                    inicioPlanilha2 = inicio.InicioPlanilha(caminho, i.ToString());
                                    dados3 = inicio.calculo1(caminho, i.ToString(), inicioPlanilha2);

                                    Maior = inicio.diferenciar(arqext2, Math.Round(Convert.ToDouble(TempData["6/192"]), 2));

                                    TempData["Diferenca5"] = (Convert.ToDouble(Maior.Split(';')[1])) - Convert.ToDouble(dados3["Arquivo3"]);
                                    TempData["DiferencaTexto5"] = Maior.Split(';')[0];
                                    TempData["6/192-FINAL"] = arqext2 + (Convert.ToDouble(TempData["Diferenca5"]));
                                    TempData["6/192-FINALB"] = arqext2 - (Convert.ToDouble(Maior.Split(';')[1])) - Convert.ToDouble(dados3["Arquivo3"]);

                                    var atualizaValorNR = new QueryMysqlTesouraria();
                                    atualizaValorNR.insereValorNR(dados3["Arquivo3"].ToString(), dataSelecionada.ToString("yyyy-MM-dd 00:00:00"));

                                    insereConferencia1.InsereConferencia((Dados.data).ToString("yyyy/MM/dd"), "Cheques Dep./TD Devolvidos", arqext2.ToString(), TempData["6/192-FINAL"].ToString(), TempData["Diferenca5"].ToString());

                                    break;

                                case 4:

                                    inicioPlanilha3 = inicio.InicioPlanilha(caminho, i.ToString());
                                    dados4 = inicio.calculo1(caminho, i.ToString(), inicioPlanilha3);
                                    TempData["NRDEVELETRONICA-FINAL"] = arqext11 + Convert.ToDouble(dados4["Arquivo4"]);

                                    break;

                                case 5:

                                    if (auxNR == 1)
                                    {
                                        TempData["NRDEVELETRONICA-FINAL"] = arqext11 + 0;
                                    }
                                    inicioPlanilha4 = inicio.InicioPlanilha(caminho, i.ToString());
                                    dados5 = inicio.calculo1(caminho, i.ToString(), inicioPlanilha4);
                                    TempData["CHEQUEDEVOLVIDO"] = Convert.ToDouble(dados5["Arquivo5"]);
                                    Maior = inicio.diferenciar(Convert.ToDouble(TempData["NRDEVELETRONICA-FINAL"]), Math.Round(Convert.ToDouble(TempData["CHEQUEDEVOLVIDO"]), 2));
                                    TempData["Diferenca11"] = Maior.Split(';')[1];
                                    TempData["DiferencaTexto11"] = Maior.Split(';')[0];
                                    insereConferencia1.InsereConferencia((Dados.data).ToString("yyyy/MM/dd"), "Cheques Devolvidos 4030", TempData["NRDEVELETRONICA-FINAL"].ToString(), TempData["CHEQUEDEVOLVIDO"].ToString(), TempData["Diferenca11"].ToString());
                                    break;
                            }
                        }
                        else
                        {
                            i = arquivos.Count();
                            break;
                        }
                        i++;
                        if (i == 6)
                        {
                            break;
                        }
                    }
                }
            }
            if (aux == 0)
            {
                return RedirectToAction("Tesouraria", new { MensagemValidacao = "Registro salvo com sucesso!!" });
            }
            else
            {

                return RedirectToAction("Tesouraria", new { Erro = "Data selecionada não e a mesma do extrato.Refaça o processo." });
            }

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

                string data = TempData["data"].ToString();
                string justificativa = receberForm["justificativa"];
                insereJustificativa.InsereJustificativa(data, justificativa);
            }
            return RedirectToAction("Tesouraria");
        }

        public ActionResult Pesquisa()
        {
            return PartialView("ViewPesquisar");
        }

        [HttpPost]
        public ActionResult Pesquisa(string data)
        {
            var dadosTableCon = new QueryMysqlTesouraria();
            var DadosTable = dadosTableCon.RecuperaDadosTabela(data);
            if (DadosTable.Count > 0)
            {
                TempData["RetornaValor"] = 0;
                TempData["historico"] = DadosTable[0]["historico"].ToString();
                TempData["extrato"] = DadosTable[0]["extrato"].ToString();
                TempData["arquivos"] = DadosTable[0]["arquivos"].ToString();
                TempData["diferenca"] = DadosTable[0]["diferenca"].ToString();

                TempData["historico1"] = DadosTable[1]["historico"].ToString();
                TempData["extrato1"] = DadosTable[1]["extrato"].ToString();
                TempData["arquivos1"] = DadosTable[1]["arquivos"].ToString();
                TempData["diferenca1"] = DadosTable[1]["diferenca"].ToString();

                TempData["historico2"] = DadosTable[2]["historico"].ToString();
                TempData["extrato2"] = DadosTable[2]["extrato"].ToString();
                TempData["arquivos2"] = DadosTable[2]["arquivos"].ToString();
                TempData["diferenca2"] = DadosTable[2]["diferenca"].ToString();

                TempData["historico3"] = DadosTable[3]["historico"].ToString();
                TempData["extrato3"] = DadosTable[3]["extrato"].ToString();
                TempData["arquivos3"] = DadosTable[3]["arquivos"].ToString();
                TempData["diferenca3"] = DadosTable[3]["diferenca"].ToString();

                TempData["historico4"] = DadosTable[4]["historico"].ToString();
                TempData["extrato4"] = DadosTable[4]["extrato"].ToString();
                TempData["arquivos4"] = DadosTable[4]["arquivos"].ToString();
                TempData["diferenca4"] = DadosTable[4]["diferenca"].ToString();

                TempData["historico5"] = DadosTable[5]["historico"].ToString();
                TempData["extrato5"] = DadosTable[5]["extrato"].ToString();
                TempData["arquivos5"] = DadosTable[5]["arquivos"].ToString();
                TempData["diferenca5"] = DadosTable[5]["diferenca"].ToString();

                TempData["historico6"] = DadosTable[6]["historico"].ToString();
                TempData["extrato6"] = DadosTable[6]["extrato"].ToString();
                TempData["arquivos6"] = DadosTable[6]["arquivos"].ToString();
                TempData["diferenca6"] = DadosTable[6]["diferenca"].ToString();

                TempData["historico7"] = DadosTable[7]["historico"].ToString();
                TempData["extrato7"] = DadosTable[7]["extrato"].ToString();
                TempData["arquivos7"] = DadosTable[7]["arquivos"].ToString();
                TempData["diferenca7"] = DadosTable[7]["diferenca"].ToString();

                TempData["historico8"] = DadosTable[8]["historico"].ToString();
                TempData["extrato8"] = DadosTable[8]["extrato"].ToString();
                TempData["arquivos8"] = DadosTable[8]["arquivos"].ToString();
                TempData["diferenca8"] = DadosTable[8]["diferenca"].ToString();

                TempData["historico9"] = DadosTable[9]["historico"].ToString();
                TempData["extrato9"] = DadosTable[9]["extrato"].ToString();
                TempData["arquivos9"] = DadosTable[9]["arquivos"].ToString();
                TempData["diferenca9"] = DadosTable[9]["diferenca"].ToString();



                TempData["data"] = data.ToString();

                var dadosJustificativa = dadosTableCon.RecuperaDadosJustificativa(data);
                var modelTesouraria = new Tesouraria();

                modelTesouraria.justificativa = dadosJustificativa[0]["justificativa"];
                TempData["dataPesquisa"] = data;

                return PartialView("ViewTabelaPesquisar", modelTesouraria);
            }
            else
            {
                TempData["RetornaValor"] = 1;
                return PartialView("ViewTabelaPesquisar");
            }
        }



        public ActionResult AtualizaJustificativa()
        {
            return PartialView("ViewPesquisar");
        }

        [HttpPost]
        public ActionResult AtualizaJustificativa(string data, FormCollection receberForm)
        {

            var atualizaJustificativa = new QueryMysqlTesouraria();

            atualizaJustificativa.atualizaJustificativa(TempData["data"].ToString(), receberForm["justificativa"].ToString());
            return RedirectToAction("Tesouraria", new { MensagemValidacao = "Registro alterado com sucesso!!" });
        }




        public ActionResult VerificaData()
        {
            return PartialView("ViewPesquisar");
        }

        [HttpPost]
        public ActionResult VerificaData(string data)
        {
            var verificarData = new QueryMysqlTesouraria();

            int retornaData = Convert.ToInt32(verificarData.VerificaData(data));
            if (retornaData != 0)
                return RedirectToAction("Tesouraria", new { Erro = "Já existe conferência para essa data." });
            else
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
            return RedirectToAction("Tesouraria", new { MensagemValidacao = "Registro  com sucesso!!" });
        }

    }
}
