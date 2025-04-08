using CharacterLogic;
using UnityEngine;
[CreateAssetMenu(fileName = "MindBlenderSA", menuName = "Scriptable Objects/PlayerState/SpecialAbilities/MindBlender")]
public class MindBlenderSA : APlayerState_SpecialAbility {
    [SerializeField] private MindBlenderCopy mindBlenderCopy;
    [SerializeField] private float abilityDuration, radius, damage, copyDuration;
    [SerializeField] private int maxNumberOfCopies;

    public override void OnEntry() {
        base.OnEntry();
        Debug.Log("MindBlender Entry");
        stateMachine.AddModifier(new MindBlenderSAModifier(abilityDuration, characterManager, mindBlenderCopy, damage, radius, copyDuration, maxNumberOfCopies));
        stateMachine.ChangeState(Player_State.Idle);
    }

    public override void OnExit() {
        base.OnExit();
    }
}
