using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingSwap : MonoBehaviour
{
    [SerializeField] Material[] _material;
    [SerializeField] MeshRenderer _targetRenderer;

    [SerializeField] private float each;

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(Swap());
    }

    // Update is called once per frame
    private IEnumerator Swap()
    {
        int i = 0;
        while (true)
        {
            yield return new WaitForSeconds(each);
            ++i;
            if (i >= _material.Length)
                i = 0;
            _targetRenderer.sharedMaterial = _material[i];
        }
    }
}
