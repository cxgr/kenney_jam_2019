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

    public bool isStoppedByPlayer = false;

    private SpeechBubble currentBubble;
    private Coroutine currentWaitingCor;

    public bool isVIP;

    public enum CarStates
    {
        Stopped,
        Accelerating,
        Going,
        Decelerating,
        Boosted,
    }

    public CarStates carState = CarStates.Stopped;
    public float acceleration = 200;
    public float accelFactor = 0f;

    public bool ToggleEngine(bool ignoreSound = false)
    {
        if (carState == CarStates.Boosted)
            return false;
        
        if (!isStoppedByPlayer && !goPathTween.IsPlaying())
            return false;

        isStoppedByPlayer = !isStoppedByPlayer;
        
        if (!ignoreSound)
            SingletonUtils<SoundManager>.Instance.PlaySound(isStoppedByPlayer ? "car_off" : "car_on");

        if (isStoppedByPlayer)
            carState = CarStates.Decelerating;
        else
            carState = CarStates.Accelerating;


        if (isStoppedByPlayer)
        {
            if (null != angeryBubble)
            {
                StopAllCoroutines();
                angeryBubble.Release();
                angeryBubble = null;
            }
            
            currentBubble = fx.GetSpeechBubble();
            currentBubble.PlayClip(transform.position + Vector3.up * .5f, "waiting");
            currentWaitingCor = StartCoroutine(waitingCor());
                    
            currentBubble.FollowMe(transform, Vector3.up * .5f);
        }
        else
        {
            if (null != currentBubble)
                currentBubble.Release();
            currentBubble = null;
        }

        //goPathTween.TogglePause();

        return isStoppedByPlayer;
    }

    public void HandleEvac()
    {
        if (!isStoppedByPlayer && !goPathTween.IsPlaying())
            return;
        
        SingletonUtils<SoundManager>.Instance.Play3D(Random.value < .5f ? "explosion1" : "explosion2", transform.position);
        SingletonUtils<SoundManager>.Instance.Play3D("explosion_addon", transform.position);
        Explode();
        return;

        isStoppedByPlayer = false;
        if (null != currentBubble)
            currentBubble.Release();
        if (null != angeryBubble)
        {
            angeryBubble.Release();
            angeryBubble = null;
        }

        StopAllCoroutines();
        
        fx.PlayEvacFx(transform.position, goPathTween.PathGetPoint(0));

        goPathTween.Restart();
    }

    public void HandleBoost()
    {
        SingletonUtils<SoundManager>.Instance.PlaySound(Random.value < .5f ? "boost1" : "boost2");
        SingletonUtils<SoundManager>.Instance.PlaySound("boost_addon");
        if (isStoppedByPlayer)
            ToggleEngine(true);
        boostTimer = map.boostDuration;
        carState = CarStates.Boosted;
    }

    IEnumerator waitingCor()
    {
        yield return new WaitForSeconds(2f);
        ui.Performance -= isVIP ? map.performanceDropVIP : map.performanceDropPerTick;
        SingletonUtils<SessionManager>.Instance.annoyed++;
        yield return new WaitForSeconds(.5f);
    }

    public void Go(List<Tile> path, VehicleSpawner owner, bool isVIP = false)
    {
        fx.AttachCarDeco(transform);
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
        goPathTween.timeScale = 5f * Random.value;
        carState = CarStates.Accelerating;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isExploding)
            return;

        var otherCar = other.GetComponent<VehicleController>();
        if (null != otherCar)
        {
            Explode();
            otherCar.Explode(true);
            SingletonUtils<SoundManager>.Instance.Play3D(Random.value < .5f ? "explosion1" : "explosion2", transform.position);
        }
    }

    private bool isExploding;
    public void Explode(bool other = false)
    {
        if (isExploding)
            return;

        SingletonUtils<SessionManager>.Instance.dead++;

        StopAllCoroutines();
        if (null != currentBubble)
        {
            currentBubble.Release();
            currentBubble = null;
        }

        if (null != angeryBubble)
        {
            angeryBubble.Release();
            angeryBubble = null;
        }

        ui.Performance -= isVIP ? map.performanceDropPerCrashVIP : map.performanceDropPerCrash;

        isExploding = true;
        goPathTween.Kill();
        transform.DOScale(1.2f, .05f).OnComplete(() =>
        {
            SingletonUtils<FxController>.Instance.PlayExplosion(transform.position);
            Despawn();
        });
    }
    
    public void Despawn(bool success = false)
    {
        if (owner)
            owner.RemoveMe(this);

        if (success)
        {
            SingletonUtils<SessionManager>.Instance.arrived++;
            SingletonUtils<SoundManager>.Instance.Play3D("arrived", transform.position);
            carState = CarStates.Stopped;
            if (null != angeryBubble)
                angeryBubble.Release();
            StopAllCoroutines();
            isAngery = false;
            
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
            if (null != angeryBubble)
                angeryBubble.Release();
            Destroy(gameObject);
        }
        
        StopAllCoroutines();
        goPathTween.Kill();
        goPathTween = null;
    }

    private string RandomHappy()
    {
        if (Random.value < .333f)
            return "happy1";
        if (Random.value < .6666f)
            return "happy2";
        return "happy3";
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward * (map.probeLen - .2f), .25f);
    }


    public bool verbose;

    public bool isAngery;
    public float angeryCDTimer;

    public float boostTimer = 0f;

    private void Update()
    {
        if (!isStoppedByPlayer && carState != CarStates.Stopped)
        {
            var probeRay = new Ray(transform.position, transform.forward);
            //if (Physics.SphereCast(probeRay, .1f, out var rHit, map.probeLen, map.probeMask, QueryTriggerInteraction.Collide))
            // ReSharper disable once Unity.PreferNonAllocApi
            var obstacles = Physics.OverlapSphere(transform.position + transform.forward * (map.probeLen - .2f), .25f,
                map.probeMask, QueryTriggerInteraction.Collide);
            if (obstacles.Length > 0 && obstacles.Any(o => o.gameObject.GetInstanceID() != gameObject.GetInstanceID()))
            {
            //if (Physics.SphereCast(probeRay, .25f, out var rHit, map.probeLen - .2f, map.probeMask,QueryTriggerInteraction.Collide))
            //var otherCar = rHit.collider.GetComponent<VehicleController>();
                var otherCar = obstacles.First(o => o.gameObject.GetInstanceID() != gameObject.GetInstanceID()).GetComponent<VehicleController>();

                var relSpd = (moveSpd * accelFactor) / (otherCar.moveSpd * Mathf.Max(otherCar.accelFactor, 0.01f));

                if (carState != CarStates.Boosted)
                    if (relSpd > 1f)
                        carState = CarStates.Decelerating;

                isAngery = true;
            }
            else if (carState != CarStates.Boosted)
            {
                if (!isStoppedByPlayer)
                    carState = CarStates.Accelerating;
            }
        }

        switch (carState)
        {
            case CarStates.Accelerating:
                accelFactor += acceleration * Time.deltaTime;
                if (!isStoppedByPlayer && accelFactor >= 1f)
                    carState = CarStates.Going;
                break;
            case CarStates.Decelerating:
                accelFactor -= acceleration * Time.deltaTime;
                if (isStoppedByPlayer && accelFactor <= 0f)
                    carState = CarStates.Stopped;
                break;
            case CarStates.Boosted:
                boostTimer -= Time.deltaTime;
                if (boostTimer > map.boostDuration * .75f)
                {
                    accelFactor += acceleration * map.boostAccelFactor * Time.deltaTime;
                    accelFactor = Mathf.Min(accelFactor, map.boostSpeedMul);
                }
                else if (boostTimer > 0f && boostTimer <= map.boostDuration * .25f)
                {
                    accelFactor -= acceleration * map.boostAccelFactor * Time.deltaTime;
                    accelFactor = Mathf.Max(accelFactor, 1f);
                }
                else if (boostTimer <= 0f)
                {
                    carState = CarStates.Going;
                }

                break;
            case CarStates.Going:
                accelFactor = 1f;
                break;
            case CarStates.Stopped:
                accelFactor = 0f;
                break;
        }

        if (carState != CarStates.Boosted)
            accelFactor = Mathf.Clamp01(accelFactor);
        if (null != goPathTween)
            goPathTween.timeScale = accelFactor;


        angeryCDTimer = Mathf.Max(angeryCDTimer - Time.deltaTime, -1f);

        if (isAngery && angeryCDTimer <= 0f && null == angeryBubble)
        {
            StartCoroutine(angeryCor());
            angeryCDTimer = map.angeryCooldown;
        }

        isAngery = false;
    }

    private SpeechBubble angeryBubble;

    IEnumerator angeryCor()
    {
        angeryBubble = fx.GetSpeechBubble();
        angeryBubble.PlayClip( transform.position + Vector3.up * .5f, "angery");
        angeryBubble.FollowMe(transform, Vector3.up * .5f, true, .35f);
        ui.Performance -= map.angeryPerformanceLossPerTick;
        SingletonUtils<SessionManager>.Instance.annoyed++;

        var honkKey = "honk" + Random.Range(1, 5).ToString();
        SingletonUtils<SoundManager>.Instance.Play3D(honkKey, transform.position);

        yield return new WaitForSeconds(2f);
        angeryBubble.Release();
        angeryBubble = null;
    }
}
