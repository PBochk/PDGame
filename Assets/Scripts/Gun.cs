using Unity.VisualScripting;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private float offsetX;
    public GameObject bulletObject;
    public Transform shotPoint;

    private float timeBtwShots;
    public float shotCooldown;

    private Player player;

    private Vector3 difference;
    private float rotZ;

    private bool isPlayerGun;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        isPlayerGun = CompareTag("PlayerGun");
        RangeAttack rangeAttack = GetComponentInParent<RangeAttack>();
        Bullet bullet = bulletObject.GetComponent<Bullet>();

        shotCooldown = rangeAttack.shotCooldown;
        bullet.damage = rangeAttack.damage;
        bullet.speed = rangeAttack.bulletSpeed;
        bullet.lifetime = rangeAttack.lifetime;

        if (!CompareTag("PlayerGun"))
        {
            bullet.enemyShooter = GetComponentInParent<Enemy>();
        }
    }

    void Update()
    {
        if (CompareTag("PlayerGun"))
        {
            difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        }
        else
        {
            difference = player.transform.position - transform.position;
            rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        }
        if (player.facingRight)
        {
            offsetX = 0f;
        }
        else
        {
            offsetX = 180f;
            rotZ *= -1;
        }
        transform.rotation = Quaternion.Euler(offsetX, 0f, rotZ);

        if (timeBtwShots <= 0)
        {
            if (Input.GetMouseButton(1) || CompareTag("EnemyGun"))
            {
                Instantiate(bulletObject, shotPoint.position, transform.rotation);
                timeBtwShots = shotCooldown;
            }
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }
}
