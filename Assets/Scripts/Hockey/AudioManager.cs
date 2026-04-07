using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip PuckCollision;
    public AudioClip Goal;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayPuckCollision()
    {
        audioSource.PlayOneShot(PuckCollision);
    }

    public void PlayGoal()
    {
        audioSource.PlayOneShot(Goal);
    }












}
