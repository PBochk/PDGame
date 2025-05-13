using System;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public int health;
    public float speed;
    public int damage;
    private float stopTime = 0;
    public float startStopTime;
    public float normalSpeed;
    private Player player;
    private Animator anim;

    private HealthDisplay healthDisplay;
    private void Start()
    {
        anim = GetComponent<Animator>();
        player = FindFirstObjectByType<Player>();
        healthDisplay = FindFirstObjectByType<HealthDisplay>();
        normalSpeed = speed;

    }
    private void Update()
    {
        //if (stopTime <= 0)
        //{
        //    speed = normalSpeed;
        //}
        //else
        //{
        //    speed = 0;
        //    stopTime -= Time.deltaTime;
        //}
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        transform.Translate(Vector2.right * speed * Time.deltaTime);
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
        player.health -= damage;
        healthDisplay.DamageTaken(damage);
        timeBtwAttack = startTimeBtwAttack;
        Debug.Log(player.health);
    }
}