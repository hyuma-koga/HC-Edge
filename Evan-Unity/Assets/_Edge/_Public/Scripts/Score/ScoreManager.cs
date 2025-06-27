using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("UI")]
    public GameObject scoreUI;

    public int score = 0;
    public TMP_Text scoreText;

    private int comboCount = 0;           // �A�����Z�J�E���g
    private float scoreMultiplier = 1f;   // �{���i1.0 �� 2.0 �� ...�j

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
        {
            SetScoreUIActive(true);
        }
        else
        {
            SetScoreUIActive(false);
        }
    }

    public void AddScore(int baseValue)
    {
        int added = Mathf.RoundToInt(baseValue * scoreMultiplier);
        score += added;
        comboCount++;
        scoreMultiplier = 1f + comboCount * 1f; // 1�{��2�{��3�{...

        UpdateScoreUI();
        Debug.Log($"Score +{added}�iCombo: {comboCount}, Multiplier: x{scoreMultiplier})");
    }

    public void ResetCombo()
    {
        comboCount = 0;
        scoreMultiplier = 1f;
    }

    public void ResetScore()
    {
        score = 0;
        ResetCombo();
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }

    public void SetScoreUIActive(bool isActive)
    {
        if (scoreUI != null)
        {
            scoreUI.SetActive(isActive);
        }
    }
}