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
}
