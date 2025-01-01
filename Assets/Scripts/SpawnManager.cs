using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints; // Assign spawn points in the Inspector
    private int currentSpawnIndex = 0;

    // Dictionary to track spawned players
    private Dictionary<ulong, GameObject> spawnedPlayers = new Dictionary<ulong, GameObject>();

    private void Start()
    {
        // Subscribe to client connection and disconnection events
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
    }

    private void OnClientConnected(ulong clientId)
{
    if (!NetworkManager.Singleton.IsServer) return; // Only the server spawns players

    if (!spawnedPlayers.ContainsKey(clientId))
    {
        SpawnPlayer(clientId);
    }
    else
    {
        Debug.LogWarning($"Client {clientId} already has a spawned player.");
    }
}


    private void OnClientDisconnected(ulong clientId)
    {
        if (spawnedPlayers.ContainsKey(clientId))
        {
            Destroy(spawnedPlayers[clientId]); // Destroy the player's car on disconnect
            spawnedPlayers.Remove(clientId);
            Debug.Log($"Player {clientId} disconnected, car removed.");
        }
    }

    private void SpawnPlayer(ulong clientId)
    {
        if (currentSpawnIndex >= spawnPoints.Length)
        {
            Debug.LogError("No available spawn points. Add more spawn points to the array.");
            return;
        }

        // Get the next available spawn point
        Transform spawnPoint = spawnPoints[currentSpawnIndex];
        currentSpawnIndex = (currentSpawnIndex + 1) % spawnPoints.Length;

        Debug.Log($"Spawning player {clientId} at spawn point {spawnPoint.position}");

        // Instantiate the player prefab
        GameObject playerCar = Instantiate(NetworkManager.Singleton.NetworkConfig.PlayerPrefab, spawnPoint.position, spawnPoint.rotation);

        // Assign ownership and spawn the object on the network
        NetworkObject networkObject = playerCar.GetComponent<NetworkObject>();
        if (networkObject != null)
        {
            networkObject.SpawnAsPlayerObject(clientId); // Assign ownership to the client
            spawnedPlayers[clientId] = playerCar; // Track the spawned car
            Debug.Log($"Player {clientId} successfully spawned at {spawnPoint.position}");
        }
        else
        {
            Debug.LogError("The player prefab is missing a NetworkObject component!");
        }
    }
}
