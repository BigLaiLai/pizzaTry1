using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float maxPower = 20f;  // 最大發射力度
    private Vector2 moveInput;    // 記錄蘑菇頭的方向
    private Rigidbody rb;
    private PlayerInputActions inputActions;
    private bool isCharging = false; // 是否正在發射

    private Vector2 initialPosition;  // 蘑菇頭起始位置
    private float chargeStartTime;    // 計算拉動時間

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Move.performed += OnMovePerformed;
        inputActions.Player.Move.canceled += OnMoveCanceled;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Player.Move.canceled -= OnMoveCanceled;
        inputActions.Disable();
    }

    private void Start()
    {
        rb.isKinematic = true; // 讓披薩預設保持不動
    }

    //private void Update()
    //{
    //    // 根據蘑菇頭的移動方向來旋轉物體
    //    if (isCharging && moveInput.magnitude > 0.1f)
    //    {
    //        float rotationAngle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
    //        transform.rotation = Quaternion.Euler(0, rotationAngle + 180f, 0); // **修正反方向**
    //    }
    //}

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();

        // 當玩家開始移動蘑菇頭時，記錄起始位置和時間
        if (!isCharging && moveInput.magnitude > 0.1f)
        {
            isCharging = true;
            initialPosition = moveInput; // 記錄蘑菇頭的起始位置
            chargeStartTime = Time.time;  // 記錄發動時間
        }
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        if (isCharging)
        {
            StartCoroutine(LaunchPizza()); // 放開時發射
            isCharging = false;
        }
        moveInput = Vector2.zero;
    }

    private IEnumerator LaunchPizza()
    {
        // 解除 Kinematic，讓剛體受力
        rb.isKinematic = false;

        // **等待一幀，確保物理引擎更新**
        yield return null;

        // 計算發射方向，使用披薩尖角的朝向
        Vector3 launchDirection = transform.forward;  // 获取披萨的尖角方向

        // 計算發射力度，根據蘑菇頭的移動距離來判斷力度
        float moveDistance = (moveInput - initialPosition).magnitude;  // 移動的距離
        float launchPower = Mathf.Clamp(moveDistance * maxPower, 0, maxPower); // 力度

        // **使用 AddForce 推動披薩**
        rb.AddForce(launchDirection * launchPower, ForceMode.Impulse);

        Debug.Log($"發射！方向: {launchDirection}, 力度: {launchPower}");
    }
}
