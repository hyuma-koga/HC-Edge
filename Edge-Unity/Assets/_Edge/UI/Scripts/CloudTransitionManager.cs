using UnityEngine;
using UnityEngine.SceneManagement;

public class CloudTransitionManager : MonoBehaviour
{
    public static CloudTransitionManager Instance;

    [Header("UI�ݒ�")]
    public RectTransform cloudPanel;
    public Canvas cloudCanvas;

    [Header("�J�ڐݒ�")]
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

    /// <summary>
    /// �ʏ�̃V�[���J�ڗp�X���C�h�J�n
    /// </summary>
    public void GoToScene(string sceneName)
    {
        if (currentState != State.None) return;

        targetScene = sceneName;
        loadScenePending = true;

        StartSlide();
    }

    /// <summary>
    /// �X���C�h���o�̂݁i�V�[���J�ڂȂ��j
    /// </summary>
    public void StartSlideOnly()
    {
        if (currentState != State.None) return;

        targetScene = "";
        loadScenePending = false;

        StartSlide();
    }

    /// <summary>
    /// �w��b��ɑJ�ډ��o���J�n
    /// </summary>
    public void GoToSceneWithDelay(float delaySeconds, string sceneName)
    {
        targetScene = sceneName;
        loadScenePending = true;

        Invoke(nameof(StartSlide), delaySeconds);
    }

    /// <summary>
    /// �_���o�X���C�h�J�n�����i�����Ăяo���p�j
    /// </summary>
    private void StartSlide()
    {
        cloudCanvas.gameObject.SetActive(true);
        cloudPanel.anchoredPosition = rightPos;

        currentState = State.SlideIn;
        elapsedTime = 0f;
    }

    void Update()
    {
        if (cloudPanel == null || cloudCanvas == null) return;

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
        cloudCanvas.sortingOrder = 1000; // ���o�p�p�l�����w�ʂɉ��Ȃ��悤�ی��I�Ɉێ�
    }
}