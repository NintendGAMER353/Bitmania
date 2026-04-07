using UnityEngine;

public class AudioManagerMario : MonoBehaviour
{
    public AudioClip MarioJump;
    public AudioClip MarioDie;
    public AudioClip GoombaDie;

    private AudioSource audioMario;

    void Awake()
    {
        audioMario = GetComponent<AudioSource>();
    }

    public void PlayMarioJump()
    {
        audioMario.PlayOneShot(MarioJump);
    }

    public void PlayMarioDie()
    {
        audioMario.PlayOneShot(MarioDie);
    }

    public void PlayGoombaDie()
    {
        audioMario.PlayOneShot(GoombaDie);
    }












}
