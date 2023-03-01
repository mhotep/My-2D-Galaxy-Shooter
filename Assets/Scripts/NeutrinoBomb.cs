using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutrinoBomb : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private GameObject _neutrinosPrefab;

    private int _numberOfObjects = 6;
    private float _radius = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            SpewNeutrinos();
        }
    }

    void SpewNeutrinos()
    {
        for (int i = 0; i < _numberOfObjects; i++)
        {
            float angle = Mathf.PI * 2 / 360f;
            float xPosition = Mathf.Cos(angle) * _radius;
            float yPosition = Mathf.Sin(angle) * _radius;
            Vector3 pos = new Vector3(xPosition, yPosition, 0);
            Instantiate(_neutrinosPrefab, transform.position, Quaternion.Euler(0, 0, 0));
        }
    }
}
