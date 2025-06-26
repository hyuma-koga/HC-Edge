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

        if (gameClearUI != null)
        {
            gameClearUI.SetActive(true);
        }

        CloudTransitionManager.Instance.GoToSceneWithDelay(3f, "TitleScene");
    }
}
