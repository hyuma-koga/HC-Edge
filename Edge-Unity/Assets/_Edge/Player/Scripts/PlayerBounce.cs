using UnityEngine;

public class PlayerBounce : MonoBehaviour
{
    public float bounceForce = 8f;
    public string targetTag = "FanBlock";

    private void OnCollisionStay(Collision collision)
    {
        // タグでFanBlockと判断（推奨）
        if (collision.gameObject.CompareTag(targetTag))
        {
            Rigidbody rb = GetComponent<Rigidbody>();

            // 真下に落ちてる or 接地してるときにだけ反発力を加える
            if (rb.linearVelocity.y <= 0.1f)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, bounceForce, rb.linearVelocity.z);
            }
        }
    }
}
