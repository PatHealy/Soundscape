using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatManager : MonoBehaviour
{
    public static ChatManager instance;
    Coroutine co;
    TextMeshProUGUI txt;
    bool chatActive = false;
    string[] notes;
    int locks = 0;

    private void Start() {
        instance = this;
        txt = GetComponent<TextMeshProUGUI>();
        notes = new string[8];
        ClearNotes();
    }

    void ShiftNotes() {
        for (int i = notes.Length - 1; i > 0; i--) {
            notes[i] = notes[i - 1];
        }
    }

    void PrintNotes() {
        for (int i = 0; i < notes.Length; i++) {
            Debug.Log(notes[i]);
        }
    }

    public void AddNote(string s) {
        if (co != null) {
            StopCoroutine(co);
        }

        ShiftNotes();
        notes[0] = s;

        txt.text = "";
        for (int i = notes.Length - 1; i >= 0; i--) {
            txt.text = txt.text + notes[i] + "\n";
        }

        co = StartCoroutine(NoteAdd());
    }

    void ClearNotes() {
        for (int i = 0; i < notes.Length; i++) {
            notes[i] = "";
        }
    }

    IEnumerator NoteAdd() {
        yield return new WaitForEndOfFrame();
        if (!chatActive) {
            chatActive = true;
            for (float i = 0f; i <= 1f; i += 0.01f) {
                txt.color = Color.Lerp(Color.clear, Color.white, i);
                yield return new WaitForFixedUpdate();
            }
        }
        
        yield return new WaitForSeconds(30f);
        yield return new WaitForFixedUpdate();

        chatActive = false;
        for (float i = 0f; i <= 1f; i += 0.01f) {
            txt.color = Color.Lerp(Color.white, Color.clear, i);
            yield return new WaitForFixedUpdate();
        }
        ClearNotes();
    }
}
