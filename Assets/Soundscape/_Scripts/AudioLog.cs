using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLog : MonoBehaviour
{
    bool isOverlapping = false;
    AudioSource aus;
    GameObject prompt;
    bool playing = false;
    public float spinSpeed = 0.5f;

    AudioSource[] allAudios;

    private void Awake() {
        aus = GetComponent<AudioSource>();
        prompt = transform.Find("Prompt").gameObject;
        prompt.SetActive(false);
    }

    private void Update() {
        if (isOverlapping && Input.GetKeyDown(KeyCode.E)) {
            if (aus.isPlaying) {
                aus.Stop();
                //todo: handle spinning!
                allAudios = FindObjectsOfType<AudioSource>();
                foreach (AudioSource a in allAudios) {
                    a.volume = 1f;
                }
                playing = false;
            } else {
                aus.Play();
                //todo: handle spinning!
                allAudios = FindObjectsOfType<AudioSource>();
                foreach (AudioSource a in allAudios) {
                    a.volume = 0.3f;
                }
                aus.volume = 1f;
                playing = true;
            }
        }
    }

    private void FixedUpdate() {
        if (playing && !aus.isPlaying) {
            playing = false;
            
            allAudios = FindObjectsOfType<AudioSource>();
            foreach (AudioSource a in allAudios) {
                a.volume = 1f;
            }
        }

        if (aus.isPlaying) {
            transform.Rotate(new Vector3(0f, spinSpeed, 0f));
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            isOverlapping = true;
            prompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            isOverlapping = false;
            prompt.SetActive(false);
        }
    }
}
