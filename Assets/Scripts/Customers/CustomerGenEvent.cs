using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "CostumerGen/Customer")]
public class CustomerGenEvent : CustomerGenerator
{
    public GameObject CustomerPrefab;
    public string accountNumber;
    public string name;
    public Sprite graphic;

    public override void GenerateCustomer(RangedFloat angryTime, RangedFloat money, string need, Transform spawnPoint)
    {
        var customer = Instantiate(CustomerPrefab, spawnPoint.position, Quaternion.identity);
        
        //TODO: make a CustomerManager to be even able to set stuff
        var cManager = customer.GetComponent<MessyTestScript>(); //customer.GetComponent<CustomerManager>();
        
        cManager.name = name;
        cManager.graphic = graphic;
        cManager.maxTime = Random.Range(angryTime.minValue,angryTime.maxValue);
        cManager.money = (int)Random.Range(money.minValue,money.maxValue);
        cManager.action = need;
        cManager.accountNumber = accountNumber;
    }
}
