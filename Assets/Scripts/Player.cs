using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{

    //declare a SErialized field private speed variable with a type of float
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 3.0f;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private GameObject _triplePrefab;

    [SerializeField] 
    private GameObject _neutrinoPrefab;

    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -.1f;

    [SerializeField]
    private int _lives = 3;

    private bool _isTripleShotActive = false;

    private bool _isNeutrinoActive = false;

    private float _thrusters = 1.0f;

    public IEnumerator _coroutine;

    //variable reference to the shield
    [SerializeField]
    private Shield _shield;

    private UIManager _uiManager;
    private SpawnManager _spawnManager;

    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private int _score;

    // int to keep count of ammo
    private int _ammo = 16;

    [SerializeField]
    private float _fuelPercentage;

    [SerializeField]
    private GameObject _thruster;
    [SerializeField]
    private GameObject _thrusterBoosted;

    private float _canThrust;

    private float _thrustRate = 0.05f;

    private bool _reFueling = false;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("The SpawnManager is Null");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_uiManager == null)
        {
            Debug.LogError("The UIManager is Null");
        }

        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("The AudioSource is null");
        }

        //this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _ammo > 0)
        {
            FireLaser();
        };
    }

    void CalculateMovement()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Time.time > _canThrust && Mathf.RoundToInt(_fuelPercentage) > 0 && _reFueling == false)
        { 
            _canThrust = Time.time + _thrustRate;
            StopCoroutine(ActivateReFuel());
            ActivateThruster();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {

            _thrusterBoosted.SetActive(false);
            _thruster.SetActive(true);
            _thrusters = 1.0f;
            //increment thruster bar
            _uiManager.UpdateThruster(_fuelPercentage);
        }
        else
        {
            if (_fuelPercentage < 1)
            {
                StartCoroutine(ActivateReFuel());
                _thrusterBoosted.SetActive(false);
                _thruster.SetActive(true);
            }
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalkInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput,verticalkInput,0);

        transform.Translate(_speed * _thrusters * Time.deltaTime * direction);

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
        else if (_isNeutrinoActive)
        {
            Instantiate(_neutrinoPrefab, new Vector3(transform.position.x, transform.position.y * 1.05f,0),Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, new Vector3(transform.position.x, transform.position.y + 1.05f, 0), Quaternion.identity);
        }

        _audioSource.Play(0);
        _ammo -= 1;
        _uiManager.UpdateAmmo(_ammo);
        
    }

    public void Damage()
    {
        //change shield animation indicating damage
        if (_shield.gameObject.activeSelf)
        {
            return;
        }

        _lives -= 1;

        StartCoroutine(Camera.main.GetComponent<CameraShakeMH>().ShakeCam(.5f, 0.15f));

        if (_lives == 2) //if lives equals 2 enable right engine
        {
            this.gameObject.transform.GetChild(2).gameObject.SetActive(true);
        }
        else if (_lives == 1) //if lives equals 1 enable left engine
        {
            this.gameObject.transform.GetChild(3).gameObject.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            _uiManager.GameOver();
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
        if (_thrusters > 1.0f | _speed > 3.5f)
        {
            return;
        }

        _speed *= _speedMultiplier;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speed = 3.5f;
    }

    public void ShieldsActive()
    {
        _shield.gameObject.SetActive(true);
    }

    public string GetScore(int points)
    {
        _score += points;
        return _score.ToString();
    }

    //replenish our ammo
    public void GetAmmo()
    {
        if (_ammo < 15)
        {
            _ammo = 15;
            _uiManager.UpdateAmmo(_ammo);
        }
    }

    //increase life
    public void UpdateLife()
    {
        if (_lives < 3)
        {
            
            switch (_lives)
            {
                case 1:
                    this.gameObject.transform.GetChild(3).gameObject.SetActive(false);
                    break;
                case 2:
                    this.gameObject.transform.GetChild(2).gameObject.SetActive(false);
                    break;
                default:
                    break;
            }

            _lives++;
            _uiManager.UpdateLives(_lives);
        }

    }

    public void NeutrinoActive()
    {
        //activate the Neutrino
        _isNeutrinoActive = true;
        _coroutine = NeutrinoPowerDownroutine();
        StartCoroutine(_coroutine);
    }

    IEnumerator NeutrinoPowerDownroutine()
    {
        yield return new WaitForSeconds(5);
        _isNeutrinoActive = false;
    }

    public void ActivateThruster()
    {
            if (Mathf.RoundToInt(_fuelPercentage) > 1 || _fuelPercentage < 100f)
            {
                _thrusters = 6.0f;
                _thruster.SetActive(false);
                _thrusterBoosted.SetActive(true);
                _fuelPercentage -= 15f * 5f * Time.deltaTime;
            }
            else
            {
                _thrusters = 1.0f;
                _thruster.SetActive(false);
                _thrusterBoosted.SetActive(true);
                _fuelPercentage = 0;
            }

            _uiManager.UpdateThruster(_fuelPercentage);
    }

    IEnumerator ActivateReFuel()
    {
        while (_fuelPercentage < 100)
        {
            _reFueling = true;
            _thrusters = 1.0f;
            yield return new WaitForSeconds(.01f);
            _fuelPercentage += 7.5f * Time.deltaTime;
            _uiManager.UpdateThruster(_fuelPercentage);

            if (_fuelPercentage >= 100f)
            {
                _fuelPercentage = 100f;
                _uiManager.UpdateThruster(_fuelPercentage);
                break;
            }
        }
        _reFueling = false;
    }
}
