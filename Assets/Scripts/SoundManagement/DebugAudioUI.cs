using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugAudioUI : MonoBehaviour
{
    public InputField sfxNameInput;
    public Button playButton;

    public void Awake()
    {
        playButton.onClick.AddListener ( OnPlayButtonPressed );
    }

    public void OnPlayButtonPressed()
    {
        SoundManager.Instance.PlaySfxAsOneShot ( sfxNameInput.text );
    }
}
