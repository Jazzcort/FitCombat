using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    public static int choice;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void server() {
        choice = 0;
        goToGameScence();
    }

    public static void host() {
        choice = 1;
        
        goToGameScence();
    }

    public static void client() {
        choice = 2;
        goToGameScence();
    }

    public static void goToGameScence() {
        SceneManager.LoadScene(1);
    }
}
