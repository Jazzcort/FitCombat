using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private float attackDamage = 100f;
    public Vector2 knockback = Vector2.zero;
    private PlayerController character;

    private void Awake()
    {
        character = GetComponentInParent<PlayerController>();
        attackDamage = GetComponentInParent<Damageable>().Atk;

    }

    private void OnEnable()
    {
        attackDamage = GetComponentInParent<Damageable>().Atk;
    }

    void Update()
    {
        attackDamage = GetComponentInParent<Damageable>().Atk;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();

        Vector2 n_knockback = knockback * (character.IsFacingRight ? new Vector2(1, 0) : new Vector2(-1, 0));

        if (damageable != null)
        {
            bool gotHit = damageable.Hit(attackDamage, n_knockback);
            if (gotHit)
            {
                Debug.Log(collision.name + " hit for " + attackDamage);
            }

        }
    }
}
