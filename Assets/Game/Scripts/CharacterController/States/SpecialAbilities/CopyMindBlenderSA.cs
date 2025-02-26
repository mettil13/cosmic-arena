using CharacterLogic;
using UnityEngine;
[CreateAssetMenu(fileName = "CopyMindBlenderSA", menuName = "Scriptable Objects/PlayerState/SpecialAbilities/CopyMindBlender")]
public class CopyMindBlenderSA : APlayerState_SpecialAbility {
    [SerializeField] PlayerSpecialAbilityCooldown cooldown;
    [SerializeField] private GameObject prefabVFX;
    [SerializeField] private float damage, radius;
    public override void OnEntry() {
        base.OnEntry();
        stateMachine.AddModifier(new CopyMindBlenderSAModifier(characterManager, radius, damage, prefabVFX));
    }

    public override void OnExit() {
        base.OnExit();
        stateMachine.AddModifier(cooldown);
    }
}
