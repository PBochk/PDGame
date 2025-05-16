using UnityEngine;

public class Bullet : MonoBehaviour
{
    public LayerMask whatIsSolid;

    private PlayerRangeAttack rangeAttack;

    [Header("Range Weapon Stats")]
    private float speed;
    private float lifetime = 1; //Пока не добавлено
    private float distance;
    private int damage;

    private void Start()
    {
        rangeAttack = FindFirstObjectByType<PlayerRangeAttack>();
        speed = rangeAttack.bulletSpeed;
        damage = rangeAttack.damage;
    }


    private void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, distance, whatIsSolid);
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Enemy"))
            {
                hitInfo.collider.GetComponent<Enemy>().TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
}
