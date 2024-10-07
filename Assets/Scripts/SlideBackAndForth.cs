using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideBackAndForth : MonoBehaviour
{
    [SerializeField] private Vector3 _moveAmount;
    [SerializeField] private float _speed = 1;
    
    private Vector3 _startPos;
    private float _timePassed = 0f;

    private void Start()
    {
        _startPos = transform.position;
    }

    private void Update()
    {
        _timePassed += Time.deltaTime * _speed;

        Vector3 newPos;

        newPos.x = Mathf.Lerp(_startPos.x + _moveAmount.x, _startPos.x - _moveAmount.x, (Mathf.Sin(_timePassed)/2) + 0.5f);
        newPos.y = Mathf.Lerp(_startPos.y + _moveAmount.y, _startPos.y - _moveAmount.y, (Mathf.Sin(_timePassed)/2) + 0.5f);
        newPos.z = Mathf.Lerp(_startPos.z + _moveAmount.z, _startPos.z - _moveAmount.z, (Mathf.Sin(_timePassed)/2) + 0.5f);

        transform.position = newPos;
    }
}
