using UnityEngine;

public class MapIconButton : MonoBehaviour
{
    [Header("非表示対象")]
    public GameObject mapIcon;

    private bool hasMoved = false;

    void Update()
    {
        if (hasMoved || mapIcon == null) return;

        // マウスが押されていて横方向に動いたとき
        if (Input.GetMouseButton(0) && Mathf.Abs(Input.GetAxis("Mouse X")) > 0.01f)
        {
            mapIcon.SetActive(false);
            hasMoved = true;
        }
    }

    public void OnClickMapIcon()
    {
        if (CloudTransitionManager.Instance != null)
        {
            CloudTransitionManager.Instance.GoToScene("TitleScene");
        }
    }
}