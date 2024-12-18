using DG.Tweening;
using UnityEngine;

public class CreditsCardDOScale : MonoBehaviour
{

    private Tween fadeTween;
    
    public void Resize(float newScale)
    {
        if (fadeTween!= null)
        {
            fadeTween.Kill(false);
        }

        fadeTween = transform.DOScale(newScale, 0.2f);
        
    }
}
