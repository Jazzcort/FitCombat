using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Unity.Netcode;

public class Damageable : NetworkBehaviour
{
    private Animator animator;
    public UnityEvent<float, Vector2> damageableHit;
    [SerializeField] private float _health = 100f;
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float def = 150f;
    [SerializeField] private float atk = 200f;
    public float Atk
    {
        get
        {
            return atk;
        }
    }
    // private UIManager UImanager;

    private float timeSinceHit = 0;
    [SerializeField] private float invincibilityTime = 0.25f;
    public float MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        private set
        {
            _maxHealth = value;
        }
    }



    public float Health
    {
        get
        {
            return _health;
        }
        private set
        {
            _health = value;

            if (_health <= 0)
            {
                IsAlive = false;
            }
        }
    }


    [SerializeField] private bool _isAlive = true;
    [SerializeField] private bool isInvinvible = false;
    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        private set
        {
            _isAlive = value;
            if (IsOwner)
            {
                animator.SetBool(AnimationStrings.isAlive, value);
                GameManager.instance.SetGameState(GameManager.State.Lose);

            }
            else
            {
                GameManager.instance.SetGameState(GameManager.State.Win);
            }

            Debug.Log("IsAlive value: " + IsAlive);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        GameManager.onGameStateChanged += OnGameStatusChangedSyncStatus;


        // UImanager = FindAnyObjectByType<UIManager>();
    }

    public override void OnDestroy() {
        base.OnDestroy();
         GameManager.onGameStateChanged -= OnGameStatusChangedSyncStatus;

    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();


    }


    [ServerRpc]
    private void SyncStatusServerRpc(float atk, float def, float hp)
    {

        this.def = def;
        _health = hp;
        this.atk = atk;

        SyncStatusClientRpc(atk, def, hp);

    }

    [ClientRpc]
    private void SyncStatusClientRpc(float atk, float def, float hp)
    {
        this.def = def;
        _health = hp;
        this.atk = atk;
    }

    public bool Hit(float damage, Vector2 knockback)
    {
        if (IsAlive && !isInvinvible)
        {
            float realDamage = (damage > def ? damage - def : 10);
            Health -= realDamage;

            isInvinvible = true;

            if (IsOwner)
            {
                animator.SetTrigger(AnimationStrings.hit);
            }


            damageableHit?.Invoke(realDamage, knockback);
            CharacterEvent.characterDamaged?.Invoke(gameObject, realDamage);

            return true;
        }

        return false;
    }

    void Update()
    {
        if (isInvinvible)
        {
            if (timeSinceHit > invincibilityTime)
            {
                isInvinvible = false;
                timeSinceHit = 0;
            }

            timeSinceHit += Time.deltaTime;
        }

    }

    private void OnGameStatusChangedSyncStatus(GameManager.State gameState)
    {
        if (IsOwner && gameState == GameManager.State.Started)
        {
            def = float.Parse(CharacterStatus.instance.def);
            _health = float.Parse(CharacterStatus.instance.hp);
            atk = float.Parse(CharacterStatus.instance.atk);
            SyncStatusServerRpc(atk, def, _health);

        }
    }

}
