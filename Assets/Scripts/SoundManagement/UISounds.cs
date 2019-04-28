using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

public class UISounds : MonoBehaviour
{
    [SerializeField]
    private Button [] buttons;
    [SerializeField]
    private TMP_InputField[] inputFields;
    [SerializeField]
    private RandomPitchSound keyboardSound;

    private void Start()
    {
        for (int i=0; i<buttons.Length; ++i )
        {
            buttons [ i ].onClick.AsObservable ()
                .Subscribe ( _ => SoundManager.Instance.PlaySfxAsOneShot ( "MouseClick" ) )
                .AddTo ( this );
        }

        for (int i=0; i<inputFields.Length;++i )
        {
            inputFields [ i ].onValueChanged.AsObservable ()
                .Subscribe ( _ => keyboardSound.PlaySound () )
                .AddTo ( this );
        }
    }
}
