using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    Transform player;

    private void OnEnable() {
        player = GameObject.Find("LocalPlayer").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.LookAt(player);
    }
}
