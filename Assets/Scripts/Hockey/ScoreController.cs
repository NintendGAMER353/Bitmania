using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    private bool hasWon = false;
    public enum Score
    {
        AiScore, PlayerScore
    }

    public Text AIScoreTxt, PlayerScoreTxt;
    private int aiScore, playerScore;
    public static PlayerController player;
    public static GameController game;

    void Start()
    {
        player = PlayerController.instance;
        game = GameController.instance;
    }

    void Update()
    {
        // Condición victoria: el jugador marca 1 punto
        if (!hasWon)
        {
            if (playerScore == 1)
            {
                hasWon = true;
                player.win = true;
            }
        }

        // Condición derrota: la IA marca 1 punto
        if (aiScore == 1)
        {
            player.DecreaseLives();
            game.r = UnityEngine.Random.Range(1, 4);    // Se vuelve a minijuegos de nivel 1
            SceneManager.LoadScene(game.r);
        }
    }

    public void Increment(Score whichScore)
    {
        if (whichScore == Score.AiScore)
        {
            AIScoreTxt.text = (++aiScore).ToString();
        }
        else
        {
            PlayerScoreTxt.text = (++playerScore).ToString();
        }
    }
}
