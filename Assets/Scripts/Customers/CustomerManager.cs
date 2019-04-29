using UnityEngine;
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
    
    public Queue<GameObject> queue = new Queue<GameObject>();
    private float _timeToQueue = 1F;
    private bool _timeSet = false;
    private List<string> names = new List<string>();
    private int nextCharacterNumber;
    private string nextCharacter;
    
    private Clock instance; //get global time from clock

    public Dictionary<string,GameObject> Customers = new Dictionary<string, GameObject>();
    
    private Transform _childTransform;

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
            print("customer: " + cName);
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
            queue.Peek().SetActive(false);
            queue.Dequeue (); // destroy
            if (queue.Count > 0)
            {
                queue.Peek().transform.position = spawnPositions[0];
                queue.Peek().GetComponent<CustomerBrain>().myTurn = true;
            }
            else { firstSpawn = true; }
            next = false;
        }

        if (_timeToQueue <= 0)
            {_timeSet = false;}
        
        if (!(_timeToQueue <= 0) || queue.Count > maximumCustomer) return;
        
        //try to add/spawn new character in queue, if it fails to often it just leaves it be
        var enqueued = false;
        var countTries = 0;
        while (true)
        {
            nextCharacterNumber = Random.Range(0, names.Count);
            nextCharacter = names[nextCharacterNumber];
            print("nexCharNum: " + nextCharacterNumber + " | nextcharacter: " + nextCharacter);
            countTries++;
            if (countTries > names.Count + 3) break;
            //check if character is already enqueued
            if (!queue.Contains(Customers[nextCharacter]))
            {
                print("ququq: " + spawnPositions[Mathf.Max(0, queue.Count)]);

                var c = Customers[nextCharacter];
                //set a character active and add to queue
                c.SetActive(true);
                c.transform.position = spawnPositions[Mathf.Max(0, queue.Count)];
                queue.Enqueue(Customers[nextCharacter]);
                enqueued = true;
                break;
            }
        }
    }
}
