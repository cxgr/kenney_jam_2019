using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FxController : MonoBehaviour
{
    public GameObject bubblePrefab;
    public ParticleSystem explosionPrefab;

    public GameObject carTrail;
    public GameObject headlights;

    public ParticleSystem evac1;
    public ParticleSystem evac2;

    public void PlayExplosion(Vector3 pos)
    {
        var fx = Instantiate(explosionPrefab, pos, Quaternion.Euler(Vector3.right * 270f), transform);
        fx.GetComponent<ParticleSystem>().Play();
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

    public void AttachCarDeco(Transform carT)
    {
        var newTrail = Instantiate(carTrail, carT);
        newTrail.transform.localRotation = Quaternion.Euler(Vector3.right * 270f);
        newTrail.transform.localPosition = Vector3.back * .2f + Vector3.up * 0.05f;
        newTrail.GetComponent<ParticleSystem>().Play();

        return;
        var l1 = Instantiate(headlights, carT);
        l1.transform.localRotation = Quaternion.Euler(Vector3.right * 20f);
        l1.transform.localPosition = new Vector3(-.08f, .1f, .2f);
        l1.SetActive(true);
        
        var l2 = Instantiate(headlights, carT);
        l2.transform.localRotation = Quaternion.Euler(Vector3.right * 20f);
        l2.transform.localPosition = new Vector3(.08f, .1f, .2f);
        l2.SetActive(true);
    }
}
