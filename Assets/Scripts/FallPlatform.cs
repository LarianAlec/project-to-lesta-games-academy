using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FallPlatform : MonoBehaviour
{
    [SerializeField]
    private float _timeToFall = 1.5f;

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true;

        if( _rb.GetComponent<MeshCollider>() != null )
        {
            _rb.GetComponent<MeshCollider>().convex = true;
        }
    }

    private IEnumerator ActivateFall()
    {
        yield return new WaitForSeconds(_timeToFall);
        _rb.isKinematic = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ActivateFall());
        }
    }

}
