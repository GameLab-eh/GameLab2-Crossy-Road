using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _maxScoreText;
    [SerializeField] private TMP_Text _coinText;
    private int _score;
    
    [HideInInspector]public static int _coins;
    [HideInInspector]public static int _maxScore;
    
    private GameObject _player;
    [SerializeField] private GameObject _mainMenu, _quitButton, _creditsButton, _creditsMenu, _pauseMenu;
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
        _scoreText.text = "Score: " + 0;
        _maxScoreText.text = "Max Score: " + 0;
        _coinText.text = "Coins: " + 0;
        // _scoreText.alpha = 0f;
        // _maxScoreText.alpha = 0f;
    }
    private void StartInizializer()
    {
        _scoreText.text = "Score: " + _score;
        _maxScoreText.text = "Max Score: " + _maxScore;
        _coinText.text = "Coins: " + _coins;
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            RedButton();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale!=0f)
            {
                _pauseMenu.SetActive(true);
                Time.timeScale=0f;
            }
            else
            {
                _pauseMenu.SetActive(false);
                Time.timeScale=1f;
            }
        }
    }


    private void OnEnable()
    {
        EventManager.OnPlayerDeath += ShowDeathMenu;
        EventManager.OnReload += AwakeInizializer;
        EventManager.OnPlayerMoveUp += ScoreUp;
        EventManager.OnCoinIncrease += CoinUp;
        EventManager.OnGameStart += GameStart;
        EventManager.OnPlayerFirstMove += DeactivateMenu;
    }
    private void OnDisable()
    {
        EventManager.OnPlayerDeath -= ShowDeathMenu;
        EventManager.OnReload -= AwakeInizializer;
        EventManager.OnPlayerMoveUp -= ScoreUp;
        EventManager.OnCoinIncrease -= CoinUp;
        EventManager.OnGameStart -= GameStart;
        EventManager.OnPlayerFirstMove -= DeactivateMenu;
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
    private void GameStart()
    {
        _scoreText.alpha = 1f;
        _maxScoreText.alpha = 1f;
        _mainMenu.SetActive(false);
        _quitButton.SetActive(true);
        _creditsButton.SetActive(true);
    }
    public void QuitButton()
    {
        Application.Quit();
    }
    private void DeactivateMenu()
    {
        _quitButton.SetActive(false);
        _creditsButton.SetActive(false);
    }

    public void CreditsMenuActivate()
    {
        _mainMenu.SetActive(false);
        _creditsMenu.SetActive(true);
    }
    public void BackToMain()
    {
        _mainMenu.SetActive(true);
        _creditsMenu.SetActive(false);
    }
    public void Resume()
    {
        _pauseMenu.SetActive(false);
        Time.timeScale=1f;
    }
    

}
