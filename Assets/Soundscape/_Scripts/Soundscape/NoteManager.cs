using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NoteManager : MonoBehaviour
{
    public static NoteManager instance;
    public GameObject notePrefab;
    public int spaceID;
    public string note_server_url;
    string getURL;
    string getRecentURL;
    string postURL;
    string nameURL;

    List<int> ids;

    private void Awake() {
        getURL = note_server_url + "get_notes/";
        getRecentURL = note_server_url + "get_recent_notes_with_id/";
        postURL = note_server_url + "post_note";
        nameURL = note_server_url + "add_user/";
        instance = this;
        ids = new List<int>();
    }

    private void Start() {
        InvokeRepeating("RetrieveNotesRepeat", 15f, 15f);
        RetrieveNotes();

        if (NameManager.instance != null)
        {
            nameURL = nameURL + NameManager.instance.playerName;
            StartCoroutine(PostName());
        }
    }

    public void RetrieveNotes() {
        StartCoroutine(GetNotes(true));
    }

    public void RetrieveNotesRepeat() {
        StartCoroutine(GetNotes(false));
    }

    IEnumerator GetNotes(bool isInitial) {
        string fullUrl = getURL + spaceID;
        if (!isInitial) {
            fullUrl = getRecentURL + spaceID;
        }
        yield return new WaitForSeconds(1f);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(fullUrl)) {
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError) {
                Debug.Log("Connection failed");
            } else {
                NotePull np = JsonUtility.FromJson<NotePull>(webRequest.downloadHandler.text);
                PlaceNotes(np.notes);
            }
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
                    if (Time.time > 5f) {
                        ChatManager.instance.AddNote(PlayerNameManager.instance.GetName(note.ip) + note.content);
                    }
                }
            } catch {
                Debug.Log("Error parsing note.");
            }
        }
    }

    IEnumerator PublishNote(string txt, Vector3 loc) {
        WWWForm form = new WWWForm();
        form.AddField("space", spaceID);
        form.AddField("x", loc.x.ToString("F4"));
        form.AddField("y", loc.y.ToString("F4"));
        form.AddField("z", loc.z.ToString("F4"));
        form.AddField("content", txt);
        using (UnityWebRequest webRequest = UnityWebRequest.Post(postURL, form)) {
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Connection failed");
            } else {
                Debug.Log(webRequest.downloadHandler.text);
            }
        }
    }

    public void PostNote(string txt, Vector3 loc) {
        GameObject n = Instantiate(notePrefab, loc, transform.rotation, transform);
        n.GetComponent<Note>().SetUp(txt);
        StartCoroutine(PublishNote(txt, loc));
    }

    public void CreateNote(string txt, Vector3 loc, int id) {
        GameObject n = Instantiate(notePrefab, loc, transform.rotation, transform);
        n.GetComponent<Note>().SetUp(txt, id);
    }

    IEnumerator PostName() {
        yield return new WaitForFixedUpdate();
        using (UnityWebRequest webRequest = UnityWebRequest.Get(nameURL)) {
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Connection failed");
            } else {
                Debug.Log(webRequest.downloadHandler.text);
            }
        }
    }
}
