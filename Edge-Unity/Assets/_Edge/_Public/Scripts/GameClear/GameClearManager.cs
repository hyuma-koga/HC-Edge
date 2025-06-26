using UnityEngine;
using UnityEngine.UI;

public class GameClearManager : MonoBehaviour
{
    public static GameClearManager Instance { get; private set; }

    [Header("UI")]
    public GameObject gameClearUI;
    public Text scoreText;

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
        if (isClearProcessed) return;

        isClearProcessed = true;

        // �X�R�AUI��\��
        ScoreManager.Instance?.SetScoreUIActive(false);

        // �X�R�A�\���X�V
        if (scoreText != null && ScoreManager.Instance != null)
        {
            scoreText.text = ScoreManager.Instance.score.ToString();
        }

        // UI�\��
        if (gameClearUI != null)
        {
            gameClearUI.SetActive(true);
        }

        // ���̃X�e�[�W�ԍ���ۑ��i�^�C�g����ʂœǂݍ��ށj
        if (StageManager.Instance != null)
        {
            int nextStage = StageManager.Instance.GetNextStageIndex();
            PlayerPrefs.SetInt("SelectedStageIndex", nextStage);
        }

        // �J�ڏ���
        if (CloudTransitionManager.Instance != null)
        {
            CloudTransitionManager.Instance.GoToSceneWithDelay(delayBeforeTransition, titleSceneName);
        }
        else
        {
            // �O�̂��߃t�H�[���o�b�N
            Invoke(nameof(LoadTitleScene), delayBeforeTransition);
        }
    }

    private void LoadTitleScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(titleSceneName);
    }
}