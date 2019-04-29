using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class EndOfDayUI : MonoBehaviour
{
    public TextMeshProUGUI commentText;
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI adviceText;
    public Button playNextDayButton;
    public Button quitYourJobButton;

    private void OnEnable()
    {
        playNextDayButton.onClick.AddListener ( OnPlayNextDayPressed );
        quitYourJobButton.onClick.AddListener ( OnQuitYourJobPressed );
        pointsText.text = string.Format ( "Customer happiness: {0}\nMoney Difference: {1}",
            App.instance.score.happiness,
            App.instance.score.moneyDifference );

        SoundManager.Instance.PlaySfxAsOneShot ( "BillsCounted" );
    }

    private void OnDisable()
    {
        playNextDayButton.onClick.RemoveAllListeners ();
        quitYourJobButton.onClick.RemoveAllListeners ();
    }

    private void OnPlayNextDayPressed()
    {
        Debug.Log ( "Play Next Day Pressed!" );
        // Reloads the current scene
        SceneManager.LoadScene ( SceneManager.GetActiveScene ().buildIndex );
    }

    private void OnQuitYourJobPressed()
    {
        Debug.Log ( "Quit your job pressed!" );
        Application.Quit ();
    }
}
