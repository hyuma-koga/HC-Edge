using UnityEngine;

public class PlayerBounce : MonoBehaviour
{
    public float bounceForce = 8f;
    public string targetTag = "FanBlock";

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            Rigidbody rb = GetComponent<Rigidbody>();

            if (rb.linearVelocity.y <= 0.1f)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, bounceForce, rb.linearVelocity.z);
            }
        }
    }
}