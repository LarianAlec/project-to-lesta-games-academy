using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTorqueExample : MonoBehaviour
{
    [SerializeField] Vector3 _direction;
    [SerializeField] float _speed = 50f;
    private Rigidbody _rb;


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {

        _rb.AddTorque(_direction * _speed, ForceMode.Force);

    }
}
