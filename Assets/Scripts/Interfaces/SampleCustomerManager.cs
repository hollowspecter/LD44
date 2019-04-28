using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SampleCustomerManager : MonoBehaviour, ICustomerProvider
{
    // variables
    public Account[] existingAccounts;
    public ICustomer currentCustomer;
    public GameObject[] customerPrefabs;
    public ICustomer[] customers;
    public Queue<ICustomer> customerQueue = new Queue<ICustomer>();
    public float spacing = 1f;
    public ICustomer leavingCustomer;
    public Transform exitDoor;
    public float speed = 1f;
    public List<GameObject> instances = new List<GameObject>();
    public bool allCustomersLeft = false;
    // interface properties
    Account[] ICustomerProvider.existingAccounts => existingAccounts;
    ICustomer ICustomerProvider.currentCustomer => currentCustomer;
    ICustomer[] ICustomerProvider.customers => customers;
    bool ICustomerProvider.allCustomersLeft => allCustomersLeft;

    // interface methods
    public ICustomer ProceedCustomer()
    {
        if(currentCustomer != null){
            StartCoroutine(LeaveCustomer(currentCustomer));
            currentCustomer = null;
        }
        if(customerQueue.Count < 1){
            return null;
        }
        return currentCustomer = customerQueue.Dequeue();
    }
    public SampleCustomer GetRandomCustomer(){
        int i = (int)(Random.value * customerPrefabs.Length);
        var g = Instantiate(customerPrefabs[i]);
        instances.Add(g);
        return g.GetComponent<SampleCustomer>();
    }

    public void CreateQueue(int length){
        allCustomersLeft = false;
        List<Account> list = new List<Account>();
        for(int i = 0; i < length; i++){
            var newCustomer = GetRandomCustomer();
            customerQueue.Enqueue(newCustomer);
            if(newCustomer.account != null){
                list.Add(newCustomer.account);
            }
        }
        customers = customerQueue.ToArray();
    }

    private void OnEnable(){
        currentCustomer = null;
    }

    private void Update(){
        if(currentCustomer != null){
            currentCustomer.position = Vector3.Lerp(currentCustomer.position,transform.TransformPoint(Vector2.zero),Time.deltaTime*speed);
        }
        int i = 1;
        foreach(var c in customerQueue){
            c.position = Vector3.Lerp(c.position,transform.TransformPoint(Vector3.forward * spacing * i),Time.deltaTime*speed);
            i++;
        }
    }
    private IEnumerator LeaveCustomer(ICustomer customer){
        customer.OnLeaveDesk();
        float t = 0;
        Vector3 pos = customer.position;
        while(t < 1f){
            t+=Time.deltaTime * speed;
            customer.position = Vector3.Lerp(pos,exitDoor.position,t);
            yield return null;
        } 
        if(customerQueue.Count < 1 && currentCustomer == null){
            allCustomersLeft = true;
            foreach(var g in instances){
                g.SetActive(false);
            }
            instances.Clear();
        }
    }
}