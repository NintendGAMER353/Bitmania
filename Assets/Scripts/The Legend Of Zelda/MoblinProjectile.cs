using UnityEngine;
using UnityEngine.SceneManagement;

public class MoblinProjectile : MonoBehaviour
{
    Rigidbody2D rb;  // Rigidbody 2D
    [SerializeField] private float velocidad;  // Velocidad proyectil lanza
    public static PlayerController player;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        player = PlayerController.instance;
    }

    void Update()
    {
        // Mover el objeto según su dirección y velocidad en cada frame
        transform.Translate(Time.deltaTime * velocidad * Vector2.left);
    }

    // Lanzar proyectil (con parámetro fuerza)
    public void Launch(Vector2 direction, float force)
    {
        rb.AddForce(direction * force);
    }


    // Al entrar en contacto con el jugador tanto este como el propio objeto se destruyen
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
