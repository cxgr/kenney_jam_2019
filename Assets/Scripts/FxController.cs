using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FxController : MonoBehaviour
{
    public GameObject bubblePrefab;
    public ParticleSystem explosionPrefab;

    public ParticleSystem evac1;
    public ParticleSystem evac2;

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

    public SpeechBubble GetSpeechBubble()
    {
        var b = Instantiate(bubblePrefab, transform);
        b.SetActive(true);
        return b.GetComponent<SpeechBubble>();
    }

    public void PlayEvacFx(Vector3 from, Vector3 to)
    {
        evac1.transform.position = from;
        evac1.Play();
        evac2.transform.position = to;
        evac2.Play();
    }
}
