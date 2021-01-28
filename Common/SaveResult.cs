using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FCInformesSolucion.Common
{
    public class SaveResult
    {
        public int ErrorId { get; set; }
        public object Entity { get; set; }

        private bool _succeeded;
        public ICollection<string> Errors { get; private set; }
        protected SaveResult(bool success)
        {
            _succeeded = success;
        }
        
        public SaveResult(int errorId, ICollection<string> errors)
        {
            ErrorId = errorId;
            Errors = errors;
            _succeeded = false;
        }

        public SaveResult(ICollection<string> errors)
        {
            Errors = errors;
            _succeeded = false;
        }

        public SaveResult(params string[] errors)
        {
            Errors = errors;
            _succeeded = false;
        }

        public SaveResult(int errorId, params string[] errors)
        {
            ErrorId = errorId;
            Errors = errors;
            _succeeded = false;
        }

        

        public string GetErrorsString()
        {
            if (Errors != null)
            {
                return string.Join("<br />", Errors);
            }
            return "";
        }

        public SaveResultException Exception(string message = null)
        {
            _succeeded = false;

            if (message != null)
            {
                var aux = new List<string> { message };
                if(Errors != null)
                    aux.AddRange(Errors);
                Errors = aux;
            }

            if (Errors != null)
            {
                if (message != null)
                {
                    return new SaveResultException(message, new Exception(GetErrorsString()), this);
                }
                return new SaveResultException(GetErrorsString(), this);
            }
            return new SaveResultException(message, this);
        }


        public SaveResultException Exception(int errorId, string message = null)
        {
            _succeeded = false;
            ErrorId = errorId;

            if (message != null)
            {
                var aux = new List<string> { message };
                aux.AddRange(Errors);
                Errors = aux;
            }

            if (Errors != null)
            {
                if (message != null)
                {
                    return new SaveResultException(message, new Exception(GetErrorsString()), this);
                }
                return new SaveResultException(GetErrorsString(), this);
            }
            return new SaveResultException(message, this);
        }

        public bool Succeeded { get { return _succeeded; } }

        

        public static SaveResult Success()
        {
            return new SaveResult(true);
        }

        public static SaveResult Success(object entity)
        {
            return new SaveResult(true){Entity = entity}; 
        }

        public static SaveResult Failed(int errorId, params string[] errors)
        {
            return new SaveResult(errorId, errors);
        }

        public static SaveResult Failed(params string[] errors)
        {
            return new SaveResult(errors);
        }
        public static SaveResult Failed(ICollection<string> errors)
        {
            return new SaveResult(errors);
        }

        public static SaveResult Failed(IEnumerable<string> errors)
        {
            return new SaveResult(errors.ToList());
        }
    }
    
    public  static class SaveResultEx
    {
        public static SaveResult SaveResult(this Exception ex, ICollection<string> errs = null)
        {
            var errors = new List<string>();

            if (errs != null)
            {
                errors.AddRange(errs);
            }

            if (ex != null)
            {
                if (ex.Data.Contains("Errors"))
                {
                    var dataErrors = ex.Data["Errors"] as IEnumerable<string>;
                    if (dataErrors != null)
                    {
                        foreach (var dataError in dataErrors)
                        {
                            errors.Add(dataError);
                        }
                    }
                }
                else
                {
                    errors.Add(Convert.ToBoolean(ConfigurationManager.AppSettings["traceErrors"])
                        ? $"{ex.Message} {ex.StackTrace}"
                        : ex.Message);

                    var innerEx = ex.InnerException;
                    while (innerEx != null)
                    {
                        errors.Add(Convert.ToBoolean(ConfigurationManager.AppSettings["traceErrors"])
                            ? $"{innerEx.Message} {innerEx.StackTrace}"
                            : innerEx.Message);
                        innerEx = innerEx.InnerException;
                    }
                }
            }
            var result = new SaveResult(errors);
            return result;
        }

        public static string FullMessage(this Exception ex)
        {
            var message = ex.Message;
            var inner = ex.InnerException;

            while (inner != null)
            {
                message = string.Concat(message, "|" , inner.Message);

                inner = inner.InnerException;
            }
            return message;
        }
    }
}
