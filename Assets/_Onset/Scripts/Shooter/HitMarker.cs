using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class HitMarker : MonoBehaviour
{
    [SerializeField]
    private Image hitMarkerImage;

    [SerializeField]
    private Ease easing;

    [SerializeField, Range(0f, 1f)]
    private float duration = 0.5f;

    [SerializeField, Range(1f, 5f)]
    private float maxScale = 3f;

    [SerializeField, Range(0f, 180f)]
    private float maxZRotation = 30f;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private GunController gunController;

    private Sequence sequence;
    private Tween rotateTween;

    private void Awake()
    {
        gunController.OnHitNote -= GunController_OnHitNote;
        gunController.OnHitNote += GunController_OnHitNote;
    }

    private void OnDestroy()
    {
        gunController.OnHitNote -= GunController_OnHitNote;
    }

    private void GunController_OnHitNote(Note note)
    {
        Animate();
    }

    private void Start()
    {
        this.hitMarkerImage.enabled = false;
    }
    public void Animate()
    {
        //Debug.Log("hit note");
        this.hitMarkerImage.enabled = true;
        audioSource.Play();

        if(sequence != null)
            sequence.Kill();

        if(rotateTween != null)
            rotateTween.Kill();

        sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(maxScale, duration).SetEase(easing));
        sequence.Append(transform.DOScale(1, duration).SetEase(easing)).OnComplete(() => this.hitMarkerImage.enabled = false);
   
        rotateTween = transform.DOLocalRotate(new Vector3(0f, 0f, Random.Range(0, maxZRotation)), duration).SetEase(easing).OnComplete( () => transform.DORewind());

    }
}
