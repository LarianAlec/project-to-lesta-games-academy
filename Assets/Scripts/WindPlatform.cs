using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindPlatform : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField]
    float _windForce = 5f;
    [SerializeField]
    float _changeTime = 2f;

    [Header("CheckRange")]
    [SerializeField]
    Vector3 _offsetCenterBox;
    [SerializeField]
    Vector3 _halfExtents;
    [SerializeField]
    LayerMask _playerMask;

    private bool _isPlayerOnPlatform;
    Vector3 _windDirection;

    private void Start()
    {
        _isPlayerOnPlatform = false;

        InvokeRepeating("ChangeWindDirection", 0f, _changeTime);
    }

    private void Update()
    {
        _isPlayerOnPlatform = CheckPlatformSurface();

        if ( _isPlayerOnPlatform )
        {
            // Activate wind!
            var players = Physics.OverlapBox(gameObject.transform.position + _offsetCenterBox, _halfExtents, Quaternion.identity, _playerMask);
            foreach (var player in players)
            {
                Rigidbody rb = player.GetComponentInParent<Rigidbody>();
                if ( rb != null )
                {
                    rb.AddForce(_windDirection.normalized * _windForce * Time.deltaTime, ForceMode.Force);
                }
            }
        }
    }

    private bool CheckPlatformSurface()
    {
        return Physics.CheckBox(gameObject.transform.position + _offsetCenterBox, _halfExtents, Quaternion.identity, _playerMask);
    }

    private void ChangeWindDirection()
    {
        _windDirection =  new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
    }


    //DebugInformation

    private void OnDrawGizmosSelected()
    {
        Color transparentRed = Color.red;
        transparentRed.a = 0.3f;
        Gizmos.color = transparentRed;
        Gizmos.DrawCube(gameObject.transform.position + _offsetCenterBox, _halfExtents * 2 );
    }
}
