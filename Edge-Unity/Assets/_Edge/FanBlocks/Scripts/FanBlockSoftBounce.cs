using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
public class FanBlockSoftBounce : MonoBehaviour
{
    public float dentDepth = 3f;
    public float dentRadius = 5f;
    public float recoverSpeed = 2.5f;

    private Mesh mesh;
    private Vector3[] originalVertices;
    private Vector3[] modifiedVertices;

    private void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        originalVertices = mesh.vertices;
        modifiedVertices = mesh.vertices;
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            DentAtPoint(contact.point);
        }
    }

    void DentAtPoint(Vector3 worldPoint)
    {
        Vector3 localPoint = transform.InverseTransformPoint(worldPoint);

        for (int i = 0; i < modifiedVertices.Length; i++)
        {
            float distance = Vector3.Distance(modifiedVertices[i], localPoint);
            if (distance < dentRadius)
            {
                float influence = 1f - (distance / dentRadius);
                modifiedVertices[i] -= Vector3.up * dentDepth * influence;
            }
        }

        mesh.vertices = modifiedVertices;
        mesh.RecalculateNormals();
    }

    private void Update()
    {
        // Œ³‚ÌŒ`‚É–ß‚·ˆ—
        for (int i = 0; i < modifiedVertices.Length; i++)
        {
            modifiedVertices[i] = Vector3.Lerp(modifiedVertices[i], originalVertices[i], Time.deltaTime * recoverSpeed);
        }

        mesh.vertices = modifiedVertices;
        mesh.RecalculateNormals();
    }
}