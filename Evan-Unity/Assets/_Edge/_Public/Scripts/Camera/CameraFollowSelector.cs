using UnityEngine;

public class CameraFollowSelector : MonoBehaviour
{
    public Transform target;         // PlayerSelector
    public Vector3 offset = new Vector3(0f, 0f, -7f); // Y�͂����ł͖���
    public float fixedY = 5f;        // Y���W���Œ�
    public float smoothSpeed = 5f;   // �Ǐ]���x
    public float fixedAngleX = 60f;  // X���Œ�p�x

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        // target��X��Z�ɑ��Έʒu�������AY�͌Œ�
        Vector3 desiredPosition = new Vector3(
            target.position.x + offset.x,
            fixedY,
            target.position.z + offset.z
        );

        // �X���[�Y�ɒǏ]
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);

        // ��]�� X���Œ�AY��Z�� 0 �ɌŒ�
        transform.rotation = Quaternion.Euler(fixedAngleX, 0f, 0f);
    }
}