using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class MeteoriteGeneration : MonoBehaviour
{
    [SerializeField] private GameObject colliderToActivateOnImpact;
    [SerializeField] private GameObject meteoritePrefab;

    [Header("Meteorite travel")]
    [SerializeField] private float meteoriteTravelTime = 1;
    [SerializeField] private float activationTimeAfterExplosion;
    [SerializeField, Range(0, 1)] private float meteoriteImpactTime = 1;
    [SerializeField] private float meteoriteStartDistance;
    [SerializeField] private Ease meteoriteEase;
    private Tween meteoriteTween;
    private GameObject generatedMeteorite;

    [Header("Signal settings")]
    [SerializeField] private GameObject meteoriteImpactSignal;
    [SerializeField] private float maxSignalSize;
    [SerializeField] private float minSignalSize;
    private float sizeDifference;
    private bool generated = false;

    private void Awake()
    {
        GenerateMeteorite(transform.position);
    }
    /// <summary>
    /// meteorite generation
    /// </summary>
    /// <param name="targetPosition">position where the meteorite is going to axplode</param>
    public void GenerateMeteorite(Vector3 targetPosition)
    {
        if (generated) return;
        generated = true;
        sizeDifference = maxSignalSize - minSignalSize;
        transform.position = targetPosition;
        generatedMeteorite = GameObject.Instantiate(meteoritePrefab, transform);
        generatedMeteorite.transform.localPosition = Vector3.up * meteoriteStartDistance;
        generatedMeteorite.transform.localScale = Vector3.one * maxSignalSize; 
        meteoriteImpactSignal.SetActive(true);
        meteoriteImpactSignal.transform.localScale = new Vector3 (minSignalSize, 1, minSignalSize);

        meteoriteTween = DOVirtual.Float(0, 1, meteoriteTravelTime, (f) => OnMeteoritePositionUpdate(f));
        meteoriteTween.SetEase(meteoriteEase);
        meteoriteTween.onComplete += OnMeteoriteImpact;
    }



    /// <summary>
    /// meteorite update for tween
    /// </summary>
    /// <param name="time">time 0 to 1 of the tween</param>
    public void OnMeteoritePositionUpdate(float time)
    {
        // distance is always y positive
        float distanceFromPoint = 1 - time;
        generatedMeteorite.transform.localPosition = Vector3.up * meteoriteStartDistance * distanceFromPoint;
        float signalSizeModifier = sizeDifference * time;
        meteoriteImpactSignal.transform.localScale = new Vector3 (minSignalSize + signalSizeModifier, 1, minSignalSize + signalSizeModifier);
        if (meteoriteImpactTime < time)
            meteoriteImpactSignal.SetActive(false);
    }

    /// <summary>
    /// meteorite on finish for tween
    /// </summary>
    public void OnMeteoriteImpact()
    {
        colliderToActivateOnImpact.SetActive(true);
        Destroy(gameObject, activationTimeAfterExplosion);
    }
}
