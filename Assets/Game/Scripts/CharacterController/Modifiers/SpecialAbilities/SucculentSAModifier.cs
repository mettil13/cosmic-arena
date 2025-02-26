using CharacterLogic;
using StateMachine;
using System.Collections.Generic;
using UnityEngine;

public class SucculentSAModifier : APlayerTimedModifier {
    private CharacterManager characterManager;
    private float radius;
    private float damage;
    private float minDistance;
    private GameObject prefabVFX, VFX;
    private CharacterManager enemyHitted;
    public SucculentSAModifier(CharacterManager characterManager, float damage, GameObject prefabVFX) {
        this.characterManager = characterManager;
        this.damage = damage;
        this.prefabVFX = prefabVFX;
    }
    public override string Name => "SucculentSA";

    public override void OnEntry(StateMachine<Player_State, Player_Status> stateMachine) {
        base.OnEntry(stateMachine);
        List<CharacterManager> hitted = new List<CharacterManager>();
        foreach (Collider hit in Physics.OverlapSphere(characterManager.transform.position, radius, characterManager.gameObject.layer)) {
            if (hit.TryGetComponent(out CharacterManager hittedCharacterManager) && characterManager != hittedCharacterManager) {
                hitted.Add(hittedCharacterManager);
            }
        }
        enemyHitted = null;
        foreach (CharacterManager enemy in hitted) {            
            float distance = (enemy.transform.position - characterManager.transform.position).sqrMagnitude;
            if (distance < minDistance) {
                minDistance = distance;
                enemyHitted = enemy;
            }
        }

        if (enemyHitted == null) return;
        
        enemyHitted.stateMachine.AddModifier(new DamageModifier(enemyHitted, damage));

        VFX = Object.Instantiate(prefabVFX, characterManager.transform.position, Quaternion.identity);
    }
    public override void OnFixedUpdate() {
        base.OnFixedUpdate();
        
        VFX.transform.position = characterManager.transform.position;
        VFX.transform.LookAt(enemyHitted.transform.position);
        VFX.transform.localScale = new Vector3(1, 1, minDistance);
    }

    public override void OnExit() {
        base.OnExit();
        Object.Destroy(VFX);
    }

}
