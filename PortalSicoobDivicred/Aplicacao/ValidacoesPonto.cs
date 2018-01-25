using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using PortalSicoobDivicred.Aplicacao;

namespace PortalSicoobDivicred
{
    public class ValidacoesPonto
    {
        public List<List<Dictionary<string, string>>> ValidarPonto()
        {
            var Firebird = new QueryFirebird();
            var Funcionarios = Firebird.RetornaListaFuncionario();


            var Pendentes = new List<Dictionary<string, string>>();
            var SemPendencias = new List<Dictionary<string, string>>();
            var FaltaConfirmacao = new List<Dictionary<string, string>>();


            #region Tratamento Ponto Firebird
            for (int i = 0; i < Funcionarios.Count; i++)
            {
                var Afastamentos = Firebird.RetornaListaAfastamentoFuncionario(Funcionarios[i]["ID_FUNCIONARIO"]);
                var Feriados = Firebird.VerificaFeriado(Funcionarios[i]["ID_FUNCIONARIO"]);
                var Marcacao = Firebird.RetornaMarcacao(Funcionarios[i]["ID_FUNCIONARIO"]);
                if (Afastamentos.Count == 0)
                {
                    if (Feriados[0]["TOTAL"].Equals("0"))
                    {
                        if (Marcacao.Count < 4 || Marcacao.Count > 4)
                        {
                            #region Tratamento Hora extra, Batida errada aprendiz e estagiario e Irani

                            if (Marcacao[0]["ID_CARGO"].Equals("2") ||
                                Funcionarios[i]["NOME"].Contains("IRANI") && Marcacao.Count == 2)
                            {
                                var Marcacao1 = DateTime.ParseExact(Marcacao[0]["HORA"], "HH:mm:ss",
                                    new DateTimeFormatInfo());
                                var Marcacao2 = DateTime.ParseExact(Marcacao[1]["HORA"], "HH:mm:ss",
                                    new DateTimeFormatInfo());
                                var JornadaTrabalhada = Marcacao2.Subtract(Marcacao1);
                                var HoraExtra = new TimeSpan();


                                var Total = new TimeSpan(00, 06, 00, 00);
                                HoraExtra = JornadaTrabalhada.Subtract(Total);
                                if (HoraExtra.Hours > 6)
                                {
                                    var FuncionarioPendente = new Dictionary<string, string>();
                                    FuncionarioPendente.Add("IdFuncionario", Funcionarios[i]["ID_FUNCIONARIO"]);
                                    FuncionarioPendente.Add("NomeFuncionario", Funcionarios[i]["NOME"]);
                                    FuncionarioPendente.Add("DataPendencia", Marcacao[0]["DATA"]);
                                    FuncionarioPendente.Add("Hora1", Marcacao[0]["HORA"]);
                                    FuncionarioPendente.Add("Hora2", Marcacao[1]["HORA"]);
                                    FuncionarioPendente.Add("Hora3", "");
                                    FuncionarioPendente.Add("Hora4", "");

                                    Pendentes.Add(FuncionarioPendente);
                                }
                            }
                            else
                            {
                                var FuncionarioPendente = new Dictionary<string, string>();
                                FuncionarioPendente.Add("IdFuncionario", Funcionarios[i]["ID_FUNCIONARIO"]);
                                FuncionarioPendente.Add("NomeFuncionario", Funcionarios[i]["NOME"]);
                                FuncionarioPendente.Add("DataPendencia", Marcacao[0]["DATA"]);
                                FuncionarioPendente.Add("Hora1", Marcacao[0]["HORA"]);
                                FuncionarioPendente.Add("Hora2", Marcacao[1]["HORA"]);
                                FuncionarioPendente.Add("Hora3", Marcacao[2]["HORA"]);
                                FuncionarioPendente.Add("Hora4", "");

                                Pendentes.Add(FuncionarioPendente);
                            }
                            if (Marcacao[0]["ID_CARGO"].Equals("58") && Marcacao.Count == 2)
                            {
                                var Marcacao1 = DateTime.ParseExact(Marcacao[0]["HORA"], "HH:mm:ss",
                                    new DateTimeFormatInfo());
                                var Marcacao2 = DateTime.ParseExact(Marcacao[1]["HORA"], "HH:mm:ss",
                                    new DateTimeFormatInfo());

                                var JornadaTrabalhada = Marcacao2.Subtract(Marcacao1);
                                var HoraExtra = new TimeSpan();

                                var Total = new TimeSpan(00, 04, 00, 00);
                                HoraExtra = JornadaTrabalhada.Subtract(Total);
                                if (HoraExtra.Hours > 4)
                                {
                                    var FuncionarioPendente = new Dictionary<string, string>();
                                    FuncionarioPendente.Add("IdFuncionario", Funcionarios[i]["ID_FUNCIONARIO"]);
                                    FuncionarioPendente.Add("NomeFuncionario", Funcionarios[i]["NOME"]);
                                    FuncionarioPendente.Add("DataPendencia", Marcacao[0]["DATA"]);
                                    FuncionarioPendente.Add("Hora1", Marcacao[0]["HORA"]);
                                    FuncionarioPendente.Add("Hora2", Marcacao[1]["HORA"]);
                                    FuncionarioPendente.Add("Hora3", "");
                                    FuncionarioPendente.Add("Hora4", "");

                                    Pendentes.Add(FuncionarioPendente);
                                }
                            }
                            else
                            {
                                var FuncionarioPendente = new Dictionary<string, string>();
                                FuncionarioPendente.Add("IdFuncionario", Funcionarios[i]["ID_FUNCIONARIO"]);
                                FuncionarioPendente.Add("NomeFuncionario", Funcionarios[i]["NOME"]);
                                FuncionarioPendente.Add("DataPendencia", Marcacao[0]["DATA"]);
                                FuncionarioPendente.Add("Hora1", Marcacao[0]["HORA"]);
                                FuncionarioPendente.Add("Hora2", Marcacao[1]["HORA"]);
                                FuncionarioPendente.Add("Hora3", Marcacao[2]["HORA"]);
                                FuncionarioPendente.Add("Hora4", "");

                                Pendentes.Add(FuncionarioPendente);
                            }
                            #endregion

                            #region Tratamento Ponto Errado

                            var Pendencia = new Dictionary<string, string>();
                            Pendencia.Add("IdFuncionario", Funcionarios[i]["ID_FUNCIONARIO"]);
                            Pendencia.Add("NomeFuncionario", Funcionarios[i]["NOME"]);
                            Pendencia.Add("DataPendencia", Marcacao[0]["DATA"]);

                            for (int j = 1; j <= Marcacao.Count; j++)
                            {
                                Pendencia.Add("Hora" + j, Marcacao[j]["HORA"]);
                            }

                            Pendentes.Add(Pendencia);

                            #endregion
                        }

                        #region Tratamento de falta de marcação
                        if (Marcacao.Count == 0)
                        {
                            var FuncionarioPendente = new Dictionary<string, string>();
                            FuncionarioPendente.Add("IdFuncionario", Funcionarios[i]["ID_FUNCIONARIO"]);
                            FuncionarioPendente.Add("NomeFuncionario", Funcionarios[i]["NOME"]);
                            FuncionarioPendente.Add("DataPendencia", Marcacao[0]["DATA"]);
                            FuncionarioPendente.Add("Hora1", "--:--");
                            FuncionarioPendente.Add("Hora2", "--:--");
                            FuncionarioPendente.Add("Hora3", "--:--");
                            FuncionarioPendente.Add("Hora4", "--:--");

                            Pendentes.Add(FuncionarioPendente);
                        }
                        #endregion
                        else
                        {
                            #region Calculo Hora Extra e Almoço

                            {
                                var Marcacao1 = DateTime.ParseExact(Marcacao[0]["HORA"], "HH:mm:ss", new DateTimeFormatInfo());
                                var Marcacao2 = DateTime.ParseExact(Marcacao[1]["HORA"], "HH:mm:ss", new DateTimeFormatInfo());
                                var Marcacao3 = DateTime.ParseExact(Marcacao[2]["HORA"], "HH:mm:ss", new DateTimeFormatInfo());
                                var Marcacao4 = DateTime.ParseExact(Marcacao[3]["HORA"], "HH:mm:ss", new DateTimeFormatInfo());

                                var Almoco = Marcacao3.Subtract(Marcacao2);

                                var JornadaTrabalhada = Marcacao4.Subtract(Marcacao1).Subtract(Almoco);
                                var HoraExtra = new TimeSpan();
                                var Total = new TimeSpan(00, 08, 00, 00);
                                HoraExtra = JornadaTrabalhada.Subtract(Total);


                                if (HoraExtra.Hours > 2)
                                {
                                    var FuncionarioPendente = new Dictionary<string, string>();
                                    FuncionarioPendente.Add("IdFuncionario", Funcionarios[i]["ID_FUNCIONARIO"]);
                                    FuncionarioPendente.Add("NomeFuncionario", Funcionarios[i]["NOME"]);
                                    FuncionarioPendente.Add("DataPendencia", Marcacao[0]["DATA"]);
                                    FuncionarioPendente.Add("Hora1", Marcacao[0]["HORA"]);
                                    FuncionarioPendente.Add("Hora2", Marcacao[1]["HORA"]);
                                    FuncionarioPendente.Add("Hora3", Marcacao[2]["HORA"]);
                                    FuncionarioPendente.Add("Hora4", Marcacao[3]["HORA"]);

                                    Pendentes.Add(FuncionarioPendente);
                                }
                                else if (Almoco.Hours >= 2 && Almoco.Minutes > 0)
                                {
                                    var FuncionarioPendente = new Dictionary<string, string>();
                                    FuncionarioPendente.Add("IdFuncionario", Funcionarios[i]["ID_FUNCIONARIO"]);
                                    FuncionarioPendente.Add("NomeFuncionario", Funcionarios[i]["NOME"]);
                                    FuncionarioPendente.Add("DataPendencia", Marcacao[0]["DATA"]);
                                    FuncionarioPendente.Add("Hora1", Marcacao[0]["HORA"]);
                                    FuncionarioPendente.Add("Hora2", Marcacao[1]["HORA"]);
                                    FuncionarioPendente.Add("Hora3", Marcacao[2]["HORA"]);
                                    FuncionarioPendente.Add("Hora4", Marcacao[3]["HORA"]);

                                    Pendentes.Add(FuncionarioPendente);
                                }
                                else if (Almoco.Hours < 1)
                                {
                                    var FuncionarioPendente = new Dictionary<string, string>();
                                    FuncionarioPendente.Add("IdFuncionario", Funcionarios[i]["ID_FUNCIONARIO"]);
                                    FuncionarioPendente.Add("NomeFuncionario", Funcionarios[i]["NOME"]);
                                    FuncionarioPendente.Add("DataPendencia", Marcacao[0]["DATA"]);
                                    FuncionarioPendente.Add("Hora1", Marcacao[0]["HORA"]);
                                    FuncionarioPendente.Add("Hora2", Marcacao[1]["HORA"]);
                                    FuncionarioPendente.Add("Hora3", Marcacao[2]["HORA"]);
                                    FuncionarioPendente.Add("Hora4", Marcacao[3]["HORA"]);

                                    Pendentes.Add(FuncionarioPendente);
                                }

                                #region Tratamento sem justificativa
                                else
                                {
                                    var Funcionario = new Dictionary<string, string>();
                                    Funcionario.Add("IdFuncionario", Funcionarios[i]["ID_FUNCIONARIO"]);
                                    Funcionario.Add("NomeFuncionario", Funcionarios[i]["NOME"]);
                                    Funcionario.Add("DataPendencia", Marcacao[0]["DATA"]);
                                    Funcionario.Add("Hora1", Marcacao[0]["HORA"]);
                                    Funcionario.Add("Hora2", Marcacao[1]["HORA"]);
                                    Funcionario.Add("Hora3", Marcacao[2]["HORA"]);
                                    Funcionario.Add("Hora4", Marcacao[3]["HORA"]);
                                    Funcionario.Add("Jornada", JornadaTrabalhada.ToString("HH:MM:SS"));
                                    Funcionario.Add("HoraExtra", HoraExtra.ToString("HH:MM:SS"));


                                    SemPendencias.Add(Funcionario);
                                }


                                #endregion

                            }

                            #endregion
                        }
                    }
                }
            }
            #endregion

            #region Tratamento Pendencias MYSQL
            var DadosRh = new QueryMysqlRh();
            var TodasPendencias = DadosRh.RetornaPendencias();

            for (int i = 0; i < TodasPendencias.Count; i++)
            {
                var DadosPendencia = DadosRh.RetornaDadosPendencias(TodasPendencias[i]["id"]);

                var Confirmar = new Dictionary<string,string>();
                Confirmar.Add("Nome",TodasPendencias[i]["nome"]);
                for (int j = 1; j <= DadosPendencia.Count; j++)
                {
                    
                }
                FaltaConfirmacao.Add(Confirmar);

            }

            #endregion

            var ListaFinal = new List<List<Dictionary<string,string>>>();
            ListaFinal.Add(Pendentes);
            ListaFinal.Add(SemPendencias);
            ListaFinal.Add(FaltaConfirmacao);

            return ListaFinal;

        }
    }
}