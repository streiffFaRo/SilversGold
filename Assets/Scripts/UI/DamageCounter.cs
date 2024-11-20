using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageCounter : MonoBehaviour
{
    private GameObject number;
    public TextMeshProUGUI numberText;
    private Tween fadeTween;
    
    private void Start()
    {
        Fade(0,3f,DestroySelf);
    }
    
    private void Fade(float endValue, float duration, TweenCallback onEnd)
    {
        if (fadeTween!= null)
        {
            fadeTween.Kill(false);
        }

        fadeTween = numberText.DOFade(endValue, duration);
        fadeTween.onComplete += onEnd;
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
