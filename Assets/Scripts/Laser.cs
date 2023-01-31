using GameDevHQ.Filebase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8f;

    // Update is called once per frame
    void Update()
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
    }
}
