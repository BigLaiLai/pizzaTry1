using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionManager : MonoBehaviour
{
    public static CharacterSelectionManager Instance;

    // 存儲披薩模型的預製體
    public GameObject[] pizzaPrefabs;  // 每個模型對應的預製體
    public Sprite[] pizzaSprites;  // 每個披薩的圖片（UI顯示用）

    // UI 按鈕預製體
    public GameObject pizzaButtonPrefab;
    public Transform buttonContainer;

    // 顯示玩家選擇的披薩圖像
    public Image[] playerImages;  // 玩家選擇的披薩圖片顯示（最多支持4個玩家）
    public Button startGameButton;

    // 儲存每個玩家的選擇
    private Dictionary<int, int> playerSelections = new Dictionary<int, int>();

    private int totalPlayers = 2;  // 預設2P遊戲，根據需要動態設置

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 設置玩家數量，並生成對應數量的披薩選擇按鈕
    public void SetupPlayerSelectionUI(int playerCount)
    {
        totalPlayers = playerCount;

        // 清理現有的按鈕
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }

        // 創建披薩選擇按鈕
        for (int i = 0; i < totalPlayers; i++)
        {
            GameObject pizzaButton = Instantiate(pizzaButtonPrefab, buttonContainer);
            pizzaButton.name = "PizzaButton" + (i + 1);
            pizzaButton.GetComponent<Button>().onClick.AddListener(() => OnPizzaButtonClicked(i));
            pizzaButton.GetComponentInChildren<Text>().text = "Player " + (i + 1);
        }

        // 設置開始遊戲按鈕為不可點擊
        startGameButton.interactable = false;
    }

    // 玩家選擇披薩後的回調
    public void OnPizzaButtonClicked(int playerIndex)
    {
        if (!playerSelections.ContainsKey(playerIndex))
        {
            playerSelections.Add(playerIndex, -1);  // -1 表示尚未選擇
        }

        // 隨機選擇一個披薩模型（這裡假設是隨機選擇，您可以根據需要改為手動選擇）
        int pizzaIndex = Random.Range(0, pizzaSprites.Length); // 隨機選擇
        playerSelections[playerIndex] = pizzaIndex;

        // 更新UI顯示玩家選擇的披薩圖像
        UpdatePizzaImage(playerIndex, pizzaSprites[pizzaIndex]);

        // 檢查是否所有玩家都已經選擇了披薩
        CheckAllPlayersSelected();
    }

    // 更新玩家選擇的披薩圖像
    private void UpdatePizzaImage(int playerIndex, Sprite selectedPizza)
    {
        if (playerIndex < playerImages.Length)
        {
            playerImages[playerIndex].sprite = selectedPizza;
        }
    }

    // 檢查是否所有玩家都已經選擇了披薩
    private void CheckAllPlayersSelected()
    {
        foreach (var selection in playerSelections)
        {
            if (selection.Value == -1)
            {
                return; // 若還有未選擇的玩家，則不啟動開始遊戲按鈕
            }
        }

        // 當所有玩家選擇完畢後啟動「開始遊戲」按鈕
        startGameButton.interactable = true;
    }

    // 開始遊戲
    public void StartGame()
    {
        // 這裡我們會根據玩家選擇的索引來加載對應的披薩模型
        for (int i = 0; i < totalPlayers; i++)
        {
            // 根據玩家選擇的披薩模型索引來加載對應的披薩模型
            int pizzaIndex = playerSelections[i];

            // 實例化玩家角色
            GameObject player = Instantiate(pizzaPrefabs[pizzaIndex], GetSpawnPosition(i), Quaternion.identity);
            player.name = "Player" + (i + 1);
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    // 根據玩家編號設置出生位置（這裡假設兩個玩家和四個玩家有不同的起始位置）
    private Vector3 GetSpawnPosition(int playerIndex)
    {
        if (totalPlayers == 2)
        {
            return playerIndex == 0 ? new Vector3(-5, 0, 0) : new Vector3(5, 0, 0);
        }
        else
        {
            // 四人遊玩時設置不同的起始位置
            switch (playerIndex)
            {
                case 0: return new Vector3(-5, 0, -5);
                case 1: return new Vector3(5, 0, -5);
                case 2: return new Vector3(-5, 0, 5);
                case 3: return new Vector3(5, 0, 5);
                default: return Vector3.zero;
            }
        }
    }
}
