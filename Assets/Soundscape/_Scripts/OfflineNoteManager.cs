using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineNoteManager : MonoBehaviour
{
    public static OfflineNoteManager instance;
    public GameObject notePrefab;
    List<int> ids;
    public TextAsset notedump;
    public bool smallMode = false;

    private void Awake() {
        instance = this;
        ids = new List<int>();
    }

    private void Start() {
        RetrieveNotes();
    }

    public void RetrieveNotes() {
        StartCoroutine(GetNotes(true));
    }

    IEnumerator GetNotes(bool isInitial) {
        yield return new WaitForEndOfFrame();
        if (notedump != null) {
            NotePull np = JsonUtility.FromJson<NotePull>(notedump.text);
            PlaceNotes(np.notes);
        }
    }

    void PlaceNotes(NoteData[] notes) {
        for (int i = notes.Length - 1; i >= 0; i--) {
            NoteData note = notes[i];
            try {
                if (!ids.Contains(note.id)) {
                    float x = float.Parse(note.x);
                    float y = float.Parse(note.y);
                    float z = float.Parse(note.z);
                    CreateNote(note.content, new Vector3(x, y, z), note.id);
                    ids.Add(note.id);
                }
            } catch {
                Debug.Log("Error parsing note.");
            }
        }
    }

    public void CreateNote(string txt, Vector3 loc, int id) {
        GameObject n = Instantiate(notePrefab, loc, transform.rotation, transform);
        if (txt.Contains("/small")) {
            n.transform.localScale = Vector3.one * 0.02f;
            txt = txt.Replace("/small", "");
        }
        n.GetComponent<Note>().SetUp(txt, id);
    }

    public void PostNote(string txt, Vector3 loc) {
        GameObject n = Instantiate(notePrefab, loc, transform.rotation, transform);
        if (smallMode) {
            n.transform.localScale = Vector3.one * 0.02f;
        }
        n.GetComponent<Note>().SetUp(txt);
    }
}
