using CharacterLogic;
using StateMachine;
using UnityEngine;

public class CharmedModifier : APlayerTimedModifier {
    private CharacterManager characterManager;
    private Vector2 direction;

    public override string Name => "Charmed";

    public CharmedModifier(float duration, CharacterManager characterManager, Vector2 direction) : base(duration){
        this.characterManager = characterManager;
        this.direction = direction;
    }

    public override void OnEntry(StateMachine<Player_State, Player_Status> stateMachine) {
        base.OnEntry(stateMachine);
    }

    public override void OnFixedUpdate() {
        base.OnFixedUpdate();

        characterManager.characterInputAdapter.Thrust = 1;
        characterManager.characterInputAdapter.Direction = direction;
    }

    public override void OnExit() {
        base.OnExit();
    }
}
