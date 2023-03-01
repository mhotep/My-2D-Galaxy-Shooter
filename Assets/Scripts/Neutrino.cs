using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neutrino : MonoBehaviour
{
    float randomX, randomY;

    private void Start()
    {
        randomX= Random.Range(-5,5);
        randomY= Random.Range(-5,5);
    }

    // Update is called once per frame
    void Update()
    {
        // mpve object to a position

        transform.Translate(new Vector3(randomX,randomY,0) * 1.5f * Time.deltaTime);

        if (transform.position.x > 10 | transform.position.y < -10 | transform.position.y > 10 | transform.position.y < -10)
        {
            Destroy(this.gameObject);
        }
    }
}
