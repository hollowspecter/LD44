using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace sfx
{
    [CreateAssetMenu ( fileName = "SoundDB", menuName = "ScriptableObjects/SoundDB", order = 1 )]
    public class SoundDb : ScriptableObject
    {
        public MainSfx Main;
        public MiscSfx Misc;
        public DialogSfx Dialog;
    }

    [Serializable]
    public class MainSfx
    {
        public AudioClip Alarm;
        public AudioClip BillsCounted;
        public AudioClip CoinCounterRunning;
        public AudioClip GunShots;
        public AudioClip MachineRunning;
        public AudioClip MouseClick;
        public AudioClip PoliceSiren;
    }

    [Serializable]
    public class MiscSfx
    {
        public AudioClip BoxHandling;
        public AudioClip ButtonPress;
        public AudioClip DoorLock;
        public AudioClip CoinsFallInJar;
        public AudioClip PokerChipSound;
        public AudioClip GrabbingPaper;
        public AudioClip TearingSound;
        public AudioClip UIAlert;
        public AudioClip BigWoosh;
    }

    [Serializable]
    public class DialogSfx
    {
        public AudioClip bop1;
        public AudioClip bop2;
        public AudioClip bop3;
        public AudioClip bop4;
        public AudioClip bop5;
        public AudioClip bop6;
        public AudioClip high1;
        public AudioClip high2;
        public AudioClip high3;
        public AudioClip high4;
        public AudioClip high5;
        public AudioClip high6;
    }
}
