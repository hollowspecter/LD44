using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using sfx;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private SoundDb m_soundDb;
    [SerializeField]
    private Sources m_sources;

    [System.Serializable]
    public class Sources
    {
        public AudioSource Music;
        public AudioSource Ambience;
        public AudioSource SFX;
    }

    private void Awake()
    {
        
    }
}
