using UnityEngine;

public abstract class CustomerSound : ScriptableObject
{
    public abstract void HelloSound(AudioSource audioSource);
    public abstract void HappySound(AudioSource audioSource);
    public abstract void SadSound(AudioSource audioSource);
    public abstract void AngrySound(AudioSource audioSource);
    public abstract void GoodbyeSound(AudioSource audioSource);
}
