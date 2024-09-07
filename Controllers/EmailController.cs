using System;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace ConcessionariaMVC.Controllers
{
    public class EmailController : Controller
    {
        public void EnviarEmail(string destinatario, string assunto, string mensagem)
        {
            var remetente = "arturtmelo1@gmail.com";
            var senha = "sigi qglu xwcb stfc";

            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(remetente, senha),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(remetente),
                    Subject = assunto,
                    Body = mensagem,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(destinatario);

                smtpClient.Send(mailMessage);
            }
            catch (SmtpException smtpEx)
            {
                throw new ApplicationException("Erro ao enviar o email: " + smtpEx.Message, smtpEx);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Ocorreu um erro ao tentar enviar o email: " + ex.Message, ex);
            }
        }
    }
}
