using physics;
using UnityEngine;

public class SwerveAtContact : MonoBehaviour
{
    [SerializeField] private CharacterPhysics characterPhysics;
    public float intensity = 50;
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            collision.gameObject.GetComponent<CharacterPhysics>().Kick(transform.parent.transform, intensity);
        }
    }
}
