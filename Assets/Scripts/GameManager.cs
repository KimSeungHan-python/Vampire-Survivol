using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Gameplay,
        Paused,
        GameOver
    }

    public GameState currentState;

    public GameState previousState;

    [Header("UI")]
    public GameObject pauseScreen;

    void Awake()
    {
        DisableScreens();
        currentState = GameState.Gameplay;
    }
    void Update()
    {
        switch (currentState)
        {
            case GameState.Gameplay:
                //Code for the gameplay state
                break;
            case GameState.Paused:
                //Code for the paused state
                break;
            case GameState.GameOver:
                //Code for the game over state
                break;
        }
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }

    public void PauseGame()
    {
        if(currentState != GameState.Paused)
        {
            previousState = currentState;
            ChangeState(GameState.Paused);
            pauseScreen.SetActive(true);
            Time.timeScale = 0f; // Freeze the game
            Debug.Log("Game Paused");
        }
    }

    public void ResumeGame()
    {
        if(currentState == GameState.Paused)
        {
            ChangeState(previousState);
            pauseScreen.SetActive(false);
            Time.timeScale = 1f; // Resume the game
            Debug.Log("Game Resumed");
        }
    }

    void DisableScreens()
    {
        pauseScreen.SetActive(false);
    }   
}
