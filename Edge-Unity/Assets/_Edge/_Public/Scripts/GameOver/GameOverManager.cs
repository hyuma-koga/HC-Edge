using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;

    [Header("UI�Q��")]
    public GameObject gameOverUI;

    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// �Q�[���I�[�o�[���o�̊J�n
    /// </summary>
    public void TriggerGameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
    }

    void Update()
    {
        if (!isGameOver) return;

        // �N���b�N�Ń^�C�g����
        if (Input.GetMouseButtonDown(0))
        {
            GoToTitle();
        }
    }

    private void GoToTitle()
    {
        if (CloudTransitionManager.Instance != null)
        {
            CloudTransitionManager.Instance.GoToScene("TitleScene");
        }
        else
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
}