using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ranking : MonoBehaviour
{
    public static Ranking Instance { get; private set; }

    [SerializeField] private List<(int dieHour, string name, int position)> rankingByTime = new();
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
            rc.SetValues(player.name, player.position);
        }
    }

    public void AddToRanking(int dieHour, string name)
    {
        rankingByTime.Add((dieHour, name, 0));
        SortRankList();
    }

    void SortRankList()
    {
        rankingByTime = rankingByTime
            .OrderBy(player => player.dieHour)
            .ToList();

        int currentPosition = 1;
        int tiePosition = 1;

        for (int i = 0; i < rankingByTime.Count; i++)
        {
            if (i > 0 && rankingByTime[i].dieHour == rankingByTime[i - 1].dieHour)
            {
                rankingByTime[i] = (rankingByTime[i].dieHour, rankingByTime[i].name, tiePosition);
            }
            else
            {
                tiePosition = currentPosition;
                rankingByTime[i] = (rankingByTime[i].dieHour, rankingByTime[i].name, currentPosition);
            }

            currentPosition++;
        }
    }
}
