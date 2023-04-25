using System.Linq.Expressions;
using System.Text;
using System;
using System.Linq;
using System.Collections.Generic;

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
            Divide(dividend, Convert.ToInt32(divisor), out string result, out StringBuilder temps);
            Console.WriteLine("{0} : {1} = {2}\n{3}",
                dividend
                ,divisor
                ,result
                ,temps); 
            
        }

        return rc;
    }
    static void Divide(string dividend, int divisor, out string result, out StringBuilder tempResults)
    {
        //var dividendIter = dividend.AsSpan().GetEnumerator();
        //
        // finden wieviel Stellen gebraucht werden bis der Divisor reinpasst
        //
        int lenDivisor = DigitCount(divisor);
        int padCount = lenDivisor;

        int idxDividend = lenDivisor - 1;
        int spaces;
        //
        // sind die ersten Ziffern (lenDivisor) kleiner als der Divisor selbst?
        //
        if ( Convert.ToInt32(dividend.Substring(0,lenDivisor)) < divisor )
        {
            // dann nehmen wir noch eine Stelle vom Dividend dazu
            //  für die erste Division
            idxDividend += 1;
            spaces = 1;
        }
        else
        {
            spaces = 0;
        }

        string strRest = dividend.Substring(0,idxDividend+1);
        
        result = String.Empty;
        tempResults = new StringBuilder();

        do
        {
            //
            // passt wie oft rein?
            //
            long quotient = Math.DivRem(Convert.ToInt64(strRest), divisor, out long rest);
            result += quotient;

            strRest = rest.ToString();
            strRest = strRest.PadLeft(padCount,'0');
            ++idxDividend;

            if ( idxDividend < dividend.Length )
            {
                strRest += dividend[idxDividend];
            }
            else
            {
                strRest += "R";
            }
            tempResults.Append    (new string(' ', spaces));
            tempResults.AppendLine(strRest);
            ++spaces;
        }
        while (idxDividend < dividend.Length);
    }

    static int DigitCount(long val)
    {
        return val.ToString().Length;
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
            /*else if ( Convert.ToInt32(divisor) )
            {
                err = "Der Divisor darf nur einstellig sein";
            }*/
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
