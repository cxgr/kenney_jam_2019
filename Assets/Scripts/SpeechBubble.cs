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
    
    
    private Transform followT;
    private Vector3 followOffset;
    private bool fwdMode;
    private float fwdOffsetLen;
    public void FollowMe(Transform t, Vector3 offset, bool fwdFollow = false, float fwdOffsetLen = 0f)
    {
        followT = t;
        followOffset = offset;
        fwdMode = fwdFollow;
    }

    void Update()
    {
        if (null != followT)
        {
            var followPos = followT.position + followOffset;
            if (fwdMode)
                followPos += followT.forward * fwdOffsetLen;
            transform.position = followPos;
        }

        transform.LookAt(camT, camT.up);
    }
}
