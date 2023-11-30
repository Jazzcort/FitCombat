using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CancelButton : MonoBehaviour
{
    [SerializeField] private UIManagerSelect manager;
    private Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => {
            manager.selecting(UIManagerSelect.State.Select);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
