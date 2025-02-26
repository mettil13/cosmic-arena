using CharacterLogic;
using UnityEngine;
[CreateAssetMenu(fileName = "MrMarvelousSA", menuName = "Scriptable Objects/PlayerState/SpecialAbilities/MrMarvelous")]
public class MrMarvelousSA : APlayerState_SpecialAbility {
    [SerializeField] PlayerSpecialAbilityCooldown cooldown;
    [SerializeField] private float radius;
    [SerializeField] private GameObject prefabVFX;

    public override void OnEntry() {
        base.OnEntry();
        stateMachine.AddModifier(new MrMarvelousSAModifier(characterManager, radius, prefabVFX));
    }

    public override void OnExit() {
        base.OnExit();
        stateMachine.AddModifier(cooldown);
    }
}
