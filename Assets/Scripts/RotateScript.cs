using System.Collections;
using DG.Tweening;
using UnityEngine;

public class RotateScript : MonoBehaviour
{
    private Tween _currentTween;
    // Start is called before the first frame update
    private void Start()
    {
        RotateEndless();
    }

    private void OnDisable()
    {
        _currentTween.Kill();
    }

    // Update is called once per frame
    private void RotateEndless()
    {
        _currentTween = transform.DOBlendableLocalRotateBy(new Vector3(0, 180, 0), 1).OnComplete(RotateEndless).SetEase(Ease.Linear);
    }
}
