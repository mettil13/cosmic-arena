using UnityEngine;
using StateMachine;

namespace CharacterLogic
{
    public class PlayerState_ASpecialAbility : APlayerState
    {
        [SerializeField] PlayerSpecialAbilityCooldown cooldown;

    }

}
