using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum UIState
{
    Paused,
    GameOver,
    InGame,
    Dialog
}

public enum DartisMood
{
    Default,
    Angry,
    Love,
    Report,
    Thonk
}

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
    [SerializeField] private GameObject _dialogImageObject;
    [SerializeField] private int _characterLimit;
    [SerializeField] private float _maxReadingTime;
    [SerializeField] private float _timeBetweenCharacters;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private AudioClip[] _backgroundMusic;
    [SerializeField] private AudioSource _backgroundAudioSource;
    [SerializeField] private AudioSource _pauseAudioSource;
    [SerializeField] private AudioSource _dialogAudioSource;
    [SerializeField] private GameObject _gameOverMenu;
    [SerializeField] private Image _dialogueIndicator;

    // Dartis config
    [SerializeField] private Sprite _default;
    [SerializeField] private Sprite _angry;
    [SerializeField] private Sprite _love;
    [SerializeField] private Sprite _report;
    [SerializeField] private Sprite _thonk;

    private bool _isPrinting;
    private int _characterCount;
    private Dictionary<string, DartisMood> _dialogues;
    [SerializeField] private Image _imageDisplay;
    private float _readingTimePassed;
    private float _characterPrintingTimePassed;

    private int _scoreCount;

    [SerializeField] private UIState _uiState;
    public UIState UIState
    {
        get
        {
            return _uiState;
        }
        set
        {
            _uiState = value;
            TriggerStateChange();
        }
    }

    private void Start()
    {
        _dialogues = new Dictionary<string, DartisMood>(10);
        _dialogueTextBox.text = string.Empty;
        _dialogueObject.SetActive(false);
        _healthBar.maxValue = 1;
        _ammoBar.maxValue = 1;

        AssignNewHealth(100, 100);
        AssignNewAmmo(1, 1);

        TriggerStateChange();

        if (!_demo) return;

        AssignNewHealth(50, 100);
        AssignNewAmmo(5, 10);
        AssignNewScore(5050);
        QueueNewDialogueText("WEW LAD", DartisMood.Default);
        QueueNewDialogueText("ITS WORKING", DartisMood.Angry);
        QueueNewDialogueText("Notice me player-kun.", DartisMood.Report);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F10) || Input.GetKeyUp(KeyCode.Escape))
        {
            if (UIState == UIState.InGame)
            {
                UIState = UIState.Paused;
            }
            else if (UIState == UIState.Paused)
            {
                UIState = UIState.InGame;
            }
        }

        var rect = _dialogueObject.GetComponent<RectTransform>();

        if (_dialogues.Count > 0 && !_dialogueObject.activeSelf)
        {
            _dialogueObject.SetActive(true);

            LeanTween.value(_dialogueObject, update =>
            {
                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, update);
            }, -100, 30, 0.66f)
            .setEase(LeanTweenType.easeOutBack);
        }
        else if (_dialogues.Count == 0 && _readingTimePassed > _maxReadingTime && _dialogueObject.activeSelf)
        {
            LeanTween.value(_dialogueObject, update =>
            {
                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, update);
            }, 30, -100, 0.66f)
            .setEase(LeanTweenType.easeInBack)
            .setOnComplete(() => _dialogueObject.SetActive(false));            
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
        else if (_dialogues.Count > 0 && _characterCount < _dialogues.ElementAt(0).Key.Length && _characterPrintingTimePassed >= _timeBetweenCharacters)
        {
            var dialogue = _dialogues.ElementAt(0);
            _dialogueTextBox.text += dialogue.Key[_characterCount];

            if (IsPunctuation(dialogue.Key, _characterCount))
            {
                _characterPrintingTimePassed -= 0.5f;
                _dialogAudioSource.pitch = UnityEngine.Random.Range(1f, 1.5f);
            }
            else
            {
                _characterPrintingTimePassed = 0;
                _dialogAudioSource.pitch = UnityEngine.Random.Range(0.9f, 1.2f);
            }

            
            _dialogAudioSource.Play();

            _imageDisplay.sprite = GetSprityForMood(dialogue.Value);
            _characterCount++;

            _dialogueIndicator.rectTransform.anchoredPosition =
                new Vector2(0, 27 - (27 * ((float)_characterCount / _dialogues.ElementAt(0).Key.Length)));
        }
        else if (_dialogues.Count > 0 && _characterCount >= _dialogues.ElementAt(0).Key.Length)
        {
            _isPrinting = false;
            var text = _dialogues.ElementAt(0);
            _dialogues.Remove(text.Key);
        }
        else if (_isPrinting)
        {
            _characterPrintingTimePassed += Time.deltaTime;
        }

    }

    private bool IsPunctuation(string input, int index)
    {
        char c = input[index];

        bool isPunc = c == '!' || c == '.' || c == '?';

        if (index < input.Length - 1)
        {
            return isPunc && input[index + 1] == ' ';
        }
        else
        {
            return isPunc;
        }
    }

    private Sprite GetSprityForMood(DartisMood mood)
    {
        switch (mood)
        {
            case DartisMood.Default:
                return _default;
            case DartisMood.Angry:
                return _angry;
            case DartisMood.Love:
                return _love;
            case DartisMood.Report:
                return _report;
            case DartisMood.Thonk:
                return _thonk;
            default:
                throw new ArgumentOutOfRangeException(nameof(mood), mood, null);
        }
    }

    public void RestartGame()
    {
        UIState = UIState.InGame;
        SceneManager.LoadScene("TheGame");
    }

    public void UnpauseGame()
    {
        UIState = UIState.InGame;
    }

    public void TriggerStateChange()
    {
        switch (UIState)
        {
            case UIState.Paused:
                _pauseMenu.SetActive(true);
                _gameOverMenu.SetActive(false);
                
                _pauseAudioSource.Play();
                _backgroundAudioSource.Pause();

                Time.timeScale = 0f;
                break;
            case UIState.GameOver:
                _pauseMenu.SetActive(false);
                _gameOverMenu.SetActive(true);

                _backgroundAudioSource.Stop();

                Time.timeScale = 0f;
                break;
            case UIState.InGame:
                _pauseMenu.SetActive(false);
                _gameOverMenu.SetActive(false);

                _pauseAudioSource.Stop();
                _backgroundAudioSource.UnPause();

                Time.timeScale = 1f;
                break;
            case UIState.Dialog:
                _pauseMenu.SetActive(false);
                _gameOverMenu.SetActive(false);

                _pauseAudioSource.Play();
                _backgroundAudioSource.Pause();

                Time.timeScale = 1f;
                break;
        }
    }

    public void AssignNewHealth(float health, float maxHealth)
    {
        var assignValue = health / maxHealth;

        _healthBar.value = assignValue;
    }

    public void AssignNewAmmo(float ammoCount, float maxAmmo)
    {
        var assignValue = ammoCount / maxAmmo;

        _ammoBar.value = assignValue;
    }

    public void IncreaseScore(int amount)
    {
        AssignNewScore(_scoreCount + amount);
    }

    public void AssignNewScore(int score)
    {
        _scoreCount = score;

        var result = Mathf.Clamp(_scoreCount, 0, 9999999999999).ToString("0000000000000");
        _score.text = result;
        _shadowScore.text = result;
    }

    public void CancelAndClearDialog()
    {
        if (_dialogues.Any())
        {
            _characterCount = _dialogues.ElementAt(0).Key.Length + 1;
        }

        _dialogues.Clear();
        _readingTimePassed = _maxReadingTime + 1;
        _isPrinting = false;
    }

    public void QueueNewDialogueText(string text, DartisMood mood)
    {
        if (text.Length > _characterLimit)
        {
            throw new FormatException(
                "String exceeds the character limit of the Dialogue box. Please split the string and parse them in one at a time.");
        }

        _dialogues.Add(text, mood);
    }

    private int _musicIndex = 0;
    public void TriggerMusic()
    {
        _backgroundAudioSource.clip = _backgroundMusic[_musicIndex];
        _musicIndex++;

        if (_musicIndex > _backgroundMusic.Length - 1)
        {
            _musicIndex = 0;
        }

        _backgroundAudioSource.Play();
    }
}