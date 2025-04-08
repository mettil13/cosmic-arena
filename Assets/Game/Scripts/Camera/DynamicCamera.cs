using UnityEngine;
using System.Collections.Generic;

public class DynamicCamera : MonoBehaviour
{
    public static DynamicCamera Instance;

    [SerializeField] private List<Transform> allPotentialTargets = new List<Transform>();
    [SerializeField] private TargetsList currentTargets;
    [SerializeField] private float minHeight = 10f;
    [SerializeField] private float padding = 2f;
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private float maxTargetDistance = 50f;

    private Vector3 velocity = Vector3.zero;

    public TargetsList CurrentTargets => currentTargets;

    [SerializeField] private string playerTag = string.Empty;

    private void Awake()
    {
        Instance = this;
        Init();
    }

    private void Start()
    {
        UpdatePlayer();
    }

    private void Update()
    {
        CleanUpTargetsList(); // Rimuove eventuali riferimenti nulli e aggiorna i target
        UpdatePlayer();
    }

    void LateUpdate()
    {
        if (currentTargets == null || currentTargets.Targets.Count == 0)
            return;

        MoveCamera();
    }

    public void Init()
    {
        //GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");
        CurrentTargets.Clear();
        //foreach (GameObject target in targets)
        //{
        //    currentTargets.Add(target.transform);
        //}
    }

    public void AddTarget(GameObject player)
    {
        currentTargets.Add(player.transform);
        //allPotentialTargets.Add(player.transform);
    }

    void MoveCamera()
    {
        Vector3 centerPoint = GetCenterPoint();
        float requiredHeight = Mathf.Max(GetGreatestDistance() + padding, minHeight);
        Vector3 newPosition = new Vector3(centerPoint.x, requiredHeight, centerPoint.z);
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }

    Vector3 GetCenterPoint()
    {
        if (currentTargets == null || currentTargets.Targets.Count == 0)
        {
            return transform.position;
        }

        if (currentTargets.Targets.Count == 1)
            return currentTargets.Targets[0].position;

        var bounds = new Bounds(currentTargets.Targets[0].position, Vector3.zero);
        foreach (Transform target in currentTargets.Targets)
        {
            if (target != null)
                bounds.Encapsulate(target.position);
        }

        return bounds.center;
    }

    float GetGreatestDistance()
    {
        if (currentTargets == null || currentTargets.Targets.Count == 0)
        {
            return 0f;
        }

        var bounds = new Bounds(currentTargets.Targets[0].position, Vector3.zero);
        foreach (Transform target in currentTargets.Targets)
        {
            if (target != null)
                bounds.Encapsulate(target.position);
        }

        return Mathf.Max(bounds.size.x, bounds.size.z);
    }

    void UpdatePlayer()
    {
        var gos = GameObject.FindGameObjectsWithTag(playerTag);
        foreach (GameObject t in gos)
        {
            if (t != null && !allPotentialTargets.Contains(t.transform))
            {
                allPotentialTargets.Add(t.transform);
            }
        }
        UpdateTargets();
    }

    public void UpdateTargets()
    {
        Vector3 massCenter = GetCenterPoint();
        List<Transform> toRemove = new List<Transform>();

        foreach (Transform t in currentTargets.Targets)
        {
            if (t == null || Vector3.Distance(t.position, massCenter) > maxTargetDistance)
            {
                toRemove.Add(t);
            }
        }

        foreach (Transform t in toRemove)
        {
            currentTargets.Remove(t);
        }

        foreach (Transform t in allPotentialTargets)
        {
            if (t != null && Vector3.Distance(t.position, massCenter) <= maxTargetDistance && !currentTargets.Targets.Contains(t))
            {
                currentTargets.Add(t);
            }
        }

        currentTargets.Targets.RemoveAll(target => target == null);

    }

    private void CleanUpTargetsList()
    {
        UpdateTargets(); // Assicura che la lista dei target sia aggiornata
        allPotentialTargets.RemoveAll(target => target == null);
    }
}
