using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoinManager : MonoBehaviour
{
    public GameObject[] presetPlayers; // 預設的披薩物件（Hierarchy 中先放好）
    public Transform[] spawnPoints2P;
    public Transform[] spawnPoints4P;

    private HashSet<int> joinedDevices = new HashSet<int>();
    private int playerCount = 0;

    [SerializeField] private int maxPlayers = 4;

    private void OnEnable()
    {
        if (PlayerInputManager.instance != null)
        {
            PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
        }
    }

    private void OnDisable()
    {
        if (PlayerInputManager.instance != null)
        {
            PlayerInputManager.instance.onPlayerJoined -= OnPlayerJoined;
        }
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        // 若裝置未綁定則拒絕
        if (playerInput.devices.Count == 0)
        {
            Debug.LogWarning("這個玩家沒有綁定任何輸入裝置！");
            Destroy(playerInput.gameObject);
            return;
        }

        int deviceId = playerInput.devices[0].deviceId;

        // 控制器是否已加入
        if (joinedDevices.Contains(deviceId))
        {
            Debug.LogWarning("這個控制器已經加入過了！");
            Destroy(playerInput.gameObject);
            return;
        }

        if (playerCount >= maxPlayers)
        {
            Debug.LogWarning("超出最大玩家數，拒絕加入！");
            Destroy(playerInput.gameObject);
            return;
        }

        // 出生點選擇邏輯
        Transform[] spawnArray = (maxPlayers <= 2) ? spawnPoints2P : spawnPoints4P;

        if (playerCount >= spawnArray.Length)
        {
            Debug.LogError("Spawn point 數量不足！");
            Destroy(playerInput.gameObject);
            return;
        }

        if (presetPlayers == null || presetPlayers.Length == 0 || presetPlayers[playerCount] == null)
        {
            Debug.LogError("預設披薩物件（presetPlayers）尚未設定！");
            Destroy(playerInput.gameObject);
            return;
        }

        // 創建小披薩玩家物件
        GameObject newPlayer = Instantiate(presetPlayers[playerCount], spawnArray[playerCount].position, spawnArray[playerCount].rotation);
        newPlayer.transform.SetParent(playerInput.transform, false); // 將 PlayerInput 作為新小披薩的父物件

        // 確保不重複添加 PlayerInput
        PlayerInput newPlayerInput = newPlayer.GetComponent<PlayerInput>();
        if (newPlayerInput == null)
        {
            newPlayerInput = newPlayer.AddComponent<PlayerInput>();
        }

       // newPlayerInput.defaultControlScheme = "Gamepad";
        newPlayerInput.actions = playerInput.actions; // 繼承原有的輸入配置

        // 更新玩家狀態
        joinedDevices.Add(deviceId);
        playerCount++;

        Debug.Log($"玩家 {playerInput.playerIndex + 1} 加入，位置：{spawnArray[playerCount - 1].position}");
    }
}
