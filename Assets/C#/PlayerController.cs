using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputActions inputActions;
    public int selectedPizzaIndex = -1;
    public bool hasSelectedPizza { get; private set; } = false;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }
    //以下要記得改B鍵紀錄已選披薩

    //private void OnEnable()
    //{
    //    inputActions.Enable();
    //    inputActions.Character.SelectPizza.performed += OnSelectPizza;
    //}

    //private void OnDisable()
    //{
    //    inputActions.Character.SelectPizza.performed -= OnSelectPizza;
    //    inputActions.Disable();
    //}

    //private void OnSelectPizza(InputAction.CallbackContext context)
    //{
    //    if (!hasSelectedPizza)
    //    {
    //        selectedPizzaIndex = (selectedPizzaIndex + 1) % 4;
    //        hasSelectedPizza = true;
    //        Debug.Log("玩家選擇了披薩：" + selectedPizzaIndex);
    //    }
    //}
}
