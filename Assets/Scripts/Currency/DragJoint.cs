using System;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class DragJoint : MonoBehaviour {
    public int layer = 0;
    public float lift = 0.01f;
    private float lerpedLift = 0f;
    private ConfigurableJoint joint;
    private Vector3 dragStart;
    private Rigidbody rigid;
    private RaycastHit hitInfo;

    private void Awake(){
        rigid = GetComponent<Rigidbody>();
    }

    private void Update(){
        if(MainCamera.ScreenPointRaycast(GameInput.position, out hitInfo, 1 << layer)){
            if(GameInput.BeginTouch()){
                Draggable drag = hitInfo.collider.GetComponent<Draggable>();
                if(drag){
                    dragStart = hitInfo.point;
                    joint = gameObject.AddComponent<ConfigurableJoint>();
                    joint.xMotion = ConfigurableJointMotion.Locked;
                    joint.yMotion = ConfigurableJointMotion.Locked;
                    joint.zMotion = ConfigurableJointMotion.Locked;
                    joint.connectedBody = drag.rigid;
                }
            }
        }
        if(GameInput.EndTouch()){
            Destroy(joint);
        }
    }
    private void FixedUpdate(){
        if(joint){
            lerpedLift = Mathf.Lerp(lerpedLift, lift, Time.fixedDeltaTime*8f);
            Vector3 worldPos;
            if(MainCamera.ScreenPointRaycast(GameInput.position, out worldPos, dragStart.y)){
                MovePosition(worldPos);                  
            }
        } else{
            lerpedLift = Mathf.Lerp(lerpedLift, 0f, Time.fixedDeltaTime*8f);
            if(hitInfo.point != Vector3.zero){
                MovePosition(hitInfo.point);      
            }else{
                Vector3 worldPos;
                if(MainCamera.ScreenPointRaycast(GameInput.position, out worldPos, dragStart.y)){
                    MovePosition(worldPos);                    
                }
            }
        }
    }
    private void MovePosition(Vector3 position){
        rigid.MovePosition(position+Vector3.up*lerpedLift);
    }
}