using UnityEngine;
using CharacterLogic;
using System.Collections.Generic;
using StateMachine;

public class MrMarvelousSAModifier : APlayerTimedModifier {
    private CharacterManager characterManager;
    private float radius;
    private List<CharacterManager> lastHitted = new List<CharacterManager>();
    private GameObject prefabVFX, VFX;
    public MrMarvelousSAModifier(float duration, CharacterManager characterManager, float radius, GameObject prefabVFX) : base(duration){
        this.characterManager = characterManager;
        this.radius = radius;
        this.prefabVFX = prefabVFX;
    }
    public override string Name => "MrMarvelousSA";

    public override void OnEntry(StateMachine<Player_State, Player_Status> stateMachine) {
        base.OnEntry(stateMachine);
        VFX = Object.Instantiate(prefabVFX, characterManager.transform.position, prefabVFX.transform.rotation);
    }
    public override void OnFixedUpdate() {
        base.OnFixedUpdate();
        List<CharacterManager> hitted = new List<CharacterManager>();
        foreach (Collider hit in Physics.OverlapSphere(characterManager.transform.position, radius, characterManager.gameObject.layer)) {
            if (hit.TryGetComponent(out CharacterManager hittedCharacterManager) && characterManager != hittedCharacterManager) {
                hitted.Add(hittedCharacterManager);

                Vector2 direction = new Vector2(hit.transform.position.x - characterManager.transform.position.x, hit.transform.position.z - characterManager.transform.position.z).normalized;
                hittedCharacterManager.stateMachine.AddModifier(new CharmedModifier(1, hittedCharacterManager, direction));
            }
        }
        foreach (CharacterManager characterManager in lastHitted) {
            if (!hitted.Contains(characterManager)) {
                characterManager.stateMachine.RemoveModifier("Charm");
            }
        }
        lastHitted = hitted;

        VFX.transform.position = characterManager.transform.position;
    }

    public override void OnExit() {
        base.OnExit();
        Object.Destroy(VFX);
    }

}
