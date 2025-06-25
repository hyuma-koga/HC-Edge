using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;        // Player
    public Transform center;        // Towerの中心
    public float distance = 6f;     // Playerからカメラまでの距離
    public float height = 2f;       // 高さ
    public float smoothSpeed = 5f;  // 追従のなめらかさ

    public float fixedAngleX = 30f; // ← X軸の固定角度

    private void LateUpdate()
    {
        if (target == null || center == null)
            return;

        // Player → Towerの方向を取得
        Vector3 directionToCenter = (center.position - target.position).normalized;

        // カメラの位置を決定
        Vector3 desiredPosition = target.position - directionToCenter * distance;
        desiredPosition.y = target.position.y + height;

        // スムーズに移動
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
        transform.position = smoothedPosition;

        // 回転だけ角度固定（X軸30度、Y軸はプレイヤーを向くように）
        Vector3 lookDir = target.position - transform.position;
        float angleY = Quaternion.LookRotation(lookDir).eulerAngles.y;

        transform.rotation = Quaternion.Euler(fixedAngleX, angleY, 0f); // ← X=30°, Yは追従
    }
}