using CharacterLogic;
using StateMachine;
using System.Collections.Generic;
using UnityEngine;

public class MindBlenderSAModifier : APlayerTimedModifier {
    private CharacterManager characterManager;
    private MindBlenderCopy mindBlenderCopy;
    private float damage, radius, copyDuration;
    private int copiesSpawned, maxNumberOfCopies;

    public override string Name => "MindBlenderSAModifier";

    public MindBlenderSAModifier(float duration, CharacterManager characterManager, MindBlenderCopy mindBlenderCopy, float damage, float radius, float copyDuration, int maxNumberOfCopies) : base(duration){
        this.characterManager = characterManager;
        this.mindBlenderCopy = mindBlenderCopy;
        this.damage = damage;
        this.radius = radius;
        this.copyDuration = copyDuration;
        this.maxNumberOfCopies = maxNumberOfCopies;
    }
    public override void OnEntry(StateMachine<Player_State, Player_Status> stateMachine) {
        base.OnEntry(stateMachine);        
    }
    public override void OnFixedUpdate() {
        base.OnFixedUpdate();
        Debug.Log(timer.ElapsedTime);
        if (timer.ElapsedTime > copiesSpawned && copiesSpawned < maxNumberOfCopies) {
            MindBlenderCopy instancedMindBlender = Object.Instantiate(mindBlenderCopy, characterManager.transform.position, characterManager.transform.rotation);
            instancedMindBlender.SetupCopy(characterManager, damage, radius, copyDuration);
            copiesSpawned++;
        }
    }

    public override void OnExit() {
        base.OnExit();
    }
}
