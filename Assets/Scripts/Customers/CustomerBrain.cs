using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UniRx;

public class CustomerBrain : MonoBehaviour, IDraggableReceiver
{
    private ICustomer cI;
    public string customerName;
    public Sprite graphic;
    public RangedFloat maxTimeRange;
    public RangedFloat moneyToUse;
    public int moneyRange;
    public string action;
    public string accountNumber;
    public int hapinessLevel = 5; //from 0 to 3. 5 is super chill 0 is pissed af
    public bool moreMoney;

    public bool myTurn = false; //if true, it will go to the counter
    public bool amDone = false; //if true, it will go away and will deactivates itself
    public Dispensary dispensary;
    public Yarn.Unity.DialogueRunner dialogueRunner;

    private CompositeDisposable disposables = new CompositeDisposable ();

    private enum NeedType
    {
        deposit,
        withdraw,
        robbery,
        makeAccount
    }

    private NeedType need;
    
    private float _maxTime;
    private float _timePast;
    private int _moneyWanting;
    private int _money;
    private int _fundCheck;
    private bool introduced = false;

    // Properties
    private bool HasAccount { get { return TellerMachine.Instance.accounts.ContainsKey ( accountNumber ); } }
    private int AccountMoney { get { return ( int ) TellerMachine.Instance.accounts [ accountNumber ].balance; } }

    private void OnEnable()
    {
        // Trigger the Sound
        SoundManager.Instance.m_doorOpen.PlaySound ();

        //TODO: check their Account -> Needs the money tracking system

//        if (!cI.hasAccount)
//        {
//            need = NeedType.makeAccount;
//        }

        amDone = false;
        introduced = false;
        hapinessLevel = 5;

        //setup their action they want to do
        if (Random.Range(0, 100) > 95) //5 percent chance
        {
            need = NeedType.robbery;
        }
        else
        {
            var temp = Random.Range(0, 2); //int Rand.Range is exclusive
            need = (NeedType)temp;
        }
        action = need.ToString();

        //set the wait time to wait
        _maxTime = Random.Range(maxTimeRange.minValue,maxTimeRange.maxValue)*5;

        //set money wanting to use
        //set money to use in range of wanting to use(difference!)
        _moneyWanting = (int)Random.Range(moneyToUse.minValue,moneyToUse.maxValue);
        _money = _moneyWanting + Random.Range(-moneyRange, moneyRange);

        // if the account has no money, they will change their action to deposit
        if (AccountMoney <= 1f)
        {
            action = "deposit";
        }
 
        //if they want to withdraw/exchange/transfer and "money" is higher than the total
        //on their bank account, the total will be the new "money"
        if (AccountMoney < _money && AccountMoney != 0)
        {
            _money = AccountMoney;
        }

        _fundCheck = AccountMoney;
    }

    private void OnDisable()
    {
        // Trigger door close sound
        SoundManager.Instance.m_doorClose.PlaySound ();
    }

    private void Update()
    {
        //as long as in the queue, time is of essence. If it is their turn, they will move to counter
        //TODO: set this somewhere to true
        if (amDone)
        {
            AngerManagment();
            dialogueRunner.Stop ();
            StartCoroutine(LeaveCounter());
            amDone = false;
            disposables.Dispose ();
            return;
        }

        if(!myTurn) 
        {
            WaitingInQueue();
            return;
        }

        if (!introduced)
        {
            Introduce();
            introduced = true;
        }

        if (moreMoney && action == "deposit")
        {
            //_money = (_moneyWanting/2) + Random.Range(-moneyRange, moneyRange);
            //plz don't overwrite this again
            if(_money>0){
                GiveMoney();
            }else{
                dialogueRunner.dialogueUI.InstantMessage("I already gave you my money!");
            }
            moreMoney = false;
        }


    }

    private void WaitingInQueue()
    {
        _timePast+= Time.deltaTime;
        
        if (_timePast > _maxTime * 0.7)
            {hapinessLevel--;print("I am getting impatient!");}
        
        if (_timePast > _maxTime)
        {
            print("I am going home!");
            hapinessLevel = hapinessLevel-2;
            amDone = true;
        }
    }
    #region switch cases
    private void Introduce()
    {
        // subscribe to buttons
        // unsubscription is automatically done when amdone is pressed
        CustomerInteractionUI.Instance.SubscribeCustomer (
            // called when moremoney button is pressed
            () => moreMoney = true,
            // called when amdone button is pressed
            () => { amDone = true; } );

        // Subcribe to end of day to rush out when the bank closes
        App.instance.EndOfDayActive
            .Subscribe ( x => { if ( x ) amDone = true; } )
            .AddTo ( disposables );

        

        //possible: needs to give you more money
        //possible: can get money
        //possible: can rob
        switch (action)
        {
            case "deposit":
            {
                    //speechBubble.text = "Hello my name is " + customerName + " and I want to " + action +" "+ _money +
                    //                    " Moneys! my Account Number is " + accountNumber + ".";

                    PostitUI.Instance.PublishToPostit ( string.Format ( "{0} ${1} TO {2}",
                            action, _money, accountNumber ) );
                    GiveMoney();
                    // start dialogue
                    dialogueRunner.StartDialogue ();
                    break;
            }

            case "withdraw":
            {
                    //speechBubble.text = "Hello my name is " + customerName + " and I want to " + action +" "+ _money +
                    //                    " Moneys! my Account Number is " + accountNumber + ".";
                    PostitUI.Instance.PublishToPostit ( string.Format ( "{0} ${1} FROM {2}",
                            action, _money, accountNumber ) );
                    // start dialogue
                    dialogueRunner.StartDialogue ();
                    break;
            }
            case "robbery":
            {
                    // TODO: DOESNT MAKE SENSE WITH ONLY THE POST IT?! 

                    //speechBubble.text = "Hands in the air! I want to have "+ _money +
                    //                    " Moneys! Give it to me now!";
                    PostitUI.Instance.PublishToPostit ( string.Format ( "GIVE ROBBER ${0}", _money ) );
                    _fundCheck = 0;
                    // start dialogue
                    dialogueRunner.StartDialogue ("Robbery");
                    break;
            }
            case "makeAccount":
            {
                    //speechBubble.text = "Hi I want to make an Account! My name is: " + customerName;
                    PostitUI.Instance.PublishToPostit ( string.Format ( "make account for {0}", customerName ) );
                    // start dialogue
                    dialogueRunner.StartDialogue ();
                    break;
            }
            default:
                break;
        }
    }
    private void AngerManagment()
    {
        //check if everything went right
        //increase angrinessLevel if necessary
        switch (action)
        {
            case "deposit":
                if (AccountMoney == _moneyWanting + _fundCheck)
                {
                    break;
                }
                else
                {
                    hapinessLevel -= 2;
                    break;
                }
            case "withdraw":
                if (AccountMoney == _fundCheck - _moneyWanting)
                {
                    break;
                }
                else
                {
                    hapinessLevel -= 2;
                    break;
                }
            case "robbery":
                if (_fundCheck >= _moneyWanting)
                {
                    break;
                }
                else
                {
                    hapinessLevel -= 2;
                    break;
                }
            case "makeAccount":
                if (cI.hasAccount)
                {
                    break;
                }
                else
                {
                    hapinessLevel -= 3;
                    break;
                }
            case "lastStraw":
                if (!gameObject.GetComponentInChildren<TimerDialogue> ().lastStrawD )
                {
                    break;
                }
                else
                {
                    hapinessLevel -= 3;
                    break;
                }                
            default:
                break;
        }
    }
    #endregion

    IEnumerator LeaveCounter()
    {
        Tween leaveTween = transform.DOMoveX(transform.position.x+-10, 1);
        yield return leaveTween.WaitForCompletion();

        App.instance.score.happiness = App.instance.score.happiness+hapinessLevel;
        myTurn = false;
        CustomerManager.next = true;
    }

    //TODO: throws money amount. Look at how Sam dispenses with his Teller machine
    private void GiveMoney()
    {
        //throw amount of _money
        dispensary.DispenseChange(_money);
        _money = 0;
    }
    
    //TODO: able to give customers money
    private void GetMoney(int amount)
    {
        //increase _fundCheck 
        _money += amount;
    }

    public bool OnReceivedDraggable(Draggable drag)
    {
        switch(drag.type){
            case Draggable.Type.Bill:
            case Draggable.Type.Coin:
            case Draggable.Type.GoldBar:
            case Draggable.Type.SilverBar:{
                GetMoney((int)drag.value);
                return true;
            }
            case Draggable.Type.Generic:{
                return false;
            }
            default: {
                throw new System.Exception("Unhandled Draggable!");
            }
        }
    }
}
