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
            Vector3 pillarXZ = stageTopPoints[selectedIndex].position;
            float fixedY = 3.8f;
            Vector3 point = new Vector3(pillarXZ.x, fixedY, pillarXZ.z);

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