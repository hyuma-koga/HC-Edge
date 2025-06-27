using UnityEngine;
using UnityEngine.UI;

public class StageProgressBarController : MonoBehaviour
{
    [Header("ステージ番号表示")]
    public Text currentStageText;
    public Text nextStageText;

    [Header("ステージ丸画像")]
    public Image currentStageCircleImage;
    public Image nextStageCircleImage;

    [Header("進捗スライダー")]
    public Image stageSliderImage;

    [Header("色設定")]
    public Color filledColor = new Color(1f, 0.6f, 0f); // オレンジ
    public Color unfilledColor = Color.gray;

    [Header("進捗の基準")]
    public Transform playerTransform;
    public float startY = 0f;
    public float goalY = 100f;

    private bool initialized = false;

    private void Start()
    {
        goalY = 0f; // 共通で固定
        UpdateStageLabelsAndColors();
    }

    private void Update()
    {
        // 初回のみプレイヤーを検索して初期化
        if (!initialized)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;

                // ステージに応じた startY を設定
                int stageIndex = StageManager.Instance?.currentStageIndex ?? 0;

                switch (stageIndex)
                {
                    case 0: startY = 95f; break;
                    case 1: startY = 135f; break;
                    case 2: startY = 195f; break;
                    default: startY = 95f; break;
                }

                initialized = true;
            }
        }

        UpdateProgressBar();
    }

    public void UpdateStageLabelsAndColors()
    {
        int currentStage = StageManager.Instance?.currentStageIndex ?? 0;
        int totalStages = StageManager.Instance?.stagePrefabs.Length ?? 1;

        int nextStage = (currentStage + 1) % totalStages;

        currentStageText.text = (currentStage + 1).ToString();
        nextStageText.text = (nextStage + 1).ToString();

        if (currentStageCircleImage != null)
            currentStageCircleImage.color = filledColor;

        if (nextStageCircleImage != null)
            nextStageCircleImage.color = unfilledColor;

        if (stageSliderImage != null)
        {
            stageSliderImage.type = Image.Type.Filled;
            stageSliderImage.fillMethod = Image.FillMethod.Horizontal;
            stageSliderImage.fillOrigin = (int)Image.OriginHorizontal.Left;
            stageSliderImage.fillAmount = 0f;
        }
    }

    private void UpdateProgressBar()
    {
        if (playerTransform == null || stageSliderImage == null) return;

        float t = Mathf.InverseLerp(startY, goalY, playerTransform.position.y);
        stageSliderImage.fillAmount = Mathf.Clamp01(t);
    }
}