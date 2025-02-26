using System.Collections.Generic;
using System.Linq; // Para usar métodos como OrderBy
using UnityEngine;

public class Ranking : MonoBehaviour
{
    public static Ranking Instance { get; private set; }

    [SerializeField] private List<(int dieHour, string name)> rankingByTime = new();
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private RankingCard card;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void InstantiateCards()
    {
        foreach (Transform child in spawnPoint)
        {
            Destroy(child.gameObject);
        }

        foreach (var player in rankingByTime)
        {
            RankingCard rc = Instantiate(card, spawnPoint);
            rc.SetValues(player.name, player.dieHour);
        }
    }

    public void AddToRanking(int dieHour, string name)
    {
        rankingByTime.Add((dieHour, name));
        SortRankList();
    }

    void SortRankList()
    {
        rankingByTime = rankingByTime.OrderBy(player => player.dieHour).ToList();
    }
}
