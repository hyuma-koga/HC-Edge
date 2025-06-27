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

                Color playerColor = Color.white;
                var renderer = GetComponent<Renderer>();
                if (renderer != null)
                {
                    playerColor = renderer.material.color;
                }

                if (collision.contactCount > 0)
                {
                    Vector3 hitPoint = collision.contacts[0].point;
                    Transform parent = collision.transform;

                    ColorSplash.Create(hitPoint, playerColor, parent);
                }
            }
        }
    }
}