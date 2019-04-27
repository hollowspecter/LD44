using UnityEngine;

public class GameInput {
    public static Vector2 position {
        get {
            return Input.mousePosition;
        }
    }

    public static bool BeginTouch(){
        return Input.GetMouseButtonDown(0);
    }
    public static bool EndTouch(){
        return Input.GetMouseButtonUp(0);
    }
}