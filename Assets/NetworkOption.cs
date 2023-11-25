using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkOption : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (SceneChanger.choice == 0) {
            NetworkManager.Singleton.StartServer();
        } else if (SceneChanger.choice == 1) {
            NetworkManager.Singleton.StartHost();
        } else if (SceneChanger.choice == 2) {
            NetworkManager.Singleton.StartClient();
        }
        Debug.Log(SceneChanger.choice);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
