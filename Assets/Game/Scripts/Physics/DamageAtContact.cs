using UnityEngine;

public class DamageAtContact : MonoBehaviour
{
    [SerializeField] private bool disappearAfterContact;

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            Debug.Log("danno al player");
        }
        if (disappearAfterContact) Disappear();
    }

    private void Disappear() {
        gameObject.SetActive(false);
    }

    
}
