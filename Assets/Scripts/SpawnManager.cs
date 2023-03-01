using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SpawnManager : MonoBehaviour
{
    public int[] table =
    {
        55, //Triple shot powerup weight
        50, //Speed powerup weight
        45, //Shield powerup weight
        40, //Ammo powerup weight
        35, //Health powerup weight
        20  //Neutrino Bomb powerup weight
    };

    [SerializeField]
    private GameObject _enemyPrefab;

    private IEnumerator coroutine;
    private IEnumerator powerUpCoroutine;
    [SerializeField]
    private GameObject _container;
    [SerializeField]
    private GameObject[] powerups;
    private bool _stopSpawning = false;

    public int total;
    public int randomNumber;
    public void Start()
    {
        //tally the total weight
        //draw a number between zero and the total weight (100).
        //calculate the total

        foreach (var item in table) 
        {
            total += item;
        }

        //randomNumber = Random.Range(0, total);
        //for (int i=0; i < table.Length; i++)
        //{
        //    if (randomNumber <= table[i])
        //    {

        //    }
        //}
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        //StartCoroutine(RareSpawnRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {

        yield return new WaitForSeconds(3.0f);

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
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {



            for (int i = 0; i < table.Length; i++)
            {
                randomNumber = Random.Range(0, total);
               // int randomPowerup = Random.Range(0, 5);
                if (randomNumber <= table[i])
                {
                    var spawnTime = Random.Range(3, 8);
                    var xPosition = Random.Range(-10.0f, 10.0f);
                    Instantiate(powerups[i], new Vector3(xPosition, 8.0f, 0), Quaternion.identity);
                    yield return new WaitForSeconds(spawnTime);
                }
                else
                {
                    randomNumber -= table[i];
                }
            }
        }
    }

    public void OnPlayerDeath()
    {

        _stopSpawning = true;
    }

    //IEnumerator RareSpawnRoutine()
    //{
    //    yield return new WaitForSeconds(Random.Range(15, 30));

    //    var rarespawnTime = Random.Range(9,20); //spawn time ranges from 9 to 20 secs
    //    var rareXPosition = Random.Range(-10.0f, 10.0f); //xposition for spawning of rare powerup
    //    var rarePowerup = Random.Range(5, 5); //range of rarepowerups -- for now just the Nutrino Bomb...more to come
    //    Instantiate(powerups[rarePowerup], new Vector3(rareXPosition, 8.0f, 0), Quaternion.identity);
    //    yield return new WaitForSeconds(rarespawnTime);
    //}
}
