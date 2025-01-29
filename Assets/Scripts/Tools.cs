using UnityEngine;

public static class Tools
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void InitSeed()
    {
        Random.InitState(123);
    }

    public static Vector2 AngleToVector2(float angle)
    {
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
    }
    
    public static Vector2 GetPositionOutsideScreen()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        screenCenter = Camera.main.ScreenToWorldPoint(screenCenter);
        return (AngleToVector2(Random.Range(0, 360)) * Random.Range(10, 20)) + screenCenter;
    }
}