using UnityEngine;
using TMPro;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private int _health = 3;
    [SerializeField] private TextMeshProUGUI _healthUI;

    private void Start()
    {
        UpdateUI_HP();
    }

    public void ApplyDamage(int damage)
    {
        _health -= damage;
        UpdateUI_HP();
    }

    private void UpdateUI_HP()
    {
        if (_healthUI != null)
        {
            _healthUI.text = "HP : " + _health.ToString();
        }
    }

    private void Update()
    {
        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<GameManager>().OnDefeatScreen();
    }

    public int GetHealth()
    {
        return _health;
    }
}
