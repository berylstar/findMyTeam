using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip bgm;
    public AudioSource audioSource;

    private void Start()
    {
        audioSource.clip = bgm;
        audioSource.Play();
    }
}
