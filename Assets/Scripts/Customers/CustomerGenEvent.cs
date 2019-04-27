using UnityEngine;

[CreateAssetMenu(menuName = "CostumerGen/Customer")]
public class CustomerGenEvent : CustomerGenerator
{
    public GameObject CustomerPrefab;
    public string accountNumber { get{
        return account.accountNumber;
    }}
    public string name {get{
        return account.firstName + account.lastName;
    }}
    public Account account;
    public Sprite graphic;
    [MinMaxRange(1,350)]public RangedFloat moneyWantingToUse;
    public int wrongMoneyUseRange; //the range from moneyWantingToUse that they give you
    [MinMaxRange(50,500)]public RangedFloat timeToGetAngry;
    
    private Vector3 spawnPoint= new Vector3(0,100,0);
    
    public override GameObject GenerateCustomer()
    {
        
        var customer = Instantiate(CustomerPrefab, spawnPoint, Quaternion.identity);
        
        var cBrain = customer.GetComponent<CustomerBrain>();
        
        cBrain.customerName = name;
        cBrain.graphic = graphic;
        cBrain.maxTimeRange = timeToGetAngry;
        cBrain.moneyToUse = moneyWantingToUse;
        cBrain.moneyRange = wrongMoneyUseRange;
        cBrain.accountNumber = accountNumber;
        
        return customer;
    }
}
