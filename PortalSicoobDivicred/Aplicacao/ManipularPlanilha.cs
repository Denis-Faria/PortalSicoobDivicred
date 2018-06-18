using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using PortalSicoobDivicred.Aplicacao;

namespace PortalSicoobDivicred.Controllers
{
    public class ManipularPlanilha
    {
        public int InicioPlanilha(string endereco, string count)
        {
            int inicio = 0;
            int k = 0;//feito para colocar o inicio do for. Considerando celulas mescladas no inicio.
            int m = 0;//feito para definir qual coluna vai servir de base para o inicio
            Conexao conectaPlanilha = new Conexao();
            DataSet output = new DataSet();
            string palavraInicio = "";
            output.Tables.Add(conectaPlanilha.importarExcel(endereco, count));


            switch (count)
            {

                case "1":
                    palavraInicio = "Conta";
                    k = 1;

                    break;

                case "2":
                    palavraInicio = "Canal";
                    k = 4;
                    break;

                case "3":
                    palavraInicio = "Conta";
                    k = 4;
                    m = 32;
                    break;

                case "4":
                    palavraInicio = "CHEQUE DEVOLVIDO CAIXA";
                    k = 4;
                    m = 12;
                    break;

                case "5":
                    palavraInicio = "Total:";
                    k = 4;
                    m = 15;
                    break;
            }

            for (int j = k; j <= output.Tables[0].Rows.Count; j++)
            {
                string nomeColuna = output.Tables[0].Columns[m].ColumnName;
                try
                {
                    if (output.Tables[0].Rows[j][nomeColuna].ToString().Length > 0)
                    {
                        if (output.Tables[0].Rows[j][nomeColuna].ToString().Contains(palavraInicio))
                        {
                            string nome = output.Tables[0].Rows[j][nomeColuna].ToString();
                            inicio = j + 1;
                            break;
                        }
                    }
                }
                catch { }
            }
            
            return inicio;
        }

        public Dictionary<string, double> calculo1(string caminho, string count, int inicio)
        {

            Dictionary<string, double> somatorio = new Dictionary<string, double>();

            
            Conexao conectaPlanilha = new Conexao();
            DataSet output = new DataSet();
            output.Tables.Add(conectaPlanilha.importarExcel(caminho, count));

            string varGuardaUltimoHist = "";
            double valorArq1 = 0;
            double valorArq3 = 0;
            double valorArq4 = 0;
            double valorArq5 = 0;//usada para tratar dados do relatorio 5472/5473/5474/232/233/234/235
            double valorArq6 = 0;
            double valorArq7 = 0;
            double valorArq8 = 0;
            double valorArq9 = 0;
            double valorArq10 = 0;
            double valorArq11 = 0;
            double valorArq12 = 0;
            double valorArq13 = 0;
            int j;
            int aux = 0;
            double[] numeros = new double[716];
            switch (count)
            {
                case "1":


                     for (j = inicio; j < output.Tables[0].Rows.Count; j++)
              
                    {

                        if ((output.Tables[0].Rows[j]["F25"]).ToString().Length != 0)
                        {
                            try
                            {
                                if (output.Tables[0].Rows[j]["F13"].ToString().Length == 0)
                                {
                                    output.Tables[0].Rows[j]["F13"] = varGuardaUltimoHist;

                                }

                                if (output.Tables[0].Rows[j]["F13"].ToString() == "3" || output.Tables[0].Rows[j]["F13"].ToString() == "4" || output.Tables[0].Rows[j]["F13"].ToString() == "5")
                                {
                                    valorArq1 = Math.Round(valorArq1 + Convert.ToDouble((output.Tables[0].Rows[j]["F25"]).ToString().Replace(" ", "").Replace("D", "").Replace("C", "")), 2);
                                    varGuardaUltimoHist = output.Tables[0].Rows[j]["F13"].ToString();
                                }
                                else if (output.Tables[0].Rows[j]["F13"].ToString() == "500" || output.Tables[0].Rows[j]["F13"].ToString() == "5472" || output.Tables[0].Rows[j]["F13"].ToString() == "5473" || output.Tables[0].Rows[j]["F13"].ToString() == "5474" || output.Tables[0].Rows[j]["F13"].ToString() == "232" || output.Tables[0].Rows[j]["F13"].ToString() == "233" || output.Tables[0].Rows[j]["F13"].ToString() == "234" || output.Tables[0].Rows[j]["F13"].ToString() == "235")
                                {
                                    if (output.Tables[0].Rows[j]["F13"].ToString()=="500")
                                    {
                                        if ((output.Tables[0].Rows[j]["F14"].ToString() == "5472" || output.Tables[0].Rows[j]["F14"].ToString() == "5473" || output.Tables[0].Rows[j]["F14"].ToString() == "5474" || output.Tables[0].Rows[j]["F14"].ToString() == "232" || output.Tables[0].Rows[j]["F14"].ToString() == "233" || output.Tables[0].Rows[j]["F14"].ToString() == "234" || output.Tables[0].Rows[j]["F14"].ToString() == "235"))
                                        {
                                            valorArq3 = valorArq3 - Convert.ToDouble((output.Tables[0].Rows[j]["F25"]).ToString().Replace(" ", "").Replace("D", "").Replace("C", ""));
                                        }
                                    }
                                    else
                                    {
                                        valorArq3 = valorArq3 + Convert.ToDouble((output.Tables[0].Rows[j]["F25"]).ToString().Replace(" ", "").Replace("D", "").Replace("C", ""));
                                    }
                                    varGuardaUltimoHist = output.Tables[0].Rows[j]["F13"].ToString();
                                }

                                else if (output.Tables[0].Rows[j]["F13"].ToString() == "821" || output.Tables[0].Rows[j]["F13"].ToString() == "822" || output.Tables[0].Rows[j]["F13"].ToString() == "7286" || output.Tables[0].Rows[j]["F13"].ToString() == "7336" || output.Tables[0].Rows[j]["F13"].ToString() == "847")
                                {
                                    valorArq4 = valorArq4 + Convert.ToDouble((output.Tables[0].Rows[j]["F25"]).ToString().Replace(" ", "").Replace("D", "").Replace("C", ""));
                                    varGuardaUltimoHist = output.Tables[0].Rows[j]["F13"].ToString();
                                }
                                else if (output.Tables[0].Rows[j]["F13"].ToString() == "6" || output.Tables[0].Rows[j]["F13"].ToString() == "192")
                                {
                                    valorArq6 = valorArq6 + Convert.ToDouble((output.Tables[0].Rows[j]["F25"]).ToString().Replace(" ", "").Replace("D", "").Replace("C", ""));
                                    varGuardaUltimoHist = output.Tables[0].Rows[j]["F13"].ToString();

                                }
                                else if (output.Tables[0].Rows[j]["F13"].ToString() == "7027")
                                {
                                    valorArq8 = valorArq8 + Convert.ToDouble((output.Tables[0].Rows[j]["F25"]).ToString().Replace(" ", "").Replace("D", "").Replace("C", ""));
                                    varGuardaUltimoHist = output.Tables[0].Rows[j]["F13"].ToString();

                                }
                                else if (output.Tables[0].Rows[j]["F13"].ToString() == "5761")
                                {
                                    valorArq9 = valorArq9 + Convert.ToDouble((output.Tables[0].Rows[j]["F25"]).ToString().Replace(" ", "").Replace("D", "").Replace("C", ""));
                                    varGuardaUltimoHist = output.Tables[0].Rows[j]["F13"].ToString();

                                }

                                else if (output.Tables[0].Rows[j]["F13"].ToString() == "820")
                                {
                                    valorArq10 = valorArq10 + Convert.ToDouble((output.Tables[0].Rows[j]["F25"]).ToString().Replace(" ", "").Replace("D", "").Replace("C", ""));
                                    varGuardaUltimoHist = output.Tables[0].Rows[j]["F13"].ToString();

                                }

                                else if (output.Tables[0].Rows[j]["F13"].ToString() == "257"|| output.Tables[0].Rows[j]["F13"].ToString() == "258")
                                {
                                    if (output.Tables[0].Rows[j]["F13"].ToString() == "500")
                                    {
                                        if (output.Tables[0].Rows[j]["F14"].ToString() == "257" || output.Tables[0].Rows[j]["F14"].ToString() == "258")
                                        {
                                            valorArq11 = valorArq11 - Convert.ToDouble((output.Tables[0].Rows[j]["F25"]).ToString().Replace(" ", "").Replace("D", "").Replace("C", ""));
                                        }
                                    }
                                    else {

                                        valorArq11 = valorArq11 + Convert.ToDouble((output.Tables[0].Rows[j]["F25"]).ToString().Replace(" ", "").Replace("D", "").Replace("C", ""));
                                    }
                                    varGuardaUltimoHist = output.Tables[0].Rows[j]["F13"].ToString();
                                }
                            }
                            catch { }
                        }
 

                    }
                    somatorio.Add("3/4/5", valorArq1);
                    somatorio.Add("5472/5473/5474/232/233/234/235", valorArq3);
                    somatorio.Add("821/2/7286/7336/847", valorArq4);
                    somatorio.Add("6/192", valorArq6);
                    somatorio.Add("7027", valorArq8);
                    somatorio.Add("5761", valorArq9);
                    somatorio.Add("820", valorArq10);
                    somatorio.Add("257/258", valorArq11);

                    break;

                case "2":
                    for (j = inicio; j < output.Tables[0].Rows.Count; j++)
                    {
                        if ((output.Tables[0].Rows[j]["F16"]).ToString().Length != 0)
                        {
                            if (output.Tables[0].Rows[j]["F16"].ToString() == "SICOOB DIVICRED" || output.Tables[0].Rows[j]["F16"].ToString() == "MX72 MANUFATURA DA MODA LTDA" || output.Tables[0].Rows[j]["F16"].ToString() == "CCLA DA REGIAO CENTRAL E OESTE")
                            {
                                valorArq5 = Math.Round(valorArq5 + Convert.ToDouble((output.Tables[0].Rows[j]["F32"]).ToString().Replace("R", "").Replace("$", "")), 2);
                            }
                        }
                    }
                    somatorio.Add("Arquivo2", valorArq5);

                    break;

                case "3":
                    for (j = inicio; j < output.Tables[0].Rows.Count; j++)
                    {
                        try
                        {
                            string auxlinha="";
                            if (output.Tables[0].Rows[j + 1]["F46"].ToString().Length == 0)
                            {
                                auxlinha = output.Tables[0].Rows[j]["F46"].ToString();
                            }
                            else
                            {
                                auxlinha = output.Tables[0].Rows[j + 1]["F46"].ToString();
                            }

                            if (output.Tables[0].Rows[j]["F33"].ToString() != "403.000.000-2" && auxlinha == "DEV. 39 REJEITADA - REDIGITALIZAR" && output.Tables[0].Rows[j]["F33"].ToString() != "")
                            {
                                valorArq7 = Math.Round(valorArq7 + Convert.ToDouble((output.Tables[0].Rows[j]["F41"])), 2);

                            }

                            
                        }
                        catch
                        {

                        }
                    }

                    somatorio.Add("Arquivo3", valorArq7);
                    break;

                case "4":
                    for (j = inicio -1; j < output.Tables[0].Rows.Count; j++)
                    {
                        try
                        {
                            if (output.Tables[0].Rows[j]["F23"].ToString().Length>0)
                            {
                                double teste = Convert.ToDouble((output.Tables[0].Rows[j]["F28"]));
                                valorArq12 = Math.Round(valorArq12 + Convert.ToDouble((output.Tables[0].Rows[j]["F28"])), 2);

                            }
                        }
                        catch
                        {

                        }
                    }

                    somatorio.Add("Arquivo4", valorArq12);
                    break;

                case "5":
                    
                    valorArq13 = Convert.ToDouble(output.Tables[0].Rows[inicio-1]["F18"]);
                    somatorio.Add("Arquivo5", valorArq13);
                    break;
            }
            return somatorio;
        }

        public string diferenciar(double extrato, double arquivo)
        {
            Dictionary<string, double> diferenca = new Dictionary<string, double>();
            double valorDiferenca = 0;
            string maior = "";
            if (extrato > arquivo)
            {
                valorDiferenca = extrato - arquivo;
                maior = "Extratos.png";
            }
            else if (extrato < arquivo)
            {
                valorDiferenca = arquivo - extrato;
                maior = "Arquivos.png";
            }
            else if (extrato == arquivo)
            {
                valorDiferenca = 0;
                maior = "ok.png";
            }

            return maior + ";" + valorDiferenca.ToString();
        }
    }
}