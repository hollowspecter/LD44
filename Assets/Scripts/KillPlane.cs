using UnityEngine;

public class KillPlane : MonoBehaviour
{
    public Collider trigger;

    private void Awake()
    {
        trigger = GetComponent<BoxCollider>();
    }

    private void OnTriggerStay(Collider other)
    {
        Destroy(other.gameObject);
    }
}
