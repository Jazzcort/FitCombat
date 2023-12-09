using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;
    public enum State { Waiting, Started, Win, Lose }
    private int connectedPlayers;

    public static Action<State> onGameStateChanged;

    [SerializeField] private State gameState;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        NetworkManager.OnServerStarted += NetworkManager_OnServerStarted;
    }

    private void NetworkManager_OnServerStarted()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += Singleton_OnClientConnectedCallback;
        }
    }

    private void Singleton_OnClientConnectedCallback(ulong obj)
    {
        connectedPlayers++;

        if (connectedPlayers >= 2)
        {
            StartGame();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameState = State.Waiting;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        NetworkManager.OnServerStarted -= NetworkManager_OnServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback -= Singleton_OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += DisconnectedCallback;
    }

    private void StartGame()
    {
        gameState = State.Started;
        StartGameClientRpc();
        NetworkManager.Singleton.OnClientDisconnectCallback += DisconnectedCallback;
    }

    private void DisconnectedCallback(ulong obj)
    {
        NetworkManager.Singleton.Shutdown();
        // At this point we must use the UnityEngine's SceneManager to switch back to the MainMenu
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);

    }

    [ClientRpc]
    private void StartGameClientRpc()
    {
        gameState = State.Started;
        onGameStateChanged?.Invoke(gameState);

    }


    public void SetGameState(State state)
    {
        this.gameState = state;
        onGameStateChanged?.Invoke(this.gameState);
    }


}
