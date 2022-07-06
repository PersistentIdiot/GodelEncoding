using System;
using System.Collections.Generic;
using NaughtyAttributes;
using PI.Math.GodelEncoding;
using TMPro;
using UnityEngine;

public enum Alphabet {
    A,
    B,
    C,
    D,
    E,
    F,
    G,
    H,
    I,
    J,
    K,
    L,
    M,
    N,
    O,
    P,
    Q,
    R,
    S,
    T,
    U,
    V,
    W,
    X,
    Y,
    Z
}

public class GodelEncodeTesting : MonoBehaviour {
    [SerializeField] List<int> _values = new() {1, 2, 3};

    [SerializeField]
    private int _primeFactorizationTest = 2 * 3 * 5;

    private Alphabet _alphabet;

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
        IntGodelEncoding.EncodeInts(_alphabet, _values, out string logString);
        Debug.Log(logString);
    }

    [Button]
    private void TestGetPrimesLessThan() {
        var factorization = PureMethods.GetPrimesLessThan(_primeFactorizationTest);
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
        string logString = $"Encoding({_values.ToCommaDelimitedString().Replace(".", "")}) ";
        var encodedValue = IntGodelEncoding.EncodeInts(_alphabet, _values, out string encodedValuesLog);
        logString += " = ";
        logString += encodedValue;
        logString += " => ";
        logString += encodedValuesLog;
        logString += "= " + encodedValue;

        logString += "\n";

        // Decoding - Expected: {1,2,3}
        logString += $"Decoding({encodedValue})";
        logString += $" =?= ";
        logString += IntGodelEncoding.DecodeInt(encodedValue, out string decodedValuesLog).ToCommaDelimitedString();

        Debug.Log(logString);
    }
}