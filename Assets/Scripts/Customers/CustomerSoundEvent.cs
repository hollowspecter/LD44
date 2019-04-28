using UnityEngine;

[CreateAssetMenu(menuName = "CostumerGen/Sound Handler")]
public class CustomerSoundEvent : CustomerSound
{
	public AudioClip[] HelloPool;
	public AudioClip[] HappyPool;
	public AudioClip[] SadPool;
	public AudioClip[] AngryPool;
	public AudioClip[] GoodbyePool;
	
	public float rRange = 0.5f;

	[MinMaxRange(0,1)]public RangedFloat volumeRangePos;
	[MinMaxRange(0.5F,2)]public RangedFloat pitchRangePos;
	
	public virtual void PlaySound(AudioSource source, AudioClip clip, float volumeScale)
	{
		source.PlayOneShot(clip, volumeScale);
	}

	public override void HelloSound(AudioSource source)
	{
		if (HelloPool.Length == 0) return;

		source.clip = HelloPool[Random.Range(0, HelloPool.Length)];
		source.volume = Random.Range(volumeRangePos.minValue, volumeRangePos.maxValue);
		source.pitch = Random.Range(pitchRangePos.minValue, pitchRangePos.maxValue);
		source.Play();
	}

	public override void HappySound(AudioSource source)
	{
		if (HappyPool.Length == 0) return;
		
		source.clip = HappyPool[Random.Range(0, HappyPool.Length)];
		source.volume = Random.Range(volumeRangePos.minValue, volumeRangePos.maxValue);
		source.pitch = Random.Range(pitchRangePos.minValue, pitchRangePos.maxValue);
		source.Play();

	}

	public override void SadSound(AudioSource source)
	{
		if (SadPool.Length == 0) return;
		
		source.clip = SadPool[Random.Range(0, SadPool.Length)];
		source.volume = Random.Range(volumeRangePos.minValue, volumeRangePos.maxValue);
		source.pitch = Random.Range(pitchRangePos.minValue, pitchRangePos.maxValue);
		source.Play();
		
	}
	
	public override void AngrySound(AudioSource source)
	{
		if (AngryPool.Length == 0) return;
		
		source.clip = AngryPool[Random.Range(0, AngryPool.Length)];
		source.volume = Random.Range(volumeRangePos.minValue, volumeRangePos.maxValue);
		source.pitch = Random.Range(pitchRangePos.minValue, pitchRangePos.maxValue);
		source.Play();
		
	}
	
	public override void GoodbyeSound(AudioSource source)
	{
		if (GoodbyePool.Length == 0) return;
		
		source.clip = GoodbyePool[Random.Range(0, GoodbyePool.Length)];
		source.volume = Random.Range(volumeRangePos.minValue, volumeRangePos.maxValue);
		source.pitch = Random.Range(pitchRangePos.minValue, pitchRangePos.maxValue);
		source.Play();
		
	}
}
