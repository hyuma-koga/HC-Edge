#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class FanBlock3DGenerator : MonoBehaviour
{
    [Header("ゲームオーバー設定")]
    public bool isGameOverBlock = false;

    public float radiusInner = 2.4f;
    public float radiusOuter = 4f;
    public float height = 0.2f;
    public float angleDegree = 70f;
    public int segments = 60;

#if UNITY_EDITOR
    private void OnValidate()
    {
        // エディタ上でパラメータ変更時に自動生成
        EditorApplication.delayCall += () =>
        {
            if (this == null) return; // 削除された場合
            GenerateFanBlock3D();
        };
    }
#endif

    private void Start()
    {
        if (Application.isPlaying)
        {
            // ゲームオーバーブロックには ScoreTrigger を作らない
            if (!isGameOverBlock)
            {
                CreateScoreTriggerSafely();
            }
            else
            {
                // GameOverBlockMarker がなければ追加
                if (GetComponent<GameOverBlockMarker>() == null)
                {
                    gameObject.AddComponent<GameOverBlockMarker>();
                }
            }
        }
    }

    public void GenerateFanBlock3D()
    {
        Mesh mesh = new Mesh();
        float angleRad = Mathf.Deg2Rad * angleDegree;
        float angleStep = angleRad / segments;

        int vCount = (segments + 1) * 2;
        Vector3[] vertices = new Vector3[vCount * 2];
        int[] triangles = new int[segments * 6 * 4 + 12];

        // 頂点生成
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

        // 三角形生成
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

            // 上面
            triangles[ti++] = j0; triangles[ti++] = j2; triangles[ti++] = j1;
            triangles[ti++] = j2; triangles[ti++] = j3; triangles[ti++] = j1;

            // 下面
            triangles[ti++] = i0; triangles[ti++] = i1; triangles[ti++] = i2;
            triangles[ti++] = i2; triangles[ti++] = i1; triangles[ti++] = i3;

            // 外周
            triangles[ti++] = i1; triangles[ti++] = j1; triangles[ti++] = i3;
            triangles[ti++] = i3; triangles[ti++] = j1; triangles[ti++] = j3;

            // 内周
            triangles[ti++] = j0; triangles[ti++] = i0; triangles[ti++] = j2;
            triangles[ti++] = j2; triangles[ti++] = i0; triangles[ti++] = i2;
        }

        // 始点端面
        triangles[ti++] = vCount + 0;
        triangles[ti++] = 0;
        triangles[ti++] = vCount + 1;

        triangles[ti++] = vCount + 1;
        triangles[ti++] = 0;
        triangles[ti++] = 1;

        // 終点端面
        int li = segments * 2;
        triangles[ti++] = vCount + li + 1;
        triangles[ti++] = li + 1;
        triangles[ti++] = vCount + li;

        triangles[ti++] = vCount + li;
        triangles[ti++] = li + 1;
        triangles[ti++] = li;

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().sharedMesh = mesh;

        MeshCollider collider = GetComponent<MeshCollider>();
        if (collider == null) collider = gameObject.AddComponent<MeshCollider>();
        collider.sharedMesh = null;
        collider.sharedMesh = mesh;
        collider.convex = true;
    }

    private void CreateScoreTriggerSafely()
    {
        float thisY = transform.position.y;
        float epsilon = 0.05f; // 誤差範囲（必要に応じて調整）

        // 自分以外の FanBlock で、ほぼ同じ高さに ScoreTrigger を持つものがあるかチェック
        bool triggerAlreadyExists = false;
        foreach (var other in Object.FindObjectsByType<FanBlock3DGenerator>(FindObjectsSortMode.None))
        {
            if (other != this &&
                Mathf.Abs(other.transform.position.y - thisY) < epsilon &&
                other.transform.Find("ScoreTrigger") != null)
            {
                triggerAlreadyExists = true;
                break;
            }
        }

        if (triggerAlreadyExists)
        {
            Debug.Log($"Y={thisY} に既に ScoreTrigger があるため生成スキップ");
            return;
        }

        // 既存のトリガーがあれば削除（再生成対策）
        Transform existing = transform.Find("ScoreTrigger");
        if (existing != null)
        {
            Destroy(existing.gameObject);
        }

        // スコアトリガー生成
        GameObject triggerObj = new GameObject("ScoreTrigger");
        triggerObj.transform.SetParent(transform, false);
        triggerObj.transform.localPosition = new Vector3(0, -0.5f, 0);

        BoxCollider trigger = triggerObj.AddComponent<BoxCollider>();
        trigger.isTrigger = true;
        trigger.size = new Vector3(6f, 0.5f, 6f);

        triggerObj.AddComponent<ScoreTrigger>();
    }
}