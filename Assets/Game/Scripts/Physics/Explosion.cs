using physics;
using System;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public GameObject trigger;
    public float timeToDisappear = 1f;

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            trigger.SetActive(true);
            Destroy(gameObject, timeToDisappear);
        }
    }

}
