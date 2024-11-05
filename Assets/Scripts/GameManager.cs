using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameManager : MonoBehaviour
{
    public static GameManager instance; //Singleton instance for global acceess to GameManager

    public enum GameState   //All the different states of the player
    {
        Gameplay,
        Paused,
        GameOver,
        LevelUp
    }

    public GameState currentState;
    public GameState previousState;

    [Header("Screens")]
    public GameObject pauseScreen;
    public GameObject resultsScreen;
    public GameObject levelUpScreen;

    [Header("Current Stat Displays")]
    public TMP_Text currentHealthDisplay;
    public TMP_Text currentRecoveryDisplay;
    public TMP_Text currentMoveSpeedDisplay;
    public TMP_Text currentMightDisplay;
    public TMP_Text currentProjectileSpeedDisplay;
    public TMP_Text currentMagnetDisplay;

    [Header("Results Screen Displays")]
    public Image chosenCharacterImage;
    public TMP_Text chosenCharacterName;
    public TMP_Text levelReachedDisplay;
    public TMP_Text timeSurvivedDisplay;
    public List<Image> chosenWeaponUI = new List<Image>(3);
    public List<Image> chosenPassiveItemsUI = new List<Image>(3);

    [Header("Stopwatch")]
    public float timeLimit;
    float stopwatchTime;
    public TMP_Text stopwatchDisplay;

    [Header("Audio")]
    public AudioClip backgroundMusic;
    private AudioSource audioSource;


    public bool isGameOver = false;

    public bool choosingUpgrade = false;

    public GameObject playerObject;

    private void Awake()
    {
        //Implement singleton pattern
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("EXTRA " + this + " DELETED");
        }
        DisableScreens(); //Hide all screens at start

        //Setting up music
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.playOnAwake = true;
        audioSource.volume = 0.25f; 
        audioSource.Play();

    }

    void Update()
    {
        switch (currentState)
        {
            case GameState.Gameplay:
                CheckForPauseAndResume();   //Check for pause action
                UpdateStopwatch();  //Updates stopwatch (clock on game)
                break;
            case GameState.Paused:
                CheckForPauseAndResume();   //Checks for resume
                break;
            case GameState.GameOver:    
                if (!isGameOver)
                {
                    isGameOver = true;  //If game over, freeze time
                    Time.timeScale = 0f;
                    Debug.Log("GAME IS OVER");
                    DisplayResults();   //Show results
                }
                break;
            case GameState.LevelUp:
                if (!choosingUpgrade)
                {
                    choosingUpgrade = true;
                    Time.timeScale = 0f;    //Freeze game while leveling up
                    Debug.Log("Upgrades shown");
                    levelUpScreen.SetActive(true);  //Show level up screen
                }
                break;
            default:
                Debug.LogWarning("STATE DOES NOT EXIST");   //Log to warn if player broke the game
                break;
        }
    }

    //Changes current game state
    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }

    public void PauseGame() //Pauses game and shows pause screen
    {
        if(currentState != GameState.Paused)
        {
            previousState = currentState;   //Saves current state
            ChangeState(GameState.Paused);  //Changes to pasused state
            Time.timeScale = 0f;            //Freezes time
            pauseScreen.SetActive(true);    //Pause screen
            Debug.Log("Game is paused");
        }
    }

    //Resume from paused state
    public void ResumeGame()
    {
        if(currentState == GameState.Paused)    //Checks if paused 
        {
            ChangeState(previousState);
            Time.timeScale = 1f;    //Resumes time
            pauseScreen.SetActive(false);   //Turns off pause
            Debug.Log("Game is resumed");
        }
    }

    void CheckForPauseAndResume()   //Checks for flag to pause/resume
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(currentState == GameState.Paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    void DisableScreens()   //Hides all UI at start
    {
        pauseScreen.SetActive(false);
        resultsScreen.SetActive(false);
        levelUpScreen.SetActive(false);
    }

    public void GameOver()  //Triggers game over and shows results screen
    {
        timeSurvivedDisplay.text = stopwatchDisplay.text;
        ChangeState(GameState.GameOver);
    }

    void DisplayResults()   //Displays results
    {
        resultsScreen.SetActive(true);
    }

    public void AssignLevelCharacterUI(CharacterScriptableObject chosenCharacterData) //Sets up character UI on results
    {
        chosenCharacterImage.sprite = chosenCharacterData.Icon;
        chosenCharacterName.text = chosenCharacterData.name;
    }
    public void AssignLevelReachedUI(int levelReachedData)  //Sets up level reached UI on results
    {
        levelReachedDisplay.text = levelReachedData.ToString();
    }

    public void AssignWeaponsAndPassiveItemsUI(List<Image> achievedWeaponsData, List<Image> achievedPassiveItemsData) //Updates UI for achieved / chosen weapons / items on results
    {
        if(achievedWeaponsData.Count != chosenWeaponUI.Count || achievedPassiveItemsData.Count != chosenPassiveItemsUI.Count)
        {
            Debug.Log("Achieved weapons and passive items data list have different lengths");
            return;
        }

        for(int i = 0; i < chosenWeaponUI.Count; i++)       //Grabs weapons
        {
            if (achievedWeaponsData[i].sprite)
            {
                chosenWeaponUI[i].enabled = true;
                chosenWeaponUI[i].sprite = achievedWeaponsData[i].sprite;
            }
            else
            {
                chosenWeaponUI[i].enabled = false;
            }
        }

        for (int i = 0; i < chosenPassiveItemsUI.Count; i++)    //Grabs items
        {
            if (achievedPassiveItemsData[i].sprite)
            {
                chosenPassiveItemsUI[i].enabled = true;
                chosenPassiveItemsUI[i].sprite = achievedPassiveItemsData[i].sprite;
            }
            else
            {
                chosenPassiveItemsUI[i].enabled = false;
            }
        }
    }

    void UpdateStopwatch()  //Updates ingame timer
    {
        stopwatchTime += Time.deltaTime;

        UpdateStopwatchDisplay();

        if(stopwatchTime >= timeLimit)
        {
            GameOver();
        }
    }

    void UpdateStopwatchDisplay()   //Updates display in game to be minutes:seconds
    {
        int minutes = Mathf.FloorToInt(stopwatchTime/60);
        int seconds = Mathf.FloorToInt(stopwatchTime%60);

        stopwatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartLevelUp()  //Inititates level up
    {
        ChangeState(GameState.LevelUp);
        playerObject.SendMessage("RemoveAndApplyUpgrades");
    }

    public void EndLevelUp()    //Ends level up (connected to button, resumes time and disables screen
    {
        choosingUpgrade = false;
        Time.timeScale = 1f;
        levelUpScreen.SetActive(false);
        ChangeState(GameState.Gameplay);
    }
}
