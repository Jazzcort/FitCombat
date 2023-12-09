using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class ExitManager : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {

#if UNITY_ANDROID
                AndroidJavaClass player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject currentActivity = player.GetStatic<AndroidJavaObject>("currentActivity");
                currentActivity.Call("finish");
#endif
            }
            // else
            // {
            //     NetworkManager.Singleton.Shutdown();
            //     // At this point we must use the UnityEngine's SceneManager to switch back to the MainMenu
            //     SceneManager.LoadScene(0);
            // }
        }
    }
}
