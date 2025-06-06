using UnityEngine;

public class Bullet : MonoBehaviour
{
    public LayerMask whatIsSolid;

    [Header("Range Weapon Stats")]
    public float speed;
    public float lifetime = 1f;
    public float distance;
    public int damage;

    //public BulletType bulletType;
    public Enemy enemyShooter;

    //public enum BulletType
    //{
    //    None,
    //    PlayerBullet,
    //    EnemyBullet
    //}
    private void Start()
    {
        Invoke("DestroyBullet", lifetime);
    }

    private void Update()
    {
        
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, distance, whatIsSolid);

        if (hitInfo.collider != null)
        {
            switch (hitInfo.collider.tag)
            {
                case "Enemy":
                {
                    hitInfo.collider.GetComponent<Enemy>().TakeDamage(damage);
                    break;
                }
                case "Player":
                {
                    var player = hitInfo.collider.GetComponent<Player>();
                    var isRedirected = player.GetComponent<PlayerSkills>().isRedirectOn;
                    if (!isRedirected)
                    {
                        player.ChangeHealth(-damage);
                    }
                    else if (enemyShooter != null)
                    {
                        enemyShooter.TakeDamage(damage);
                    }
                    break;
                }
            }           
            Destroy(gameObject);
        }
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
