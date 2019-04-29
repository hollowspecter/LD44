using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using sfx;
using DG.Tweening;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField]
    private SoundDb m_soundDb;
    [SerializeField]
    private Sources m_sources;
    [SerializeField]
    private Settings m_settings;

    public RandomPitchSound m_doorOpen;
    public RandomPitchSound m_doorClose;
    public RandomPitchSound m_coinDrop;

    [System.Serializable]
    public class Sources
    {
        public AudioSource Music;
        public AudioSource Ambience;
        public AudioSource SFX;
    }

    [System.Serializable]
    public class Settings
    {
        public bool AutostartMusic = true;
        public bool AutostartAmbient = false;
        public float FadeDuration = 3f;
    }

    private Dictionary<string, AudioClip> m_sounds = new Dictionary<string, AudioClip> ();

    #region lifecycle

    void Awake()
    {
        // Singleton
        if ( Instance != null && Instance != this )
        {
            Destroy ( gameObject );
        }
        Instance = this;
        DontDestroyOnLoad ( gameObject );

        // Registry
        RegisterSounds();
    }

    private void Start()
    {
        if ( m_settings.AutostartMusic ) ToggleMusic ();
        if ( m_settings.AutostartAmbient ) ToggleAmbience ();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    #endregion

    #region private_methods

    private void RegisterSounds()
    {
        // Main
        m_sounds.Add ( nameof ( m_soundDb.Main.Alarm ), m_soundDb.Main.Alarm );
        m_sounds.Add ( nameof ( m_soundDb.Main.BillsCounted ), m_soundDb.Main.BillsCounted );
        m_sounds.Add ( nameof ( m_soundDb.Main.CoinCounterRunning ), m_soundDb.Main.CoinCounterRunning );
        m_sounds.Add ( nameof ( m_soundDb.Main.GunShots ), m_soundDb.Main.GunShots );
        m_sounds.Add ( nameof ( m_soundDb.Main.MachineRunning ), m_soundDb.Main.MachineRunning );
        m_sounds.Add ( nameof ( m_soundDb.Main.MouseClick ), m_soundDb.Main.MouseClick );
        m_sounds.Add ( nameof ( m_soundDb.Main.PoliceSiren ), m_soundDb.Main.PoliceSiren );

        // Misc
        m_sounds.Add ( nameof ( m_soundDb.Misc.BoxHandling ), m_soundDb.Misc.BoxHandling );
        m_sounds.Add ( nameof ( m_soundDb.Misc.ButtonPress ), m_soundDb.Misc.ButtonPress );
        m_sounds.Add ( nameof ( m_soundDb.Misc.DoorLock ), m_soundDb.Misc.DoorLock );
        m_sounds.Add ( nameof ( m_soundDb.Misc.CoinsFallInJar ), m_soundDb.Misc.CoinsFallInJar );
        m_sounds.Add ( nameof ( m_soundDb.Misc.PokerChipSound ), m_soundDb.Misc.PokerChipSound );
        m_sounds.Add ( nameof ( m_soundDb.Misc.GrabbingPaper ), m_soundDb.Misc.GrabbingPaper );
        m_sounds.Add ( nameof ( m_soundDb.Misc.TearingSound ), m_soundDb.Misc.TearingSound );
        m_sounds.Add ( nameof ( m_soundDb.Misc.UIAlert ), m_soundDb.Misc.UIAlert );
        m_sounds.Add ( nameof ( m_soundDb.Misc.BigWoosh ), m_soundDb.Misc.BigWoosh );

        // Dialog
<<<<<<< HEAD
        m_sounds.Add(nameof(m_soundDb.Dialog.oldlady), m_soundDb.Dialog.oldlady);
        m_sounds.Add(nameof(m_soundDb.Dialog.tryin), m_soundDb.Dialog.tryin);
=======
        m_sounds.Add ( nameof ( m_soundDb.Dialog.oldlady ), m_soundDb.Dialog.oldlady );
        m_sounds.Add ( nameof ( m_soundDb.Dialog.tryin ), m_soundDb.Dialog.tryin );
>>>>>>> develop
    }

    private void ToggleSource( AudioSource _source )
    {
        if ( !_source.isPlaying )
        {
            // fade in
            _source.Play ();
            _source.DOFade ( 1f, m_settings.FadeDuration )
                   .From ( 0f );
        }
        else
        {
            // fade out
            _source.DOFade ( 0f, m_settings.FadeDuration )
                   .OnComplete ( _source.Stop );
        }
    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if ( scene.name == "MainMenu" )
        {
            ToggleAmbience ();
        }

        if (scene.name == "MainGame" )
        {
            ToggleAmbience ();
        }
    }

    #endregion

    #region public_methods

    public void PlaySfxAsOneShot(string _key )
    {
        AudioClip clip;
        m_sounds.TryGetValue ( _key, out clip );
        if ( clip != null ) m_sources.SFX.PlayOneShot ( clip );
        else Debug.LogWarningFormat ( "The sfx {0} has not been found!", _key );
    }

    public void ToggleMusic() { ToggleSource ( m_sources.Music ); }

    public void ToggleAmbience() { ToggleSource ( m_sources.Ambience ); }

    #endregion
}
