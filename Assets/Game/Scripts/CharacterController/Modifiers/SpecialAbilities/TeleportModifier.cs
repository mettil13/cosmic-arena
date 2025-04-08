using CharacterLogic;
using StateMachine;
using UnityEngine;
using UnityEngine.VFX;

public class TeleportModifier : APlayerTimedModifier {
    private CharacterManager characterManager;
    private float distance;
    private GameObject prefabVFX;
    private GameObject VFX;

    private bool tipped;

    public override string Name => "Teleport";

    public TeleportModifier(float duration, CharacterManager characterManager, float distance, GameObject prefabVFX) : base(duration) {
        this.characterManager = characterManager;
        this.distance = distance - 0.7f;
        this.prefabVFX = prefabVFX;
    }
    public override void OnEntry(StateMachine<Player_State, Player_Status> stateMachine) {        
        base.OnEntry(stateMachine);
        tipped = false;
        VFX = Object.Instantiate(prefabVFX, characterManager.transform.position + new Vector3(characterManager.characterInputAdapter.LastValidDirection.x, 0, characterManager.characterInputAdapter.LastValidDirection.y) * characterManager.transform.localScale.z * distance/2f, Quaternion.LookRotation(new Vector3(-characterManager.characterInputAdapter.LastValidDirection.x, 0, -characterManager.characterInputAdapter.LastValidDirection.y), Vector3.up));
        VFX.transform.localScale = new Vector3(VFX.transform.localScale.x * distance, VFX.transform.localScale.y, VFX.transform.localScale.z * distance);
        
    }

    public override void OnUpdate(ref float delta) {
        base.OnUpdate(ref delta);
        if (timer.ElapsedTime > .1f && !tipped) {
            if (Physics.Raycast(characterManager.transform.position, characterManager.transform.forward, out RaycastHit hit, distance, LayerMask.NameToLayer("Wall"))) {
                if (hit.distance - 0.7f < distance) {
                    distance = hit.distance;
                }
                characterManager.transform.position += characterManager.transform.forward * distance;
            }
            else {
                characterManager.transform.position += new Vector3(characterManager.characterInputAdapter.LastValidDirection.x, 0, characterManager.characterInputAdapter.LastValidDirection.y) * distance;
            }
            tipped = true;
        }
    }

    public override void OnExit() {
        base.OnExit();
        Object.Destroy(VFX);
    }
}

