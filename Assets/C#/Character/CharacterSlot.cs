using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; // 加入這行

public class CharacterSlot : MonoBehaviour
{
    public int characterIndex;
    public Image characterImage; // 顯示角色圖像
    public List<Image> playerColorIndicators = new List<Image>(); // 顯示玩家顏色的區域

    public bool isSelected = false;

    public void LockSlot()
    {
        isSelected = true;
        characterImage.color = Color.gray; // 選擇後灰色顯示
    }

    public void AddPlayerColor(int playerIndex)
    {
        // 根據玩家索引給顏色
        Color playerColor = GetPlayerColor(playerIndex);
        if (!playerColorIndicators[playerIndex].enabled)
        {
            playerColorIndicators[playerIndex].color = playerColor;
            playerColorIndicators[playerIndex].enabled = true;
        }
    }

    private Color GetPlayerColor(int playerIndex)
    {
        switch (playerIndex)
        {
            case 0: return Color.green; // 玩家 1: 綠色
            case 1: return Color.blue;  // 玩家 2: 藍色
            case 2: return Color.yellow; // 玩家 3: 黃色
            case 3: return Color.red;   // 玩家 4: 紅色
            default: return Color.white;
        }
    }
}
