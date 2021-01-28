using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FCInformesSolucion.Services
{
    public interface IEmailService
    {
        bool SendEmail(string from, 
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
                string bcc = "");
    }
}