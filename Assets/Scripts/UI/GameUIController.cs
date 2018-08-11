using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Slider _ammoBar;
    [SerializeField] private TextMeshProUGUI _score;
    [SerializeField] private TextMeshProUGUI _shadowScore;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _maxAmmo;
    [SerializeField] private bool _demo;

    private void Start()
    {
        _healthBar.maxValue = _maxHealth;
        _ammoBar.maxValue = _maxAmmo;
        AssignNewHealth(_maxHealth);
        AssignNewAmmo(_maxAmmo);
        
        if (!_demo) return;
        
        AssignNewHealth(_maxHealth / 2);
        AssignNewAmmo(_maxAmmo / 2);
        AssignNewScore(5050);
    }

    public void AssignNewHealth(float health)
    {
        var assignValue = Mathf.Clamp(health, 0, _maxHealth);

        _healthBar.value = assignValue;
    }

    public void AssignNewAmmo(float ammoCount)
    {
        var assignValue = Mathf.Clamp(ammoCount, 0, _maxAmmo);

        _ammoBar.value = assignValue;
    }

    public void AssignNewScore(int score)
    {
        var result = Mathf.Clamp(score, 0, 9999999999999).ToString("0000000000000");
        _score.text = result;
        _shadowScore.text = result;
    }
}