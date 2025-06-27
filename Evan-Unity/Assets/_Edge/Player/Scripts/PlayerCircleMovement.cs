using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerCircleMovement : MonoBehaviour
{
    public Transform center;            // 塔の中心（Transform）
    public float radius = 2.5f;         // 円運動の半径
    public float sensitivity = 5f;      // マウス感度
    public GameObject mapButton;        // ← HierarchyのMap_Buttonをアサイン

    private Rigidbody rb;
    private float angle = 0f;
    private float previousMouseX;
    private bool mapHidden = false;     // 一度だけ非表示にするためのフラグ

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        previousMouseX = Input.mousePosition.x;
    }

    void Update()
    {
        float currentMouseX = Input.mousePosition.x;
        float deltaX = currentMouseX - previousMouseX;

        if (!mapHidden && Mathf.Abs(deltaX) > 0.01f && Input.GetMouseButton(0))
        {
            if (mapButton != null)
            {
                mapButton.SetActive(false);
            }
            mapHidden = true;
        }

        if (Input.GetMouseButton(0))
        {
            angle += deltaX * sensitivity * Time.deltaTime;
            angle %= Mathf.PI * 2f;
        }

        previousMouseX = currentMouseX;
        center.position = new Vector3(center.position.x, transform.position.y, center.position.z);
        
        Vector3 offset = new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle)) * radius;
        Vector3 targetPos = center.position + offset;
        
        targetPos.y = rb.position.y;
        rb.MovePosition(targetPos);
        transform.LookAt(new Vector3(center.position.x, transform.position.y, center.position.z));
    }
}