#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class FanBlock3DGenerator : MonoBehaviour
{
    public float radiusInner = 2.4f;
    public float radiusOuter = 4f;
    public float height = 0.2f;
    public float angleDegree = 70f;
    public int segments = 60;

#if UNITY_EDITOR
    private void OnValidate()
    {
        // �G�f�B�^�̎��t���[����Generate�����s�isharedMesh�ύX�͂��̃^�C�~���O��OK�j
        EditorApplication.delayCall += () =>
        {
            if (this == null) return; // �폜���ꂽ�ꍇ
            GenerateFanBlock3D();
        };
    }
#endif

    public void GenerateFanBlock3D()
    {
        Mesh mesh = new Mesh();
        float angleRad = Mathf.Deg2Rad * angleDegree;
        float angleStep = angleRad / segments;

        int vCount = (segments + 1) * 2;
        Vector3[] vertices = new Vector3[vCount * 2];
        int[] triangles = new int[segments * 6 * 4 + 12]; // �[�ʒǉ��̂���+12

        // ���_����
        for (int i = 0; i <= segments; i++)
        {
            float angle = angleStep * i;
            float sin = Mathf.Sin(angle);
            float cos = Mathf.Cos(angle);

            Vector3 inner = new Vector3(radiusInner * sin, 0, radiusInner * cos);
            Vector3 outer = new Vector3(radiusOuter * sin, 0, radiusOuter * cos);

            vertices[i * 2] = inner;
            vertices[i * 2 + 1] = outer;
            vertices[i * 2 + vCount] = inner + Vector3.up * height;
            vertices[i * 2 + 1 + vCount] = outer + Vector3.up * height;
        }

        // �O�p�`����
        int ti = 0;
        for (int i = 0; i < segments; i++)
        {
            int i0 = i * 2;
            int i1 = i0 + 1;
            int i2 = i0 + 2;
            int i3 = i0 + 3;

            int j0 = i0 + vCount;
            int j1 = i1 + vCount;
            int j2 = i2 + vCount;
            int j3 = i3 + vCount;

            // ���
            triangles[ti++] = j0; triangles[ti++] = j2; triangles[ti++] = j1;
            triangles[ti++] = j2; triangles[ti++] = j3; triangles[ti++] = j1;

            // ����
            triangles[ti++] = i0; triangles[ti++] = i1; triangles[ti++] = i2;
            triangles[ti++] = i2; triangles[ti++] = i1; triangles[ti++] = i3;

            // �O��
            triangles[ti++] = i1; triangles[ti++] = j1; triangles[ti++] = i3;
            triangles[ti++] = i3; triangles[ti++] = j1; triangles[ti++] = j3;

            // ����
            triangles[ti++] = j0; triangles[ti++] = i0; triangles[ti++] = j2;
            triangles[ti++] = j2; triangles[ti++] = i0; triangles[ti++] = i2;
        }

        // �n�_���̒[��
        triangles[ti++] = vCount + 0; // ���
        triangles[ti++] = 0;          // ����
        triangles[ti++] = vCount + 1; // ��O

        triangles[ti++] = vCount + 1;
        triangles[ti++] = 0;
        triangles[ti++] = 1;

        // �I�_���̒[��
        int li = segments * 2;
        triangles[ti++] = vCount + li + 1; // ��O
        triangles[ti++] = li + 1;          // ���O
        triangles[ti++] = vCount + li;     // ���

        triangles[ti++] = vCount + li;
        triangles[ti++] = li + 1;
        triangles[ti++] = li;

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().sharedMesh = mesh;
    }
}