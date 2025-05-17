using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemySpawn : MonoBehaviour
{
    public List<GameObject> enemies;
    private List<EnemyStats> enemyStats = new List<EnemyStats>();

    private GameObject player;
    private PlayerStats playerStats;

   

    public Vector2 spawnPoint;
    public float zoom;
    float screenEdgeRight_WorldPos;
    float screenEdgeLeft_WorldPos;
    float screenEdgeTop_WorldPos;
    float screenEdgeBottom_WorldPos;

    int roundNumber = 0;
    int enemiesSpawned = 0;
    float roundSpawnCount = 0f;
    float timeSinceSearch = 0f;
    public Text waveText;

    public int timeBetweenRounds = 0;

    public void enemyDeleted()
    {
        enemiesSpawned--;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();

        for (int i = 0; i < enemies.Count; ++i)
        {
            enemyStats.Add(enemies[i].GetComponent<EnemyStats>());
        }
    }

    private void NewRound()
    {
        Wait(timeBetweenRounds);
        roundNumber++;
        enemiesSpawned = 0;
        // WAIT timeBetweenRounds
        roundSpawnCount = Mathf.Max(roundNumber * (roundNumber/3), roundNumber);
        Debug.Log("Spawn Count: " + roundSpawnCount.ToString());
        
    }
    
    void Update()
    {
        // If spawn count not met
        if (enemiesSpawned < roundSpawnCount)
        {
            CheckEnemySpawn();
        }
        // If spawncount met and enemies all dead
        else if (enemiesSpawned >= roundSpawnCount && !CheckEnemiesAlive())
        {
            waveText.text = "Wave: " + roundNumber.ToString();

            NewRound();
        }
    }

    bool CheckEnemiesAlive()
    {
        timeSinceSearch += Time.deltaTime;
        if (timeSinceSearch >= 2f && playerStats.Health > 0)
        {
            timeSinceSearch = 0f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;
    }

    void CheckEnemySpawn()
    {
        // For each enemy type
        for (int i = 0; i < enemies.Count; ++i)
        {
            // Add length of last frame to time since spawn of this enemy type
            enemyStats[i].timeSinceSpawn += Time.deltaTime;

            // Check time since this enemy type was spawned
            if (enemyStats[i].SpawnRate < enemyStats[i].TimeSinceSpawn)
            {
                // New enemy of this type
                NewEnemy(enemyStats[i].gameObject);
                // Reset this enemy's timeSinceSpawn
                enemyStats[i].timeSinceSpawn = 0f;
                enemiesSpawned++;
            }
        }
    }

    private IEnumerator Wait(int seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    // Subroutine creates enemy, takes argument of enemy type
    void NewEnemy(GameObject enemy)
    {
        // Sets screen edge sides
        screenEdgeRight_WorldPos = Camera.main.ViewportToWorldPoint(new Vector3(zoom, 0, 0.0f)).x;
        screenEdgeLeft_WorldPos = Camera.main.ViewportToWorldPoint(new Vector3(-zoom, 0, 0.0f)).x;
        screenEdgeTop_WorldPos = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, zoom, 0.0f)).y;
        screenEdgeBottom_WorldPos = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, -zoom, 0.0f)).y;

        // Randomly selects a side
        int side = Random.Range(0,4);
        float xPosition;
        float yPosition;

        // Sets x and y based on side generated
        switch (side)
        {
            // Spawn top
            case 0:
                // Random x
                xPosition = Random.Range(screenEdgeLeft_WorldPos, screenEdgeRight_WorldPos);
                // On the top of the screen
                spawnPoint = new Vector2(xPosition, screenEdgeTop_WorldPos);
                break;
            // Spawn bottom
            case 1:
                // Random x
                xPosition = Random.Range(screenEdgeLeft_WorldPos, screenEdgeRight_WorldPos);
                // On the bottom of the screen
                spawnPoint = new Vector2(xPosition, screenEdgeBottom_WorldPos);
                break;

            // Spawn Left
            case 2:
                // Random y
                yPosition = Random.Range(screenEdgeTop_WorldPos, screenEdgeBottom_WorldPos);
                // On the left of the screen
                spawnPoint = new Vector2(screenEdgeLeft_WorldPos, yPosition);
                break;

            // Spawn Right
            case 3:
                // Random y
                yPosition = Random.Range(screenEdgeTop_WorldPos, screenEdgeBottom_WorldPos);
                // On the right side of the screen
                spawnPoint = new Vector2(screenEdgeRight_WorldPos, yPosition);
                break;

            default:
                break;
        }
        // Instantiates an enemy at the position generated
        Instantiate(enemy, spawnPoint, transform.rotation);
    }
}
