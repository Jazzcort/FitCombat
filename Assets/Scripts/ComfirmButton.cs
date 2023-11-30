using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComfirmButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI inputText;
    private Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(async () =>
        {
            
            var tt = await RelayManager.instance.StartClientWithRelay(inputText.text.Substring(0, 6));
            SceneChanger.client();
            Debug.Log(inputText.text + "entered");
            Debug.Log(tt);
        });


    }

    // Update is called once per frame
    void Update()
    {

    }
}
