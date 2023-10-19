using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] GameObject _object;
    [SerializeField] float _spawnInterval = 1.5f;
    [SerializeField] Transform _transformParent;

    [SerializeField] float _spreadX = 0f;
    [SerializeField] float _spreadY = 0f;
    [SerializeField] float _spreadZ = 0f;


    private void Start()
    {
        InvokeRepeating("SpawnObject", 0f, _spawnInterval);
    }

    private void SpawnObject()
    {
        Instantiate(_object, new Vector3(_transformParent.position.x + Random.Range(-_spreadX, _spreadX),
                                        _transformParent.position.y + Random.Range(-_spreadY, _spreadY),
                                        _transformParent.position.z + Random.Range(-_spreadZ, _spreadZ)),
                                        Quaternion.identity);
    }
}
