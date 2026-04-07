using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public bool win = false;
    public int lives = 4;
    public int points = 0;
    public int totalPoints = 0;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // No se destruye entre escenas
        }
        else
        {
            Destroy(gameObject); // Evita duplicados
        }
    }

    void Update()
    {
        Debug.Log("Puntos: " + points);
        Debug.Log("Puntos Totales: " + totalPoints);
        Debug.Log("Vidas: " + lives);
    }

    public void DecreaseLives()
    {
        lives--;
    }

    public void IncreasePoints()
    {
        points++;
        totalPoints++;
    }

    public void DecreasePointsTo0()
    {
        points = 0;
    }
}
