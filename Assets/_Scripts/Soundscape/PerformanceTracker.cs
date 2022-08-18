using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PerformanceTracker : MonoBehaviour
{
    public Transform cursor;
    public AudioSource a;
    float startX;
    float totalDistance;
    float time;

    public float[] timestamps;
    public string[] titles;

    public TextMeshPro txt;

    int ind = -1;

    private void Start() {
        startX = cursor.localPosition.x;
        totalDistance = -cursor.localPosition.x - cursor.localPosition.x;
    }

    private void FixedUpdate() {
        time = a.time;
        cursor.localPosition = new Vector3( ((time/a.clip.length) * totalDistance)+ startX, cursor.localPosition.y, cursor.localPosition.z);
        for (int i = 0; i < timestamps.Length; i++) {
            if (time > timestamps[i]) {
                if (i != timestamps.Length - 1) {
                    if (time < timestamps[i + 1] && ind != i) {
                        UpdatePlaque(i);
                    }
                } else if(ind != i) {
                    UpdatePlaque(i);
                }
            }
        }
    }

    void UpdatePlaque(int i) {
        ind = i;
        txt.text = "Now Playing: " + titles[i];
    }
}
