using System.Collections;
using DG.Tweening;
using UnityEngine;

public class SpaceshipSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _shipsToSpawn;

    [SerializeField] private Vector2 _relativeStartEnd = new Vector2(-100, 100);

    [Range(0, 100)]
    [SerializeField] private float _spawnChance;

    [SerializeField] private float _spawnFrequency;

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
        var shipToSpawn = _shipsToSpawn[Random.Range(0, _shipsToSpawn.Length)];
        var spawnedShip = Instantiate(shipToSpawn, transform);
        spawnedShip.transform.position = new Vector3(spawnedShip.transform.position.x, spawnedShip.transform.position.y + 10, spawnedShip.transform.position.z + _relativeStartEnd.x);
        spawnedShip.transform.DOMoveZ(_relativeStartEnd.y, 60).SetSpeedBased().SetEase(Ease.Linear).SetAutoKill()
            .OnComplete(() => StartCoroutine(DestroyDelayed(spawnedShip)));
    }

    private IEnumerator DestroyDelayed(GameObject target)
    {
        yield return new WaitForSeconds(0.1f);
        target.GetComponent<Thrust2D>().currentSequence.Kill();
        Destroy(target);
        // ReSharper disable once IteratorNeverReturns
    }
}
