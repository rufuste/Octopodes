using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyAttack : MonoBehaviour
{
    private float attackRange;
    private float attackDelay;
    private GameObject[] players;
    private GameObject player;
    private PlayerStats playerStats;
    private float damage;

    private EnemyStats stats;
    float timer;
    
    void Start()
    {
        // Creates reference to the EnemyStats script attached to this instance of an enemy
        stats = gameObject.GetComponent<EnemyStats>();
        damage = stats.Damage;
        attackDelay = stats.AttackDelay;
        attackRange = stats.AttackRange;

        players = GameObject.FindGameObjectsWithTag ("Player");
        player = players[Random.Range(0, players.Length)];
        playerStats = player.GetComponent<PlayerStats>();
    }

    // Every frame
    void Update()
    {
        // Increase timer by the time taken to complete the last frame
        timer += Time.deltaTime;

        // If enemy has health, enough time has elapsed since the last attack and is close enough to the player
        if (stats.Health > 0 && timer > attackDelay && Vector2.Distance(transform.position, player.transform.position) <= attackRange)
        {
            Attack();
        }
    }

    void Attack()
    {
        // Reset time since last attack
        timer = 0f;
        // If player has health to lose
        if (playerStats.Health > 0)
        {
            // Player takes damage equal to this enemy's damage
            playerStats.TakeDamage(damage);
        }
    }
}
