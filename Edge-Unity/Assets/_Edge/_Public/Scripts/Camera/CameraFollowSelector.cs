using UnityEngine;

public class CameraFollowSelector : MonoBehaviour
{
    public Transform target;         // PlayerSelector
    public Vector3 offset = new Vector3(0f, 0f, -7f); // Yはここでは無視
    public float fixedY = 5f;        // Y座標を固定
    public float smoothSpeed = 5f;   // 追従速度
    public float fixedAngleX = 60f;  // X軸固定角度

    private void LateUpdate()
    {
        if (target == null) return;

        // targetのXとZに相対位置を加え、Yは固定
        Vector3 desiredPosition = new Vector3(
            target.position.x + offset.x,
            fixedY,
            target.position.z + offset.z
        );

        // スムーズに追従
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);

        // 回転は X軸固定、YとZは 0 に固定
        transform.rotation = Quaternion.Euler(fixedAngleX, 0f, 0f);
    }
}