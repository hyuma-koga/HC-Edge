using UnityEngine;

public class ColorSplash : MonoBehaviour
{
    public Sprite splashSprite;
    public static Sprite staticSplashSprite;

    private void Awake()
    {
        staticSplashSprite = splashSprite;
    }

    public static void Create(Vector3 position, Color tintColor, Transform parent = null)
    {
        if (staticSplashSprite == null)
        {
            Debug.LogWarning("スプラッシュ画像(Sprite)が設定されていません");
            return;
        }

        GameObject splash = new GameObject("ColorSplash");
        splash.transform.position = position + Vector3.forward * 0.01f;
        splash.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        if (parent != null)
        {
            splash.transform.SetParent(parent);
        }

        SpriteRenderer sr = splash.AddComponent<SpriteRenderer>();
        sr.sprite = staticSplashSprite;
        sr.color = tintColor;
        sr.sortingOrder = 100;
        splash.AddComponent<SelfDestruct>().lifeTime = 1f;
    }
}