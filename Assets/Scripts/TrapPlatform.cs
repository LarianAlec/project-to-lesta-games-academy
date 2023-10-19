using System.Collections;
using UnityEngine;

public class TrapPlatform : MonoBehaviour
{
    [Header("StateMaterials")]
    [SerializeField]
    Material _idleStateMaterial;
    [SerializeField]
    Material _readyStateMaterial;
    [SerializeField]
    Material _activateStateMaterial;
    [SerializeField]
    Material _reloadStateMaterial;

    [Header("Properties")]
    [SerializeField]
    int _damage = 1;
    [SerializeField]
    float _reloadTime = 5f;

    [Header("OverlapBoxRange")]
    [SerializeField]
    Vector3 _offsetPosition;
    [SerializeField]
    Vector3 _halfExtents;
    [SerializeField]
    LayerMask _playerMask;

    MeshRenderer _renderer;
    bool _isReloaded;

    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _isReloaded = true;
    }

    IEnumerator TrapActivate()
    {
        _isReloaded = false;

        // Setup orange color for 1 sec
        SetupColor(_readyStateMaterial);
        yield return new WaitForSeconds(1f);

        // Red blink and damage to all
        SetupColor(_activateStateMaterial);
        ApplyDamageAllOnPlatform();
        yield return new WaitForSeconds(0.2f);

        // reload 
        float currentTime = 0f;
        while (currentTime < _reloadTime)
        {
            SetupColor(_idleStateMaterial);
            yield return new WaitForSeconds(0.5f);

            SetupColor(_reloadStateMaterial);
            yield return new WaitForSeconds(0.5f);
            currentTime += 1f;
        }

        // end reload
        _isReloaded = true;
        SetupColor(_idleStateMaterial);
    }


    private void ApplyDamageAllOnPlatform()
    {
        var players = Physics.OverlapBox(gameObject.transform.position + _offsetPosition, _halfExtents, Quaternion.identity, _playerMask);
        foreach (Collider player in players)
        {
            if (player != null)
            {
                Debug.Log("Apply damage player: " + player.name);
                var hp = player.GetComponentInParent<HealthManager>();
                if (hp != null)
                {
                    hp.ApplyDamage(_damage);
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && _isReloaded)
        {
            // Activate trap logic 
            StartCoroutine(TrapActivate());
        }
    }

    private void SetupColor(Material colorMaterial)
    {
        _renderer.material = colorMaterial;
    }

    // Draw Debug Information (overlap box range)
    private void OnDrawGizmosSelected()
    {
        Color transparentRed = Color.red;
        transparentRed.a = 0.3f;
        Gizmos.color = transparentRed;
        Gizmos.DrawCube(gameObject.transform.position + _offsetPosition, _halfExtents);
    }


}
