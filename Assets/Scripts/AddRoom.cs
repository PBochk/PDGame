using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    public GameObject wallDestroyedEffect;
    public GameObject chestSpawnEffect0;
    public GameObject chestSpawnEffect1;
    [SerializeField] private AudioClip wallDestroyedSound;
    [SerializeField] private AudioClip enemySpawnSound;
    private AudioSource audioSource;

    [Header("Walls")]
    public GameObject[] walls;

    [Header("Enemies")]
    public GameObject[] enemyTypes;
    public Transform[] enemySpawners;

    [HideInInspector] public List<GameObject> enemies;

    private TriviaVariants trivias;
    private bool spawned;

    private int currentWave = 0;
    private int waves = 2;
    [HideInInspector] private int XPToEnd;
    public GameObject spawnEffect;

    private Player player;

    public GameObject portal;
    private void Start()
    {
        audioSource = FindFirstObjectByType<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        trivias = GameObject.FindGameObjectWithTag("Trivias").GetComponent<TriviaVariants>();
        StartCoroutine(GetXPTOEnd());
    }

    IEnumerator GetXPTOEnd()
    {
        yield return new WaitForSeconds(3f);
        XPToEnd = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomVariants>().XPToEnd;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!spawned && other.CompareTag("Player"))
        {
            StartCoroutine(WaitForSpawnEnemies());
            spawned = true;
        }
    }

    IEnumerator WaitForSpawnEnemies()
    {
        yield return new WaitForSeconds(1f);
        SpawnEnemies();
        StartCoroutine(CheckEnemies());

    }

    IEnumerator CheckEnemies()
    {
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => enemies.Count == 0);
        yield return new WaitForSeconds(0.5f);
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
            }
        }
        else
        {
            currentWave++;
            SpawnEnemies();
            StartCoroutine(CheckEnemies());
        }
    }

    private void SpawnEnemies()
    {
        audioSource.PlayOneShot(enemySpawnSound, 0.5f);
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
        audioSource.PlayOneShot(wallDestroyedSound);
        foreach (GameObject wall in walls)
        {
            Instantiate(wallDestroyedEffect, wall.transform.position, Quaternion.identity);
            if (wall != null && wall.transform.childCount != 0)
            {
                Destroy(wall);
            }
        }
    }
    
    public void SpawnTrivia()
    {
        var rand = Random.Range(0, 2);
        if (trivias.spawnedTrivias.Count == 0 || rand == 1 && trivias.trivias.Count > 0)
        {
            var trivia = trivias.trivias[Random.Range(0, trivias.trivias.Count - 1)];
            trivias.spawnedTrivias.Add(trivia.GetComponent<TriviaDialogue>());
            trivias.trivias.Remove(trivia);
            Instantiate(trivia, enemySpawners[0].transform.position,  Quaternion.identity);
            Instantiate(chestSpawnEffect0, enemySpawners[0].transform.position, Quaternion.identity);
            Instantiate(chestSpawnEffect1, enemySpawners[0].transform.position, Quaternion.identity);
        }
    }

    public void CheckXP()
    {
        Debug.Log(player.currentXP + "есть/надо" + XPToEnd);
        if (player.currentXP >= XPToEnd)
        {
            var firstRoom = FindFirstObjectByType<RoomVariants>().GetComponent<RoomVariants>().rooms[0];
            Instantiate(portal, firstRoom.transform.position, Quaternion.identity);
            trivias.isPortalSpawned = true;
        }
    }
}
