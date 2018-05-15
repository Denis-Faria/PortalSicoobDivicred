using System;
using System.Collections.Generic;
using System.Globalization;
using PortalSicoobDivicred.Aplicacao;

namespace PortalSicoobDivicred
{
    public class ValidacoesPonto
    {
        public List<List<Dictionary<string, string>>> ValidarPonto(DateTime DataConsulta)
        {
            var Firebird = new QueryFirebird();
            var Funcionarios = Firebird.RetornaListaFuncionario();


            var Pendentes = new List<Dictionary<string, string>>();
            var SemPendencias = new List<Dictionary<string, string>>();
            var DadosRh = new QueryMysqlRh();


            #region Tratamento Ponto Firebird

            for (var i = 0; i < Funcionarios.Count; i++)
            {
                var Afastamentos =
                    Firebird.RetornaListaAfastamentoFuncionario(Funcionarios[i]["ID_FUNCIONARIO"], DataConsulta);
                var Feriados = Firebird.VerificaFeriado(Funcionarios[i]["ID_FUNCIONARIO"]);
                var Marcacao = Firebird.RetornaMarcacao(Funcionarios[i]["ID_FUNCIONARIO"], DataConsulta);
                var ConfirmaMysql = DadosRh.ValidaFirebirdMysql(Funcionarios[i]["ID_FUNCIONARIO"], DataConsulta);
                if (!ConfirmaMysql)
                    if (Afastamentos.Count == 0)
                        if (Feriados[0]["TOTAL"].Equals("0"))
                        {
                            #region Tratamento de falta de marcação

                            if (Marcacao.Count == 0)
                            {
                                var FuncionarioPendente = new Dictionary<string, string>();
                                FuncionarioPendente.Add("IdFuncionario", Funcionarios[i]["ID_FUNCIONARIO"]);
                                FuncionarioPendente.Add("NomeFuncionario", Funcionarios[i]["NOME"]);
                                FuncionarioPendente.Add("DataPendencia", DataConsulta.ToString("dd/MM/yyyy"));
                                FuncionarioPendente.Add("Hora0", "--:--");
                                FuncionarioPendente.Add("Hora1", "--:--");
                                FuncionarioPendente.Add("Hora2", "--:--");
                                FuncionarioPendente.Add("Hora3", "--:--");
                                FuncionarioPendente.Add("TotalHorario", "4");

                                Pendentes.Add(FuncionarioPendente);
                            }

                            #endregion

                            else if (Marcacao.Count < 4 || Marcacao.Count > 4)
                            {
                                #region Tratamento Hora extra, Batida errada aprendiz e estagiario e Irani

                                if (Marcacao[0]["ID_CARGO"].Equals("2") ||
                                    Funcionarios[i]["NOME"].Contains("IRANI") && Marcacao.Count == 2)
                                {
                                    if (Marcacao.Count == 2)
                                    {
                                        var Marcacao1 = DateTime.ParseExact(Marcacao[0]["HORA"], "HH:mm:ss",
                                            new DateTimeFormatInfo());
                                        var Marcacao2 = DateTime.ParseExact(Marcacao[1]["HORA"], "HH:mm:ss",
                                            new DateTimeFormatInfo());
                                        var JornadaTrabalhada = Marcacao2.Subtract(Marcacao1);
                                        var HoraExtra = new TimeSpan();


                                        var Total = new TimeSpan(00, 06, 00, 00);
                                        HoraExtra = JornadaTrabalhada.Subtract(Total);

                                        if (HoraExtra.Hours >= 0 && HoraExtra.Minutes > 5)
                                        {
                                            var FuncionarioPendente = new Dictionary<string, string>();
                                            FuncionarioPendente.Add("IdFuncionario", Funcionarios[i]["ID_FUNCIONARIO"]);
                                            FuncionarioPendente.Add("NomeFuncionario", Funcionarios[i]["NOME"]);
                                            FuncionarioPendente.Add("DataPendencia", Marcacao[0]["DATA"]);
                                            FuncionarioPendente.Add("Hora0", Marcacao[0]["HORA"]);
                                            FuncionarioPendente.Add("Hora1", Marcacao[1]["HORA"]);
                                            FuncionarioPendente.Add("Hora2", "");
                                            FuncionarioPendente.Add("Hora3", "");
                                            FuncionarioPendente.Add("TotalHorario", "4");

                                            Pendentes.Add(FuncionarioPendente);
                                        }
                                    }
                                    else
                                    {
                                        var FuncionarioPendente = new Dictionary<string, string>();
                                        FuncionarioPendente.Add("IdFuncionario", Funcionarios[i]["ID_FUNCIONARIO"]);
                                        FuncionarioPendente.Add("NomeFuncionario", Funcionarios[i]["NOME"]);
                                        FuncionarioPendente.Add("DataPendencia", Marcacao[0]["DATA"]);
                                        FuncionarioPendente.Add("TotalHorario", Marcacao.Count.ToString());

                                        for (var j = 0; j < Marcacao.Count; j++)
                                            FuncionarioPendente.Add("Hora" + j, Marcacao[j]["HORA"]);


                                        Pendentes.Add(FuncionarioPendente);
                                    }
                                }
                                else if (Marcacao[0]["ID_CARGO"].Equals("58"))
                                {
                                    if (Marcacao.Count == 2)
                                    {
                                        var Marcacao1 = DateTime.ParseExact(Marcacao[0]["HORA"], "HH:mm:ss",
                                            new DateTimeFormatInfo());
                                        var Marcacao2 = DateTime.ParseExact(Marcacao[1]["HORA"], "HH:mm:ss",
                                            new DateTimeFormatInfo());

                                        var JornadaTrabalhada = Marcacao2.Subtract(Marcacao1);
                                        var HoraExtra = new TimeSpan();

                                        var Total = new TimeSpan(00, 04, 00, 00);
                                        HoraExtra = JornadaTrabalhada.Subtract(Total);

                                        if (HoraExtra.Hours >= 4 && HoraExtra.Minutes >= 5)
                                        {
                                            var FuncionarioPendente = new Dictionary<string, string>();
                                            FuncionarioPendente.Add("IdFuncionario", Funcionarios[i]["ID_FUNCIONARIO"]);
                                            FuncionarioPendente.Add("NomeFuncionario", Funcionarios[i]["NOME"]);
                                            FuncionarioPendente.Add("DataPendencia", Marcacao[0]["DATA"]);
                                            FuncionarioPendente.Add("Hora0", Marcacao[0]["HORA"]);
                                            FuncionarioPendente.Add("Hora1", Marcacao[1]["HORA"]);
                                            FuncionarioPendente.Add("Hora2", "");
                                            FuncionarioPendente.Add("Hora3", "");
                                            FuncionarioPendente.Add("TotalHorario", "4");

                                            Pendentes.Add(FuncionarioPendente);
                                        }
                                    }
                                    else
                                    {
                                        var FuncionarioPendente = new Dictionary<string, string>();
                                        FuncionarioPendente.Add("IdFuncionario", Funcionarios[i]["ID_FUNCIONARIO"]);
                                        FuncionarioPendente.Add("NomeFuncionario", Funcionarios[i]["NOME"]);
                                        FuncionarioPendente.Add("DataPendencia", Marcacao[0]["DATA"]);
                                        FuncionarioPendente.Add("TotalHorario", Marcacao.Count.ToString());

                                        for (var j = 0; j < Marcacao.Count; j++)
                                            FuncionarioPendente.Add("Hora" + j, Marcacao[j]["HORA"]);


                                        Pendentes.Add(FuncionarioPendente);
                                    }
                                }
                                else
                                {
                                    #region Tratamento Ponto Errado

                                    var Pendencia = new Dictionary<string, string>();
                                    Pendencia.Add("IdFuncionario", Funcionarios[i]["ID_FUNCIONARIO"]);
                                    Pendencia.Add("NomeFuncionario", Funcionarios[i]["NOME"]);
                                    Pendencia.Add("DataPendencia", Marcacao[0]["DATA"]);
                                    Pendencia.Add("TotalHorario", Marcacao.Count.ToString());

                                    for (var j = 0; j < Marcacao.Count; j++)
                                        Pendencia.Add("Hora" + j, Marcacao[j]["HORA"]);

                                    Pendentes.Add(Pendencia);

                                    #endregion
                                }

                                #endregion
                            }
                            else
                            {
                                #region Calculo Hora Extra e Almoço

                                {
                                    var Marcacao1 = DateTime.ParseExact(Marcacao[0]["HORA"], "HH:mm:ss",
                                        new DateTimeFormatInfo());
                                    var Marcacao2 = DateTime.ParseExact(Marcacao[1]["HORA"], "HH:mm:ss",
                                        new DateTimeFormatInfo());
                                    var Marcacao3 = DateTime.ParseExact(Marcacao[2]["HORA"], "HH:mm:ss",
                                        new DateTimeFormatInfo());
                                    var Marcacao4 = DateTime.ParseExact(Marcacao[3]["HORA"], "HH:mm:ss",
                                        new DateTimeFormatInfo());

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
                                        FuncionarioPendente.Add("Hora0", Marcacao[0]["HORA"]);
                                        FuncionarioPendente.Add("Hora1", Marcacao[1]["HORA"]);
                                        FuncionarioPendente.Add("Hora2", Marcacao[2]["HORA"]);
                                        FuncionarioPendente.Add("Hora3", Marcacao[3]["HORA"]);
                                        FuncionarioPendente.Add("TotalHorario", "4");

                                        Pendentes.Add(FuncionarioPendente);
                                    }
                                    else if (Almoco.Hours >= 2 && Almoco.Minutes > 0)
                                    {
                                        var FuncionarioPendente = new Dictionary<string, string>();
                                        FuncionarioPendente.Add("IdFuncionario", Funcionarios[i]["ID_FUNCIONARIO"]);
                                        FuncionarioPendente.Add("NomeFuncionario", Funcionarios[i]["NOME"]);
                                        FuncionarioPendente.Add("DataPendencia", Marcacao[0]["DATA"]);
                                        FuncionarioPendente.Add("Hora0", Marcacao[0]["HORA"]);
                                        FuncionarioPendente.Add("Hora1", Marcacao[1]["HORA"]);
                                        FuncionarioPendente.Add("Hora2", Marcacao[2]["HORA"]);
                                        FuncionarioPendente.Add("Hora3", Marcacao[3]["HORA"]);
                                        FuncionarioPendente.Add("TotalHorario", "4");

                                        Pendentes.Add(FuncionarioPendente);
                                    }
                                    else if (Almoco.Hours < 1 && Almoco.Minutes <= 59)
                                    {
                                        var FuncionarioPendente = new Dictionary<string, string>();
                                        FuncionarioPendente.Add("IdFuncionario", Funcionarios[i]["ID_FUNCIONARIO"]);
                                        FuncionarioPendente.Add("NomeFuncionario", Funcionarios[i]["NOME"]);
                                        FuncionarioPendente.Add("DataPendencia", Marcacao[0]["DATA"]);
                                        FuncionarioPendente.Add("Hora0", Marcacao[0]["HORA"]);
                                        FuncionarioPendente.Add("Hora1", Marcacao[1]["HORA"]);
                                        FuncionarioPendente.Add("Hora2", Marcacao[2]["HORA"]);
                                        FuncionarioPendente.Add("Hora3", Marcacao[3]["HORA"]);
                                        FuncionarioPendente.Add("TotalHorario", "4");

                                        Pendentes.Add(FuncionarioPendente);
                                    }

                                    #region Tratamento sem justificativa

                                    else
                                    {
                                        var Funcionario = new Dictionary<string, string>();
                                        Funcionario.Add("IdFuncionario", Funcionarios[i]["ID_FUNCIONARIO"]);
                                        Funcionario.Add("NomeFuncionario", Funcionarios[i]["NOME"]);
                                        Funcionario.Add("DataPendencia", Marcacao[0]["DATA"]);
                                        Funcionario.Add("Hora0", Marcacao[0]["HORA"]);
                                        Funcionario.Add("Hora1", Marcacao[1]["HORA"]);
                                        Funcionario.Add("Hora2", Marcacao[2]["HORA"]);
                                        Funcionario.Add("Hora3", Marcacao[3]["HORA"]);
                                        Funcionario.Add("Jornada", JornadaTrabalhada.ToString());
                                        Funcionario.Add("HoraExtra", HoraExtra.ToString());


                                        SemPendencias.Add(Funcionario);
                                    }

                                    #endregion
                                }

                                #endregion
                            }
                        }
            }

            #endregion

            var ListaFinal = new List<List<Dictionary<string, string>>>();
            ListaFinal.Add(Pendentes);
            ListaFinal.Add(SemPendencias);

            return ListaFinal;
        }

        public List<Dictionary<string, string>> RetornaPendenciasMysql()
        {
            var FaltaConfirmacao = new List<Dictionary<string, string>>();
            var DadosRh = new QueryMysqlRh();
            var Firebird = new QueryFirebird();

            var TodasPendencias = DadosRh.RetornaPendencias();

            for (var i = 0; i < TodasPendencias.Count; i++)
            {
                var DadosPendencia = DadosRh.RetornaDadosPendencias(TodasPendencias[i]["id"]);

                var Confirmar = new Dictionary<string, string>();
                if (TodasPendencias[i]["validacaogestor"].Equals("S"))
                    Confirmar.Add("ConfirmaGestor", "green");
                else
                    Confirmar.Add("ConfirmaGestor", "red");
                Confirmar.Add("IdPendencia", TodasPendencias[i]["id"]);
                Confirmar.Add("Nome", TodasPendencias[i]["nome"]);
                Confirmar.Add("Data", TodasPendencias[i]["data"]);
                Confirmar.Add("TotalHorario", DadosPendencia.Count.ToString());
                var Validado = false;
                for (var j = 0; j < DadosPendencia.Count; j++)
                    if (DadosPendencia[j]["idjustificativafirebird"].Equals("0") &&
                        DadosPendencia[j]["observacao"] == null)
                    {
                        Confirmar.Add("Horario" + j, DadosPendencia[j]["horario"]);
                    }
                    else
                    {
                        try
                        {
                            if (DadosPendencia[j]["observacao"].Length > 0)
                            {
                                var Justificativa = DadosPendencia[j]["observacao"];
                                try
                                {
                                    Confirmar.Add("Justificativa" + i, Justificativa);
                                }
                                catch
                                {
                                }
                            }
                            else
                            {
                                var Justificativa =
                                    Firebird.RecuperaJustificativasFuncioanrio(
                                        DadosPendencia[j]["idjustificativafirebird"]);
                                try
                                {
                                    Confirmar.Add("Justificativa" + i, Justificativa[0]["DESCRICAO"]);
                                }
                                catch
                                {
                                }
                            }
                        }
                        catch
                        {
                            var Justificativa =
                                Firebird.RecuperaJustificativasFuncioanrio(
                                    DadosPendencia[j]["idjustificativafirebird"]);
                            try
                            {
                                Confirmar.Add("Justificativa" + i, Justificativa[0]["DESCRICAO"]);
                            }
                            catch
                            {
                            }
                        }

                        Validado = true;
                        Confirmar.Add("Horario" + j, DadosPendencia[j]["horario"]);
                    }

                if (Validado)
                    Confirmar.Add("Justificado", "true");
                else
                    Confirmar.Add("Justificado", "false");

                FaltaConfirmacao.Add(Confirmar);
            }

            return FaltaConfirmacao;
        }

        public List<Dictionary<string, string>> RetornaPendenciasFuncionario(string IdFuncionario)
        {
            var FaltaConfirmacao = new List<Dictionary<string, string>>();

            var DadosRh = new QueryMysqlRh();
            var QueryFire = new QueryFirebird();
            var TodasPendencias = DadosRh.RetornaPendenciasUsuario(IdFuncionario);

            for (var i = 0; i < TodasPendencias.Count; i++)
            {
                var DadosPendencia = DadosRh.RetornaDadosPendencias(TodasPendencias[i]["id"]);
                var IdFuncionarioFirebird = QueryFire.RetornaIdFuncionario(TodasPendencias[i]["nome"]);

                var Confirmar = new Dictionary<string, string>();
                if (TodasPendencias[i]["validacaogestor"].Equals("S"))
                    Confirmar.Add("ConfirmaGestor", "true");
                else
                    Confirmar.Add("ConfirmaGestor", "false");
                Confirmar.Add("IdPendencia", TodasPendencias[i]["id"]);
                Confirmar.Add("Nome", TodasPendencias[i]["nome"]);
                Confirmar.Add("IdFuncionarioFireBird", IdFuncionarioFirebird[0]["ID_FUNCIONARIO"]);
                Confirmar.Add("Data", TodasPendencias[i]["data"]);
                Confirmar.Add("TotalHorario", DadosPendencia.Count.ToString());
                var Validado = false;
                for (var j = 0; j < DadosPendencia.Count; j++)
                    if (DadosPendencia[j]["idjustificativafirebird"].Equals("0") &&
                        DadosPendencia[j]["observacao"] == null)
                    {
                        Confirmar.Add("Horario" + j, DadosPendencia[j]["horario"]);
                    }
                    else
                    {
                        Validado = true;
                        Confirmar.Add("Horario" + j, DadosPendencia[j]["horario"]);
                    }

                if (Validado)
                    Confirmar.Add("Justificado", "true");
                else
                    Confirmar.Add("Justificado", "false");

                FaltaConfirmacao.Add(Confirmar);
            }

            return FaltaConfirmacao;
        }

        public List<Dictionary<string, string>> RetornaPendenciasSetor(string IdSetor)
        {
            var FaltaConfirmacao = new List<Dictionary<string, string>>();

            var DadosRh = new QueryMysqlRh();
            var QueryFire = new QueryFirebird();
            var TodasPendencias = DadosRh.RetornaPendenciasSetor(IdSetor);

            for (var i = 0; i < TodasPendencias.Count; i++)
            {
                var DadosPendencia = DadosRh.RetornaDadosPendencias(TodasPendencias[i]["id"]);
                var IdFuncionarioFirebird = QueryFire.RetornaIdFuncionario(TodasPendencias[i]["nome"]);

                var Confirmar = new Dictionary<string, string>();
                if (TodasPendencias[i]["validacaogestor"].Equals("S"))
                    Confirmar.Add("ConfirmaGestor", "true");
                else
                    Confirmar.Add("ConfirmaGestor", "false");
                Confirmar.Add("IdPendencia", TodasPendencias[i]["id"]);
                Confirmar.Add("Nome", TodasPendencias[i]["nome"]);
                Confirmar.Add("IdFuncionarioFireBird", IdFuncionarioFirebird[0]["ID_FUNCIONARIO"]);
                Confirmar.Add("Data", TodasPendencias[0]["data"]);
                Confirmar.Add("TotalHorario", DadosPendencia.Count.ToString());
                var Validado = false;

                for (var j = 0; j < DadosPendencia.Count; j++)
                    if (DadosPendencia[j]["idjustificativafirebird"].Equals("0") &&
                        DadosPendencia[j]["observacao"] == null)
                    {
                        Confirmar.Add("Horario" + j, DadosPendencia[j]["horario"]);
                    }
                    else
                    {
                        Validado = true;
                        if (DadosPendencia[j]["observacao"] != null)
                        {
                            if (DadosPendencia[j]["observacao"].Length > 0)
                            {
                                var Justificativa = DadosPendencia[j]["observacao"];
                                try
                                {
                                    Confirmar.Add("Justificativa" + i, Justificativa);
                                }
                                catch
                                {
                                }
                            }
                            else
                            {
                                var Justificativa =
                                    QueryFire.RecuperaJustificativasFuncioanrio(
                                        DadosPendencia[j]["idjustificativafirebird"]);
                                try
                                {
                                    Confirmar.Add("Justificativa" + i, Justificativa[0]["DESCRICAO"]);
                                }
                                catch
                                {
                                }
                            }
                        }
                        else
                        {
                            var Justificativa =
                                QueryFire.RecuperaJustificativasFuncioanrio(
                                    DadosPendencia[j]["idjustificativafirebird"]);
                            try
                            {
                                Confirmar.Add("Justificativa" + i, Justificativa[0]["DESCRICAO"]);
                            }
                            catch
                            {
                            }
                        }

                        Confirmar.Add("Horario" + j, DadosPendencia[j]["horario"]);
                    }

                if (Validado)
                    Confirmar.Add("Justificado", "true");
                else
                    Confirmar.Add("Justificado", "false");

                FaltaConfirmacao.Add(Confirmar);
            }

            return FaltaConfirmacao;
        }

        public List<Dictionary<string, string>> RetornaHistoricoPendencias(string DataInicial, string DataFinal)
        {
            var FaltaConfirmacao = new List<Dictionary<string, string>>();

            var DadosRh = new QueryMysqlRh();
            var Firebird = new QueryFirebird();
            var TodasPendencias = DadosRh.RetornaHistoricoPendencias(DataInicial, DataFinal);

            for (var i = 0; i < TodasPendencias.Count; i++)
            {
                var DadosPendencia = DadosRh.RetornaDadosPendencias(TodasPendencias[i]["id"]);

                var Confirmar = new Dictionary<string, string>();

                Confirmar.Add("IdPendencia", TodasPendencias[i]["id"]);
                Confirmar.Add("Nome", TodasPendencias[i]["nome"]);
                Confirmar.Add("Data", TodasPendencias[i]["data"]);
                Confirmar.Add("TotalHorario", DadosPendencia.Count.ToString());
                var Validado = false;
                for (var j = 0; j < DadosPendencia.Count; j++)
                    if (DadosPendencia[j]["idjustificativafirebird"].Equals("0"))
                    {
                        Confirmar.Add("Horario" + j, DadosPendencia[j]["horario"]);
                    }
                    else
                    {
                        var Justificativa =
                            Firebird.RecuperaJustificativasFuncioanrio(DadosPendencia[j]["idjustificativafirebird"]);
                        try
                        {
                            Confirmar.Add("Justificativa" + i, Justificativa[0]["DESCRICAO"]);
                        }
                        catch
                        {
                        }

                        Validado = true;
                        Confirmar.Add("Horario" + j, DadosPendencia[j]["horario"]);
                    }

                if (Validado)
                    Confirmar.Add("Justificado", "true");
                else
                    Confirmar.Add("Justificado", "false");

                FaltaConfirmacao.Add(Confirmar);
            }

            return FaltaConfirmacao;
        }
    }
}