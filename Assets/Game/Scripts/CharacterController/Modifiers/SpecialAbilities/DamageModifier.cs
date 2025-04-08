using CharacterLogic;
using StateMachine;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class DamageModifier : APlayerTimedModifier {
    private CharacterManager characterManager;
    private CharacterManager caster;
    private float damage;

    public override string Name => "Damage";

    public DamageModifier(float duration, CharacterManager characterManager, float damage) : base(duration) {
        this.characterManager = characterManager;
        this.damage = damage;
    }
    public override void OnEntry(StateMachine<Player_State, Player_Status> stateMachine) {
        base.OnEntry(stateMachine);
        //prendi danno
        
    }

    public override void OnExit() {
        base.OnExit();
    }
}
