using System;
using System.Collections.Generic;

namespace FCInformesSolucion.Common
{
    public class SaveResultException : Exception
    {
        public SaveResult SaveResult { get; set; }
        public SaveResultException(string message, SaveResult saveResult = null) : 
            this(message, null, saveResult)
        {
            
        }

        public SaveResultException(string message, Exception innerException):
            this(message, innerException, null)
        {
            
        }


        public SaveResultException(string message, Exception innerException, SaveResult saveResult):
            base(message, innerException)
        {
            SaveResult = saveResult;
            
            Log(Messages);
        }

        public string Messages
        {
            get
            {
                var message = $"{Message}";
                var inner = InnerException;
                while (inner != null)
                {
                    message = string.Concat(message, "\r\n", inner.Message);
                    inner = inner.InnerException;
                }
                return message;
            }
        }

        private void Log(params string[] mensajes)
        {
            try
            {
                var rutaLogs = $"{AppDomain.CurrentDomain.BaseDirectory}logs";

                if (!System.IO.Directory.Exists(rutaLogs))
                {
                    System.IO.Directory.CreateDirectory(rutaLogs);
                }

                var path = $"{rutaLogs}\\log_{DateTime.Now.ToString("ddMMyyyy")}.txt";

                using (var sw = new System.IO.StreamWriter(path, true))
                {
                    sw.WriteLine();
                    sw.WriteLine($"-{DateTime.Now:ApplicationContext.DateTimeFormat}");
                    foreach (var mensaje in mensajes)
                    {
                        sw.WriteLine(mensaje);
                    }
                    var inner = InnerException;

                    sw.WriteLine($"Exception: {Message}");
                    sw.WriteLine($"StackTrace: {StackTrace}");

                    while (inner != null)
                    {
                        sw.WriteLine($"Exception: {inner.Message}");
                        sw.WriteLine($"StackTrace: {inner.StackTrace}");
                        inner = inner.InnerException;
                    }                    
                    sw.Close();
                }
            }
            catch
            {

            }
        }
    }
}
