using System;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public int health;
    public float speed;
    public int enemyDamage;
    private float stopTime = 0;
    public float startStopTime;
    public float normalSpeed;
    private Player player;
    private Animator anim;
    private AddRoom room;

    private HealthDisplay healthDisplay;
    private void Start()
    {
        anim = GetComponent<Animator>();
        player = FindFirstObjectByType<Player>();
        healthDisplay = FindFirstObjectByType<HealthDisplay>();
        normalSpeed = speed;
        room = GetComponentInParent<AddRoom>();

    }
    private void Update()
    {
        if (stopTime <= 0)
        {
            speed = normalSpeed;
        }
        else
        {
            speed = 0;
            stopTime -= Time.deltaTime;
        }

        if (health <= 0)
        {
            Destroy(gameObject);
            room.enemies.Remove(gameObject);
        }

        if (player.transform.position.x > transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        stopTime = startStopTime;
        health -= damage;
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (timeBtwAttack <= 0)
            {
                anim.SetTrigger("enemyAttack");
                Debug.Log("Attack");
            }
            timeBtwAttack -= Time.deltaTime;
        }
    }

    public void OnEnemyAttack()
    {
        player.health -= enemyDamage;
        healthDisplay.DamageTaken(enemyDamage);
        timeBtwAttack = startTimeBtwAttack;
        Debug.Log(player.health);
    }
}