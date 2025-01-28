using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SalvatoreTempClasses
{
    public class GameManagerTemp : MonoBehaviour
    {
        public GameObject[] spawnPoints;
        public List<PlayerInput> playerList = new List<PlayerInput>();
        [SerializeField] InputAction joinAction;
        [SerializeField] InputAction leaveAction;

        //INSTANCES
        public static GameManagerTemp instance = null;

        //EVENTS
        public event System.Action<PlayerInput> OnPlayerJoinedGame;
        public event System.Action<PlayerInput> OnPlayerLeftGame;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != null)
                Destroy(gameObject);


            joinAction.Enable();
            joinAction.performed += context => JoinAction(context);
            leaveAction.Enable();
            leaveAction.performed += context => LeaveAction(context);
        }

        private void Start()
        {
            PlayerInputManager.instance.JoinPlayer(0,-1,null);
        }

        void OnPlayerJoined (PlayerInput playerInput)
        {
            playerList.Add(playerInput);

            Debug.Log("Player joined");
            if (OnPlayerJoinedGame != null) {
                OnPlayerJoinedGame(playerInput);
            }
        }

        void OnPlayerLeft(PlayerInput playerInput)
        {
            Debug.Log("Player left");
            if (OnPlayerLeftGame != null)
            {
                OnPlayerLeftGame(playerInput);
            }
        }

        void JoinAction(InputAction.CallbackContext context) { 
            PlayerInputManager.instance.JoinPlayerFromActionIfNotAlreadyJoined(context);
        }

        void LeaveAction(InputAction.CallbackContext context)
        {
            if (playerList.Count > 1)
            {
                foreach (var player in playerList)
                {
                    foreach (var device in player.devices)
                    {
                        if (device != null && context.control.device == device)
                        {
                            UnregisterPlayer(player);
                            return;
                        }

                    }
                }
            }
        }
        
        void UnregisterPlayer(PlayerInput playerInput)
        {
            playerList.Remove(playerInput);

            if (OnPlayerLeftGame != null)
            {
                OnPlayerLeftGame(playerInput);
            }

            Destroy(playerInput.transform.parent.gameObject);
        }

    }
}