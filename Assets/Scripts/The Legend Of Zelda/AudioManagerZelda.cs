using UnityEngine;

public class AudioManagerZelda : MonoBehaviour
{

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

}
