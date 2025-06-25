using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonHandler : MonoBehaviour
{
    public RectTransform cloudPanel; // 雲のパネル（Image）
    public Canvas cloudCanvas;       // 雲Canvas（他UIとは別）

    public float slideDuration = 0.5f;
    public float centerWaitDuration = 0.5f;
    public string nextSceneName = "GameScene";

    private enum SlideState { None, SlideIn, Waiting, SlideOut }
    private SlideState currentState = SlideState.None;

    private Vector2 rightPos = new Vector2(720f, 0f);
    private Vector2 centerPos = new Vector2(0f, 0f);
    private Vector2 leftPos = new Vector2(-720f, 0f);

    private float elapsedTime = 0f;

    private static PlayButtonHandler persistentInstance;

    public void OnClickPlay()
    {
        if (persistentInstance == null)
        {
            persistentInstance = this;
            DontDestroyOnLoad(cloudCanvas.gameObject); // Canvasごと保持
        }

        cloudPanel.anchoredPosition = rightPos;
        currentState = SlideState.SlideIn;
        elapsedTime = 0f;
    }

    void Update()
    {
        switch (currentState)
        {
            case SlideState.SlideIn:
                elapsedTime += Time.deltaTime;
                float tIn = Mathf.Clamp01(elapsedTime / slideDuration);
                cloudPanel.anchoredPosition = Vector2.Lerp(rightPos, centerPos, tIn);

                if (tIn >= 1f)
                {
                    currentState = SlideState.Waiting;
                    elapsedTime = 0f;
                    SceneManager.sceneLoaded += OnSceneLoaded;
                    SceneManager.LoadScene(nextSceneName);
                }
                break;

            case SlideState.Waiting:
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= centerWaitDuration)
                {
                    currentState = SlideState.SlideOut;
                    elapsedTime = 0f;
                }
                break;

            case SlideState.SlideOut:
                elapsedTime += Time.deltaTime;
                float tOut = Mathf.Clamp01(elapsedTime / slideDuration);
                cloudPanel.anchoredPosition = Vector2.Lerp(centerPos, leftPos, tOut);

                if (tOut >= 1f)
                {
                    Destroy(cloudCanvas.gameObject); // 最終的に破棄
                    persistentInstance = null;
                    currentState = SlideState.None;
                }
                break;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        Canvas targetCanvas = Object.FindFirstObjectByType<Canvas>();
        if (targetCanvas != null && cloudCanvas != null)
        {
            cloudCanvas.sortingOrder = 1000; // 雲が前面に出るように（任意）
        }
    }
}