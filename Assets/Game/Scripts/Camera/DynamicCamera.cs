using UnityEngine;
using System.Collections.Generic;

public class DynamicCamera : MonoBehaviour
{
    [SerializeField] private List<Transform> allPotentialTargets;
    [SerializeField] private TargetsList currentTargets;
    [SerializeField] private float minHeight = 10f;
    [SerializeField] private float padding = 2f;
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private float maxTargetDistance = 50f;

    private Vector3 velocity = Vector3.zero;

    [SerializeField] private string playerTag = string.Empty;

    private void Start()
    {
        var gos = GameObject.FindGameObjectsWithTag(playerTag);
        foreach (GameObject t in gos)
        {
            allPotentialTargets.Add(t.transform);
        }
    }

    private void Update()
    {
        UpdateTargets();
    }

    void LateUpdate()
    {
        if (currentTargets == null || currentTargets.Targets.Count == 0)
            return;

        MoveCamera();
    }

    void MoveCamera()
    {
        Vector3 centerPoint = GetCenterPoint();

        float requiredHeight = GetGreatestDistance() + padding;
        requiredHeight = Mathf.Max(requiredHeight, minHeight);

        Vector3 newPosition = new Vector3(centerPoint.x, requiredHeight, centerPoint.z);

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);

        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }

    Vector3 GetCenterPoint()
    {
        if (currentTargets.Targets.Count == 1)
            return currentTargets.Targets[0].position;

        var bounds = new Bounds(currentTargets.Targets[0].position, Vector3.zero);
        foreach (Transform target in currentTargets.Targets)
        {
            bounds.Encapsulate(target.position);
        }

        return bounds.center;
    }

    float GetGreatestDistance()
    {
        var bounds = new Bounds(currentTargets.Targets[0].position, Vector3.zero);
        foreach (Transform target in currentTargets.Targets)
        {
            bounds.Encapsulate(target.position);
        }

        return Mathf.Max(bounds.size.x, bounds.size.z);
    }

    void UpdateTargets()
    {
        Vector3 massCenter = GetCenterPoint();

        currentTargets.Targets.Clear();

        foreach (Transform t in allPotentialTargets)
        {
            if (t != null && Vector3.Distance(t.position, massCenter) <= maxTargetDistance)
            {
                currentTargets.Add(t);
            }
        }
    }
}