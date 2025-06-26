using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    public GameObject[] stagePrefabs;
    public Transform stageSpawnPoint;

    public int currentStageIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void LoadStage(int index)
    {
        currentStageIndex = index;

        foreach (Transform child in stageSpawnPoint)
        {
            Destroy(child.gameObject);
        }

        if (index >= 0 && index < stagePrefabs.Length)
        {
            Instantiate(stagePrefabs[index], stageSpawnPoint.position, Quaternion.identity, stageSpawnPoint);
        }
    }

    public int GetNextStageIndex()
    {
        return (currentStageIndex + 1) % stagePrefabs.Length;
    }

    public void SaveNextStageToPrefs()
    {
        int nextStage = GetNextStageIndex();
        PlayerPrefs.SetInt("SelectedStageIndex", nextStage);
    }
}
