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
            Divide(dividend, Convert.ToInt32(divisor), out string result, out List<string> temps);
            Console.WriteLine($"{dividend} : {divisor} = {result}"); 
            foreach( string temp in temps )
            {
                Console.WriteLine(  temp);
            }
        }

        return rc;
    }
    static void Divide(string dividend, int divisor, out string result, out List<string> tempResults)
    {
        //
        // finden wieviel Stellen gebraucht werden bis der Divisor reinpasst
        //
        int stelle = DigitCount(divisor);
        string blanks;
        if ( Convert.ToInt32(dividend.Substring(0,stelle)) < divisor )
        {
            stelle += 1;
            blanks = " ";
        }
        else
        {
            blanks = "";
        }

        string strRest = dividend.Substring(0,stelle);
        
        result = String.Empty;
        tempResults = new List<string>();
        stelle -= 1;

        int padCount = DigitCount(divisor);
        do
        {
            //
            // passt wie oft rein?
            //
            long quotient = Math.DivRem(Convert.ToInt64(strRest), divisor, out long rest);
            result += quotient;

            strRest = rest.ToString();
            strRest = strRest.PadLeft(padCount,'0');
            ++stelle;

            if ( stelle < dividend.Length )
            {
                strRest += dividend[stelle];
            }
            else
            {
                strRest += "R";
            }

            
            tempResults.Add(blanks + strRest);
            blanks += " ";
        }
        while (stelle < dividend.Length);
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
