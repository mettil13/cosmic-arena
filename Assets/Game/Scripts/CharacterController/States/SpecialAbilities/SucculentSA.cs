using CharacterLogic;
using UnityEngine;
[CreateAssetMenu(fileName = "SucculentSA", menuName = "Scriptable Objects/PlayerState/SpecialAbilities/Succulent")]
public class SucculentSA : APlayerState_SpecialAbility {
    [SerializeField] PlayerSpecialAbilityCooldown cooldown;
    [SerializeField] private float radius;
    [SerializeField] private GameObject prefabVFX;

    public override void OnEntry() {
        base.OnEntry();
        stateMachine.AddModifier(new SucculentSAModifier(characterManager, radius, prefabVFX));
    }

    public override void OnExit() {
        base.OnExit();
        stateMachine.AddModifier(cooldown);
    }
}
