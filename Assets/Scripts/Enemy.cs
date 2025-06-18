using System;
using TMPro;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Enemy : MonoBehaviour
{
    public GameObject takeDamageEffect;
    [SerializeField] private AudioClip enemyHitSound;
    [SerializeField] private AudioClip enemyTakeDamageSound;
    private AudioSource audioSource;
    private Rigidbody2D rb;

    private float timeBtwAttack;

    [Header("Enemy Stats")]
    public int health; // Здоровье
    public float speed; // Скорость передвижения
    private float currentSpeed = 0; 
    public int enemyDamage; // Урон ближней атаки
    public float meleeAttackCooldown; // Время восстановления одной ближней атаки в секундах
    public float stunTime; // Время оглушения врага после получения им урона P.S. Сделал, потому что смог. Опциональный параметр.
    private float currentStunTime = 0;
    public bool isRanged;

    private Player player;
    private Animator anim;
    private AddRoom room;
    private PlayerSkills pSk;
    public float knockback = 3f;

    public int xp;
    private void Start()
    {
        audioSource = FindFirstObjectByType<AudioSource>();
        anim = GetComponent<Animator>();
        player = FindFirstObjectByType<Player>();
        room = GetComponentInParent<AddRoom>();
        rb = GetComponent<Rigidbody2D>();
        pSk = player.GetComponent<PlayerSkills>();
    }

    private void Update()
    {
        if (health <= 0)
        {
            player.GainXP(xp);
            Destroy(gameObject);
            if (room != null) //для тестовой комнаты
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
    }

    private void FixedUpdate()
    {
        MoveEnemy();
    }

    public void MoveEnemy()
    {
        if (!pSk.isOverloadOn)
        {
            Vector2 direction;
            Vector2 targetPosition;
            if (currentStunTime <= 0)
            {
                currentSpeed = speed;
                if (isRanged)
                {
                    direction = (transform.position - player.transform.position).normalized;
                }
                else
                {
                    direction = (player.transform.position - transform.position).normalized;
                }
                targetPosition = (Vector2)transform.position + direction * currentSpeed * Time.deltaTime;
            }
            else
            {
                currentSpeed = 0;
                direction = (transform.position - player.transform.position).normalized;
                targetPosition = (Vector2)transform.position + direction * knockback * Time.deltaTime;
                currentStunTime -= Time.fixedDeltaTime;
            }

            rb.MovePosition(targetPosition);
        }
    }

    public void TakeDamage(int damage)
    {
        Instantiate(takeDamageEffect, transform.position, Quaternion.identity);
        audioSource.PlayOneShot(enemyTakeDamageSound);
        currentStunTime = stunTime;
        health -= damage;
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (timeBtwAttack <= 0)
            {
                timeBtwAttack = meleeAttackCooldown;
                anim.SetTrigger("enemyAttack");
            }
        }
        if (timeBtwAttack > 0)
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }

    public void OnEnemyAttack()
    {
        audioSource.PlayOneShot(enemyHitSound);
        if (!pSk.isRedirectOn)
        {
            player.ChangeHealth(-enemyDamage);
            player.Knockback(this);
        }
        else
        {
            TakeDamage(enemyDamage);
        }
    }
}