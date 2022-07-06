using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Windows;

namespace PI.Math.GodelEncoding {
    public abstract class GodelEncoding<T> {
        public abstract int Encode(Enum alphabet, List<T> values);
    }

    public class Wrapped<T> {
        public T TValue;
    }

    public class Int : Wrapped<int> {
        public int Value { get => TValue; }

        public Int(int value) {
            TValue = value;
        }
    }

    public class IntGodelEncoding : GodelEncoding<int> {
        /// <summary>
        /// Encodes <paramref name="values"/> using 
        /// </summary>
        /// <param name="alphabet">Alphabet to "use" ToDo: Make method without it, it's unnecessary</param>
        /// <param name="values">Values to encode using alphabet</param>
        /// <remarks>Godel Encoding: https://www.wikiwand.com/en/G%C3%B6del_numbering#/G%C3%B6del's_encoding</remarks>
        /// <returns></returns>
        public static int EncodeInts(Enum alphabet, List<int> values) {
            IntGodelEncoding encoder = new IntGodelEncoding();
            return encoder.Encode(alphabet, values);
        }

        public static int EncodeInts(Enum alphabet, List<int> values, out string log) {
            IntGodelEncoding encoder = new IntGodelEncoding();
            return encoder.Encode(alphabet, values, out log);
        }

        public static List<int> DecodeInt(int n, out string log) {
            log = "Let as an exercise to the reader";

            var factors = PureMethods.GetPrimeFactorsOf(n, out string factorsLog);

            // Inverse(?) Sieve, optimize later
            int encodedN = n;
            var returnValues = new List<int>();
            for (int i = 0; i < factors.Count; i++) {
                int count = 0;
                while (encodedN % factors[i] == 0) {
                    encodedN /= factors[i];
                    count++;
                }
                Debug.Assert(count >0);
                returnValues.Add(count);
            }
            return returnValues;
        }

        public override int Encode(Enum alphabet, List<int> values) {
            double returnValue = 1;
            int count = values.Count;

            for (int i = 0; i < count; i++) {
                var @base = PureMethods.GetNthPrime(i);
                var exponent = values[i];
                returnValue *= System.Math.Pow(@base, exponent);
            }
            return (int)returnValue;
        }

        public int Encode(Enum alphabet, List<int> values, out string log) {
            double returnValue = 1;

            int count = values.Count;

            string expansion = "";

            for (int i = 0; i < count; i++) {
                var @base = PureMethods.GetNthPrime(i);
                expansion += $"( [{@base}]^";

                var exponent = values[i];
                expansion += $"[{exponent}] )";
                returnValue *= System.Math.Pow(@base, exponent);
            }
            log = expansion;
            //log += "\n";
            //log += expansion + " = " + returnValue;
            return (int)returnValue;
        }
    }

    // ToDo: Move to new namespace
    // ToDo: Wrap int so it can be replaced later.
    public static class PureMethods {
        /// <summary>
        /// Creates a string of the form "1,2,3,4."
        /// <para>Example (List of int):  {1,2,3,4} => "1,2,3,4."</para>>
        /// </summary>
        /// <param name="delimiter">Delimiting string. ", " by default.</param>
        /// <param name="end">Ending string. "." by default.</param>
        /// <returns></returns>
        public static string ToCommaDelimitedString<T>(this IEnumerable<T> enumerable, string delimiter = ", ", string end = ".") {
            var list = enumerable.ToList();
            string returnString = "";

            for (int i = 0; i < list.Count; i++) {
                returnString += list[i];
                returnString += i + 1 < list.Count ? delimiter : end;
            }

            return returnString;
        }

        public static List<int> GetPrimeFactorsOf(int n, out string logString) {
            var primeFactors = GetPrimesLessThan(n);


            // Remove any primes that don't evenly divide n
            for (int i = primeFactors.Count - 1; i >= 0; i--) {
                if (n % primeFactors[i] != 0) {
                    //Debug.Log($"{nameof(GodelEncodeTesting)}.{nameof(GetPrimeFactorsOf)}() - {primeFactors[i]}%{n} != 0, removing");
                    primeFactors.RemoveAt(i);
                }
            }

            logString = primeFactors.ToCommaDelimitedString();

            return primeFactors;
        }

        /// List of Primes
        public static List<int> Primes
        {
            get
            {

                if (_primes == null) {
                    var primeFile = Resources.Load<TextAsset>("Primes");
                    Debug.Assert(primeFile != null);
                    string[] primeStrings = Regex.Split(primeFile.text, ",");

                    _primes = new List<int>();

                    foreach (string primeString in primeStrings) {
                        if (int.TryParse(primeString, out int primeNumber)) {
                            _primes.Add(primeNumber);
                        }
                        else {
                            Debug.Log($"{nameof(PureMethods)}.{nameof(Primes)}() - Failed to parse {primeString}!");
                        }
                    }
                }
                return _primes;
            }
        }
        private static List<int> _primes;

        /// Returns the Nth prime
        public static int GetNthPrime(int n) {
            return Primes[n];
        }

        /// <summary>
        /// Get a list of all prime integers less than n.
        /// </summary>
        /// <remarks>Uses Sieve of Eratosthenes</remarks>
        public static List<int> GetPrimesLessThan(int n) {
            var returnValue = new List<int>();

            var s = SieveOfEratosthenes(n);

            while (s.MoveNext()) {
                returnValue.Add((int)s.Current!);
            }

            return returnValue;

            // https://codereview.stackexchange.com/questions/82863/sieve-of-eratosthenes-in-c
            IEnumerator SieveOfEratosthenes(int upperLimit) {
                //BitArray works just like a bool[] but takes up a lot less space.
                BitArray composite = new BitArray(upperLimit);

                //Only need to cross off numbers up to sqrt.
                int sqrt = (int)Mathf.Sqrt(upperLimit);

                for (int p = 2; p <= sqrt; ++p) {
                    if (composite[p]) continue; //The number is crossed off; skip it

                    yield return p; //Not crossed off means it's prime. Return it.

                    //Cross off each multiple of this prime
                    //Start at the prime squared, because lower numbers will
                    //have been crossed off already. No need to check them.
                    for (int i = p * p; i < upperLimit; i += p)
                        composite[i] = true;
                }

                //The remaining numbers not crossed off are also prime.
                for (int p = sqrt + 1; p < upperLimit; ++p) {
                    if (!composite[p]) yield return p;
                }
            }
        }
    }
}