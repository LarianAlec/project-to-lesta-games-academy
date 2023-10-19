using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class ProjectileSettingsComponent : MonoBehaviour
{
    [SerializeField] Vector3 _direction;
    [SerializeField] float _impulseValue = 20f;
    [SerializeField] float _lifeTime = 3f;
    [SerializeField] bool _isDeadly = false;
    [SerializeField] int _damage = 1;

    private Rigidbody _rb;


    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        _rb.AddForce(_direction.normalized * _impulseValue, ForceMode.Impulse);

        Destroy(gameObject, _lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_isDeadly) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject;
            var playerHp = player.GetComponentInParent<HealthManager>();
            playerHp.ApplyDamage(_damage);

            // Activate ragdoll
            var ragdoll = player.GetComponentInChildren<RagdollController>();
            ragdoll?.ActivateRagdollForSeconds(1f);

        }
    }


}
