using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomPitchSound : MonoBehaviour
{
    [SerializeField]
    private AudioClip [] m_soundfiles;
    [SerializeField]
    private Vector2 m_pitchMinMax;

    private AudioSource m_audio;

    private void Awake()
    {
        m_audio = GetComponent<AudioSource> ();
    }

    public void PlaySound()
    {
        m_audio.Stop ();
        m_audio.clip = m_soundfiles [ Random.Range ( 0, m_soundfiles.Length ) ];
        m_audio.pitch = Random.Range ( m_pitchMinMax.x, m_pitchMinMax.y );
        m_audio.Play ();
    }
}
