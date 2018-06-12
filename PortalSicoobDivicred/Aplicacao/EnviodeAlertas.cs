using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using Limilabs.Client.SMTP;
using Limilabs.Mail;
using Limilabs.Mail.Headers;

namespace PortalSicoobDivicred.Aplicacao
{
    public class EnviodeAlertas
    {

        public void EnviaEmail(string Email,string Mensagem)
        {
            var builder = new MailBuilder();
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

                smtp.Close();
            }
        }

        public  void CadastraAlerta(string IdAlerta,string Mensagem)
        {
            var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;

            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";

            request.Headers.Add("authorization", "Basic ODAwZTEyMTEtYmRmOS00ZmRhLTljNjUtZmMxMTBmM2UxNDdm");

            var serializer = new JavaScriptSerializer();
            var obj = new
            {
                app_id = "a41d4d09-06f8-4912-9a7a-50d4303637e6",
                contents = new { en = Mensagem },
                include_player_ids = new string[] { IdAlerta }
            };



            var param = serializer.Serialize(obj);
            byte[] byteArray = Encoding.UTF8.GetBytes(param);

            string responseContent = null;

            try
            {
                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
            }

            System.Diagnostics.Debug.WriteLine(responseContent);
        }
    }
}