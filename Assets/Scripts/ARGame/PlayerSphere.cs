using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSphere : MonoBehaviour
{
    [SerializeField]
    private PlayerCircle _playerCircle;

    [SerializeField]
    private float _maxDistance;

    [SerializeField]
    private float _followSpeed;

    [SerializeField]
    private float _returnSpeed;

    private Vector3 _offset;
    private Vector3 _originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        _offset = _playerCircle.transform.localPosition - transform.localPosition;
        _originalPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //keep distance to player circle under max distance
        Vector3 targetPosition = transform.localPosition + _offset;
        if (_playerCircle.transform.localPosition != targetPosition)
        {
            targetPosition.z = 0;
            _playerCircle.transform.localPosition = Vector3.Lerp(_playerCircle.transform.localPosition, targetPosition, Time.deltaTime * _followSpeed);
        }

        if(transform.localPosition.z != _originalPosition.z)
        {
            Vector3 newPosition = transform.localPosition;
            newPosition.z = _originalPosition.z;
            transform.localPosition = Vector3.Lerp(transform.localPosition, newPosition, Time.deltaTime * _returnSpeed);
        }
    }


}
