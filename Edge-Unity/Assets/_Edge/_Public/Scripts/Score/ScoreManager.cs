using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int score = 0;
    public TMP_Text scoreText;

    private int comboCount = 0;           // òAë±â¡éZÉJÉEÉìÉg
    private float scoreMultiplier = 1f;   // î{ó¶Åi1.0 Å® 2.0 Å® ...Åj

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
        scoreMultiplier = 1f + comboCount * 1f; // 1î{Å®2î{Å®3î{...

        UpdateScoreUI();
        Debug.Log($"Score +{added}ÅiCombo: {comboCount}, Multiplier: x{scoreMultiplier})");
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