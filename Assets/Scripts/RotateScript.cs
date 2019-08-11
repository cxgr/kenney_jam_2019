using System.Collections;
using DG.Tweening;
using UnityEngine;

public class RotateScript : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        RotateEndless();
    }

    // Update is called once per frame
    private void RotateEndless()
    {
        transform.DOBlendableLocalRotateBy(new Vector3(0, 180, 0), 1).OnComplete(RotateEndless).SetEase(Ease.Linear);
    }
}
