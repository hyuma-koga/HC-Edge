using UnityEngine;

public class PlayerSquashBounce : MonoBehaviour
{
    public float squashAmountY = 0.6f;   // Y方向に潰す比率
    public float squashAmountX = 1.2f;   // XZ方向に広げる比率
    public float squashSpeed = 10f;      // 潰れるスピード
    public float returnSpeed = 5f;       // 元に戻るスピード

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

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetCombo();
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