using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;

//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private AudioClip takeDamageSound;
    private AudioSource audioSource;
    public static Player Instance { get; private set; }
    private PlayerUI playerUI;
    private PlayerSkills pSk;
    private Rigidbody2D rb;
    public GameInput GameInput { get; private set; }
    private Vector2 moveInput;
    public bool facingRight = true;

    private Animator anim;
    public bool isRestrained;

    [Header("Player Stats")]
    public float movingSpeed;
    public float dashSpeed = 10f;
    public float dashCooldown;
    private float currentDashCooldown = 1f;
    public float dashDuration;
    private float currentDashDuration = 0f;
    [HideInInspector] public float currentSpeed = 0;
    public float health;
    public float maxHealth;
    public float regen = 0;

    //Переменные для отбрасывания
    private float knockback = 10f;
    private float stunTime = 0.1f;
    private float currentStunTime = 0f;
    private Enemy knockedEnemy;

    public int currentXP;
    private int XPToLvlUp;
    public bool isLvlUp;

    private void Start()
    {
        isRestrained = true;
        audioSource = FindFirstObjectByType<AudioSource>();
        anim = GetComponent<Animator>();
        playerUI = FindFirstObjectByType<PlayerUI>();
        pSk = GetComponent<PlayerSkills>();
    }

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        GameInput = gameObject.AddComponent<GameInput>();
    }

    private void Update()
    {
        Vector2 mousePos = Mouse.current.position.value;
        Vector2 playerScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        if ((!facingRight && mousePos.x > playerScreenPos.x
            || facingRight && mousePos.x < playerScreenPos.x)
            && !isRestrained)
        {
            Flip();
        }
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (moveInput.x == 0 && moveInput.y == 0)
        {
            anim.SetBool("isRunning", false);
        }
        else if (!isRestrained)
        {
            anim.SetBool("isRunning", true);
        }

        if (health <= 0)
        {
            if (pSk.hasBackup)
            {
                health = maxHealth;
                pSk.Revive();
            }
            else
            {
                SceneManager.LoadScene("GameOver");
            }
        }

        if (health < maxHealth)
        {
            health += regen;
        }

    }

    private void FixedUpdate()
    {
        if (!isRestrained)
        {
            if (Input.GetKey(KeyCode.Space) && currentDashCooldown <= 0)
            {
                currentDashCooldown = dashCooldown;
                currentDashDuration = dashDuration;
            }
            if (currentDashDuration > 0)
            {
                currentDashDuration -= Time.fixedDeltaTime;
            }
            if (currentDashCooldown > 0)
            {
                currentDashCooldown -= Time.fixedDeltaTime;
            }

            Vector2 targetPosition;
            moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (currentStunTime <= 0 || knockedEnemy == null)
            {
                targetPosition = GameInput.Instance.GetMovementVector();
                if (currentDashDuration > 0)
                {
                    currentSpeed = dashSpeed;
                }
                else
                {
                    currentSpeed = movingSpeed;
                }

            }
            else
            {
                currentStunTime -= Time.fixedDeltaTime;
                targetPosition = (transform.position - knockedEnemy.transform.position).normalized;
                currentSpeed = knockback;
            }
            rb.MovePosition(rb.position + targetPosition * (currentSpeed * Time.fixedDeltaTime));
        }
    }

    public Vector3 GetPlayerScreenPosition()
    {
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }

    private void Flip()
    {
        facingRight = !facingRight;
        var Rotation = transform.rotation;
        Rotation.y = facingRight ? 0 : 180;
        transform.rotation = Rotation;
    }

    public void ChangeHealth(int dif)
    {
        if (dif < 0)
        {
            audioSource.PlayOneShot(takeDamageSound);
        }
        health += dif;
        playerUI.HealthChanged();
    }

    public void Knockback(Enemy enemy)
    {
        currentStunTime = stunTime;
        knockedEnemy = enemy;
    }

    public void GainXP(int gainedXP)
    {
        currentXP += gainedXP;
        playerUI.XPChanged();
        if (currentXP >= XPToLvlUp)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        XPToLvlUp = (int)((XPToLvlUp + 200) * 1.2f); 
    }
}
