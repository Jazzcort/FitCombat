using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    private string userKey = null;
    public static CharacterStatus instance;
    public string atk = null;
    public string def = null;
    public string hp = null;
    public static Action<string> onGeneratedRoomcode;

    private void Awake()
    {
#if (!UNITY_EDITOR && UNITY_ANDROID)
        CreatePushClass(new AndroidJavaClass("com.unity3d.player.UnityPlayer"));
#endif

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }


    }
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            Debug.Log(userKey + "userKey");
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        // userKey = "jbKZfj4SN1NB7zI9Eid7eHOwvG73";
        if (userKey != null)
        {
            // try
            // {
            //     DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            //     await reference.Child("Game Data").Child(userKey)
            //     .GetValueAsync().ContinueWith(task =>
            //     {
            //         if (task.IsCompleted)
            //         {
            //             DataSnapshot snp = task.Result;
            //             atk = snp.Child("atk").Value.ToString();
            //             def = snp.Child("def").Value.ToString();
            //             hp = snp.Child("hp").Value.ToString();
            //         }
            //     });
            // } catch (Exception e) {
            //     Debug.Log(e.Message);
            // }



            if (atk != null)
            {
                Debug.Log("atk: " + atk);
            }

            if (def != null)
            {
                Debug.Log("def: " + def);
            }

            if (hp != null)
            {
                Debug.Log("hp: " + hp);
            }
        }
        else
        {
            atk = "200";
            def = "150";
            hp = "300";

            if (atk != null)
            {
                Debug.Log("atk: " + atk);
            }

            if (def != null)
            {
                Debug.Log("def: " + def);
            }

            if (hp != null)
            {
                Debug.Log("hp: " + hp);
            }
        }


    }

    public void CreatePushClass(AndroidJavaClass UnityPlayer)
    {
#if UNITY_ANDROID
        AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject>("getIntent");
        bool key_hasExtra = IsBool(intent, "userKey");
        bool atk_hasExtra = IsBool(intent, "atk");
        bool def_hasExtra = IsBool(intent, "def");
        bool hp_hasExtra = IsBool(intent, "hp");

        AndroidJavaObject extras = GetExtras(intent);

        if (extras != null)
        {
            if (key_hasExtra)
                userKey = GetProperty(extras, "userKey");
            if (atk_hasExtra)
                atk = GetProperty(extras, "atk");
            if (def_hasExtra)
                def = GetProperty(extras, "def");
            if (hp_hasExtra)
                hp = GetProperty(extras, "hp");

        }
#endif
    }

    private bool IsBool(AndroidJavaObject intent, string method)
    {
        bool b = false;

        try
        {
            b = intent.Call<bool>("hasExtra", method);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        return b;
    }

    private AndroidJavaObject GetExtras(AndroidJavaObject intent)
    {
        AndroidJavaObject extras = null;

        try
        {
            extras = intent.Call<AndroidJavaObject>("getExtras");
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        return extras;
    }

    private string GetProperty(AndroidJavaObject extras, string name)
    {
        string s = string.Empty;

        try
        {
            s = extras.Call<string>("getString", name);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        return s;
    }
}
