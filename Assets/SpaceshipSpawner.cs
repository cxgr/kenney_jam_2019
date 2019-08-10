using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpaceshipSpawner : MonoBehaviour
{
    [SerializeField] private SpaceShipItem[] shipsToSpawn;
    [SerializeField] private Vector2 _relativeStartEnd = new Vector2(-100, 100);

    [Range(0, 100)]
    [SerializeField] private float _spawnChance = .5f;

    [SerializeField] private float _spawnFrequency = 3f;

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(Tick());
    }

    private IEnumerator Tick()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnFrequency);

            if (Random.value > _spawnChance / 100f)
                Spawn();
        }
        // ReSharper disable once IteratorNeverReturns
    }

    // Update is called once per frame
    private void Spawn()
    {
        var shipToSpawn = shipsToSpawn[Random.Range(0, shipsToSpawn.Length)];
        var spawnedShip = Instantiate(shipToSpawn.spaceShipGameObject, transform);

        var t = spawnedShip.transform;
        var pos = t.position;
        pos.y = Random.Range(shipToSpawn._heightMinMax.x, shipToSpawn._heightMinMax.y);
        t.position = pos;
        spawnedShip.transform.rotation = Quaternion.Euler(360f * Random.value * Vector3.up);

        var from = t.position + t.forward * _relativeStartEnd.x;
        var to = t.position + t.forward * _relativeStartEnd.y;

        t.position = from;
        t.LookAt(to, Vector3.up);
        
        var rndSpd = Random.Range(shipToSpawn.SpeedMinMaxVector2.x, shipToSpawn.SpeedMinMaxVector2.y);
        Debug.Log(from);
        Debug.Log(to);
        var duration = Vector3.Distance(from, to) / rndSpd;
        Debug.Log(duration);
        spawnedShip.transform.DOMove(to, duration).SetEase(Ease.Linear)
            .OnComplete(() => Destroy(spawnedShip.gameObject));
    }

    private IEnumerator DestroyDelayed(GameObject target)
    {
        yield return new WaitForSeconds(0.1f);
        //target.GetComponent<Thrust2D>().currentSequence.Kill();
        Destroy(target);
        // ReSharper disable once IteratorNeverReturns
    }

    [Serializable]
    internal class SpaceShipItem
    {
        [SerializeField]
        internal GameObject spaceShipGameObject;
        [SerializeField]
        internal Vector2 SpeedMinMaxVector2;

        [SerializeField] internal Vector2 _heightMinMax;
    }
}
