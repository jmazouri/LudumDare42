using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUIController : MonoBehaviour
{
    [SerializeField] private GameObject _controlsMenu;
    [SerializeField] private GameObject _mainMenu;
    
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