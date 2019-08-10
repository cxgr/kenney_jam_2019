using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
    public Animator anim;
    public Transform camT;

    private void Awake()
    {
        camT = Camera.main.transform;
    }

    public void PlayClip(Vector3 pos, string clip)
    {
        transform.position = pos;
        anim.Play(clip);
    }

    public void Release()
    {
        anim.Play("blank");
        Destroy(gameObject, 1f);
    }

    void Update()
    {
        transform.LookAt(camT, camT.up);
    }
}
