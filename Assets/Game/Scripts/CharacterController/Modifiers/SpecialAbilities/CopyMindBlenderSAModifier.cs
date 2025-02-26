using CharacterLogic;
using StateMachine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CopyMindBlenderSAModifier : APlayerTimedModifier {
    private CharacterManager characterManager;
    private GameObject prefabVFX, VFX;
    private CharacterManager enemyHitted;
    private float damage;

    public override string Name => "Teleport";

    public CopyMindBlenderSAModifier(CharacterManager characterManager, float radius, float damage, GameObject prefabVFX) {
        this.characterManager = characterManager;
        this.prefabVFX = prefabVFX;
        this.damage = damage;
    }
    public override void OnEntry(StateMachine<Player_State, Player_Status> stateMachine) {
        base.OnEntry(stateMachine);
        
    }

    public override void OnFixedUpdate() {
        base.OnFixedUpdate();
        characterManager.characterInputAdapter.Thrust = 1;
        List<CharacterManager> hitted = new List<CharacterManager>();
        foreach (Collider hit in Physics.OverlapSphere(characterManager.transform.position, 3, characterManager.gameObject.layer)) {
            if (hit.TryGetComponent(out CharacterManager hittedCharacterManager) && characterManager != hittedCharacterManager) {
                hitted.Add(hittedCharacterManager);
            }
        }
        enemyHitted = null;
        float minDistance = 0;
        foreach (CharacterManager enemy in hitted) {
            float distance = (enemy.transform.position - characterManager.transform.position).sqrMagnitude;
            if (distance < minDistance) {
                minDistance = distance;
                enemyHitted = enemy;
            }
        }
        if (enemyHitted == null) return;

        timer.Stop();
        characterManager.transform.position = Vector3.Lerp(characterManager.transform.position, enemyHitted.transform.position, Time.deltaTime * 10);
        characterManager.transform.LookAt(enemyHitted.transform);

        if((characterManager.transform.position - enemyHitted.transform.position).magnitude < 0.1) {

            enemyHitted.stateMachine.AddModifier(new DamageModifier(enemyHitted, damage));
            timer.EndTimer();

        }

    }

    public override void OnExit() {
        Object.Destroy(characterManager.transform.gameObject);
        base.OnExit();
        
    }
}
