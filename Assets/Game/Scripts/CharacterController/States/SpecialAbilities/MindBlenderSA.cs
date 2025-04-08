using CharacterLogic;
using UnityEngine;
[CreateAssetMenu(fileName = "MindBlenderSA", menuName = "Scriptable Objects/PlayerState/SpecialAbilities/MindBlender")]
public class MindBlenderSA : APlayerState_SpecialAbility {
    [SerializeField] private float radius;
    [SerializeField] private CopyMindBlenderSA copyMindBlenderSA;

    public override void OnEntry() {
        base.OnEntry();
        stateMachine.AddModifier(new MindBlenderSAModifier(1, characterManager, copyMindBlenderSA));
        stateMachine.ChangeState(Player_State.Idle);
    }

    public override void OnExit() {
        base.OnExit();
    }
}
