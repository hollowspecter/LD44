using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PostitUI : MonoBehaviour
{
    // Singleton
    public static PostitUI Instance { get; private set; }

    public GameObject postitPanel;
    public TextMeshProUGUI tmpPostitText;

    private void Awake()
    {
        // Singleton
        if ( Instance != null && Instance != this )
        {
            Destroy ( gameObject );
        }
        Instance = this;
    }

    public void PublishToPostit(string _text)
    {
        tmpPostitText.text = _text;
    }
}
