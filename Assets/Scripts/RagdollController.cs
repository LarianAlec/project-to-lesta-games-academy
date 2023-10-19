using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody[] _rigidbodies;
    [SerializeField]
    private Collider[] _colliders;
    [SerializeField]
    private Collider _mainCollider;
    [SerializeField]
    private Rigidbody _mainRigidbody;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private ThirdPersonCam _thirdPersonCam;

    private Vector3 _currentPosition;



    private void Start()
    {
        foreach (var rigidbody in _rigidbodies)
        {
            rigidbody.isKinematic = true;
        }

        foreach (var collider in _colliders)
        {
            collider.enabled = false;
        }
        _animator.enabled = true;
        _mainCollider.enabled = true;
    }

    [ContextMenu("EnableRagdoll")]
    public void EnableRagdoll()
    {
        _animator.enabled = false;
        _mainRigidbody.isKinematic = true;
        _mainCollider.enabled = false;
        _mainRigidbody.gameObject.GetComponent<PlayerMovement>().enabled = false;
        _thirdPersonCam.enabled = false;

        foreach (var collider in _colliders)
        {
            collider.enabled = true;
        }

        foreach (var rigidbody in _rigidbodies)
        {
            rigidbody.isKinematic = false;
        }
    }

    [ContextMenu("DisableRagdoll")]
    public void DisableRagdoll()
    {

        foreach (var rigidbody in _rigidbodies)
        {
            rigidbody.isKinematic = true;
        }

        foreach (var collider in _colliders)
        {
            collider.enabled = false;
        }

        _mainRigidbody.isKinematic = false;
        _mainCollider.enabled = true;
        _mainRigidbody.gameObject.GetComponent<PlayerMovement>().enabled = true;
        _thirdPersonCam.enabled = true;
        _animator.enabled = true;
    }


    public void ActivateRagdollForSeconds(float time)
    {
        StartCoroutine(RagdollInSeconds(time));
    }

    IEnumerator RagdollInSeconds(float time)
    {
        EnableRagdoll();
        yield return new WaitForSeconds(time);
        DisableRagdoll();
    }

}
