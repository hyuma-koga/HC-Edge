using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerCircleMovement : MonoBehaviour
{
    public Transform center;            // 塔の中心（Transform）
    public float radius = 2.5f;         // 円運動の半径
    public float sensitivity = 5f;      // マウス感度

    private Rigidbody rb;
    private float angle = 0f;
    private float previousMouseX;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        previousMouseX = Input.mousePosition.x;
    }

    void Update()
    {
        // 長押し時に角度を更新
        if (Input.GetMouseButton(0))
        {
            float currentMouseX = Input.mousePosition.x;
            float deltaX = currentMouseX - previousMouseX;
            angle += deltaX * sensitivity * Time.deltaTime;
            angle %= Mathf.PI * 2f;

            previousMouseX = currentMouseX;
        }
        else
        {
            previousMouseX = Input.mousePosition.x;
        }

        // 高さを追従して自然落下対応
        center.position = new Vector3(center.position.x, transform.position.y, center.position.z);

        // 新しい位置を算出（XZ平面）
        Vector3 offset = new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle)) * radius;
        Vector3 targetPos = center.position + offset;
        targetPos.y = rb.position.y;

        rb.MovePosition(targetPos);

        // 塔の中心を向く
        transform.LookAt(new Vector3(center.position.x, transform.position.y, center.position.z));
    }
}