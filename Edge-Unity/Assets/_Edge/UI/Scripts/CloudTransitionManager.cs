using UnityEngine;
using UnityEngine.SceneManagement;

public class CloudTransitionManager : MonoBehaviour
{
    public static CloudTransitionManager Instance;

    [Header("UI設定")]
    public RectTransform cloudPanel;
    public Canvas cloudCanvas;

    [Header("遷移設定")]
    public float slideDuration = 0.5f;
    public float centerWaitDuration = 0.5f;

    private Vector2 rightPos = new Vector2(720f, 0f);
    private Vector2 centerPos = new Vector2(0f, 0f);
    private Vector2 leftPos = new Vector2(-720f, 0f);

    private string targetScene = "";
    private float elapsedTime = 0f;
    private bool loadScenePending = false;

    private enum State { None, SlideIn, Wait, SlideOut }
    private State currentState = State.None;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// 通常のシーン遷移用スライド開始
    public void GoToScene(string sceneName)
    {
        if (currentState != State.None) return;

        targetScene = sceneName;
        loadScenePending = true;

        StartSlide();
    }

    /// スライド演出のみ（シーン遷移なし）
    public void StartSlideOnly()
    {
        if (currentState != State.None) return;

        targetScene = "";
        loadScenePending = false;

        StartSlide();
    }

    /// 指定秒後に遷移演出を開始
    public void GoToSceneWithDelay(float delaySeconds, string sceneName)
    {
        targetScene = sceneName;
        loadScenePending = true;

        Invoke(nameof(StartSlide), delaySeconds);
    }

    /// 雲演出スライド開始処理（内部呼び出し用）
    private void StartSlide()
    {
        cloudCanvas.gameObject.SetActive(true);
        cloudPanel.anchoredPosition = rightPos;

        currentState = State.SlideIn;
        elapsedTime = 0f;
    }

    void Update()
    {
        if (cloudPanel == null || cloudCanvas == null)
        {
            return;
        }

        switch (currentState)
        {
            case State.SlideIn:
                elapsedTime += Time.deltaTime;
                cloudPanel.anchoredPosition = Vector2.Lerp(rightPos, centerPos, elapsedTime / slideDuration);

                if (elapsedTime >= slideDuration)
                {
                    currentState = State.Wait;
                    elapsedTime = 0f;

                    if (loadScenePending && !string.IsNullOrEmpty(targetScene))
                    {
                        SceneManager.sceneLoaded += OnSceneLoaded;
                        SceneManager.LoadScene(targetScene);
                    }
                }
                break;

            case State.Wait:
                elapsedTime += Time.deltaTime;

                if (elapsedTime >= centerWaitDuration)
                {
                    currentState = State.SlideOut;
                    elapsedTime = 0f;
                }
                break;

            case State.SlideOut:
                elapsedTime += Time.deltaTime;
                cloudPanel.anchoredPosition = Vector2.Lerp(centerPos, leftPos, elapsedTime / slideDuration);

                if (elapsedTime >= slideDuration)
                {
                    currentState = State.None;
                    cloudCanvas.gameObject.SetActive(false);
                }
                break;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        cloudCanvas.sortingOrder = 1000; // 演出用パネルが背面に回らないよう保険的に維持
    }
}