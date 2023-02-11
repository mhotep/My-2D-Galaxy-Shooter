using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField]
    private GameObject _enemyPrefab;

    private IEnumerator coroutine;
    private IEnumerator powerUpCoroutine;
    [SerializeField]
    private GameObject _container;
    [SerializeField]
    private GameObject[] powerups;
    private bool _stopSpawning = false;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {

        //yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false) 
        {
            var xPosition = Random.Range(-10.0f, 10.0f);
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(xPosition, 8.0f, 0), Quaternion.identity);
            newEnemy.transform.parent = _container.transform;
            yield return new WaitForSeconds(5);
        }

    }

    IEnumerator SpawnPowerupRoutine()
    {
        //yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            var spawnTime = Random.Range(3, 8);
            var xPosition = Random.Range(-10.0f, 10.0f);
            int randomPowerup = Random.Range(0, 3);
            GameObject newPowerup = Instantiate(powerups[randomPowerup], new Vector3(xPosition, 8.0f,0), Quaternion.identity);
            yield return new WaitForSeconds(spawnTime);
        }
    }

    public void OnPlayerDeath()
    {

        _stopSpawning = true;
    }

}
