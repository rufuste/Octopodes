using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private float speed;
    private float maxDistance;


    private GameObject[] targets;
    private Transform target;
    private Rigidbody2D rb;
    private EnemyStats enemyStats;
    private GameObject gameManager;
    private EnemySpawn enemySpawn;
    
    // Start is called before the first frame update
    void Start()
    {
        enemyStats = gameObject.GetComponent<EnemyStats>();

        gameManager = GameObject.FindGameObjectWithTag("GameController");
        enemySpawn = gameManager.GetComponent<EnemySpawn>();

        speed = enemyStats.MoveSpeed;
        maxDistance = enemyStats.MaxDistanceFromPlayer;

        targets = GameObject.FindGameObjectsWithTag("Player");  
        target = targets[Random.Range(0, targets.Length)].transform;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // If distance from player exceeds max distance, reset spawn timer and destroy this instance
        if (Vector2.Distance(gameObject.transform.position, target.transform.position) > maxDistance)
        {
            enemySpawn.enemyDeleted();
            enemyStats.timeSinceSpawn = enemyStats.SpawnRate;
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        // Direction delta calcualted between player and enemy
        Vector2 direction = Vector2.zero;
        direction = target.position - transform.position;
        // Velocity added in the direction, given a magnitude of zero and then sped up accordingly
        rb.linearVelocity = direction.normalized * speed;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // On collision with player, freeze
        if (collision.collider.tag == "Player")
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // On leave collision with player, unfreeze
        if (collision.collider.tag == "Player")
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }


}
