using UnityEngine;
using UnityEngine.UI;

public class GameClearManager : MonoBehaviour
{
    public static GameClearManager Instance { get; private set; }

    [Header("UI")]
    public GameObject gameClearUI;
    public Text scoreText;
    public Text stageNumberText; // �� �ǉ�

    [Header("�ݒ�")]
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

        // �X�e�[�W�ԍ��̕\���� AdvanceToNextStageAndSave() �̑O�ɍs��
        if (stageNumberText != null && StageManager.Instance != null)
        {
            int stageNumber = StageManager.Instance.currentStageIndex + 1;
            stageNumberText.text = $"{stageNumber}";
        }

        // �X�e�[�W�i�s�͌�ɂ��炷
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