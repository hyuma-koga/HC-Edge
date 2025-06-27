using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    [Header("�X�e�[�W�ݒ�")]
    public GameObject[] stagePrefabs;
    public Transform stageSpawnPoint;

    [Header("���݂̃X�e�[�W")]
    public int currentStageIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        LoadCurrentStageFromPrefs();
        LoadStage(currentStageIndex);
    }

    // ���݂̃X�e�[�W��ǂݍ��݁E����
    public void LoadStage(int index)
    {
        currentStageIndex = index;

        // �Â��X�e�[�W���폜
        foreach (Transform child in stageSpawnPoint)
        {
            Destroy(child.gameObject);
        }

        // �V�����X�e�[�W�𐶐�
        if (index >= 0 && index < stagePrefabs.Length)
        {
            Instantiate(stagePrefabs[index], stageSpawnPoint.position, Quaternion.identity, stageSpawnPoint);
        }
    }

    // ���̃X�e�[�W�C���f�b�N�X���擾�i���[�v�j
    public int GetNextStageIndex()
    {
        return (currentStageIndex + 1) % stagePrefabs.Length;
    }

    // ���݂̃X�e�[�W��ۑ�
    public void SaveCurrentStageToPrefs()
    {
        PlayerPrefs.SetInt("SelectedStageIndex", currentStageIndex);
        PlayerPrefs.Save();
    }

    // �ۑ����ꂽ�X�e�[�W��ǂݍ���
    public void LoadCurrentStageFromPrefs()
    {
        currentStageIndex = PlayerPrefs.GetInt("SelectedStageIndex", 0);
    }

    // ���̃X�e�[�W��I�����ĕۑ��i�Q�[���N���A���ȂǂɌĂԁj
    public void AdvanceToNextStageAndSave()
    {
        currentStageIndex = GetNextStageIndex();
        SaveCurrentStageToPrefs();
    }
}