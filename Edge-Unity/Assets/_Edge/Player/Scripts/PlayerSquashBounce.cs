using UnityEngine;

public class PlayerSquashBounce : MonoBehaviour
{
    public float squashAmountY = 0.6f;   // Y�����ɒׂ��䗦
    public float squashAmountX = 1.2f;   // XZ�����ɍL����䗦
    public float squashSpeed = 10f;      // �ׂ��X�s�[�h
    public float returnSpeed = 5f;       // ���ɖ߂�X�s�[�h

    private Vector3 originalScale;
    private Vector3 squashScale;
    private Vector3 currentVelocity;
    private bool isSquashing = false;
    private bool isReturning = false;

    void Start()
    {
        originalScale = transform.localScale;
        squashScale = new Vector3(
            originalScale.x * squashAmountX,
            originalScale.y * squashAmountY,
            originalScale.z * squashAmountX
        );
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("FanBlock"))
        {
            isSquashing = true;
            isReturning = false;
        }
    }

    void Update()
    {
        if (isSquashing)
        {
            transform.localScale = Vector3.SmoothDamp(transform.localScale, squashScale, ref currentVelocity, 1f / squashSpeed);

            if (Vector3.Distance(transform.localScale, squashScale) < 0.01f)
            {
                isSquashing = false;
                isReturning = true;
            }
        }
        else if (isReturning)
        {
            transform.localScale = Vector3.SmoothDamp(transform.localScale, originalScale, ref currentVelocity, 1f / returnSpeed);

            if (Vector3.Distance(transform.localScale, originalScale) < 0.01f)
            {
                transform.localScale = originalScale;
                isReturning = false;
            }
        }
    }
}