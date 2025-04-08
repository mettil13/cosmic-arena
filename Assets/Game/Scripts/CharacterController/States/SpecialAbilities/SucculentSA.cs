using CharacterLogic;
using UnityEngine;
[CreateAssetMenu(fileName = "SucculentSA", menuName = "Scriptable Objects/PlayerState/SpecialAbilities/Succulent")]
public class SucculentSA : APlayerState_SpecialAbility {
    [SerializeField] private float radius;
    [SerializeField] private GameObject prefabVFX;

    public override void OnEntry() {
        base.OnEntry();
        stateMachine.AddModifier(new SucculentSAModifier(1, characterManager, radius, prefabVFX));
        stateMachine.ChangeState(Player_State.Idle);
    }

    public override void OnExit() {
        base.OnExit();
    }
}
