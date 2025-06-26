using UnityEngine;

public class PlayerSelectorBounce : MonoBehaviour
{
    [Header("バウンドアニメーション設定")]
    public float squashAmountY = 0.6f;
    public float squashAmountX = 1.2f;
    public float squashSpeed = 10f;
    public float returnSpeed = 5f;
    public float bounceInterval = 1.0f;
    public float bounceHeight = 0.3f;
    public float bounceDuration = 0.3f;

    private Vector3 originalScale;
    private Vector3 squashScale;
    private Vector3 currentVelocity;
    private Vector3 basePosition;

    private float bounceTimer;
    private float bounceProgress = 0f;

    private enum BounceState
    {
        Idle,
        Squashing,
        Returning,
        Bouncing
    }
    private BounceState state = BounceState.Idle;

    void Start()
    {
        originalScale = transform.localScale;
        squashScale = new Vector3(
            originalScale.x * squashAmountX,
            originalScale.y * squashAmountY,
            originalScale.z * squashAmountX
        );
        basePosition = transform.position;
        bounceTimer = bounceInterval;
    }

    void Update()
    {
        bounceTimer -= Time.deltaTime;

        if (state == BounceState.Idle && bounceTimer <= 0f)
        {
            state = BounceState.Squashing;
            bounceTimer = bounceInterval;
        }

        switch (state)
        {
            case BounceState.Squashing:
                transform.localScale = Vector3.SmoothDamp(transform.localScale, squashScale, ref currentVelocity, 1f / squashSpeed);

                if (Vector3.Distance(transform.localScale, squashScale) < 0.01f)
                {
                    transform.localScale = squashScale;
                    state = BounceState.Returning;
                }
                break;

            case BounceState.Returning:
                transform.localScale = Vector3.SmoothDamp(transform.localScale, originalScale, ref currentVelocity, 1f / returnSpeed);

                if (Vector3.Distance(transform.localScale, originalScale) < 0.01f)
                {
                    transform.localScale = originalScale;
                    state = BounceState.Bouncing;
                    bounceProgress = 0f;
                }
                break;

            case BounceState.Bouncing:
                bounceProgress += Time.deltaTime / bounceDuration;
                float heightOffset = Mathf.Sin(bounceProgress * Mathf.PI) * bounceHeight;
                transform.position = basePosition + Vector3.up * heightOffset;

                if (bounceProgress >= 1f)
                {
                    transform.position = basePosition;
                    state = BounceState.Idle;
                }
                break;
        }
    }

    public void SetBasePosition(Vector3 position)
    {
        basePosition = position;
        transform.position = position;
    }

    public void SetBaseScale(Vector3 scale)
    {
        originalScale = scale;
        transform.localScale = scale;
        squashScale = new Vector3(
            originalScale.x * squashAmountX,
            originalScale.y * squashAmountY,
            originalScale.z * squashAmountX
        );
    }
}