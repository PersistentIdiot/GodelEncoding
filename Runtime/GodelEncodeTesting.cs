using System;
using System.Collections.Generic;
using GodelEncoding.Core;
using GodelEncoding.Utility;
using NaughtyAttributes;
using UnityEngine;

namespace GodelEncoding.Runtime {
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
            string logString = "LogString";
            IntGodelEncoder encoder = new IntGodelEncoder();
            encoder.Encode(_values, s => logString += s);
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
            string log = "";
            PureMethods.GetPrimeFactorsOf(_primeFactorizationTest, s => log += s);
            Debug.Log(log);
        }

        [Button]
        private void TestEncodeDecode() {
            IntGodelEncoder encoder = new IntGodelEncoder();
            // Encoding
            string logString = $"GodelEncoding({_values.ToCommaDelimitedString().Replace(".", "")}) ";

            string encodedValuesLog = "";
            var encodedInt = encoder.Encode(_values, s => encodedValuesLog += s);
            logString += $" = {encodedInt} => {encodedValuesLog} = {encodedInt}";

            // Line Break
            logString += "\n";

            // Decoding
            string decodedValuesLog = "";
            logString += $"GodelDecoding({encodedInt})";


            var decodedValues = encoder.Decode(encodedInt, s => decodedValuesLog += s);
            logString += $" = ({decodedValues.ToCommaDelimitedString().Replace(".", "")}) ";
            logString += $" => ";


            // Operator
            string x = "g";
            logString += $"({PureMethods.GetPrimeFactorsOf(encodedInt, s => { }).ToCommaDelimitedString().Replace(".", "")})";
            logString += $" {nameof(x)} ";
            logString += $"({decodedValues.ToCommaDelimitedString().Replace(".", "")}) ";



            logString += $"\n";
            var godelOperatorOutput = GodelOperations.GodelOperatorString(decodedValues, PureMethods.GetPrimeFactorsOf(encodedInt, s => { }));
            logString += $"GodelOperatorString: {godelOperatorOutput}";


            logString += $"\n";

            Func<string, string, string> stringExponentiation = (s1, s2) => $"{s1}^{s2}";
            Func<string, string, string> stringMultiplication = (s1, s2) => stringExponentiation.Invoke($"( [{s1}]", $"[{s2}] ) ");

            var output = GodelOperations.GodelOperator<string>(decodedValues, PureMethods.GetPrimeFactorsOf(encodedInt, s => { }),
                stringExponentiation, stringMultiplication);
            logString += $"GenericGodelOperator: {output}";

            Debug.Log(logString);
        }

        [Button]
        private void TestDecodeGodelEncodedInt() {
            GodelEncodedInt godelEncodedInt = new GodelEncodedInt(_values);
            var decodedValues = godelEncodedInt.DecodeGodelEncodedInt();

            bool different = _values.Count != decodedValues.Count;

            for (int i = 0; i < _values.Count; i++) {
                different &= _values[i] == decodedValues[i];
            }

            Debug.Log($"Different: {different}, _values: {_values.ToCommaDelimitedString()}, decodedValues: {decodedValues.ToCommaDelimitedString()}");
            
        }

        [Button]
        private void TestDecodeGodelEncodedIntTwo() {
            GodelEncodedInt godelEncodedInt = new GodelEncodedInt(_values);
        }
    }
}