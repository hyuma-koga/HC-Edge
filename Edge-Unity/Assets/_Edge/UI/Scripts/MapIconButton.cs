using UnityEngine;

public class MapIconButton : MonoBehaviour
{
    [Header("”ñ•\Ž¦‘ÎÛ")]
    public GameObject mapIcon;

    private bool hasMoved = false;

    void Update()
    {
        if (hasMoved || mapIcon == null)
        {
            return;
        }

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