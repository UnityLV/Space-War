using UnityEngine;
using UnityEngine.UI;

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

    private static float _resetTimeCoefficient;
    
    public static float StartGameTime()
    {
        return UnityEngine.Time.time - _resetTimeCoefficient;
    }
    
    public static void ResetTime()
    {
        _resetTimeCoefficient = UnityEngine.Time.time;
    }
    
    public static Canvas GetCanvas()
    {
        GameObject canvasObject = GameObject.Find("Canvas");
        if (canvasObject == null)
        {
            canvasObject = new GameObject("Canvas");
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.AddComponent<CanvasScaler>();
            canvasObject.AddComponent<GraphicRaycaster>();
            
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }
        return canvasObject.GetComponent<Canvas>();
    }
}