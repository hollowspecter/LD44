using System;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class DragJoint : MonoBehaviour {
    public int layer = 0;
    public float lift = 0.01f;
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
            Vector3 worldPos;
            if(MainCamera.ScreenPointRaycast(GameInput.position, out worldPos, dragStart.y)){
                rigid.MovePosition(worldPos+Vector3.up*lift);
            }
        } else{
            if(hitInfo.point != Vector3.zero){
                rigid.MovePosition(hitInfo.point+Vector3.up*lift);
            }else{
                Vector3 worldPos;
                if(MainCamera.ScreenPointRaycast(GameInput.position, out worldPos, dragStart.y)){
                    rigid.MovePosition(worldPos+Vector3.up*lift);
                }
            }
        }
    }
}