using UnityEngine;

public class StageSelectorController : MonoBehaviour
{
    public Transform[] stageTopPoints;
    public GameObject playerSelector;

    void Start()
    {
        int selectedIndex = PlayerPrefs.GetInt("SelectedStageIndex", 0);

        if (selectedIndex >= 0 && selectedIndex < stageTopPoints.Length && playerSelector != null)
        {
            Vector3 point = stageTopPoints[selectedIndex].position + Vector3.up * 1.5f; // ’Œ‚Ìã + ­‚µã

            var bounce = playerSelector.GetComponent<PlayerSelectorBounce>();
            if (bounce != null)
            {
                bounce.SetBasePosition(point);
            }
            else
            {
                playerSelector.transform.position = point;
            }
        }
    }
}