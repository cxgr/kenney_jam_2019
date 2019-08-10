using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class VehicleController : MonoBehaviour
{
    public float moveSpd;
    
    public List<Tile> path;
    public VehicleSpawner owner;

    private Tween goPathTween;

    private MapHolder map => SingletonUtils<MapHolder>.Instance;
    private FxController fx => SingletonUtils<FxController>.Instance;

    private UiManager ui => SingletonUtils<UiManager>.Instance;

    private bool isStopped = false;

    private SpeechBubble currentBubble;
    private Coroutine currentWaitingCor;

    public bool isVIP;

    public bool ToggleEngine()
    {
        if (!isStopped && !goPathTween.IsPlaying())
            return false;
        
        isStopped = !isStopped;

        if (isStopped)
        {
            currentBubble = fx.GetSpeechBubble();
            currentBubble.PlayClip(transform.position + Vector3.up * .5f, "waiting");
            currentWaitingCor = StartCoroutine(waitingCor());
        }
        else
        {
            if (null != currentBubble)
                currentBubble.Release();
            currentBubble = null;
        }

        goPathTween.TogglePause();

        return isStopped;
    }

    public void HandleEvac()
    {
        if (!isStopped && !goPathTween.IsPlaying())
            return;

        isStopped = false;
        if (null != currentBubble)
            currentBubble.Release();
        StopAllCoroutines();
        
        fx.PlayEvacFx(transform.position, goPathTween.PathGetPoint(0));

        goPathTween.Restart();
    }

    IEnumerator waitingCor()
    {
        yield return new WaitForSeconds(2f);
        ui.Performance -= isVIP ? map.performanceDropVIP : map.performanceDropPerTick;
        yield return new WaitForSeconds(.5f);
    }

    public void Go(List<Tile> path, VehicleSpawner owner, bool isVIP = false)
    {
        transform.localScale = Vector3.one * .7f;
        
        this.path = path;
        this.owner = owner;
        this.isVIP = isVIP;
        
        transform.LookAt(path[1].GetMovementPos(), Vector3.up);

        goPathTween = transform.DOPath(
                path.Select(p => map.mapGraph[p.coordTuple].GetMovementPos()).ToArray(),
                path.Count / moveSpd * map.carSlowdown, PathType.CatmullRom , PathMode.Full3D, 20)
            .SetOptions(false, AxisConstraint.None, AxisConstraint.X | AxisConstraint.Z)
            .SetLookAt(0.001f, null, Vector3.up).SetEase(Ease.Linear)
            .OnComplete(() => Despawn(true));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isExploding)
            return;

        var otherCar = other.GetComponent<VehicleController>();
        if (null != otherCar)
        {
            Explode();
            otherCar.Explode();
        }
    }

    private bool isExploding;
    public void Explode()
    {
        if (isExploding)
            return;
        
        StopAllCoroutines();
        if (null != currentBubble)
        {
            currentBubble.Release();
            currentBubble = null;
        }

        ui.Performance -= isVIP ? map.performanceDropPerCrashVIP : map.performanceDropPerCrash;

        isExploding = true;
        transform.DOScale(1.2f, .05f).OnComplete(() =>
        {
            SingletonUtils<FxController>.Instance.PlayExplosion(transform.position);
            Despawn();
        });
    }
    
    public void Despawn(bool success = false)
    {
        owner.RemoveMe(this);

        if (success)
        {
            currentBubble = fx.GetSpeechBubble();
            currentBubble.PlayClip(transform.position + Vector3.up * .5f, RandomHappy());

            ui.Performance += isVIP ? map.performanceGainPerArrivalVIP : map.performanceGainPerArrival;

            GetComponent<Collider>().enabled = false;
            Destroy(gameObject, 1f);
        }
        else
        {
            if (null != currentBubble)
                currentBubble.Release();
            Destroy(gameObject);
        }
    }

    private string RandomHappy()
    {
        if (Random.value < .333f)
            return "happy1";
        if (Random.value < .6666f)
            return "happy2";
        return "happy3";
    }
}
