using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowTile : MonoBehaviour
{
    Material m;
    public Renderer secondR;
    Material second;
    
    private void Start() {
        m = GetComponent<Renderer>().material;
        second = secondR.material;
        StartCoroutine(ColorCycle());
    }

    IEnumerator ColorCycle() {
        yield return new WaitForEndOfFrame();
        Color[] colors = new Color[] { Color.red, Color.yellow, Color.green, Color.cyan, Color.blue, Color.magenta};
        int colorIndex = 0;
        m.color = colors[0];
        m.SetColor("_EmissionColor", colors[0]);
        while (true) {
            for (float k = 0f; k <= 1f; k += 0.01f) {
                //m.SetColor("Color", Color.Lerp(colors[colorIndex], colors[(colorIndex + 1) % colors.Length], k));
                m.color = Color.Lerp(colors[colorIndex], colors[(colorIndex + 1) % colors.Length], k);
                m.SetColor("_EmissionColor", Color.Lerp(colors[colorIndex], colors[(colorIndex + 1) % colors.Length], k));
                second.SetColor("_EmissionColor", Color.Lerp(colors[colorIndex], colors[(colorIndex + 1) % colors.Length], k));
                second.color = m.color;
                yield return new WaitForFixedUpdate();
            }
            colorIndex = (colorIndex + 1) % colors.Length;
        }
    }
}
