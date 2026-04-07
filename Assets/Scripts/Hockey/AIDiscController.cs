using UnityEngine;
using UnityEngine.UIElements;

public class AIScriptController : MonoBehaviour
{
    public float MaxMovementSpeed;
    public float force;
    private Rigidbody2D rb;
    private Vector2 startingPosition;

    public Rigidbody2D rbPuck;

    public Transform AIBoundaryHolder;
    private Boundary AIBoundary;

    public Transform PuckBoundaryHolder;
    private Boundary puckBoundary;

    private Vector2 targetPosition;
    private bool isFirstTimeInOpponentsHalf = true;
    private float offsetXFromTarget;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startingPosition = rb.position;

        AIBoundary = new Boundary(AIBoundaryHolder.GetChild(0).position.y,
                                      AIBoundaryHolder.GetChild(1).position.y,
                                      AIBoundaryHolder.GetChild(2).position.x,
                                      AIBoundaryHolder.GetChild(3).position.x);

        puckBoundary = new Boundary(PuckBoundaryHolder.GetChild(0).position.y,
                                    PuckBoundaryHolder.GetChild(1).position.y,
                                    PuckBoundaryHolder.GetChild(2).position.x,
                                    PuckBoundaryHolder.GetChild(3).position.x);
    }

    void FixedUpdate()
    {
        if (!PuckController.wasGoal)
        {
            float movementSpeed;

            if (rbPuck.position.y < puckBoundary.Down)
            {
                if (isFirstTimeInOpponentsHalf)
                {
                    isFirstTimeInOpponentsHalf = false;
                    offsetXFromTarget = Random.Range(-1, 1);
                }
                movementSpeed = MaxMovementSpeed * Random.Range(0.2f, 0.5f);
                targetPosition = new Vector2(Mathf.Clamp(rbPuck.position.x + offsetXFromTarget, AIBoundary.Left, AIBoundary.Right),
                                                         startingPosition.y);
            }
            else
            {
                isFirstTimeInOpponentsHalf = true;
                movementSpeed = Random.Range(MaxMovementSpeed * 0.4f, MaxMovementSpeed);
                targetPosition = new Vector2(Mathf.Clamp(rbPuck.position.x, AIBoundary.Left, AIBoundary.Right),
                                             Mathf.Clamp(rbPuck.position.y, AIBoundary.Down, AIBoundary.Up));
            }

            rb.MovePosition(Vector2.MoveTowards(rb.position, targetPosition, movementSpeed * force * Time.fixedDeltaTime));
        }
    }
}
