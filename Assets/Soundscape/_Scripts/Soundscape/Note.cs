using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Note : MonoBehaviour
{
    public TextMeshPro tmp;
    public Transform model;
    float counter = 0f;
    public int id;

    public void SetUp(string txt) {
        tmp.text = txt;
        tmp.transform.parent.gameObject.SetActive(false);
        transform.Rotate(new Vector3(0f, Random.Range(0f,180f), 0f));
    }

    public void SetUp(string txt, int i) {
        id = i;
        SetUp(txt);
    }

    private void OnMouseOver() {
        counter += 0.1f;
        if (counter >= 1.3f) {
            counter = 1.3f;
        }
    }

    private void FixedUpdate() {
        if (counter != 0f) {
            counter -= 0.1f;
            if (counter < 0f) {
                counter = 0f;
            }
            if (counter >= 1f && !tmp.transform.parent.gameObject.activeSelf) {
                tmp.transform.parent.gameObject.SetActive(true);
            } else if (counter < 1f && tmp.transform.parent.gameObject.activeSelf) {
                tmp.transform.parent.gameObject.SetActive(false);
            }
            model.transform.localPosition = new Vector3(0f, (counter/8f)*transform.localScale.y, 0f);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.CompareTag("Note")) {
            if (collision.collider.gameObject.GetComponent<Note>().id > id && !SceneManager.GetActiveScene().name.Contains("Asocial")) {
                Debug.Log("Destroyed");
                Destroy(gameObject);
            } else if (SceneManager.GetActiveScene().name.Contains("Asocial")) {
                transform.Translate(new Vector3(Random.value, 0f, Random.value));
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("NoteTrigger")) {
            counter += 0.1f;
            if (counter >= 1.3f) {
                counter = 1.3f;
            }
        }
    }
}
