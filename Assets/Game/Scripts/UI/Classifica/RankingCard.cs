using TMPro;
using UnityEngine;

public class RankingCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private TextMeshProUGUI cardRank;

    public void SetParam(string name, string rank)
    {
        cardName.text = name;
        cardRank.text = rank;
    }
}
