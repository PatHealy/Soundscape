using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour
{
    TextMeshProUGUI txt;

    // Start is called before the first frame update
    void Start()
    {
        txt = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (txt.text == "WASD to move" && (Input.GetKeyDown(KeyCode.W))) {
            txt.text = "Arrows to look around";
        } else if (txt.text == "Arrows to look around" && Input.GetKeyDown(KeyCode.UpArrow)) {
            txt.text = "Spacebar to jump";
        } else if (txt.text == "Spacebar to jump" && (Input.GetKeyDown(KeyCode.Space))) {
            txt.text = "T to write a message";
        } else if (txt.text == "T to write a message" && Input.GetKeyDown(KeyCode.T)) {
            txt.text = "Enter to post, Escape to cancel";
        } else if (txt.text == "Enter to post, Escape to cancel" && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return))) {
            txt.text = "";
            Destroy(gameObject);
        }
    }
}
