using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        //assign the animator component to anim
        _explosionAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
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
        if (other.tag == "Player")
        {
            Player _player = other.transform.GetComponent<Player>();
            if (_player != null) 
            {
                _player.Damage();
                //trigger anim
                _explosionAnim.SetTrigger("OnEnemyDeath");
                _speed = 0;
                Destroy(this.gameObject, 2.8f);
            }
            
        }

        if (other.tag == "Laser")
        {

            Destroy(other.gameObject);
                
            //update UI with new score
            _uimanager = GameObject.Find("Canvas").GetComponent<UIManager>();
            if (_uimanager != null)
            {
                _uimanager.Updatescore();
            }
            //trigger anim
            _explosionAnim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            Destroy(this.gameObject, 2.8f);
            
        }
    }

}
