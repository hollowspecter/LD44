using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UniRx.Triggers;
using UniRx;
using UnityEngine.Events;

public class MainMenu : MonoBehaviour
{
    
    [Space]
    [Header("Credits options")]
    public RectTransform creditsWindow;
    public RectTransform creditsStartPos;
    public RectTransform creditsTargetPos;
    
    private bool creditsEnabled = false;
    
    [Space]
    [Header("Tutorial options")]
    public RectTransform tutorialWindow;
    public RectTransform tutorialStartPos;
    public RectTransform tutorialTargetPos;

    [Space]
    [Header ( "Buttons" )]
    public UnityEngine.UI.Button tutorialButton;
    public UnityEngine.UI.Button startGameButton;
    public UnityEngine.UI.Button creditsButton;
    public UnityEngine.UI.Button quitButton;
    
    private bool tutorialEnabled = false;

    [Space] [Header("Animation options")] 
    public float speed;

    private Sequence cPopUp;
    private Sequence cPopOut;
    private Sequence tPopUp;
    private Sequence tPopOut;
    private Vector3 fullSize = Vector3.one;
    private Vector3 minSize = new Vector3(0.001F, 0.001F,0.001F);
    
    [Header("parallax")] 
    public bool parallaxing = false;
    public Transform parallaxTransform;
//    public float Margin;
//    public float Layer;
//    float x;
//    float y;
    float easing = 0.2f;
    private Vector3 pz;
    Vector3 startPos;

    #region parallax
    private void Start ()
    {
        if (!parallaxing) return;
        startPos = parallaxTransform.position;

        // add click sound
        this.UpdateAsObservable ()
            .Where ( _ => Input.GetMouseButtonDown ( 0 ) )
            .Subscribe ( _ => SoundManager.Instance.PlaySfxAsOneShot ( "MouseClick" ) )
            .AddTo ( this );

        // subscribe to doubleclicks
        SubscribeToButton ( tutorialButton, ShowTutorial );
        SubscribeToButton ( startGameButton, StartGame );
        SubscribeToButton ( creditsButton, ShowCredits );
        SubscribeToButton ( quitButton, QuitGame );
    }

    private void Update()
    {
        if (!parallaxing) return;
        ParallaxMouse(parallaxTransform);
    }

    private void ParallaxMouse(Transform trans)
    {
        //parallxing ellements in ui
        Vector3 pz = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        pz.z = 0;
        trans.position = pz;
        
        trans.position = new Vector3(startPos.x + (pz.x * easing), startPos.y + (pz.y * easing), 0);
    }

    private void SubscribeToButton(UnityEngine.UI.Button button, UnityAction action )
    {
        var clickStream = button.onClick.AsObservable ();
        clickStream.Buffer ( clickStream.Throttle ( System.TimeSpan.FromMilliseconds ( 250 ) ) )
                   .Where ( xs => xs.Count >= 2 )
                   .Subscribe ( _ => action () )
                   .AddTo ( this );
    }
    #endregion

    #region Credits
    public void ShowCredits()
    {
        SoundManager.Instance.PlaySfxAsOneShot ( "UIAlert" );
        StartCoroutine (!creditsEnabled ? CreditOpenCoroutine() : CreditCloseCoroutine());
    }

    IEnumerator CreditOpenCoroutine()
    {
        cPopUp = DOTween.Sequence();
        cPopUp.Append(creditsWindow.transform.DOMove(creditsTargetPos.position, speed, true) )
        //cPopUp.Append(creditsWindow.DOAnchorPos(creditsTargetPos.position, speed, true) )
            .SetEase(Ease.OutQuint)
            .Insert(0,creditsWindow.DOScale(fullSize,speed));
        yield return cPopUp.WaitForCompletion();
        creditsEnabled = true;

    }
    
    IEnumerator CreditCloseCoroutine()
    {
        cPopOut = DOTween.Sequence();
        cPopOut.Append(creditsWindow.DOAnchorPos(creditsStartPos.position, speed, true))
            .SetEase(Ease.OutQuint)
            .Insert(0, creditsWindow.DOScale(minSize, speed));

        yield return cPopOut.WaitForCompletion();
        creditsEnabled = false;
       
    }
    #endregion

    #region Tutorial
    public void ShowTutorial()
    {
        SoundManager.Instance.PlaySfxAsOneShot ( "UIAlert" );
        StartCoroutine (!tutorialEnabled ? TutorialOpenCoroutine() : TutorialCloseCoroutine());
    }

    IEnumerator TutorialOpenCoroutine()
    {
        tPopUp = DOTween.Sequence();
        tPopUp.Append ( tutorialWindow.transform.DOMove ( tutorialTargetPos.position, speed, true ) )
        //tPopUp.Append(tutorialWindow.DOAnchorPos(tutorialTargetPos.position, speed, true))
            .SetEase(Ease.OutQuint)
            .Insert(0, tutorialWindow.DOScale(fullSize,speed));
        yield return tPopUp.WaitForCompletion();
        tutorialEnabled = true;

    }
    
    IEnumerator TutorialCloseCoroutine()
    {
        tPopOut = DOTween.Sequence();
        tPopOut.Append(tutorialWindow.DOAnchorPos(tutorialStartPos.position, speed, true))
            .SetEase(Ease.OutQuint)
            .Insert(0,tutorialWindow.DOScale(minSize, speed));
        yield return tPopOut.WaitForCompletion();
        tutorialEnabled = false;
    }
    #endregion

    public void StartGame()
    {
        SoundManager.Instance.PlaySfxAsOneShot ( "UIAlert" );
        SceneManager.LoadScene("MainGame", LoadSceneMode.Single);
        print("Loading the game scene!");
    }

    public void QuitGame()
    {
        SoundManager.Instance.PlaySfxAsOneShot ( "UIAlert" );
        Application.Quit();
        print("Quitting the game! Not supported in Editor Mode!");
    }
   
}
