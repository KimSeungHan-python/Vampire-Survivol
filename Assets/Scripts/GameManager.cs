using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public enum GameState
    {
        Gameplay,
        Paused,
        GameOver,
        LevelUp
    }

    public GameState currentState;

    public GameState previousState;

    [Header("Damage Text Settings")]
    public Canvas damageTextCanvas;
    public float textFontSize = 20;
    public TMP_FontAsset textFont;
    public Camera referenceCamera;

    [Header("Screens")]
    public GameObject pauseScreen;
    public GameObject resultsScreen;
    public GameObject levelUpScreen;

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

    //Flag to check if the player is choosing their upgrades
    public bool choosingUpgrade;

    public GameObject playerObject;

    [Header("Results screen Display")]
    public Image chosenCharacterImage;
    public TextMeshProUGUI chosenCharacterName;
    public TextMeshProUGUI levelReachedDisplay;
    public TextMeshProUGUI timeSurvivedDisplay;

    public List<Image> chosenWeaponUI = new List<Image>(6);
    public List<Image> chosenPassiveItemUI = new List<Image>(6);

    [Header("Stopwatch")]
    public float timelimit; // The tiem limit in seconds
    float stoptwatchTime;
    public TextMeshProUGUI stopwatchDisplay;

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
                UpdateStopwatch();
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
            case GameState.LevelUp:
                if(!choosingUpgrade)
                {
                    choosingUpgrade = true;
                    Time.timeScale = 0f; // Freeze the game
                    levelUpScreen.SetActive(true);
                    Debug.Log("Level Up! Choose your upgrade.");
                    // Implement additional level up logic here (e.g., show level up screen, pause game, etc.)
                }                
                break;
        }
    }

    public static void GenerateFloatingText(string text, Transform target, float duration =1f, float speed =1f)
    {
        // If the canvas is not set, end the function so we don't
        // generate any floating text;
        if(!instance.damageTextCanvas)
            return;
        
        // Find a relevant camera that we can use to convert the world
        // position to a screen position
        if(!instance.referenceCamera)
            instance.referenceCamera = Camera.main;

        instance.StartCoroutine(instance.GenerateFloatingTextCoroutine(text, target, duration, speed));
    }

    IEnumerator GenerateFloatingTextCoroutine(string text, Transform target, float duration, float speed)
    {
        // Create a new TextMeshProUGUI object
        GameObject textObj = new GameObject("Damage Floating Text");
        textObj.transform.SetParent(instance.damageTextCanvas.transform);

        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = instance.textFontSize;
        if(instance.textFont)
            tmp.font = instance.textFont;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.enableWordWrapping = false;
        tmp.raycastTarget = false;

        float elapsedTime = 0f;
        Vector3 startPosition = instance.referenceCamera.WorldToScreenPoint(target.position);
        Vector3 endPosition = startPosition + new Vector3(0, 100, 0); // Move up by 100 units

        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            tmp.transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        Destroy(textObj);
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
        levelUpScreen.SetActive(false);
    }

    public void GameOver()
    {
        timeSurvivedDisplay.text = stopwatchDisplay.text;
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

    void UpdateStopwatch()
    {
        stoptwatchTime += Time.deltaTime;
        UpdateStopwatchDisplay();
        
        if(stoptwatchTime >= timelimit)
        {
            playerObject.SendMessage("Kill");          // You can add additional logic here if needed when the time limit is reached
        }
    }

    void UpdateStopwatchDisplay()
    {
        int minutes = Mathf.FloorToInt(stoptwatchTime / 60f);
        int seconds = Mathf.FloorToInt(stoptwatchTime % 60f);
        stopwatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartLevelUP()
    {
        ChangeState(GameState.LevelUp);
        playerObject.SendMessage("RemoveAndApplyUpgradeOptions");
    }

    public void EndLevelUP()
    {
        choosingUpgrade = false;
        levelUpScreen.SetActive(false);
        Time.timeScale = 1f; // Resume the game
        ChangeState(GameState.Gameplay);
    }
}
