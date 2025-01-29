using UnityEngine;

public class InputReader
{
    public static Vector2 ReadInput()
    {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    public static Vector3 ReadMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}