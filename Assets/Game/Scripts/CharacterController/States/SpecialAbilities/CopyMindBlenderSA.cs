using CharacterLogic;
using UnityEngine;
[CreateAssetMenu(fileName = "CopyMindBlenderSA", menuName = "Scriptable Objects/PlayerState/SpecialAbilities/CopyMindBlender")]
public class CopyMindBlenderSA : APlayerState_SpecialAbility {
    [SerializeField] private GameObject prefabVFX;
    [SerializeField] private float damage, radius;
    public override void OnEntry() {
        base.OnEntry();
        stateMachine.AddModifier(new CopyMindBlenderSAModifier(1, characterManager, radius, damage, prefabVFX));
        stateMachine.ChangeState(Player_State.Idle);
    }

    public override void OnExit() {
        base.OnExit();
    }
}
