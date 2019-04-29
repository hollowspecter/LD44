using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    public GameObject debugAccountView;
    public KeyCode debugAccountViewKey = KeyCode.Escape;

    void Update()
    {
        if ( Input.GetKeyDown ( debugAccountViewKey ) )
        {
            debugAccountView.SetActive ( !debugAccountView.activeSelf );
        }
    }
}
