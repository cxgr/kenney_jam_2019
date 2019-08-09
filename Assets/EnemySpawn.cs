using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    public class EnemySpawn : MonoBehaviour
    {
        [SerializeField] private List<Transform> _spawnPositions;
        [SerializeField] private List<GameObject> _gameObjectsToSpawn;
        [SerializeField] private Vector3 _spawnOffset;
        [SerializeField] private float spawnTime = 4f;

        // ReSharper disable once UnusedMember.Local
        void Start()
        {
            StartCoroutine(SpawnByTimer());
        }

        // Update is called once per frame
        private IEnumerator SpawnByTimer()
        {
            do
            {
                yield return new WaitForSeconds(spawnTime);

                var objectToSpawn = _gameObjectsToSpawn[Random.Range(0, _gameObjectsToSpawn.Count)];
                var positionToSpawn = _spawnPositions[Random.Range(0, _spawnPositions.Count)];
                var newGo = Instantiate(objectToSpawn, positionToSpawn);

                Debug.Log(newGo.name + " spawned at " + newGo.transform.position);
            
            } while (useGUILayout);
        }
    }
}
