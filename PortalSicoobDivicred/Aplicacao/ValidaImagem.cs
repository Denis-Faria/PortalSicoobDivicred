using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

public sealed class ValidaImagem
{


    public bool IsValidImage(byte[] bytes)
    {
        try
        {
            using(MemoryStream ms = new MemoryStream( bytes ))
                Image.FromStream( ms );
        }
        catch(ArgumentException)
        {
            return false;
        }
        return true;
    }
}