using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Draggable : MonoBehaviour {
    public Rigidbody rigid;
    private void Awake(){
        rigid = GetComponent<Rigidbody>();
    }
}