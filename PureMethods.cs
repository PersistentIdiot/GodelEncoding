// ToDo: Move to new namespace
// ToDo: Wrap int so it can be replaced later.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public static class PureMethods {
    
    /// List of Primes ToDo: Move to repository of some kind
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

    //
    public static List<int> GetPrimeFactorsOf(int n, Action<string> logAction) {
        var primeFactors = PrimeCountingFunctionInt(n);


        // Remove any primes that don't evenly divide n
        for (int i = primeFactors.Count - 1; i >= 0; i--) {
            if (n % primeFactors[i] != 0) {
                primeFactors.RemoveAt(i);
            }
        }

        logAction.Invoke(primeFactors.ToCommaDelimitedString());

        return primeFactors;
    }

    

    /// Returns the Nth prime
    public static int GetNthPrime(int n) {
        return Primes[n];
    }

    /// <summary>
    /// Get a list of all prime integers less than n.https://www.wikiwand.com/en/Prime-counting_function
    /// </summary>
    /// <remarks>Implemented using Sieve of Eratosthenes</remarks>
    public static List<int> PrimeCountingFunctionInt(int n) {
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