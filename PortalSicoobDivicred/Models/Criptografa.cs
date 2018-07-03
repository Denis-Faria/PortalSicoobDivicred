using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PortalSicoobDivicred.Models
{
    public class Criptografa
    {
        private static byte[] _chave = { };
        private static readonly byte[] Iv = {12, 34, 56, 78, 90, 102, 114, 126};

        private static readonly string chaveCriptografia = "fabricio1234567";

        //Criptografa o Cookie
        public static string Criptografar(string valor)
        {
            DESCryptoServiceProvider des;
            MemoryStream ms;
            CryptoStream cs;
            byte[] input;

            des = new DESCryptoServiceProvider();
            ms = new MemoryStream();

            input = Encoding.UTF8.GetBytes(valor);
            _chave = Encoding.UTF8.GetBytes(chaveCriptografia.Substring(0, 8));

            cs = new CryptoStream(ms, des.CreateEncryptor(_chave, Iv), CryptoStreamMode.Write);
            cs.Write(input, 0, input.Length);
            cs.FlushFinalBlock();

            return Convert.ToBase64String(ms.ToArray());
        }

        //Descriptografa o cookie
        public static string Descriptografar(string valor)
        {
            DESCryptoServiceProvider des;
            MemoryStream ms;
            CryptoStream cs;
            byte[] input;

            des = new DESCryptoServiceProvider();
            ms = new MemoryStream();

            input = Convert.FromBase64String(valor.Replace(" ", "+"));

            _chave = Encoding.UTF8.GetBytes(chaveCriptografia.Substring(0, 8));

            cs = new CryptoStream(ms, des.CreateDecryptor(_chave, Iv), CryptoStreamMode.Write);
            cs.Write(input, 0, input.Length);
            cs.FlushFinalBlock();

            return Encoding.UTF8.GetString(ms.ToArray());
        }
    }
}