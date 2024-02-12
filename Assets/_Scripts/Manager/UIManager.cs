using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text Score;
    private GameObject _player;
    [SerializeField] private CanvasGroup _deathMenu;
    private void Awake()
    {
        _player=GameObject.FindWithTag("Player");
    }

    private void OnEnable()
    {
        EventManager.OnPlayerDeath += ShowDeathMenu;
    }
    private void OnDisable()
    {
        EventManager.OnPlayerDeath -= ShowDeathMenu;
    }

    private void ShowDeathMenu()
    {
        _deathMenu.alpha = 1f;
    }

    public void ResetTheGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        Debug.Log(sceneName);
        if (sceneName == "Game1")
        {
            SceneManager.LoadScene("Game2");
        }
        else
        {
            SceneManager.LoadScene("Game1");
        }
        
        _deathMenu.alpha = 0f;
        
    }

}
