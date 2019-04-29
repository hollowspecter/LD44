using UnityEngine;

[CreateAssetMenu(menuName = "CostumerGen/Customer")]
public class CustomerGenEvent : CustomerGenerator
{
    public GameObject CustomerPrefab;
    public Dispensary dispensaryPrefab;
    public string accountNumber { get{
        return account.accountNumber;
    }}
    public string name {get{
        return account.firstName + account.lastName;
    }}
    public Account account;
    [MinMaxRange(1,2500)]public RangedFloat moneyWantingToUse;
    public int wrongMoneyUseRange; //the range from moneyWantingToUse that they give you
    [MinMaxRange(15,75)]public RangedFloat timeToGetAngry;
    
    private Vector3 spawnPoint= new Vector3(0,100,0);
    
    public override GameObject GenerateCustomer()
    {
        // create the account, key is accountnumber!
        if ( !TellerMachine.Instance.accounts.ContainsKey ( accountNumber ) )
        {
            Debug.Log ( "Creating a new Account: " + accountNumber );
            TellerMachine.Instance.accounts.Add ( accountNumber, account );
        }

        var customer = Instantiate(CustomerPrefab, spawnPoint, Quaternion.identity);
        //var dispensary = Instantiate(dispensaryPrefab,customer.transform,false);
        
        var cBrain = customer.GetComponent<CustomerBrain>();
        //cBrain.dispensary = dispensary;
        //dispensary.name = cBrain.name + "Dispensery";
        //dispensary.RegisterReceiver(cBrain);

        cBrain.customerName = name;
        cBrain.maxTimeRange = timeToGetAngry;
        cBrain.moneyToUse = moneyWantingToUse;
        cBrain.moneyRange = wrongMoneyUseRange;
        cBrain.accountNumber = accountNumber;

        return customer;
    }
}
