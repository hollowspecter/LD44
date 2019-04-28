using System;
using UnityEngine;

public class SampleCustomer : MonoBehaviour, ICustomer, IDraggableReceiver
{
    // variables
    public Account account;
    public Dispensary customerDesk;
    public Draggable[] changePrefabs;
    public float withDraw = 0f;
    public float withDrawn = 0f;
    public float deposit = 0f;
    public bool deposited = false;
    public bool fulfilledNeed {
        get {
            return withDrawn >= withDraw && deposited;
        }
    }  

    // interface properties
    float ICustomer.happiness => fulfilledNeed?1f:0f;
    bool ICustomer.fulfilledNeed => fulfilledNeed;
    Account ICustomer.account => account;    
    public Vector3 position { get => transform.position; set => transform.position = value; }

    // interface methods
    public void OnAccountCreated(Account account)
    {
        this.account = account;
    }

    public void OnArrivedAtDesk(Dispensary customerDesk)
    {
        this.customerDesk = customerDesk;
        customerDesk.RegisterReceiver(this);
        customerDesk.SetChangePrefabs(changePrefabs);
    }

    public void OnLeaveDesk()
    {
        customerDesk.RemoveReceiver(this);
    }

    public void OnDialogueTrigger(string trigger)
    {
        if(customerDesk){
            if(trigger == "deposit"){
                if(deposited){ throw new Exception("Already deposited money!"); }
                customerDesk.DispenseChange(deposit);
                deposited = true;
            }
        }
    }

    public bool OnReceivedDraggable(Draggable drag)
    {
        switch(drag.type){
            case Draggable.Type.Bill:
            case Draggable.Type.Coin:
            case Draggable.Type.GoldBar:
            case Draggable.Type.SilverBar:{
                withDrawn += drag.value;
                return true;
            }
            case Draggable.Type.Generic:{
                return false;
            }
            default: {
                throw new Exception("Unhandled Draggable!");
            }
        }
    }
}