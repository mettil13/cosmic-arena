using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{

    public float gameDuration = 300f;
    public TextMeshProUGUI timerText;
    private bool suddenDeathMode = false;

    void Update()
    {
        if (gameDuration > 0)
        {
            gameDuration -= Time.deltaTime;
            UpdateTimerDisplay();
        }
        else
        {
            if (!suddenDeathMode)
            {
                ActivateSuddenDeath();
            }
            else
            {
                //EndGameWithLifeCheck();
            }
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(gameDuration / 60);
        int seconds = Mathf.FloorToInt(gameDuration % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void CheckPlayerCount(int playerCount)
    {
        if (playerCount == 4 && !suddenDeathMode && gameDuration > 60f)
        {
            ActivateSuddenDeath();
        }
    }

    void ActivateSuddenDeath()
    {
        suddenDeathMode = true;
        gameDuration = 60f;
        Debug.Log("Modalità Sudden Death attivata!");
    }


    //ipotesi bislacca
    //void EndGameWithLifeCheck()
    //{
    //    Debug.Log("Tempo scaduto. Controllo delle vite...");
    //    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
    //    GameObject winner = null;
    //    float maxLife = 0f;

    //    foreach (GameObject player in players)
    //    {
    //        PlayerStats stats = player.GetComponent<PlayerStats>();
    //        if (stats != null && stats.currentLife > maxLife)
    //        {
    //            maxLife = stats.currentLife;
    //            winner = player;
    //        }
    //    }

    //    if (winner != null)
    //    {
    //        Debug.Log($"Il vincitore è: {winner.name} con {maxLife} punti vita!");
    //    }
    //}
}
