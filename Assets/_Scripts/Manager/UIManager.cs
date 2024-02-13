using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private int _score;
    
    private GameObject _player;
    [SerializeField] private CanvasGroup _deathMenu;

    private bool isPlayerAlive;
    
    private void Awake()
    {
        AwakeInizializer();
    }
    private void AwakeInizializer()
    {
        _player=GameObject.FindWithTag("Player");
        _score = 0;
        _scoreText.text = "Score:" + _score;
    }



    private void OnEnable()
    {
        EventManager.OnPlayerDeath += ShowDeathMenu;
        EventManager.OnReload += AwakeInizializer;
        EventManager.OnPlayerMoveUp += ScoreUp;
    }
    private void OnDisable()
    {
        EventManager.OnPlayerDeath -= ShowDeathMenu;
        EventManager.OnReload -= AwakeInizializer;
        EventManager.OnPlayerMoveUp -= ScoreUp;
    }

    private void ShowDeathMenu()
    {
        isPlayerAlive = false;
        _deathMenu.alpha = 1f;
    }
    
    private void ScoreUp()
    {
        Debug.Log("hello");
        _score++;
        _scoreText.text = "Score:" + _score;
    }

    public void RedButton()
    {
        if (_deathMenu.alpha >= 1f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            EventManager.OnReload?.Invoke();
            _deathMenu.alpha = 0f;
            _score = 0;
        }
        
    }
    

}
