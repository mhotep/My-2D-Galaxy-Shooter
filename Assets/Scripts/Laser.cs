using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8f;
    private bool _isEnemyLaser = false;

    // Update is called once per frame
    void Update()
    {
        if (_isEnemyLaser == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }


    }

    private void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y > 8f)
        {

            if (this.transform.parent)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);

        }
        else if (transform.position.y < -8f)
        {
            Destroy(this.gameObject);
        }
    }

    private void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -8f)
        {

            if (this.transform.parent)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);

        }
        else if (transform.position.y < -8f)
        {
            Destroy(this.gameObject);
        }
    }
    
    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (_isEnemyLaser == true && other.tag == "Player") 
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
        }
    }

}
