using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

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
}