using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using CharacterLogic;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI; // Importa DOTween!

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float gameTimer = 300f; // 5 minuti
    public float reduceTime = 30f;
    public float thresholdReduceTime = 30f; // la threshold non sarebbe nel GDD!
    private bool changeModeFirstTime = false; // per ridurre il tempo solo una volta
    public TextMeshProUGUI timerText;
    public GameObject victoryPanel;
    public GameObject pausePanel;
    public Button pauseButton;
    public string sceneToLoad = "MainMenu";

    [SerializeField] private List<GameObject> playerObjects;
    [SerializeField] private List<CharacterHealth> characterHealthList = new();
    private bool gameEnded = false;
    private bool isPaused = false;
    private bool gameStarted = false; // Timer parte solo dopo il countdown

    public UnityEvent dieEvent = new();

    private void Awake() => Instance = this;

    void Start()
    {
        pauseButton.enabled = false;
        PopulatePlayersAndHealth();
        StartCoroutine(StartCountdown()); // Avvia il countdown all'inizio
        dieEvent.AddListener(() =>
        {
            if (playerObjects.Count > 0 && playerObjects[0] != null)
                OnCharacterDeathOrDisappear(playerObjects[0]);
        });

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) dieEvent.Invoke();

        if (!gameStarted || gameEnded || isPaused) return;

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

    IEnumerator StartCountdown()
    {
        timerText.gameObject.SetActive(true);

        string[] countdownTexts = { "3", "2", "1", "Via!" };

        foreach (string text in countdownTexts)
        {
            timerText.text = text;
            timerText.transform.localScale = Vector3.zero;

            timerText.transform.DOScale(Vector3.one, 0.5f)
                      .SetEase(Ease.OutBounce);

            yield return new WaitForSeconds(1f);
        }

        gameStarted = true;
        pauseButton.enabled = true;
        UpdateTimerDisplay();
    }

    int RemainingCharacters() =>
        characterHealthList.Count(h => h != null && h.HP > 0);

    CharacterHealth DetermineWinnerByHealth() =>
        characterHealthList.Where(h => h != null && h.HP > 0)
                           .OrderByDescending(h => h.HP)
                           .FirstOrDefault();

    CharacterHealth DetermineLastStanding() =>
        characterHealthList.FirstOrDefault(h => h != null && h.HP > 0);

    void EndGame(CharacterHealth winner)
    {
        gameEnded = true;
        ShowVictoryPanel(winner);

        RefreshLists();

        foreach (var player in playerObjects)
        {
            var characterManager = player?.GetComponent<CharacterManager>();
            characterManager?.stateMachine.ChangeState(Player_State.Pause);
        }
    }


    void ShowVictoryPanel(CharacterHealth winner)
    {
        Time.timeScale = 0;
        victoryPanel.SetActive(true);
    }

    public void OnCharacterDeathOrDisappear(GameObject character)
    {
        if (character == null) return;
        var health = character.GetComponent<CharacterHealth>();
        if (health != null) health.HP = 0;

        characterHealthList[0].TakeDamage(1000,new());
        characterHealthList.RemoveAt(0);

        if (!gameEnded && 
            characterHealthList.Count() <= 4 && 
            changeModeFirstTime == false && 
            gameTimer > thresholdReduceTime)

            ReduceTimer(reduceTime);

        if (!gameEnded && RemainingCharacters() == 1) EndGame(DetermineLastStanding());

        //if (!gameEnded)
        //{
        //    ReduceTimer(30f);
        //    if (RemainingCharacters() == 1) EndGame(DetermineLastStanding());
        //}
    }


    void ReduceTimer(float amount)
    {
        changeModeFirstTime = true;
        gameTimer = Mathf.Max(0, gameTimer - amount);
        UpdateTimerDisplay();
    }

    public void PauseGame()
    {
        isPaused = true;
        pausePanel?.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel?.SetActive(false);
    }

    public void RestartGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void ExitToScene() => SceneManager.LoadScene(sceneToLoad);

    void PopulatePlayersAndHealth()
    {
        playerObjects = GameObject.FindGameObjectsWithTag("Player").ToList();
        characterHealthList = playerObjects
                              .Select(obj => obj.GetComponent<CharacterHealth>())
                              .Where(h => h != null)
                              .ToList();
    }

    void RefreshLists()
    {
        playerObjects.RemoveAll(po => po == null);
        characterHealthList.RemoveAll(chl => chl == null);
    }
}
