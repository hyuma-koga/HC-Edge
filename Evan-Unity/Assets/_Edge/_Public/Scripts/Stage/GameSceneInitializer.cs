using UnityEngine;

public class GameSceneInitializer : MonoBehaviour
{
    void Start()
    {
        StageManager.Instance?.LoadStage(StageManager.Instance.currentStageIndex);
        var camFollow = Camera.main.GetComponent<CameraFollow>();

        if (camFollow != null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            GameObject centerObj = GameObject.FindWithTag("TowerCenter");

            if (playerObj != null)
            {
                camFollow.target = playerObj.transform;
            }
                
            if (centerObj != null)
            {
                camFollow.center = centerObj.transform;
            } 
        }

        ScoreManager.Instance?.SetScoreUIActive(true);
    }
}