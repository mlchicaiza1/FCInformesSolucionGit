using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace FCInformesSolucion.Services
{
    public class EmailService : IEmailService
    {
        public bool SendEmail(string from, 
                string to, 
                string subject, 
                string message, 
                string smtpServer, 
                string user, 
                string password, 
                int port, 
                bool enableSsl, 
                List<string> attachmentPaths, 
                string cc = "", 
                string bcc = "")
        {
            try
            {
                var correo = new MailMessage();

                correo.From = new MailAddress(from);
                correo.To.Add(to);

                if (!string.IsNullOrEmpty(cc))
                    correo.CC.Add(cc);

                if (!string.IsNullOrEmpty(bcc))
                    correo.Bcc.Add(bcc);

                correo.Subject = subject;
                correo.Body = message;
                correo.IsBodyHtml = true;
                correo.Priority = MailPriority.Normal;

                #region Adjuntos

                if (attachmentPaths != null)
                {
                    foreach (var pathFile in attachmentPaths)
                    {
                        if (!File.Exists(pathFile))
                        {
                            continue;
                        }

                        var Data = new Attachment(pathFile);

                        //Obtengo las propiedades del archivo.
                        var disposition = Data.ContentDisposition;
                        disposition.CreationDate = File.GetCreationTime(pathFile);
                        disposition.ModificationDate = File.GetLastWriteTime(pathFile);
                        disposition.ReadDate = File.GetLastAccessTime(pathFile);
                        //Agrego el archivo al mensaje
                        correo.Attachments.Add(Data);
                    }
                }

                #endregion

                var smtp = new SmtpClient
                {
                    Host = smtpServer,
                    Port = port,
                    EnableSsl = enableSsl,
                    Credentials = new System.Net.NetworkCredential(user, password)
                };

                try
                {
                    smtp.Send(correo);
                    smtp.Dispose();
                }
                catch (Exception ex)
                {   
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        } 
    }
}