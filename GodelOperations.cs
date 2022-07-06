using System;
using System.Collections.Generic;
using PI.Math.GodelEncoding;
using UnityEngine;


// Godel Operator
// Dot product? Does more than a dot product. Like a dot product equipped with a Func.
// ToDo: https://www.wikiwand.com/en/G%C3%B6del_operation - See if related
// ToDo: GodelOperator(List<int> values, List<int> alphabet, Func<int, int, int> exponentiation, Func<int, int, int>)
public static class GodelOperations {
    /*
    public T GodelOperator<T>() {
        return GodelOperatorString();
    }
    */

    public static string GodelOperator<T>(List<int> values, List<int> alphabet, Func<T, T, T> inner, Func<T, T, T> outer) {

        
        
        Func<string, string, string> stringExponentiation = (s1, s2) => $"{s1}^{s2}";
        Func<string, string, string> stringMultiplication = (s1, s2) => stringExponentiation.Invoke($"( [{s1}]",$"[{s2}] ) ");

        
        
        
        Debug.Assert(values.Count == alphabet.Count);
        string returnString = "";

        returnString += $"({values.ToCommaDelimitedString().Replace(".", "")})";
        returnString += $" x ";
        returnString += $"({alphabet.ToCommaDelimitedString().Replace(".", "")})";

        returnString += $" => ";

        string outputString = "";
        for (int i = 0; i < values.Count; i++) {
            outputString += stringMultiplication.Invoke(alphabet[i].ToString(), values[i].ToString());
        }
        
        /*
        string outputString = "";
        for (int i = 0; i < values.Count; i++) {
            outputString += $" ([{alphabet[i]}]^[{values[i]}]) ";
        }
        */

        returnString += outputString;
        
        
        return returnString;
    }

    public static string GodelOperator(List<int> values, List<int> alphabet) {

        Func<string, string, string> stringExponentiation = (s1, s2) => $"{s1}^{s2}";
        Func<string, string, string> stringMultiplication = (s1, s2) => stringExponentiation.Invoke($"( [{s1}]",$"[{s2}] ) ");

        Debug.Assert(values.Count == alphabet.Count);
        string returnString = "";

        returnString += $"({values.ToCommaDelimitedString().Replace(".", "")})";
        returnString += $" x ";
        returnString += $"({alphabet.ToCommaDelimitedString().Replace(".", "")})";

        returnString += $" => ";

        string outputString = "";
        for (int i = 0; i < values.Count; i++) {
            outputString += stringMultiplication.Invoke(alphabet[i].ToString(), values[i].ToString());
        }
        
        /*
        string outputString = "";
        for (int i = 0; i < values.Count; i++) {
            outputString += $" ([{alphabet[i]}]^[{values[i]}]) ";
        }
        */

        returnString += outputString;
        
        
        return returnString;
    }
    
    public static string GodelOperatorString(List<int> values, List<int> alphabet) {
        Debug.Assert(values.Count == alphabet.Count);
        string returnString = "";

        returnString += $"({values.ToCommaDelimitedString().Replace(".", "")})";
        returnString += $" x ";
        returnString += $"({alphabet.ToCommaDelimitedString().Replace(".", "")})";

        returnString += $" => ";

        string outputString = "";
        for (int i = 0; i < values.Count; i++) {
            outputString += $" ([{alphabet[i]}]^[{values[i]}]) ";
        }

        returnString += outputString;
        
        
        return returnString;
    }
}