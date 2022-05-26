using System;
using System.Linq;

namespace Spi;

class Division
{
    public static int Main(string[] args)
    {
        int rc = 99;
        if (!ReadArgs(args, out string dividend, out string divisor))
        {
            rc = 4;
        }
        else
        {
            Divide(dividend, Convert.ToInt32(divisor)); 
        }

        return rc;
    }
    static void Divide(string dividend, int divisor)
    {
        string rechnung = $"{dividend} : {divisor} = ";
        Console.WriteLine(rechnung);

        int i=0;
        int tmp = Convert.ToInt32(dividend.Substring(0,1));
        if (tmp < divisor)
        {
            ++i;
            tmp = Convert.ToInt32(dividend.Substring(0,2));
        }

        string line;
        string strTmp;
        bool end = false;
        do
        {
            int quotient = Math.DivRem(tmp, divisor, out int remainder);
            strTmp = remainder.ToString();
            ++i;
            if ( i < dividend.Length )
            {
                strTmp += dividend[i];
                tmp = Convert.ToInt32(strTmp);
            }
            else
            {
                strTmp += "R";
                end = true;
            }
            line = new string(' ',i-1) + strTmp;
            line = line.PadRight(rechnung.Length) + quotient;
            Console.WriteLine(line);
        }
        while (!end);
    }

    static bool isAllDigits(string str)
    {
        return str.All(c => c >= '0' && c <= '9');
    }

    static bool ReadArgs(string[] args, out string dividend, out string divisor)
    {
        string err = String.Empty;

        dividend = divisor = String.Empty;
        if ( args.Length != 2)
        {
            err = "Bitte Divident und Divisor eingeben";
        }
        else
        {
            dividend = args[0];
            divisor  = args[1];
            if ( !(isAllDigits(dividend) && isAllDigits(divisor)) ) 
            {
                err = "Divident und Divisor müssen Zahlen sein";
            }
            else if (divisor.Length > 1)
            {
                err = "Der Divisor darf nur einstellig sein";
            }
            else if ( "0".Equals(divisor) )
            {
                err = "Divison durch Null scheidet völlig aus";
            }
            else if ( dividend.Length == 1 && dividend[0] < divisor[0] )
            {
                err = "Dividend muß größer Divisor sein";
            }
        }

        if ( string.IsNullOrEmpty(err) )
        {
            return true;
        }
        else
        {
            Console.Error.WriteLine(err);
            return false;
        }
    }
}
