using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkInstancer : MonoBehaviour
{
    private NetworkManager netManager;
    public enum NetworkState {Server, Host, Client};
    public NetworkState state;

    private void Start()
    {
        netManager = GetComponent<NetworkManager>();
        if (state == NetworkState.Server) {
            netManager.StartServer();
        } else if (state == NetworkState.Host) {
            netManager.StartHost();
        } else {
            netManager.StartClient();
        }
    }
}
