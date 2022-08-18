using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NoteManager : MonoBehaviour
{
    public static NoteManager instance;
    public GameObject notePrefab;
    public int spaceID;
    string getURL = "https://www.soundscape.social/get_notes/";
    string getRecentURL = "https://www.soundscape.social/get_recent_notes_with_id/"; //"http://www.soundscape.social/get_recent_notes/";
    string postURL = "https://www.soundscape.social/post_note";
    string nameURL = "https://www.soundscape.social/add_user/";
    public bool smallMode = false;

    List<int> ids;

    public GameObject[] curtains;

    private void Awake() {
        instance = this;
        ids = new List<int>();
    }

    private void Start() {
        InvokeRepeating("RetrieveNotesRepeat", 15f, 15f);
        RetrieveNotes();

        nameURL = nameURL + NameManager.instance.playerName;
        StartCoroutine(PostName());
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
            if (webRequest.isNetworkError) {
                Debug.Log("Connection failed");
            } else {
                //Debug.Log(webRequest.downloadHandler.text);
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
                        Debug.Log("Updating chat!");
                        ChatManager.instance.AddNote(PlayerNameManager.instance.GetName(note.ip) + note.content.Replace("/small",""));
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
            if (webRequest.isNetworkError) {
                Debug.Log("Connection failed");
            } else {
                Debug.Log(webRequest.downloadHandler.text);
            }
        }
    }

    public void PostNote(string txt, Vector3 loc) {
        if (txt == "musicircus") {
            foreach (GameObject g in curtains) {
                g.SetActive(false);
            }
        } else {
            GameObject n = Instantiate(notePrefab, loc, transform.rotation, transform);
            if (smallMode) {
                txt = txt + "/small";
                n.transform.localScale = Vector3.one * 0.02f;
            }
            n.GetComponent<Note>().SetUp(txt.Replace("/small",""));
            StartCoroutine(PublishNote(txt, loc));
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

    IEnumerator PostName() {
        yield return new WaitForFixedUpdate();
        using (UnityWebRequest webRequest = UnityWebRequest.Get(nameURL)) {
            yield return webRequest.SendWebRequest();
            if (webRequest.isNetworkError) {
                Debug.Log("Connection failed");
            } else {
                Debug.Log(webRequest.downloadHandler.text);
            }
        }
    }
}
