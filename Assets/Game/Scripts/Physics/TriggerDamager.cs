using physics;
using UnityEngine;
using UnityEngine.VFX;

public class TriggerDamager : MonoBehaviour
{
    [SerializeField] private CharacterPhysics characterPhysics;
    public VisualEffect vfxGO;

    private void OnEnable() {
        vfxGO.Play();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            Debug.Log("danno");
            characterPhysics.Kick(transform.parent.transform, 100);
        }
    }
}
