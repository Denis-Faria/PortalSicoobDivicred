using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using OneSignal.CSharp.SDK;
using OneSignal.CSharp.SDK.Resources;
using OneSignal.CSharp.SDK.Resources.Notifications;
using MailAddress = System.Net.Mail.MailAddress;

namespace PortalSicoobDivicred.Aplicacao
{
    public class EnviodeAlertas
    {

        public async Task EnviaEmail(string email, string mensagem)
        {
            SmtpClient client = new SmtpClient();
            client.Host = "mail.divicred.com.br";
            client.Port = 587;
            client.EnableSsl = false;
            client.Credentials = new NetworkCredential("correio@divicred.com.br", "DIVICRED@4030");
            MailMessage mail = new MailMessage();
            mail.Sender = new MailAddress("correio@divicred.com.br", "PORTAL SICOOB DIVICRED");
            mail.From = new MailAddress("correio@divicred.com.br", "SICOOB DIVICRED");
            mail.To.Add(new MailAddress(email));
            mail.Subject = "ALTERAÇÕES PORTAL SICOOB DIVICRED";
            mail.Body = " ALTERAÇÃO<br/> <p>" + mensagem + "</p>";
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            try
            {
                await client.SendMailAsync(mail);
            }
            catch
            {
                //trata erro
            }

            /*      var builder = new MailBuilder();
                  builder.From.Add(new MailBox("correio@divicred.com.br", "Portal Sicoob Divicred - Notificação"));
                  builder.To.Add(new MailBox(Email));
                  builder.Subject = "Portal Sicoob Divicred - Notificação";
                  builder.Html = "<p>"+Mensagem+"</p> <br/> " ;

                  var email = builder.Create();
                  using (var smtp = new Smtp())
                  {
                      smtp.Connect("mail.divicred.com.br", 587);
                      smtp.UseBestLogin("correio@divicred.com.br", "DIVICRED@4030");

                      var result = smtp.SendMessage(email);
                      if (result.Status == SendMessageStatus.Success)
                      {
                      }

                      smtp.Close();*/
            // }
        }


        public void CadastraAlerta(string idAlerta, string mensagem)
        {
            var client = new OneSignalClient("ODAwZTEyMTEtYmRmOS00ZmRhLTljNjUtZmMxMTBmM2UxNDdm");
            var options = new NotificationCreateOptions();

            options.AppId = new Guid("a41d4d09-06f8-4912-9a7a-50d4303637e6");
            options.Contents.Add(LanguageCodes.English, mensagem);
            options.IncludePlayerIds = new[] { idAlerta };

            client.Notifications.Create(options);



        }


        public async Task EnviaAlertaFuncionario(Dictionary<string, string> funcionarioEnvio, string mensagem, string idAplicativo)
        {
            var cadastroAlerta = new QueryMysql();
            
            if (funcionarioEnvio["notificacaoemail"].Equals("Sim"))
            {


                cadastroAlerta.cadastrarAlert(funcionarioEnvio["id"], idAplicativo, mensagem);


                await EnviaEmail(funcionarioEnvio["email"], mensagem);

                try
                {
                    if (funcionarioEnvio["idnotificacao"].Length > 0)
                    {
                        CadastraAlerta(funcionarioEnvio["idnotificacao"], mensagem);
                    }
                }
                catch
                {
                    //ignored
                }
            }
            else
            {
                cadastroAlerta.cadastrarAlert(funcionarioEnvio["id"], idAplicativo, mensagem);
                try
                {
                    if (funcionarioEnvio["idnotificacao"].Length > 0)
                    {
                        CadastraAlerta(funcionarioEnvio["idnotificacao"], mensagem);
                    }
                }
                catch
                {
                    //ignored
                }
            }
        }

        public async Task EnviaAlertaGestor(Dictionary<string, string> funcionarioEnvio, string mensagem, string idAplicativo)
        {
            var cadastroAlerta = new QueryMysql();

            if (funcionarioEnvio["notificacaoemail"].Equals("Sim"))
            {

                cadastroAlerta.cadastrarAlert(funcionarioEnvio["id"], idAplicativo, mensagem);


                await EnviaEmail(funcionarioEnvio["email"], mensagem);

                try
                {
                    if (funcionarioEnvio["idnotificacao"].Length > 0)
                    {
                        CadastraAlerta(funcionarioEnvio["idnotificacao"], mensagem);
                    }
                }
                catch
                {
                    //ignored
                }
            }
            else
            {
                cadastroAlerta.cadastrarAlert(funcionarioEnvio["id"], idAplicativo, mensagem);
                try
                {
                    if (funcionarioEnvio["idnotificacao"].Length > 0)
                    {
                        CadastraAlerta(funcionarioEnvio["idnotificacao"], mensagem);
                    }
                }
                catch
                {
                    //ignored
                }
            }
        }
    }
}