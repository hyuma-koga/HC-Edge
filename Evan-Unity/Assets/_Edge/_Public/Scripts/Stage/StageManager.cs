using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    [Header("ステージ設定")]
    public GameObject[] stagePrefabs;
    public Transform stageSpawnPoint;

    [Header("現在のステージ")]
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

    // 現在のステージを読み込み・生成
    public void LoadStage(int index)
    {
        currentStageIndex = index;

        // 古いステージを削除
        foreach (Transform child in stageSpawnPoint)
        {
            Destroy(child.gameObject);
        }

        // 新しいステージを生成
        if (index >= 0 && index < stagePrefabs.Length)
        {
            Instantiate(stagePrefabs[index], stageSpawnPoint.position, Quaternion.identity, stageSpawnPoint);
        }
    }

    // 次のステージインデックスを取得（ループ）
    public int GetNextStageIndex()
    {
        return (currentStageIndex + 1) % stagePrefabs.Length;
    }

    // 現在のステージを保存
    public void SaveCurrentStageToPrefs()
    {
        PlayerPrefs.SetInt("SelectedStageIndex", currentStageIndex);
        PlayerPrefs.Save();
    }

    // 保存されたステージを読み込み
    public void LoadCurrentStageFromPrefs()
    {
        currentStageIndex = PlayerPrefs.GetInt("SelectedStageIndex", 0);
    }

    // 次のステージを選択して保存（ゲームクリア時などに呼ぶ）
    public void AdvanceToNextStageAndSave()
    {
        currentStageIndex = GetNextStageIndex();
        SaveCurrentStageToPrefs();
    }
}