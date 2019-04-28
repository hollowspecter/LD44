using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using UnityEngine.SceneManagement;

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
    #endregion

    #region Credits
    public void ShowCredits()
    {
        StartCoroutine(!creditsEnabled ? CreditOpenCoroutine() : CreditCloseCoroutine());
    }

    IEnumerator CreditOpenCoroutine()
    {
        cPopUp = DOTween.Sequence();
        cPopUp.Append(creditsWindow.DOAnchorPos(creditsTargetPos.position, speed, true))
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
        StartCoroutine(!tutorialEnabled ? TutorialOpenCoroutine() : TutorialCloseCoroutine());
    }

    IEnumerator TutorialOpenCoroutine()
    {
        tPopUp = DOTween.Sequence();
        tPopUp.Append(tutorialWindow.DOAnchorPos(tutorialTargetPos.position, speed, true))
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
        
        //TODO: add real scene to load!
//        SceneManager.LoadScene("TellerGame", LoadSceneMode.Single);
        print("Loading the game scene!");
    }

    public void QuitGame()
    {
        //TODO: enable
//        Application.Quit();
        print("Quitting the game!");
    }
   
}
