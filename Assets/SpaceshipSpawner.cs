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

        spawnedShip.transform.position = spawnedShip.transform.forward * _relativeStartEnd.x;

        spawnedShip.transform.DOLocalMoveZ(_relativeStartEnd.x, 0);

        spawnedShip.transform.position = new Vector3(spawnedShip.transform.position.x, Random.Range(shipToSpawn._heightMinMax.x, shipToSpawn._heightMinMax.y), spawnedShip.transform.position.z);
        spawnedShip.transform.DOLocalMoveZ(_relativeStartEnd.y, Random.Range(shipToSpawn.SpeedMinMaxVector2.x, shipToSpawn.SpeedMinMaxVector2.y)).SetSpeedBased().SetEase(Ease.Linear).SetAutoKill()
            .OnComplete(() => StartCoroutine(DestroyDelayed(spawnedShip)));
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
