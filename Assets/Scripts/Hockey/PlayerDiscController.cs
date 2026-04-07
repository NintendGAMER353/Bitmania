using Unity.VisualScripting;
using UnityEngine;

public class BlueDiscController : MonoBehaviour
{
    CircleCollider2D playerCollider;
    Rigidbody2D rb; // Rigidbody 2D
    bool isClicked = true; // Booleano para saber si se ha hecho clic
    bool canMove; // Booleano para saber si se puede mover al jugador

    public Transform PlayerBoundaryHolder; // Almacena el GameObject "PlayerBoundaryHolder"
    Boundary playerBoundary; // Almacena los valores de los 4 puntos del GameObject "PlayerBoundaryHolder" (Struct "Boundary")

    void Awake()
    {
        playerCollider = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        playerBoundary = new Boundary(PlayerBoundaryHolder.GetChild(0).position.y,
                                      PlayerBoundaryHolder.GetChild(1).position.y,
                                      PlayerBoundaryHolder.GetChild(2).position.x,
                                      PlayerBoundaryHolder.GetChild(3).position.x);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // Posición global (Para todas las pantallas)
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (isClicked)
            {
                isClicked = false;

                // Comprueba que el cursor está dentro del área del objeto
                if (playerCollider.OverlapPoint(mousePos))
                {
                    canMove = true;
                }
                else
                {
                    canMove = false;
                }
            }

            // Si se puede mover se mueve usando como límite los 4 puntos de "playerBoundary"
            if (canMove)
            {
                Vector2 clampedMousePos = new Vector2(Mathf.Clamp(mousePos.x, playerBoundary.Left, playerBoundary.Right),
                                                      Mathf.Clamp(mousePos.y, playerBoundary.Down, playerBoundary.Up));
                rb.MovePosition(clampedMousePos);
            }
        }
        else
        {
            isClicked = true;
        }
    }
}
