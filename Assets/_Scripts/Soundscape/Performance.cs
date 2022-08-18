using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Performance : MonoBehaviour
{
    AudioSource[] sources;

    private void Awake() {
        if (sources == null) {
            sources = GetComponentsInChildren<AudioSource>(true);
        }
    }
    private void OnEnable() {
        if (sources == null) {
            sources = GetComponentsInChildren<AudioSource>(true);
        }
        if (TimeManager.instance != null) {
            float offset = TimeManager.instance.GetOffset(sources[0]);
            foreach (AudioSource a in sources) {
                a.Play();
                a.time = offset;
            }
        }
    }
}
