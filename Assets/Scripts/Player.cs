using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //declare a SErialized field private speed variable with a type of float
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2.0f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _triplePrefab;

    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -.1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnmanager;
    private UIManager _uiManager;

    private bool _isTripleShotActive = false;
    private bool _isShieldActive = false;
    public IEnumerator _coroutine;

    //variable reference to the shield visualizer
    private GameObject _shieldActive;

    [SerializeField]
    private int _score;

    // Start is called before the first frame update
    void Start()
    {

        _spawnmanager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnmanager == null)
        {
            Debug.LogError("The SpawnManager is Null");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_uiManager == null)
        {
            Debug.LogError("The UIManager is Null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        };
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalkInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput,verticalkInput,0);

        transform.Translate(direction  * _speed * Time.deltaTime);

        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        if (transform.position.x > 11)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }
        else if (transform.position.x < -11)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        
        if (_isTripleShotActive)
        {
            Instantiate(_triplePrefab, new Vector3(transform.position.x, transform.position.y + 1.05f, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, new Vector3(transform.position.x, transform.position.y + 1.05f, 0), Quaternion.identity);
        }
    }

    public void Damage()
    {

        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shieldActive = this.gameObject.transform.GetChild(0).gameObject;
            _shieldActive.SetActive(false);
            return;
        }

        _lives -= 1;

        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _spawnmanager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
            
    }

    public void TripleShotActive()
    { 
        _isTripleShotActive = true;
        _coroutine = TripleShotPowerDownRoutine();
        StartCoroutine(_coroutine);
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedActive()
    {
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speed /= _speedMultiplier;
    }

    public void ShieldsActive()
    { 
        _isShieldActive = true;
        _shieldActive = this.gameObject.transform.GetChild(0).gameObject;
        _shieldActive.SetActive(true);
    }

    public string GetScore(int points)
    {
        _score += points;
        return _score.ToString(); 
    }
}
