using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispensary : MonoBehaviour {
    public Draggable[] changePrefabs;
    public TriggerProxy trigger;
    public Transform outlet;
    private Coroutine dispenseRoutine;
    public float dispenseForce = 1f;
    public float dispenseDelay = 0.3f;
    private Queue<Draggable> dispenseQueue = new Queue<Draggable>();
    private IDraggableReceiver receiver;
    public float kept = 0f;
    public float dispensed = 0f;
    private void OnEnable(){
        SortChange();
        dispenseRoutine = StartCoroutine(DispenseRoutine());
        trigger.stay = (c)=>OnTriggerStay(c);
        kept = 0f;
        dispensed = 0f;
    }
    private void OnDisable(){
        receiver = null;
    }

    public void RegisterReceiver(IDraggableReceiver receiver){
        if(this.receiver != null){
            throw new Exception("Only one receiver per Dispensary");
        }
        this.receiver = receiver;
    }
    public void RemoveReceiver(IDraggableReceiver receiver){
        if(this.receiver != receiver){
            throw new Exception("Removing wrong receiver!");
        }
        this.receiver = null;
    }

    public void SetChangePrefabs(Draggable[] changePrefabs){
        this.changePrefabs = changePrefabs;
        SortChange();
    }

    private void OnTriggerStay(Collider collider){
        Draggable drag = collider.GetComponent<Draggable>();
        if(drag){
            if(receiver != null && receiver.OnReceivedDraggable(drag)){
                Keep(drag);
            }else{
                DispenseQueued(drag);
            }
        }
    }

    private void Keep(Draggable drag){
        kept += drag.value;
        Destroy(drag.gameObject);
    }

    private void SortChange(){
        float[] values = new float[changePrefabs.Length];
        for(int i = 0; i < values.Length; i++){
            values[i] = changePrefabs[i].value;
        }
        Array.Sort(values,changePrefabs);
    }

    public float DispenseChange(float amount){
        float dispensed = 0;
        if(changePrefabs.Length > 0){
            for(int i = changePrefabs.Length-1; i >= 0;i--){
                var prefab = changePrefabs[i];
                for(int count = 1;prefab.value <= amount; count++){
                    Draggable drag = Instantiate<Draggable>(prefab);
                    dispensed += drag.value;
                    amount -= drag.value;
                    DispenseQueued(drag);
                    if(count > 2){
                        if(i > 0){
                            var next = changePrefabs[i-1];
                            if(next.value == prefab.value){
                                break;
                            }
                        }
                    }
                }
            }
        }
        this.dispensed += dispensed;
        return dispensed;
    }
    private void DispenseQueued(Draggable drag){    
        drag.gameObject.SetActive(false);    
        dispenseQueue.Enqueue(drag);
    }
    private IEnumerator DispenseRoutine(){
        while(enabled){
            if(dispenseQueue.Count > 0){
                var drag = dispenseQueue.Dequeue();
                drag.gameObject.SetActive(true);
                drag.transform.position = outlet.position;
                drag.rigid.position = outlet.position;
                drag.rigid.velocity = Vector3.zero;
                drag.rigid.angularVelocity = Vector3.zero;
                drag.rigid.AddForce(outlet.forward*dispenseForce, ForceMode.VelocityChange);
            }
            yield return new WaitForSeconds(dispenseDelay);
        }
    }
}