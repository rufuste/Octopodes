using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    private EnemyStats enemyStats;
    private PlayerStats playerStats;
    private float bulletDamage;
    private GameObject player;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right * speed * 10);
        
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();
        bulletDamage = playerStats.Damage;
    }
    
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            enemyStats = collision.collider.GetComponent<EnemyStats>();
            enemyStats.TakeDamage(bulletDamage);
            Destroy(gameObject);
        } 
    }

}
