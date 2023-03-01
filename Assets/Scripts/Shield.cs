using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private int _shieldLives = 3;
    private Animator _shieldAnim;

    // Start is called before the first frame update
    void Start()
    {
        _shieldAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy" | other.tag == "Laser")
        {
            switch (_shieldLives)
            {
                case 3:
                    //change shield aniomation to indicate 1 hit
                    _shieldAnim.SetTrigger("On1Hit");
                    _shieldLives -= 1;
                    break;
                case 2:
                    //change shield animation to indicate 2 hits
                    _shieldAnim.SetTrigger("On2Hits");
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

        }

    }

    IEnumerator ShieldPowerDownRoutine()
    {
        yield return new WaitForSecondsRealtime(3);
        _shieldLives = 3;
        this.gameObject.SetActive(false);
    }
}


