using System.Collections;
using UnityEngine;

public class Elettroshock : MonoBehaviour
{
    public Collider triggerCollider;
    public float onTime = 1.0f;
    public float offTime = 3.0f;

    private void Start() {
        StartCoroutine(Intermittence());
    }

    private IEnumerator Intermittence() {
        while (true) {
            triggerCollider.enabled = true;
            yield return new WaitForSeconds(onTime);
            triggerCollider.enabled = false;
            yield return new WaitForSeconds(offTime);
        }
    }
}
