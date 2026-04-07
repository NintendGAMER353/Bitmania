using System.Collections;
using UnityEngine;

public class PuckController : MonoBehaviour
{
    public ScoreController ScoreControllerInstance;
    public static bool wasGoal { get; private set; }
    public float maxSpeed;
    private Rigidbody2D rb;
    public AudioManager audioManager;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        wasGoal = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "AIGoal")
        {
            ScoreControllerInstance.Increment(ScoreController.Score.PlayerScore);
            wasGoal = true;
            audioManager.PlayGoal();
            StartCoroutine(ResetPuck());
        }
        else if (other.tag == "PlayerGoal")
        {
            ScoreControllerInstance.Increment(ScoreController.Score.AiScore);
            wasGoal = true;
            audioManager.PlayGoal();
            StartCoroutine(ResetPuck());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        audioManager.PlayPuckCollision();
    }

    private IEnumerator ResetPuck()
    {
        yield return new WaitForSecondsRealtime(1);
        wasGoal = false;
        // Posición inicial del disco tras un punto
        rb.linearVelocity = rb.position = new Vector2(-3.4f, 0);

    }

    private void FixedUpdate()
    {
        // Limitar la velocidad del disco
        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxSpeed);
    }
}
