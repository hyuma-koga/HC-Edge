using UnityEngine;

public class PlayButton : MonoBehaviour
{
    public void OnClickPlay()
    {
        if (CloudTransitionManager.Instance != null)
        {
            CloudTransitionManager.Instance.GoToScene("GameScene");
        }
    }
}