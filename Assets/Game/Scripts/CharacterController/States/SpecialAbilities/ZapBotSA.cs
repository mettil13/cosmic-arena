using CharacterLogic;
using UnityEngine;
[CreateAssetMenu(fileName = "ZapBotSA", menuName = "Scriptable Objects/PlayerState/SpecialAbilities/ZapBot")]
public class ZapBotSA : APlayerState_SpecialAbility {
    [SerializeField] private float distance;
    [SerializeField] private GameObject prefabVFXenter;

    public override void OnEntry() {
        base.OnEntry();
        Debug.Log("ZapBotSA Entry");
        stateMachine.AddModifier(new TeleportModifier(1, characterManager, distance, prefabVFXenter));
        stateMachine.ChangeState(Player_State.Idle);
    }

    public override void OnExit() {
        base.OnExit();
    }
}
