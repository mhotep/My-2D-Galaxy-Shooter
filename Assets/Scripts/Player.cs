using System.Collections;
using System.Collections.Generic;
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
    private float _fireRate = 0.5f;
    private float _canFire = -.1f;

    [SerializeField]
    private int _lives = 3;

    private bool _isTripleShotActive = false;

    [SerializeField]
    private float _thrusters = 1.0f;
    public IEnumerator _coroutine;

    //variable reference to the shield visualizer
    private GameObject _shieldActive;
    //variable keeping count of shield lives.
    private int _shieldLives;
//    private bool _isShieldActive;

    //private animation handle
    private Animator _shieldAnim;

    private UIManager _uiManager;
    private SpawnManager _spawnManager;

    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private int _score;

    // int to keep count of ammo
    private int _ammo;

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

        _shieldActive = this.gameObject.transform.GetChild(1).gameObject;
        _shieldAnim = _shieldActive.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && _speed == 3.5f)
        {
            _thrusters = 3.0f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _thrusters= 1.0f;
        }

        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _ammo > 0)
        {
            FireLaser();
        };
    }

    void CalculateMovement()
    {

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalkInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput,verticalkInput,0);

        transform.Translate(direction  * _speed * _thrusters * Time.deltaTime);

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

        _audioSource.Play(0);
        _ammo -= 1;
    }

    public void Damage()
    {
        //change shield animation indicating damage
        if (_shieldActive.activeSelf)
        {
                switch (_shieldLives)
                {
                    case 3:
                        //change shield aniomation to indicate 1 hit
                        _shieldAnim.SetTrigger("On1Hit");
                        _shieldLives -= 1;
                        //StartCoroutine(ShieldPowerDownRoutine());
                        break;
                    case 2:
                        //change shield animation to indicate 2 hits
                        _shieldAnim.SetTrigger("On2Hits");
                        //StartCoroutine(ShieldPowerDownRoutine());
                        _shieldLives -= 1;
                        break;
                    case 1:
                        //change shield animatIon to indicate 3 hits
                        _shieldAnim.SetTrigger("On3Hits");
                        _shieldLives -= 1;
                        StartCoroutine(ShieldPowerDownRoutine());
                    break;
                    default:
                        break;
                }
                return;
        }

        _lives -= 1;

        if (_lives == 2) //if lives equals 2 enable right engine
        {
            this.gameObject.transform.GetChild(2).gameObject.SetActive(true);
        }
        else if (_lives == 1) //if lives equals 1 enable left engine
        {
            this.gameObject.transform.GetChild(3) .gameObject.SetActive(true);
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
        _shieldLives= 3;
        _shieldActive.SetActive(true);
   //     _isShieldActive = true;
    }

    public string GetScore(int points)
    {
        _score += points;
        return _score.ToString();
    }

    IEnumerator ShieldPowerDownRoutine()
    {
        yield return new WaitForSecondsRealtime(3);
        _shieldActive.SetActive(false);
    }

    //return count of ammo remaining to UIManager
    public string GetAmmo(int ammo)
    {
        _ammo += ammo;
        return _ammo.ToString();

    }
}
