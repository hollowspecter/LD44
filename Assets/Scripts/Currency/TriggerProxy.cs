using UnityEngine;
[RequireComponent(typeof(Collider))]
public class TriggerProxy : MonoBehaviour {
    public delegate void OnTrigger(Collider collider);
    public OnTrigger enter;
    public OnTrigger stay;
    public OnTrigger exit;
    
    private void OnTriggerEnter(Collider collider){
        if(enter == null){return;}
        enter(collider);
    }
    private void OnTriggerStay(Collider collider){
        if(stay == null){return;}
        stay(collider);
    }
    private void OnTriggerExit(Collider collider){
        if(exit == null){return;}
        exit(collider);
    }
}