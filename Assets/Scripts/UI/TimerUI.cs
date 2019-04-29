using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    public Text timerText;

    void Update()
    {
        timerText.text = string.Format ( "END OF DAY IN {0}s",
            ( App.instance.timeUntilDayEnds - App.instance.time ).ToString("0")
            );
    }
}
