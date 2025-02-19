using physics;
using System;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public Collider triggerCollider;
    public float timeToDisappear = 1f;

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) { // anche "oggetti"
            triggerCollider.enabled = true;
            Destroy(gameObject, timeToDisappear);
        }
    }

}
