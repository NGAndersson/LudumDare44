using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkateSoundPlayer : MonoBehaviour
{
    public AudioSource playerSource;
    public AudioClip[] skateSounds;

    public void PlayRandomSkateSound()
    {
        AudioClip clip = skateSounds[Random.Range(0, skateSounds.Length)];

        playerSource.PlayOneShot(clip);
    }

}
