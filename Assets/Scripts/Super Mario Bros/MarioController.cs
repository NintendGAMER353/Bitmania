using UnityEngine;
using UnityEngine.SceneManagement;
public class MarioController : MonoBehaviour
{
    [Header("Teclas Movimiento")]
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode jumpKey = KeyCode.W;

    [Header("Parámetros Movimiento")]
    public float moveSpeed = 5f;
    public float acceleration = 10f;
    public float deceleration = 20f;
    public float velocityPower = 0.9f;
    public float maxSpeed = 5f;

    [Header("Parámetros Salto")]
    public float jumpForce = 12f;
    public float jumpCutMultiplier = 0.5f;
    public float maxJumpHeight = 2f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;

    [Header("Gravedad")]

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("Límites Cámara")]

    public bool limitarMovimiento = true;
    public float margenIzquierdo = 0.5f;
    public float margenDerecho = 0.5f;

    private float limiteIzquierdo;
    private float limiteDerecho;

    private Rigidbody2D rb;
    private Collider2D col;
    private float moveInput;
    private bool isGrounded;
    private float jumpStartY;
    private bool isDead = false;
    private bool hasWon = false; // Evitar múltiples ejecuciones
    Animator animator;
    public AudioManagerMario audioMario;
    public static PlayerController player;
    public static GameController game;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        // Calcula los límites de movimiento según la cámara
        if (limitarMovimiento)
        {
            Camera cam = Camera.main;
            float camHeight = 2f * cam.orthographicSize;
            float camWidth = camHeight * cam.aspect;
            Vector3 camPos = cam.transform.position;

            limiteIzquierdo = camPos.x - camWidth / 2f + margenIzquierdo;
            limiteDerecho = camPos.x + camWidth / 2f - margenDerecho;
        }
    }

    void Start()
    {
        player = PlayerController.instance;
        game = GameController.instance;
    }

    void Update()
    {
        if (isDead || hasWon) return;

        moveInput = 0f;

        // Entrada de movimiento horizontal
        if (Input.GetKey(leftKey))
        {
            moveInput = -1f;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (Input.GetKey(rightKey))
        {
            moveInput = 1f;
            transform.localScale = new Vector3(1, 1, 1);
        }

        animator.SetFloat("Speed", Mathf.Abs(moveInput));

        // Saltar
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }

        // Recortar salto si se suelta la tecla
        if (Input.GetKeyUp(jumpKey) && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
        }

        // Limitar altura de salto
        if (transform.position.y >= jumpStartY + maxJumpHeight && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        }

        // Verifica si está en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        animator.SetBool("isJumping", !isGrounded);

        // Condición victoria: Matar a todos los Goombas
        if (!hasWon)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length == 0)
            {
                hasWon = true;
                player.win = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (isDead || hasWon) return;

        // Movimiento horizontal con aceleración
        float targetSpeed = moveInput * moveSpeed;
        float speedDiff = targetSpeed - rb.linearVelocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, velocityPower) * Mathf.Sign(speedDiff);

        rb.AddForce(movement * Vector2.right);

        // Limitar velocidad máxima
        rb.linearVelocity = new Vector2(Mathf.Clamp(rb.linearVelocity.x, -maxSpeed, maxSpeed), rb.linearVelocity.y);

        // Ajuste de gravedad según movimiento vertical
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = fallMultiplier;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetKey(jumpKey))
        {
            rb.gravityScale = lowJumpMultiplier;
        }
        else
        {
            rb.gravityScale = 1f;
        }

        // Limita el movimiento horizontal
        if (limitarMovimiento)
        {
            float clampedX = Mathf.Clamp(transform.position.x, limiteIzquierdo, limiteDerecho);
            transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
        }
    }

    // Condición derrota: Mario choca contra un Goomba
    public void Die()
    {
        isDead = true;
        col.enabled = false;
        animator.SetTrigger("Die");

        player.DecreaseLives();
        game.r = UnityEngine.Random.Range(1, 4);
        SceneManager.LoadScene(game.r);
    }

    // Salto
    public void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        jumpStartY = transform.position.y;
        animator.SetBool("isJumping", true);
        audioMario.PlayMarioJump();
    }
}
