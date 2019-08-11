using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    bool audioOn = true;
    public bool IsAudioOn => audioOn;
    
    private Dictionary<string, AudioSource> soundsDict = new Dictionary<string, AudioSource>();
    
    private AudioSource loopSrc => GetComponent<AudioSource>();

    public void SetAudio(bool on)
    {
        if (on && !audioOn)
        {
            loopSrc.Play();
        }
        else
        {
            loopSrc.Stop();
        }

        audioOn = on;
    }

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            var src = transform.GetChild(i).GetComponent<AudioSource>();
            soundsDict.Add(src.gameObject.name, src);
        }
        
        loopSrc.Play();
    }

    public void PlaySound(string key)
    {
        if (!audioOn)
            return;
        if (soundsDict.ContainsKey(key))
        {
            var s = soundsDict[key];
            s.PlayOneShot(s.clip);
        }
        else
        {
            Debug.LogError($"no key for {key}");
        }
    }

    public void Play3D(string key, Vector3 position)
    {
        if (!audioOn) return;
        
        if (soundsDict.ContainsKey(key))
        {
            var s = soundsDict[key];
            AudioSource.PlayClipAtPoint(s.clip, position, s.volume);
        }
        else
        {
            Debug.LogError($"no key for {key}");
        }
    }
}
