using UnityEngine;
using UnityEngine.Events;

public class EnterTriggerComponent : MonoBehaviour
{
    [SerializeField] string _tag;
    [SerializeField] UnityEvent _action;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter trigger!");
        if (other.gameObject.CompareTag(_tag))
        {
            _action?.Invoke();
        }
    }
}
