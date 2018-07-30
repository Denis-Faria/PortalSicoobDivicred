using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace PortalSicoobDivicred.Aplicacao
{
    public class ValidacoesPonto
    {
        public List<List<Dictionary<string, string>>> ValidarPonto(DateTime dataConsulta)
        {
            var firebird = new QueryFirebird();
            var funcionarios = firebird.RetornaListaFuncionario();


            var pendentes = new List<Dictionary<string, string>>();
            var semPendencias = new List<Dictionary<string, string>>();
            var dadosRh = new QueryMysqlRh();


            #region Tratamento Ponto Firebird

            for (var i = 0; i < funcionarios.Count; i++)
            {
                var afastamentos =
                    firebird.RetornaListaAfastamentoFuncionario(funcionarios[i]["ID_FUNCIONARIO"], dataConsulta);
                var id = funcionarios[i]["ID_FUNCIONARIO"];
                var admissao = firebird.RetornaDataAdmissao(funcionarios[i]["ID_FUNCIONARIO"], dataConsulta);

                var marcacao = firebird.RetornaMarcacao(funcionarios[i]["ID_FUNCIONARIO"], dataConsulta);
                var confirmaMysql = dadosRh.ValidaFirebirdMysql(funcionarios[i]["ID_FUNCIONARIO"], dataConsulta);
                if (!confirmaMysql)
                    if (admissao.Count > 0)
                    {
                        var calendarioFuncionario =
                            firebird.VerificaCalendario(funcionarios[i]["ID_FUNCIONARIO"], dataConsulta);
                        var feriados = firebird.VerificaFeriado(funcionarios[i]["ID_FUNCIONARIO"],
                            calendarioFuncionario[0]["ID_CALENDARIO"], dataConsulta);

                        if (afastamentos.Count == 0)
                            if (feriados[0]["TOTAL"].Equals("0"))
                            {
                                var tabelaJornada = firebird.BuscaJornada(
                                    funcionarios[i]["ID_FUNCIONARIO"],
                                    dataConsulta.ToString("yyyy/MM/dd"));

                                // Trata Jornada especial
                                if (tabelaJornada.Count > 0 &&
                                    tabelaJornada[0]["ID_JORNADA"].Equals("3"))
                                {
                                    if (marcacao.Count == 0)
                                    {
                                        var funcionarioPendente = new Dictionary<string, string>();
                                        funcionarioPendente.Add("IdFuncionario", funcionarios[i]["ID_FUNCIONARIO"]);
                                        funcionarioPendente.Add("NomeFuncionario", funcionarios[i]["NOME"]);
                                        funcionarioPendente.Add("DataPendencia", dataConsulta.ToString("dd/MM/yyyy"));
                                        funcionarioPendente.Add("Hora0", "--:--");
                                        funcionarioPendente.Add("Hora1", "--:--");
                                        funcionarioPendente.Add("Hora2", "--:--");
                                        funcionarioPendente.Add("Hora3", "--:--");
                                        funcionarioPendente.Add("TotalHorario", "4");

                                        pendentes.Add(funcionarioPendente);
                                    }
                                    else if ((marcacao.Count <= 2 || marcacao.Count > 4) && marcacao.Count != 3)
                                    {
                                        #region Tratamento Hora extra, Batida errada aprendiz e estagiario e Irani

                                        if (marcacao[0]["ID_CARGO"].Equals("2") ||
                                            funcionarios[i]["NOME"].Contains("IRANI") && marcacao.Count == 2)
                                        {
                                            if (marcacao.Count == 2)
                                            {
                                                var marcacao1 = DateTime.ParseExact(marcacao[0]["HORA"], "HH:mm:ss",
                                                    new DateTimeFormatInfo());
                                                var marcacao2 = DateTime.ParseExact(marcacao[1]["HORA"], "HH:mm:ss",
                                                    new DateTimeFormatInfo());
                                                var jornadaTrabalhada = marcacao2.Subtract(marcacao1);
                                                TimeSpan horaExtra;


                                                var total = new TimeSpan(00, 06, 00, 00);
                                                horaExtra = jornadaTrabalhada.Subtract(total);

                                                if (horaExtra.Hours >= 0 && horaExtra.Minutes > 5)
                                                {
                                                    var funcionarioPendente = new Dictionary<string, string>();
                                                    funcionarioPendente.Add("IdFuncionario",
                                                        funcionarios[i]["ID_FUNCIONARIO"]);
                                                    funcionarioPendente.Add("NomeFuncionario", funcionarios[i]["NOME"]);
                                                    funcionarioPendente.Add("DataPendencia", marcacao[0]["DATA"]);
                                                    funcionarioPendente.Add("Hora0", marcacao[0]["HORA"]);
                                                    funcionarioPendente.Add("Hora1", marcacao[1]["HORA"]);
                                                    funcionarioPendente.Add("Hora2", "");
                                                    funcionarioPendente.Add("Hora3", "");
                                                    funcionarioPendente.Add("TotalHorario", "4");

                                                    pendentes.Add(funcionarioPendente);
                                                }
                                            }
                                            else
                                            {
                                                var funcionarioPendente = new Dictionary<string, string>();
                                                funcionarioPendente.Add("IdFuncionario",
                                                    funcionarios[i]["ID_FUNCIONARIO"]);
                                                funcionarioPendente.Add("NomeFuncionario", funcionarios[i]["NOME"]);
                                                funcionarioPendente.Add("DataPendencia", marcacao[0]["DATA"]);
                                                funcionarioPendente.Add("TotalHorario", marcacao.Count.ToString());

                                                for (var j = 0; j < marcacao.Count; j++)
                                                    funcionarioPendente.Add("Hora" + j, marcacao[j]["HORA"]);


                                                pendentes.Add(funcionarioPendente);
                                            }
                                        }
                                        else if (marcacao[0]["ID_CARGO"].Equals("58"))
                                        {
                                            if (marcacao.Count == 2)
                                            {
                                                var marcacao1 = DateTime.ParseExact(marcacao[0]["HORA"], "HH:mm:ss",
                                                    new DateTimeFormatInfo());
                                                var marcacao2 = DateTime.ParseExact(marcacao[1]["HORA"], "HH:mm:ss",
                                                    new DateTimeFormatInfo());

                                                var jornadaTrabalhada = marcacao2.Subtract(marcacao1);
                                                TimeSpan horaExtra;

                                                var total = new TimeSpan(00, 04, 00, 00);
                                                horaExtra = jornadaTrabalhada.Subtract(total);

                                                if (horaExtra.Hours >= 4 && horaExtra.Minutes >= 5)
                                                {
                                                    var funcionarioPendente = new Dictionary<string, string>();
                                                    funcionarioPendente.Add("IdFuncionario",
                                                        funcionarios[i]["ID_FUNCIONARIO"]);
                                                    funcionarioPendente.Add("NomeFuncionario", funcionarios[i]["NOME"]);
                                                    funcionarioPendente.Add("DataPendencia", marcacao[0]["DATA"]);
                                                    funcionarioPendente.Add("Hora0", marcacao[0]["HORA"]);
                                                    funcionarioPendente.Add("Hora1", marcacao[1]["HORA"]);
                                                    funcionarioPendente.Add("Hora2", "");
                                                    funcionarioPendente.Add("Hora3", "");
                                                    funcionarioPendente.Add("TotalHorario", "4");

                                                    pendentes.Add(funcionarioPendente);
                                                }
                                            }
                                            else
                                            {
                                                var funcionarioPendente = new Dictionary<string, string>();
                                                funcionarioPendente.Add("IdFuncionario",
                                                    funcionarios[i]["ID_FUNCIONARIO"]);
                                                funcionarioPendente.Add("NomeFuncionario", funcionarios[i]["NOME"]);
                                                funcionarioPendente.Add("DataPendencia", marcacao[0]["DATA"]);
                                                funcionarioPendente.Add("TotalHorario", marcacao.Count.ToString());

                                                for (var j = 0; j < marcacao.Count; j++)
                                                    funcionarioPendente.Add("Hora" + j, marcacao[j]["HORA"]);


                                                pendentes.Add(funcionarioPendente);
                                            }
                                        }
                                        else
                                        {
                                            #region Tratamento Ponto Errado

                                            var pendencia = new Dictionary<string, string>();
                                            pendencia.Add("IdFuncionario", funcionarios[i]["ID_FUNCIONARIO"]);
                                            pendencia.Add("NomeFuncionario", funcionarios[i]["NOME"]);
                                            pendencia.Add("DataPendencia", marcacao[0]["DATA"]);
                                            pendencia.Add("TotalHorario", marcacao.Count.ToString());

                                            for (var j = 0; j < marcacao.Count; j++)
                                                pendencia.Add("Hora" + j, marcacao[j]["HORA"]);

                                            pendentes.Add(pendencia);

                                            #endregion
                                        }

                                        #endregion
                                    }
                                    else if(marcacao.Count == 3 )
                                    {
                                        if(marcacao[0]["ID_CARGO"].Equals( "2" ) ||
                                            funcionarios[i]["NOME"].Contains( "IRANI" ) && marcacao.Count == 2)
                                        {
                                            if(marcacao.Count == 2)
                                            {
                                                var marcacao1 = DateTime.ParseExact( marcacao[0]["HORA"], "HH:mm:ss",
                                                    new DateTimeFormatInfo() );
                                                var marcacao2 = DateTime.ParseExact( marcacao[1]["HORA"], "HH:mm:ss",
                                                    new DateTimeFormatInfo() );
                                                var jornadaTrabalhada = marcacao2.Subtract( marcacao1 );
                                                TimeSpan horaExtra;


                                                var total = new TimeSpan( 00, 06, 00, 00 );
                                                horaExtra = jornadaTrabalhada.Subtract( total );

                                                if(horaExtra.Hours >= 0 && horaExtra.Minutes > 5)
                                                {
                                                    var funcionarioPendente = new Dictionary<string, string>();
                                                    funcionarioPendente.Add( "IdFuncionario",
                                                        funcionarios[i]["ID_FUNCIONARIO"] );
                                                    funcionarioPendente.Add( "NomeFuncionario", funcionarios[i]["NOME"] );
                                                    funcionarioPendente.Add( "DataPendencia", marcacao[0]["DATA"] );
                                                    funcionarioPendente.Add( "Hora0", marcacao[0]["HORA"] );
                                                    funcionarioPendente.Add( "Hora1", marcacao[1]["HORA"] );
                                                    funcionarioPendente.Add( "Hora2", "" );
                                                    funcionarioPendente.Add( "Hora3", "" );
                                                    funcionarioPendente.Add( "TotalHorario", "4" );

                                                    pendentes.Add( funcionarioPendente );
                                                }
                                            }
                                            else
                                            {
                                                var funcionarioPendente = new Dictionary<string, string>();
                                                funcionarioPendente.Add( "IdFuncionario",
                                                    funcionarios[i]["ID_FUNCIONARIO"] );
                                                funcionarioPendente.Add( "NomeFuncionario", funcionarios[i]["NOME"] );
                                                funcionarioPendente.Add( "DataPendencia", marcacao[0]["DATA"] );
                                                funcionarioPendente.Add( "TotalHorario", marcacao.Count.ToString() );

                                                for(var j = 0; j < marcacao.Count; j++)
                                                    funcionarioPendente.Add( "Hora" + j, marcacao[j]["HORA"] );


                                                pendentes.Add( funcionarioPendente );
                                            }
                                        }
                                        else if(marcacao[0]["ID_CARGO"].Equals( "58" ))
                                        {
                                            if(marcacao.Count == 2)
                                            {
                                                var marcacao1 = DateTime.ParseExact( marcacao[0]["HORA"], "HH:mm:ss",
                                                    new DateTimeFormatInfo() );
                                                var marcacao2 = DateTime.ParseExact( marcacao[1]["HORA"], "HH:mm:ss",
                                                    new DateTimeFormatInfo() );

                                                var jornadaTrabalhada = marcacao2.Subtract( marcacao1 );
                                                TimeSpan horaExtra;

                                                var total = new TimeSpan( 00, 04, 00, 00 );
                                                horaExtra = jornadaTrabalhada.Subtract( total );

                                                if(horaExtra.Hours >= 4 && horaExtra.Minutes >= 5)
                                                {
                                                    var funcionarioPendente = new Dictionary<string, string>();
                                                    funcionarioPendente.Add( "IdFuncionario",
                                                        funcionarios[i]["ID_FUNCIONARIO"] );
                                                    funcionarioPendente.Add( "NomeFuncionario", funcionarios[i]["NOME"] );
                                                    funcionarioPendente.Add( "DataPendencia", marcacao[0]["DATA"] );
                                                    funcionarioPendente.Add( "Hora0", marcacao[0]["HORA"] );
                                                    funcionarioPendente.Add( "Hora1", marcacao[1]["HORA"] );
                                                    funcionarioPendente.Add( "Hora2", "" );
                                                    funcionarioPendente.Add( "Hora3", "" );
                                                    funcionarioPendente.Add( "TotalHorario", "4" );

                                                    pendentes.Add( funcionarioPendente );
                                                }
                                            }
                                            else
                                            {
                                                var funcionarioPendente = new Dictionary<string, string>();
                                                funcionarioPendente.Add( "IdFuncionario",
                                                    funcionarios[i]["ID_FUNCIONARIO"] );
                                                funcionarioPendente.Add( "NomeFuncionario", funcionarios[i]["NOME"] );
                                                funcionarioPendente.Add( "DataPendencia", marcacao[0]["DATA"] );
                                                funcionarioPendente.Add( "TotalHorario", marcacao.Count.ToString() );

                                                for(var j = 0; j < marcacao.Count; j++)
                                                    funcionarioPendente.Add( "Hora" + j, marcacao[j]["HORA"] );


                                                pendentes.Add( funcionarioPendente );
                                            }
                                        }
                                        else
                                        {
                                            #region Tratamento Ponto Errado

                                            var pendencia = new Dictionary<string, string>();
                                            pendencia.Add( "IdFuncionario", funcionarios[i]["ID_FUNCIONARIO"] );
                                            pendencia.Add( "NomeFuncionario", funcionarios[i]["NOME"] );
                                            pendencia.Add( "DataPendencia", marcacao[0]["DATA"] );
                                            pendencia.Add( "TotalHorario", marcacao.Count.ToString() );

                                            for(var j = 0; j < marcacao.Count; j++)
                                                pendencia.Add( "Hora" + j, marcacao[j]["HORA"] );

                                            pendentes.Add( pendencia );

                                            #endregion
                                        }
                                    }
                                    else
                                    {
                                        try
                                        {
                                            var marcacao1 = DateTime.ParseExact(marcacao[0]["HORA"], "HH:mm:ss",
                                                new DateTimeFormatInfo());
                                            var marcacao2 = DateTime.ParseExact(marcacao[1]["HORA"], "HH:mm:ss",
                                                new DateTimeFormatInfo());
                                            var marcacao3 = DateTime.ParseExact(marcacao[2]["HORA"], "HH:mm:ss",
                                                new DateTimeFormatInfo());
                                            var marcacao4 = DateTime.ParseExact(marcacao[3]["HORA"], "HH:mm:ss",
                                                new DateTimeFormatInfo());

                                            var almoco = marcacao3.Subtract(marcacao2);

                                            var jornadaTrabalhada = marcacao4.Subtract(marcacao1).Subtract(almoco);
                                            TimeSpan horaExtra;
                                            var total = new TimeSpan(00, 06, 00, 00);
                                            horaExtra = jornadaTrabalhada.Subtract(total);


                                            if (horaExtra.Hours > 2)
                                            {
                                                var funcionarioPendente = new Dictionary<string, string>();
                                                funcionarioPendente.Add("IdFuncionario",
                                                    funcionarios[i]["ID_FUNCIONARIO"]);
                                                funcionarioPendente.Add("NomeFuncionario", funcionarios[i]["NOME"]);
                                                funcionarioPendente.Add("DataPendencia", marcacao[0]["DATA"]);
                                                funcionarioPendente.Add("Hora0", marcacao[0]["HORA"]);
                                                funcionarioPendente.Add("Hora1", marcacao[1]["HORA"]);
                                                funcionarioPendente.Add("Hora2", marcacao[2]["HORA"]);
                                                funcionarioPendente.Add("Hora3", marcacao[3]["HORA"]);
                                                funcionarioPendente.Add("TotalHorario", "4");

                                                pendentes.Add(funcionarioPendente);
                                            }
                                            else
                                            {
                                                var funcionario = new Dictionary<string, string>();
                                                funcionario.Add("IdFuncionario", funcionarios[i]["ID_FUNCIONARIO"]);
                                                funcionario.Add("NomeFuncionario", funcionarios[i]["NOME"]);
                                                funcionario.Add("DataPendencia", marcacao[0]["DATA"]);
                                                funcionario.Add("Hora0", marcacao[0]["HORA"]);
                                                funcionario.Add("Hora1", marcacao[1]["HORA"]);
                                                funcionario.Add("Hora2", marcacao[2]["HORA"]);
                                                funcionario.Add("Hora3", marcacao[3]["HORA"]);
                                                funcionario.Add("Jornada", jornadaTrabalhada.ToString());
                                                funcionario.Add("HoraExtra", horaExtra.ToString());


                                                semPendencias.Add(funcionario);
                                            }
                                        }
                                        catch
                                        {
                                            var marcacao1 = DateTime.ParseExact(marcacao[0]["HORA"], "HH:mm:ss",
                                                new DateTimeFormatInfo());
                                            var marcacao2 = DateTime.ParseExact(marcacao[1]["HORA"], "HH:mm:ss",
                                                new DateTimeFormatInfo());


                                            var jornadaTrabalhada = marcacao2.Subtract(marcacao1);
                                            TimeSpan horaExtra;
                                            var total = new TimeSpan(00, 06, 00, 00);
                                            horaExtra = jornadaTrabalhada.Subtract(total);


                                            if (horaExtra.Hours > 2)
                                            {
                                                var funcionarioPendente = new Dictionary<string, string>();
                                                funcionarioPendente.Add("IdFuncionario",
                                                    funcionarios[i]["ID_FUNCIONARIO"]);
                                                funcionarioPendente.Add("NomeFuncionario", funcionarios[i]["NOME"]);
                                                funcionarioPendente.Add("DataPendencia", marcacao[0]["DATA"]);
                                                funcionarioPendente.Add("Hora0", marcacao[0]["HORA"]);
                                                funcionarioPendente.Add("Hora1", marcacao[1]["HORA"]);
                                                funcionarioPendente.Add("Hora2", marcacao[2]["HORA"]);
                                                funcionarioPendente.Add("Hora3", marcacao[3]["HORA"]);
                                                funcionarioPendente.Add("TotalHorario", "4");

                                                pendentes.Add(funcionarioPendente);
                                            }
                                            else
                                            {
                                                var funcionario = new Dictionary<string, string>();
                                                funcionario.Add("IdFuncionario", funcionarios[i]["ID_FUNCIONARIO"]);
                                                funcionario.Add("NomeFuncionario", funcionarios[i]["NOME"]);
                                                funcionario.Add("DataPendencia", marcacao[0]["DATA"]);
                                                funcionario.Add("Hora0", marcacao[0]["HORA"]);
                                                funcionario.Add("Hora1", marcacao[1]["HORA"]);
                                                funcionario.Add("Jornada", jornadaTrabalhada.ToString());
                                                funcionario.Add("HoraExtra", horaExtra.ToString());


                                                semPendencias.Add(funcionario);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (marcacao.Count == 0)
                                    {
                                        var funcionarioPendente = new Dictionary<string, string>();
                                        funcionarioPendente.Add("IdFuncionario", funcionarios[i]["ID_FUNCIONARIO"]);
                                        funcionarioPendente.Add("NomeFuncionario", funcionarios[i]["NOME"]);
                                        funcionarioPendente.Add("DataPendencia", dataConsulta.ToString("dd/MM/yyyy"));
                                        funcionarioPendente.Add("Hora0", "--:--");
                                        funcionarioPendente.Add("Hora1", "--:--");
                                        funcionarioPendente.Add("Hora2", "--:--");
                                        funcionarioPendente.Add("Hora3", "--:--");
                                        funcionarioPendente.Add("TotalHorario", "4");

                                        pendentes.Add(funcionarioPendente);
                                    }

                                    #endregion

                                    else if (marcacao.Count < 4 || marcacao.Count > 4)
                                    {
                                        #region Tratamento Hora extra, Batida errada aprendiz e estagiario e Irani

                                        if (marcacao[0]["ID_CARGO"].Equals("2") ||
                                            funcionarios[i]["NOME"].Contains("IRANI") && marcacao.Count == 2)
                                        {
                                            if (marcacao.Count == 2)
                                            {
                                                var marcacao1 = DateTime.ParseExact(marcacao[0]["HORA"], "HH:mm:ss",
                                                    new DateTimeFormatInfo());
                                                var marcacao2 = DateTime.ParseExact(marcacao[1]["HORA"], "HH:mm:ss",
                                                    new DateTimeFormatInfo());
                                                var jornadaTrabalhada = marcacao2.Subtract(marcacao1);
                                                TimeSpan horaExtra;


                                                var total = new TimeSpan(00, 06, 00, 00);
                                                horaExtra = jornadaTrabalhada.Subtract(total);

                                                if (horaExtra.Hours >= 0 && horaExtra.Minutes > 5)
                                                {
                                                    var funcionarioPendente = new Dictionary<string, string>();
                                                    funcionarioPendente.Add("IdFuncionario",
                                                        funcionarios[i]["ID_FUNCIONARIO"]);
                                                    funcionarioPendente.Add("NomeFuncionario", funcionarios[i]["NOME"]);
                                                    funcionarioPendente.Add("DataPendencia", marcacao[0]["DATA"]);
                                                    funcionarioPendente.Add("Hora0", marcacao[0]["HORA"]);
                                                    funcionarioPendente.Add("Hora1", marcacao[1]["HORA"]);
                                                    funcionarioPendente.Add("Hora2", "");
                                                    funcionarioPendente.Add("Hora3", "");
                                                    funcionarioPendente.Add("TotalHorario", "4");

                                                    pendentes.Add(funcionarioPendente);
                                                }
                                            }
                                            else
                                            {
                                                var funcionarioPendente = new Dictionary<string, string>();
                                                funcionarioPendente.Add("IdFuncionario",
                                                    funcionarios[i]["ID_FUNCIONARIO"]);
                                                funcionarioPendente.Add("NomeFuncionario", funcionarios[i]["NOME"]);
                                                funcionarioPendente.Add("DataPendencia", marcacao[0]["DATA"]);
                                                funcionarioPendente.Add("TotalHorario", marcacao.Count.ToString());

                                                for (var j = 0; j < marcacao.Count; j++)
                                                    funcionarioPendente.Add("Hora" + j, marcacao[j]["HORA"]);


                                                pendentes.Add(funcionarioPendente);
                                            }
                                        }
                                        else if (marcacao[0]["ID_CARGO"].Equals("58"))
                                        {
                                            if (marcacao.Count == 2)
                                            {
                                                var marcacao1 = DateTime.ParseExact(marcacao[0]["HORA"], "HH:mm:ss",
                                                    new DateTimeFormatInfo());
                                                var marcacao2 = DateTime.ParseExact(marcacao[1]["HORA"], "HH:mm:ss",
                                                    new DateTimeFormatInfo());

                                                var jornadaTrabalhada = marcacao2.Subtract(marcacao1);
                                                TimeSpan horaExtra;

                                                var total = new TimeSpan(00, 04, 00, 00);
                                                horaExtra = jornadaTrabalhada.Subtract(total);

                                                if (horaExtra.Hours >= 4 && horaExtra.Minutes >= 5)
                                                {
                                                    var funcionarioPendente = new Dictionary<string, string>();
                                                    funcionarioPendente.Add("IdFuncionario",
                                                        funcionarios[i]["ID_FUNCIONARIO"]);
                                                    funcionarioPendente.Add("NomeFuncionario", funcionarios[i]["NOME"]);
                                                    funcionarioPendente.Add("DataPendencia", marcacao[0]["DATA"]);
                                                    funcionarioPendente.Add("Hora0", marcacao[0]["HORA"]);
                                                    funcionarioPendente.Add("Hora1", marcacao[1]["HORA"]);
                                                    funcionarioPendente.Add("Hora2", "");
                                                    funcionarioPendente.Add("Hora3", "");
                                                    funcionarioPendente.Add("TotalHorario", "4");

                                                    pendentes.Add(funcionarioPendente);
                                                }
                                            }
                                            else
                                            {
                                                var funcionarioPendente = new Dictionary<string, string>();
                                                funcionarioPendente.Add("IdFuncionario",
                                                    funcionarios[i]["ID_FUNCIONARIO"]);
                                                funcionarioPendente.Add("NomeFuncionario", funcionarios[i]["NOME"]);
                                                funcionarioPendente.Add("DataPendencia", marcacao[0]["DATA"]);
                                                funcionarioPendente.Add("TotalHorario", marcacao.Count.ToString());

                                                for (var j = 0; j < marcacao.Count; j++)
                                                    funcionarioPendente.Add("Hora" + j, marcacao[j]["HORA"]);


                                                pendentes.Add(funcionarioPendente);
                                            }
                                        }
                                        else
                                        {
                                            #region Tratamento Ponto Errado

                                            var pendencia = new Dictionary<string, string>();
                                            pendencia.Add("IdFuncionario", funcionarios[i]["ID_FUNCIONARIO"]);
                                            pendencia.Add("NomeFuncionario", funcionarios[i]["NOME"]);
                                            pendencia.Add("DataPendencia", marcacao[0]["DATA"]);
                                            pendencia.Add("TotalHorario", marcacao.Count.ToString());

                                            for (var j = 0; j < marcacao.Count; j++)
                                                pendencia.Add("Hora" + j, marcacao[j]["HORA"]);

                                            pendentes.Add(pendencia);

                                            #endregion
                                        }

                                        #endregion
                                    }
                                    else
                                    {
                                        #region Calculo Hora Extra e Almoço

                                        {
                                            var marcacao1 = DateTime.ParseExact(marcacao[0]["HORA"], "HH:mm:ss",
                                                new DateTimeFormatInfo());
                                            var marcacao2 = DateTime.ParseExact(marcacao[1]["HORA"], "HH:mm:ss",
                                                new DateTimeFormatInfo());
                                            var marcacao3 = DateTime.ParseExact(marcacao[2]["HORA"], "HH:mm:ss",
                                                new DateTimeFormatInfo());
                                            var marcacao4 = DateTime.ParseExact(marcacao[3]["HORA"], "HH:mm:ss",
                                                new DateTimeFormatInfo());

                                            var almoco = marcacao3.Subtract(marcacao2);

                                            var jornadaTrabalhada = marcacao4.Subtract(marcacao1).Subtract(almoco);
                                            TimeSpan horaExtra;
                                            var total = new TimeSpan(00, 08, 00, 00);
                                            horaExtra = jornadaTrabalhada.Subtract(total);


                                            if (horaExtra.Hours > 2)
                                            {
                                                var funcionarioPendente = new Dictionary<string, string>();
                                                funcionarioPendente.Add("IdFuncionario",
                                                    funcionarios[i]["ID_FUNCIONARIO"]);
                                                funcionarioPendente.Add("NomeFuncionario", funcionarios[i]["NOME"]);
                                                funcionarioPendente.Add("DataPendencia", marcacao[0]["DATA"]);
                                                funcionarioPendente.Add("Hora0", marcacao[0]["HORA"]);
                                                funcionarioPendente.Add("Hora1", marcacao[1]["HORA"]);
                                                funcionarioPendente.Add("Hora2", marcacao[2]["HORA"]);
                                                funcionarioPendente.Add("Hora3", marcacao[3]["HORA"]);
                                                funcionarioPendente.Add("TotalHorario", "4");

                                                pendentes.Add(funcionarioPendente);
                                            }
                                            else if (almoco.Hours >= 2 && almoco.Minutes > 0)
                                            {
                                                var funcionarioPendente = new Dictionary<string, string>();
                                                funcionarioPendente.Add("IdFuncionario",
                                                    funcionarios[i]["ID_FUNCIONARIO"]);
                                                funcionarioPendente.Add("NomeFuncionario", funcionarios[i]["NOME"]);
                                                funcionarioPendente.Add("DataPendencia", marcacao[0]["DATA"]);
                                                funcionarioPendente.Add("Hora0", marcacao[0]["HORA"]);
                                                funcionarioPendente.Add("Hora1", marcacao[1]["HORA"]);
                                                funcionarioPendente.Add("Hora2", marcacao[2]["HORA"]);
                                                funcionarioPendente.Add("Hora3", marcacao[3]["HORA"]);
                                                funcionarioPendente.Add("TotalHorario", "4");

                                                pendentes.Add(funcionarioPendente);
                                            }
                                            else if (almoco.Hours < 1 && almoco.Minutes <= 59)
                                            {
                                                var funcionarioPendente = new Dictionary<string, string>();
                                                funcionarioPendente.Add("IdFuncionario",
                                                    funcionarios[i]["ID_FUNCIONARIO"]);
                                                funcionarioPendente.Add("NomeFuncionario", funcionarios[i]["NOME"]);
                                                funcionarioPendente.Add("DataPendencia", marcacao[0]["DATA"]);
                                                funcionarioPendente.Add("Hora0", marcacao[0]["HORA"]);
                                                funcionarioPendente.Add("Hora1", marcacao[1]["HORA"]);
                                                funcionarioPendente.Add("Hora2", marcacao[2]["HORA"]);
                                                funcionarioPendente.Add("Hora3", marcacao[3]["HORA"]);
                                                funcionarioPendente.Add("TotalHorario", "4");

                                                pendentes.Add(funcionarioPendente);
                                            }

                                            #region Tratamento sem justificativa

                                            else
                                            {
                                                var funcionario = new Dictionary<string, string>();
                                                funcionario.Add("IdFuncionario", funcionarios[i]["ID_FUNCIONARIO"]);
                                                funcionario.Add("NomeFuncionario", funcionarios[i]["NOME"]);
                                                funcionario.Add("DataPendencia", marcacao[0]["DATA"]);
                                                funcionario.Add("Hora0", marcacao[0]["HORA"]);
                                                funcionario.Add("Hora1", marcacao[1]["HORA"]);
                                                funcionario.Add("Hora2", marcacao[2]["HORA"]);
                                                funcionario.Add("Hora3", marcacao[3]["HORA"]);
                                                funcionario.Add("Jornada", jornadaTrabalhada.ToString());
                                                funcionario.Add("HoraExtra", horaExtra.ToString());


                                                semPendencias.Add(funcionario);
                                            }

                                            #endregion
                                        }

                                        #endregion
                                    }
                                }
                            }
                    }
            }

            var listaFinal = new List<List<Dictionary<string, string>>>();
            listaFinal.Add(pendentes);
            listaFinal.Add(semPendencias);

            return listaFinal;
        }

        public List<Dictionary<string, string>> RetornaPendenciasMysql()
        {
            var faltaConfirmacao = new List<Dictionary<string, string>>();
            var dadosRh = new QueryMysqlRh();
            var firebird = new QueryFirebird();

            var todasPendencias = dadosRh.RetornaPendencias();

            for (var i = 0; i < todasPendencias.Count; i++)
            {
                var dadosPendencia = dadosRh.RetornaDadosPendencias(todasPendencias[i]["id"]);

                var confirmar = new Dictionary<string, string>();
                if (todasPendencias[i]["validacaogestor"].Equals("S"))
                    confirmar.Add("ConfirmaGestor", "green");
                else
                    confirmar.Add("ConfirmaGestor", "red");
                confirmar.Add("IdPendencia", todasPendencias[i]["id"]);
                confirmar.Add("Nome", todasPendencias[i]["nome"]);
                confirmar.Add("Data", todasPendencias[i]["data"]);
                confirmar.Add("TotalHorario", dadosPendencia.Count.ToString());
                var validado = false;
                for (var j = 0; j < dadosPendencia.Count; j++)
                    if (dadosPendencia[j]["idjustificativafirebird"].Equals("0") &&
                        dadosPendencia[j]["observacao"] == null)
                    {
                        confirmar.Add("Horario" + j, dadosPendencia[j]["horario"]);
                    }
                    else
                    {
                        try
                        {
                            if (dadosPendencia[j]["observacao"].Length > 0)
                            {
                                var justificativa = dadosPendencia[j]["observacao"];
                                try
                                {
                                    confirmar.Add("Justificativa" + i, justificativa);
                                }
                                catch
                                {
                                    // ignored
                                }
                            }
                            else
                            {
                                var justificativa =
                                    firebird.RecuperaJustificativasFuncioanrio(
                                        dadosPendencia[j]["idjustificativafirebird"]);
                                try
                                {
                                    confirmar.Add("Justificativa" + i, justificativa[0]["DESCRICAO"]);
                                }
                                catch
                                {
                                    // ignored
                                }
                            }
                        }
                        catch
                        {
                            var justificativa =
                                firebird.RecuperaJustificativasFuncioanrio(
                                    dadosPendencia[j]["idjustificativafirebird"]);
                            try
                            {
                                confirmar.Add("Justificativa" + i, justificativa[0]["DESCRICAO"]);
                            }
                            catch
                            {
                                // ignored
                            }
                        }

                        validado = true;
                        confirmar.Add("Horario" + j, dadosPendencia[j]["horario"]);
                    }

                if (validado)
                    confirmar.Add("Justificado", "true");
                else
                    confirmar.Add("Justificado", "false");

                faltaConfirmacao.Add(confirmar);
            }

            return faltaConfirmacao;
        }

        public List<Dictionary<string, string>> RetornaPendenciasFuncionario(string idFuncionario)
        {
            var faltaConfirmacao = new List<Dictionary<string, string>>();

            var dadosRh = new QueryMysqlRh();
            var queryFire = new QueryFirebird();
            var todasPendencias = dadosRh.RetornaPendenciasUsuario(idFuncionario);

            for (var i = 0; i < todasPendencias.Count; i++)
            {
                var dadosPendencia = dadosRh.RetornaDadosPendencias(todasPendencias[i]["id"]);
                var idFuncionarioFirebird = queryFire.RetornaIdFuncionario(todasPendencias[i]["nome"]);

                var confirmar = new Dictionary<string, string>();
                if (todasPendencias[i]["validacaogestor"].Equals("S"))
                    confirmar.Add("ConfirmaGestor", "true");
                else
                    confirmar.Add("ConfirmaGestor", "false");
                confirmar.Add("IdPendencia", todasPendencias[i]["id"]);
                confirmar.Add("Nome", todasPendencias[i]["nome"]);
                confirmar.Add("IdFuncionarioFireBird", idFuncionarioFirebird[0]["ID_FUNCIONARIO"]);
                confirmar.Add("Data", todasPendencias[i]["data"]);
                confirmar.Add("TotalHorario", dadosPendencia.Count.ToString());
                var validado = false;
                for (var j = 0; j < dadosPendencia.Count; j++)
                    if (dadosPendencia[j]["idjustificativafirebird"].Equals("0") &&
                        dadosPendencia[j]["observacao"] == null)
                    {
                        confirmar.Add("Horario" + j, dadosPendencia[j]["horario"]);
                    }
                    else
                    {
                        validado = true;
                        confirmar.Add("Horario" + j, dadosPendencia[j]["horario"]);
                    }

                if (validado)
                    confirmar.Add("Justificado", "true");
                else
                    confirmar.Add("Justificado", "false");

                faltaConfirmacao.Add(confirmar);
            }

            return faltaConfirmacao;
        }

        public List<Dictionary<string, string>> RetornaPendenciasSetor(ArrayList idSetor)
        {
            var faltaConfirmacao = new List<Dictionary<string, string>>();

            var dadosRh = new QueryMysqlRh();
            var queryFire = new QueryFirebird();
            var count = 0;
            for (int k = 0; k < idSetor.Count; k++)
            {
                var todasPendencias = dadosRh.RetornaPendenciasSetor(idSetor[k].ToString());

                for (var i = 0; i < todasPendencias.Count; i++)
                {
                    var dadosPendencia = dadosRh.RetornaDadosPendencias(todasPendencias[i]["id"]);
                    var idFuncionarioFirebird = queryFire.RetornaIdFuncionario(todasPendencias[i]["nome"]);

                    var confirmar = new Dictionary<string, string>();
                    if (todasPendencias[i]["validacaogestor"].Equals("S"))
                        confirmar.Add("ConfirmaGestor", "true");
                    else
                        confirmar.Add("ConfirmaGestor", "false");
                    confirmar.Add("IdPendencia", todasPendencias[i]["id"]);
                    confirmar.Add("Nome", todasPendencias[i]["nome"]);
                    confirmar.Add("IdFuncionarioFireBird", idFuncionarioFirebird[0]["ID_FUNCIONARIO"]);
                    confirmar.Add("Data", todasPendencias[i]["data"]);
                    confirmar.Add("TotalHorario", dadosPendencia.Count.ToString());
                    var validado = false;

                    for (var j = 0; j < dadosPendencia.Count; j++)
                        if (dadosPendencia[j]["idjustificativafirebird"].Equals("0") &&
                            (dadosPendencia[j]["observacao"] == null || dadosPendencia[j]["observacao"].Length == 0))
                        {
                            confirmar.Add("Horario" + j, dadosPendencia[j]["horario"]);
                        }
                        else
                        {
                            validado = true;
                            if (dadosPendencia[j]["observacao"] != null)
                            {
                                if (dadosPendencia[j]["observacao"].Length > 0)
                                {
                                    var justificativa = dadosPendencia[j]["observacao"];
                                    try
                                    {
                                        confirmar.Add("Justificativa" + count, justificativa);
                                    }
                                    catch
                                    {
                                        // ignored
                                    }
                                }
                                else
                                {
                                    var justificativa =
                                        queryFire.RecuperaJustificativasFuncioanrio(
                                            dadosPendencia[j]["idjustificativafirebird"]);
                                    try
                                    {
                                        confirmar.Add("Justificativa" + count, justificativa[0]["DESCRICAO"]);
                                    }
                                    catch
                                    {
                                        // ignored
                                    }
                                }
                            }
                            else
                            {
                                var justificativa =
                                    queryFire.RecuperaJustificativasFuncioanrio(
                                        dadosPendencia[j]["idjustificativafirebird"]);
                                try
                                {
                                    confirmar.Add("Justificativa" + count, justificativa[0]["DESCRICAO"]);
                                }
                                catch
                                {
                                    // ignored
                                }
                            }

                            confirmar.Add("Horario" + j, dadosPendencia[j]["horario"]);
                        }

                    if (validado)
                        confirmar.Add("Justificado", "true");
                    else
                        confirmar.Add("Justificado", "false");

                    faltaConfirmacao.Add(confirmar);
                    count++;
                }
            }

            return faltaConfirmacao;
        }

        public List<Dictionary<string, string>> RetornaPendenciasFuncionarioValidar(ArrayList idFuncionario)
        {
            var faltaConfirmacao = new List<Dictionary<string, string>>();

            var dadosRh = new QueryMysqlRh();
            var queryFire = new QueryFirebird();
            var count = 0;
            for(int k = 0; k < idFuncionario.Count; k++)
            {
                var todasPendencias = dadosRh.RetornaPendenciasUsuario(  idFuncionario[k].ToString() );

                for(var i = 0; i < todasPendencias.Count; i++)
                {
                    
                    var dadosPendencia = dadosRh.RetornaDadosPendencias( todasPendencias[i]["id"] );
                    var idFuncionarioFirebird = queryFire.RetornaIdFuncionario( todasPendencias[i]["nome"] );

                    var confirmar = new Dictionary<string, string>();
                    if(todasPendencias[i]["validacaogestor"].Equals( "S" ))
                        confirmar.Add( "ConfirmaGestor", "true" );
                    else
                        confirmar.Add( "ConfirmaGestor", "false" );
                    confirmar.Add( "IdPendencia", todasPendencias[i]["id"] );
                    confirmar.Add( "Nome", todasPendencias[i]["nome"] );
                    confirmar.Add( "IdFuncionarioFireBird", idFuncionarioFirebird[0]["ID_FUNCIONARIO"] );
                    confirmar.Add( "Data", todasPendencias[i]["data"] );
                    confirmar.Add( "TotalHorario", dadosPendencia.Count.ToString() );
                    var validado = false;

                    for(var j = 0; j < dadosPendencia.Count; j++)
                        if(dadosPendencia[j]["idjustificativafirebird"].Equals( "0" ) &&
                            dadosPendencia[j]["observacao"] == null)
                        {
                            confirmar.Add( "Horario" + j, dadosPendencia[j]["horario"] );
                        }
                        else
                        {
                            validado = true;
                            if(dadosPendencia[j]["observacao"] != null)
                            {
                                if(dadosPendencia[j]["observacao"].Length > 0)
                                {
                                    var justificativa = dadosPendencia[j]["observacao"];
                                    try
                                    {
                                        confirmar.Add( "Justificativa" + count, justificativa );
                                    }
                                    catch
                                    {
                                        // ignored
                                    }
                                }
                                else
                                {
                                    var justificativa =
                                        queryFire.RecuperaJustificativasFuncioanrio(
                                            dadosPendencia[j]["idjustificativafirebird"] );
                                    try
                                    {
                                        confirmar.Add( "Justificativa" + count, justificativa[0]["DESCRICAO"] );
                                    }
                                    catch
                                    {
                                        // ignored
                                    }
                                }
                            }
                            else
                            {
                                var justificativa =
                                    queryFire.RecuperaJustificativasFuncioanrio(
                                        dadosPendencia[j]["idjustificativafirebird"] );
                                try
                                {
                                    confirmar.Add( "Justificativa" + count, justificativa[0]["DESCRICAO"] );
                                }
                                catch
                                {
                                    // ignored
                                }
                            }

                            confirmar.Add( "Horario" + j, dadosPendencia[j]["horario"] );
                        }

                    if(validado)
                        confirmar.Add( "Justificado", "true" );
                    else
                        confirmar.Add( "Justificado", "false" );

                    faltaConfirmacao.Add( confirmar );
                    count++;
                }
            }

            return faltaConfirmacao;
        }

        public List<Dictionary<string, string>> RetornaHistoricoPendencias(string dataInicial, string dataFinal)
        {
            var faltaConfirmacao = new List<Dictionary<string, string>>();

            var dadosRh = new QueryMysqlRh();
            var firebird = new QueryFirebird();
            var todasPendencias = dadosRh.RetornaHistoricoPendencias(dataInicial, dataFinal);

            for (var i = 0; i < todasPendencias.Count; i++)
            {
                var dadosPendencia = dadosRh.RetornaDadosPendencias(todasPendencias[i]["id"]);

                var confirmar = new Dictionary<string, string>();

                confirmar.Add("IdPendencia", todasPendencias[i]["id"]);
                confirmar.Add("Nome", todasPendencias[i]["nome"]);
                confirmar.Add("Data", todasPendencias[i]["data"]);
                confirmar.Add("TotalHorario", dadosPendencia.Count.ToString());
                var validado = false;
                for (var j = 0; j < dadosPendencia.Count; j++)
                    if (dadosPendencia[j]["idjustificativafirebird"].Equals("0"))
                    {
                        confirmar.Add("Horario" + j, dadosPendencia[j]["horario"]);
                    }
                    else
                    {
                        var justificativa =
                            firebird.RecuperaJustificativasFuncioanrio(dadosPendencia[j]["idjustificativafirebird"]);
                        try
                        {
                            confirmar.Add("Justificativa" + i, justificativa[0]["DESCRICAO"]);
                        }
                        catch
                        {
                            // ignored
                        }

                        validado = true;
                        confirmar.Add("Horario" + j, dadosPendencia[j]["horario"]);
                    }

                if (validado)
                    confirmar.Add("Justificado", "true");
                else
                    confirmar.Add("Justificado", "false");

                faltaConfirmacao.Add(confirmar);
            }

            return faltaConfirmacao;
        }
    }
}