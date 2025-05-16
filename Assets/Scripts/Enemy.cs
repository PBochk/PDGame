using System;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float timeBtwAttack;

    [Header("Enemy Stats")]
    public int health; // Здоровье
    public float speed; // Скорость передвижения
    private float currentSpeed = 0; 
    public int enemyDamage; // Урон ближней атаки
    public float meleeAttackCooldown; // Время восстановления одной ближней атаки в секундах
    public float stunTime; // Время оглушения врага после получения им урона P.S. Сделал, потому что смог. Опциональный параметр.
    private float currentStunTime = 0;


    private Player player;
    private Animator anim;
    private AddRoom room;

    private HealthDisplay healthDisplay;
    private void Start()
    {
        anim = GetComponent<Animator>();
        player = FindFirstObjectByType<Player>();
        healthDisplay = FindFirstObjectByType<HealthDisplay>();
        room = GetComponentInParent<AddRoom>();
    }

    private void Update()
    {
        if (currentStunTime <= 0)
        {
            currentSpeed = speed;
        }
        else
        {
            currentSpeed = 0;
            currentStunTime -= Time.deltaTime;
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

        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, currentSpeed * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        currentStunTime = stunTime;
        health -= damage;
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (timeBtwAttack <= 0)
            {
                anim.SetTrigger("enemyAttack");
            }
            timeBtwAttack -= Time.deltaTime;
        }
    }

    public void OnEnemyAttack()
    {
        player.health -= enemyDamage;
        healthDisplay.DamageTaken(enemyDamage);
        timeBtwAttack = meleeAttackCooldown;
    }
}