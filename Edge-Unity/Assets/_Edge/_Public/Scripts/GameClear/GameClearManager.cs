using UnityEngine;
using UnityEngine.UI;

public class GameClearManager : MonoBehaviour
{
    public static GameClearManager Instance { get; private set; }

    [Header("UI")]
    public GameObject gameClearUI;
    public Text scoreText;

    [Header("ê›íË")]
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

        StageManager.Instance?.AdvanceToNextStageAndSave();

        ScoreManager.Instance?.SetScoreUIActive(false);

        if (scoreText != null && ScoreManager.Instance != null)
        {
            scoreText.text = ScoreManager.Instance.score.ToString();
        }

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