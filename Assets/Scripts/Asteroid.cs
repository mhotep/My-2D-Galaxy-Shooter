using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 20f;
    [SerializeField]
    private GameObject _Explosion;
    private SpawnManager _spawnManager;



    // Start is called before the first frame update
    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("The SpawnManager is Null");
        }



    }
    // Update is called once per frame
    void Update()
    {
        //rotate asteroid on the zed axis
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);

    }

    // check for laser collision
    // instantiate explosion at the position of the asteroid
    // destroy the explosion after 3 seconds
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Instantiate(_Explosion, transform.position, Quaternion.identity);

            Destroy(other.gameObject);
            _spawnManager.StartSpawning();
            Destroy(this.gameObject);
        }
    }
}
