using NUnit.Framework;
using physics;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class FlussoGravitazionale : MonoBehaviour
{
    [SerializeField] private bool includePlayer = true;

    [SerializeField] private float forceIntensity;
    [SerializeField] private Vector3 forceDirection;

    [SerializeField] private float movementModifier;

    private List<GameObject> inFlowEntities = new List<GameObject>();

    private void Update() {
        forceDirection.Normalize();

        foreach (GameObject entity in inFlowEntities) {
            Debug.Log(entity.name + " " + forceDirection);
            entity.GetComponent<Rigidbody>().AddForce(forceDirection * forceIntensity);
            if (entity.CompareTag("Player"))
            {
                //
            }
            else
            {
                //
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            if (!includePlayer) return;
            CharacterPhysics character = other.gameObject.GetComponent<CharacterPhysics>();
            character.GenerateMovementModifier(gameObject, movementModifier);
            }
        inFlowEntities.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other) {
        if (inFlowEntities.Contains(other.gameObject)) {
            inFlowEntities.Remove(other.gameObject);
            
            if (other.gameObject.CompareTag("Player"))
            {
                CharacterPhysics character = other.gameObject.GetComponent<CharacterPhysics>();
                character.ClearMovementModifier(gameObject);
                return;
            }
        }
    }
}
