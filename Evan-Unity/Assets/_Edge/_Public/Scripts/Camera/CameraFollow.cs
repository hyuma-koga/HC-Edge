using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;        // Player
    public Transform center;        // Tower�̒��S
    public float distance = 6f;     // Player����J�����܂ł̋���
    public float height = 2f;       // ����
    public float smoothSpeed = 5f;  // �Ǐ]�̂Ȃ߂炩��
    public float fixedAngleX = 30f; // X���̌Œ�p�x

    private void LateUpdate()
    {
        if (target == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null) 
            {
                target = playerObj.transform;
            }
        }

        if (center == null)
        {
            GameObject centerObj = GameObject.FindWithTag("TowerCenter");
            if (centerObj != null) 
            {
                center = centerObj.transform;
            }
        }

        if (target == null || center == null)
        {
            return;
        }
           
        Vector3 directionToCenter = (center.position - target.position).normalized;

        Vector3 desiredPosition = target.position - directionToCenter * distance;
        desiredPosition.y = target.position.y + height;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
        transform.position = smoothedPosition;

        Vector3 lookDir = target.position - transform.position;
        float angleY = Quaternion.LookRotation(lookDir).eulerAngles.y;
        transform.rotation = Quaternion.Euler(fixedAngleX, angleY, 0f);
    }
}