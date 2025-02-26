using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Stars : MonoBehaviour
{
    public float minAlpha = 0.3f;
    public float maxAlpha = 1f;
    public float duration = 1.5f;

    private RawImage image;

    void Start() {
        image = GetComponent<RawImage>();
        StartCoroutine(Twinkle());
    }

    private IEnumerator Twinkle() {
        while (true) {
            yield return LerpAlpha(minAlpha, maxAlpha, duration / 2);
            yield return LerpAlpha(maxAlpha, minAlpha, duration / 2);
        }
    }

    private IEnumerator LerpAlpha(float from, float to, float time) {
        float elapsed = 0f;
        Color color = image.color;

        while (elapsed < time) {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(from, to, elapsed / time);
            image.color = color;
            yield return null;
        }

        color.a = to;
        image.color = color;
    }
}
