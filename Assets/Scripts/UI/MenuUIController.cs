using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIController : MonoBehaviour
{
    [SerializeField] private GameObject _controlsMenu;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private RectTransform _heart;

    public LTDescr HeartTween;

    public void Start()
    {
        HeartTween = LeanTween
            .scale(_heart.gameObject, new Vector3(1.25f, 1.25f), 0.33f)
            .setLoopPingPong()
            .setEase(LeanTweenType.easeOutCirc);
    }

    public void ShowControlsMenu()
    {
        _mainMenu.SetActive(false);
        _controlsMenu.SetActive(true);
    }

    public void ShowMainMenu()
    {
        _mainMenu.SetActive(true);
        _controlsMenu.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("TheGame");
    }
}