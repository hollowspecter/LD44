using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Draggable : MonoBehaviour {
    public Rigidbody rigid;
    public enum Type {
        Generic,
        Coin,
        Bill,
        GoldBar,
        SilverBar
    }
    public float value = 0f;
    public Type type = Type.Generic;
    private void Awake(){
        rigid = GetComponent<Rigidbody>();
    }
}