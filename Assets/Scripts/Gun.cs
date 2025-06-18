using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class Gun : MonoBehaviour
{
    [SerializeField] private AudioClip PlayerShotSound;
    [SerializeField] private AudioClip EnemyShotSound;
    private AudioSource audioSource;
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
        audioSource = FindFirstObjectByType<AudioSource>();
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
        if (CompareTag("PlayerGun") && !player.isRestrained)
        {
            difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        }
        else if (!CompareTag("PlayerGun"))
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
        if (!CompareTag("PlayerGun") || !player.isRestrained)
        {
            transform.rotation = Quaternion.Euler(offsetX, 0f, rotZ);
        }

        if (timeBtwShots <= 0)
        {
            if (Input.GetMouseButton(0) && !player.isRestrained || CompareTag("EnemyGun"))
            {
                Instantiate(bulletObject, shotPoint.position, transform.rotation);
                timeBtwShots = shotCooldown;
            }
            if (Input.GetMouseButton(0) && !player.isRestrained)
            {
                audioSource.PlayOneShot(PlayerShotSound);
            }
            if (CompareTag("EnemyGun"))
            {
                audioSource.PlayOneShot(EnemyShotSound);
            }

        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }
}

//[SerializeField] private AudioClip sound;
//private AudioSource audioSource;

//audioSource = FindFirstObjectByType<AudioSource>();

//audioSource.PlayOneShot(sound);
