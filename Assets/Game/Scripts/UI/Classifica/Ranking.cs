using System.Collections.Generic;
using UnityEngine;

public class Ranking : MonoBehaviour
{
    public static Ranking Instance {  get; private set; }

    [SerializeField] private List<string> ranking = new();
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private RankingCard card;


    private void Awake()
    {
        Instance = this;
    }
    private void OnEnable()
    {
        InstantiateCards();
    }

    public void InstantiateCards()
    {
        foreach (var player in ranking)
        {
            string index = ranking.IndexOf(player).ToString();
            RankingCard rc = Instantiate(card, spawnPoint);
            rc.SetParam(player, index);
        }
    }

    public void AddToRanking(string name)
    {
        ranking.Add(name);
    }

}
