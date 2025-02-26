using System.Collections;
using UnityEngine;

public class Elettroshock : MonoBehaviour
{
    public GameObject trigger;
    public float onTime = 1.0f;
    public float offTime = 3.0f;

    private void Start() {
        StartCoroutine(Intermittence());
    }

    private IEnumerator Intermittence() {
        while (true) {
            trigger.SetActive(true);
            yield return new WaitForSeconds(onTime);
            trigger.SetActive(false);
            yield return new WaitForSeconds(offTime);
        }
    }
}
