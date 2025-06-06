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
    public static Player Instance { get; private set; }
    private HealthDisplay healthDisplay;
    private Rigidbody2D rb;
    public GameInput GameInput { get; private set; }
    private Vector2 moveInput;
    public bool facingRight = true;

    private Animator anim;

    [Header("Player Stats")]
    [SerializeField] private float movingSpeed = 10f;
    private float currentSpeed;
    public int health;
    public int maxHealth;

    //Переменные для отбрасывания
    private float knockback = 10f;
    private float stunTime = 0.1f;
    private float currentStunTime = 0f;
    private Enemy knockedEnemy;


    private void Start()
    {
        //maxHealth = health;
        anim = GetComponent<Animator>();
        healthDisplay = FindFirstObjectByType<HealthDisplay>();
        currentSpeed = movingSpeed;
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
        if (!facingRight && mousePos.x > playerScreenPos.x
            || facingRight && mousePos.x < playerScreenPos.x)
        {
            Flip();
        }
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (moveInput.x == 0 && moveInput.y == 0)
        {
            anim.SetBool("isRunning", false);
        }
        else
        {
            anim.SetBool("isRunning", true);
        }

        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void FixedUpdate()
    {
        Vector2 targetPosition;

        if (currentStunTime <= 0)
        {
            targetPosition = GameInput.Instance.GetMovementVector();
            currentSpeed = movingSpeed;
        }
        else
        {
            currentStunTime -= Time.fixedDeltaTime;
            targetPosition = (transform.position - knockedEnemy.transform.position).normalized;
            currentSpeed = knockback;
        }
        rb.MovePosition(rb.position + targetPosition * (currentSpeed * Time.fixedDeltaTime));

        
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
        health += dif;
        healthDisplay.HealthChanged();
    }

    public void Knockback(Enemy enemy)
    {
        currentStunTime = stunTime;
        knockedEnemy = enemy;
    }




}
