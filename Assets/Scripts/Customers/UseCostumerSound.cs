using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UseCostumerSound : MonoBehaviour
{
    public CustomerSoundEvent CustomerSound;
    
    private AudioSource _mouth;

    private void Awake()
    {
        _mouth = GetComponent<AudioSource>();
    }

    public void Hello() { CustomerSound.HelloSound(_mouth); }

    public void Happy() { CustomerSound.HappySound(_mouth); }

    public void Sad() { CustomerSound.SadSound(_mouth); }
    
    public void Angry() { CustomerSound.AngrySound(_mouth); }
    
    public void Goodbye() { CustomerSound.GoodbyeSound(_mouth); }
}
