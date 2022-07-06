using System;
using System.Collections.Generic;
using GodelEncoding.Utility;
using UnityEngine;

namespace GodelEncoding.Core {
    public abstract class GodelEncoding<T> {
        public abstract T Encode(List<T> values, Action<string> visitorAction);
        public abstract List<T> Decode(T n, Action<string> visitorAction);
    }

    public class IntGodelEncoding : GodelEncoding<int> {
        public override int Encode(List<int> values, Action<string> visitorAction) {
            visitorAction?.Invoke(nameof(Encode));
            double returnValue = 1;
            int count = values.Count;


            for (int i = 0; i < count; i++) {
                var @base = PureMethods.GetNthPrime(i);
                visitorAction?.Invoke($"( [{@base}]^");

                var exponent = values[i];
                visitorAction?.Invoke($"[{exponent}] )");
                returnValue *= System.Math.Pow(@base, exponent);
            }
            return (int)returnValue;
        }

        public override List<int> Decode(int n, Action<string> visitorAction) {
            visitorAction?.Invoke("Logging not implemented yet.");
            var factors = PureMethods.GetPrimeFactorsOf(n);

            // Inverse Sieve(?), optimize later
            var values = new List<int>();

            for (int i = 0; i < factors.Count; i++) {
                int count = 0;

                while (n % factors[i] == 0) {
                    n /= factors[i];
                    count++;
                }
                Debug.Assert(count > 0);
                values.Add(count);
            }
            return values;
        }
    }
}