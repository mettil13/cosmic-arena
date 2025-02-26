using CharacterLogic;
using StateMachine;
using UnityEngine;

public class TeleportModifier : APlayerTimedModifier {
    private CharacterManager characterManager;
    private float distance;
    private GameObject prefabVFXenter, prefabVFXexit;
    private GameObject VFXenter, VFXexit;

    public override string Name => "Teleport";

    public TeleportModifier(CharacterManager characterManager, float distance, GameObject prefabVFXenter, GameObject prefabVFXexit) {
        this.characterManager = characterManager;
        this.distance = distance - 0.7f;

    }
    public override void OnEntry(StateMachine<Player_State, Player_Status> stateMachine) {
        base.OnEntry(stateMachine);
        if (Physics.Raycast(characterManager.transform.position, characterManager.transform.forward, out RaycastHit hit, distance, LayerMask.NameToLayer("Wall"))) {
            if(hit.distance - 0.7f < distance) {
                distance = hit.distance;
            }
            characterManager.transform.position += characterManager.transform.forward * distance;
        }
        VFXenter = Object.Instantiate(prefabVFXenter, characterManager.transform.position, characterManager.transform.rotation);
        VFXexit = Object.Instantiate(prefabVFXexit, characterManager.transform.position, characterManager.transform.rotation);
    }

    public override void OnExit() {
        base.OnExit();
        Object.Destroy(VFXenter);
        Object.Destroy(VFXexit);
    }
}

