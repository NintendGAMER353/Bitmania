using UnityEngine;

public class MenuController : MonoBehaviour
{
    public AudioManagerMainMenu audioManagerMenu;

    void Awake()
    {

        audioManagerMenu = GetComponent<AudioManagerMainMenu>();
    }

    public void StartAction()
    {
        audioManagerMenu.PlayPlayButtonSound();
    }
    public void ExitAction()
    {
        audioManagerMenu.PlayExitButtonSound();
        Application.Quit();
    }
}
