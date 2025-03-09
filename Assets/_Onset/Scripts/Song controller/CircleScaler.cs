using DG.Tweening;
using UnityEngine;

public class CircleScaler : MonoBehaviour
{
    [SerializeField, Range(1f, 5f)]
    private float startScale;

    [SerializeField, Range(1f, 5f)]
    private float endScale;

    [SerializeField]
    private Ease easing;

    private float duration;
    private Tween scaleTween;

    public void Setup(float duration)
    {
        this.transform.localScale = Vector3.one * startScale;
        this.duration = duration;
        ScaleDown();
    }

    private void ScaleDown()
    {
        if(scaleTween != null)
            scaleTween.Kill();

        scaleTween = this.transform.DOScale(Vector3.one * endScale, duration).SetEase(easing).
            OnComplete(() => this.GetComponent<Note>().PoolingSystem.AddToPool(this.gameObject));
    }
}
