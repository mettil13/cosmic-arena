using CharacterLogic;
using StateMachine;
using System.Collections.Generic;
using UnityEngine;

public class MindBlenderSAModifier : APlayerTimedModifier {
    private CharacterManager characterManager;
    private CopyMindBlenderSA copyMindBlenderSA;

    public override string Name => "MindBlenderSAModifier";

    public MindBlenderSAModifier(float duration, CharacterManager characterManager, CopyMindBlenderSA copyMindBlenderSA) : base(duration){
        this.characterManager = characterManager;
        this.copyMindBlenderSA = copyMindBlenderSA;
    }
    public override void OnEntry(StateMachine<Player_State, Player_Status> stateMachine) {
        base.OnEntry(stateMachine);
        List<CharacterManager> copies = new List<CharacterManager> { };
        int maxNumberOfCopies = 4;
        int copiesSpawned = 0;
        if (timer.ElapsedTime % 0.5 < 0.1) {
            if(copiesSpawned < maxNumberOfCopies) {
                copies.Add(Object.Instantiate(characterManager, characterManager.transform.position, characterManager.transform.rotation));
                copiesSpawned++;
            }
        }
        foreach(CharacterManager copy in copies) {            
            copy.stateMachine.states[Player_State.SpecialAbility] = copyMindBlenderSA;
            copy.stateMachine.ChangeState(Player_State.SpecialAbility);
        }
    }

    public override void OnExit() {
        base.OnExit();
    }
}
