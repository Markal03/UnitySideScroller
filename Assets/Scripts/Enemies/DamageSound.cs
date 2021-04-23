using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSound : MonoBehaviour
{
    [SerializeField]
    public AudioClip[] clips;

    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayDamageSound()
    {
        AudioClip clip = GetRandomClip();
        audioSource.pitch = (Random.Range(0.6f, 1.4f));
        audioSource.PlayOneShot(clip);
    }

    private AudioClip GetRandomClip()
    {
        return clips[UnityEngine.Random.Range(0, clips.Length)];
    }
}
