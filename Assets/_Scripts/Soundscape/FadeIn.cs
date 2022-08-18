using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    Image im;
    // Start is called before the first frame update
    void Start()
    {
        im = GetComponent<Image>();
        StartCoroutine(Fade());
    }

    IEnumerator Fade() {
        yield return new WaitForEndOfFrame();
        for (float i = 0f; i <= 1f; i += 0.01f) {
            im.color = Color.Lerp(Color.black, Color.clear, i);
            yield return new WaitForFixedUpdate();
        }
    }

}
