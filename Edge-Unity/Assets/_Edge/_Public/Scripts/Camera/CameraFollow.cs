using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;        // Player
    public Transform center;        // Towerの中心
    public float distance = 6f;     // Playerからカメラまでの距離
    public float height = 2f;       // 高さ
    public float smoothSpeed = 5f;  // 追従のなめらかさ

    private void LateUpdate()
    {
        if (target == null || center == null)
            return;

        // Player → Towerの方向を取得
        Vector3 directionToCenter = (center.position - target.position).normalized;

        // Playerの背後にカメラを配置
        Vector3 desiredPosition = target.position - directionToCenter * distance;
        desiredPosition.y = target.position.y + height;

        // スムーズに移動
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
        transform.position = smoothedPosition;

        // Playerを見る
        transform.LookAt(target.position + Vector3.up * 1f);
    }
}