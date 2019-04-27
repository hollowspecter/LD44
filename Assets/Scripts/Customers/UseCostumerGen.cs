using UnityEngine;
using Random = UnityEngine.Random;

public class UseCostumerGen : MonoBehaviour
{
    public CustomerGenEvent cGen;
    public int maximumCustomer = 5;
    [MinMaxRange(1,350)]public RangedFloat moneyAtHand;
    [MinMaxRange(50,500)]public RangedFloat timeToGetAngry;
    public Transform spawnLocation;
    
    private enum NeedType
    {
        deposit,
        withdraw,
        exchange
    }

    private NeedType _need;
    private Transform _childTransform;

//    private Clock _clockTime;
    
    private void Start()
    {
//        _clockTime = Clock.instance;
        _need = (NeedType)Random.Range(0, 2);
        Spawn();
    }

    protected virtual void Spawn()
    {
       cGen.GenerateCustomer();
    }
}
