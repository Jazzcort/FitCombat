using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.TextCore.Text;

public class UIManager : NetworkBehaviour
{
    [SerializeField] private GameObject waitingPanel;
    public GameObject damageTextPrefab;
    public GameObject winText;
    public GameObject loseText;
    public GameObject roomCode;
    // public GameObject healthTextPrefab;
    public Canvas gameCanvas;

    private float gameFinishedTime = -1f;
    private float closeTime = 3f;

    private void Awake()
    {
        gameCanvas = FindObjectOfType<Canvas>();
        CharacterStatus.onGeneratedRoomcode += ShowRoomCode;

    }
    private void OnEnable()
    {
        CharacterEvent.characterDamaged += CharacterTookDamage;
        GameManager.onGameStateChanged += OnGameChangedCallback;
    }

    private void OnDisable()
    {
        CharacterEvent.characterDamaged -= CharacterTookDamage;
        GameManager.onGameStateChanged -= OnGameChangedCallback;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        CharacterStatus.onGeneratedRoomcode -= ShowRoomCode;
    }

    private void OnGameChangedCallback(GameManager.State gameState)
    {
        switch (gameState)
        {
            case GameManager.State.Waiting:
                waitingPanel.SetActive(true);
                break;
            case GameManager.State.Started:
                waitingPanel.SetActive(false);
                break;
            case GameManager.State.Win:

                WinningMessage();
                gameFinishedTime = 0;

                break;
            case GameManager.State.Lose:

                LoseMessage();
                gameFinishedTime = 0;

                break;

        }
    }

    private void ShowRoomCode(string jCode)
    {
        RectTransform textTransform = Instantiate(roomCode, new Vector3(Screen.width / 2, (Screen.height / 4) * 3, 0), Quaternion.identity).GetComponent<RectTransform>();
        TMP_Text rc = textTransform.GetComponent<TMP_Text>();
        rc.text = "Room Code: "+ jCode;

        textTransform.SetParent(waitingPanel.transform);
    }

    public void WinningMessage()
    {
        RectTransform textTransform = Instantiate(winText, new Vector3(Screen.width / 2, (Screen.height / 3) * 2, 0), Quaternion.identity).GetComponent<RectTransform>();

        textTransform.SetParent(gameCanvas.transform);
    }

    public void LoseMessage()
    {
        RectTransform textTransform = Instantiate(loseText, new Vector3(Screen.width / 2, (Screen.height / 3) * 2, 0), Quaternion.identity).GetComponent<RectTransform>();

        textTransform.SetParent(gameCanvas.transform);
    }

    public void CharacterTookDamage(GameObject character, float damageReceived)
    {
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);
        spawnPosition.y += 20;

        TMP_Text tmpText = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();

        tmpText.text = "-" + damageReceived.ToString();
    }

    void Update()
    {
        if (gameFinishedTime != -1)
        {
            gameFinishedTime += Time.deltaTime;
            if (gameFinishedTime >= closeTime)
            {
#if UNITY_ANDROID
                AndroidJavaClass player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject currentActivity = player.GetStatic<AndroidJavaObject>("currentActivity");
                currentActivity.Call("finish");
#endif
            }
        }
    }

}
