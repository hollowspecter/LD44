using UnityEngine;

public abstract class CustomerGenerator : ScriptableObject
{
    public abstract void GenerateCustomer(RangedFloat angryTime, RangedFloat money,
        string need, Transform spawnPoint);
}
