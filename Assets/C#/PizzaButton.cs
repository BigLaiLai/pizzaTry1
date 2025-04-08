using UnityEngine;
using UnityEngine.UI;

public class PizzaButton : MonoBehaviour
{
    public Image pizzaImage; // 按鈕上顯示的披薩圖像
    public Sprite[] pizzaSprites; // 可選的披薩圖像

    // 用來初始化披薩圖片
    public void SetPizzaImage(int pizzaIndex)
    {
        pizzaImage.sprite = pizzaSprites[pizzaIndex]; // 更新按鈕上的披薩圖片
    }

    // 玩家選擇披薩後的回調
    public void OnButtonClick()
    {
        // 呼叫 CharacterSelectionManager 中的 OnPizzaButtonClicked 方法
        int playerIndex = int.Parse(gameObject.name.Substring(10)) - 1; // 根據按鈕名稱取得玩家索引
        CharacterSelectionManager.Instance.OnPizzaButtonClicked(playerIndex);
    }
}
