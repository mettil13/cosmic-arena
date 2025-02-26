using UnityEngine;

public class DamageAtContact : MonoBehaviour
{
    [SerializeField] private bool disappearAfterContact;
    //[SerializeField] private CharacterPhysics characterPhysics;

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            //characterPhysics.Kick(transform.parent.transform, 50); ???
            Debug.Log("danno al player");
        }
        if (disappearAfterContact) Disappear();
    }

    private void Disappear() {
        gameObject.SetActive(false);
    }

    
}
