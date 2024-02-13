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
        AwakeInizializer();
    }
    private void AwakeInizializer()
    {
        _player=GameObject.FindWithTag("Player");
        
    }


    private void OnEnable()
    {
        EventManager.OnPlayerDeath += ShowDeathMenu;
        EventManager.OnReload += AwakeInizializer;
    }
    private void OnDisable()
    {
        EventManager.OnPlayerDeath -= ShowDeathMenu;
        EventManager.OnReload -= AwakeInizializer;
    }

    private void ShowDeathMenu()
    {
        _deathMenu.alpha = 1f;
    }

    public void RedButton()
    {
        if (_deathMenu.alpha >= 1f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            EventManager.OnReload?.Invoke();
            _deathMenu.alpha = 0f;
        }
        
    }
    

}
