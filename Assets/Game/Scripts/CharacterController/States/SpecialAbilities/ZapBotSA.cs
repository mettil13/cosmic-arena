using CharacterLogic;
using UnityEngine;
[CreateAssetMenu(fileName = "ZapBotSA", menuName = "Scriptable Objects/PlayerState/SpecialAbilities/ZapBot")]
public class ZapBotSA : APlayerState_SpecialAbility {
    [SerializeField] PlayerSpecialAbilityCooldown cooldown;
    [SerializeField] private float distance;
    [SerializeField] private GameObject prefabVFXenter, prefabVFXexit;

    public override void OnEntry() {
        base.OnEntry();
        stateMachine.AddModifier(new TeleportModifier(characterManager, distance, prefabVFXenter, prefabVFXexit));
    }

    public override void OnExit() {
        base.OnExit();
        stateMachine.AddModifier(cooldown);
    }
}
