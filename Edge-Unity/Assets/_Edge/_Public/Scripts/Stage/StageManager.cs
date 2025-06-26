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

    /// <summary>
    /// 現在のステージを読み込み・生成
    /// </summary>
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
        else
        {
            Debug.LogWarning($"Stage index {index} は範囲外です");
        }
    }

    /// <summary>
    /// 次のステージインデックスを取得（ループ）
    /// </summary>
    public int GetNextStageIndex()
    {
        return (currentStageIndex + 1) % stagePrefabs.Length;
    }

    /// <summary>
    /// 現在のステージを保存
    /// </summary>
    public void SaveCurrentStageToPrefs()
    {
        PlayerPrefs.SetInt("SelectedStageIndex", currentStageIndex);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 保存されたステージを読み込み
    /// </summary>
    public void LoadCurrentStageFromPrefs()
    {
        currentStageIndex = PlayerPrefs.GetInt("SelectedStageIndex", 0);
    }

    /// <summary>
    /// 次のステージを選択して保存（ゲームクリア時などに呼ぶ）
    /// </summary>
    public void AdvanceToNextStageAndSave()
    {
        currentStageIndex = GetNextStageIndex();
        SaveCurrentStageToPrefs();
    }
}