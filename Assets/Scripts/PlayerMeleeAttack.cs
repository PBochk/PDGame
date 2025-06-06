using System;
using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{

    public Transform attackPos; // Центр коллайдера атаки
    public LayerMask enemy; // Маска, определяющая врагов
    public Animator anim;

    [Header("Player Melee Attack Stats")]
    public float attackCooldown; // Время восстановления одной ближней атаки в секундах
    public float attackRange; // Радиус круга - коллайдера атаки
    public int damage; // Урон ближней атаки 

    public float timeBtwAttack;

    private void Update()
    {
        if (timeBtwAttack <= 0)
        {
            if (Input.GetMouseButton(0))
            {
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
            enemies[i].GetComponent<Enemy>().TakeDamage(damage);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
