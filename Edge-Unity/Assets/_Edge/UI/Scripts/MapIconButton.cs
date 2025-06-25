using UnityEngine;

public class MapIconButton : MonoBehaviour
{
    public void OnClickMapIcon()
    {
        if (CloudTransitionManager.Instance != null)
        {
            CloudTransitionManager.Instance.GoToScene("TitleScene");
        }
    }
}