using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    [Header("Walls")]
    public GameObject[] walls;

    [Header("Enemies")]
    public GameObject[] enemyTypes;
    public Transform[] enemySpawners;

    [HideInInspector] public List<GameObject> enemies;

    private TriviaVariants trivias;
    private bool spawned;

    private int currentWave = 0;
    private int waves = 1;
    private int XPToEnd = 0;

    public GameObject spawnEffect;

    private Player player;

    public GameObject portal;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        trivias = GameObject.FindGameObjectWithTag("Trivias").GetComponent<TriviaVariants>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!spawned && other.CompareTag("Player"))
        {
            SpawnEnemies();
            StartCoroutine(CheckEnemies());
            spawned = true;
        }
    }

    IEnumerator CheckEnemies()
    {
        yield return new WaitForSeconds(2f);
        yield return new WaitUntil(() => enemies.Count == 0);
        yield return new WaitForSeconds(1f);
        if (currentWave == waves - 1)
        {
            DestroyWalls();
            if (trivias.spawnedTrivias.Count < 5)
            {
                SpawnTrivia();
            }
            if (!trivias.isPortalSpawned)
            {
                CheckXP();
                trivias.isPortalSpawned = true;
            }
        }
        else
        {
            currentWave++;
            SpawnEnemies();
            StartCoroutine(CheckEnemies());
        }
    }

    public void SpawnEnemies()
    {
        foreach (Transform spawner in enemySpawners)
        {
            GameObject enemyType = enemyTypes[Random.Range(0, enemyTypes.Length)];
            GameObject enemy = Instantiate(enemyType, spawner.position, Quaternion.identity) as GameObject;
            Instantiate(spawnEffect, enemy.transform.position, Quaternion.identity);
            enemy.transform.parent = transform;
            enemies.Add(enemy);
        }
    }

    public void DestroyWalls()
    {
        foreach (GameObject wall in walls)
        {
            if (wall != null && wall.transform.childCount != 0)
            {
                Destroy(wall);
            }
        }
    }
    
    public void SpawnTrivia()
    {
        var rand = Random.Range(0, 2);
        if (trivias.spawnedTrivias.Count == 0 || rand == 1)
        {
            var trivia = trivias.trivias[Random.Range(0, trivias.trivias.Count)];
            trivias.spawnedTrivias.Add(trivia.GetComponent<TriviaDialogue>());
            trivias.trivias.Remove(trivia);
            Instantiate(trivia, enemySpawners[0].transform.position,  Quaternion.identity);
        }
    }

    public void CheckXP()
    {
        if (player.currentXP >= XPToEnd)
        {
            var firstRoom = FindFirstObjectByType<RoomVariants>().GetComponent<RoomVariants>().rooms[0];
            Instantiate(portal, firstRoom.transform.position, Quaternion.identity);
        }
    }
}
