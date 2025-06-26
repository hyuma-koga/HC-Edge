using UnityEngine;

public class PlayerSquashBounce : MonoBehaviour
{
    public float squashAmountY = 0.6f;
    public float squashAmountX = 1.2f;
    public float squashSpeed = 10f;
    public float returnSpeed = 5f;

    private Vector3 originalScale;
    private Vector3 squashScale;
    private Vector3 currentVelocity;
    private bool isSquashing = false;
    private bool isReturning = false;
    private bool gameClearedOrOver = false;

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
        if (!collision.gameObject.CompareTag("FanBlock"))
        {
            return;
        }

        isSquashing = true;
        isReturning = false;

        // コンボリセット
        ScoreManager.Instance?.ResetCombo();

        if (gameClearedOrOver)
        {
            return;
        }

        if (collision.gameObject.GetComponent<FinalFanBlockMarker>() != null)
        {
            gameClearedOrOver = true;
            GameClearManager.Instance?.TriggerGameClear();
            return;
        }

        if (collision.gameObject.GetComponent<GameOverBlockMarker>() != null)
        {
            gameClearedOrOver = true;
            GameOverManager.Instance?.TriggerGameOver();
            return;
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