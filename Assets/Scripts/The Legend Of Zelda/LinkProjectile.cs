using UnityEngine;

public class LinkProjectile : MonoBehaviour
{
    Rigidbody2D rb;  // Rigidbody 2D
    [SerializeField] private float velocidad;  // Velocidad proyectil espada

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Mover el objeto según su dirección y velocidad en cada frame
        transform.Translate(Time.deltaTime * velocidad * Vector2.right);
    }

    // Lanzar proyectil (con parámetro fuerza)
    public void Launch(Vector2 direction, float force)
    {
        rb.AddForce(direction * force);
    }

    // Al entrar en contacto con el enemigo tanto este como el propio objeto se destruyen
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }

    }

}
