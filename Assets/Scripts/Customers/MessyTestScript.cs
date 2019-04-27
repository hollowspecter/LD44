using UnityEngine;
using TMPro;

[RequireComponent(typeof(CustomerSoundEvent))]
public class MessyTestScript : MonoBehaviour
{
    public string name;
    public Sprite graphic;
    public float maxTime;
    public int money;
    public string action;
    public string accountNumber;
    
    public TMP_Text text;
    private SpriteRenderer _sRend;

    private void Awake()
    {
        _sRend = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        maxTime = Mathf.Floor((maxTime / 60));
        _sRend.sprite = graphic;   
        //if the account has no money, they will change their action to deposit
        //if they want to withdraw/exchange/transfer and "money" is higher than the total on their bank account, the total will be the new "money"
        text.text = "Hello my name is " + name + " and I want to " + action +" "+ money +
                    " Moneys! But I will only wait for " + maxTime + " min until I will leave!" + 
                    "Oh, right, my Account Number is " + accountNumber + ".";
    }
    
    //
}
