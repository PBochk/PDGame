using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.InputSystem.Android.LowLevel.AndroidGameControllerState;

public class AddRoom : MonoBehaviour
{
    [Header("Walls")]
    public GameObject[] walls;

    [Header("Enemies")]
    public GameObject[] enemyTypes;
    public Transform[] enemySpawners;

    [HideInInspector] public List<GameObject> enemies;

    private GameObject[] trivias;
    private bool spawned;

    private int currentWave = 0;
    private int waves = 3;


    public GameObject spawnEffect;

    private void Start()
    {
        trivias = GameObject.FindGameObjectWithTag("Trivias").GetComponent<TriviaVariants>().trivias;
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
            SpawnReward();
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
    
    public void SpawnReward()
    {
        Instantiate(trivias[0], enemySpawners[0].transform.position,  Quaternion.identity);
    }
}
