using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set; }

    [Header("UI")]
    public GameObject gameOverUI;
    public Image timerImage; // 円形のImage
    public float timerDuration = 10f;

    [Header("遷移先")]
    public string titleSceneName = "TitleScene";

    private float timer;
    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (timerImage != null) timerImage.fillAmount = 1f;
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        // スコアUI非表示
        ScoreManager.Instance?.SetScoreUIActive(false);

        // ゲームオーバーUI表示
        gameOverUI.SetActive(true);

        // タイマー開始
        timer = timerDuration;
    }

    private void Update()
    {
        if (!isGameOver) return;

        // タイマー進行
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            float t = Mathf.Clamp01(timer / timerDuration);

            if (timerImage != null)
            {
                timerImage.fillAmount = t;
            }

            if (timer <= 0f)
            {
                GoToTitle();
            }
        }

        // マウスクリックでも即時タイトルへ
        if (Input.GetMouseButtonDown(0))
        {
            GoToTitle();
        }
    }

    private void GoToTitle()
    {
        if (CloudTransitionManager.Instance != null)
        {
            CloudTransitionManager.Instance.GoToScene(titleSceneName);
        }
        else
        {
            SceneManager.LoadScene(titleSceneName);
        }
    }
}