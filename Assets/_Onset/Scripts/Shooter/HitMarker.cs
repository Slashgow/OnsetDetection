using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class HitMarker : MonoBehaviour
{
    [SerializeField]
    private Image hitMarkerImlage;

    [SerializeField]
    private Ease easing;

    [SerializeField, Range(0f, 1f)]
    private float duration = 0.5f;

    [SerializeField, Range(1f, 5f)]
    private float maxScale = 3f;

    [SerializeField, Range(0f, 180f)]
    private float maxZRotation = 30f;

    [SerializeField]
    private GunController gunController;

    private void Awake() => gunController.OnHitNote += GunController_OnHitNote;
    private void OnDestroy() => gunController.OnHitNote -= GunController_OnHitNote;

    private void GunController_OnHitNote(Note note) => Animate();

    public void Animate()
    {
        transform.DOScale(maxScale, duration).SetEase(easing).SmoothRewind();
        transform.DOLocalRotate(new Vector3(0f, 0f, Random.Range(0, maxZRotation)), duration).SetEase(easing).SmoothRewind();
    }
}
