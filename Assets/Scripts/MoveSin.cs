using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSin : MonoBehaviour
{
    private float _sinCenterX;
    private float _xAmplitude;
    private float _xFrequency;
    private float _sinMove;
    private Vector2 _pos;

    // Start is called before the first frame update
    void Start()
    {
        _sinCenterX = transform.position.x;
        _xAmplitude = Random.Range(-1f,2f);
        _xFrequency = Random.Range(-1f,2f);

    }

    private void FixedUpdate()
    {   _pos = transform.position;
        _sinMove = Mathf.Sin(_pos.y * _xFrequency) * _xAmplitude; 
        _pos.x = _sinCenterX + _sinMove;

        transform.position = _pos;   
    }
}
