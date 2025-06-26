using UnityEngine;

public class GameSceneInitializer : MonoBehaviour
{
    void Start()
    {
        // ステージ生成
        StageManager.Instance?.LoadStage(StageManager.Instance.currentStageIndex);
        var camFollow = Camera.main.GetComponent<CameraFollow>();

        if (camFollow != null)
        {
            // ステージ内の Player（Tag: Player を仮定）
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

        // スコアUI表示（初期化直後に有効に）
        ScoreManager.Instance?.SetScoreUIActive(true);
    }
}