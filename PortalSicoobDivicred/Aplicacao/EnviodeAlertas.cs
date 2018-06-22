using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using Limilabs.Client.SMTP;
using Limilabs.Mail;
using Limilabs.Mail.Headers;
using OneSignal.CSharp.SDK;
using OneSignal.CSharp.SDK.Resources;
using OneSignal.CSharp.SDK.Resources.Notifications;
using MailAddress = System.Net.Mail.MailAddress;

namespace PortalSicoobDivicred.Aplicacao
{
    public class EnviodeAlertas
    {

        public async Task EnviaEmail(string Email, string Mensagem)
        {
            SmtpClient client = new SmtpClient();
            client.Host = "mail.divicred.com.br";
            client.Port = 587;
            client.EnableSsl = false;
            client.Credentials = new System.Net.NetworkCredential("correio@divicred.com.br", "DIVICRED@4030");
            MailMessage mail = new MailMessage();
            mail.Sender = new MailAddress("correio@divicred.com.br", "PORTAL SICOOB DIVICRED");
            mail.From = new MailAddress("correio@divicred.com.br", "SICOOB DIVICRED");
            mail.To.Add(new MailAddress(Email));
            mail.Subject = "ALTERAÇÕES PORTAL SICOOB DIVICRED";
            mail.Body = " ALTERAÇÃO<br/> <p>"+Mensagem+"</p>";
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            try
            {
                await client.SendMailAsync(mail);
            }
            catch (System.Exception erro)
            {
                //trata erro
            }
            finally
            {
                mail = null;
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


        public void CadastraAlerta(string IdAlerta, string Mensagem)
        {
            var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;

            var client = new OneSignalClient("ODAwZTEyMTEtYmRmOS00ZmRhLTljNjUtZmMxMTBmM2UxNDdm");
            var options = new NotificationCreateOptions();

            options.AppId = new Guid("a41d4d09-06f8-4912-9a7a-50d4303637e6");
            options.Contents.Add(LanguageCodes.English, Mensagem);
            options.IncludePlayerIds = new string[] { IdAlerta };

            client.Notifications.Create(options);



        }
    }
}