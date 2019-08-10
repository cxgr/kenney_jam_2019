using DG.Tweening;
using UnityEngine;

public class Thrust2D : MonoBehaviour
{
    [SerializeField] private Vector2 _scaleMultiplierMinMax = new Vector2(.5f, 1.25f);

    [SerializeField] private Vector3[] scaleOriginal;

    [SerializeField] private float _scaleChangeSpeed = 1.5f;

    [SerializeField] private Transform[] targets;

    public Sequence currentSequence;
    // ReSharper disable once UnusedMember.Local
    private void Start()
    {
        scaleOriginal = new Vector3[targets.Length];
        for (var i = 0; i < scaleOriginal.Length; i++)
        {
            scaleOriginal[i] = targets[i].localScale;
        }

        Animate();

    }

    // Update is called once per frame
    private void Animate()
    {
        var randomValue = Random.Range(_scaleMultiplierMinMax.x, _scaleMultiplierMinMax.y);
        currentSequence = DOTween.Sequence();
        currentSequence.OnComplete(Animate);
        currentSequence.SetSpeedBased();
        for (var index = 0; index < targets.Length; index++)
        {
            var target = targets[index];
            var targetScale = scaleOriginal[index] * randomValue;
            if (index == 0)
                currentSequence.Append(target.DOScale(targetScale, _scaleChangeSpeed).SetSpeedBased());
            else
            {
                currentSequence.Join(target.DOScale(targetScale, _scaleChangeSpeed).SetSpeedBased());
            }
        }

        currentSequence.Play();
    }
}
