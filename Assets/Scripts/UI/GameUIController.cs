using System;
using System.Collections.Generic;
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
    [SerializeField] private GameObject _dialogueObject;
    [SerializeField] private TextMeshProUGUI _dialogueTextBox;
    [SerializeField] private int _characterLimit;
    [SerializeField] private float _maxReadingTime;
    [SerializeField] private float _timeBetweenCharacters;
    private bool _isPrinting;
    private int _characterCount;
    private List<string> _dialogues;
    private float _readingTimePassed;
    private float _characterPrintingTimePassed;

    private void Start()
    {
        _dialogues = new List<string>(10);
        _dialogueTextBox.text = string.Empty;
        _dialogueObject.SetActive(false);
        _healthBar.maxValue = _maxHealth;
        _ammoBar.maxValue = _maxAmmo;
        AssignNewHealth(_maxHealth);
        AssignNewAmmo(_maxAmmo);

        if (!_demo) return;

        AssignNewHealth(_maxHealth / 2);
        AssignNewAmmo(_maxAmmo / 2);
        AssignNewScore(5050);
        QueueNewDialogueText("WEW LAD");
        QueueNewDialogueText("ITS WORKING");
        QueueNewDialogueText("Notice me player-kun.");
    }

    void Update()
    {
        if (_dialogues.Count > 0 && !_dialogueObject.active)
        {
            _dialogueObject.SetActive(true);
        }
        else if (_dialogueObject.active && _dialogues.Count == 0 && _readingTimePassed > _maxReadingTime)
        {
            _dialogueObject.SetActive(false);
        }

        if (!_isPrinting && _dialogueObject.activeInHierarchy && (_readingTimePassed >= _maxReadingTime || _dialogueTextBox.text == string.Empty))
        {
            _characterCount = 0;
            _isPrinting = true;
            _dialogueTextBox.text = string.Empty;
            _readingTimePassed = 0f;
        }
        else if (!_isPrinting && _dialogueObject.activeInHierarchy)
        {
            _readingTimePassed += Time.deltaTime;
        }
        else if (_characterCount < _dialogues[0].Length && _characterPrintingTimePassed >= _timeBetweenCharacters)
        {
            _dialogueTextBox.text += _dialogues[0][_characterCount];
            _characterCount++;
            _characterPrintingTimePassed = 0;
        }
        else if (_characterCount >= _dialogues[0].Length)
        {
            _isPrinting = false;
            var text = _dialogues[0];
            _dialogues.Remove(text);
        }
        else if (_isPrinting)
        {
            _characterPrintingTimePassed += Time.deltaTime;
        }
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

    public void QueueNewDialogueText(string text)
    {
        if (text.Length > _characterLimit)
        {
            throw new FormatException(
                "String exceeds the character limit of the Dialogue box. Please split the string and parse them in one at a time.");
        }

        _dialogues.Add(text);
    }
}