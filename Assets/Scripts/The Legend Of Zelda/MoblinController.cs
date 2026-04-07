using UnityEngine;
using UnityEngine.UIElements;

public class MoblinController : MonoBehaviour
{
    Rigidbody2D rb;  // Rigidbody 2D
    Animator a;  // Animator
    SpriteRenderer sr;  // SpriteRenderer
    public GameObject ProjectileMoblin;  // GameObject proyectil lanza (En escena)
    int direction = 1;  // Dirección (Izquierda / Derecha)
    public bool vertical;  // Orientación (Vertical / Horizontal)
    Vector2 position; // Movimiento enemigo
    public float speed = 5f;  // Velocidad enemigo
    public float changeTime = 3.0f;  // Reseteo timer movimiento
    float timer;  // Timer movimiento
    bool canLaunch = true;  // Control para lanzar ataque (Al terminar el timer de ataque)
    float launchCooldown = 4f;  // Reseteo timer cooldown ataque
    float launchCooldownTimer = 0f;  // Timer cooldown ataque

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        a = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        timer = changeTime;
    }

    void Update()
    {
        // Bajar contador movimiento y al reiniciar el contador cambiar de dirección
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
            vertical = !vertical;
        }

        // Manejar cooldown del disparo
        if (!canLaunch)
        {
            launchCooldownTimer -= Time.deltaTime;
            if (launchCooldownTimer <= 0f)
            {
                canLaunch = true;
            }
        }

        // Probabilidad de disparar (si está permitido)
        if (canLaunch && Random.value < 0.01f)
        {
            Launch();
            canLaunch = false;
            launchCooldownTimer = launchCooldown;
        }

    }

    void FixedUpdate()
    {
        // Manejar movimiento (según la dirección) y animaciones de movimiento
        position = rb.position;
        if (vertical)
        {
            position.y += speed * direction * Time.deltaTime;

            a.SetFloat("WUp", direction > 0 ? 1 : 0);
            a.SetFloat("WDown", direction < 0 ? 1 : 0);

            a.SetFloat("WLeftRight", 0);
            sr.flipX = false;
        }
        else
        {
            position.x += speed * direction * Time.deltaTime;
            a.SetFloat("WLeftRight", 1);
            sr.flipX = direction < 0;

            a.SetFloat("WUp", 0);
            a.SetFloat("WDown", 0);
        }
        rb.MovePosition(position);
    }

    // Disparar proyectil
    void Launch()
    {
        GameObject projectileMoblin = Instantiate(ProjectileMoblin, rb.position + Vector2.left * 0.5f, Quaternion.identity);
        MoblinProjectile p = projectileMoblin.GetComponent<MoblinProjectile>();
        p.Launch(position, 0);
    }

    // Al entrar en colisión con una pared cambia de dirección
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") && timer != 0)
        {
            vertical = !vertical;
        }
    }
}
