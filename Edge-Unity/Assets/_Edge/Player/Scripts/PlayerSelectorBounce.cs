using UnityEngine;

public class PlayerSelectorBounce : MonoBehaviour
{
    public float bounceHeight = 0.5f;
    public float bounceSpeed = 2f;

    private Vector3 basePosition;
    private float bounceTimer;

    void Update()
    {
        bounceTimer += Time.deltaTime * bounceSpeed;
        float offsetY = Mathf.Sin(bounceTimer) * bounceHeight;
        transform.position = basePosition + new Vector3(0, offsetY, 0);
    }

    public void SetBasePosition(Vector3 position)
    {
        basePosition = position;
        transform.position = position;
    }
}