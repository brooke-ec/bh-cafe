using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clang : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clip;
    public float volume = 0.5f;

    public void ClangPlay()
    {
        audioSource.PlayOneShot(clip, volume);
    }
}
