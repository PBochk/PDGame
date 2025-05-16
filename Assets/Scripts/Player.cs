using TMPro;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    private Rigidbody2D rb;
    public GameInput GameInput { get; private set; }
    private Vector2 moveInput;
    public bool facingRight = true;

    private Animator anim;

    [Header("Player Stats")]
    [SerializeField] private float movingSpeed = 10f;
    public int health;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        GameInput = gameObject.AddComponent<GameInput>();
    }

    private void Update()
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if(!facingRight && moveInput.x > 0
            || facingRight && moveInput.x < 0) 
        {
            Flip();
        }
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
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVector();
        rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));
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
}
