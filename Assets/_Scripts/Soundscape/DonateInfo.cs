using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DonateInfo : MonoBehaviour
{
    public static DonateInfo instance;
    TextMeshProUGUI txt;
    Coroutine co;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        txt = GetComponent<TextMeshProUGUI>();
    }

    public void SetDonateInfo(string info) {

        if (co != null) {
            StopCoroutine(co);
        }
        txt.text = "";
        txt.color = Color.clear;
        co = StartCoroutine(LoadDonateInfo(info));
    }

    IEnumerator LoadDonateInfo(string info) {
        yield return new WaitForSeconds(30f);
        txt.text = info.Replace(". ", ". \n");
        for (float i = 0f; i <= 1f; i += 0.01f) {
            txt.color = Color.Lerp(Color.clear, Color.white, i);
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(40f);

        for (float i = 0f; i <= 1f; i += 0.01f) {
            txt.color = Color.Lerp(Color.white, Color.clear, i);
            yield return new WaitForFixedUpdate();
        }

        txt.text = "";
    }

    
}
