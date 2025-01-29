using UnityEngine;

public class Trigger : MonoBehaviour
{
    private Collider trigger;

    private void Awake() {
        trigger = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other) {
        
    }
}
