using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankingCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dieHourText;

    public void SetValues(string playerName, int dieHour)
    {
        nameText.text = playerName;
        dieHourText.text = dieHour.ToString();
    }
}
