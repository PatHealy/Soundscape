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
    public TextMeshPro timer1;
    public TextMeshPro timer2;
    public GameObject curtains;

    private void Awake() {
        instance = this;
        startTime = new System.DateTime(year, month, day, hour, minute, second);
        if (SceneManager.GetActiveScene().name.Contains("Asocial")) {
            startTime = System.DateTime.UtcNow;
        }
        double timeDif = CalculateTimeDifference().TotalSeconds;
        Debug.Log(timeDif);
        if (timeDif < 0) {
            //Debug.Log("Started timer");
            StartCoroutine(StartSequence());
        }
    }

    public System.TimeSpan CalculateTimeDifference() {
        return System.DateTime.UtcNow - startTime;
    }

    public void SetOffset(AudioSource a, double timeDif) {
        a.time = (float)(((timeDif % a.clip.length) + a.clip.length) % a.clip.length);
    }

    public float GetOffset(AudioSource a) {
        //Debug.Log("Getting offset!");
        double timeDif = CalculateTimeDifference().TotalSeconds;
        return (float)(((timeDif % a.clip.length) + a.clip.length) % a.clip.length);
    }

    IEnumerator StartSequence() {
        curtains.SetActive(true);
        yield return new WaitForEndOfFrame();
        System.TimeSpan ts = -CalculateTimeDifference();
        while (ts.TotalSeconds > 0) {
            timer1.text = ts.ToString().Substring(0, ts.ToString().LastIndexOf("."));
            timer2.text = timer1.text;
            yield return new WaitForSeconds(1f);
            ts = -CalculateTimeDifference();
        }

        timer1.text = "";
        timer2.text = timer1.text;
        curtains.SetActive(false);
    }
}
