﻿using System.Collections;
using DG.Tweening;
using UnityEngine;
using TMPro;

public class CustomerBrain : MonoBehaviour
{
    private ICustomer cI;
    public string customerName;
    public Sprite graphic;
    public RangedFloat maxTimeRange;
    public RangedFloat moneyToUse;
    public int moneyRange;
    public string action;
    public string accountNumber;
    public int angrinessLevel = 0; //from 0 to 3. 0 is super chill 3 is pissed af
    public bool moreMoney;

    public bool myTurn = false; //if true, it will go to the counter
    public bool amDone = false; //if true, it will go away and will deactivates itself
    
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
    private int _accountMoney = 230;
    private int _fundCheck;
    private bool introduced = false;
    
    public TMP_Text speechBubble;

    private void OnEnable()
    {
        //TODO: check their Account -> Needs the money tracking system

        if (!cI.hasAccount)
        {
            need = NeedType.makeAccount;
        }

        myTurn = false;
        amDone = false;
        introduced = false;
        angrinessLevel = 0;

        
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
        //maxTime = Mathf.Floor((maxTime / 60));
        
        
        //set money wanting to use
        //set money to use in range of wanting to use(difference!)
        _moneyWanting = (int)Random.Range(moneyToUse.minValue,moneyToUse.maxValue);
        _money = _moneyWanting + Random.Range(-moneyRange, moneyRange);

        //if the account has no money, they will change their action to deposit
        if (_accountMoney == 0)
        {
            action = "deposit";
        }
 
        //if they want to withdraw/exchange/transfer and "money" is higher than the total
        //on their bank account, the total will be the new "money"
        if (_accountMoney < _money && _accountMoney != 0)
        {
            _money = _accountMoney;
        }

        _fundCheck = _accountMoney;
    }

    private void Update()
    {
        //as long as in the queue, time is of essence. If it is their turn, they will move to counter
        //TODO: set this somewhere to true
        if(!myTurn) 
            {WaitingInQueue();}

        if (!introduced)
        {
            Introduce();
            introduced = true;
        }

        if (moreMoney)
        {
            _money = (_moneyWanting/2) + Random.Range(-moneyRange, moneyRange);
            GiveMoney();
            moreMoney = false;
        }

        if (!amDone) return;
        AngerManagment();
        StartCoroutine(LeaveCounter());
    }

    private void WaitingInQueue()
    {
        _timePast+= Time.deltaTime;
        
        if (_timePast > _maxTime * 0.7)
            {angrinessLevel = 1;print("I am getting impatient!");}
        
        if (_timePast > _maxTime)
        {
            print("I am going home!");
            angrinessLevel = 2;
            amDone = true;
        }
    }
    #region switch cases
    private void Introduce()
    {
        //possible: needs to give you more money
        //possible: can get money
        //possible: can rob
        switch (action)
        {
            case "deposit":
            {
                speechBubble.text = "Hello my name is " + customerName + " and I want to " + action +" "+ _money +
                                    " Moneys! my Account Number is " + accountNumber + ".";
                GiveMoney();
                
                //set new _money amount if we have to through again

                break;
            }

            case "withdraw":
            {
                speechBubble.text = "Hello my name is " + customerName + " and I want to " + action +" "+ _money +
                                    " Moneys! my Account Number is " + accountNumber + ".";
                break;
            }
            case "robbery":
            {
                speechBubble.text = "Hands in the air! I want to have "+ _money +
                                    " Moneys! Give it to me now!";
                _fundCheck = 0;
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
                if (_accountMoney == _moneyWanting + _fundCheck)
                {
                    break;
                }
                else
                {
                    angrinessLevel += 2;
                    break;
                }
            case "withdraw":
                if (_accountMoney == _fundCheck - _moneyWanting)
                {
                    break;
                }
                else
                {
                    angrinessLevel += 2;
                    break;
                }
            case "robbery":
                if (_fundCheck >= _moneyWanting)
                {
                    break;
                }
                else
                {
                    angrinessLevel += 2;
                    break;
                }
            default:
                break;
        }
    }
    #endregion

    IEnumerator LeaveCounter()
    {
        Tween leaveTween = transform.DOMoveX(transform.position.x, -100);
        yield return leaveTween.WaitForCompletion();
        //TODO: give angryness value to the Scoreboard 
        CustomerManager.next = true;
    }
    


    //TODO: throws money amount. Look at how Sam dispenses with his Teller machine
    private void GiveMoney()
    {
        //throw amount of _money
    }
    
    //TODO: able to give customers money
    private void GetRobbedMoney()
    {
        //increase _fundCheck 
    }
    
    
}