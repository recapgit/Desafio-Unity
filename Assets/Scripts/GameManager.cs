using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public enum GameState
    {
        InGame,
        Paused,
        GameOver
    }

    [Header("Screens")]
    public GameState currentState;
    public GameState previousState;

    [Header("UI")]
    public GameObject pauseScreen;
    public GameObject gameOverScreen;

    [Header("Death Screen")]
    public TextMeshProUGUI levelReachedDisplay;
    public TextMeshProUGUI timeSurvivedDisplay;

    [Header("Relogio")]
    public float timeLimit;
    float clockTime;
    public TextMeshProUGUI clockTimeDisplay;

    [Header("Game")]
    public GameObject playerObject;
    public bool isGameOver = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DisableScreens();
    }

    void Update()
    {
        switch (currentState)
        {
            case GameState.InGame:
                PauseAndResume();
                UpdateclockTime();
                break;
            
            case GameState.Paused:
                PauseAndResume();
                break;

            case GameState.GameOver:
                if (!isGameOver)
                {
                    isGameOver = true;
                    Time.timeScale = 0f;
                    DisplayGameOver();
                }
                break;

            default:
                break;
        }
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }

    public void PauseGame()
    {
        if (currentState != GameState.Paused)
        {
            previousState = currentState;
            ChangeState(GameState.Paused);
            Time.timeScale = 0f;
            pauseScreen.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
             ChangeState(previousState);
             Time.timeScale = 1f;
            pauseScreen.SetActive(false);
        }
    }

    public void PauseAndResume()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.Paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void DisableScreens()
    {
        pauseScreen.SetActive(false);
        gameOverScreen.SetActive(false);
    }

    public void GameOver()
    {
        timeSurvivedDisplay.text = clockTimeDisplay.text;
        ChangeState(GameState.GameOver);
    }

    void DisplayGameOver()
    {
        gameOverScreen.SetActive(true);
    }

    public void AssignLevelReachedUI(int levelReachedData)
    {
        levelReachedDisplay.text = levelReachedData.ToString();
    }

    void UpdateclockTime()
    {
        clockTime += Time.deltaTime;
        UpdateclockTimeDisplay();
        if (clockTime >= timeLimit)
        {
            playerObject.SendMessage("Kill");
        }
    }

    void UpdateclockTimeDisplay()
    {
        int minutes = Mathf.FloorToInt(clockTime / 60);
        int seconds = Mathf.FloorToInt(clockTime % 60);

        clockTimeDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}   
