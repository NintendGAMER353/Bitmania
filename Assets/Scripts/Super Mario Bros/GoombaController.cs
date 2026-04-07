using UnityEngine;
using System.Collections;

public class GoombaController : MonoBehaviour
{
    // Velocidad de movimiento del Goomba
    public float moveSpeed = 1f;

    // Referencia al Animator para controlar animaciones
    public Animator animator;

    // Tiempo que tarda en desaparecer tras morir
    public float deathDelay = 0.5f;

    // Referencia al Rigidbody2D del Goomba
    private Rigidbody2D rb;

    // Referencia al Collider2D del Goomba
    private Collider2D col;

    // Dirección actual de movimiento (true = derecha, false = izquierda)
    private bool movingRight = false;

    // Indica si el Goomba está muerto
    private bool isDead = false;

    // Límite izquierdo del área visible
    private float limiteIzquierdo;

    // Límite derecho del área visible
    private float limiteDerecho;

    void Awake()
    {
        // Obtener componentes Rigidbody2D y Collider2D
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        // Calcular los bordes visibles de la cámara
        Camera cam = Camera.main;
        float camHeight = 2f * cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;
        Vector3 camPos = cam.transform.position;

        // Calcular límites horizontales de movimiento
        limiteIzquierdo = camPos.x - camWidth / 2f;
        limiteDerecho = camPos.x + camWidth / 2f;
    }

    void Update()
    {
        // Si está muerto, no se mueve
        if (isDead) return;

        // Movimiento a la izquierda
        rb.linearVelocity = new Vector2((movingRight ? 1 : -1) * moveSpeed, rb.linearVelocity.y);

        // Mantener al Goomba dentro de los límites horizontales de la cámara
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, limiteIzquierdo, limiteDerecho);
        transform.position = pos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si está muerto, ignorar colisiones
        if (isDead) return;

        // Si colisiona con el jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            // Obtener la dirección del impacto
            Vector2 normal = collision.contacts[0].normal;

            // Obtener el controlador de Mario
            MarioController mario = collision.collider.GetComponent<MarioController>();

            if (normal.y < -0.5f)
            {
                // Si Mario cae sobre el Goomba, lo mata
                Die();

                // Mario rebota hacia arriba
                Rigidbody2D marioRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (marioRb != null)
                {
                    marioRb.linearVelocity = new Vector2(marioRb.linearVelocity.x, 10f);
                }
            }
            else
            {
                // Si Mario choca desde un lado, Mario muere
                mario.Die();
            }
        }
    }

    // Muerte del Goomba
    public void Die()
    {
        if (isDead) return;
        isDead = true;

        // Activar animación de muerte
        animator.SetTrigger("Die");

        // Detener el movimiento
        rb.linearVelocity = Vector2.zero;

        // Cambiar cuerpo a cinemático (para no interactuar con física)
        rb.bodyType = RigidbodyType2D.Kinematic;

        // Desactivar colisiones
        col.enabled = false;

        // Iniciar cuenta atrás para destruir el objeto
        StartCoroutine(DeathDelay());
    }

    // Espera un tiempo antes de destruir el Goomba
    private IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(deathDelay);
        Destroy(gameObject);
    }
}
