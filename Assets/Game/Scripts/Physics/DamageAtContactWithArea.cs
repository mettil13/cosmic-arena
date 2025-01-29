using System;
using UnityEngine;

public class DamageAtContactWithArea : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) { // anche oggetti
            
        }
    }

}
