using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _maxScoreText;
    [SerializeField] private TMP_Text _coinText;
    private int _score;
    
    [HideInInspector]public static int _coins;
    [HideInInspector]public static int _maxScore;
    
    private GameObject _player;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private CanvasGroup _deathMenu;

    private bool isPlayerAlive;
    
    private void Awake()
    {
        AwakeInizializer();
    }
    private void Start()
    {
        StartInizializer();
    }
    private void AwakeInizializer()
    {
        _player=GameObject.FindWithTag("Player");
        _score = 0;
        // _scoreText.alpha = 0f;
        // _maxScoreText.alpha = 0f;
    }
    private void StartInizializer()
    {
        RestoreData();
        
        _scoreText.text = "Score: " + _score;
        _maxScoreText.text = "Max Score: " + _maxScore;
        _maxScoreText.text = "Max Score: " + _maxScore;
        _coinText.text = "Coins: " + _coins;
        
    }


    private void OnEnable()
    {
        EventManager.OnPlayerDeath += ShowDeathMenu;
        EventManager.OnReload += AwakeInizializer; StartInizializer();
        EventManager.OnPlayerMoveUp += ScoreUp;
        EventManager.OnCoinIncrease += CoinUp;
        EventManager.OnGameStart += GameStart;
    }
    private void OnDisable()
    {
        EventManager.OnPlayerDeath -= ShowDeathMenu;
        EventManager.OnReload -= AwakeInizializer;
        EventManager.OnPlayerMoveUp -= ScoreUp;
        EventManager.OnCoinIncrease -= CoinUp;
        EventManager.OnGameStart -= GameStart;
    }

    private void ShowDeathMenu()
    {
        isPlayerAlive = false;
        _deathMenu.alpha = 1f;
    }
    
    private void ScoreUp()
    {
        _score++;
        _scoreText.text = "Score: " + _score;
        if (_score>_maxScore)
        {
            _maxScore = _score;
            _maxScoreText.text = "Max Score: " + _maxScore;
        }
        
    }
    private void CoinUp(int coinToAdd)
    {
        _coins += coinToAdd;
        _coinText.text = "Coins: " + _coins;
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
    private void RestoreData()
    {
        BinaryDataSaver data = SaveSystem.LoadPlayerData();

        _maxScore = data.maxScore;
        _coins = data.coins;
    }
    private void GameStart()
    {
        _scoreText.alpha = 1f;
        _maxScoreText.alpha = 1f;
        _mainMenu.SetActive(false);
        
    }
    

}
