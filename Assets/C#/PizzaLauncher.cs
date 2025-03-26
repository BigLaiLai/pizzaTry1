using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PizzaLauncher : MonoBehaviour
{
    public float maxPower = 20f;
    public float minPower = 5f;
    private Vector2 stickInput;
    private Rigidbody rb;
    private bool isCharging = false;
    public float bounceForce = 1.5f;
    public float boundaryLimit = 4f;
    public float rotationSpeed = 100f;
    public LineRenderer lineRenderer;
    public float maxLineLength = 2f;

    public Transform lineStartTransform;
    private float lineLength; // 存儲箭頭長度
    private Vector3 launchDirection; // 存儲發射方向

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.freezeRotation = true;

        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.enabled = false;
        }
    }

    private void Update()
    {
        Gamepad gamepad = Gamepad.current;
        if (gamepad == null) return;

        stickInput = gamepad.leftStick.ReadValue();

        // 当摇杆被推动时，开始充能
        if (stickInput.magnitude > 0.1f)
        {
            isCharging = true;

            // **直接使用摇杆的X和Y值计算方向，转化为XZ平面上的方向**
            Vector3 direction = new Vector3(stickInput.x, 0, stickInput.y);

            // **让小披萨转向摇杆方向**
            transform.forward = direction; // 使用摇杆方向作为披萨的前方

            if (lineRenderer != null)
            {
                lineRenderer.enabled = true;

                // 根据摇杆的输入长度来设定箭头的长度
                lineLength = Mathf.Lerp(0, maxLineLength, stickInput.magnitude);

                Vector3 arrowStart = lineStartTransform.position;
                Vector3 arrowEnd = transform.position + direction * 3f; // **箭头指向摇杆的方向**

                lineRenderer.SetPosition(0, arrowStart);
                lineRenderer.SetPosition(1, arrowEnd);
            }

            // 计算发射方向：使用披萨的前方向
            launchDirection = transform.forward;  // 直接使用披萨的前方向（transform.forward）
        }

        // 当摇杆松开时，触发发射
        if (isCharging && stickInput.magnitude <= 0.1f)
        {
            StartCoroutine(LaunchPizza());
            isCharging = false;

            if (lineRenderer != null && !isCharging)
            {
                lineRenderer.enabled = false;
            }
        }

        // 限制披萨位置不超过边界
        ClampPizzaPosition();
    }



    private IEnumerator LaunchPizza()
    {
        yield return null;

        // 解除Kinematic状态，使物体受到力的影响
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // 计算发射力度，基于摇杆的输入距离
        float launchPower = Mathf.Max(stickInput.magnitude * maxPower, minPower);

        // 使用 AddForce 发射披萨
        rb.AddForce(launchDirection * launchPower, ForceMode.Impulse);

        Debug.Log($"发射！方向: {launchDirection}, 力度: {launchPower}");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Vector3 normal = collision.contacts[0].normal;
            rb.velocity = Vector3.Reflect(rb.velocity, normal) * bounceForce;
        }
    }

    private void ClampPizzaPosition()
    {
        float x = Mathf.Clamp(transform.position.x, -boundaryLimit, boundaryLimit);
        float z = Mathf.Clamp(transform.position.z, -boundaryLimit, boundaryLimit);
        transform.position = new Vector3(x, transform.position.y, z);
    }
}
