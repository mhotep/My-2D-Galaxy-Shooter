using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SpawnManager : MonoBehaviour
{
    public int[] powerupTable =
    {
        55, //Triple shot powerup weight
        50, //Speed powerup weight
        45, //Shield powerup weight
        40, //Ammo powerup weight
        35, //Health powerup weight
        20  //Neutrino Bomb powerup weight
    };

    public int[] enemyTable =
    {
        10, // enemy flying straight weight
        2  // enemy who fluctuates weight
    };

    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject _container;
    [SerializeField]
    private GameObject[] powerups;
    private bool _stopSpawning = false;
    [SerializeField]
    private GameObject[] _enemies;

    public int powerupTotal;
    public int enemyTotal;
    public int randomPowerup;
    public int randomEnemy;

    public void Start()
    {

        foreach (var item in powerupTable) 
        {
            powerupTotal += item;
        }

        foreach (var item in enemyTable)
        {
            enemyTotal += item;
        }

    }

    public void StartSpawning()
    {
        StartCoroutine(Camera.main.GetComponent<CameraShakeMH>().ShakeCam(2f, 0.15f));
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {

        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false) 
        {


            for (int i = 0; i < _enemies.Length; i++) 
            {
                randomEnemy = Random.Range(0, enemyTotal);
                if (randomEnemy <= enemyTable[i])
                {
                    var xPosition = Random.Range(-10.0f, 10.0f);
                    GameObject newEnemy = Instantiate(_enemies[i], new Vector3(xPosition, 8.0f, 0), Quaternion.identity);
                    newEnemy.transform.parent = _container.transform;
                    yield return new WaitForSeconds(5);
                }
                else
                {
                    randomEnemy -= enemyTable[i];
                }
            }
        }

    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            for (int i = 0; i < powerupTable.Length; i++)
            {
                randomPowerup = Random.Range(0, powerupTotal);
                if (randomPowerup <= powerupTable[i])
                {
                    var spawnTime = Random.Range(3, 8);
                    var xPosition = Random.Range(-10.0f, 10.0f);
                    Instantiate(powerups[i], new Vector3(xPosition, 8.0f, 0), Quaternion.identity);
                    yield return new WaitForSeconds(spawnTime);
                }
                else
                {
                    randomPowerup -= powerupTable[i];
                }
            }
        }
    }

    public void OnPlayerDeath()
    {

        _stopSpawning = true;
    }
}
