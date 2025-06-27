using UnityEngine;
using UnityEngine.UI;

public class StageProgressBarController : MonoBehaviour
{
    [Header("�X�e�[�W�ԍ��\��")]
    public Text currentStageText;
    public Text nextStageText;

    [Header("�X�e�[�W�ۉ摜")]
    public Image currentStageCircleImage;
    public Image nextStageCircleImage;

    [Header("�i���X���C�_�[")]
    public Image stageSliderImage;

    [Header("�F�ݒ�")]
    public Color filledColor = new Color(1f, 0.6f, 0f); // �I�����W
    public Color unfilledColor = Color.gray;

    [Header("�i���̊")]
    public Transform playerTransform;
    public float startY = 0f;
    public float goalY = 100f;

    private bool initialized = false;

    private void Start()
    {
        goalY = 0f; // ���ʂŌŒ�
        UpdateStageLabelsAndColors();
    }

    private void Update()
    {
        // ����̂݃v���C���[���������ď�����
        if (!initialized)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;

                // �X�e�[�W�ɉ����� startY ��ݒ�
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