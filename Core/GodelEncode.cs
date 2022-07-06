using System;
using System.Collections.Generic;
using GodelEncoding.Utility;
using UnityEngine;

namespace GodelEncoding.Core {
    /// List<int> => GodelEncodedInt
    public class GodelEncodedInt {
        public int Value { get; }

        public GodelEncodedInt(List<int> values) {
            IntGodelEncoder encoder = new IntGodelEncoder();
            Value = encoder.Encode(values);
        }

        public List<int> Decode() {
            return this.DecodeGodelEncodedInt();
        }
    }

    /// int -> GodelDecodedInts
    public class GodelDecodedInts {
        public List<int> Value { get; }

        public GodelDecodedInts(int n) {
            IntGodelEncoder encoder = new IntGodelEncoder();
            Value = encoder.Decode(n);
        }

        public int Encode() {
            return this.EncodeGodelDecodedInts();
        }
    }
    public static class GodelUtilities {
        // ToDo: Convert List<int> usages to GodelDecodedInts
        public static List<int> DecodeGodelEncodedInt(this GodelEncodedInt godelEncodedInt) {
            IntGodelEncoder encoder = new IntGodelEncoder();
            return encoder.Decode(godelEncodedInt.Value);
        }

        public static int EncodeGodelDecodedInts(this GodelDecodedInts godelDecodedInts) {
            IntGodelEncoder encoder = new IntGodelEncoder();
            return encoder.Encode(godelDecodedInts.Value);
        }
    }

    public abstract class GodelEncoder<T> {
        public abstract T Encode(List<T> values, Action<string> visitorAction = null);
        public abstract List<T> Decode(T n, Action<string> visitorAction = null);
    }

    public class IntGodelEncoder : GodelEncoder<int> {
        public override int Encode(List<int> values, Action<string> visitorAction = null) {
            visitorAction?.Invoke(nameof(Encode));
            double returnValue = 1;
            int count = values.Count;


            for (int i = 0; i < count; i++) {
                var @base = PureMethods.GetNthPrime(i);
                visitorAction?.Invoke($"( [{@base}]^");

                var exponent = values[i];
                visitorAction?.Invoke($"[{exponent}] )");
                returnValue *= Math.Pow(@base, exponent);
            }
            return (int)returnValue;
        }

        public override List<int> Decode(int n, Action<string> visitorAction = null) {
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