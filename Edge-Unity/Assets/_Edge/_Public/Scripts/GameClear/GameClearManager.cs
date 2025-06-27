using UnityEngine;
using UnityEngine.UI;

public class GameClearManager : MonoBehaviour
{
    public static GameClearManager Instance { get; private set; }

    [Header("UI")]
    public GameObject gameClearUI;
    public Text scoreText;
    public Text stageNumberText;

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
        if (isClearProcessed)
        {
            return;
        }

        isClearProcessed = true;

        ScoreManager.Instance?.SetScoreUIActive(false);

        if (scoreText != null && ScoreManager.Instance != null)
        {
            scoreText.text = ScoreManager.Instance.score.ToString();
        }

        // ステージ番号の表示は AdvanceToNextStageAndSave() の前に行う
        if (stageNumberText != null && StageManager.Instance != null)
        {
            int stageNumber = StageManager.Instance.currentStageIndex + 1;
            stageNumberText.text = $"{stageNumber}";
        }

        // ステージ進行は後にずらす
        StageManager.Instance?.AdvanceToNextStageAndSave();

        if (gameClearUI != null)
        {
            gameClearUI.SetActive(true);
        }

        if (CloudTransitionManager.Instance != null)
        {
            CloudTransitionManager.Instance.GoToSceneWithDelay(delayBeforeTransition, titleSceneName);
        }
        else
        {
            Invoke(nameof(LoadTitleScene), delayBeforeTransition);
        }
    }

    private void LoadTitleScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(titleSceneName);
    }
}