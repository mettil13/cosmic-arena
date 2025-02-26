using physics;
using UnityEngine;
using UnityEngine.VFX;

public class TriggerDamager : MonoBehaviour
{
    public VisualEffect vfxGO;
    [SerializeField] private float intensity = 100;

    private void OnEnable() {
        vfxGO.Play();
    }

    private void OnDisable() {
        vfxGO.Stop();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            Debug.Log("danno");
            other.GetComponent<CharacterPhysics>().Kick(transform.parent.transform, intensity);
        }
    }
}
