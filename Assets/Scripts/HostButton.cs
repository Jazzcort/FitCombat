using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HostButton : MonoBehaviour
{
    [SerializeField] private UIManagerSelect manager;
    private Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(async () => {
            manager.selecting(UIManagerSelect.State.Waiting);
            SceneChanger.host();
            var jCode = await RelayManager.instance.StartHostWithRelay();
            CharacterStatus.onGeneratedRoomcode?.Invoke(jCode);
            Debug.Log(jCode);
            
        });
        
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
