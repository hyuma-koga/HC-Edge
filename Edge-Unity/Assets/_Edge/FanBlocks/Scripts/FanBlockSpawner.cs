using UnityEngine;

public class FanBlockSpawner : MonoBehaviour
{
    public GameObject fanBlockPrefab;
    public float startY = 0f;
    public float endY = 40f;
    public float intervalY = 3f;

    private int blocksPerLevel = 2;
    private float radius = 0f; // �z�u�p�̔��a�i���̒��S����̋����j

    void Start()
    {
        SpawnFanBlocks();
    }

    void SpawnFanBlocks()
    {
        for (float y = startY; y <= endY; y += intervalY)
        {
            for (int i = 0; i < blocksPerLevel; i++)
            {
                // �e�u���b�N���~����ɔz�u
                float angle = (360f / blocksPerLevel) * i + Random.Range(-15f, 15f); // �������������_������
                float rad = Mathf.Deg2Rad * angle;
                float x = Mathf.Cos(rad) * radius;
                float z = Mathf.Sin(rad) * radius;

                Vector3 position = new Vector3(x, y, z);
                Quaternion rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

                GameObject block = Instantiate(fanBlockPrefab, position, rotation, this.transform);

                FanBlock3DGenerator generator = block.GetComponent<FanBlock3DGenerator>();
                
                if (generator != null)
                {
                    generator.GenerateFanBlock3D();
                }
            }
        }
    }
}