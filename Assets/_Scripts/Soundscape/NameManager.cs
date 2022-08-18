using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameManager : MonoBehaviour
{
    public static NameManager instance;
    public string playerName;
    public Text txt;

    private void Awake() {
        instance = this;
        DontDestroyOnLoad(gameObject);
        SetName();
    }

    public void SetName() {
        playerName = txt.text;
    }
}
