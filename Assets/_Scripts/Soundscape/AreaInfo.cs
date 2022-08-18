using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaInfo : MonoBehaviour
{
    public string donateInfo;
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && other.gameObject.name == "LocalPlayer") {
            DonateInfo.instance.SetDonateInfo(donateInfo);
        }
    }
}
