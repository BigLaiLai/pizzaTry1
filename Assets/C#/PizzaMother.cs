using UnityEngine;

public class PizzaMother : MonoBehaviour
{
    public float rotationSpeed = 30f; // 自轉速度
    public float impactForce = 5f;    // 受撞擊的位移力
    private Rigidbody rb;
    public Transform detectionCube;  // 偵測範圍的 Cube（不做為子物件）

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // 先讓它不受物理影響，只自轉
    }

    void Update()
    {
        // 持續自轉（無論是否被撞）
        transform.Rotate(-Vector3.forward * rotationSpeed * Time.deltaTime);

        // 保持 Cube 的相對位置，但不受縮放影響
        detectionCube.position = transform.position; // 保持相對位置
        detectionCube.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0); // 只旋轉而不受縮放影響
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pizza"))
        {
            Vector3 forceDirection = collision.contacts[0].point - transform.position;
            forceDirection.y = 0;
            forceDirection.Normalize();

            rb.isKinematic = false; // 開始受物理影響
            rb.AddForce(forceDirection * impactForce, ForceMode.Impulse);
            Debug.Log("大披薩被撞到了，開始位移！");
        }
    }
}
