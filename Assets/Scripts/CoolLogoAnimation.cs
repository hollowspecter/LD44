using UnityEngine;
using DG.Tweening;

public class CoolLogoAnimation : MonoBehaviour
{
    public RectTransform rect;
    public float duration;
    public Vector3 strength;
    public int vibrato;
    [Range(0,90)]
    public float random;

    public float delay;

    private Sequence mySequence;
    private void Start()
    {  
        mySequence = DOTween.Sequence();
        mySequence.Append(this.transform.DOShakeRotation(duration, strength, vibrato, random).SetRelative().SetEase(EaseFactory.StopMotion(8,Ease.Linear)).SetDelay(delay))
            .Append(this.transform.DOShakeRotation(duration, strength, vibrato, random).SetEase(EaseFactory.StopMotion(8,Ease.Linear)).SetDelay(delay))
            .SetDelay(delay); ;
        
        mySequence.SetLoops(-1, LoopType.Restart);
    }
}
