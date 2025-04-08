using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ranking : MonoBehaviour
{
    public static Ranking Instance { get; private set; }

    [SerializeField] private List<(int dieHour, float hp, string name, int position)> rankingByTime = new();
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

    public void AddToRanking(int dieHour, float hp, string name)
    {
        rankingByTime.Add((dieHour, hp, name, 0));
        SortRankList();
    }

    void SortRankList()
    {
        rankingByTime = rankingByTime
            .OrderBy(player => player.dieHour)   // Ordina per tempo di morte (prima chi è morto prima)
            .ThenByDescending(player => player.hp) // Se il tempo è uguale, ordina per HP decrescente
            .ToList();

        int currentPosition = 1;
        int tiePosition = 1;


        for (int i = 0; i < rankingByTime.Count; i++)
        {
            Debug.Log("RANKING: " + rankingByTime[i]);
            bool condition = i > 0 &&
                    rankingByTime[i].dieHour == rankingByTime[i - 1].dieHour &&
                    rankingByTime[i].hp == rankingByTime[i - 1].hp;

            if (condition)
            {
                // Se anche gli HP sono uguali, manteniamo la stessa posizione
                rankingByTime[i] = (rankingByTime[i].dieHour, rankingByTime[i].hp, rankingByTime[i].name, tiePosition);
                tiePosition++;
            }
            else
            {
                // Aggiorna la posizione
                tiePosition = currentPosition;
                rankingByTime[i] = (rankingByTime[i].dieHour, rankingByTime[i].hp, rankingByTime[i].name, currentPosition);
                currentPosition++;
            }

        }
    }
}
