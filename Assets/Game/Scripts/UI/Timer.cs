using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public GameManager gameManager;
    public TextMeshProUGUI timerText;

    void Start()
    {
        if (gameManager != null)
        {
            timerText.text = FormatTime(gameManager.gameTimer);
        }
    }

    void Update()
    {
        if (gameManager != null && !gameManager.gameObject.GetComponent<GameManager>().Equals(null))
        {
            timerText.text = FormatTime(gameManager.gameTimer);
        }
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return $"{minutes:00}:{seconds:00}";
    }
}
