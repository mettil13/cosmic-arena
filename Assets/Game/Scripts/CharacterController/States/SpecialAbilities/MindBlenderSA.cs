using CharacterLogic;
using UnityEngine;
[CreateAssetMenu(fileName = "MindBlenderSA", menuName = "Scriptable Objects/PlayerState/SpecialAbilities/MindBlender")]
public class MindBlenderSA : APlayerState_SpecialAbility {
    [SerializeField] PlayerSpecialAbilityCooldown cooldown;
    [SerializeField] private float radius;
    [SerializeField] private GameObject prefabVFX;
    [SerializeField] private CopyMindBlenderSA copyMindBlenderSA;

    public override void OnEntry() {
        base.OnEntry();
        stateMachine.AddModifier(new MindBlenderSAModifier(characterManager, prefabVFX, copyMindBlenderSA));
    }

    public override void OnExit() {
        base.OnExit();
        stateMachine.AddModifier(cooldown);
    }
}
