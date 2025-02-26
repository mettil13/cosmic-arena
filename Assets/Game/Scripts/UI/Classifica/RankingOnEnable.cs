using UnityEngine;

public class RankingOnEnable : MonoBehaviour
{
    private void OnEnable()
    {
        Ranking.Instance.InstantiateCards();
    }  
}
