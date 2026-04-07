using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AudioManagerMainMenu : MonoBehaviour
{
    public AudioClip PlayButtonSound;
    public AudioClip ExitButtonSound;
    private AudioSource audioMenu;
    int r;

    void Awake()
    {
        audioMenu = GetComponent<AudioSource>();
        r = UnityEngine.Random.Range(1, 4);
    }

    public void PlayPlayButtonSound()
    {
        StartCoroutine(WaitSoundPlay());
    }

    public void PlayExitButtonSound()
    {
        StartCoroutine(WaitSoundExit());
    }

    IEnumerator WaitSoundPlay()
    {
        audioMenu.PlayOneShot(PlayButtonSound);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(r);
    }

    IEnumerator WaitSoundExit()
    {
        audioMenu.PlayOneShot(ExitButtonSound);
        yield return new WaitForSeconds(1f);
        Application.Quit();
    }
}
