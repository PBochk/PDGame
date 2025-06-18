using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    [SerializeField] private AudioClip sound;
    private AudioSource audioSource;
    public Transform attackPos; // ����� ���������� �����
    public LayerMask enemy; // �����, ������������ ������
    public Animator anim;

    [Header("Player Melee Attack Stats")]
    public float attackCooldown; // ����� �������������� ����� ������� ����� � ��������
    public float attackRange; // ������ ����� - ���������� �����
    public int damage; // ���� ������� ����� 

    private float timeBtwAttack;
    private Player player;
    private List<Collider2D> damagedEnemies;
    private void Start()
    {
        audioSource = FindFirstObjectByType<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        damagedEnemies = new List<Collider2D>();
    }
    private void Update()
    {
        if (timeBtwAttack <= 0)
        {
            if (Input.GetMouseButton(1) && !player.isRestrained)
            {
                audioSource.PlayOneShot(sound);
                damagedEnemies.Clear();
                anim.SetTrigger("attack");
                timeBtwAttack = attackCooldown;
            }
        }
        if (timeBtwAttack > 0)
        { 
            timeBtwAttack -= Time.deltaTime;
        } 
    }

    public void OnAttack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemy);
        for (int i = 0; i < enemies.Length; i++)
        {
            var enemyCollider = enemies[i];
            if (!damagedEnemies.Contains(enemyCollider))
            {
                damagedEnemies.Add(enemyCollider);
                enemyCollider.GetComponent<Enemy>().TakeDamage(damage);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
