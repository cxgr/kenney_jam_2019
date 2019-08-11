using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    bool audioOn = true;
    public bool IsAudioOn => audioOn;

    private AudioSource loopSrc => GetComponent<AudioSource>();

    public void SetAudio(bool on)
    {
        if (on && !audioOn)
        {
            loopSrc.volume = 1f;
            loopSrc.Play();
        }
        else
        {
            loopSrc.volume = 0f;
            loopSrc.Stop();
        }

        audioOn = on;
    }

    private void Awake()
    {
        loopSrc.Play();
    }
}
