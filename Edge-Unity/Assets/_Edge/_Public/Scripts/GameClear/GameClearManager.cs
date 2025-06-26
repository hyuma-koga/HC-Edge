using UnityEngine;
using UnityEngine.UI;

public class GameClearManager : MonoBehaviour
{
    public static GameClearManager Instance { get; private set; }

    [Header("UI")]
    public GameObject gameClearUI;
    public Text scoreText;

    [Header("設定")]
    public float delayBeforeTransition = 3f;
    public string titleSceneName = "TitleScene";

    private bool isClearProcessed = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (gameClearUI != null)
        {
            gameClearUI.SetActive(false);
        }
    }

    public void TriggerGameClear()
    {
        if (isClearProcessed) return;

        isClearProcessed = true;

        // スコアUI非表示
        ScoreManager.Instance?.SetScoreUIActive(false);

        // スコア表示更新
        if (scoreText != null && ScoreManager.Instance != null)
        {
            scoreText.text = ScoreManager.Instance.score.ToString();
        }

        // UI表示
        if (gameClearUI != null)
        {
            gameClearUI.SetActive(true);
        }

        // 次のステージ番号を保存（タイトル画面で読み込む）
        if (StageManager.Instance != null)
        {
            int nextStage = StageManager.Instance.GetNextStageIndex();
            PlayerPrefs.SetInt("SelectedStageIndex", nextStage);
        }

        // 遷移処理
        if (CloudTransitionManager.Instance != null)
        {
            CloudTransitionManager.Instance.GoToSceneWithDelay(delayBeforeTransition, titleSceneName);
        }
        else
        {
            // 念のためフォールバック
            Invoke(nameof(LoadTitleScene), delayBeforeTransition);
        }
    }

    private void LoadTitleScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(titleSceneName);
    }
}