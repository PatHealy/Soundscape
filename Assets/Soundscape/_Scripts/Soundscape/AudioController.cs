using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {
    public Transform[] audioSpectrumObjects;
    public float heightMultiplier;
    public int numberOfSamples = 10;
    public FFTWindow fftWindow;
    public float lerpTime = 1f;

    private AudioSource audioSource;

    private void OnEnable() {
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate() {
        float[] spectrum = new float[numberOfSamples];
        audioSource.GetSpectrumData(spectrum, 0, fftWindow); // for normal builds

        float av = 0f;

        for (var i = 0; i < spectrum.Length; i++) {
            av += spectrum[i];
        }
        av = av / spectrum.Length;

        audioSpectrumObjects[0].localScale = Vector3.one * (1 + av*heightMultiplier);
    }
}
