using System;
using System.Collections.Generic;
using NaughtyAttributes;
using PI.Math.GodelEncoding;
using TMPro;
using UnityEngine;

public class GodelEncodeTesting : MonoBehaviour {
    [SerializeField] List<int> _values = new() {1, 2, 3};

    [SerializeField]
    private int _primeFactorizationTest = 2 * 3 * 5;

    private void EnsureInit() {
        if (_values == null) {
            Debug.Log($"{nameof(GodelEncodeTesting)}.{nameof(EnsureInit)}() - Setting _values");
            _values = new List<int> {1, 2};
        }
    }

    private void OnValidate() {
        EnsureInit();
    }

    [Button]
    private void TestEncodeInts() {
        string logString="LogString";
        IntGodelEncoding.EncodeValues(_values, s => logString += s);
        //IntGodelEncoding.EncodeValues(_values, out string logString);
        Debug.Log(logString);
    }

    [Button]
    private void TestGetPrimesLessThan() {
        var factorization = PureMethods.PrimeCountingFunctionInt(_primeFactorizationTest);
        var logString = $"Primes less than {_primeFactorizationTest}: {factorization.ToCommaDelimitedString()}";
        Debug.Log(logString);
    }

    [Button]
    private void TestGetPrimeFactors() {
        PureMethods.GetPrimeFactorsOf(_primeFactorizationTest, out string log);
        Debug.Log(log);
    }

    [Button]
    private void TestEncodeDecode() {
        // Encoding
        string logString = $"GodelEncoding({_values.ToCommaDelimitedString().Replace(".", "")}) ";

        string encodedValuesLog = "";
        var encodedInt = IntGodelEncoding.EncodeValues(_values,s => encodedValuesLog += s);
        logString += $" = {encodedInt} => {encodedValuesLog} = {encodedInt}";

        // Line Break
        logString += "\n";

        // Decoding
        string decodedValuesLog = "";
        logString += $"GodelDecoding({encodedInt})";
        var decodedValues = IntGodelEncoding.DecodeInt(encodedInt,s => decodedValuesLog += s );
        logString += $" = ({decodedValues.ToCommaDelimitedString().Replace(".", "")}) ";
        logString += $" => ";


        // Operator
        string x = "g";
        logString += $"({PureMethods.GetPrimeFactorsOf(encodedInt, out string _).ToCommaDelimitedString().Replace(".", "")})";
        logString += $" {nameof(x)} ";
        logString += $"({decodedValues.ToCommaDelimitedString().Replace(".", "")}) ";



        logString += $"\n";
        var godelOperatorOutput = GodelOperations.GodelOperatorString(decodedValues, PureMethods.GetPrimeFactorsOf(encodedInt, out string _));
        logString += $"GodelOperatorString: {godelOperatorOutput}";


        logString += $"\n";

        Func<string, string, string> stringExponentiation = (s1, s2) => $"{s1}^{s2}";
        Func<string, string, string> stringMultiplication = (s1, s2) => stringExponentiation.Invoke($"( [{s1}]", $"[{s2}] ) ");

        var output = GodelOperations.GodelOperator<string>(decodedValues, PureMethods.GetPrimeFactorsOf(encodedInt, out string _),
            stringExponentiation, stringMultiplication);
        logString += $"GenericGodelOperator: {output}";

        Debug.Log(logString);
    }
}