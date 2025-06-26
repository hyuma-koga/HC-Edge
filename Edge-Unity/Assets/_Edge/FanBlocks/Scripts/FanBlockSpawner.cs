using System.Collections.Generic;
using UnityEngine;

public class FanBlockSpawner : MonoBehaviour
{
    public GameObject fanBlockPrefab;
    public GameObject gameOverBlockPrefab;

    public float startY = 0f;
    public float endY = 90f;
    public float intervalY = 5f;

    private int blocksPerLevel = 2;
    private float radius = 0f; // �z�u�p�̔��a

    // �ǉ��F�Q�[���I�[�o�[�u���b�N��Y���W�ꗗ
    private HashSet<float> gameOverYSet = new HashSet<float>();

    void Start()
    {
        SpawnFanBlocks();
    }

    void SpawnFanBlocks()
    {
        for (float y = startY; y <= endY; y += intervalY)
        {
            if (Mathf.Approximately(y, startY) || Mathf.Approximately(y, endY))
                continue;

            // �Q�[���I�[�o�[�u���b�N�F15f���Ƃ�1��
            if (Mathf.Repeat(y, 15f) < 0.01f && gameOverBlockPrefab != null)
            {
                float angle = Random.Range(0f, 360f);
                float rad = Mathf.Deg2Rad * angle;
                float x = Mathf.Cos(rad) * radius;
                float z = Mathf.Sin(rad) * radius;
                Vector3 pos = new Vector3(x, y, z);
                Quaternion rot = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

                GameObject gBlock = Instantiate(gameOverBlockPrefab, pos, rot, this.transform);

                FanBlock3DGenerator gen = gBlock.GetComponent<FanBlock3DGenerator>();
                if (gen != null) gen.GenerateFanBlock3D();

                gBlock.AddComponent<GameOverBlockMarker>();

                // ����Y�ɂ͒ʏ�u���b�N��u���Ȃ��悤�L�^
                gameOverYSet.Add(y);
                continue;
            }
        }

        // �ʏ��FanBlock����
        for (float y = startY; y <= endY; y += intervalY)
        {
            if (Mathf.Approximately(y, startY) || Mathf.Approximately(y, endY))
                continue;

            // ����Y�ɃQ�[���I�[�o�[�u���b�N������Ȃ�X�L�b�v
            if (gameOverYSet.Contains(y)) continue;

            for (int i = 0; i < blocksPerLevel; i++)
            {
                float angle = (360f / blocksPerLevel) * i + Random.Range(-15f, 15f);
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