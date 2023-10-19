using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FloatPlatform : MonoBehaviour
{
	[SerializeField] float _distance = 5f; //Distance that moves the object
	[SerializeField] bool _horizontal = true; // movement is horizontal or vertical
	[SerializeField] float _speed = 3f;
	[SerializeField] float _offset = 0f; // modify position at the start 

	private bool isForward = true; //If the movement is out
	private Vector3 _startPos;
	private Rigidbody _rb;
   
    void Awake()
    {
		_startPos = transform.position;
		if (_horizontal)
			transform.position += Vector3.right * _offset;
		else
			transform.position += Vector3.forward * _offset;
	}

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
		if (_horizontal)
		{
			if (isForward)
			{
				if (_rb.position.x < _startPos.x + _distance)
				{
					_rb.MovePosition(_rb.position + Vector3.right * Time.deltaTime * _speed);
				}
				else
					isForward = false;
			}
			else
			{
				if (_rb.position.x > _startPos.x)
				{
					_rb.MovePosition(_rb.position - Vector3.right * Time.deltaTime * _speed);

                }
				else
					isForward = true;
			}
		}
		else
		{
			if (isForward)
			{
				if (_rb.position.z < _startPos.z + _distance)
				{
					_rb.MovePosition(_rb.position + Vector3.forward * Time.deltaTime * _speed);
				}
				else
					isForward = false;
			}
			else
			{
				if (_rb.position.z > _startPos.z)
				{
					_rb.MovePosition(_rb.position - Vector3.forward * Time.deltaTime * _speed);
				}
				else
					isForward = true;
			}
		}
    }


    private void OnTriggerEnter(Collider other)
    {

        var parentPlayerTransform = other.transform.parent;

        if (parentPlayerTransform != null)
        {
            parentPlayerTransform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var parentPlayerTransform = other.transform.parent;
        if (parentPlayerTransform != null)
        {
            parentPlayerTransform.SetParent(null);
        }
    }
}
