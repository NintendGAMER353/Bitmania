using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static PlayerController player;
    public static GameController instance;
    public int r = 0;

    void Awake()
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

    void Start()
    {
        player = PlayerController.instance;
    }

    void Update()
    {
        if (player.win)
        {
            player.win = false;
            player.IncreasePoints();
            PointsConditions();
            StartMiniGame();
        }

        if (player.lives == 0)
        {
            Application.Quit();
        }

        Debug.Log("Velocidad: " + Time.timeScale);
    }

    public void StartMiniGame()
    {
        if (player.points >= 0 && player.points <= 2)
        {
            r = UnityEngine.Random.Range(1, 4);
        }
        if (player.points >= 3 && player.points <= 5)
        {
            r = UnityEngine.Random.Range(4, 7);
        }
        if (player.points >= 6 && player.points <= 8)
        {
            r = UnityEngine.Random.Range(7, 10);
        }
        SceneManager.LoadScene(r);
    }

    public void PointsConditions()
    {
        if (player.points == 2 || player.points == 5 || player.points == 8)
        {
            IncreaseSpeed();
        }
        if (player.points == 8)
        {
            player.DecreasePointsTo0();
        }
    }

    public void IncreaseSpeed()
    {
        Time.timeScale += 0.2f;
    }
}
