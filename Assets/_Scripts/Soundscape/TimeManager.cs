using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    public System.DateTime startTime;
    public int year, month, day, hour, minute, second;
    public bool startToday = false;

    private void Awake() {
        instance = this;
        startTime = new System.DateTime(year, month, day, hour, minute, second);

        if (startToday) {
            System.DateTime now = System.DateTime.UtcNow;
            startTime = new System.DateTime(now.Year, now.Month, now.Day, hour, minute, second);
        }
    }

    public System.TimeSpan CalculateTimeDifference() {
        return System.DateTime.UtcNow - startTime;
    }

    public void SetOffset(AudioSource a, double timeDif) {
        a.time = (float)(((timeDif % a.clip.length) + a.clip.length) % a.clip.length);
    }

    public float GetOffset(AudioSource a) {
        double timeDif = CalculateTimeDifference().TotalSeconds;
        return (float)(((timeDif % a.clip.length) + a.clip.length) % a.clip.length);
    }
}
