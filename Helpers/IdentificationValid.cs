using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FCInformesSolucion.Helpers
{
    public class IdentificationValidAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            var identification = value?.ToString() ?? "";
            var msg = "";

            if (ValidarRUC(identification, ref msg))
            {
                return ValidationResult.Success;
            }
            return new  ValidationResult(msg);
        }

        public static bool ValidarRUC(string Identificacion, ref string msj)
        {
            try
            {
                if (Identificacion == null)
                {
                    msj = "Ingrese identificación";
                    return false;
                }

                var suma = 0;
                var Residuo = 0;
                var NumeroProvincias = 24;
                var Modulo = 11;
                var Numero = Identificacion.Trim();
                int p1 = 0, p2 = 0, p3 = 0, p4 = 0, p5 = 0, p6 = 0, p7 = 0, p8 = 0, p9 = 0;

                if (Numero.Length < 10)
                {
                    msj = "Número de digitos ingresado no valido para cédula o ruc";
                    return false;
                }


                /* Los primeros dos digitos corresponden al codigo de la provincia */
                var Provincia = Convert.ToInt32(Numero.Substring(0, 2));
                if (Numero.Length == 13 && (Provincia < 1 || Provincia > NumeroProvincias))
                {
                    msj = "Dígito de verificación no valido";
                    return false;
                }

                ///* Aqui almacenamos los digitos de la cedula en variables. */
                var d1 = Convert.ToInt32(Numero.Substring(0, 1));
                var d2 = Convert.ToInt32(Numero.Substring(1, 1));
                var d3 = Convert.ToInt32(Numero.Substring(2, 1));
                var d4 = Convert.ToInt32(Numero.Substring(3, 1));
                var d5 = Convert.ToInt32(Numero.Substring(4, 1));
                var d6 = Convert.ToInt32(Numero.Substring(5, 1));
                var d7 = Convert.ToInt32(Numero.Substring(6, 1));
                var d8 = Convert.ToInt32(Numero.Substring(7, 1));
                var d9 = Convert.ToInt32(Numero.Substring(8, 1));
                var d10 = Convert.ToInt32(Numero.Substring(9, 1));

                /**
                 * Danner Marate 
                 * Validaciones que solo aplican para ruc
                 * **/
                var esRuc = false;
                if (Numero.Length == 13)
                {
                    esRuc = true;
                    /* Los primeros dos digitos corresponden al codigo de la provincia */
                    Provincia = Convert.ToInt32(Numero.Substring(0, 2));
                    if (esRuc && (Provincia < 1 || Provincia > NumeroProvincias))
                    {
                        msj = "Dígito de verificación no valido";
                        return false;
                    }
                }



                ///* El tercer digito es: */                           
                ///* 9 para sociedades privadas y extranjeros   */         
                ///* 6 para sociedades publicas */         

                //Solo aplica para la validacion de cedulas
                if (Numero.Length == 13 && (d3 == 7 || d3 == 8))
                {
                    msj = "Identificación incorrecta, verifique";
                    return false;
                }

                Func<int, int> calc = (d) =>
                {
                    var p = d * 2;
                    return p > 9 ? p - 9 : p;
                };

                var nat = false; //Natural
                var pub = false; //Publica
                var pri = false; //Privada

                /* menor que 6 (0,1,2,3,4,5) para personas naturales */
                if (d3 < 6) //Modulo 10
                {
                    nat = true;
                    p1 = calc(d1);
                    p2 = d2;
                    p3 = calc(d3);
                    p4 = d4;
                    p5 = calc(d5);
                    p6 = d6;
                    p7 = calc(d7);
                    p8 = d8;
                    p9 = calc(d9);
                    Modulo = 10;
                }
                else if (d3 == 6 && esRuc)   /* Solo para sociedades publicas (modulo 11) */
                {                   /* Aqui el digito verficador esta en la posicion 9, en las otras 2 en la pos. 10 */
                    pub = true;

                    p1 = d1 * 3;
                    p2 = d2 * 2;
                    p3 = d3 * 7;
                    p4 = d4 * 6;
                    p5 = d5 * 5;
                    p6 = d6 * 4;
                    p7 = d7 * 3;
                    p8 = d8 * 2;
                    p9 = 0;
                }
                else if (d3 == 9 && esRuc)
                {
                    pri = true;

                    p1 = d1 * 4;
                    p2 = d2 * 3;
                    p3 = d3 * 2;
                    p4 = d4 * 7;
                    p5 = d5 * 6;
                    p6 = d6 * 5;
                    p7 = d7 * 4;
                    p8 = d8 * 3;
                    p9 = d9 * 2;
                }

                suma = p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9;

                //retorna el valor del resto y nos da el residuo
                var ValorDecimales = (int)(suma / Modulo);
                Residuo = suma - (ValorDecimales * Modulo);

                /* Si residuo=0, &digitoVerificador=0, caso contrario 10 - residuo*/
                var DigitoVerificador = 0;

                if (Residuo == 0)
                {
                    DigitoVerificador = 0;
                }
                else
                {
                    DigitoVerificador = Modulo - Residuo;
                }

                if (pub)
                {
                    if (DigitoVerificador != d9)
                    {
                        msj = "El ruc de la empresa del sector público es incorrecto.";
                        return false;
                    }
                    /* El ruc de las empresas del sector publico terminan con 0001*/
                    var aux = Numero.Length == 13 ? Numero.Substring(9, 4) : Numero.Substring(9, Numero.Length - 9);
                    if (aux != "0001")
                    {
                        msj = "El ruc de la empresa del sector público debe terminar con 0001";
                        return false;
                    }
                }
                else if (pri)
                {
                    if (DigitoVerificador != d10)
                    {
                        msj = "El ruc de la empresa del sector privado es incorrecto.";
                        return false;
                    }

                    var aux = Numero.Length == 13 ? Numero.Substring(10, 3) : Numero.Substring(10, Numero.Length - 10);

                    if (aux != "001")
                    {
                        msj = "El ruc de la empresa del sector privado debe terminar con 001";
                        return false;
                    }
                }
                else if (nat)
                {
                    if (DigitoVerificador != d10)
                    {
                        msj = "El número de cédula de la persona natural es incorrecto.";
                        return false;
                    }
                    var aux = Numero.Length == 13 ? Numero.Substring(10, 3) : Numero.Substring(10, Numero.Length - 10);
                    if (Numero.Length > 10 && aux != "001")
                    {
                        msj = "El ruc de la persona natural debe terminar con 001";
                        return false;
                    }
                }
                return true;
            }
            catch
            {
                msj = "Identificación incorrecta";
                return true;
            }
        }
    }
}