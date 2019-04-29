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
        if (other.gameObject.name.Contains("Coin"))
        {
            SoundManager.Instance.m_coinDrop.PlaySound ();
        }
        Destroy(other.gameObject);
    }
}
