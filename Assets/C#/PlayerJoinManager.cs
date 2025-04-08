using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoinManager : MonoBehaviour
{
    public GameObject[] playerPrefabs; // ぃ┸履家 Prefab 皚
    public Transform[] spawnPoints2P; // 2P 家Αネ翴
    public Transform[] spawnPoints4P; // 4P 家Αネ翴

    private int playerCount = 0; // 讽玡產计秖

    private void Start()
    {
        PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        playerCount++;

        // 匡拒ネ翴
        Transform spawnPoint = (playerCount <= 2) ? spawnPoints2P[playerCount - 1] : spawnPoints4P[playerCount - 1];

        // 絋玂Τì镑家玥箇砞材
        GameObject selectedPrefab = playerPrefabs[playerCount - 1 % playerPrefabs.Length];

        // ネΘぃ家產
        GameObject newPlayer = Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);
        playerInput.transform.SetParent(newPlayer.transform, false);

        Debug.Log($"產 {playerInput.playerIndex + 1} ㄏノ {selectedPrefab.name}竚{spawnPoint.position}");
    }
}
