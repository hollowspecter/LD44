using UnityEngine;
using TMPro;

public class CustomerBrain : MonoBehaviour
{
    public string customerName;
    public Sprite graphic;
    public RangedFloat maxTimeRange;
    public RangedFloat moneyToUse;
    public int moneyRange;
    public string action;
    public string accountNumber;

    public bool myTurn = false; //if true, it will go to the counter
    public bool amDone = false; //if true, it will go away and will deactivates itself
    
    private enum NeedType
    {
        none,
        deposit,
        withdraw,
        robbery
    }

    private NeedType need;
    
    private float maxTime;
    private int moneyWanting;
    private int money;
    private int accountMoney;
    
    public TMP_Text speechBubble;
    
//    private SpriteRenderer _sRend;

//    private void Awake()
//    {
//        _sRend = GetComponent<SpriteRenderer>();
//    }

    private void OnEnable()
    {
        //TODO: check their Account -> Needs the money tracking system

        myTurn = false;
        amDone = false;
        
        //setup their action they want to do
        if (Random.Range(0, 100) > 95) //5 percent chance
        {
            need = NeedType.robbery;
        }
        else
        {
            need = (NeedType)Random.Range(1, 2);
        }
        action = need.ToString();

        
        //set the wait time to wait
        maxTime = Random.Range(maxTimeRange.minValue,maxTimeRange.maxValue);
        maxTime = Mathf.Floor((maxTime / 60));
        
        
        //set money wanting to use
        //set money to use in range of wanting to use(difference!)
        moneyWanting = (int)Random.Range(moneyToUse.minValue,moneyToUse.maxValue);
        money = moneyWanting + Random.Range(-moneyRange, moneyRange);

        //if the account has no money, they will change their action to deposit
        if (accountMoney == 0)
        {
            action = "deposit";
        }
        
        //if they want to withdraw/exchange/transfer and "money" is higher than the total
        //on their bank account, the total will be the new "money"
        if (accountMoney < money && accountMoney != 0)
        {
            money = accountMoney;
        } 
               
        speechBubble.text = "Hello my name is " + customerName + " and I want to " + action +" "+ money +
                            " Moneys! But I will only wait for " + maxTime + " min until I will leave!" + 
                            "Oh, right, my Account Number is " + accountNumber + ".";

    }

//    private void Start()
//    {
//        _sRend.sprite = graphic;   
//
//    }

    private void Update()
    {
        if (!amDone) return;
        CustomerManager.next = true;
    }
}
