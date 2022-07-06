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
        public abstract T Encode(List<T> values);
        public abstract List<T> Decode(T n);
    }
    
    public class IntGodelEncoding : GodelEncoding<int> {
        public override int Encode(List<int> values) {
            return EncodeValues(values, s => { });
        }

        public override List<int> Decode(int n) {
            return DecodeInt(n, s => { });
        }
        
        public static int EncodeValues(List<int> values, Action<string> visitorAction = null) {
            visitorAction?.Invoke(nameof(EncodeValues));
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
        
        public static List<int> DecodeInt(int n, Action<string> visitorAction = null) {
            visitorAction?.Invoke("Logging not implemented yet.");
            var factors = PureMethods.GetPrimeFactorsOf(n, s => { });

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

    

    /*
    public class Wrapped<T> {
        public T TValue;
    }

    public class Int : Wrapped<int> {
        public int Value { get => TValue; }

        public Int(int value) {
            TValue = value;
        }
    }
    */
}