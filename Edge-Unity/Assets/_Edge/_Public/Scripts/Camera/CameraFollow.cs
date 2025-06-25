using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;        // Player
    public Transform center;        // Tower�̒��S
    public float distance = 6f;     // Player����J�����܂ł̋���
    public float height = 2f;       // ����
    public float smoothSpeed = 5f;  // �Ǐ]�̂Ȃ߂炩��

    private void LateUpdate()
    {
        if (target == null || center == null)
            return;

        // Player �� Tower�̕������擾
        Vector3 directionToCenter = (center.position - target.position).normalized;

        // Player�̔w��ɃJ������z�u
        Vector3 desiredPosition = target.position - directionToCenter * distance;
        desiredPosition.y = target.position.y + height;

        // �X���[�Y�Ɉړ�
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
        transform.position = smoothedPosition;

        // Player������
        transform.LookAt(target.position + Vector3.up * 1f);
    }
}