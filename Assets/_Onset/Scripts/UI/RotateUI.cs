using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RotateUI : MonoBehaviour
{
    [SerializeField]
    private Toggle button;

    [SerializeField]
    private Ease easing;

    [SerializeField, Range(0f,5f)]
    private float loopDuration;

    [SerializeField, Range(0f, 5f)]
    private float backDuration;

    private Tween rotateTween;

    private void Awake()
    {
        button.onValueChanged.AddListener(Animate);
    }


    private void Animate(bool value)
    {
        Debug.Log($"{this.gameObject.name} is {value}");
        if(value )
            Rotate();
        else
            ResetToInitialRotation();
    }

    private void Rotate()
    {
        if (rotateTween != null)
            rotateTween.Kill();

        rotateTween = this.transform.DOLocalRotate(new Vector3(0f, 0f, 180f), loopDuration).SetEase(easing).SetLoops(-1, LoopType.Incremental).SetUpdate(true);
    }

    private void ResetToInitialRotation()
    {
        if (rotateTween != null)
            rotateTween.Kill();

        rotateTween = this.transform.DOLocalRotate(Vector3.zero, backDuration).SetEase(easing).SetUpdate(true);
    }

    private void OnDestroy()
    {
        rotateTween.Kill();
        button.onValueChanged.RemoveListener(Animate);
    }
}
