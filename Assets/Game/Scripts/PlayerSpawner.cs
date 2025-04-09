using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints = new Transform[6];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DevicesManager.instance.SpawnPlayers(spawnPoints);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
