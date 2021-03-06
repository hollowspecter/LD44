﻿using UnityEngine;
using System.Collections.Generic;
using UniRx;

public class CustomerManager : MonoBehaviour
{
    public static bool next;
    public CustomerGenEvent[] cGen;
    public int maximumCustomer = 5;
    public float minTime;
    public float maxTime;
    public Vector3[] spawnPositions;

    public LinkedList<GameObject> llQueue= new LinkedList<GameObject>();
    private float _timeToQueue = 1F;
    private bool _timeSet = false;
    private List<string> names = new List<string>();
    private int nextCharacterNumber;
    private string nextCharacter;
    private bool firstSpawn = true;
    
    private Clock instance; //get global time from clock

    public Dictionary<string,GameObject> Customers = new Dictionary<string, GameObject>();
    
    private Transform _childTransform;

    private void OnEnable()
    {
        next = false;
    }

    private void Start()
    {
        // Deactivate spawner if end of day is reached
        App.instance.EndOfDayActive
            .Subscribe ( x => { if ( x ) gameObject.SetActive ( false ); } )
            .AddTo ( this );

        Spawn();
    }

    //has customer an account?
    //-> gets created in CustomerGenEvent
    protected virtual void Spawn()
    {
        foreach (var customer in cGen)
        {
            var c = customer.GenerateCustomer();
            var cName = c.GetComponent<CustomerBrain>().customerName;
            Customers.Add(cName,c);
            Customers[cName].SetActive(false);
            names.Add(cName);
        }
    }

    private void Update()
    {
        if (!_timeSet)
        {
            _timeToQueue = Random.Range(minTime, maxTime);
            _timeSet = true;
        }
        _timeToQueue -=Time.deltaTime;

        //if the CustomerBrain knows it is done and has moved away -> dequeue it 
        if (next)
        {
            llQueue.First.Value.SetActive(false);
            llQueue.RemoveFirst();

            if (llQueue.Count > 0)
            {
                UpdateQueuePositions();
                llQueue.First.Value.GetComponent<CustomerBrain>().myTurn = true;
            }
            else { firstSpawn = true; }
            next = false;
        }

        if (_timeToQueue <= 0)
            {_timeSet = false;}

        if (!(_timeToQueue <= 0) || llQueue.Count > maximumCustomer) return;
//        if (!(_timeToQueue <= 0) || queue.Count > maximumCustomer) return;
        
        //try to add/spawn new character in queue, if it fails to often it just leaves it be
        var enqueued = false;
        var countTries = 0;
        while (true)
        {
            nextCharacterNumber = Random.Range(0, names.Count);
            nextCharacter = names[nextCharacterNumber];
            
            countTries++;
            if (countTries > names.Count + 3) break;

            if (!llQueue.Contains(Customers[nextCharacter]))
            {
                var c = Customers[nextCharacter];
                //set a character active and add to queue
                c.SetActive(true);
                c.transform.position = spawnPositions[Mathf.Max(0, llQueue.Count)];
                if (firstSpawn)
                {
                    c.GetComponent<CustomerBrain>().myTurn = true;
                    firstSpawn = false;
                }

                llQueue.AddLast(Customers[nextCharacter]);
                enqueued = true;
                break;
            }   
        }
    }

    private void UpdateQueuePositions()
    {
        llQueue.First.Value.transform.position = spawnPositions[0];
        if (llQueue.First.Next != null) llQueue.First.Next.Value.transform.position = spawnPositions[1];
        if (llQueue.First.Next == null) return;
        if (llQueue.First.Next.Next == null) return;
        llQueue.First.Next.Next.Value.transform.position = spawnPositions[2];
        if (llQueue.First.Next.Next.Next != null)
            llQueue.First.Next.Next.Next.Value.transform.position = spawnPositions[3];
    }
}