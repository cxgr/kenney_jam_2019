using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using UnityEngine;

[RequireComponent(typeof(DOTweenPath))]
public class EnemyBase : MonoBehaviour
{
    [SerializeField]
    private Path pathComponent;
    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void NewTarget()
    {
        float closest = float.MaxValue;
        foreach (var target in FindObjectsOfType<Killable>())
        {
            var dist=Vector2.Distance(target.transform.position, transform.position);
            if (dist < closest)
                closest = dist;
        }
    }
}
