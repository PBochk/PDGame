using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    [Header("Walls")]
    public GameObject[] walls;

    [Header("Enemies")]
    public GameObject[] enemyTypes;
    public Transform[] enemySpawners;

    [HideInInspector] public List<GameObject> enemies;

    //private RoomVariants variants;
    private bool spawned;
    //private bool wallsDestroyed;

    private void Start()
    {
        //variants = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomVariants>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!spawned && other.CompareTag("Player"))
        {
            foreach (Transform spawner in enemySpawners)
            {
                GameObject enemyType = enemyTypes[Random.Range(0, enemyTypes.Length)];
                GameObject enemy = Instantiate(enemyType, spawner.position, Quaternion.identity) as GameObject;
                enemy.transform.parent = transform;
                enemies.Add(enemy);
            }
            spawned = true;
            StartCoroutine(CheckEnemies());
        }
    }

    IEnumerator CheckEnemies()
    {
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => enemies.Count == 0);
        DestroyWalls();
    }


    public void DestroyWalls()
    {
        foreach(GameObject wall in walls)
        {
            if (wall != null && wall.transform.childCount != 0)
            {
                Destroy(wall);
            }
        }
        //wallsDestroyed = true;
    }

    //private void OnTriggerStay2D(Collider2D other)
    //{
    //    if(wallsDestroyed && other.CompareTag("Wall"))
    //    {
    //        //Destroy(other.gameObject);
    //    }
    //}
    
}
