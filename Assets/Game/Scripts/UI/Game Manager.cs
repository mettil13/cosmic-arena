using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;
using System.Collections.Generic;
using CharacterLogic;
using System;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }

    public float gameTimer = 300f; // 5 minuti
    public TextMeshProUGUI timerText;
    public GameObject victoryPanel;
    public TextMeshProUGUI victoryText;
    public GameObject pausePanel;
    public string sceneToLoad = "MainMenu";

    [SerializeField] private List<GameObject> playerObjects;
    [SerializeField] private List<CharacterHealth> characterHealthList = new List<CharacterHealth>();
    private bool gameEnded = false;
    private bool isPaused = false;

    public UnityEvent dieEvent = new();

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        PopulatePlayersAndHealth();
        UpdateTimerDisplay();

        dieEvent.AddListener(() => OnCharacterDeathOrDisappear(playerObjects[0]));//da modificare
    }

    void PopulatePlayersAndHealth()
    {
        playerObjects = GameObject.FindGameObjectsWithTag("Player").ToList();
        characterHealthList = playerObjects.Select(obj => obj.GetComponent<CharacterHealth>())
                                           .Where(health => health != null)
                                           .ToList();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            dieEvent.Invoke();

        if (gameEnded || isPaused) return;

        if (gameTimer > 0)
        {
            gameTimer -= Time.deltaTime;
            UpdateTimerDisplay();
        }
        else
        {
            EndGame(DetermineWinnerByHealth());
        }

        if (RemainingCharacters() == 1)
        {
            EndGame(DetermineLastStanding());
        }

        RefreshLists();
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(gameTimer / 60);
        int seconds = Mathf.FloorToInt(gameTimer % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    int RemainingCharacters() => characterHealthList.Count(health => health != null && health.HP > 0);

    CharacterHealth DetermineWinnerByHealth()
    {
        return characterHealthList.Where(health => health != null && health.HP > 0)
                                  .OrderByDescending(health => health.HP)
                                  .FirstOrDefault();
    }

    CharacterHealth DetermineLastStanding()
    {
        return characterHealthList.FirstOrDefault(health => health != null && health.HP > 0);
    }

    void EndGame(CharacterHealth winner)
    {
        gameEnded = true;
        PauseGame();
        ShowVictoryPanel(winner);

        foreach (var player in playerObjects)
        {
            var characterManager = player.GetComponent<CharacterManager>();
            if (characterManager != null)
            {
                characterManager.stateMachine.ChangeState(Player_State.Pause);
            }
        }
    }

    void ShowVictoryPanel(CharacterHealth winner)
    {
        victoryPanel.SetActive(true);
        timerText.gameObject.SetActive(false);
        victoryText.text = winner != null ? $"{winner.gameObject.name} ha vinto!" : "Pareggio!";
    }

    public void OnCharacterDeathOrDisappear(GameObject character)
    {
        var health = character.GetComponent<CharacterHealth>();

        health.TakeDamage(100000, new GameObject());
        if (health != null)
        {
            health.HP = 0;
        }

        if (!gameEnded)
        {
            ReduceTimer(30f);

            if (RemainingCharacters() == 1)
            {
                EndGame(DetermineLastStanding());
            }
        }
    }

    void ReduceTimer(float amount)
    {
        gameTimer = Mathf.Max(0, gameTimer - amount);
        UpdateTimerDisplay();
    }

    public void PauseGame()
    {
        isPaused = true;
        if (pausePanel != null) pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        if (pausePanel != null) pausePanel.SetActive(false);
    }

    public void RestartGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void ExitToScene() => SceneManager.LoadScene(sceneToLoad);

    void RefreshLists()
    {
        playerObjects.RemoveAll(po => po == null);
        characterHealthList.RemoveAll(chl => chl == null);
    }


}
