using CharacterLogic;
using UnityEngine;
[CreateAssetMenu(fileName = "MrMarvelousSA", menuName = "Scriptable Objects/PlayerState/SpecialAbilities/MrMarvelous")]
public class MrMarvelousSA : APlayerState_SpecialAbility {
    [SerializeField] private float radius;
    [SerializeField] private GameObject prefabVFX;

    public override void OnEntry() {
        base.OnEntry();
        Debug.Log("MrMarvelousSA Entry");
        stateMachine.AddModifier(new MrMarvelousSAModifier(1, characterManager, radius, prefabVFX));
        stateMachine.ChangeState(Player_State.Idle);
    }

    public override void OnExit() {
        base.OnExit();
    }
}
