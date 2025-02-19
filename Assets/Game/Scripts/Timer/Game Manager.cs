using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;
using CharacterLogic;

public class GameManager : MonoBehaviour
{
    public float gameTimer = 300f;
    public TextMeshProUGUI timerText;
    public CharacterManager[] characters;
    public GameObject victoryPanel;
    public TextMeshProUGUI victoryText;

    public GameObject pausePanel;

    public string sceneToLoad = "MainMenu";

    private bool gameEnded = false;
    private bool suddenDeathMode = false;
    private bool isPaused = false;
    private bool inSpecialMode = false;

    void Update()
    {
        if (gameEnded || isPaused) return;

        if (characters.Any(c => c.stateMachine.CurrentStateEnum == Player_State.Pause)) return;

        if (RemainingCharacters() == 4 && !inSpecialMode)
        {
            ActivateSpecialMode();
        }

        if (gameTimer > 0)
        {
            gameTimer -= Time.deltaTime;
            UpdateTimerDisplay();
        }
        else
        {
            if (!suddenDeathMode)
            {
                EndGame(DetermineWinnerByHealth());
            }
        }

        if (RemainingCharacters() == 1)
        {
            EndGame(DetermineLastStanding());
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(gameTimer / 60);
        int seconds = Mathf.FloorToInt(gameTimer % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    void ActivateSpecialMode()
    {
        inSpecialMode = true;
        gameTimer = 120f;
        Debug.Log("Modalità Speciale attivata! Il timer è stato accorciato.");

    }

    int RemainingCharacters() => characters.Count(c => c.stateMachine.CurrentStateEnum != Player_State.Dead);

    CharacterManager DetermineWinnerByHealth()
    {
        var aliveCharacters = characters.Where(c => c.stateMachine.CurrentStateEnum != Player_State.Dead);
        if (!aliveCharacters.Any()) return null;

        return aliveCharacters
            .OrderByDescending(c => c.GetComponent<CharacterHealth>()?.HP ?? 0)
            .FirstOrDefault();
    }

    CharacterManager DetermineLastStanding() => characters.FirstOrDefault(c => c.stateMachine.CurrentStateEnum != Player_State.Dead);

    void EndGame(CharacterManager winner)
    {
        gameEnded = true;
        PauseGame();
        ShowVictoryPanel(winner);

        foreach (var character in characters)
        {
            character.stateMachine.ChangeState(Player_State.Pause);
        }
    }

    void ShowVictoryPanel(CharacterManager winner)
    {
        victoryPanel.SetActive(true);
        DisableTimer();
        victoryText.text = winner != null
            ? $"{winner.gameObject.name} ha vinto!"
            : "Pareggio!";
    }

    public void CloseVictoryPanel()
    {
        victoryPanel.SetActive(false);
        ResumeGame();
    }

    public void RestartGame()
    {
        ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitToScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void ReduceTimerOnElimination(float reduction)
    {
        if (!gameEnded)
        {
            gameTimer = Mathf.Max(0, gameTimer - reduction);
        }
    }

    public void OnCharacterDeath(CharacterManager character)
    {
        ReduceTimerOnElimination(10f);

        if (RemainingCharacters() == 1)
        {
            EndGame(DetermineLastStanding());
        }
        else if (RemainingCharacters() == 4 && !suddenDeathMode && gameTimer > 60f)
        {
            ActivateSpecialMode();
        }
    }

    public void CheckPlayerCount(int playerCount)
    {
        if (playerCount == 4 && !inSpecialMode && gameTimer > 60f)
        {
            ActivateSpecialMode();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        DisableTimer();
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        EnableTimer();
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    void DisableTimer() => timerText.gameObject.SetActive(false);

    void EnableTimer() => timerText.gameObject.SetActive(true);

}
