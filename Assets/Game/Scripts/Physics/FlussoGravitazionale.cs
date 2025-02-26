using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class FlussoGravitazionale : MonoBehaviour
{
    [SerializeField] private bool includePlayer = true;
    private List<GameObject> inFlowEntities = new List<GameObject>();

    private void Update() {
        foreach (var entity in inFlowEntities) {

        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!includePlayer) {
            if (other.gameObject.CompareTag("Player")) {
                return;
            }
            else {
                inFlowEntities.Add(other.gameObject);
            }
        }
        inFlowEntities.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other) {
        if (inFlowEntities.Contains(other.gameObject)) {
            inFlowEntities.Remove(other.gameObject);
        }
    }
}
