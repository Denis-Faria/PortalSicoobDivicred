using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PortalSicoobDivicred.Models
{
    public class Criptografa
    {
        private static byte[] chave = { };
        private static readonly byte[] iv = {12, 34, 56, 78, 90, 102, 114, 126};

        private static readonly string chaveCriptografia = "fabricio1234567";

        //Criptografa o Cookie
        public static string Criptografar(string valor)
        {
            DESCryptoServiceProvider des;
            MemoryStream ms;
            CryptoStream cs;
            byte[] input;

            try
            {
                des = new DESCryptoServiceProvider();
                ms = new MemoryStream();

                input = Encoding.UTF8.GetBytes(valor);
                chave = Encoding.UTF8.GetBytes(chaveCriptografia.Substring(0, 8));

                cs = new CryptoStream(ms, des.CreateEncryptor(chave, iv), CryptoStreamMode.Write);
                cs.Write(input, 0, input.Length);
                cs.FlushFinalBlock();

                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Descriptografa o cookie
        public static string Descriptografar(string valor)
        {
            DESCryptoServiceProvider des;
            MemoryStream ms;
            CryptoStream cs;
            byte[] input;

            try
            {
                des = new DESCryptoServiceProvider();
                ms = new MemoryStream();

                input = new byte[valor.Length];
                input = Convert.FromBase64String(valor.Replace(" ", "+"));

                chave = Encoding.UTF8.GetBytes(chaveCriptografia.Substring(0, 8));

                cs = new CryptoStream(ms, des.CreateDecryptor(chave, iv), CryptoStreamMode.Write);
                cs.Write(input, 0, input.Length);
                cs.FlushFinalBlock();

                return Encoding.UTF8.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}