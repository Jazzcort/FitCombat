using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerSelect : MonoBehaviour
{
    [Header(" Panels ")]
    [SerializeField] private GameObject selectPanel;
    [SerializeField] private GameObject waitingPanel;
    public enum State { Select, Waiting }
    // Start is called before the first frame update
    void Start()
    {
        showSelectPanel();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void selecting(UIManagerSelect.State choice)
    {
        if (choice == State.Select)
        {
            showSelectPanel();
        }
        else if (choice == State.Waiting)
        {
            showWaitingPAnel();
        }
    }

    private void showSelectPanel()
    {
        selectPanel.SetActive(true);
        waitingPanel.SetActive(false);

    }

    private void showWaitingPAnel()
    {
        selectPanel.SetActive(false);
        waitingPanel.SetActive(true);
    }
}
