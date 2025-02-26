using physics;
using UnityEngine;

public class SwerveAtContact : MonoBehaviour
{
    [SerializeField] private CharacterPhysics characterPhysics;
    private void OnCollisionEnter(Collision collision) {
        characterPhysics.Kick(transform.parent.transform, 50);
    }
}
