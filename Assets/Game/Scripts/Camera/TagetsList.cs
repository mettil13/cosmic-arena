using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "TargetsList", menuName = "Scriptable Objects/TargetsList")]
public class TargetsList : ScriptableObject
{
    [SerializeField] private List<Transform> targets = new();

    public List<Transform> Targets
    {
        get { return targets; }
        private set { targets = value; }
    }

    public void Clear()
    {
        targets.Clear();
    }

    public void Add(Transform t)
    {
        if (t != null && !targets.Contains(t))
        {
            targets.Add(t);
        }
    }

    public void Remove(Transform t)
    {
        if (t != null && targets.Contains(t))
        {
            targets.Remove(t);
        }
    }
}