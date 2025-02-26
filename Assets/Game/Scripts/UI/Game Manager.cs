using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using CharacterLogic;
using UnityEngine.Events;
using DG.Tweening; // Importa DOTween!

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float gameTimer = 300f; // 5 minuti
    public TextMeshProUGUI timerText;
    public GameObject victoryPanel;
    public TextMeshProUGUI victoryText;
    public GameObject pausePanel;
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
        PopulatePlayersAndHealth();
        StartCoroutine(StartCountdown()); // Avvia il countdown all'inizio
        dieEvent.AddListener(() => OnCharacterDeathOrDisappear(playerObjects[0]));
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

            // Effetto Bouncy con DOTween
            timerText.transform.DOScale(Vector3.one, 0.5f)
                      .SetEase(Ease.OutBounce);

            yield return new WaitForSeconds(1f); // Pausa tra i numeri
        }

        gameStarted = true; // Avvia il timer
        UpdateTimerDisplay(); // Mostra il tempo normale
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
        PauseGame();
        ShowVictoryPanel(winner);

        foreach (var player in playerObjects)
        {
            var characterManager = player.GetComponent<CharacterManager>();
            characterManager?.stateMachine.ChangeState(Player_State.Pause);
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
        if (health != null) health.HP = 0;

        if (!gameEnded)
        {
            ReduceTimer(30f);
            if (RemainingCharacters() == 1) EndGame(DetermineLastStanding());
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
