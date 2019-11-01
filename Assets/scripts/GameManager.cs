using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public delegate void GameDelegate();
    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameOverConfirmed;

    public static GameManager Instance;

    public GameObject startPage;
    public GameObject gameOverPage;
    public GameObject countdownPage;
    public Text scoreText;
    public Text scoreNum;
    enum PageState
    {
        None,
        Start,
        GameOver,
        Countdown
    }
    int score = 0;
    bool gameOver = true;

    public bool GameOver { get { return gameOver; } }
    public int Score { get { return score; } }

    void Awake()
    {
        if (Instance!= null)
        {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void OnEnable()
    {
        TapController.OnPlayerDied += OnPlayerDied;
        TapController.OnPlayerScored += OnPlayerScored;
        CountdownText.OnCountdownFinished += OnCountdownFinished;
    }
    void OnDisable()
    {
        TapController.OnPlayerDied -= OnPlayerDied;
        TapController.OnPlayerScored -= OnPlayerScored;
        CountdownText.OnCountdownFinished -= OnCountdownFinished;
    }

    void OnCountdownFinished()
    {
        SetPageState(PageState.None);
        OnGameStarted();
        score = 0;
        gameOver = false;
    }
    void OnPlayerScored()
    {

        score++;
        scoreText.text = score.ToString();
    }
    void OnPlayerDied()
    {
        gameOver = true;
        int savedScore = PlayerPrefs.GetInt("Highscore");
        if (score > savedScore)
        {
            PlayerPrefs.SetInt("Highscore", score);
        }
        SetPageState(PageState.GameOver);
        scoreNum.text = score.ToString();


    }
    void SetPageState(PageState state)
    {
        switch (state) {
            case PageState.None:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);
                break;
            case PageState.Start:
                startPage.SetActive(true);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);

                break;
            case PageState.GameOver:
                startPage.SetActive(false);
                gameOverPage.SetActive(true);
                countdownPage.SetActive(false);

                break;
            case PageState.Countdown:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(true);
                break;
        }
    }

     public void ConfirmGameOver()
     {
            //activated when replay button is hit
            OnGameOverConfirmed(); //event
            scoreText.text = "0";
            SetPageState(PageState.Start);
     }
     public void StartGame()
     {
            //activated when play button is hit
            SetPageState(PageState.Countdown);

     }
}