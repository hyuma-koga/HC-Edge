using UnityEngine;

public class GameSceneInitializer : MonoBehaviour
{
    void Start()
    {
        // �X�e�[�W����
        StageManager.Instance?.LoadStage(StageManager.Instance.currentStageIndex);
        var camFollow = Camera.main.GetComponent<CameraFollow>();

        if (camFollow != null)
        {
            // �X�e�[�W���� Player�iTag: Player ������j
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

        // �X�R�AUI�\���i����������ɗL���Ɂj
        ScoreManager.Instance?.SetScoreUIActive(true);
    }
}