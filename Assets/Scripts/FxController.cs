using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FxController : MonoBehaviour
{
    public ParticleSystem explosionPrefab;

    public void PlayExplosion(Vector3 pos)
    {
        var fx = Instantiate(explosionPrefab, pos, Quaternion.Euler(Vector3.right * 270f), transform);
        fx.GetComponent<ParticleSystem>().Play();
    }

    IEnumerator killDelayed(float delay, GameObject go)
    {
        yield return new WaitForSeconds(delay);
        Destroy(go);
    }
}
