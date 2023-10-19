using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EndlessRotatingComponent : MonoBehaviour
{
    [Header("Rotation params")]
    [SerializeField]
    private Vector3 _rotationDirection;
    [SerializeField]
    Vector3 _pointPosition; // allows rotate around point in world space
    [SerializeField]
    float _rotationSpeed = 10f;

    //private bool _enabled = true;
    private Rigidbody _rb;

    private void Start()
    {
        _pointPosition = transform.position;
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true;
    }

    private void Update()
    {
        transform.RotateAround(_pointPosition, _rotationDirection, _rotationSpeed * Time.deltaTime);
    }
}
