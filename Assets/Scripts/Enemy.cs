using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    [SerializeField]
    private float _speed = 4.0f;
    private float randomX;

    private UIManager _uimanager;
    private Player _player;
    // get handle to the animator component
    private Animator _explosionAnim;
    private AudioSource _audioSource;

    [SerializeField]
    private GameObject _laserPrefab;
    private float _canfire = -1;
    private float _fireRate= 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        //assign the animator component to anim
        _explosionAnim = GetComponent<Animator>();
        _audioSource  = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (Time.time > _canfire && _speed > 0)
        {
            _fireRate = Random.Range(3f, 7f);
            _canfire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab,transform.position,Quaternion.identity);
            Laser[] lasers= enemyLaser.GetComponentsInChildren<Laser>();
            
            for (int i =0; i<lasers.Length; i++) 
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }
    
    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -5f)
        {
            randomX = Random.Range(-10.0f, 10.0f);
            transform.position = new Vector3(randomX, 11.0f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player" && _speed > 0)
        {
            Player _player = other.transform.GetComponent<Player>();
            if (_player != null) 
            {
                StartCoroutine(Camera.main.GetComponent<CameraShakeMH>().ShakeCam(.25f, 0.15f));
                _player.Damage();
                //trigger anim
                Explosion();
            }
            
        }

        if (other.tag == "Laser" | other.tag == "NeutrinoBomb")
        {
            Collider2D enemyCollider = GetComponent<Collider2D>();
            if (enemyCollider != null)
            {
                Destroy(enemyCollider);
            }
            else
            {
                Debug.Log(" enemy collider is null");
            }
            StartCoroutine(Camera.main.GetComponent<CameraShakeMH>().ShakeCam(.25f, 0.15f));
            Destroy(other.gameObject);
                
            //update UI with new score
            _uimanager = GameObject.Find("Canvas").GetComponent<UIManager>();
            if (_uimanager != null)
            {
                _uimanager.Updatescore();
            }
            Explosion();
        }
    }

    void Explosion()

    {
        _explosionAnim.SetTrigger("OnEnemyDeath");
        _speed = 0;
        _audioSource.Play();

        Destroy(this.gameObject, 2.8f);

    }


}
