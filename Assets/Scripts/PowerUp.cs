using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private float _randomX;
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField] //Id for powerups 0 = Triple Shot 1 = speed 2 = shields
    private int powerupID;
    [SerializeField]
    private AudioClip _clip;

    // Update is called once per frame
    void Update()
    {

        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -4.5f)
        {
            Destroy(this.gameObject);
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_clip, transform.position, 10000F);
            if (player != null)
            {
                switch (powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        //play speed powerup
                        player.SpeedActive();
                        break;
                    case 2:
                        player.ShieldsActive();
                        break;
                    case 3:
                        player.GetAmmo();
                        break;
                    default:
                        break;
                }


                Destroy(this.gameObject);
            }

        }

    }

}
