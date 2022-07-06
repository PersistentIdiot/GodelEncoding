using System;
using System.Collections.Generic;
using PI.Math.GodelEncoding;
using TMPro;
using UnityEngine;

public enum Alphabet{A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z}
public class GodelEncodeTesting : MonoBehaviour {
    [SerializeField] List<int> _values = new List<int> {1, 2};
    private Alphabet _alphabet;

    private void EnsureInit() {
        if (_values == null) {
            Debug.Log($"{nameof(GodelEncodeTesting)}.{nameof(EnsureInit)}() - Setting _values");
            _values = new List<int> {1, 2};
        }
    }
    
    private void OnValidate() {
        EnsureInit();
        IntGodelEncoding.EncodeInts(_alphabet, _values, out string logString);
        Debug.Log(logString);
    }
}