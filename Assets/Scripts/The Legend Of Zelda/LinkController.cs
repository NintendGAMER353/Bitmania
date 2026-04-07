using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class LinkController : MonoBehaviour
{
    SpriteRenderer sr;  // SpriteRenderer
    Rigidbody2D rb;  // Rigidbody 2D
    Animator a;  // Animator
    public GameObject ProjectileSword;  // GameObject proyectil espada (En escena)
    public float speed = 10f;  // Velocidad jugador
    public InputAction Move;  // Input movimiento
    public Vector2 move;  // Movimiento jugador

    private bool hasWon = false; // Evitar múltiples victorias
    public static PlayerController player;
    public static GameController game;

    void Awake()
    {
        Move.Enable();
        rb = GetComponent<Rigidbody2D>();
        a = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        player = PlayerController.instance;
        game = GameController.instance;
    }
    void Update()
    {
        // Ejecutar la función para las animaciones de movimiento y al pulsar "Z" ejecutar la función para lanzar proyectil
        movement();
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Launch();
        }

        // Condición victoria: Matar a todos los Moblins
        if (!hasWon)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length == 0)
            {
                hasWon = true; // 🛡️ Prevenir futuras ejecuciones
                player.win = true;
            }
        }

    }

    void FixedUpdate()
    {
        // Mover al jugador
        Vector2 position = rb.position + move * speed * Time.deltaTime;
        rb.MovePosition(position);
    }

    // Disparar proyectil
    void Launch()
    {
        GameObject projectileObject = Instantiate(ProjectileSword, rb.position + Vector2.right * 0.5f, Quaternion.identity);
        LinkProjectile p = projectileObject.GetComponent<LinkProjectile>();
        p.Launch(move, 200);
        a.SetBool("ALeftRight", true);
        Invoke("ResetAttackAnimation", 0.3f);
    }

    // Controlar animaciones de movimiento según el último movimiento
    void movement()
    {
        move = Move.ReadValue<Vector2>();

        if (move == new Vector2(1, 0))
        {
            a.SetFloat("WLeftRight", 1);
        }
        else if (move == new Vector2(-1, 0))
        {
            a.SetFloat("WLeftRight", 1);
            sr.flipX = true;
        }
        else
        {
            a.SetFloat("WLeftRight", 0);
            sr.flipX = false;
        }

        if (move == new Vector2(0, 1))
        {
            a.SetFloat("WUp", 1);
        }
        else
        {
            a.SetFloat("WUp", 0);
        }

        if (move == new Vector2(0, -1))
        {
            a.SetFloat("WDown", 1);
        }
        else
        {
            a.SetFloat("WDown", 0);
        }
    }

    // Condición derrota: Link choca contra el proyectil de un Moblin o contra el Moblin
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            player.DecreaseLives();
            game.r = UnityEngine.Random.Range(1, 4);    // Se vuelve a minijuegos de nivel 1
            SceneManager.LoadScene(game.r);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Destroy(gameObject);
            player.DecreaseLives();
            game.r = UnityEngine.Random.Range(1, 4);    // Se vuelve a minijuegos de nivel 1
            SceneManager.LoadScene(game.r);
        }
    }

    void ResetAttackAnimation()
    {
        a.SetBool("ALeftRight", false);
    }
}
