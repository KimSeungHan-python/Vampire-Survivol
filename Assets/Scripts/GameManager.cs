using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System.Reflection;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public enum GameState
    {
        Gameplay,
        Paused,
        GameOver
    }

    public GameState currentState;

    public GameState previousState;

    [Header("Screens")]
    public GameObject pauseScreen;
    public GameObject resultsScreen;

    [Header("Current Stats Display")]
    //Current stat displays
    public TextMeshProUGUI currentHealthDisplay;
    public TextMeshProUGUI currentMightDisplay;
    public TextMeshProUGUI currentMoveSpeedDisplay;
    public TextMeshProUGUI currentProjectileSpeedDisplay;
    public TextMeshProUGUI currentRecoveryDisplay;
    public TextMeshProUGUI currentMagnetDisplay;
    //Flag to check if the game is over
    public bool isGameOver = false;

    [Header("Results screen Display")]
    public Image chosenCharacterImage;
    public TextMeshProUGUI chosenCharacterName;
    public TextMeshProUGUI levelReachedDisplay;

    public List<Image> chosenWeaponUI = new List<Image>(6);
    public List<Image> chosenPassiveItemUI = new List<Image>(6);

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        DisableScreens();
        //currentState = GameState.Gameplay;
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
                if(!isGameOver)
                {
                    isGameOver = true;
                    Time.timeScale = 0f; // Freeze the game
                    Debug.Log("Game Over!");
                    DisplayResults();
                    // Implement additional game over logic here (e.g., show game over screen, stop player movement, etc.)
                }   
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
        resultsScreen.SetActive(false);
    }

    public void GameOver()
    {
        ChangeState(GameState.GameOver);
    }

    void DisplayResults()
    {
        resultsScreen.SetActive(true);
        // Populate results screen with relevant data (e.g., score, stats, etc.)
    }

    public void AssignChosenCharacterUI(CharacterScriptableObject characterData)
    {
        chosenCharacterImage.sprite = characterData.Icon;
        chosenCharacterName.text = characterData.Name;
    }

    public void AssignLevelReachedUI(int levelReachedData)
    {
        levelReachedDisplay.text = levelReachedData.ToString();
    }

    public void AssignChosenWeaponAndPassiveItemUI(List<Image> chosenWeaponUIData, List<Image> chosenPassiveItemUIData)
    {
        if(chosenWeaponUIData.Count != chosenWeaponUI.Count || chosenPassiveItemUIData.Count != chosenPassiveItemUI.Count)
        {
            Debug.LogError("Mismatch in UI list sizes!");
            return;
        }
        for(int i = 0; i < chosenWeaponUIData.Count; i++)
        {
            if(chosenWeaponUI[i].sprite)
            {
                chosenWeaponUIData[i].enabled = true;
                chosenWeaponUIData[i].sprite = chosenWeaponUI[i].sprite;
                
            }
            else
            {
                chosenWeaponUIData[i].enabled = false;
            }
        }
        for(int i = 0; i < chosenPassiveItemUIData.Count; i++)
        {
            if(chosenPassiveItemUI[i].sprite)
            {
                chosenPassiveItemUIData[i].enabled = true;
                chosenPassiveItemUIData[i].sprite = chosenPassiveItemUI[i].sprite;
                
            }
            else
            {
                chosenPassiveItemUIData[i].enabled = false;
            }
        }


    }
}
