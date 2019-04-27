using System;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class MainCamera : MonoBehaviour {
    public static Camera cam;

    private void Awake(){
        if(cam != null){
            throw new Exception("Only one MainCamera allowed!");
        }
        cam = GetComponent<Camera>();
    }

    public static bool ScreenPointRaycast(Vector2 screenPoint, out RaycastHit hitInfo, int layerMask){
        Ray r = cam.ScreenPointToRay(screenPoint);
        //Debug.DrawRay(r.origin,r.direction,Color.red,3f);
        if(Physics.Raycast(r.origin,r.direction,out hitInfo,2f,layerMask)){
            return true;
        }
        return false;
    }
    public static bool ScreenPointRaycast(Vector2 screenPoint, out Vector3 worldPos, float height){
        Ray r = cam.ScreenPointToRay(screenPoint);
        //Debug.DrawRay(r.origin,r.direction,Color.red,1f);
        Plane plane = new Plane(Vector3.up,Vector3.up*height);
        float enter;
        if(plane.Raycast(r, out enter)){                 
            worldPos = r.origin+r.direction*enter;       
            return true;
        }
        worldPos = Vector3.zero;
        return true;
    }
}